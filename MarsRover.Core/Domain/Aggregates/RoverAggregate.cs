using EventFlow.Aggregates;
using EventFlow.Aggregates.ExecutionResults;
using MarsRover.Core.Domain.Enums;
using MarsRover.Core.Domain.Events;
using MarsRover.Core.Domain.ValueTypes;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MarsRover.Core.Domain.Aggregates
{
    public class RoverAggregate : AggregateRoot<RoverAggregate, Identity>
    {
        public RoverPosition RoverPosition { get; private set; }
        public Identity SizeSurfaceId { get; private set; }

        public RoverAggregate(Identity id) : base(id)
        { }

        #region Aggregate methods

        public IExecutionResult DeployRover(string roverPositionInput, Identity sizeSurfaceId)
        {
            RoverPosition roverPosition = ParsePosition(roverPositionInput);

            Emit(new DeployRoverEvent(roverPosition, sizeSurfaceId));

            return ExecutionResult.Success();
        }

        public IExecutionResult TurnLeft()
        {
            Emit(new MoveRoverEvent(Movement.L));

            return ExecutionResult.Success();
        }

        public IExecutionResult TurnRight()
        {
            Emit(new MoveRoverEvent(Movement.R));

            return ExecutionResult.Success();
        }

        public IExecutionResult MoveAsync()
        {
            MoveRoverAsync().ConfigureAwait(false).GetAwaiter().GetResult();

            Emit(new MoveRoverEvent(Movement.M));

            return ExecutionResult.Success();
        }

        #endregion

        #region Apply methods

        public void Apply(DeployRoverEvent aggregateEvent)
        {
            this.RoverPosition = aggregateEvent.RoverPosition;
            this.SizeSurfaceId = aggregateEvent.SizeSurfaceId;
        }

        public void Apply(MoveRoverEvent aggregateEvent)
        {
            switch (aggregateEvent.MoveRover)
            {
                case Movement.L:
                    TurnLeftRover();
                    break;

                case Movement.R:
                    TurnRightRover();
                    break;

                case Movement.M:
                    MoveRoverAsync().GetAwaiter().GetResult();
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(aggregateEvent.MoveRover), aggregateEvent.MoveRover, null);
            }
        }

        #endregion

        #region Private methods

        private RoverPosition ParsePosition(string roverPositionInput)
        {
            var roverPositionArray = roverPositionInput.Split(' ');

            if (roverPositionArray.Length == 3)
            {
                string orientation = roverPositionArray[2].ToUpper();

                if (orientation.Equals("N", StringComparison.InvariantCultureIgnoreCase) ||
                    orientation.Equals("S", StringComparison.InvariantCultureIgnoreCase) ||
                    orientation.Equals("E", StringComparison.InvariantCultureIgnoreCase) ||
                    orientation.Equals("W", StringComparison.InvariantCultureIgnoreCase))
                {
                    RoverPosition roverPosition = new RoverPosition()
                    {
                        Orientation = (Orientation)Enum.Parse(typeof(Orientation), orientation),
                        X = int.Parse(roverPositionArray[0]),
                        Y = int.Parse(roverPositionArray[1])
                    };

                    return roverPosition;
                }
            }

            return null;
        }

        private async Task<bool> IsRoverInsideBoundariesAsync()
        {
            IAggregateStore aggregateStore = Helpers.Helpers.RootResolver.Resolve<IAggregateStore>();
            LocationAggregate sizeAggregate = await aggregateStore.LoadAsync<LocationAggregate, Identity>(this.SizeSurfaceId, CancellationToken.None);

            if (RoverPosition.X > sizeAggregate.Size.Width || RoverPosition.X < 0 || RoverPosition.Y > sizeAggregate.Size.Height || RoverPosition.Y < 0)
            {
                return false;
            }

            return true;
        }

        private void TurnRightRover()
        {
            this.RoverPosition.Orientation = (this.RoverPosition.Orientation + 1) > Orientation.W ? Orientation.N : this.RoverPosition.Orientation + 1;
        }

        private void TurnLeftRover()
        {
            this.RoverPosition.Orientation = (this.RoverPosition.Orientation - 1) < Orientation.N ? Orientation.W : this.RoverPosition.Orientation - 1;
        }

        private async Task MoveRoverAsync()
        {
            int roverX = this.RoverPosition.X;
            int roverY = this.RoverPosition.Y;

            switch (this.RoverPosition.Orientation)
            {
                case Orientation.N:
                    this.RoverPosition.Y++;
                    break;

                case Orientation.S:
                    this.RoverPosition.Y--;
                    break;
                case Orientation.W:
                    this.RoverPosition.X--;
                    break;

                case Orientation.E:
                    this.RoverPosition.X++;
                    break;

                default:
                    throw new InvalidOperationException();
            }

            if (!await IsRoverInsideBoundariesAsync())
            {
                this.RoverPosition.X = roverX;
                this.RoverPosition.Y = roverY;
                Console.WriteLine();
            }
        }

        #endregion
    }
}