namespace Lexicon.TODO;

public class TaskManager
{
	private readonly List<TaskItem> tasks = [];

	public int NextId => tasks.Count > 0 ? tasks.Max(t => t.Id) + 1 : 1;

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

	public bool UpdateTask(int id, string? title = null, DateTime? dueDate = null, TaskState? state = null, string? project = null)
	{
		if (title is not null && string.IsNullOrWhiteSpace(title))
			throw new ArgumentException("Title cannot be empty or whitespace.", nameof(title));

		var taskIndex = tasks.FindIndex(t => t.Id == id);

		if (taskIndex == -1)
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

	public TaskItem? FindById(int id) => tasks
		.FirstOrDefault(t => t.Id == id);

	public IReadOnlyList<TaskItem> GetAllTasks() => tasks
		.AsReadOnly();
}