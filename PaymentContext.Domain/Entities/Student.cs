using System.Collections.Generic;
using System.Linq;

namespace PaymentContext.Domain.Entities
{
    public class Student
    {
        private IList<Subscription> _subscriptions;

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Document { get; private set; }
        public string Email { get; private set; }
        public string Address { get; private set; }
        public IReadOnlyCollection<Subscription> Subscriptions
        { 
            get
            {
                return _subscriptions.ToArray();
            }
        }

        public Student(string firstName, string lastname, string document, string email)
        {
            FirstName = firstName;
            LastName = lastname;
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