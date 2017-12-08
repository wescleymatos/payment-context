using System.Collections.Generic;
using System.Linq;
using PaymentContext.Domain.ValueObjects;

namespace PaymentContext.Domain.Entities
{
    public class Student
    {
        private IList<Subscription> _subscriptions;

        public Name Name { get; private set; }
        public Document Document { get; private set; }
        public Email Email { get; private set; }
        public string Address { get; private set; }
        public IReadOnlyCollection<Subscription> Subscriptions
        { 
            get => _subscriptions.ToArray();
        }

        public Student(Name name, Document document, Email email)
        {
            Name = name;
            Document = document;
            Email = email;
            _subscriptions = new List<Subscription>();
        }

        public void AddSubscription(Subscription subscription)
        {
            foreach (var item in Subscriptions)
                item.Inactivate();

            _subscriptions.Add(subscription);
        }
    }
}