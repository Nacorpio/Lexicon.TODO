using Lexicon.TODO.Helpers;

namespace Lexicon.TODO;

public static class MenuHandlers
{
	/// <summary>
	/// Handles the display of tasks by presenting a sorting menu and showing tasks based on the selected sorting option.
	/// </summary>
	/// <param name="taskManager">
	/// An instance of <see cref="TaskManager"/> that provides access to the tasks to be displayed and sorted.
	/// </param>
	/// <remarks>
	/// This method allows users to choose how tasks are sorted (e.g., by due date or by project) and displays the sorted tasks.
	/// If an invalid choice is made, an error message is displayed.
	/// </remarks>
	public static void HandleShowTasks ( TaskManager taskManager )
	{
		MenuHelper.ShowMenu<SortMenuOption> (
			header: "Sort Tasks",
			description: $"Choose how to sort the tasks",
			getLabel: option => option.ToLabel (),
			headerColor: ConsoleColor.Green );

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

	/// <summary>
	/// Handles the process of adding a new task to the task manager.
	/// </summary>
	/// <param name="taskManager">
	/// An instance of <see cref="TaskManager"/> used to manage the collection of tasks.
	/// </param>
	/// <remarks>
	/// This method prompts the user to input the task title, due date, and optionally a project name.
	/// It then creates a new task using the provided details and adds it to the task manager.
	/// </remarks>
	/// <example>
	/// <code>
	/// var taskManager = new TaskManager();
	/// MenuHandlers.HandleAddTask(taskManager);
	/// </code>
	/// </example>
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

	/// <summary>
	/// Handles the process of editing a task by providing options to update, mark as completed, or delete the task.
	/// </summary>
	/// <param name="taskManager">
	/// An instance of <see cref="TaskManager"/> used to manage and retrieve tasks.
	/// </param>
	/// <remarks>
	/// This method interacts with the user to select a task by its ID and perform the desired edit operation.
	/// If the task is not found, an appropriate message is displayed.
	/// </remarks>
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

	/// <summary>
	/// Updates the details of an existing task identified by its ID.
	/// </summary>
	/// <param name="taskManager">
	/// An instance of <see cref="TaskManager"/> used to manage tasks.
	/// </param>
	/// <param name="id">
	/// The unique identifier of the task to be updated.
	/// </param>
	/// <remarks>
	/// This method allows updating the title, due date, and project of a task. 
	/// If a field is left blank during input, the current value of that field is retained.
	/// </remarks>
	private static void HandleUpdateTask ( TaskManager taskManager, int id )
	{
		var newTitle = InputHelper.ReadOptionalString ( "Enter new title (leave blank to keep current): " );
		var newDueDateInput =
			InputHelper.ReadDateTime ( "Enter new due date (yyyy-MM-dd) (leave blank to keep current): " );
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

	/// <summary>
	/// Handles the deletion of a task by its ID.
	/// </summary>
	/// <param name="taskManager">The <see cref="TaskManager"/> instance used to manage tasks.</param>
	/// <remarks>
	/// This method prompts the user to enter the ID of the task to delete. If the task exists, 
	/// the user is asked to confirm the deletion. Upon confirmation, the task is deleted. 
	/// If the task does not exist or the deletion is canceled, appropriate messages are displayed.
	/// </remarks>
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

	/// <summary>
	/// Marks a task as completed based on its ID.
	/// </summary>
	/// <param name="taskManager">The <see cref="TaskManager"/> instance used to manage tasks.</param>
	/// <remarks>
	/// Prompts the user to enter the ID of the task they wish to complete. If the task is found, 
	/// it is marked as completed. Otherwise, a message is displayed indicating that no task was found with the specified ID.
	/// </remarks>
	public static void HandleCompleteTask ( TaskManager taskManager )
	{
		var id = InputHelper.ReadInt ( "Enter the ID of the task to complete: " );

		Console.WriteLine ( taskManager.UpdateTask ( id, state: TaskState.Completed )
			? $"Task with ID {id} marked as completed."
			: $"No task found with ID {id}." );
	}
}