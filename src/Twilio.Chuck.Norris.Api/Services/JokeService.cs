using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Twilio.Chuck.Norris.Api.Enums;
using Twilio.Chuck.Norris.Api.Models;

namespace Twilio.Chuck.Norris.Api.Services
{
    public class JokeService : IJokeService
    {
        public async Task<ChuckNorrisResponse> GetJokeAsync(string message)
        {
            var categoryCheck = GetCategory(message);
            var cat = categoryCheck.category.ToString();
            var category = cat != string.Empty ? cat.ToLower() : "";
             
            var response = await GetNewJokeAsync(category).ConfigureAwait(false);

            return response;
        }

        

        private (JokeCategory category, bool hasCategory) GetCategory(string message)
        {
            var hasCategory = Enum.TryParse(message, true, out JokeCategory cat);

            return (category: cat, hasCategory: hasCategory);
        }

        private async Task<ChuckNorrisResponse> GetNewJokeAsync(string category)
        {
            ChuckNorrisResponse joke = null;

            using (var httpClient = new HttpClient())
            {

                try
                {
                    var apiEndpointRandom = "https://api.chucknorris.io/jokes/random";
                    var apiEndpointCategory = $"https://api.chucknorris.io/jokes/random?category={category}";
                    var apiEndPoint = category != "0" ? apiEndpointCategory : apiEndpointRandom;
                    httpClient.BaseAddress = new Uri(apiEndPoint);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var responseMessage = await 
                        httpClient
                            .GetAsync(apiEndPoint);

                    if (responseMessage.IsSuccessStatusCode)
                    {
                        joke = await 
                            responseMessage
                                .Content
                                .ReadAsAsync<ChuckNorrisResponse>();
                    }

                }
                catch (Exception e)
                {
                    LogHelper.Error($"JokeService:GetNewJoke Exceptioned with {e.Message}");
                }
            }
            return joke;
        }
        
    }

    
}
