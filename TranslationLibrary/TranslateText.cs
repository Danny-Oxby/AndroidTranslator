using Amazon.Translate.Model;
using Amazon.Translate;
using System.Threading.Tasks;
using System;
using System.Collections;
using System.Linq.Expressions;
using System.IO;
using System.Net.Http;

namespace TranslationLibrary
{
    public class TranslateText
    {
        //base langauge can be auto is unsure on the langauge but should ever happen with the current product
        public static async Task<string> TranslatingTextAsync(string inputtext, string newLangauge, string oldlanguage = "auto")
        {
            string errorpos = "before try";
            string configvalues;
            try
            {
                errorpos = "during connection";

                AmazonTranslateClient client = FindConnection.RetrieveConnection();

                if (client == null)
                {
                    errorpos = "connection failed";
                    throw new Exception("maunal throw for null client");
                }
                if (client.Config.AWSTokenProvider == null)
                {
                    errorpos = "AWSTokenProvider failed";
                    throw new Exception("maunal throw for null AWSTokenProvider");
                }
                

                if(client.Config.RegionEndpointServiceName != "translate")
                {
                    return client.Config.RegionEndpointServiceName;
                    //return "unexpected config";
                }
                configvalues = string.Concat("Fail request ", client.Config.FastFailRequests, "\n");
                configvalues = string.Concat("Conections ", client.Config.MaxConnectionsPerServer, "\n");

                errorpos = "during request";

                var request = new TranslateTextRequest
                {
                    SourceLanguageCode = oldlanguage,
                    TargetLanguageCode = newLangauge,
                    Text = inputtext,
                };

                errorpos = "during async";

                TranslateTextResponse response = null;
                try
                {
                    response = await client.TranslateTextAsync(request);
                }
                /*
                //catch (DetectedLanguageLowConfidenceException) // it is non of these errors
                //{
                //    return "one"; 
                //}
                //catch (InternalServerException)
                //{
                //    return "two";
                //}
                //catch (InvalidRequestException)
                //{
                //    return "three";
                //}
                //catch (ResourceNotFoundException)
                //{
                //    return "four";
                //}
                //catch (ServiceUnavailableException)
                //{
                //    return "five";
                //}
                //catch (TextSizeLimitExceededException)
                //{
                //    return "six";
                //}
                //catch (TooManyRequestsException)
                //{
                //    return "seven";
                //}
                //catch (UnsupportedLanguagePairException)
                //{
                //    return "eight";
                //}
                //catch (UnauthorizedAccessException)
                //{
                //    return "nine";
                //}*/
                catch (HttpRequestException tex)
                {
                    return string.Concat(tex.Message);
                }

                return response.TranslatedText;

            }
            catch(Exception ex)
            {
                SqlLibrary.Log.LogIssue(ex.Message, nameof(TranslatingTextAsync), SqlLibrary.IssueList.Translation, 
                    string.Concat("Params : ", inputtext, " target: ", newLangauge, " base: ", oldlanguage));

                string exceptiostring = ex.Message;

                //exceptiostring = string.Concat(exceptiostring, "\n", ex.GetType().Name);
                //exceptiostring = string.Concat(exceptiostring, "\n", ex.GetType().FullName);

                //if(ex.GetType() == typeof(InternalServerException))
                //    exceptiostring = string.Concat(exceptiostring, "\n", "internal error");
                //if (ex.GetType() == typeof(ServiceUnavailableException))
                //    exceptiostring = string.Concat(exceptiostring, "\n", "service availibility error");

                //exceptiostring = string.Concat(exceptiostring, "\n", ex.ToString());
                //exceptiostring = string.Concat(exceptiostring, "\n", ex.ErrorCode);


                //if (ex.Data != null) 
                //{
                //    //exceptiostring = string.Concat(exceptiostring, "\n");
                //    foreach (DictionaryEntry item in ex.Data)
                //    {
                //        exceptiostring = string.Concat(exceptiostring, "'" + item.Key.ToString() + "'", item.Value, "\n");
                //    }
                //} 
                //try
                //{
                //    File.WriteAllText(@"C:\Users\oxbyd\OneDrive - Sheffield Hallam University\FYP\TranslationLibrary\errorMessage.txt",
                //        ex.ToString());
                //} catch(Exception tex)
                //{
                //    string.Concat(tex.Message, "\n", exceptiostring);
                //}

                return string.Concat("Generic catch exception\n", exceptiostring, "\n", errorpos, "\n", ex.GetType().FullName, "\n"
                    , ex.ToString());
            }
        }
    }
}
