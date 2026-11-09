using System.Globalization;

namespace Lexicon.TODO.Helpers;

/// <summary>
/// Renders numbered console menus from the values of an enum.
/// </summary>
public static class MenuHelper
{
	private const ConsoleColor DividerColor = ConsoleColor.DarkGray;

	/// <summary>
	/// Displays a menu based on the values of a specified enum type.
	/// </summary>
	/// <typeparam name="TEnum">The enum type whose values are displayed as menu options.</typeparam>
	/// <param name="header">The header text displayed at the top of the menu.</param>
	/// <param name="description">Optional description text displayed below the header and above the menu options.</param>
	/// <param name="getLabel">A function that provides the display label for each enum value.</param>
	/// <param name="headerColor">Optional color for the header text. If not specified, the default console color is used.</param>
	/// <remarks>
	/// Each menu option is displayed in the format <c>{number}. {label}</c>, where <c>{number}</c> is the numeric
	/// value of the enum and <c>{label}</c> is the text returned by <paramref name="getLabel"/>.
	/// Divider lines are as wide as the widest of the header, the description, and the formatted menu items.
	/// </remarks>
	public static void ShowMenu<TEnum> (
		string header, string? description,
		Func<TEnum, string> getLabel,
		ConsoleColor? headerColor = null )
		where TEnum : struct, Enum
	{
		var menuItems = Enum.GetValues<TEnum> ();
		var width = CalculateMenuWidth ( header, description, menuItems, getLabel );

		ConsoleHelper.PrintDivider ( width, color: DividerColor );
		ConsoleHelper.PrintLine ( header.ToUpperInvariant (), headerColor );

		if ( !string.IsNullOrWhiteSpace ( description ) )
		{
			ConsoleHelper.PrintDivider ( width, color: DividerColor );
			ConsoleHelper.PrintLine ( description, DividerColor );
		}

		ConsoleHelper.PrintDivider ( width, color: DividerColor );

		foreach ( TEnum option in menuItems )
			ConsoleHelper.PrintLine ( FormatMenuItem ( option, getLabel ) );

		ConsoleHelper.PrintDivider ( width, color: DividerColor );
	}

	/// <summary>
	/// Formats a single menu item as <c>{number}. {label}</c>.
	/// </summary>
	private static string FormatMenuItem<TEnum> ( TEnum option, Func<TEnum, string> getLabel )
		where TEnum : struct, Enum =>
		$"{Convert.ToInt32 ( option, CultureInfo.InvariantCulture )}. {getLabel ( option )}";

	/// <summary>
	/// Calculates the divider width: the widest of the header, the description, and the formatted menu items.
	/// </summary>
	/// <remarks>
	/// Measures the same string that <see cref="ShowMenu{TEnum}"/> prints, so the width cannot drift from the output.
	/// An enum with no values yields the header or description width.
	/// </remarks>
	private static int CalculateMenuWidth<TEnum> (
		string header, string? description,
		TEnum[] menuItems,
		Func<TEnum, string> getLabel )
		where TEnum : struct, Enum
	{
		var width = menuItems
			.Select ( option => FormatMenuItem ( option, getLabel ).Length )
			.DefaultIfEmpty ( 0 )
			.Max ();

		width = Math.Max ( width, header.Length );

		if ( !string.IsNullOrWhiteSpace ( description ) )
			width = Math.Max ( width, description.Length );

		return width;
	}
}
