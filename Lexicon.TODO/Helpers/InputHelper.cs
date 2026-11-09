using System.Globalization;

namespace Lexicon.TODO.Helpers;

public static class InputHelper
{
	private static string ReadLineOrThrow () =>
		Console.ReadLine () ?? throw new EndOfStreamException ( "Input ended." );

	public static string ReadNonEmptyString ( string prompt )
	{
		while (true)
		{
			Console.Write ( prompt );

			var input = ReadLineOrThrow ();

			if ( !string.IsNullOrWhiteSpace ( input ) )
				return input;

			Console.WriteLine ( "Input cannot be empty. Please try again." );
		}
	}

	public static bool Confirm ( string prompt )
	{
		while (true)
		{
			Console.Write ( prompt );

			var input = ReadLineOrThrow ().Trim ().ToLowerInvariant ();

			switch (input)
			{
				case "y":
				case "yes":
					return true;

				case "n":
				case "no":
					return false;

				default:
					Console.WriteLine ( "Invalid input. Please enter 'y' or 'n'." );
					break;
			}
		}
	}

	public static string? ReadOptionalString ( string prompt )
	{
		Console.Write ( prompt );

		var input = ReadLineOrThrow ();

		return string.IsNullOrWhiteSpace ( input )
			? null
			: input;
	}

	public static int ReadInt ( string prompt )
	{
		while (true)
		{
			Console.Write ( prompt );

			var input = ReadLineOrThrow ();

			if ( int.TryParse ( input, out var result ) )
				return result;

			Console.WriteLine ( "Invalid integer. Please try again." );
		}
	}

	public static DateTime ReadDateTime ( string prompt )
	{
		while (true)
		{
			Console.Write ( prompt );

			var input = ReadLineOrThrow ();

			if ( string.IsNullOrWhiteSpace ( input ) )
				return DateTime.MaxValue;

			if ( DateTime.TryParseExact ( input, "yyyy-MM-dd",
				    CultureInfo.InvariantCulture,
				    DateTimeStyles.None,
				    out DateTime result ) )
				return result;

			Console.WriteLine ( "Invalid date format. Please try again." );
		}
	}
}