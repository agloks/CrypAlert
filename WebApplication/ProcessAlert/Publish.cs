using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;

namespace ProcessAlert
{
    public enum MessageType
    {
        Sms = 1,
        Email = 2,
    }
    public class Letter
    {
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public MessageType Type { get; set; }
        public Letter(MessageType type, string message, string phone, string email)
        {
            this.Message = message;
            this.Phone = phone;
            this.Email = email;
            this.Type = type;
        }
    }
    
    public class Publish
    {
        public static void SendSMS(Letter letter)
        {
            var awsCredentials = new Amazon.Runtime.BasicAWSCredentials(Env.ACESS_KEY, Env.PRIVATE_KEY); 
            AmazonSimpleNotificationServiceClient snsClient = new AmazonSimpleNotificationServiceClient(awsCredentials, Amazon.RegionEndpoint.USWest2);
            PublishRequest pubRequest = new PublishRequest();
            pubRequest.Message = letter.Message;
            pubRequest.PhoneNumber = letter.Phone; //+1XXX5550100
            // add optional MessageAttributes, for example:
            //   pubRequest.MessageAttributes.Add("AWS.SNS.SMS.SenderID", new MessageAttributeValue
            //      { StringValue = "SenderId", DataType = "String" });
            Task<PublishResponse> pubResponse = snsClient.PublishAsync(pubRequest);
            Task.WaitAll(pubResponse);
            Console.WriteLine("All Fine");
        }
        
        public static void SendEmail(Letter letter)
        {
            // Replace sender@example.com with your "From" address.
            // This address must be verified with Amazon SES.
            string senderAddress = Env.PRIVATE_EMAIL;

            // Replace recipient@example.com with a "To" address. If your account
            // is still in the sandbox, this address must be verified.
            string receiverAddress = letter.Email;

            // The configuration set to use for this email. If you do not want to use a
            // configuration set, comment out the following property and the
            // ConfigurationSetName = configSet argument below. 
            // string configSet = "ConfigSet";

            // The subject line for the email.
            string subject = "CrypAlert!";

            // The email body for recipients with non-HTML email clients.
            string textBody = letter.Message;
        
            // The HTML body of the email.
            string htmlBody = @"<html>
            <head></head>
            <body>
              <h1>Amazon SES Test (AWS SDK for .NET)</h1>
              <p>This email was sent with
                <a href='https://aws.amazon.com/ses/'>Amazon SES</a> using the
                <a href='https://aws.amazon.com/sdk-for-net/'>
                  AWS SDK for .NET</a>.</p>
            </body>
            </html>";
            
            var awsCredentials = new Amazon.Runtime.BasicAWSCredentials(Env.ACESS_KEY, Env.PRIVATE_KEY); 
            using (var client = new AmazonSimpleEmailServiceClient(awsCredentials, Amazon.RegionEndpoint.USEast1))
            {
                var sendRequest = new SendEmailRequest
                {
                    Source = senderAddress,
                    Destination = new Destination
                    {
                        ToAddresses =
                            new List<string> { receiverAddress }
                    },
                    Message = new Message
                    {
                        Subject = new Content(subject),
                        Body = new Body
                        {
                            Html = new Content
                            {
                                Charset = "UTF-8",
                                Data = htmlBody
                            },
                            Text = new Content
                            {
                                Charset = "UTF-8",
                                Data = textBody
                            }
                        }
                    },
                };
                try
                {
                    Console.WriteLine("Sending email using Amazon SES...");
                    var response = client.SendEmailAsync(sendRequest);
                    Task.WaitAll(response);
                    Console.WriteLine("The email was sent successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("The email was not sent.");
                    Console.WriteLine("Error message: " + ex.Message);

                }
            }
        }
    }
}