namespace Lexicon.TODO.Models;

/// <summary>
/// Represents the state of a task.
/// </summary>
public enum TaskState
{
	/// <summary>
	/// Indicates that the task is pending and yet to be completed.
	/// </summary>
	Pending,

	/// <summary>
	/// Indicates that the task has been completed.
	/// </summary>
	Completed
}