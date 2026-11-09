using System;
using System.Collections.Generic;
using System.Text;

namespace Lexicon.TODO;

public static class MenuHandlers
{
	public static void HandleShowTasks(TaskManager taskManager)
	{
		MenuHelper.ShowMenu<SortMenuOption>("Sort Tasks", $"Choose how to sort the tasks", option => option.ToLabel());

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

	public static void HandleEditTask(TaskManager taskManager)
	{
		var id = InputHelper.ReadInt("Enter the ID of the task to edit: ");
		var task = taskManager.FindById(id);

		if (task is null)
		{
			Console.WriteLine($"No task found with ID {id}.");
			return;
		}

		ConsoleHelper.PrintLine();
		ConsoleHelper.PrintTasks([task]);
		ConsoleHelper.PrintLine();

		MenuHelper.ShowMenu<EditMenuOption>("Edit Task", $"Choose what to edit for task ID {id}", option => option.ToLabel());

		var choice = InputHelper.ReadInt("Enter your choice: ");

		switch ((EditMenuOption)choice)
		{
			case EditMenuOption.MarkAsCompleted:
			{		
				if (taskManager.CompleteTask(id))
					Console.WriteLine($"Task with ID {id} marked as completed.");
				else
					Console.WriteLine($"No task found with ID {id}.");

				break;
			}

			case EditMenuOption.Delete:
			{
				if (taskManager.DeleteTask(id))
					Console.WriteLine($"Task with ID {id} deleted.");
				else
					Console.WriteLine($"No task found with ID {id}.");
					
				break;
			}
			
			case EditMenuOption.Update:
			{
				var newTitle = InputHelper.ReadOptionalString("Enter new title (leave blank to keep current): ");
				var newDueDateInput = InputHelper.ReadDateTime("Enter new due date (yyyy-MM-dd) (leave blank to keep current): ");
				var newProject = InputHelper.ReadOptionalString("Enter new project name (leave blank to keep current): ");

				if (taskManager.UpdateTask(id,
					title: string.IsNullOrWhiteSpace(newTitle) ? null : newTitle,
					dueDate: newDueDateInput == default ? null : newDueDateInput,
					project: string.IsNullOrWhiteSpace(newProject) ? null : newProject))
				{
					Console.WriteLine($"Task with ID {id} updated.");
				}
				else
				{
					Console.WriteLine($"No task found with ID {id}.");
				}

				break;
			}

			default:

				Console.WriteLine("Invalid choice. Please try again.");

				break;

		}
	}

	public static void HandleDeleteTask(TaskManager taskManager)
	{
		var id = InputHelper.ReadInt("Enter the ID of the task to delete: ");

		if (!taskManager.ContainsTask(id))
		{
			Console.WriteLine($"No task found with ID {id}.");
			return;
		}

		if (InputHelper.Confirm($"Are you sure you want to delete task with ID {id}? (y/n): "))
		{
			if (taskManager.DeleteTask(id))
			{
				Console.WriteLine($"Task with ID {id} deleted.");
			}
			else
			{
				Console.WriteLine($"No task found with ID {id}.");
			}
		}
		else
		{
			Console.WriteLine("Deletion cancelled.");
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
