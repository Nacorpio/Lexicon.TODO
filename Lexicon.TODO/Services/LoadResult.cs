using Lexicon.TODO.Models;

namespace Lexicon.TODO.Services;

public record LoadResult(List<TaskItem> Tasks, string? Error = null)
{
	public bool CanSave => Error is null;
}