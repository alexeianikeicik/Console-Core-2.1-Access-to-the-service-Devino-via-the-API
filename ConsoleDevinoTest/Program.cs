using System;
using System.Net;
using System.IO;
using System.Text;
using System.Text.Json;
using DevinoTest.Services;
using DevinoTest.Dto;

namespace ConsoleDevinoTest
{
    class Program
    {
        //вспомогательные классы для десериализации Json строки в объект с ответом от сервера о балансе лицевого счёта
        public class Rootobject1
        {
            public Result result { get; set; }
        }

        public class Result
        {
            public Account account { get; set; }
            public object[] validPacketChannels { get; set; }
        }

        public class Account
        {
            public int companyId { get; set; }
            public string accountType { get; set; }
            public float balance { get; set; }
            public float credit { get; set; }
            public float reserveSms { get; set; }
            public float reserveViber { get; set; }
            public float reserve { get; set; }
            public int currencyId { get; set; }
            public bool isBlocked { get; set; }
            public float notifyThreshold { get; set; }
        }

        //вспомогательные классы для десериализации Json строки в объект с ответом от сервера о результате отправки СМС
        public class Rootobject2
        {
            public Result2[] result { get; set; }
        }

        public class Result2
        {
            public string code { get; set; }
            public string messageId { get; set; }
            public object segmentsId { get; set; }
        }




        static void Main(string[] args)
        {
            Console.WriteLine("devino");
            //Имя компании от лица, которой будет осуществяться рассылка
            //string CompanyName = "devino";
            string CompanyName = "DVNtelecom";
            string responseString;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.devino.online/billing-api/companies/current/account");
                request.Method = "GET";
                //Уникальный API ключ идентификатор на сайте рассылки devino
                request.Headers["Authorization"] = "Key b867e5e3-837e-4abd-9296-ed3dda6b7559";
                request.Headers["Content-Type"] = "application/json";

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Stream dataStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    responseString = reader.ReadToEnd();
                    reader.Close();
                    dataStream.Close();
                }
                //Console.WriteLine(responseString);
                Rootobject1 myDeserializedClass = JsonSerializer.Deserialize<Rootobject1>(responseString);
                Console.WriteLine($"Баланс на лицевом счете: {myDeserializedClass.result.account.balance} рублей");
                //Отправить СМС если на балансе больше 100 рублей
                if (myDeserializedClass.result.account.balance > 100)
                {
                    byte[] response2;
                    Rootobject2 myDeserializedClass2;

                    foreach (SmsDto item in SmsService.GetSmsList())
                    {
                        using (var client = new WebClient())
                        {
                            client.Headers["Authorization"] = "Key b867e5e3-837e-4abd-9296-ed3dda6b7559";
                            client.Headers["Content-Type"] = "application/json";
                            try
                            {
                                response2 = client.UploadData("https://api.devino.online/sms/messages", "POST", Encoding.Default.GetBytes("{ " +
                                "\"messages\": [" +
                                    "{" +
                                  "      \"from\": \"" + $"{CompanyName}\"," +
                                  "        \"to\": \"" + $"{item.Phone}\"," +
                                        "\"text\": \"" + $"{item.Message}\"" +
                                    "}" +
                                  "]" +
                                "}"));

                                responseString = Encoding.Default.GetString(response2);
                                //Console.WriteLine(responseString);
                                myDeserializedClass2 = JsonSerializer.Deserialize<Rootobject2>(responseString);
                                Console.WriteLine($"messageId = {myDeserializedClass2.result[0].messageId} и результат отправки сообщения: Code = {myDeserializedClass2.result[0].code}");
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"Не отправлено сообщение: {e.Message}");
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Проблема при подключении к серверу: {e.Message}");
            }
        }
    }
}
