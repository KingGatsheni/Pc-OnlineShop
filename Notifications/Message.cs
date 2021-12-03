using Vonage;
using Vonage.Request;
using Vonage.Messaging;
using System;

namespace OnlineShop.Notifications
{
    public class Message
    {
        Credentials credentials;
        VonageClient VonageClient;
        public Message()
        {
            credentials = Credentials.FromApiKeyAndSecret(
                "c91f25b7",
               "qg8bREVArLnpr48N"
                );
            VonageClient = new VonageClient(credentials);
        }
        public  void Execute()
        {
           
                SendSmsResponse response = VonageClient.SmsClient.SendAnSms(new SendSmsRequest()
                {
                    To = "27839578644",
                    From = "PcWizrd",
                    Text = "Thank Sihle You for Shopping with PcWizard Your Order is Being Packed. OrderNo: #26 See you Soon for the Pick up have a great day!!!"
                });           
        }
    }
}
