using AWSSDK;
using Amazon.Textract;
using Amazon.Textract.Model;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace OCRLibrary
{
    //Amazon Textract can extract printed text, forms and tables in English, German, French, Spanish, Italian and Portuguese.
    public class ReadImage
    {
        //https://gist.github.com/normj/86e4eceffc14c183c6040a5705e3918b
        public static async Task<string> ReturnImageText(string image) ///data/user/0/com.companyname.allergytranslator/cache/38195ae7492c4bc59b5f5d4f6a312954.jpg
        {
            string response = null;
            try
            {
                AmazonTextractClient client = FindConnection.RetrieveConnection();

                var bytes = File.ReadAllBytes(image);

                var detectResponse = await client.DetectDocumentTextAsync(new DetectDocumentTextRequest
                {
                    Document = new Document
                    {
                        Bytes = new MemoryStream(bytes)
                    }
                });

                foreach (var block in detectResponse.Blocks) // only want the line type returns
                {
                    //response = string.Concat(response, "\n", ($"Type {block.BlockType}, Text: {block.Text}"));

                    if (block.BlockType == BlockType.LINE)
                    {
                        response = string.Concat(response, " \n ", block.Text);
                    }
                }
            }
            catch(Exception ex)
            {
                //this wnet wrong
                SqlLibrary.Log.LogIssue(ex.Message, nameof(ReturnImageText), SqlLibrary.IssueList.Image_Reading, string.Concat("Params : ", image));
                return ex.Message;
            }
            return response;
        }
    }
}
