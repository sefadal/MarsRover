using EventFlow;
using EventFlow.Configuration;
using EventFlow.Extensions;
using MarsRover.Application.CommandHandlers;
using MarsRover.Core.Domain.Commands;

namespace MarsRover.Application
{
    public class RoverModule : IModule
    {
        public void Register(IEventFlowOptions eventFlowOptions)
        {
            eventFlowOptions.AddDefaults(typeof(SizeCommandHandler).Assembly);
            eventFlowOptions.AddDefaults(typeof(CreateSizeCommand).Assembly);
        }
    }
}