# CEFR Word Fetcher (C# + WordsAPI)

A SOLID-principled C# console application to:

* Load CEFR-level vocabulary from a dataset
* Randomly select 10 words (optionally by level)
* Fetch definitions and usage examples using WordsAPI
* Output to console or save as JSON

## 🚀 Technologies

* C# (.NET 8.0)
* HttpClient
* CSV parsing (CsvHelper)
* JSON serialization (Newtonsoft.Json)
* SOLID Principles implementation

## 📁 Project Structure

```
CEFRWordFetcher/
├── CEFRWordFetcher.sln
├── /CEFRWordFetcher.Core              # Domain models and interfaces
│   ├── Models/
│   │   ├── CEFRWord.cs
│   │   └── WordInfo.cs
│   ├── Interfaces/
│   │   ├── IWordSelector.cs
│   │   └── IWordsApiService.cs
│   └── Services/
│       └── RandomWordSelector.cs
│
├── /CEFRWordFetcher.Infrastructure    # Data loading (CSV)
│   └── CsvLoader.cs
│
├── /CEFRWordFetcher.Api               # WordsAPI HTTP service
│   └── WordsApiService.cs
│
├── /CEFRWordFetcher.ConsoleApp        # Main app (UI entry point)
│   └── Program.cs
│
├── /data
│   └── cefr_dataset.csv
├── /output
│   └── output.json
```

## 🧪 SOLID Principles Applied

| Principle | Implementation |
|-----------|----------------|
| **S - Single Responsibility** | Each class has one job (API, CSV loading, word selection, etc.) |
| **O - Open/Closed** | New word selectors can be added without modifying existing code |
| **L - Liskov Substitution** | Interfaces can be mocked or replaced with different implementations |
| **I - Interface Segregation** | Small, focused interfaces like `IWordsApiService`, `IWordSelector` |
| **D - Dependency Inversion** | Main app depends on abstractions, not concrete implementations |

## ▶️ Usage

### Basic Usage
```bash
dotnet run --project CEFRWordFetcher.ConsoleApp
```

### Filter by CEFR Level
```bash
dotnet run --project CEFRWordFetcher.ConsoleApp -- --level=A1
dotnet run --project CEFRWordFetcher.ConsoleApp -- --level=B2
```

## 🔑 WordsAPI Setup

1. Sign up: [https://rapidapi.com/dpventures/api/wordsapi](https://rapidapi.com/dpventures/api/wordsapi)
2. Set your API key: `export WORDS_API_KEY="your_api_key_here"`
3. Run the application to fetch detailed word information

## 📄 License

Maintenance, Author Cotneo
