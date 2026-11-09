namespace Lexicon.TODO.Helpers;

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
		new ( "ID", ColumnAlignment.Right, static task => task.Id.ToString () ),
		new ( "Title", ColumnAlignment.Left, static task => task.Title ),
		new ( "Due Date", ColumnAlignment.Left, static task => task.DueDate.ToString ( "yyyy-MM-dd" ) ),
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
	/// <param name="separator">The string used to create the divider line. If not specified, '-' is used.</param>
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
		string? separator = null, 
		ConsoleColor? color = null )
	{
		ArgumentOutOfRangeException.ThrowIfNegative ( length );
		
		var divider = string.Concat ( Enumerable.Repeat ( separator ?? "-", length ) );
		
		PrintLine ( divider, color );
	}

	/// <summary>
	/// Prints a divider line to the console, with a length calculated based on the provided header, description, and menu item labels.
	/// </summary>
	/// <typeparam name="T">The enum type used to generate menu items.</typeparam>
	/// <param name="header">The header text displayed above the divider.</param>
	/// <param name="description">An optional description text displayed above the divider.</param>
	/// <param name="getLabel">A delegate that returns the display label for an enum value.</param>
	/// <param name="color">
	/// An optional <see cref="ConsoleColor"/> to use for the divider line.
	/// If not specified, the default console color is used.
	/// </param>
	/// <remarks>
	/// The divider length is dynamically calculated to ensure proper alignment with the menu content.
	/// </remarks>
	public static void PrintMenuDivider<T> (
		string header, string? description,
		Func<T, string> getLabel,
		ConsoleColor? color = null )
		where T : struct, Enum
	{
		int length = CalculateDividerLength ( header, description, getLabel );
		var divider = new string ( '-', length );

		PrintLine ( divider, color );
	}

	/// <summary>
	/// Calculates the length of the divider required to properly format the menu display.
	/// </summary>
	/// <typeparam name="T">The enum type used to generate menu items.</typeparam>
	/// <param name="header">The header text to include in the calculation.</param>
	/// <param name="description">The optional description text to include in the calculation.</param>
	/// <param name="getLabel">A delegate that returns the display label for an enum value.</param>
	/// <returns>The maximum length between the header, description, and the widest formatted menu item label.</returns>
	/// <remarks>
	/// The menu item length is determined by combining the numeric value of the enum, a 3-character separator, 
	/// and the length of the label returned by <paramref name="getLabel"/>.
	/// </remarks>
	private static int CalculateDividerLength<T> (
		string header, string? description,
		Func<T, string> getLabel )
		where T : struct, Enum
	{
		var menuItems = Enum.GetValues<T> ();

		int dividerLength = Math.Max ( header.Length,
			menuItems.Max ( option =>
				(Convert.ToInt32 ( option ).ToString ().Length + 3 + getLabel ( option ).Length) ) );

		if ( !string.IsNullOrWhiteSpace ( description ) )
			dividerLength = Math.Max ( dividerLength, description.Length );

		return dividerLength;
	}
}