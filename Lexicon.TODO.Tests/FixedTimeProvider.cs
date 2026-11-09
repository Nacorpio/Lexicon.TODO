namespace Lexicon.TODO.Tests;

/// <summary>
/// A <see cref="TimeProvider"/> pinned to one instant in UTC, so "today" is deterministic in tests.
/// </summary>
internal sealed class FixedTimeProvider ( DateTime utcNow ) : TimeProvider
{
	public override DateTimeOffset GetUtcNow () => new ( DateTime.SpecifyKind ( utcNow, DateTimeKind.Utc ) );

	public override TimeZoneInfo LocalTimeZone => TimeZoneInfo.Utc;
}
