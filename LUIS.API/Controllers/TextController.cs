using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime.Models;


namespace LUIS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TextController : ControllerBase
    {
        private static string _predictionKey = Environment.GetEnvironmentVariable("LUIS_PREDICTION_KEY");
        private static string _predictionEndpoint = Environment.GetEnvironmentVariable("LUIS_ENDPOINT_NAME");
        private static string _appId = Environment.GetEnvironmentVariable("LUIS_APP_ID");


        private static LUISRuntimeClient CreateClient()
        {
            var credentials = new ApiKeyServiceClientCredentials(_predictionKey);
            return new LUISRuntimeClient(credentials, new System.Net.Http.DelegatingHandler[] { })
            {
                Endpoint = _predictionEndpoint
            };
        }

        [HttpGet]
        public async Task<IActionResult> GetPredictionAsync(string query)
        {
            try
            {
                using (var luisClient = CreateClient())
                {
                    var requestOptions = new PredictionRequestOptions
                    {
                        PreferExternalEntities = true
                    };

                    var predictionRequest = new PredictionRequest
                    {
                        Query = query,
                        Options = requestOptions
                    };


                    var result = await luisClient.Prediction.GetSlotPredictionAsync(Guid.Parse(_appId),
                        slotName: "staging",
                        predictionRequest,
                        verbose: true,
                        showAllIntents: true,
                        log: true);

                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
