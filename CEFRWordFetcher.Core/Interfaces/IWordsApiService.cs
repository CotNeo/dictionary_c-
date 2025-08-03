using CEFRWordFetcher.Core.Models;

namespace CEFRWordFetcher.Core.Interfaces;

/// <summary>
/// Interface for WordsAPI service operations.
/// </summary>
public interface IWordsApiService
{
    /// <summary>
    /// Fetches detailed information about a word from WordsAPI.
    /// </summary>
    /// <param name="word">The word to fetch information for</param>
    /// <returns>Detailed word information or null if not found</returns>
    Task<WordInfo?> GetWordInfoAsync(string word);
    
    /// <summary>
    /// Fetches detailed information for multiple words.
    /// </summary>
    /// <param name="words">List of words to fetch information for</param>
    /// <returns>Dictionary mapping words to their information</returns>
    Task<Dictionary<string, WordInfo?>> GetWordsInfoAsync(IEnumerable<string> words);
}
