using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.ComponentModel.DataAnnotations;

namespace LampaEventService
{
    //Класс для отправки сообщений
    //
    //При инициализации нужно указать:
    // 1) string - тип ящика для отправки(подхватывается из adm_Emails с условием по полю Type,
    //    важно, чтобы запись при выборке была уникальной, она используется для отправки сообщений)
    // 2) List<string> - список емейл, которым будут уходить письмо
    // 3) string - тема сообщения
    // 4) string - тело сообщения
    //Например:
    // MailSender mail = new MailSender("Уведомления",EmList, "Тестовое уведомление", "Тестовое сообщение \\n 123")
    //Методы:
    // mail.Send() - отправляет сообщение с параметрами, указанными при инициализации экземпляра класса
    public class MailSender
    {
        public MailSender(string emtype, List<string> emlist, string subj, string bod)
        {
            emailType = emtype;
            emailList = emlist;
            subject = subj;
            body = bod;
        }
        private string emailType { get; set; }
        private List<string> emailList { get; set; }
        private string subject { get; set; }
        private string body { get; set; }


        //Метод для отправки сообщений.
        public void Send()
        {
            //Вытаскиваем параметры сервера для рассылки
            EasySQL serverParams = new EasySQL("select * from adm_Emails where Type ='" + emailType + "'");
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict = serverParams.RowToDictionary().ToDictionary(entry => entry.Key,
                                                         entry => entry.Value);
            //Задаем параметры сервера для рассылки
            string from = dict["box"];
            string password = dict["password"];
            string outServer = dict["outServer"];
            int outPort = Convert.ToInt32(dict["outPort"]);
            SmtpClient client = new SmtpClient();
            client.Port = outPort;
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(from, password);
            client.Host = outServer;
            
            //Формируем сообщение для отправки
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(from);
            foreach (string email in emailList)
            {
                if (new EmailAddressAttribute().IsValid(email))
                {
                    try
                    {
                        mail.To.Add(email);
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            mail.Subject = subject;
            mail.Body = body;

            //Отправляем сообщение с настроенного клиента
            if (mail.To.Any())
            {
                client.Send(mail);
            }
        }
    }
}
