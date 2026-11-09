using System.Text.Json;
using System.Text.Json.Serialization;
using Lexicon.TODO.Models;

namespace Lexicon.TODO.Services;

/// <summary>
/// Provides functionality for storing and retrieving tasks in a JSON file.
/// </summary>
/// <remarks>
/// This class handles the serialization and deserialization of tasks to and from a JSON file.
/// It ensures data integrity by writing to a temporary file before replacing the original file.
/// </remarks>
public class TaskStorage
{
	/// <summary>
	/// Provides configuration options for JSON serialization and deserialization.
	/// </summary>
	/// <remarks>
	/// This field is used to configure JSON serialization settings, such as enabling indented formatting
	/// and adding custom converters like <see cref="JsonStringEnumConverter"/> for handling enums as strings.
	/// </remarks>
	private static readonly JsonSerializerOptions JsonOptions = new ()
	{
		WriteIndented = true,
		Converters = { new JsonStringEnumConverter () }
	};

	/// <summary>
	/// Initializes a new instance of the <see cref="TaskStorage"/> class with the specified file path.
	/// </summary>
	/// <param name="filePath">The path to the file where tasks will be stored.</param>
	/// <exception cref="ArgumentException">
	/// Thrown if <paramref name="filePath"/> is <see langword="null"/>, empty, or consists only of white-space characters.
	/// </exception>
	public TaskStorage ( string filePath )
	{
		ArgumentException.ThrowIfNullOrWhiteSpace ( filePath );

		FilePath = filePath;
	}

	/// <summary>
	/// Gets the file path where tasks are stored.
	/// </summary>
	/// <remarks>
	/// This property holds the path to the file used for saving and loading tasks.
	/// It is initialized through the constructor and cannot be modified afterward.
	/// </remarks>
	public string FilePath { get; }

	/// <summary>
	/// Saves the specified list of tasks to the file in JSON format.
	/// </summary>
	/// <param name="tasks">The list of tasks to save.</param>
	/// <remarks>
	/// The tasks are serialized as indented JSON. If the directory for the file does not exist, it is created.
	/// To ensure data integrity, the method writes to a temporary file first and then replaces the original file.
	/// </remarks>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="tasks"/> is <see langword="null"/>.</exception>
	/// <exception cref="IOException">Thrown if an I/O error occurs during the save operation.</exception>
	public void Save ( IReadOnlyList<TaskItem> tasks )
	{
		var directory = Path.GetDirectoryName ( FilePath );

		if ( !string.IsNullOrEmpty ( directory ) )
			Directory.CreateDirectory ( directory );

		var json = JsonSerializer.Serialize ( tasks, JsonOptions );

		// Write to a temp file first, then replace the real one, so a crash
		// halfway through the write cannot destroy the existing data.
		var tempPath = FilePath + ".tmp";

		File.WriteAllText ( tempPath, json );
		File.Move ( tempPath, FilePath, overwrite: true );
	}

	/// <summary>
	/// Loads the list of tasks from the JSON file specified by <see cref="FilePath"/>.
	/// </summary>
	/// <returns>
	/// A <see cref="List{T}"/> of <see cref="TaskItem"/> objects representing the tasks.
	/// If the file does not exist or is empty, an empty list is returned.
	/// </returns>
	/// <remarks>
	/// This method reads the contents of the JSON file, deserializes it into a list of <see cref="TaskItem"/> objects,
	/// and returns the result. If the file is missing, it safely returns an empty list to avoid exceptions.
	/// </remarks>
	/// <exception cref="JsonException">
	/// Thrown if the JSON content in the file is invalid or cannot be deserialized into a list of <see cref="TaskItem"/>.
	/// </exception>
	public List<TaskItem> Load ()
	{
		if ( !File.Exists ( FilePath ) )
			return [];

		var json = File.ReadAllText ( FilePath );

		return JsonSerializer.Deserialize<List<TaskItem>> ( json, JsonOptions ) ?? [];
	}
}