using Lexicon.TODO.Helpers;

namespace Lexicon.TODO;

public static class MenuHandlers
{
	public static void HandleShowTasks ( TaskManager taskManager )
	{
		MenuHelper.ShowMenu<SortMenuOption> (
			header: "Sort Tasks",
			description: $"Choose how to sort the tasks",
			getLabel: option => option.ToLabel () );

		var choice = InputHelper.ReadInt ( "Enter your choice: " );

		Console.Clear ();

		switch ((SortMenuOption) choice)
		{
			case SortMenuOption.ByDueDate:
				ConsoleHelper.PrintTasks ( taskManager.GetTasksSortedByDueDate () );
				break;

			case SortMenuOption.ByProject:
				ConsoleHelper.PrintTasks ( taskManager.GetTasksSortedByProject () );
				break;

			default:
				Console.WriteLine ( "Invalid choice. Please try again." );
				break;
		}
	}

	public static void HandleAddTask ( TaskManager taskManager )
	{
		var title = InputHelper.ReadNonEmptyString ( "Enter task title: " );
		var dueDate = InputHelper.ReadDateTime ( "Enter due date (yyyy-MM-dd): " );
		var project = InputHelper.ReadOptionalString ( "Enter project name (optional): " );

		var task = taskManager.AddTask ( title, dueDate,
			string.IsNullOrWhiteSpace ( project )
				? null
				: project );

		Console.WriteLine ( $"Task added with ID: {task.Id}" );
	}

	public static void HandleEditTask ( TaskManager taskManager )
	{
		var id = InputHelper.ReadInt ( "Enter the ID of the task to edit: " );
		var task = taskManager.FindById ( id );

		if ( task is null )
		{
			Console.WriteLine ( $"No task found with ID {id}." );
			return;
		}

		Console.Clear ();

		ConsoleHelper.PrintLine ();
		ConsoleHelper.PrintTasks ( [task] );
		ConsoleHelper.PrintLine ();

		MenuHelper.ShowMenu<EditMenuOption> (
			header: "Edit Task",
			description: $"Choose what to edit for task ID {id}",
			getLabel: option => option.ToLabel () );

		var choice = InputHelper.ReadInt ( "Enter your choice: " );

		Console.Clear ();

		switch ((EditMenuOption) choice)
		{
			case EditMenuOption.MarkAsCompleted:
			{
				Console.WriteLine ( taskManager.CompleteTask ( id )
					? $"Task with ID {id} marked as completed."
					: $"No task found with ID {id}." );

				break;
			}

			case EditMenuOption.Delete:
			{
				HandleDeleteTask ( taskManager );
				break;
			}

			case EditMenuOption.Update:
			{
				HandleUpdateTask ( taskManager, id );
				break;
			}

			default:
			{
				Console.WriteLine ( "Invalid choice. Please try again." );
				break;
			}
		}
	}

	private static void HandleUpdateTask ( TaskManager taskManager, int id )
	{
		var newTitle = InputHelper.ReadOptionalString ( "Enter new title (leave blank to keep current): " );
		var newDueDateInput = InputHelper.ReadDateTime ( "Enter new due date (yyyy-MM-dd) (leave blank to keep current): " );
		var newProject = InputHelper.ReadOptionalString ( "Enter new project name (leave blank to keep current): " );

		Console.WriteLine ( taskManager.UpdateTask ( id,
			title: string.IsNullOrWhiteSpace ( newTitle )
				? null
				: newTitle,
			dueDate: newDueDateInput == DateTime.MaxValue
				? null
				: newDueDateInput,
			project: string.IsNullOrWhiteSpace ( newProject )
				? null
				: newProject )
			? $"Task with ID {id} updated."
			: $"No task found with ID {id}." );

		return;
	}

	public static void HandleDeleteTask ( TaskManager taskManager )
	{
		var id = InputHelper.ReadInt ( "Enter the ID of the task to delete: " );

		if ( !taskManager.ContainsTask ( id ) )
		{
			Console.WriteLine ( $"No task found with ID {id}." );
			return;
		}

		if ( InputHelper.Confirm ( $"Are you sure you want to delete task with ID {id}? (y/n): " ) )
		{
			Console.WriteLine ( taskManager.DeleteTask ( id )
				? $"Task with ID {id} deleted."
				: $"No task found with ID {id}." );
		}
		else
		{
			Console.WriteLine ( "Deletion cancelled." );
		}
	}

	public static void HandleCompleteTask ( TaskManager taskManager )
	{
		var id = InputHelper.ReadInt ( "Enter the ID of the task to complete: " );

		Console.WriteLine ( taskManager.UpdateTask ( id, state: TaskState.Completed )
			? $"Task with ID {id} marked as completed."
			: $"No task found with ID {id}." );
	}
}