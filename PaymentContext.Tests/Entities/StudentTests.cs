using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.ValueObjects;

namespace PaymentContext.Entities.Tests
{
    [TestClass]
    public class StudentTets
    {
        private readonly Student _student;
        private readonly Subscription _subscription;
        private readonly Name _name;
        private readonly Email _email;
        private readonly Document _document;
        private readonly Address _address;

        public StudentTets()
        {
            _name = new Name("Tony", "Stark");
            _document = new Document("99927286392", EDocumentType.CPF);
            _email = new Email("tony@marvel.com");
            _address = new Address("new street", "nova york", "Nova York", "USA", "4565757");
            _student = new Student(_name, _document, _email);
            _subscription = new Subscription(null);
        }

        [TestMethod]
        public void ShouldReturnErrorWhenHadActiveSubscription()
        {
            
            var payment = new PayPalPayment("2535136357357357", DateTime.Now, DateTime.Now.AddDays(5), 10, 10, _document, "Empresas Stark", _address, _email);

            _subscription.AddPayments(payment);
            _student.AddSubscription(_subscription);
            _student.AddSubscription(_subscription);
            
            Assert.IsTrue(_student.Invalid);
        }

        [TestMethod]
        public void ShouldReturnErrorWhenSubscriptionHasNoPayment()
        {
            _student.AddSubscription(_subscription);
            
            Assert.IsTrue(_student.Invalid);
        }

        [TestMethod]
        public void ShouldReturnSuccessWhenAddSubscriptionHasPayment()
        {
            var payment = new PayPalPayment("2535136357357357", DateTime.Now, DateTime.Now.AddDays(5), 10, 10, _document, "Empresas Stark", _address, _email);

            _subscription.AddPayments(payment);
            _student.AddSubscription(_subscription);
            
            Assert.IsTrue(_student.Valid);
        }
    }
}