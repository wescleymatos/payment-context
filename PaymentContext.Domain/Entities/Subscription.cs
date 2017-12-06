namespace PaymentContext.Domain.Entities
{
    public class Subscription
    {
        public Datetime CreateDate { get; set; }
        public Datetime LastUpdateDate { get; set; }
        public Datetime? ExpireDate { get; set; }
        public bool Active { get; set; }
        public List<Payment> Payments { get; set; }
    }
}