using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace TranslateTextFromImage;

public class Translator
{
    private static readonly HttpClient client = new HttpClient();
    private readonly string subscriptionKey = "7773477cf35c42b0afa488e8d785755b";
    private readonly string endpoint = "https://api.cognitive.microsofttranslator.com/";

    public async Task<string> TranslateAsync(string text, string targetLanguage)
    {
        var route = $"/translate?api-version=3.0&to={targetLanguage}";
        var requestBody = new object[] { new { Text = text } };
        var requestContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

        client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
        client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Region", "centralus");

        var response = await client.PostAsync(endpoint + route, requestContent);
        var responseBody = await response.Content.ReadAsStringAsync();

        try
        {
            var translationResult = JArray.Parse(responseBody);
            return translationResult[0]["translations"][0]["text"].ToString();
        }
        catch (JsonReaderException)
        {
            var errorResponse = JObject.Parse(responseBody);
            return $"Error: {errorResponse["error"]["message"]}";
        }
    }
}