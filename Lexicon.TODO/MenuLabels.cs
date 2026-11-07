namespace Lexicon.TODO;

public static class MenuLabels 
{
	public static string ToLabel(this MainMenuOption option) => option switch
	{
		MainMenuOption.ShowTasks => "Show Tasks",
		MainMenuOption.AddTask => "Add Task",
		MainMenuOption.SortTasks => "Sort Tasks",
		MainMenuOption.EditTask => "Edit Task",
		MainMenuOption.SaveAndQuit => "Save and Quit",

		_ => throw new ArgumentOutOfRangeException(nameof(option), option, null)
	};

	public static string ToLabel(this SortMenuOption option) => option switch
	{
		SortMenuOption.ByDueDate => "Sort by Due Date",
		SortMenuOption.ByProject => "Sort by Project",

		_ => throw new ArgumentOutOfRangeException(nameof(option), option, null)
	};

	public static string ToLabel(this EditMenuOption option) => option switch
	{
		EditMenuOption.Update => "Update",
		EditMenuOption.MarkAsCompleted => "Mark As Completed",
		EditMenuOption.Delete => "Delete",

		_ => throw new ArgumentOutOfRangeException(nameof(option), option, null)
	};

	public static string ToLabel(this UpdateMenuOption option) => option switch
	{
		UpdateMenuOption.EditTitle => "Edit Title",
		UpdateMenuOption.EditDueDate => "Edit Due Date",
		UpdateMenuOption.EditProject => "Edit Project",

		_ => throw new ArgumentOutOfRangeException(nameof(option), option, null)
	};
}
