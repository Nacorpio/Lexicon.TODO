using Lexicon.TODO.Models;

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

/// <summary>
/// Provides helper methods for console-based operations, such as printing formatted tables and rows.
/// </summary>
/// <remarks>
/// This class is designed to assist with rendering structured data, like task lists, in a console application.
/// It includes methods for printing tables, rows, and other formatted outputs.
/// </remarks>
public static partial class ConsoleHelper
{
	/// <summary>
	/// Prints a formatted table to the console based on the provided column definitions and task items.
	/// </summary>
	/// <param name="columnDefs">The definitions of the table columns, including headers, alignments, and value selectors.</param>
	/// <param name="tasks">The list of task items to be displayed in the table rows.</param>
	/// <param name="separator">The string used to separate columns in the table.</param>
	/// <remarks>
	/// This method renders a table with a header row and task rows. It calculates column widths and aligns content
	/// based on the provided column definitions. The table is printed row by row to the console.
	/// </remarks>
	public static void PrintTable (
		IReadOnlyList<ColumnDefinition> columnDefs,
		IReadOnlyList<TaskItem> tasks,
		string separator )
	{
		if ( columnDefs.Count == 0 )
			return;

		(int[] widths, var alignments, var rows) = PrepareTable ( columnDefs, tasks );

		var dividerLength = widths.Sum () + (separator.Length * (widths.Length - 1));

		PrintHeader ( columnDefs, widths, alignments, separator );
		PrintDivider ( dividerLength );
		PrintTaskRows ( rows, widths, alignments, separator );
	}

	/// <summary>
	/// Prints the rows of a table to the console.
	/// </summary>
	/// <param name="rows">The collection of rows, where each row is an array of column values.</param>
	/// <param name="widths">The widths of each column, used for formatting the rows.</param>
	/// <param name="alignments">The alignment of each column (left or right).</param>
	/// <param name="separator">The string used to separate columns in the rows.</param>
	/// <remarks>
	/// This method iterates through the provided rows and prints each row to the console.
	/// Each row is formatted according to the specified column widths and alignments.
	/// </remarks>
	private static void PrintTaskRows (
		IReadOnlyList<string[]> rows,
		int[] widths,
		ColumnAlignment[] alignments,
		string separator )
	{
		foreach ( var row in rows )
		{
			PrintRow ( row, widths, alignments, separator );
		}
	}

	/// <summary>
	/// Prepares the data required to render a table, including column widths, alignments, and rows.
	/// </summary>
	/// <param name="columnDefs">The definitions of the table columns, including headers, alignments, and value selectors.</param>
	/// <param name="tasks">The list of task items to be included in the table rows.</param>
	/// <returns>
	/// A tuple containing:
	/// <list type="bullet">
	/// <item><description>An array of column widths.</description></item>
	/// <item><description>An array of column alignments.</description></item>
	/// <item><description>A list of rows, where each row is an array of string values.</description></item>
	/// </list>
	/// </returns>
	/// <remarks>
	/// This method calculates the maximum width for each column based on the header and the values of the tasks.
	/// It also determines the alignment for each column and constructs the rows of the table.
	/// </remarks>
	private static (int[] Widths, ColumnAlignment[] Alignments, List<string[]> Rows) PrepareTable (
		IReadOnlyList<ColumnDefinition> columnDefs,
		IReadOnlyList<TaskItem> tasks )
	{
		int columnCount = columnDefs.Count;
		var widths = new int[columnCount];
		var alignments = new ColumnAlignment[columnCount];
		var rows = new List<string[]> ( tasks.Count );

		for (int i = 0; i < columnCount; i++)
		{
			widths[i] = columnDefs[i].Header.Length;
			alignments[i] = columnDefs[i].Alignment;
		}

		foreach ( TaskItem task in tasks )
		{
			var row = new string[columnCount];

			for (int i = 0; i < columnCount; i++)
			{
				var value = columnDefs[i].ValueSelector ( task ) ?? string.Empty;
				row[i] = value;
				if ( value.Length > widths[i] ) widths[i] = value.Length;
			}

			rows.Add ( row );
		}

		return (widths, alignments, rows);
	}

	/// <summary>
	/// Prints a single row of a table to the console.
	/// </summary>
	/// <param name="columns">The array of column values to be printed in the row.</param>
	/// <param name="widths">The widths of each column, used for formatting.</param>
	/// <param name="alignments">The alignment of each column (left or right).</param>
	/// <param name="separator">The string used to separate columns in the row.</param>
	/// <param name="color">The optional color to use for the row text. If not specified, the default console color is used.</param>
	/// <remarks>
	/// This method formats and prints the provided column values according to the specified widths and alignments.
	/// If the lengths of <paramref name="columns"/>, <paramref name="widths"/>, and <paramref name="alignments"/> do not match,
	/// an exception is thrown.
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// Thrown if the lengths of <paramref name="columns"/>, <paramref name="widths"/>, and <paramref name="alignments"/> do not match.
	/// </exception>
	public static void PrintRow (
		string[] columns, int[] widths, ColumnAlignment[] alignments, string separator, ConsoleColor? color = null )
	{
		if ( columns.Length != widths.Length || columns.Length != alignments.Length )
			throw new ArgumentException ( "Columns, widths, and alignments must have the same length." );

		for (var i = 0; i < columns.Length; i++)
		{
			if ( i > 0 )
				Print ( separator );

			Print ( alignments[i] == ColumnAlignment.Right
				? columns[i].PadLeft ( widths[i] )
				: columns[i].PadRight ( widths[i] ), color );
		}

		PrintLine ();
	}


	/// <summary>
	/// Prints the header row of a table to the console.
	/// </summary>
	/// <param name="columnDefs">The definitions of the columns, including headers and alignment.</param>
	/// <param name="widths">The widths of each column.</param>
	/// <param name="alignments">The alignment of each column (left or right).</param>
	/// <param name="separator">The string used to separate columns in the header.</param>
	/// <remarks>
	/// This method formats and prints the column headers with the specified alignment and separator.
	/// The header row is displayed in cyan color.
	/// </remarks>
	/// <exception cref="ArgumentException">
	/// Thrown if the lengths of <paramref name="columnDefs"/>, <paramref name="widths"/>, and <paramref name="alignments"/> do not match.
	/// </exception>
	private static void PrintHeader (
		IReadOnlyList<ColumnDefinition> columnDefs,
		int[] widths,
		ColumnAlignment[] alignments,
		string separator )
	{
		PrintRow ( [.. columnDefs.Select ( cd => cd.Header )],
			widths,
			alignments,
			separator,
			ConsoleColor.Cyan );
	}

	/// <summary>
	/// Prints the rows of a task table to the console.
	/// </summary>
	/// <param name="columnDefs">The definitions of the columns, including headers and value selectors.</param>
	/// <param name="tasks">The list of tasks to be displayed in the table.</param>
	/// <param name="widths">The widths of each column.</param>
	/// <param name="alignments">The alignment of each column (left or right).</param>
	/// <param name="separator">The string used to separate columns in the table rows.</param>
	/// <remarks>
	/// This method iterates through the provided tasks, formats their values according to the column definitions,
	/// and prints each row with the specified alignment and separator.
	/// </remarks>
	private static void PrintTaskRows (
		IReadOnlyList<ColumnDefinition> columnDefs,
		IReadOnlyList<TaskItem> tasks,
		int[] widths,
		ColumnAlignment[] alignments,
		string separator )
	{
		foreach ( TaskItem task in tasks )
		{
			PrintRow ( [.. columnDefs.Select ( cd => cd.ValueSelector ( task ) )],
				widths,
				alignments,
				separator );
		}
	}
}