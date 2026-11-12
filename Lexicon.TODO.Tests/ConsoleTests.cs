using Lexicon.TODO.Helpers;
using Lexicon.TODO.Models;

namespace Lexicon.TODO.Tests;

/// <summary>
/// Tests in this collection redirect <see cref="Console.In"/> and <see cref="Console.Out"/>, which are process-wide,
/// so they must not run in parallel with each other.
/// </summary>
[CollectionDefinition ( "Console", DisableParallelization = true )]
public sealed class ConsoleCollection;

/// <summary>
/// Swaps console input and output for one test and restores them on dispose.
/// </summary>
internal sealed class ConsoleScope : IDisposable
{
	private readonly TextReader originalIn = Console.In;
	private readonly TextWriter originalOut = Console.Out;
	private readonly StringWriter output = new ();

	public ConsoleScope ( string input = "" )
	{
		Console.SetIn ( new StringReader ( input ) );
		Console.SetOut ( output );
	}

	public string Output => output.ToString ();

	public string[] Lines => Output.Split ( Environment.NewLine, StringSplitOptions.RemoveEmptyEntries );

	public void Dispose ()
	{
		Console.SetIn ( originalIn );
		Console.SetOut ( originalOut );
	}
}

[Collection ( "Console" )]
public class InputHelperTests
{
	[Fact]
	public void ReadDateTime_BlankInput_ReturnsNull ()
	{
		using var scope = new ConsoleScope ( "\n" );

		Assert.Null ( InputHelper.ReadDateTime ( "> " ) );
	}

	[Fact]
	public void ReadDateTime_TrimsWhitespaceAroundDate ()
	{
		using var scope = new ConsoleScope ( "  2026-01-02  \n" );

		Assert.Equal ( new DateTime ( 2026, 1, 2 ), InputHelper.ReadDateTime ( "> " ) );
	}

	[Fact]
	public void ReadDateTime_InvalidThenValid_RepromptsOnce ()
	{
		using var scope = new ConsoleScope ( "02/01/2026\n2026-01-02\n" );

		var result = InputHelper.ReadDateTime ( "> " );

		Assert.Equal ( new DateTime ( 2026, 1, 2 ), result );
		Assert.Contains ( "Invalid date format", scope.Output );
	}

	[Fact]
	public void ReadRequiredDateTime_BlankThenValid_RequiresADate ()
	{
		using var scope = new ConsoleScope ( "\n2026-01-02\n" );

		var result = InputHelper.ReadRequiredDateTime ( "> " );

		Assert.Equal ( new DateTime ( 2026, 1, 2 ), result );
		Assert.Contains ( "A date is required", scope.Output );
	}

	[Fact]
	public void ReadInt_InvalidThenValid_ReturnsInteger ()
	{
		using var scope = new ConsoleScope ( "abc\n 7 \n" );

		Assert.Equal ( 7, InputHelper.ReadInt ( "> " ) );
		Assert.Contains ( "Invalid integer", scope.Output );
	}

	[Fact]
	public void ReadNonEmptyString_SkipsBlankLines_AndTrims ()
	{
		using var scope = new ConsoleScope ( "   \n  hello  \n" );

		Assert.Equal ( "hello", InputHelper.ReadNonEmptyString ( "> " ) );
	}

	[Fact]
	public void ReadOptionalString_BlankReturnsNull_OtherwiseTrimmed ()
	{
		using var scope = new ConsoleScope ( "   \n  Work \n" );

		Assert.Null ( InputHelper.ReadOptionalString ( "> " ) );
		Assert.Equal ( "Work", InputHelper.ReadOptionalString ( "> " ) );
	}

	[Theory]
	[InlineData ( "y\n", true )]
	[InlineData ( "YES\n", true )]
	[InlineData ( "n\n", false )]
	[InlineData ( "maybe\nno\n", false )]
	public void Confirm_AcceptsYesNoVariants ( string input, bool expected )
	{
		using var scope = new ConsoleScope ( input );

		Assert.Equal ( expected, InputHelper.Confirm ( "> " ) );
	}

	[Fact]
	public void Readers_ThrowEndOfStream_WhenInputEnds ()
	{
		using var scope = new ConsoleScope ( "" );

		Assert.Throws<EndOfStreamException> ( () => InputHelper.ReadInt ( "> " ) );
		Assert.Throws<EndOfStreamException> ( () => InputHelper.WaitForEnter ( "> " ) );
	}
}

[Collection ( "Console" )]
public class ConsoleHelperTests
{
	[Fact]
	public void PrintTasks_Empty_PrintsPlaceholder ()
	{
		using var scope = new ConsoleScope ();

		ConsoleHelper.PrintTasks ( [] );

		Assert.Contains ( "No tasks available.", scope.Output );
	}

	[Fact]
	public void PrintTasks_DividerIsDashes_AndMatchesHeaderWidth ()
	{
		using var scope = new ConsoleScope ();
		TaskItem[] tasks =
		[
			new ( 1, "Short", new DateTime ( 2026, 1, 2 ), TaskState.Pending, "Work" ),
			new ( 12, "A much longer title", new DateTime ( 2026, 1, 3 ), TaskState.Completed, null )
		];

		ConsoleHelper.PrintTasks ( tasks );

		var lines = scope.Lines;
		var header = lines[0];
		var divider = lines[1];

		Assert.StartsWith ( "ID", header );
		Assert.Equal ( header.Length, divider.Length );
		Assert.All ( divider, c => Assert.Equal ( '-', c ) );
		Assert.All ( lines.Skip ( 2 ), row => Assert.Equal ( header.Length, row.Length ) );
		Assert.Contains ( "2026-01-02", lines[2] );
	}

	[Fact]
	public void PrintDivider_RepeatsCharacter ()
	{
		using var scope = new ConsoleScope ();

		ConsoleHelper.PrintDivider ( 5, '=' );

		Assert.Equal ( "=====", scope.Lines[0] );
	}

	[Fact]
	public void Clear_DoesNotThrow_WhenOutputRedirected ()
	{
		using var scope = new ConsoleScope ();

		var exception = Record.Exception ( ConsoleHelper.Clear );

		Assert.Null ( exception );
	}
}

[Collection ( "Console" )]
public class MenuHelperTests
{
	[Fact]
	public void ShowMenu_DividersMatchWidestLine ()
	{
		using var scope = new ConsoleScope ();

		MenuHelper.ShowMenu<MainMenuOption> ( "Main", "A description that is wider than any item", o => o.ToLabel () );

		var lines = scope.Lines;
		var contentWidth = lines.Where ( l => !l.All ( c => c == '-' ) ).Max ( l => l.Length );
		var dividers = lines.Where ( l => l.All ( c => c == '-' ) ).ToArray ();

		Assert.Equal ( 4, dividers.Length );
		Assert.All ( dividers, d => Assert.Equal ( contentWidth, d.Length ) );
		Assert.Contains ( "1. Show Tasks", lines );
		Assert.Contains ( "4. Save and Quit", lines );
	}

	[Fact]
	public void ShowMenu_WithoutDescription_PrintsThreeDividers ()
	{
		using var scope = new ConsoleScope ();

		MenuHelper.ShowMenu<SortMenuOption> ( "Sort", null, o => o.ToLabel () );

		var dividers = scope.Lines.Where ( l => l.All ( c => c == '-' ) ).ToArray ();

		Assert.Equal ( 3, dividers.Length );
		Assert.All ( dividers, d => Assert.Equal ( "2. Sort by Due Date".Length, d.Length ) );
	}
}
