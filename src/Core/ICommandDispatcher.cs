using System.Threading.Tasks;

namespace Core
{
    public interface ICommandDispatcher
    {
        Task DispatchAsync(ICommand command);
    }
}