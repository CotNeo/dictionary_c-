namespace CEFRWordFetcher.Core.Models;

/// <summary>
/// Represents detailed information about a word from WordsAPI.
/// </summary>
public class WordInfo
{
    /// <summary>
    /// The word itself
    /// </summary>
    public string Word { get; set; } = string.Empty;
    
    /// <summary>
    /// Phonetic pronunciation
    /// </summary>
    public string? Pronunciation { get; set; }
    
    /// <summary>
    /// Audio pronunciation URL
    /// </summary>
    public string? Audio { get; set; }
    
    /// <summary>
    /// List of definitions for different parts of speech
    /// </summary>
    public List<WordDefinition> Definitions { get; set; } = new();
    
    /// <summary>
    /// List of examples showing word usage
    /// </summary>
    public List<string> Examples { get; set; } = new();
    
    /// <summary>
    /// List of synonyms
    /// </summary>
    public List<string> Synonyms { get; set; } = new();
    
    /// <summary>
    /// List of antonyms
    /// </summary>
    public List<string> Antonyms { get; set; } = new();
    
    /// <summary>
    /// Rhyming words
    /// </summary>
    public List<string> Rhymes { get; set; } = new();
    
    /// <summary>
    /// Syllables information
    /// </summary>
    public Syllables? Syllables { get; set; }
    
    /// <summary>
    /// Frequency information
    /// </summary>
    public Frequency? Frequency { get; set; }
}

/// <summary>
/// Represents a definition of a word
/// </summary>
public class WordDefinition
{
    /// <summary>
    /// Part of speech
    /// </summary>
    public string PartOfSpeech { get; set; } = string.Empty;
    
    /// <summary>
    /// The definition text
    /// </summary>
    public string Definition { get; set; } = string.Empty;
    
    /// <summary>
    /// Synonyms for this specific definition
    /// </summary>
    public List<string> Synonyms { get; set; } = new();
    
    /// <summary>
    /// Antonyms for this specific definition
    /// </summary>
    public List<string> Antonyms { get; set; } = new();
    
    /// <summary>
    /// Examples for this specific definition
    /// </summary>
    public List<string> Examples { get; set; } = new();
}

/// <summary>
/// Represents syllable information
/// </summary>
public class Syllables
{
    /// <summary>
    /// Total number of syllables
    /// </summary>
    public int Count { get; set; }
    
    /// <summary>
    /// List of syllables
    /// </summary>
    public List<string> List { get; set; } = new();
}

/// <summary>
/// Represents frequency information
/// </summary>
public class Frequency
{
    /// <summary>
    /// Zipf frequency score
    /// </summary>
    public double Zipf { get; set; }
    
    /// <summary>
    /// Per million frequency
    /// </summary>
    public double PerMillion { get; set; }
    
    /// <summary>
    /// Diversity score
    /// </summary>
    public double Diversity { get; set; }
}
