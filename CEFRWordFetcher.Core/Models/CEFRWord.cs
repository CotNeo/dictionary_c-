namespace CEFRWordFetcher.Core.Models;

/// <summary>
/// Represents a word with its CEFR (Common European Framework of Reference) level information.
/// </summary>
public class CEFRWord
{
    /// <summary>
    /// The word itself
    /// </summary>
    public string Word { get; set; } = string.Empty;
    
    /// <summary>
    /// The CEFR level (A1, A2, B1, B2, C1, C2)
    /// </summary>
    public string Level { get; set; } = string.Empty;
    
    /// <summary>
    /// Part of speech (noun, verb, adjective, etc.)
    /// </summary>
    public string PartOfSpeech { get; set; } = string.Empty;
    
    /// <summary>
    /// Frequency score or rank
    /// </summary>
    public double? Frequency { get; set; }
    
    /// <summary>
    /// Additional metadata about the word
    /// </summary>
    public Dictionary<string, string> Metadata { get; set; } = new();
}
