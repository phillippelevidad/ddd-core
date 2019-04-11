using MediatR;

namespace Core
{
    public interface ICommandHandler<TCommand> : INotificationHandler<TCommand> where TCommand : ICommand
    {
    }
}