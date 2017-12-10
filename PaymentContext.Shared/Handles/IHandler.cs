using PaymentContext.Shared.Commands;

namespace PaymentContext.Shared.Handles
{
    public interface IHandler<T> where T : ICommand
    {
         ICommandResult Handle(T command);
    }
}