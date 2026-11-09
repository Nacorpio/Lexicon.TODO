using System.Globalization;
using Lexicon.TODO.Models;

namespace Lexicon.TODO.Helpers;

public static partial class ConsoleHelper
{
	/// <summary>
	/// Clears the console when output is attached to a terminal.
	/// </summary>
	/// <remarks>
	/// <see cref="Console.Clear"/> throws <see cref="IOException"/> when standard output is redirected
	/// (for example, piped to a file), so this method is a no-op in that case.
	/// </remarks>
	public static void Clear ()
	{
		if ( !Console.IsOutputRedirected )
			Console.Clear ();
	}

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
		new ( "ID", ColumnAlignment.Right, static task => task.Id.ToString ( CultureInfo.InvariantCulture ) ),
		new ( "Title", ColumnAlignment.Left, static task => task.Title ),
		new ( "Due Date", ColumnAlignment.Left, static task => task.DueDate.ToString ( "yyyy-MM-dd", CultureInfo.InvariantCulture ) ),
		new ( "Project", ColumnAlignment.Left, static task => task.Project ?? string.Empty ),
		new ( "State", ColumnAlignment.Left, static task => task.State.ToString () )
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
			PrintLine ( "No tasks available." );
			return;
		}

		PrintTable ( DefaultTaskColumns, tasks, " | " );
	}

	/// <summary>
	/// Prints a divider line to the console using a specified character and length, optionally with a specified color.
	/// </summary>
	/// <param name="length">The length of the divider line. Must be a non-negative value.</param>
	/// <param name="separator">The character repeated to form the divider line. Defaults to '-'.</param>
	/// <param name="color">
	/// An optional <see cref="ConsoleColor"/> to use for the divider line text.
	/// If not specified, the default console color is used.
	/// </param>
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown if the <paramref name="length"/> is negative.
	/// </exception>
	/// <remarks>
	/// This method is useful for visually separating sections of console output.
	/// </remarks>
	public static void PrintDivider ( int length,
		char separator = '-',
		ConsoleColor? color = null )
	{
		ArgumentOutOfRangeException.ThrowIfNegative ( length );

		PrintLine ( new string ( separator, length ), color );
	}
}