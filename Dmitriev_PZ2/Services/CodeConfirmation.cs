using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Dmitriev_PZ2.Services
{
    internal class CodeConfirmation
    {
        Random random = new Random();
        /// <summary>
        /// Метод отправки электронного письма (email) с кодом подтверждения. 
        /// </summary>
        public string SendEmail(string email)
        {
            try
            {
                MailAddress from = new MailAddress("esosnov@internet.ru", "Кодовая рассылка");
                MailAddress to = new MailAddress(email);
                MailMessage message = new MailMessage(from, to);
                message.Subject = "Код подтверждения";
                string code = random.Next(100000, 999999).ToString();
                message.Body = $"Ваш код подтверждения: {code}";
                message.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient("smtp.mail.ru", 587);
                smtp.Credentials = new NetworkCredential("esosnov@internet.ru", "6mEq9yVqLeN7pw2G3mAb");
                smtp.EnableSsl = true;
                smtp.Send(message);
                return code;
            }
            catch (SmtpException smtpEx)
            {
                MessageBox.Show($"SMTP ошибка: {smtpEx.Message}");
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
                return null;
            }
        }
    }
}
