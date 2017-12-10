using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentContext.Domain.Commands;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.Handles;
using PaymentContext.Tests.Mocks;

namespace PaymentContext.Tests.Handles
{
    [TestClass]
    public class SubscriptionHandlerTests
    {
        [TestMethod]
        public void ShouldReturnErrorWhenDocumentExists()
        {
            var emailService = new FakeEmailService();
            var studentRepository = new FakeStudentRepository();
            var handler = new SubscriptionHandle(studentRepository, emailService);
            
            var command = new CreateBoletoSubscriptionCommand();
            command.FirstName = "Tony";
            command.LastName = "Stark";
            command.Document = "99999999999";
            command.Email = "tony@marvel.com";
            command.BoletoNumber = "1324254342";
            command.BarCode = "1234456245";
            command.PaymentNumber = "3134324";
            command.PaidDate = DateTime.Now;
            command.ExpireDate = DateTime.Now.AddMonths(1);
            command.Total = 60;
            command.TotalPaid = 60;
            command.Payer = "Empresas Stark";
            command.PayerDocument = "12345678910";
            command.PayerDocumentType = EDocumentType.CPF;
            command.PayerEmail = "vigadores@marvel.com";
            command.Street = "rua teste";
            command.City = "cidade teste";
            command.State = "estado teste";
            command.Country = "USA";
            command.ZipCode = "877659870";

            handler.Handle(command);

            Assert.AreEqual(false, handler.Valid);
        }
    }
}