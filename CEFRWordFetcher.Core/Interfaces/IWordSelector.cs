using CEFRWordFetcher.Core.Models;

namespace CEFRWordFetcher.Core.Interfaces;

/// <summary>
/// Interface for word selection strategies.
/// </summary>
public interface IWordSelector
{
    /// <summary>
    /// Selects a specified number of words from the given collection.
    /// </summary>
    /// <param name="words">Collection of CEFR words to select from</param>
    /// <param name="count">Number of words to select</param>
    /// <param name="level">Optional CEFR level filter (A1, A2, B1, B2, C1, C2)</param>
    /// <returns>Selected words</returns>
    IEnumerable<CEFRWord> SelectWords(IEnumerable<CEFRWord> words, int count, string? level = null);
}
