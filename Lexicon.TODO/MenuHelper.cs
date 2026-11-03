using System;
using System.Collections.Generic;
using System.Text;

namespace Lexicon.TODO;

public static class MenuHelper
{
	public static void Show<T>(Func<T, string> getLabel) where T : struct, Enum
	{
		foreach (var option in Enum.GetValues<T>())
			Console.WriteLine($"{Convert.ToInt32(option)}. {getLabel(option)}");
	}
}
