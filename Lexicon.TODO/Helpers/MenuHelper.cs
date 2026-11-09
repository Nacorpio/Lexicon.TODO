namespace Lexicon.TODO.Helpers;

public static class MenuHelper
{
	/// <summary>
	/// Displays a menu based on the values of a specified enum type.
	/// </summary>
	/// <typeparam name="TEnum">The enum type whose values are displayed as menu options.</typeparam>
	/// <param name="header">The header text displayed at the top of the menu.</param>
	/// <param name="description">Optional description text displayed below the header and above the menu options.</param>
	/// <param name="getLabel">A function that provides the display label for each enum value.</param>
	/// <param name="headerColor">Optional color for the header text. If not specified, the default console color is used.</param>
	/// <remarks>
	/// Each menu option is displayed in the format: `{number}. {label}`, where `{number}` is the numeric value of the enum and `{label}` is the text returned by <paramref name="getLabel"/>.
	/// </remarks>
	public static void ShowMenu<TEnum> (
		string header, string? description,
		Func<TEnum, string> getLabel,
		ConsoleColor? headerColor = null )
		where TEnum : struct, Enum
	{
		var menuItems = Enum.GetValues<TEnum> ();

		ConsoleHelper.PrintMenuDivider ( header, description, getLabel, ConsoleColor.DarkGray );
		ConsoleHelper.PrintLine ( header.ToUpperInvariant (), headerColor );

		// If a description is provided, print it in dark gray color below the header.
		if ( !string.IsNullOrWhiteSpace ( description ) )
		{
			ConsoleHelper.PrintMenuDivider ( header, description, getLabel, ConsoleColor.DarkGray );
			ConsoleHelper.PrintLine ( description, ConsoleColor.DarkGray );
		}

		ConsoleHelper.PrintMenuDivider ( header, description, getLabel, ConsoleColor.DarkGray);

		// Print each menu option with its corresponding numeric value and label.
		foreach ( TEnum option in menuItems )
			ConsoleHelper.PrintLine ( $"{Convert.ToInt32 ( option )}. {getLabel ( option )}" );

		ConsoleHelper.PrintMenuDivider ( header, description, getLabel, ConsoleColor.DarkGray );
	}
}