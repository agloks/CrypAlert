using System.Threading.Tasks;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;

namespace ProcessAlert
{
    public class Publish
    {
        static void SendSMS(string message, string phone)
        {
            var awsCredentials = new Amazon.Runtime.BasicAWSCredentials("a_k", "a_s"); 
            AmazonSimpleNotificationServiceClient snsClient = new AmazonSimpleNotificationServiceClient(awsCredentials, Amazon.RegionEndpoint.USWest2);
            PublishRequest pubRequest = new PublishRequest();
            pubRequest.Message = message;
            pubRequest.PhoneNumber = phone; //+1XXX5550100
            // add optional MessageAttributes, for example:
            //   pubRequest.MessageAttributes.Add("AWS.SNS.SMS.SenderID", new MessageAttributeValue
            //      { StringValue = "SenderId", DataType = "String" });
            Task<PublishResponse> pubResponse = snsClient.PublishAsync(pubRequest);
        }
    }
}