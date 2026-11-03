namespace Lexicon.TODO;

/// <summary>
/// Manages an in-memory collection of task items and provides operations to add, update, complete, delete, and query
/// tasks by identifier.
/// </summary>
/// <remarks>Task identifiers start at 1 and are assigned as the current highest identifier plus 1. Removed
/// identifiers are not reused. Returned task lists are read-only views over the current collection.</remarks>
public class TaskManager
{
	private readonly List<TaskItem> tasks = [];

	/// <summary>
	/// Gets the next available task identifier.
	/// </summary>
	/// <remarks>Returns 1 when no tasks exist; otherwise returns the highest existing task ID plus 1.</remarks>
	public int NextId => tasks.Count > 0 ? tasks.Max(t => t.Id) + 1 : 1;


	/// <summary>
	/// Creates a pending task item and adds it to the collection.
	/// </summary>
	/// <remarks>Leading and trailing whitespace in <paramref name="title"/> is removed before storing.</remarks>
	/// <param name="title">Task title. Cannot be null, empty, or whitespace.</param>
	/// <param name="dueDate">Due date for the task.</param>
	/// <param name="project">Project name associated with the task, or <see langword="null"/> when not assigned.</param>
	/// <returns>The created <see cref="TaskItem"/>.</returns>
	public TaskItem AddTask(string title, DateTime dueDate, string? project = null)
	{
		ArgumentNullException.ThrowIfNullOrWhiteSpace(title, nameof(title));

		var task = new TaskItem(
			Id: NextId,
			Title: title.Trim(),
			DueDate: dueDate,
			State: TaskState.Pending, Project: project);

		tasks.Add(task);

		return task;
	}

	/// <summary>
	/// Updates an existing task identified by <paramref name="id"/> with the provided field values.
	/// </summary>
	/// <param name="id">The unique identifier of the task to update.</param>
	/// <param name="title">The new title to set. If <see langword="null"/>, the current title is kept.</param>
	/// <param name="dueDate">The new due date to set. If <see langword="null"/>, the current due date is kept.</param>
	/// <param name="state">The new state to set. If <see langword="null"/>, the current state is kept.</param>
	/// <param name="project">The new project to set. If <see langword="null"/>, the current project is kept.</param>
	/// <returns><see langword="true"/> when a task with the specified <paramref name="id"/> is found and updated; otherwise, <see
	/// langword="false"/>.</returns>
	/// <exception cref="ArgumentException"><paramref name="title"/> is empty or consists only of white-space characters.</exception>
	public bool UpdateTask(int id, 
		string? title = null, 
		DateTime? dueDate = null, 
		TaskState? state = null, 
		string? project = null)
	{
		if (title is not null && string.IsNullOrWhiteSpace(title))
			throw new ArgumentException("Title cannot be empty or whitespace.", nameof(title));

		if (!TryFindIndex(id, out var taskIndex))
			return false;

		var existingTask = tasks[taskIndex];

		var updatedTask = existingTask with
		{
			Title = title?.Trim() ?? existingTask.Title,
			DueDate = dueDate ?? existingTask.DueDate,
			State = state ?? existingTask.State,
			Project = project ?? existingTask.Project
		};

		tasks[taskIndex] = updatedTask;

		return true;
	}

	/// <summary>
	/// Marks the specified task as completed.
	/// </summary>
	/// <param name="id">The identifier of the task to complete.</param>
	/// <returns><see langword="true"/> if the task was updated to <see cref="TaskState.Completed"/>; otherwise, <see
	/// langword="false"/>.</returns>
	public bool CompleteTask(int id)
	{
		return UpdateTask(id, state: TaskState.Completed);
	}

	/// <summary>
	/// Removes the task with the specified identifier.
	/// </summary>
	/// <param name="id">The identifier of the task to remove.</param>
	/// <returns><see langword="true"/> if a task with the specified identifier was found and removed; otherwise, <see
	/// langword="false"/>.</returns>
	public bool DeleteTask(int id)
	{
		if (!TryFindIndex(id, out var taskIndex))
			return false;

		tasks.RemoveAt(taskIndex);

		return true;
	}

	/// <summary>
	/// Finds a task item by its identifier.
	/// </summary>
	/// <param name="id">The identifier of the task item to find.</param>
	/// <returns>The task item with the specified identifier, or <see langword="null"/> if no matching task item is found.</returns>
	public TaskItem? FindById(int id) => tasks
		.FirstOrDefault(t => t.Id == id);

	/// <summary>
	/// Gets all tasks as a read-only list.
	/// </summary>
	/// <remarks>The returned list is read-only and cannot be modified through this API.</remarks>
	/// <returns>A read-only list of all task items.</returns>
	public IReadOnlyList<TaskItem> GetAllTasks() => tasks
		.AsReadOnly();

	
	/// <summary>
	/// Attempts to locate a task with the specified identifier and output its index.
	/// </summary>
	/// <param name="id">The identifier of the task to locate.</param>
	/// <param name="index">When this method returns, contains the zero-based index of the matching task if found; otherwise, -1.</param>
	/// <returns><see langword="true"/> if a matching task is found; otherwise, <see langword="false"/>.</returns>
	private bool TryFindIndex(int id, out int index)
	{
		index = tasks.FindIndex(t => t.Id == id);
		return index != -1;
	}
}