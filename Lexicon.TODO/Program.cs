using Lexicon.TODO;
using Lexicon.TODO.Helpers;
using Lexicon.TODO.Models;
using Lexicon.TODO.Services;

string filePath = Path.Combine ( AppContext.BaseDirectory, "Data", "tasks.json" );

TaskStorage taskStorage = new ( filePath );

LoadResult loadResult = taskStorage.Load ();
TaskManager taskManager = new ( loadResult.Tasks );

if ( !loadResult.CanSave )
{
	Console.WriteLine ( $"Could not read {filePath}: {loadResult.Error}" );
	Console.WriteLine ( "Starting empty. Changes will NOT be saved this session." );

	InputHelper.WaitForEnter ( "Press Enter to continue..." );
}

taskStorage.Load ();

try
{
	while (true)
	{
		ConsoleHelper.Clear ();

		MenuHelper.ShowMenu<MainMenuOption> ( "Main Menu",
			description:
			$"You have {taskManager.PendingCount} tasks to do ({taskManager.OverdueCount} overdue), and {taskManager.CompletedCount} completed.",
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

			case MainMenuOption.SaveAndQuit:
			{
				try
				{
					if ( loadResult.CanSave )
						taskStorage.Save ( [.. taskManager.Tasks] );
				}
				catch (Exception ex)
				{
					Console.WriteLine ( $"Error saving tasks: {ex.Message}" );
					break;
				}

				Console.WriteLine ( "Exiting..." );

				return 0;
			}

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