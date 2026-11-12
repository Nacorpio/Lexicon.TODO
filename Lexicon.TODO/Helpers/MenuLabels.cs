using Lexicon.TODO.Models;

namespace Lexicon.TODO.Helpers;

/// <summary>
/// Display labels for the menu enums. Each switch is exhaustive, so adding an enum member without a label
/// throws at first use rather than printing the raw member name.
/// </summary>
public static class MenuLabels
{
	/// <summary>
	/// Converts a <see cref="MainMenuOption"/> enumeration value to its corresponding label as a string.
	/// </summary>
	/// <param name="option">The <see cref="MainMenuOption"/> value to convert.</param>
	/// <returns>A string representation of the specified <see cref="MainMenuOption"/>.</returns>
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown when the provided <paramref name="option"/> is not a valid <see cref="MainMenuOption"/> value.
	/// </exception>
	public static string ToLabel ( this MainMenuOption option ) => option switch
	{
		MainMenuOption.ShowTasks => "Show Tasks",
		MainMenuOption.AddTask => "Add Task",
		MainMenuOption.EditTask => "Edit Task",
		MainMenuOption.SaveAndQuit => "Save and Quit",

		_ => throw new ArgumentOutOfRangeException ( nameof(option), option, null )
	};

	/// <summary>
	/// Converts a <see cref="SortMenuOption"/> enumeration value to its corresponding label as a string.
	/// </summary>
	/// <param name="option">The <see cref="SortMenuOption"/> value to convert.</param>
	/// <returns>A string representation of the specified <see cref="SortMenuOption"/>.</returns>
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown when the provided <paramref name="option"/> is not a valid <see cref="SortMenuOption"/> value.
	/// </exception>
	public static string ToLabel ( this SortMenuOption option ) => option switch
	{
		SortMenuOption.ByDueDate => "Sort by Due Date",
		SortMenuOption.ByProject => "Sort by Project",

		_ => throw new ArgumentOutOfRangeException ( nameof(option), option, null )
	};

	/// <summary>
	/// Converts an <see cref="EditMenuOption"/> enumeration value to its corresponding label as a string.
	/// </summary>
	/// <param name="option">The <see cref="EditMenuOption"/> value to convert.</param>
	/// <returns>A string representation of the specified <see cref="EditMenuOption"/>.</returns>
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown when the provided <paramref name="option"/> is not a valid <see cref="EditMenuOption"/> value.
	/// </exception>
	public static string ToLabel ( this EditMenuOption option ) => option switch
	{
		EditMenuOption.Update => "Update",
		EditMenuOption.MarkAsCompleted => "Mark As Completed",
		EditMenuOption.Delete => "Delete",

		_ => throw new ArgumentOutOfRangeException ( nameof(option), option, null )
	};

	/// <summary>
	/// Converts an <see cref="UpdateMenuOption"/> enumeration value to its corresponding label as a string.
	/// </summary>
	/// <param name="option">The <see cref="UpdateMenuOption"/> value to convert.</param>
	/// <returns>A string representation of the specified <see cref="UpdateMenuOption"/>.</returns>
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown when the provided <paramref name="option"/> is not a valid <see cref="UpdateMenuOption"/> value.
	/// </exception>
	public static string ToLabel ( this UpdateMenuOption option ) => option switch
	{
		UpdateMenuOption.EditTitle => "Edit Title",
		UpdateMenuOption.EditDueDate => "Edit Due Date",
		UpdateMenuOption.EditProject => "Edit Project",

		_ => throw new ArgumentOutOfRangeException ( nameof(option), option, null )
	};
}