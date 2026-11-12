using Lexicon.TODO.Helpers;
using Lexicon.TODO.Models;
using Lexicon.TODO.Services;

namespace Lexicon.TODO;

/// <summary>
/// Provides static methods for handling various menu-related operations in the TO-DO application.
/// </summary>
/// <remarks>
/// This class includes methods for managing tasks, such as displaying, adding, editing, deleting, 
/// and completing tasks. It interacts with the <see cref="TaskManager"/> to perform these operations 
/// and provides user-friendly menus for task management.
/// </remarks>
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
			description: "Choose how to sort the tasks",
			getLabel: option => option.ToLabel (),
			headerColor: ConsoleColor.Green );

		var choice = InputHelper.ReadInt ( "Enter your choice: " );

		ConsoleHelper.Clear ();

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
		var dueDate = InputHelper.ReadRequiredDateTime ( "Enter due date (yyyy-MM-dd): " );
		var project = InputHelper.ReadOptionalString ( "Enter project name (optional): " );

		TaskItem task = taskManager.AddTask ( title, dueDate, project );

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

		ConsoleHelper.Clear ();

		ConsoleHelper.PrintLine ();
		ConsoleHelper.PrintTasks ( [task] );
		ConsoleHelper.PrintLine ();

		MenuHelper.ShowMenu<EditMenuOption> (
			header: "Edit Task",
			description: $"Choose what to edit for task ID {id}",
			getLabel: option => option.ToLabel () );

		var choice = InputHelper.ReadInt ( "Enter your choice: " );

		ConsoleHelper.Clear ();

		ConsoleHelper.PrintTasks ( [task] );
		ConsoleHelper.PrintLine ();

		switch ((EditMenuOption) choice)
		{
			case EditMenuOption.MarkAsCompleted:
				HandleCompleteTask ( taskManager, id );
				break;

			case EditMenuOption.Delete:
				HandleDeleteTask ( taskManager, id );
				break;

			case EditMenuOption.Update:
				HandleUpdateTask ( taskManager, id );
				break;

			default:
				Console.WriteLine ( "Invalid choice. Please try again." );
				break;
		}
	}

	/// <summary>
	/// Updates the details of a specific task identified by its ID.
	/// </summary>
	/// <param name="taskManager">
	/// An instance of <see cref="TaskManager"/> used to manage and update tasks.
	/// </param>
	/// <param name="id">
	/// The unique identifier of the task to be updated.
	/// </param>
	/// <remarks>
	/// This method provides a menu-driven interface for updating specific attributes of a task,
	/// such as its title, due date, or associated project. It uses the <see cref="MenuHelper.ShowMenu{TEnum}"/>
	/// method to display options and the <see cref="InputHelper"/> methods to collect user input.
	/// Leaving the due date blank keeps the current value and reports that no change was made.
	/// An invalid menu option prints an error message; nothing is thrown.
	/// </remarks>
	private static void HandleUpdateTask ( TaskManager taskManager, int id )
	{
		MenuHelper.ShowMenu<UpdateMenuOption> (
			header: "Update Task",
			description: $"Choose what to update for task ID {id}",
			getLabel: option => option.ToLabel () );

		var choice = InputHelper.ReadInt ( "Enter your choice: " );

		ConsoleHelper.Clear ();


		bool updated;

		switch ((UpdateMenuOption) choice)
		{
			case UpdateMenuOption.EditTitle:
			{
				updated = taskManager.UpdateTask ( id,
					title: InputHelper.ReadNonEmptyString ( "Enter new title: " ) );

				break;
			}

			case UpdateMenuOption.EditDueDate:
			{
				updated = taskManager.UpdateTask ( id,
					dueDate: InputHelper.ReadDateTime ( "Enter new due date (yyyy-MM-dd): " ) );

				break;
			}

			case UpdateMenuOption.EditProject:
			{
				updated = taskManager.UpdateTask ( id,
					project: InputHelper.ReadNonEmptyString ( "Enter new project name: " ) );

				break;
			}

			default:
			{
				Console.WriteLine ( "Invalid choice. Please try again." );

				return;
			}
		}

		Console.WriteLine ( updated
			? $"Task with ID {id} updated."
			: $"No task found with ID {id}." );
	}

	/// <summary>
	/// Handles the deletion of a task by its ID.
	/// </summary>
	/// <param name="taskManager">The <see cref="TaskManager"/> instance used to manage tasks.</param>
	/// <param name="id">The unique identifier of the task to be deleted.</param>
	/// <remarks>
	/// Asks the user to confirm the deletion. Upon confirmation, the task is deleted.
	/// If the task does not exist or the deletion is cancelled, an appropriate message is displayed.
	/// </remarks>
	private static void HandleDeleteTask ( TaskManager taskManager, int id )
	{
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
	/// <param name="id">The unique identifier of the task to be marked as completed.</param>
	/// <remarks>
	/// If the task is found, it is marked as completed. Otherwise, a message is displayed
	/// indicating that no task was found with the specified ID.
	/// </remarks>
	private static void HandleCompleteTask ( TaskManager taskManager, int id )
	{
		Console.WriteLine ( taskManager.CompleteTask ( id )
			? $"Task with ID {id} marked as completed."
			: $"No task found with ID {id}." );
	}
}