using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MimeKit;
using Shared.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SendMail_Api.Presentation.Controller
{
    [Route("api/send-mail")]
    [ApiController]
    public class SendMailController : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> SendMailAsync([FromForm] SendMaiDto sendMailDto)
        {
            try
            {
                using MimeMessage mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress("it inventech", sendMailDto.Username));
                mimeMessage.To.Add(new MailboxAddress(sendMailDto.Receiver, sendMailDto.Receiver));
                mimeMessage.Subject = "ทดสอบการส่ง E-mail";

                BodyBuilder bodyBuilder = new()
                {
                    TextBody = sendMailDto.Message
                };

                if (sendMailDto.File != null && sendMailDto.File.Length > 0)
                {
                    using var memoryStream = new MemoryStream();
                    await sendMailDto.File.CopyToAsync(memoryStream);
                    var fileBytes = memoryStream.ToArray();
                    bodyBuilder.Attachments.Add(sendMailDto.File.FileName, fileBytes);
                }


                mimeMessage.Body = bodyBuilder.ToMessageBody();

                SecureSocketOptions secureSocketOptions = sendMailDto.Port switch
                {
                    465 => SecureSocketOptions.SslOnConnect,
                    587 => SecureSocketOptions.StartTls,
                    _ => SecureSocketOptions.Auto,

                };

                using SmtpClient mailClient = new();
                mailClient.ServerCertificateValidationCallback = (s, c, h, e) => true;
                 mailClient.Connect(sendMailDto.SmtpServer, sendMailDto.Port, secureSocketOptions);
                 mailClient.Authenticate(sendMailDto.Username, sendMailDto.Password);
                 mailClient.Send(mimeMessage);
                 mailClient.Disconnect(true);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("ไม่สามารถส่งอีเมลได้" + ex.Message);
                //Sending failed
            }

            return Ok();
        }
    }
}
