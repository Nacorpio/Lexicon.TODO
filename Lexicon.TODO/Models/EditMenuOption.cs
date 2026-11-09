namespace Lexicon.TODO.Models;

/// <summary>
/// Represents the available options in the edit menu.
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
