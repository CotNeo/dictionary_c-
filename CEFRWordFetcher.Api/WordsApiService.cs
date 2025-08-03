using CEFRWordFetcher.Core.Interfaces;
using CEFRWordFetcher.Core.Models;
using Newtonsoft.Json;

namespace CEFRWordFetcher.Api;

/// <summary>
/// Service for fetching word information from WordsAPI.
/// </summary>
public class WordsApiService : IWordsApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _apiHost;
    private readonly string _baseUrl;
    
    /// <summary>
    /// Initializes a new instance of the WordsApiService class.
    /// </summary>
    /// <param name="apiKey">WordsAPI API key</param>
    /// <param name="apiHost">WordsAPI host</param>
    public WordsApiService(string apiKey, string apiHost = "wordsapiv1.p.rapidapi.com")
    {
        _httpClient = new HttpClient();
        _apiKey = apiKey;
        _apiHost = apiHost;
        _baseUrl = "https://wordsapiv1.p.rapidapi.com/words/";
        
        // Configure headers
        _httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Key", _apiKey);
        _httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Host", _apiHost);
        
        Console.WriteLine($"[WordsApiService] Initialized with API host: {_apiHost}");
    }
    
    /// <summary>
    /// Fetches detailed information about a word from WordsAPI.
    /// </summary>
    /// <param name="word">The word to fetch information for</param>
    /// <returns>Detailed word information or null if not found</returns>
    public async Task<WordInfo?> GetWordInfoAsync(string word)
    {
        Console.WriteLine($"[WordsApiService] Fetching information for word: {word}");
        
        try
        {
            var url = $"{_baseUrl}{Uri.EscapeDataString(word)}";
            Console.WriteLine($"[WordsApiService] Making request to: {url}");
            
            var response = await _httpClient.GetAsync(url);
            
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"[WordsApiService] API request failed for '{word}': {response.StatusCode}");
                return null;
            }
            
            var json = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[WordsApiService] Received response for '{word}' ({json.Length} characters)");
            
            var wordInfo = JsonConvert.DeserializeObject<WordInfo>(json);
            
            if (wordInfo != null)
            {
                Console.WriteLine($"[WordsApiService] Successfully parsed word info for '{word}'");
                Console.WriteLine($"[WordsApiService] Definitions count: {wordInfo.Definitions.Count}");
                Console.WriteLine($"[WordsApiService] Examples count: {wordInfo.Examples.Count}");
            }
            
            return wordInfo;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[WordsApiService] Error fetching word info for '{word}': {ex.Message}");
            return null;
        }
    }
    
    /// <summary>
    /// Fetches detailed information for multiple words.
    /// </summary>
    /// <param name="words">List of words to fetch information for</param>
    /// <returns>Dictionary mapping words to their information</returns>
    public async Task<Dictionary<string, WordInfo?>> GetWordsInfoAsync(IEnumerable<string> words)
    {
        Console.WriteLine($"[WordsApiService] Starting batch fetch for {words.Count()} words");
        
        var results = new Dictionary<string, WordInfo?>();
        var wordList = words.ToList();
        
        // Process words with a small delay to respect API rate limits
        foreach (var word in wordList)
        {
            Console.WriteLine($"[WordsApiService] Processing word {wordList.IndexOf(word) + 1}/{wordList.Count}: {word}");
            
            var wordInfo = await GetWordInfoAsync(word);
            results[word] = wordInfo;
            
            // Add a small delay between requests to be respectful to the API
            if (wordList.IndexOf(word) < wordList.Count - 1)
            {
                await Task.Delay(100); // 100ms delay between requests
            }
        }
        
        var successCount = results.Values.Count(w => w != null);
        Console.WriteLine($"[WordsApiService] Batch fetch completed. Success: {successCount}/{wordList.Count}");
        
        return results;
    }
    
    /// <summary>
    /// Disposes the HTTP client.
    /// </summary>
    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}
