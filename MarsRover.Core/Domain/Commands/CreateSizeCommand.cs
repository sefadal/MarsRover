using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using MarsRover.Core.Domain.Aggregates;
using MarsRover.Core.Domain.ValueTypes;

namespace MarsRover.Core.Domain.Commands
{
    public class CreateSizeCommand : Command<LocationAggregate, Identity, IExecutionResult>
    {
        public CreateSizeCommand(Identity id, string surfaceSizeInput) : base(id)
        {
            SurfaceSizeInput = surfaceSizeInput;
        }

        public string SurfaceSizeInput { get; }
    }
}