using System.Globalization;

namespace Lexicon.TODO;

public static class InputHelper 
{
	private static string ReadLineOrThrow() =>
		Console.ReadLine() ?? throw new EndOfStreamException("Input ended.");

	public static string ReadNonEmptyString(string prompt)
	{
		while (true)
		{
			Console.Write(prompt);

			var input = ReadLineOrThrow();

			if (!string.IsNullOrWhiteSpace(input))
				return input;

			Console.WriteLine("Input cannot be empty. Please try again.");
		}
	}

	public static bool Confirm(string prompt)
	{
		while (true)
		{
			Console.Write(prompt);

			var input = ReadLineOrThrow().Trim().ToLowerInvariant();

			if (input == "y" || input == "yes")
				return true;
			if (input == "n" || input == "no")
				return false;

			Console.WriteLine("Invalid input. Please enter 'y' or 'n'.");
		}
	}

	public static string? ReadOptionalString(string prompt)
	{
		Console.Write(prompt);

		var input = ReadLineOrThrow();

		return string.IsNullOrWhiteSpace(input) 
			? null 
			: input;
	}

	public static int ReadInt(string prompt)
	{
		while (true)
		{
			Console.Write(prompt);

			var input = ReadLineOrThrow();

			if (int.TryParse(input, out var result))
				return result;

			Console.WriteLine("Invalid integer. Please try again.");
		}
	}

	public static DateTime ReadDateTime(string prompt)
	{
		while (true)
		{
			Console.Write(prompt);

			var input = ReadLineOrThrow();

			if (DateTime.TryParseExact(input, "yyyy-MM-dd", 
				CultureInfo.InvariantCulture, 
				DateTimeStyles.None, 
				out var result))
				return result;

			Console.WriteLine("Invalid date format. Please try again.");
		}
	}
}
