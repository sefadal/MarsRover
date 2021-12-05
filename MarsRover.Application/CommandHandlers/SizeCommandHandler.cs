using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using MarsRover.Core.Domain.Aggregates;
using MarsRover.Core.Domain.Commands;
using MarsRover.Core.Domain.ValueTypes;
using System.Threading;
using System.Threading.Tasks;

namespace MarsRover.Application.CommandHandlers
{
    public class SizeCommandHandler : CommandHandler<LocationAggregate, Identity, IExecutionResult, CreateSizeCommand>
    {
        public override async Task<IExecutionResult> ExecuteCommandAsync(LocationAggregate aggregate, CreateSizeCommand command, CancellationToken cancellationToken)
        {
            IExecutionResult executionResult = aggregate.Initialize(command.SurfaceSizeInput);

            return await Task.FromResult(executionResult);
        }
    }
}