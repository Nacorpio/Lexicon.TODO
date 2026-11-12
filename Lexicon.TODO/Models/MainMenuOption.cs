namespace Lexicon.TODO.Models;

/// <summary>
/// Represents the top-level actions available from the main menu.
/// </summary>
public enum MainMenuOption
{
	/// <summary>
	/// List tasks, after choosing a sort order.
	/// </summary>
	ShowTasks = 1,

	/// <summary>
	/// Create a new task.
	/// </summary>
	AddTask = 2,

	/// <summary>
	/// Update, complete, or delete an existing task.
	/// </summary>
	EditTask = 3,

	/// <summary>
	/// Save the tasks to the data file, then exit the application.
	/// </summary>
	SaveAndQuit = 4
}
