using Lexicon.TODO;

var taskManager = new TaskManager();

try 
{
	while (true)
	{
		MenuHelper.Show<MainMenuOption>(option => option.ToLabel());

		var choice = InputHelper.ReadInt("Enter your choice: ");

		switch ((MainMenuOption)choice)
		{
			case MainMenuOption.AddTask:
				HandleAddTask(taskManager);
				break;

			case MainMenuOption.ShowTasks:
				HandleShowTasks(taskManager);
				break;

			case MainMenuOption.CompleteTask:
				HandleCompleteTask(taskManager);
				break;

			case MainMenuOption.Exit:
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

static void HandleCompleteTask(TaskManager taskManager)
{
	var id = InputHelper.ReadInt("Enter the ID of the task to complete: ");

	if (taskManager.UpdateTask(id, state: TaskState.Completed))
	{
		Console.WriteLine($"Task with ID {id} marked as completed.");
	}
	else
	{
		Console.WriteLine($"No task found with ID {id}.");
	}
}

static void HandleShowTasks(TaskManager taskManager)
{
	var tasks = taskManager.GetAllTasks();

	if (tasks.Count == 0)
	{
		Console.WriteLine("No tasks available.");
		return;
	}

	Console.WriteLine("Current Tasks:");

	foreach (var task in tasks)
	{
		Console.WriteLine($"ID: {task.Id}, Title: {task.Title}, Due: {task.DueDate:yyyy-MM-dd}, State: {task.State}, Project: {task.Project}");
	}
}
