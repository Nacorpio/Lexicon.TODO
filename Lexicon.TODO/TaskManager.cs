namespace Lexicon.TODO;

/// <summary>
/// Manages a collection of tasks, providing functionality to add, update, delete, and query tasks.
/// </summary>
/// <remarks>
/// This class maintains an in-memory list of tasks and enforces rules for managing them. 
/// Task IDs are automatically generated based on the highest current ID, allowing reuse of IDs after deletion.
/// Methods return <see langword="false"/> or <see langword="null"/> for invalid operations, instead of throwing exceptions.
/// </remarks>
public class TaskManager
{
	private readonly List<TaskItem> tasks = [];

	/// <summary>
	/// Gets the next available task ID.
	/// </summary>
	/// <value>
	/// The next ID is calculated as one greater than the highest current task ID. 
	/// If no tasks exist, the value is <c>1</c>.
	/// </value>
	/// <remarks>
	/// This property ensures that task IDs are unique and sequential, even if tasks are deleted.
	/// </remarks>
	public int NextId => tasks.Count > 0 ? tasks.Max(t => t.Id) + 1 : 1;


	public IReadOnlyList<TaskItem> CompletedTasks => tasks
		.Where(t => t.State == TaskState.Completed)
		.ToList()
		.AsReadOnly();

	public IReadOnlyList<TaskItem> PendingTasks => tasks
		.Where(t => t.State == TaskState.Pending)
		.ToList()
		.AsReadOnly();

	public IReadOnlyList<TaskItem> OverdueTasks => tasks
		.Where(t => t.State == TaskState.Pending && t.DueDate < DateTime.Today)
		.ToList()
		.AsReadOnly();


	/// <summary>Adds a pending task with the next Id. The title is trimmed.</summary>
	/// <exception cref="ArgumentException"><paramref name="title"/> is null, empty or whitespace.</exception>
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
	/// Changes the given fields of a task. A <see langword="null"/> argument keeps the current value,
	/// so a project cannot be cleared through this method.
	/// </summary>
	/// <returns><see langword="false"/> if no task has this <paramref name="id"/>.</returns>
	/// <exception cref="ArgumentException"><paramref name="title"/> is supplied but empty or whitespace.</exception>
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

		TaskItem existingTask = tasks[taskIndex];

		TaskItem updatedTask = existingTask with
		{
			Title = title?.Trim() ?? existingTask.Title,
			DueDate = dueDate ?? existingTask.DueDate,
			State = state ?? existingTask.State,
			Project = project ?? existingTask.Project
		};

		tasks[taskIndex] = updatedTask;

		return true;
	}

	public bool ContainsTask(int id) => tasks.Any(t => t.Id == id);

	/// <summary>Marks a task completed. Completing an already completed task is not an error.</summary>
	/// <returns><see langword="false"/> only if no task has this <paramref name="id"/>.</returns>
	public bool CompleteTask(int id)
	{
		return UpdateTask(id, state: TaskState.Completed);
	}

	public bool DeleteTask(int id)
	{
		if (!TryFindIndex(id, out var taskIndex))
			return false;

		tasks.RemoveAt(taskIndex);

		return true;
	}

	public TaskItem? FindById(int id) => tasks
		.FirstOrDefault(t => t.Id == id);


	/// <summary>Returns a read-only view of the live list, not a copy.</summary>
	public IReadOnlyList<TaskItem> GetAllTasks() => tasks
		.AsReadOnly();

	
	public IReadOnlyList<TaskItem> GetTasksSortedByDueDate() => tasks
		.OrderBy(t => t.DueDate)
		.ToList()
		.AsReadOnly();

	public IReadOnlyList<TaskItem> GetTasksSortedByProject() => tasks
		.OrderBy(t => t.Project is null)
		.ThenBy(t => t.Project)
		.ThenBy(t => t.DueDate)
		.ToList()
		.AsReadOnly();


	private bool TryFindIndex(int id, out int index)
	{
		index = tasks.FindIndex(t => t.Id == id);
		return index != -1;
	}
}