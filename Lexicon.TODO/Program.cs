using Lexicon.TODO;

var taskManager = new TaskManager();

AddSampleTasks(taskManager);

try 
{
	while (true)
	{
		Console.Clear();

		MenuHelper.ShowMenu<MainMenuOption>("Main Menu",
			description: $"You have {taskManager.PendingTasks.Count} tasks to do, and {taskManager.CompletedTasks.Count} completed.", 
			getLabel: option => option.ToLabel(), 
			headerColor: ConsoleColor.Green);

		var choice = InputHelper.ReadInt("Enter your choice: ");

		Console.Clear();

		switch ((MainMenuOption)choice)
		{
			case MainMenuOption.AddTask:
				MenuHandlers.HandleAddTask(taskManager);
				break;

			case MainMenuOption.ShowTasks:
				MenuHandlers.HandleShowTasks(taskManager);
				break;

			case MainMenuOption.EditTask:
				//HandleEditTask(taskManager);
				break;

			case MainMenuOption.SaveAndQuit:
				Console.WriteLine("Exiting...");
				return;

			default:
				Console.WriteLine("Invalid choice. Please try again.");
				break;
		}

		Console.WriteLine();
		Console.WriteLine("Press Enter to continue...");

		Console.ReadLine();
	}
}
catch (EndOfStreamException)
{
	Console.WriteLine("Input ended unexpectedly. Exiting...");
}

static void AddSampleTasks(TaskManager taskManager)
{
	taskManager.AddTask("Finish project report", new DateTime(2024, 6, 15), "Work");
	taskManager.AddTask("Grocery shopping", new DateTime(2024, 6, 10), "Personal");
	taskManager.AddTask("Doctor's appointment", new DateTime(2024, 6, 12), "Health");
}