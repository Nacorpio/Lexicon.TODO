namespace Lexicon.TODO;

/// <summary>
/// Holds the in-memory task list and the rules for changing it.
/// </summary>
/// <remarks>
/// Ids are the current highest Id plus 1, so the highest Id can be reused after that task is deleted.
/// Methods report a missing Id by returning <see langword="false"/> or <see langword="null"/>; they do not throw for it.
/// </remarks>
public class TaskManager
{
	private readonly List<TaskItem> tasks = [];

	/// <summary>The Id the next added task will get: 1 for an empty list, otherwise highest Id + 1.</summary>
	public int NextId => tasks.Count > 0 ? tasks.Max(t => t.Id) + 1 : 1;


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

	
	private bool TryFindIndex(int id, out int index)
	{
		index = tasks.FindIndex(t => t.Id == id);
		return index != -1;
	}
}