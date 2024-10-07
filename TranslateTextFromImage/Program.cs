namespace TranslateTextFromImage;

using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Buffers;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

class Program
{

    private static readonly string endpoint = "https://qq-dev-us.cognitiveservices.azure.com/";
    private static readonly string apiKey = "3e62fbe67ce248daa328089100d149e8";

    public static async Task Main(string[] args)
    {
        Console.Write("Введіть шлях до зображення: ");
        string imagePath = Console.ReadLine();
        var ocrservice = new OCRService();
        var text = await ocrservice.ExtractTextUsingAzure(imagePath);
        Console.WriteLine("Текст з зображення:");
        Console.WriteLine(text);

        Console.WriteLine();
        Console.WriteLine("Виберіть мову для озвучування тексту:");
        Console.WriteLine("1. Українська (uk)");
        Console.WriteLine("2. Англійська (en)");
        Console.WriteLine("3. Німецька (de)");
        Console.WriteLine("4. Румунська (ro)");
        Console.WriteLine("5. Угорська (hu)");
        Console.WriteLine("6. Французька (fr)");
        Console.WriteLine("За замовчуванням англійська");
        Console.Write("Введіть номер мови: ");

        string languageChoice = Console.ReadLine();

        string language = languageChoice switch
        {
            "1" => "uk",
            "2" => "en",
            "3" => "de",
            "4" => "ro",
            "5" => "hu",
            "6" => "fr",
            _ => "en"
        };

        var translator = new Translator();
        string translatedText = await translator.TranslateAsync(text, language);
        Console.WriteLine();
        Console.WriteLine("Перекладений текст:");
        Console.WriteLine(translatedText);

        var textToSpeech = new TextToSpeech();
        await textToSpeech.SpeakAsync(translatedText, language);
    }

}
