using System.Security.Cryptography.X509Certificates;
using DevinoTest.Dto;

namespace DevinoTest.Services
{
    public class SmsService
    {
        public static SmsDto[] GetSmsList()
        {
            return new []
            {
                new SmsDto { Phone = "123456789", Message = "Добрый день, Ваша заявка выполнена!"},
                new SmsDto { Phone = "923456789", Message = "Добрый день, Ваша заявка выполнена!"},
                new SmsDto { Phone = "823456789", Message = "Добрый день, Ваша заявка выполнена!"},
                new SmsDto { Phone = "723456789", Message = "Добрый день, Ваша заявка выполнена!"},
                new SmsDto { Phone = "623456789", Message = "Добрый день, Ваша заявка выполнена!"},
                new SmsDto { Phone = "523456789", Message = "Добрый день, Ваша заявка выполнена!"},
                new SmsDto { Phone = "423456789", Message = "Добрый день, Ваша заявка выполнена!"},
                new SmsDto { Phone = "323456789", Message = "Добрый день, Ваша заявка выполнена!"},
                new SmsDto { Phone = "223456789", Message = "Добрый день, Ваша заявка выполнена!"} 
            };
        }
    }
}