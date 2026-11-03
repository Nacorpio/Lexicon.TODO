using Lexicon.TODO;

var taskManager = new TaskManager();

try 
{
	while (true)
	{
		ShowMenu();

		var choice = InputHelper.ReadInt("Enter your choice: ");

		switch (choice)
		{
			case 1:
				HandleAddTask(taskManager);
				break;
			case 2:
				Console.WriteLine("Exiting...");
				return;
			default:
				Console.WriteLine("Invalid choice. Please try again.");
				break;
		}
	}
}
catch (EndOfStreamException)
{
	Console.WriteLine("Input ended unexpectedly. Exiting...");
}


static void HandleAddTask(TaskManager taskManager)
{
	var title = InputHelper.ReadNonEmptyString("Enter task title: ");
	var dueDate = InputHelper.ReadDateTime("Enter due date (yyyy-MM-dd): ");
	var project = InputHelper.ReadOptionalString("Enter project name (optional): ");
	
	try
	{
		var task = taskManager.AddTask(title!, dueDate, 
			string.IsNullOrWhiteSpace(project) 
				? null 
				: project);
		
		Console.WriteLine($"Task added with ID: {task.Id}");
	}
	catch (ArgumentNullException ex)
	{
		Console.WriteLine($"Error: {ex.Message}");
	}
}

static void ShowMenu()
{
	Console.WriteLine("1. Add Task");
	Console.WriteLine("2. Exit");
}