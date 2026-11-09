namespace Lexicon.TODO;

/// <summary>
/// Represents a task item with details such as ID, title, due date, state, and associated project.
/// </summary>
/// <remarks>
/// This record is used to encapsulate the properties of a task and provides functionality to determine if the task is overdue.
/// </remarks>
public record TaskItem (
	int Id,
	string Title,
	DateTime DueDate,
	TaskState State,
	string? Project )
{
	/// <summary>
	/// Gets a value indicating whether the task is overdue.
	/// </summary>
	/// <value>
	/// <c>true</c> if the current date and time is greater than or equal to the task's due date; otherwise, <c>false</c>.
	/// </value>
	public bool IsOverdue => DateTime.Now >= DueDate;
}