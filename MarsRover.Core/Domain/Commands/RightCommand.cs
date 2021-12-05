using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using MarsRover.Core.Domain.Aggregates;
using MarsRover.Core.Domain.ValueTypes;

namespace MarsRover.Core.Domain.Commands
{
    public class RightCommand : Command<RoverAggregate, Identity, IExecutionResult>
    {
        public RightCommand(Identity id) : base(id)
        { }
    }
}