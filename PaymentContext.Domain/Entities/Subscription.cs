namespace PaymentContext.Domain.Entities
{
    public class Subscription
    {
        private IList<Payment> _payments;

        public Datetime CreateDate { get; private set; }
        public Datetime LastUpdateDate { get; private set; }
        public Datetime? ExpireDate { get; private set; }
        public bool Active { get; private set; }
        public IReadOnlyCollection<Payment> Payments 
        { 
            get
            {
                return _payments.ToArray();
            }
        }
        

        public Subscription(Datetime? expireDate)
        {
            CreateDate = Datetime.Now;
            LastUpdateDate = Datetime.Now;
            ExpireDate = expireDate;
            Active = true;
            _payments = new List<Payment>();
        }

        public void AddPayments(Payment payment)
        {
            _payments.Add(payment);
        }

        public void Activate()
        {
            Active = true;
            LastUpdateDate = Datetime.Now;
        }

        public void Inactivate()
        {
            Active = false;
            LastUpdateDate = Datetime.Now;
        }
    }
}