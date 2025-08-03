using CEFRWordFetcher.Core.Models;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace CEFRWordFetcher.Infrastructure;

/// <summary>
/// Loads CEFR words from CSV files.
/// </summary>
public class CsvLoader
{
    /// <summary>
    /// Loads CEFR words from a CSV file.
    /// </summary>
    /// <param name="filePath">Path to the CSV file</param>
    /// <returns>Collection of CEFR words</returns>
    public async Task<IEnumerable<CEFRWord>> LoadWordsAsync(string filePath)
    {
        Console.WriteLine($"[CsvLoader] Starting to load words from: {filePath}");
        
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"[CsvLoader] Error: File not found at {filePath}");
            throw new FileNotFoundException($"CSV file not found: {filePath}");
        }
        
        try
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null,
                Delimiter = ",",
                HasHeaderRecord = true
            });
            
            var words = new List<CEFRWord>();
            var records = csv.GetRecords<dynamic>();
            
            foreach (var record in records)
            {
                try
                {
                    var word = new CEFRWord
                    {
                        Word = record.word?.ToString() ?? string.Empty,
                        Level = record.level?.ToString() ?? string.Empty,
                        PartOfSpeech = record.pos?.ToString() ?? string.Empty,
                        Frequency = ParseFrequency(record.frequency?.ToString())
                    };
                    
                    if (!string.IsNullOrEmpty(word.Word))
                    {
                        words.Add(word);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[CsvLoader] Warning: Failed to parse record: {ex.Message}");
                }
            }
            
            Console.WriteLine($"[CsvLoader] Successfully loaded {words.Count} words from CSV");
            Console.WriteLine($"[CsvLoader] Level distribution: {string.Join(", ", words.GroupBy(w => w.Level).Select(g => $"{g.Key}:{g.Count()}"))}");
            
            return words;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[CsvLoader] Error loading CSV file: {ex.Message}");
            throw;
        }
    }
    
    /// <summary>
    /// Parses frequency value from string.
    /// </summary>
    /// <param name="frequencyString">Frequency as string</param>
    /// <returns>Parsed frequency or null</returns>
    private static double? ParseFrequency(string? frequencyString)
    {
        if (string.IsNullOrEmpty(frequencyString))
            return null;
            
        if (double.TryParse(frequencyString, out var frequency))
            return frequency;
            
        return null;
    }
}
