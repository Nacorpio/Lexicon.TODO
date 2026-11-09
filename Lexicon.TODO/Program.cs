using Lexicon.TODO;
using Lexicon.TODO.Helpers;
using Lexicon.TODO.Models;
using Lexicon.TODO.Services;

var taskManager = new TaskManager ();

AddSampleTasks ( taskManager );

try
{
	while (true)
	{
		ConsoleHelper.Clear ();

		MenuHelper.ShowMenu<MainMenuOption> ( "Main Menu",
			description:
			$"You have {taskManager.PendingCount} tasks to do, and {taskManager.CompletedCount} completed.",
			getLabel: option => option.ToLabel (),
			headerColor: ConsoleColor.Green );

		var choice = InputHelper.ReadInt ( "Enter your choice: " );

		ConsoleHelper.Clear ();

		switch ((MainMenuOption) choice)
		{
			case MainMenuOption.AddTask:
				MenuHandlers.HandleAddTask ( taskManager );
				break;

			case MainMenuOption.ShowTasks:
				MenuHandlers.HandleShowTasks ( taskManager );
				break;

			case MainMenuOption.EditTask:
				MenuHandlers.HandleEditTask ( taskManager );
				break;

			case MainMenuOption.Quit:
				Console.WriteLine ( "Exiting..." );
				return 0;

			default:
				Console.WriteLine ( "Invalid choice. Please try again." );
				break;
		}

		Console.WriteLine ();
		InputHelper.WaitForEnter ( "Press Enter to continue..." );
	}
}
catch (EndOfStreamException)
{
	Console.WriteLine ( "Input ended unexpectedly. Exiting..." );
	return 1;
}

// Seeds demo data relative to today so one task is overdue and two are upcoming.
static void AddSampleTasks ( TaskManager taskManager )
{
	var today = DateTime.Today;

	taskManager.AddTask ( "Finish project report", today.AddDays ( 5 ), "Work" );
	taskManager.AddTask ( "Grocery shopping", today.AddDays ( -2 ), "Personal" );
	taskManager.AddTask ( "Doctor's appointment", today.AddDays ( 1 ), "Health" );
}