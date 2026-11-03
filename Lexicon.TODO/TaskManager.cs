namespace Lexicon.TODO;

public class TaskManager
{
	private readonly List<TaskItem> tasks = [];

	public TaskItem AddTask(string title, DateTime dueDate, string? project = null)
	{
		ArgumentNullException.ThrowIfNullOrWhiteSpace(title, nameof(title));

		int nextId = tasks.Max(t => t.Id) + 1;

		var task = new TaskItem(
			Id: nextId,
			Title: title,
			DueDate: dueDate,
			Status: TaskStatus.Pending, Project: project);

		tasks.Add(task);

		return task;
	}

	public bool UpdateTask(int id, string? title = null, DateTime? dueDate = null, TaskStatus? status = null, string? project = null)
	{
		var taskIndex = tasks.FindIndex(t => t.Id == id);

		if (taskIndex == -1)
		{
			return false;
		}

		var existingTask = tasks[taskIndex];

		var updatedTask = existingTask with
		{
			Title = title ?? existingTask.Title,
			DueDate = dueDate ?? existingTask.DueDate,
			Status = status ?? existingTask.Status,
			Project = project ?? existingTask.Project
		};

		tasks[taskIndex] = updatedTask;

		return true;
	}

	public TaskItem FindById(int id) => tasks
		.FirstOrDefault(t => t.Id == id) ??
		throw new KeyNotFoundException($"Task with ID {id} not found.");

	public IReadOnlyList<TaskItem> GetAllTasks() => tasks.AsReadOnly();
}