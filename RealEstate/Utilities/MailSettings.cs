using System.Net.Mail;
using System.Net;

namespace RealEstate.Utilities
{
	public class Email
	{
		public string Subject { get; set; }
		public string Body { get; set; }
		public string Recipient { get; set; }
	}
	public static class MailSettings
	{
		public static void SendEmail(Email email)
		{

			// create STMP Client 
			var client = new SmtpClient("smtp.gmail.com", 587);

			client.EnableSsl = true;

			//create newtwork credentials
			client.Credentials = new NetworkCredential("kareemhamada219@gmail.com", "ibts ioog qasg itpi");

			client.Send("kareemhamada219@gmail.com", email.Recipient, email.Subject, email.Body);
		}
	}
}
