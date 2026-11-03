namespace Lexicon.TODO;

public record TaskItem(
	int Id,
	string Title,
	DateTime DueDate,
	TaskStatus Status,
	string? Project);