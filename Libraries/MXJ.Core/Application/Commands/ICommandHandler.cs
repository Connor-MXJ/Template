using MXJ.Core.Bus.Message;
using MXJ.Core.Domain.Handlers;

namespace MXJ.Core.Application.Commands
{
    /// <summary>
    /// Represents that the implemented classes are command handlers.
    /// </summary>
    /// <typeparam name="TCommand">The type of the command to be handled.</typeparam>
    [RegisterDispatch]
    public interface ICommandHandler<TCommand> : IHandler<TCommand>
        where TCommand : ICommand
    {

    }
}
