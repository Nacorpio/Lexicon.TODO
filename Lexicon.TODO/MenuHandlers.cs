using System;
using System.Collections.Generic;
using System.Text;

namespace Lexicon.TODO;

public static class MenuHandlers
{
	public static void HandleShowTasks(TaskManager taskManager)
	{
		MenuHelper.Show<SortMenuOption>("Sort Tasks", $"Choose how to sort the tasks", option => option.ToLabel());

		var choice = InputHelper.ReadInt("Enter your choice: ");

		Console.Clear();

		switch ((SortMenuOption)choice)
		{
			case SortMenuOption.ByDueDate:
				ConsoleHelper.PrintTasks(taskManager.GetTasksSortedByDueDate());
				break;

			case SortMenuOption.ByProject:
				ConsoleHelper.PrintTasks(taskManager.GetTasksSortedByProject());
				break;

			default:
				Console.WriteLine("Invalid choice. Please try again.");
				break;
		}
	}

	public static void HandleAddTask(TaskManager taskManager)
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

	public static void HandleCompleteTask(TaskManager taskManager)
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
}
