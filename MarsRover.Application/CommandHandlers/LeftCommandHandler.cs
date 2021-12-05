using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using MarsRover.Core.Domain.Aggregates;
using MarsRover.Core.Domain.Commands;
using MarsRover.Core.Domain.ValueTypes;
using System.Threading;
using System.Threading.Tasks;

namespace MarsRover.Application.CommandHandlers
{
    public class LeftCommandHandler : CommandHandler<RoverAggregate, Identity, IExecutionResult, LeftCommand>
    {
        public override async Task<IExecutionResult> ExecuteCommandAsync(RoverAggregate aggregate, LeftCommand command, CancellationToken cancellationToken)
        {
            IExecutionResult executionResult = aggregate.TurnLeft();

            return await Task.FromResult(executionResult);
        }
    }
}