namespace Lexicon.TODO;

/// <summary>
/// Represents the available options in the edit menu.
/// </summary>
/// <summary>
/// Option to update the details of a task.
/// </summary>
/// <summary>
/// Option to mark a task as completed.
/// </summary>
/// <summary>
/// Option to delete a task.
/// </summary>
public enum EditMenuOption 
{
	/// <summary>
	/// Represents the option to update the details of an existing task in the edit menu.
	/// </summary>
	Update = 1,
	
	/// <summary>
	/// Represents the option to mark an existing task as completed in the edit menu.
	/// </summary>
	MarkAsCompleted = 2,
	
	/// <summary>
	/// Represents the option to delete an existing task in the edit menu.
	/// </summary>
	Delete = 3
}
