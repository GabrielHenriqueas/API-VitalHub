using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace WebAPI.Utils.OCR
{
    public class OcrService
    {
        private readonly string _subscriptKey = "5262bde2d72b4b46b0bd24fb23194964";

        private readonly string _endpoint = "https://vitalhub3dt.cognitiveservices.azure.com/";

        public async Task<string> RecognizeTextAsync(Stream imageStream)
        {
            try
            {
                var client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(_subscriptKey))
                {
                    Endpoint = _endpoint
                };

                var ocrResult = await client.RecognizePrintedTextInStreamAsync(true, imageStream);

                return ProcessRecognitionResult(ocrResult);
            }
            catch (Exception ex)
            {

                return"Erro ao reconhecer o texto: " + ex.Message;
            }

        }
            private static string ProcessRecognitionResult(OcrResult result)
            {
                try
                {
                    var recognizedText = "";
                    
                    foreach(var region in result.Regions)
                    {
                        foreach (var line in region.Lines)
                        {
                            foreach (var word in line.Words)
                            {
                                recognizedText += word.Text + " ";
                            }
                            recognizedText += "\n";
                        }
                    }    
                    return recognizedText;

                }
                catch (Exception)
                {

                    throw;
                }
            }
    }
}
