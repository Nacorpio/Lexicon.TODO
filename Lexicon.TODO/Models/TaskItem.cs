namespace Lexicon.TODO.Models;

/// <summary>
/// Represents a task item with details such as ID, title, due date, state, and associated project.
/// </summary>
/// <remarks>
/// This record encapsulates the properties of a task and offers state and due-date convenience checks.
/// </remarks>
public record TaskItem (
	int Id,
	string Title,
	DateTime DueDate,
	TaskState State,
	string? Project )
{
	/// <summary>
	/// Determines whether the task's due date falls before the given day.
	/// </summary>
	/// <param name="today">The date to compare against; only its date component is used.</param>
	/// <returns><see langword="true"/> if the due date is earlier than <paramref name="today"/>.</returns>
	/// <remarks>
	/// This check ignores the task's state. Callers that want "pending and overdue" combine it with <see cref="IsPending"/>.
	/// Taking the date as a parameter keeps this record free of clock dependencies.
	/// </remarks>
	public bool IsOverdueOn ( DateTime today ) => DueDate.Date < today.Date;
	
	
	/// <summary>
	/// Gets a value indicating whether the task has been completed.
	/// </summary>
	/// <value>
	/// <c>true</c> if the task's state is <see cref="TaskState.Completed"/>; otherwise, <c>false</c>.
	/// </value>
	public bool IsCompleted => State == TaskState.Completed;
	
	/// <summary>
	/// Gets a value indicating whether the task is pending.
	/// </summary>
	/// <value>
	/// <c>true</c> if the task state is <see cref="TaskState.Pending"/>; otherwise, <c>false</c>.
	/// </value>
	public bool IsPending => State == TaskState.Pending;
}