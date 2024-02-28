using Moq;
using System.Net.Mail;
using BlazorAppUpload.Data;

namespace BlazorAppUpload.Tests
{
    public class SendMailHelperTests
    {
        [Fact]
        public void ValidateEmail_ValidEmail_ReturnsTrue()
        {
            // Arrange
            var sendMailHelper = new SendMailHelper
            {
                EmailTo = "test@example.com"
            };

            // Act
            sendMailHelper.ValidateEmail();

            // Assert
            Assert.True(sendMailHelper.MailIsValid);
        }

        [Fact]
        public void ValidateEmail_InvalidEmail_ReturnsFalse()
        {
            // Arrange
            var sendMailHelper = new SendMailHelper
            {
                EmailTo = "invalidemail"
            };

            // Act
            sendMailHelper.ValidateEmail();

            // Assert
            Assert.False(sendMailHelper.MailIsValid);
        }

        [Fact]
        public void SendMail_Successful()
        {
            // Arrange
            var sendMailHelper = new SendMailHelper
            {
                EmailTo = "recipient@example.com"
            };

            var smtpClientMock = new Mock<SmtpClient>("smtp.gmail.com", 587);
            smtpClientMock.SetupAllProperties();

            var mailMessage = new MailMessage();
            smtpClientMock.Setup(client => client.Send(mailMessage)).Verifiable();

            // Act
            sendMailHelper.SendMail();

            // Assert
            Assert.Equal("Mail Sent", sendMailHelper.Message);
        }

        [Fact]
        public void SendMail_Failure()
        {
            // Arrange
            var sendMailHelper = new SendMailHelper
            {
                EmailTo = "recipient@example.com"
            };

            var smtpClientMock = new Mock<SmtpClient>("smtp.gmail.com", 587);
            smtpClientMock.SetupAllProperties();

            var mailMessage = new MailMessage();
            smtpClientMock.Setup(client => client.Send(mailMessage)).Throws(new Exception("Failed to send mail"));

            // Act
            sendMailHelper.SendMail();

            // Assert
            Assert.Equal("Failed to send mail", sendMailHelper.Message);
        }
    }
}
