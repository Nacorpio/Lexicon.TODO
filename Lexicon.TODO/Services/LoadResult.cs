using Lexicon.TODO.Models;

namespace Lexicon.TODO.Services;

/// <summary>
/// Represents the result of a task loading operation, containing a list of tasks and an optional error message.
/// </summary>
/// <remarks>
/// This record encapsulates the outcome of loading tasks, providing both the loaded tasks and any error encountered during the process.
/// If no error occurs, the <c>Error</c> property is <c>null</c>, and the <c>CanSave</c> property indicates that changes can be saved.
/// </remarks>
public record LoadResult(List<TaskItem> Tasks, string? Error = null)
{
	/// <summary>
	/// Gets a value indicating whether changes can be saved.
	/// </summary>
	/// <value>
	/// <c>true</c> if there is no error; otherwise, <c>false</c>.
	/// </value>
	/// <remarks>
	/// This property evaluates to <c>true</c> when the <c>Error</c> property is <c>null</c>, 
	/// indicating that the loaded tasks are in a valid state for saving. If an error occurred 
	/// during the loading process, this property will return <c>false</c>.
	/// </remarks>
	public bool CanSave => Error is null;
}