using System.Globalization;

namespace Lexicon.TODO.Helpers;

/// <summary>
/// Provides utility methods for reading and validating user input from the console.
/// </summary>
/// <remarks>
/// This static class contains methods to handle various types of user input, such as strings, integers, 
/// and dates, with built-in validation and error handling. It is designed to simplify the process of 
/// obtaining and validating input in console applications.
/// </remarks>
public static class InputHelper
{
	/// <summary>
	/// Reads a line of input from the console or throws an exception if the input stream ends unexpectedly.
	/// </summary>
	/// <returns>The string entered by the user.</returns>
	/// <exception cref="EndOfStreamException">Thrown if the input stream ends unexpectedly.</exception>
	/// <remarks>
	/// This method ensures that a valid string is returned. If the input stream is closed or no input is available, 
	/// an <see cref="EndOfStreamException"/> is thrown.
	/// </remarks>
	private static string ReadLineOrThrow () =>
		Console.ReadLine () ?? throw new EndOfStreamException ( "Input ended." );


	/// <summary>
	/// Reads a non-empty string input from the console after displaying a prompt.
	/// </summary>
	/// <param name="prompt">The message to display to the user before reading input.</param>
	/// <returns>The non-empty string entered by the user.</returns>
	/// <remarks>
	/// The method repeatedly prompts the user until a non-empty, non-whitespace input is provided.
	/// </remarks>
	/// <exception cref="EndOfStreamException">Thrown if the input stream ends unexpectedly.</exception>
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

	/// <summary>
	/// Prompts the user with a message and waits for a confirmation input.
	/// </summary>
	/// <param name="prompt">The message to display to the user before reading input.</param>
	/// <returns>
	/// <see langword="true"/> if the user confirms with "y" or "yes", 
	/// <see langword="false"/> if the user declines with "n" or "no".
	/// </returns>
	/// <remarks>
	/// The method repeatedly prompts the user until a valid input ("y", "yes", "n", or "no") is provided.
	/// </remarks>
	/// <exception cref="EndOfStreamException">Thrown if the input stream ends unexpectedly.</exception>
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

	/// <summary>
	/// Reads an optional string input from the console after displaying a prompt.
	/// </summary>
	/// <param name="prompt">The message to display to the user before reading input.</param>
	/// <returns>
	/// The string entered by the user, or <see langword="null"/> if the input is empty or consists only of whitespace.
	/// </returns>
	/// <remarks>
	/// This method allows the user to provide an optional input. If the input is empty or whitespace, 
	/// it returns <see langword="null"/>. Otherwise, it returns the trimmed input string.
	/// </remarks>
	/// <exception cref="EndOfStreamException">Thrown if the input stream ends unexpectedly.</exception>
	public static string? ReadOptionalString ( string prompt )
	{
		Console.Write ( prompt );

		var input = ReadLineOrThrow ();

		return string.IsNullOrWhiteSpace ( input )
			? null
			: input;
	}

	/// <summary>
	/// Reads an integer value from the console after displaying a prompt.
	/// </summary>
	/// <param name="prompt">The message to display to the user before reading input.</param>
	/// <returns>The integer value entered by the user.</returns>
	/// <remarks>
	/// If the user enters an invalid integer, they will be prompted to try again until a valid integer is provided.
	/// </remarks>
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

	/// <summary>
	/// Reads a <see cref="DateTime"/> value from the console input, prompting the user with the specified message.
	/// </summary>
	/// <param name="prompt">The message displayed to the user to request input.</param>
	/// <returns>
	/// A <see cref="DateTime"/> value parsed from the user's input if valid, or <see cref="DateTime.MaxValue"/> 
	/// if the input is empty or whitespace.
	/// </returns>
	/// <remarks>
	/// The method expects the input to be in the format "yyyy-MM-dd". If the input is invalid, the user will be
	/// prompted to try again until a valid date is provided.
	/// </remarks>
	/// <exception cref="EndOfStreamException">Thrown if the input stream ends unexpectedly.</exception>
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