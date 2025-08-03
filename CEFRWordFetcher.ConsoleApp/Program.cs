using CEFRWordFetcher.Api;
using CEFRWordFetcher.Core.Interfaces;
using CEFRWordFetcher.Core.Models;
using CEFRWordFetcher.Core.Services;
using CEFRWordFetcher.Infrastructure;
using Newtonsoft.Json;

namespace CEFRWordFetcher.ConsoleApp;

/// <summary>
/// Main application class that orchestrates the CEFR word fetching process.
/// </summary>
class Program
{
    private static readonly string DataPath = "data/cefr_dataset.csv";
    private static readonly string OutputPath = "output/output.json";
    
    /// <summary>
    /// Main entry point of the application.
    /// </summary>
    /// <param name="args">Command line arguments</param>
    static async Task Main(string[] args)
    {
        Console.WriteLine("=== CEFR Word Fetcher ===");
        Console.WriteLine("A SOLID-principled C# console application");
        Console.WriteLine();
        
        try
        {
            // 1. Load CEFR dataset
            Console.WriteLine("[Program] Step 1: Loading CEFR dataset");
            var words = await LoadCefrDataset();
            
            // 2. Select words randomly
            Console.WriteLine("[Program] Step 2: Selecting words randomly");
            var selectedWords = await SelectWords(words, args);
            
            // 3. Fetch detailed information from WordsAPI
            Console.WriteLine("[Program] Step 3: Fetching word information from WordsAPI");
            var wordInfoMap = await FetchWordInformation(selectedWords);
            
            // 4. Display results
            Console.WriteLine("[Program] Step 4: Displaying results");
            DisplayResults(selectedWords, wordInfoMap);
            
            // 5. Save to JSON file
            Console.WriteLine("[Program] Step 5: Saving results to JSON file");
            await SaveResultsToJson(selectedWords, wordInfoMap);
            
            Console.WriteLine("[Program] Application completed successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Program] Error: {ex.Message}");
            Console.WriteLine($"[Program] Stack trace: {ex.StackTrace}");
        }
    }
    
    /// <summary>
    /// Loads the CEFR dataset from CSV file.
    /// </summary>
    /// <returns>Collection of CEFR words</returns>
    private static async Task<IEnumerable<CEFRWord>> LoadCefrDataset()
    {
        Console.WriteLine($"[Program] Loading dataset from: {DataPath}");
        
        var csvLoader = new CsvLoader();
        var words = await csvLoader.LoadWordsAsync(DataPath);
        
        Console.WriteLine($"[Program] Successfully loaded {words.Count()} words from dataset");
        return words;
    }
    
    /// <summary>
    /// Selects words using the word selector service.
    /// </summary>
    /// <param name="words">Available words</param>
    /// <param name="args">Command line arguments</param>
    /// <returns>Selected words</returns>
    private static async Task<IEnumerable<CEFRWord>> SelectWords(IEnumerable<CEFRWord> words, string[] args)
    {
        // Parse command line arguments for level filter
        string? levelFilter = null;
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i].StartsWith("--level="))
            {
                levelFilter = args[i].Substring("--level=".Length);
                break;
            }
        }
        
        Console.WriteLine($"[Program] Level filter from command line: {levelFilter ?? "None"}");
        
        // Use dependency injection principle - depend on abstraction
        IWordSelector wordSelector = new RandomWordSelector();
        var selectedWords = wordSelector.SelectWords(words, 10, levelFilter);
        
        Console.WriteLine($"[Program] Selected {selectedWords.Count()} words for processing");
        return selectedWords;
    }
    
    /// <summary>
    /// Fetches detailed word information from WordsAPI.
    /// </summary>
    /// <param name="selectedWords">Selected words</param>
    /// <returns>Dictionary mapping words to their detailed information</returns>
    private static async Task<Dictionary<string, WordInfo?>> FetchWordInformation(IEnumerable<CEFRWord> selectedWords)
    {
        // Get API credentials (in production, these should come from environment variables or config)
        var apiKey = GetApiKey();
        
        if (string.IsNullOrEmpty(apiKey))
        {
            Console.WriteLine("[Program] Warning: No API key found. Skipping WordsAPI calls.");
            Console.WriteLine("[Program] Please set your WordsAPI key in the GetApiKey() method or use environment variables.");
            return new Dictionary<string, WordInfo?>();
        }
        
        // Use dependency injection principle - depend on abstraction
        IWordsApiService wordsApiService = new WordsApiService(apiKey);
        var wordStrings = selectedWords.Select(w => w.Word);
        var wordInfoMap = await wordsApiService.GetWordsInfoAsync(wordStrings);
        
        Console.WriteLine($"[Program] Successfully fetched information for {wordInfoMap.Values.Count(w => w != null)} words");
        return wordInfoMap;
    }
    
    /// <summary>
    /// Displays the results in the console.
    /// </summary>
    /// <param name="selectedWords">Selected CEFR words</param>
    /// <param name="wordInfoMap">Detailed word information</param>
    private static void DisplayResults(IEnumerable<CEFRWord> selectedWords, Dictionary<string, WordInfo?> wordInfoMap)
    {
        Console.WriteLine();
        Console.WriteLine("=== SELECTED WORDS ===");
        Console.WriteLine();
        
        foreach (var word in selectedWords)
        {
            Console.WriteLine($"Word: {word.Word}");
            Console.WriteLine($"CEFR Level: {word.Level}");
            Console.WriteLine($"Part of Speech: {word.PartOfSpeech}");
            
            if (word.Frequency.HasValue)
            {
                Console.WriteLine($"Frequency: {word.Frequency}");
            }
            
            // Display detailed information if available
            if (wordInfoMap.TryGetValue(word.Word, out var wordInfo) && wordInfo != null)
            {
                Console.WriteLine($"Pronunciation: {wordInfo.Pronunciation ?? "N/A"}");
                
                if (wordInfo.Definitions.Any())
                {
                    Console.WriteLine("Definitions:");
                    foreach (var definition in wordInfo.Definitions.Take(2)) // Show first 2 definitions
                    {
                        Console.WriteLine($"  • ({definition.PartOfSpeech}) {definition.Definition}");
                    }
                }
                
                if (wordInfo.Examples.Any())
                {
                    Console.WriteLine("Examples:");
                    foreach (var example in wordInfo.Examples.Take(2)) // Show first 2 examples
                    {
                        Console.WriteLine($"  • {example}");
                    }
                }
                
                if (wordInfo.Synonyms.Any())
                {
                    Console.WriteLine($"Synonyms: {string.Join(", ", wordInfo.Synonyms.Take(5))}");
                }
            }
            
            Console.WriteLine(new string('-', 50));
        }
    }
    
    /// <summary>
    /// Saves the results to a JSON file.
    /// </summary>
    /// <param name="selectedWords">Selected CEFR words</param>
    /// <param name="wordInfoMap">Detailed word information</param>
    private static async Task SaveResultsToJson(IEnumerable<CEFRWord> selectedWords, Dictionary<string, WordInfo?> wordInfoMap)
    {
        // Ensure output directory exists
        var outputDir = Path.GetDirectoryName(OutputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }
        
        // Create result object
        var result = new
        {
            GeneratedAt = DateTime.UtcNow,
            TotalWords = selectedWords.Count(),
            Words = selectedWords.Select(word => new
            {
                CefrWord = word,
                DetailedInfo = wordInfoMap.TryGetValue(word.Word, out var info) ? info : null
            }).ToList()
        };
        
        // Serialize to JSON
        var json = JsonConvert.SerializeObject(result, Formatting.Indented);
        await File.WriteAllTextAsync(OutputPath, json);
        
        Console.WriteLine($"[Program] Results saved to: {OutputPath}");
        Console.WriteLine($"[Program] File size: {json.Length} characters");
    }
    
    /// <summary>
    /// Gets the WordsAPI key from environment variables or returns a placeholder.
    /// </summary>
    /// <returns>API key or empty string</returns>
    private static string GetApiKey()
    {
        // Try to get from environment variable first
        var apiKey = Environment.GetEnvironmentVariable("WORDS_API_KEY");
        
        if (!string.IsNullOrEmpty(apiKey))
        {
            Console.WriteLine("[Program] Using API key from environment variable");
            return apiKey;
        }
        
        // For development, you can hardcode your API key here
        // In production, always use environment variables or secure configuration
        Console.WriteLine("[Program] No API key found in environment variables");
        Console.WriteLine("[Program] Please set WORDS_API_KEY environment variable or update GetApiKey() method");
        
        return string.Empty; // Return empty for now - you'll need to add your actual API key
    }
}
