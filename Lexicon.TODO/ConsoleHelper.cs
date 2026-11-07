using System;
using System.Collections.Generic;
using System.Text;

namespace Lexicon.TODO;

public static class ConsoleHelper 
{
	public static void PrintTasks(IReadOnlyList<TaskItem> tasks)
	{
		if (tasks.Count == 0)
		{
			Console.WriteLine("No tasks available.");
			return;
		}

		Console.WriteLine("Tasks:");

		foreach (var task in tasks)
		{
			Console.WriteLine($"ID: {task.Id}, Title: {task.Title}, Due: {task.DueDate:yyyy-MM-dd}, State: {task.State}, Project: {task.Project}");
		}
	}

	public static void PrintTask(TaskItem task)
	{
		Console.WriteLine($"ID: {task.Id}, Title: {task.Title}, Due: {task.DueDate:yyyy-MM-dd}, State: {task.State}, Project: {task.Project}");
	}
}
