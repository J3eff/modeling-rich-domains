using PaymentContext.Shered.Commands;

namespace PaymentContext.Shared.Handlers
{
    public interface IHandler<T> where T : ICommand
    {
        ICommandResult Handle(T command);
    }

}