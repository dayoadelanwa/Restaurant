namespace PaymentProcessor
{
    public class ProcessPayment : IProcessPayment
    {
        bool IProcessPayment.PaymentProcessor()
        {
            //implement custom logic and get card details etc
            return true;
        }
    }
}