using Lexicon.TODO;
using Lexicon.TODO.Helpers;
using Lexicon.TODO.Models;
using Lexicon.TODO.Services;

// Set up the file path for the tasks.json data file, which is located in the "Data" directory relative to the application's base directory.
string filePath = Path.Combine ( AppContext.BaseDirectory, "Data", "tasks.json" );

// Create an instance of TaskStorage to handle loading and saving tasks from/to the specified JSON file.
TaskStorage taskStorage = new ( filePath );

// Load tasks from the JSON file and capture the result, which includes the loaded tasks and any error message if loading failed.
LoadResult loadResult = taskStorage.Load ();

// Create a TaskManager instance with the loaded tasks.
TaskManager taskManager = new ( loadResult.Tasks );

// If loading failed, inform the user and indicate that changes cannot be saved.
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

		// Display the main menu with task statistics and prompt the user for a choice.
		MenuHelper.ShowMenu<MainMenuOption> ( "Main Menu",
			description:
				$"You have {taskManager.PendingCount} tasks to do " +
				$"({taskManager.OverdueCount} overdue), and " +
				$"{taskManager.CompletedCount} completed.",
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
					// Attempt to save the tasks to the data file if loading was successful.
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