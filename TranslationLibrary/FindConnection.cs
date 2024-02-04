using Amazon.Translate.Model;
using Amazon.Translate;
using System;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3.Model.Internal.MarshallTransformations;

namespace TranslationLibrary
{
    // https://github.com/awsdocs/aws-doc-sdk-examples/tree/main/dotnetv3
    internal class FindConnection
    {
        // https://eu-north-1.console.aws.amazon.com/cloudwatch/home?region=eu-north-1#home:
        private static readonly RegionEndpoint Region = RegionEndpoint.EUWest2; //set to London bu consider changin based on location
        private static AmazonTranslateClient clientconnection { get; set; } = null;

        public static AmazonTranslateClient RetrieveConnection()
        {
            try
            {
                if (clientconnection == null)
                {
                    //find a way to hid this infomrtion for secutiry reasons
                    clientconnection = new AmazonTranslateClient("AKIASEP7KVAX4R2W5THG", "Tmn45zW5eYnqUxWCUxymTAqPsuUZluTQitB2qgLQ", Region);
                }

                return clientconnection;
                //var srcText = "milk 20%, wheat 65%, eggs 15%";
            }catch
            {
                return null;
            }
        }

    }

}
