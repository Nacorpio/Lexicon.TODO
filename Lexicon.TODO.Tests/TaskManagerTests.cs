using Lexicon.TODO.Models;
using Lexicon.TODO.Services;

namespace Lexicon.TODO.Tests;

public class TaskManagerTests
{
	private static readonly DateTime Today = new ( 2026, 11, 9 );

	private static TaskManager CreateManager () => new ( [], new FixedTimeProvider ( Today ) );

	[Fact]
	public void AddTask_AssignsSequentialIdsStartingAtOne ()
	{
		var manager = CreateManager ();

		var first = manager.AddTask ( "A", Today );
		var second = manager.AddTask ( "B", Today );

		Assert.Equal ( 1, first.Id );
		Assert.Equal ( 2, second.Id );
	}

	[Fact]
	public void AddTask_AfterDeletingHighestId_ReusesThatId ()
	{
		// Documents the chosen max + 1 policy. Change this test if the policy changes.
		var manager = CreateManager ();
		manager.AddTask ( "A", Today );
		var second = manager.AddTask ( "B", Today );

		manager.DeleteTask ( second.Id );
		var third = manager.AddTask ( "C", Today );

		Assert.Equal ( second.Id, third.Id );
	}

	[Fact]
	public void AddTask_TrimsTitleAndProject_AndStoresBlankProjectAsNull ()
	{
		var manager = CreateManager ();

		var trimmed = manager.AddTask ( "  Title  ", Today, "  Work  " );
		var blank = manager.AddTask ( "Other", Today, "   " );

		Assert.Equal ( "Title", trimmed.Title );
		Assert.Equal ( "Work", trimmed.Project );
		Assert.Null ( blank.Project );
	}

	[Theory]
	[InlineData ( "" )]
	[InlineData ( "   " )]
	public void AddTask_BlankTitle_Throws ( string title )
	{
		var manager = CreateManager ();

		Assert.Throws<ArgumentException> ( () => manager.AddTask ( title, Today ) );
	}

	[Fact]
	public void UpdateTask_NullArguments_KeepCurrentValues ()
	{
		var manager = CreateManager ();
		var task = manager.AddTask ( "Title", Today, "Work" );

		var updated = manager.UpdateTask ( task.Id, dueDate: Today.AddDays ( 1 ) );
		var result = manager.FindById ( task.Id );

		Assert.True ( updated );
		Assert.NotNull ( result );
		Assert.Equal ( "Title", result.Title );
		Assert.Equal ( "Work", result.Project );
		Assert.Equal ( TaskState.Pending, result.State );
		Assert.Equal ( Today.AddDays ( 1 ), result.DueDate );
	}

	[Fact]
	public void UpdateTask_WhitespaceTitle_Throws ()
	{
		var manager = CreateManager ();
		var task = manager.AddTask ( "Title", Today );

		Assert.Throws<ArgumentException> ( () => manager.UpdateTask ( task.Id, title: "   " ) );
	}

	[Fact]
	public void UpdateTask_UnknownId_ReturnsFalse ()
	{
		var manager = CreateManager ();

		Assert.False ( manager.UpdateTask ( 42, title: "X" ) );
	}

	[Fact]
	public void CompleteTask_SetsStateAndUpdatesCounts ()
	{
		var manager = CreateManager ();
		var task = manager.AddTask ( "Title", Today );
		manager.AddTask ( "Other", Today );

		var completed = manager.CompleteTask ( task.Id );

		Assert.True ( completed );
		Assert.Equal ( TaskState.Completed, manager.FindById ( task.Id )!.State );
		Assert.Equal ( 1, manager.CompletedCount );
		Assert.Equal ( 1, manager.PendingCount );
		Assert.Single ( manager.CompletedTasks );
		Assert.Single ( manager.PendingTasks );
	}

	[Fact]
	public void DeleteTask_RemovesTask_AndReturnsFalseForUnknownId ()
	{
		var manager = CreateManager ();
		var task = manager.AddTask ( "Title", Today );

		Assert.True ( manager.DeleteTask ( task.Id ) );
		Assert.Null ( manager.FindById ( task.Id ) );
		Assert.False ( manager.DeleteTask ( task.Id ) );
	}

	[Fact]
	public void OverdueTasks_UsesInjectedClock_AndIgnoresCompletedTasks ()
	{
		var manager = CreateManager ();
		var overdue = manager.AddTask ( "Late", Today.AddDays ( -1 ) );
		manager.AddTask ( "Due today", Today );
		manager.AddTask ( "Future", Today.AddDays ( 1 ) );
		var completedLate = manager.AddTask ( "Done late", Today.AddDays ( -5 ) );
		manager.CompleteTask ( completedLate.Id );

		var result = manager.OverdueTasks;

		Assert.Single ( result );
		Assert.Equal ( overdue.Id, result[0].Id );
		Assert.Equal ( 1, manager.OverdueCount );
	}

	[Fact]
	public void GetTasksSortedByDueDate_IsAscending ()
	{
		var manager = CreateManager ();
		manager.AddTask ( "C", Today.AddDays ( 2 ) );
		manager.AddTask ( "A", Today );
		manager.AddTask ( "B", Today.AddDays ( 1 ) );

		var titles = manager.GetTasksSortedByDueDate ().Select ( t => t.Title );

		Assert.Equal ( [ "A", "B", "C" ], titles );
	}

	[Fact]
	public void GetTasksSortedByProject_GroupsCaseInsensitively_ThenDueDate_NullsLast ()
	{
		var manager = CreateManager ();
		manager.AddTask ( "no project", Today );
		manager.AddTask ( "work late", Today.AddDays ( 1 ), "work" );
		manager.AddTask ( "Home", Today, "Home" );
		manager.AddTask ( "Work early", Today, "Work" );

		var titles = manager.GetTasksSortedByProject ().Select ( t => t.Title );

		Assert.Equal ( [ "Home", "Work early", "work late", "no project" ], titles );
	}

	[Fact]
	public void IsOverdueOn_ComparesDateOnly_AndIgnoresState ()
	{
		var task = new TaskItem ( 1, "T", new DateTime ( 2026, 11, 8, 23, 0, 0 ), TaskState.Completed, null );

		Assert.True ( task.IsOverdueOn ( new DateTime ( 2026, 11, 9, 0, 0, 1 ) ) );
		Assert.False ( task.IsOverdueOn ( new DateTime ( 2026, 11, 8 ) ) );
	}
}
