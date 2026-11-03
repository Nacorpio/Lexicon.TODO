namespace Lexicon.TODO;

public record TaskItem(
	int Id,
	string Title,
	DateTime DueDate,
	TaskState State,
	string? Project);