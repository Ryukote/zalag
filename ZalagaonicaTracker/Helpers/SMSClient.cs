namespace ZalagaonicaTracker.Helpers
{
    public static class SMSClient
    {
        private static string smsAPIToken = "uuFdUbDrdePoZ579NTw6fmq9IwkbS0FwzF6U5Osm";
        public static async Task SendSMS(string toPhoneNumber, string pawnShopContactNumber)
        {
            SMSApi.Api.IClient client = new SMSApi.Api.ClientOAuth(smsAPIToken);

            var smsFactory = new SMSApi.Api.SMSFactory(client);

            string smsContent = $"Postovani, vidjeli smo Vas oglas i zanima nas otkup. Javite nam se na {pawnShopContactNumber} kako bismo dali ponudu. Nadamo se suradnji. Gradske Zalagaonice.";

            await smsFactory.ActionSend(toPhoneNumber, smsContent).ExecuteAsync();
        }
    }
}
