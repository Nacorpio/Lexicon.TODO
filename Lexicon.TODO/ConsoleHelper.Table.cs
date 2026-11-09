using System;
using System.Collections.Generic;
using System.Text;

namespace Lexicon.TODO
{
	public static partial class ConsoleHelper
	{
		public static void PrintTable(
			IReadOnlyList<ColumnDefinition> columnDefs,
			IReadOnlyList<TaskItem> tasks,
			string separator)
		{
			if (columnDefs.Count == 0) 
				return;

			var (widths, alignments, rows) = PrepareTable(columnDefs, tasks);

			PrintHeader(columnDefs, widths, alignments, separator);
			PrintTaskRows(rows, widths, alignments, separator);
		}

		private static void PrintTaskRows(
			IReadOnlyList<string[]> rows,
			int[] widths,
			ColumnAlignment[] alignments,
			string separator)
		{
			foreach (var row in rows)
			{
				PrintRow(row, widths, alignments, separator);
			}
		}

		private static (int[] Widths, ColumnAlignment[] Alignments, List<string[]> Rows) PrepareTable(
			IReadOnlyList<ColumnDefinition> columnDefs,
			IReadOnlyList<TaskItem> tasks)
		{
			int columnCount = columnDefs.Count;
			var widths = new int[columnCount];
			var alignments = new ColumnAlignment[columnCount];
			var rows = new List<string[]>(tasks.Count);

			for (int i = 0; i < columnCount; i++)
			{
				widths[i] = columnDefs[i].Header.Length;
				alignments[i] = columnDefs[i].Alignment;
			}

			foreach (var task in tasks)
			{
				var row = new string[columnCount];

				for (int i = 0; i < columnCount; i++)
				{
					var value = columnDefs[i].ValueSelector(task) ?? string.Empty;
					row[i] = value;
					if (value.Length > widths[i]) widths[i] = value.Length;
				}

				rows.Add(row);
			}

			return (widths, alignments, rows);
		}

		private static void PrintHeader(
			IReadOnlyList<ColumnDefinition> columnDefs,
			int[] widths,
			ColumnAlignment[] alignments,
			string separator)
		{
			PrintRow([.. columnDefs.Select(cd => cd.Header)], 
				widths, 
				alignments,
				separator, 
				ConsoleColor.Cyan);
		}

		private static void PrintTaskRows(
			IReadOnlyList<ColumnDefinition> columnDefs,
			IReadOnlyList<TaskItem> tasks,
			int[] widths,
			ColumnAlignment[] alignments,
			string separator)
		{
			foreach (var task in tasks)
			{
				PrintRow([.. columnDefs.Select(cd => cd.ValueSelector(task))], 
					widths, 
					alignments, 
					separator);
			}
		}
	}
}
