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
	public int NextId => tasks.Count > 0
		? tasks.Max ( t => t.Id ) + 1
		: 1;


	/// <summary>
	/// Gets a read-only list of tasks that have been completed.
	/// </summary>
	/// <value>
	/// A collection of <see cref="TaskItem"/> instances where the <see cref="TaskItem.State"/> is <see cref="TaskState.Completed"/>.
	/// </value>
	/// <remarks>
	/// This property filters the internal task list to include only tasks marked as completed.
	/// The returned list is immutable and reflects the state of completed tasks at the time of access.
	/// </remarks>
	public IReadOnlyList<TaskItem> CompletedTasks => tasks
		.Where ( t => t.State == TaskState.Completed )
		.ToList ()
		.AsReadOnly ();

	/// <summary>
	/// Gets a read-only list of tasks that are currently pending.
	/// </summary>
	/// <remarks>
	/// A task is considered pending if its state is <see cref="TaskState.Pending"/>.
	/// This property filters the tasks managed by the <see cref="TaskManager"/> to include only those
	/// with a pending state and returns them as a read-only collection.
	/// </remarks>
	/// <value>
	/// A read-only list of <see cref="TaskItem"/> instances representing the pending tasks.
	/// </value>
	public IReadOnlyList<TaskItem> PendingTasks => tasks
		.Where ( t => t.State == TaskState.Pending )
		.ToList ()
		.AsReadOnly ();

	/// <summary>
	/// Gets a read-only list of tasks that are overdue.
	/// </summary>
	/// <remarks>
	/// A task is considered overdue if its state is <see cref="TaskState.Pending"/> 
	/// and its due date is earlier than the current date.
	/// </remarks>
	/// <value>
	/// A read-only list of <see cref="TaskItem"/> instances representing overdue tasks.
	/// </value>
	public IReadOnlyList<TaskItem> OverdueTasks => tasks
		.Where ( t => t.State == TaskState.Pending && t.DueDate < DateTime.Today )
		.ToList ()
		.AsReadOnly ();


	/// <summary>
	/// Adds a new task to the task manager with a pending state.
	/// </summary>
	/// <param name="title">
	/// The title of the task. This value is trimmed and cannot be null, empty, or whitespace.
	/// </param>
	/// <param name="dueDate">
	/// The due date of the task.
	/// </param>
	/// <param name="project">
	/// The optional project name associated with the task. Can be <see langword="null"/>.
	/// </param>
	/// <returns>
	/// The newly created <see cref="TaskItem"/> instance representing the added task.
	/// </returns>
	/// <exception cref="ArgumentException">
	/// Thrown when <paramref name="title"/> is null, empty, or consists only of whitespace.
	/// </exception>
	public TaskItem AddTask ( string title, DateTime dueDate, string? project = null )
	{
		ArgumentException.ThrowIfNullOrWhiteSpace ( title, nameof(title) );

		var task = new TaskItem (
			Id: NextId,
			Title: title.Trim (),
			DueDate: dueDate,
			State: TaskState.Pending, Project: project );

		tasks.Add ( task );

		return task;
	}

	/// <summary>
	/// Changes the given fields of a task. A <see langword="null"/> argument keeps the current value,
	/// so a project cannot be cleared through this method.
	/// </summary>
	/// <returns><see langword="false"/> if no task has this <paramref name="id"/>.</returns>
	/// <exception cref="ArgumentException"><paramref name="title"/> is supplied but empty or whitespace.</exception>
	public bool UpdateTask (
		int id,
		string? title = null,
		DateTime? dueDate = null,
		TaskState? state = null,
		string? project = null )
	{
		if ( title is not null && string.IsNullOrWhiteSpace ( title ) )
			throw new ArgumentException ( "Title cannot be empty or whitespace.", nameof(title) );

		if ( !TryFindIndex ( id, out var taskIndex ) )
			return false;

		TaskItem existingTask = tasks[taskIndex];

		TaskItem updatedTask = existingTask with
		{
			Title = title?.Trim () ?? existingTask.Title,
			DueDate = dueDate ?? existingTask.DueDate,
			State = state ?? existingTask.State,
			Project = project ?? existingTask.Project
		};

		tasks[taskIndex] = updatedTask;

		return true;
	}

	/// <summary>
	/// Determines whether a task with the specified ID exists in the task collection.
	/// </summary>
	/// <param name="id">The unique identifier of the task to locate.</param>
	/// <returns>
	/// <see langword="true"/> if a task with the specified ID exists; otherwise, <see langword="false"/>.
	/// </returns>
	/// <remarks>
	/// This method performs a search within the in-memory task collection to check for the existence of a task with the given ID.
	/// </remarks>
	public bool ContainsTask ( int id ) => tasks.Any ( t => t.Id == id );

	/// <summary>
	/// Marks the task with the specified <paramref name="id"/> as completed.
	/// </summary>
	/// <param name="id">The unique identifier of the task to mark as completed.</param>
	/// <returns>
	/// <see langword="true"/> if the task was successfully marked as completed; 
	/// otherwise, <see langword="false"/> if no task with the specified <paramref name="id"/> exists.
	/// </returns>
	/// <remarks>
	/// Completing an already completed task does not result in an error.
	/// </remarks>
	public bool CompleteTask ( int id )
	{
		return UpdateTask ( id, state: TaskState.Completed );
	}

	/// <summary>
	/// Deletes a task with the specified ID from the task collection.
	/// </summary>
	/// <param name="id">The unique identifier of the task to delete.</param>
	/// <returns>
	/// <see langword="true"/> if the task was successfully deleted; 
	/// otherwise, <see langword="false"/> if no task with the specified ID exists.
	/// </returns>
	/// <remarks>
	/// This method removes the task from the internal collection if it exists. 
	/// Task IDs are not reused after deletion.
	/// </remarks>
	public bool DeleteTask ( int id )
	{
		if ( !TryFindIndex ( id, out var taskIndex ) )
			return false;

		tasks.RemoveAt ( taskIndex );

		return true;
	}

	/// <summary>
	/// Finds a task by its unique identifier.
	/// </summary>
	/// <param name="id">The unique identifier of the task to find.</param>
	/// <returns>
	/// The <see cref="TaskItem"/> with the specified ID if found; otherwise, <see langword="null"/>.
	/// </returns>
	/// <remarks>
	/// This method searches the in-memory collection of tasks for a task with the specified ID.
	/// If no task with the given ID exists, it returns <see langword="null"/>.
	/// </remarks>
	public TaskItem? FindById ( int id ) => tasks
		.FirstOrDefault ( t => t.Id == id );


	/// <summary>
	/// Retrieves all tasks managed by the <see cref="TaskManager"/>.
	/// </summary>
	/// <returns>
	/// A read-only list of all tasks currently managed by the <see cref="TaskManager"/>.
	/// </returns>
	/// <remarks>
	/// The returned list is a live view of the internal task collection, meaning any changes
	/// to the tasks within the <see cref="TaskManager"/> will be reflected in the returned list.
	/// </remarks>
	public IReadOnlyList<TaskItem> GetAllTasks () => tasks
		.AsReadOnly ();


	/// <summary>
	/// Retrieves a read-only list of all tasks sorted by their due date in ascending order.
	/// </summary>
	/// <returns>
	/// A read-only list of <see cref="TaskItem"/> objects, sorted by the <see cref="TaskItem.DueDate"/> property.
	/// </returns>
	/// <remarks>
	/// Tasks with earlier due dates will appear first in the returned list. 
	/// This method does not modify the underlying task collection.
	/// </remarks>
	public IReadOnlyList<TaskItem> GetTasksSortedByDueDate () => tasks
		.OrderBy ( t => t.DueDate )
		.ToList ()
		.AsReadOnly ();

	/// <summary>
	/// Retrieves a list of tasks sorted by their associated project name.
	/// </summary>
	/// <remarks>
	/// Tasks are first sorted by whether they have an associated project (tasks without a project appear last),
	/// then alphabetically by project name, and finally by due date within each project.
	/// </remarks>
	/// <returns>
	/// A read-only list of <see cref="TaskItem"/> objects sorted by project and due date.
	/// </returns>
	public IReadOnlyList<TaskItem> GetTasksSortedByProject () => tasks
		.OrderBy ( t => t.Project is null )
		.ThenBy ( t => t.Project )
		.ThenBy ( t => t.DueDate )
		.ToList ()
		.AsReadOnly ();


	/// <summary>
	/// Attempts to find the index of a task with the specified ID in the task list.
	/// </summary>
	/// <param name="id">The unique identifier of the task to locate.</param>
	/// <param name="index">
	/// When this method returns, contains the zero-based index of the task in the list, 
	/// if the task is found; otherwise, -1. This parameter is passed uninitialized.
	/// </param>
	/// <returns>
	/// <see langword="true"/> if a task with the specified ID is found; otherwise, <see langword="false"/>.
	/// </returns>
	/// <remarks>
	/// This method is used internally to locate tasks by their ID without exposing the index directly.
	/// </remarks>
	private bool TryFindIndex ( int id, out int index )
	{
		index = tasks.FindIndex ( t => t.Id == id );
		return index != -1;
	}
}