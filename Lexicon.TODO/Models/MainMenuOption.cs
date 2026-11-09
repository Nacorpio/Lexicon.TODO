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
	/// Exit the application. Tasks are held in memory only and are not persisted.
	/// </summary>
	Quit = 4
}
