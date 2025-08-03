using CEFRWordFetcher.Core.Interfaces;
using CEFRWordFetcher.Core.Models;

namespace CEFRWordFetcher.Core.Services;

/// <summary>
/// Service for randomly selecting words from a CEFR dataset.
/// </summary>
public class RandomWordSelector : IWordSelector
{
    private readonly Random _random;
    
    /// <summary>
    /// Initializes a new instance of the RandomWordSelector class.
    /// </summary>
    public RandomWordSelector()
    {
        _random = new Random();
    }
    
    /// <summary>
    /// Initializes a new instance of the RandomWordSelector class with a specific seed.
    /// </summary>
    /// <param name="seed">Random seed for reproducible results</param>
    public RandomWordSelector(int seed)
    {
        _random = new Random(seed);
    }
    
    /// <summary>
    /// Selects a specified number of words randomly from the given collection.
    /// </summary>
    /// <param name="words">Collection of CEFR words to select from</param>
    /// <param name="count">Number of words to select</param>
    /// <param name="level">Optional CEFR level filter (A1, A2, B1, B2, C1, C2)</param>
    /// <returns>Selected words</returns>
    public IEnumerable<CEFRWord> SelectWords(IEnumerable<CEFRWord> words, int count, string? level = null)
    {
        Console.WriteLine($"[RandomWordSelector] Starting word selection process");
        Console.WriteLine($"[RandomWordSelector] Total words available: {words.Count()}");
        Console.WriteLine($"[RandomWordSelector] Requested count: {count}");
        Console.WriteLine($"[RandomWordSelector] Level filter: {level ?? "None"}");
        
        // Filter by level if specified
        var filteredWords = words;
        if (!string.IsNullOrEmpty(level))
        {
            filteredWords = words.Where(w => string.Equals(w.Level, level, StringComparison.OrdinalIgnoreCase));
            Console.WriteLine($"[RandomWordSelector] Words after level filtering: {filteredWords.Count()}");
        }
        
        // Convert to list for random selection
        var wordList = filteredWords.ToList();
        
        if (wordList.Count == 0)
        {
            Console.WriteLine($"[RandomWordSelector] No words found matching criteria");
            return Enumerable.Empty<CEFRWord>();
        }
        
        // Adjust count if we have fewer words than requested
        var actualCount = Math.Min(count, wordList.Count);
        Console.WriteLine($"[RandomWordSelector] Actual count to select: {actualCount}");
        
        // Perform random selection
        var selectedWords = new List<CEFRWord>();
        var availableIndices = Enumerable.Range(0, wordList.Count).ToList();
        
        for (int i = 0; i < actualCount; i++)
        {
            if (availableIndices.Count == 0) break;
            
            var randomIndex = _random.Next(availableIndices.Count);
            var wordIndex = availableIndices[randomIndex];
            selectedWords.Add(wordList[wordIndex]);
            availableIndices.RemoveAt(randomIndex);
        }
        
        Console.WriteLine($"[RandomWordSelector] Successfully selected {selectedWords.Count} words");
        Console.WriteLine($"[RandomWordSelector] Selected words: {string.Join(", ", selectedWords.Select(w => w.Word))}");
        
        return selectedWords;
    }
}
