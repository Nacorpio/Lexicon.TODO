namespace Lexicon.TODO;

public static class InputHelper 
{
	public static string ReadNonEmptyString(string prompt)
	{
		while (true)
		{
			Console.Write(prompt);

			var input = Console.ReadLine();

			if (!string.IsNullOrWhiteSpace(input))
				return input;

			Console.WriteLine("Input cannot be empty. Please try again.");
		}
	}

	public static string? ReadOptionalString(string prompt)
	{
		Console.Write(prompt);

		var input = Console.ReadLine();

		return string.IsNullOrWhiteSpace(input) 
			? null 
			: input;
	}

	public static int ReadInt(string prompt)
	{
		while (true)
		{
			Console.Write(prompt);

			var input = Console.ReadLine();

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

			var input = Console.ReadLine();

			if (DateTime.TryParse(input, out var result))
				return result;

			Console.WriteLine("Invalid date format. Please try again.");
		}
	}
}
