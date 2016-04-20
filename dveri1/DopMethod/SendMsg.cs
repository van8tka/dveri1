using System;
using System.Net.Mail;
using System.Net;

namespace dveri1.DopMethod
{
    public class SendMsg
    {
        
        public static bool Message(string Body, string Them, string adresKlient)
        {
            string SystemEmail = "systememail11@yandex.ru";
            string Passwword = "qazWSX123";
            try
            {
                             
               ClassLog.Write("Создание сообщения для отправки");
             
                MailMessage message = new MailMessage();
              
             
                message.To.Add(adresKlient);
             
                message.From = new System.Net.Mail.MailAddress(SystemEmail, "ЛюксЕвроСтрой");

                message.Subject = Them; // указание темы письма
                message.BodyEncoding = System.Text.Encoding.UTF8; // указание кодировки письма
                message.IsBodyHtml = false; // указание формата письма (true - HTML, false - не HTML)
               
              
                message.Body = Body; // указание текста (тела) письма
                //SmtpClient client = new SmtpClient("smtp.mail.ru", 25);
                SmtpClient client = new SmtpClient("smtp.yandex.ru", 25);
                // создание нового подключения к серверу "smtp.domain.tld"
                client.DeliveryMethod = SmtpDeliveryMethod.Network; // определяет метод отправки сообщений
                client.EnableSsl = true; // отключает необходимость использования защищенного соединения с сервером
                client.UseDefaultCredentials = false; // отключение использования реквизитов авторизации "по-умолчанию"
                // указание нужных реквизитов (имени пользователя и пароля) для авторизации на SMTP-сервере
                client.Credentials = new NetworkCredential(SystemEmail, Passwword);
                // указание нужных реквизитов (имени пользователя и пароля) для авторизации на SMTP-сервере
                client.Send(message); // отправка сообщения     
                return true;
            }
            catch(Exception er)
            {
                ClassLog.Write("Ошибка при создании и отправки сообщения: "+ er);
                return false;
            }
            
        }
    }
}