using System;
using System.Collections.Generic;
using System.Text;

namespace Lexicon.TODO;

public static class MenuHelper
{
	/// <summary>
	/// Writes each value of the enum type to the console as its numeric value followed by a label.
	/// </summary>
	/// <remarks>Output format: `{number}. {label}`.</remarks>
	/// <typeparam name="T">The enum type whose values are displayed.</typeparam>
	/// <param name="header">The header text to display above the menu options.</param>
	/// <param name="getLabel">A function that returns the display label for each enum value.</param>
	/// <param name="headerColor">The color to use for the header text.</param>
	public static void Show<T>(string header, Func<T, string> getLabel, ConsoleColor? headerColor = null) where T : struct, Enum
	{
		var menuItems = Enum.GetValues<T>();
		var menuItemCount = menuItems.Length;

		int dividerLength = CalculateDividerLength(header, getLabel);
		var divider = new string('-', dividerLength);

		ConsoleHelper.PrintLine(header.ToUpperInvariant(), headerColor);
		ConsoleHelper.PrintLine(divider);

		foreach (var option in menuItems)
			ConsoleHelper.PrintLine($"{Convert.ToInt32(option)}. {getLabel(option)}");

		ConsoleHelper.PrintLine(divider);
	}

	/// <summary>
	/// Calculates the divider width needed to fit the header text and the widest formatted enum menu item label.
	/// </summary>
	/// <remarks>Menu item length is calculated from the enum numeric value text, a 3-character separator, and the
	/// label length.</remarks>
	/// <typeparam name="T">The enum type used to generate menu items.</typeparam>
	/// <param name="header">The header text to include in the width calculation.</param>
	/// <param name="getLabel">A delegate that returns the display label for an enum value.</param>
	/// <returns>The greater of the header length and the maximum formatted menu item length.</returns>
	private static int CalculateDividerLength<T>(string header, Func<T, string> getLabel) where T : struct, Enum
	{
		var menuItems = Enum.GetValues<T>();
		var menuItemCount = menuItems.Length;
		int dividerLength = Math.Max(header.Length, menuItems.Max(option => (Convert.ToInt32(option).ToString().Length + 3 + getLabel(option).Length)));

		return dividerLength;
	}
}
