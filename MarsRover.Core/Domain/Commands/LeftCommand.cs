using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using MarsRover.Core.Domain.Aggregates;
using MarsRover.Core.Domain.ValueTypes;

namespace MarsRover.Core.Domain.Commands
{
    public class LeftCommand : Command<RoverAggregate, Identity, IExecutionResult>
    {
        public LeftCommand(Identity id) : base(id)
        { }
    }
}