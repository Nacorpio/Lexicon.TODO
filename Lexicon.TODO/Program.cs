using Lexicon.TODO;

Console.WriteLine("Hello, World!");

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