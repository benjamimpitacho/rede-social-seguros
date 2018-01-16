using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(
                    new System.Net.Mail.MailAddress("geral@falarseguros.pt", "InsuranceTestApp"),
                    new System.Net.Mail.MailAddress("benjamim_pitacho@hotmail.com"));
                m.Subject = "Email de Teste #1";
                m.Body = "Este é um e-mail de teste da aplicação de seguros. Obrigado.";
                m.IsBodyHtml = false;
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp-pt.securemail.pro", 25)
                {
                    Credentials = new System.Net.NetworkCredential("geral@falarseguros.pt", "fsarruda33"),
                    EnableSsl = false
                };
                smtp.Send(m);
                Console.WriteLine("Mail sent!");
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try
            {
                System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(
                    new System.Net.Mail.MailAddress("geral@falarseguros.pt", "InsuranceTestApp"),
                    new System.Net.Mail.MailAddress("benjamim_pitacho@hotmail.com"));
                m.Subject = "Email de Teste #1.1";
                m.Body = "Este é um e-mail de teste da aplicação de seguros. Obrigado.";
                m.IsBodyHtml = false;
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp-pt.securemail.pro", 465)
                {
                    Credentials = new System.Net.NetworkCredential("smtp@falarseguros.pt", "fsarruda33"),
                    EnableSsl = true
                };
                smtp.Send(m);
                Console.WriteLine("Mail sent!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try
            {
                System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(
                    new System.Net.Mail.MailAddress("geral@falarseguros.pt", "InsuranceTestApp"),
                    new System.Net.Mail.MailAddress("benjamim_pitacho@hotmail.com"));
                m.Subject = "Email de Teste #2";
                m.Body = "Este é um e-mail de teste da aplicação de seguros. Obrigado.";
                m.IsBodyHtml = false;
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.falarseguros.pt", 25)
                {
                    Credentials = new System.Net.NetworkCredential("geral@falarseguros.pt", "fsarruda33"),
                    EnableSsl = false
                };
                smtp.Send(m);
                Console.WriteLine("Mail sent!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try
            {
                System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(
                    new System.Net.Mail.MailAddress("geral@falarseguros.pt", "InsuranceTestApp"),
                    new System.Net.Mail.MailAddress("benjamim_pitacho@hotmail.com"));
                m.Subject = "Email de Teste #3";
                m.Body = "Este é um e-mail de teste da aplicação de seguros. Obrigado.";
                m.IsBodyHtml = false;
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.falarseguros.pt", 25)
                {
                    Credentials = new System.Net.NetworkCredential("smtp@falarseguros.pt", "fsarruda33"),
                    EnableSsl = false
                };
                smtp.Send(m);
                Console.WriteLine("Mail sent!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}
