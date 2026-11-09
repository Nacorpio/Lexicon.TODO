namespace Lexicon.TODO.Models;

/// <summary>
/// Represents the available sort orders in the show-tasks menu.
/// </summary>
public enum SortMenuOption
{
	/// <summary>
	/// Sort tasks by due date, earliest first.
	/// </summary>
	ByDueDate = 1,

	/// <summary>
	/// Sort tasks by project name, then by due date; tasks without a project come last.
	/// </summary>
	ByProject = 2
}
