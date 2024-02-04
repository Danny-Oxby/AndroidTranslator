using System;
using System.Threading.Tasks;
using AWSSDK;
using Amazon;
using Amazon.Textract;

namespace OCRLibrary
{
    // https://github.com/awsdocs/aws-doc-sdk-examples/tree/main/dotnetv3
    internal class FindConnection
    {
        // https://eu-north-1.console.aws.amazon.com/cloudwatch/home?region=eu-north-1#home:
        private static readonly RegionEndpoint Region = RegionEndpoint.EUWest2; //set to London bu consider changin based on location
        private static AmazonTextractClient clientconnection { get; set; } = null;

        public static AmazonTextractClient RetrieveConnection()
        {
            if(clientconnection == null)
            {
                //find a way to hid this infomrtion for secutiry reasons
                clientconnection = new AmazonTextractClient("YOURAccessKey", "YOURSecreatKey", Region);
            }

            return clientconnection;
        }

    }

}
