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
	/// <param name="getLabel">A function that returns the display label for each enum value.</param>
	public static void Show<T>(Func<T, string> getLabel) where T : struct, Enum
	{
		foreach (var option in Enum.GetValues<T>())
			Console.WriteLine($"{Convert.ToInt32(option)}. {getLabel(option)}");
	}
}
