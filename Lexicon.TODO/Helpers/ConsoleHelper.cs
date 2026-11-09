namespace Lexicon.TODO.Helpers;

/// <summary>
/// Defines a column in a tabular display of task information.
/// </summary>
/// <remarks>
/// Each column is characterized by a header, an alignment, and a function to extract the corresponding value from a <see cref="TaskItem"/>.
/// This record is primarily used to configure the layout and content of task tables displayed in the console.
/// </remarks>
public record ColumnDefinition ( string Header, ColumnAlignment Alignment, Func<TaskItem, string> ValueSelector );

/// <summary>
/// Specifies the alignment of content within a column.
/// </summary>
/// <remarks>
/// This enumeration is used to define how text is aligned in a column when displaying data in a tabular format.
/// </remarks>
public enum ColumnAlignment
{
	Left,
	Right
}

public static partial class ConsoleHelper
{
	/// <summary>
	/// Defines the default columns used for displaying task information in a table format.
	/// </summary>
	/// <remarks>
	/// Each column is represented by a <see cref="ColumnDefinition"/> that specifies the header, alignment, 
	/// and a function to extract the corresponding value from a <see cref="TaskItem"/>.
	/// The default columns include:
	/// - ID: Right-aligned, displays the task ID.
	/// - Title: Left-aligned, displays the task title.
	/// - Due Date: Left-aligned, displays the due date in "yyyy-MM-dd" format.
	/// - Project: Left-aligned, displays the associated project or an empty string if none.
	/// - State: Left-aligned, displays the task state.
	/// </remarks>
	private static readonly IReadOnlyList<ColumnDefinition> DefaultTaskColumns =
	[
		new ColumnDefinition ( "ID", ColumnAlignment.Right, static task => task.Id.ToString () ),
		new ColumnDefinition ( "Title", ColumnAlignment.Left, static task => task.Title ),
		new ColumnDefinition ( "Due Date", ColumnAlignment.Left, static task => task.DueDate.ToString ( "yyyy-MM-dd" ) ),
		new ColumnDefinition ( "Project", ColumnAlignment.Left, static task => task.Project ?? string.Empty ),
		new ColumnDefinition ( "State", ColumnAlignment.Left, static task => task.State.ToString () )
	];

	
	/// <summary>
	/// Prints a message to the console, optionally using a specified color.
	/// </summary>
	/// <param name="message">The message to be printed to the console.</param>
	/// <param name="color">
	/// An optional <see cref="ConsoleColor"/> to use for the message text. 
	/// If not specified, the default console color is used.
	/// </param>
	/// <remarks>
	/// If a color is specified, the console's foreground color is temporarily changed 
	/// to the specified color while printing the message, and then reset to the default color.
	/// </remarks>
	public static void Print ( string message, ConsoleColor? color = null )
	{
		if ( color.HasValue )
			Console.ForegroundColor = color.Value;

		Console.Write ( message );
		Console.ResetColor ();
	}

	/// <summary>
	/// Prints a message followed by a newline to the console, optionally using a specified color.
	/// </summary>
	/// <param name="message">The message to be printed to the console.</param>
	/// <param name="color">
	/// An optional <see cref="ConsoleColor"/> to use for the message text.
	/// If not specified, the default console color is used.
	/// </param>
	/// <remarks>
	/// If a color is specified, the console's foreground color is temporarily changed
	/// to the specified color while printing the message, and then reset to the default color.
	/// </remarks>
	public static void PrintLine ( string message, ConsoleColor? color = null )
	{
		if ( color.HasValue )
			Console.ForegroundColor = color.Value;

		Console.WriteLine ( message );
		Console.ResetColor ();
	}

	/// <summary>
	/// Writes a blank line to the console.
	/// </summary>
	/// <remarks>
	/// This method is useful for adding spacing between console outputs.
	/// </remarks>
	public static void PrintLine ()
	{
		Console.WriteLine ();
	}

	/// <summary>
	/// Prints a list of tasks to the console in a tabular format.
	/// </summary>
	/// <param name="tasks">The list of tasks to be printed. Each task is represented as a <see cref="TaskItem"/>.</param>
	/// <remarks>
	/// If the provided list of tasks is empty, a message indicating that no tasks are available will be displayed.
	/// </remarks>
	public static void PrintTasks ( IReadOnlyList<TaskItem> tasks )
	{
		if ( tasks.Count == 0 )
		{
			ConsoleHelper.PrintLine ( "No tasks available." );
			return;
		}

		PrintTable ( DefaultTaskColumns, tasks, " | " );
	}
}