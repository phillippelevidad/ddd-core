using System.Threading.Tasks;

namespace Core
{
    public interface ICommandHandler
    {
        Task HandleAsync(ICommand command);
    }
}