using System;
using System.Collections.Generic;
using System.Text;

namespace Lexicon.TODO.Helpers;

public record ColumnDefinition(string Header, ColumnAlignment Alignment, Func<TaskItem, string> ValueSelector);

public enum ColumnAlignment { Left, Right }

public static partial class ConsoleHelper
{
	private static readonly IReadOnlyList<ColumnDefinition> DefaultTaskColumns =
	[
		new ColumnDefinition("ID", ColumnAlignment.Right, static task => task.Id.ToString()),
		new ColumnDefinition("Title", ColumnAlignment.Left, static task => task.Title),
		new ColumnDefinition("Due Date", ColumnAlignment.Left, static task => task.DueDate.ToString("yyyy-MM-dd")),
		new ColumnDefinition("Project", ColumnAlignment.Left, static task => task.Project ?? string.Empty),
		new ColumnDefinition("State", ColumnAlignment.Left, static task => task.State.ToString())
	];

	public static void Print(string message, ConsoleColor? color = null)
	{
		if (color.HasValue)
			Console.ForegroundColor = color.Value;

		Console.Write(message);
		Console.ResetColor();
	}

	public static void PrintLine(string message, ConsoleColor? color = null)
	{
		if (color.HasValue)
			Console.ForegroundColor = color.Value;

		Console.WriteLine(message);
		Console.ResetColor();
	}

	public static void PrintLine()
	{
		Console.WriteLine();
	}

	public static void PrintTasks(IReadOnlyList<TaskItem> tasks)
	{
		if (tasks.Count == 0)
		{
			ConsoleHelper.PrintLine("No tasks available.");
			return;
		}

		PrintTable(DefaultTaskColumns, tasks, " | ");
	}
}
