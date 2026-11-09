namespace Lexicon.TODO.Models;

/// <summary>
/// Represents the available options for updating a task in the menu.
/// </summary>
public enum UpdateMenuOption
{
	/// <summary>
	/// Represents the option to edit the title of a task in the update menu.
	/// </summary>
	EditTitle = 1,
	
	/// <summary>
	/// Represents the option to edit the due date of a task in the update menu.
	/// </summary>
	EditDueDate = 2,
	
	/// <summary>
	/// Represents the option to edit the project associated with a task in the update menu.
	/// </summary>
	EditProject = 3
}