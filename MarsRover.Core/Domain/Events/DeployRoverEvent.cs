﻿using EventFlow.Aggregates;
using EventFlow.EventStores;
using MarsRover.Core.Domain.Aggregates;
using MarsRover.Core.Domain.ValueTypes;

namespace MarsRover.Core.Domain.Events
{
    [EventVersion("DeployRover", 1)]
    public class DeployRoverEvent : AggregateEvent<RoverAggregate, Identity>
    {
        public DeployRoverEvent(RoverPosition roverPosition, Identity sizeSurfaceId)
        {
            RoverPosition = roverPosition;
            SizeSurfaceId = sizeSurfaceId;
        }

        public RoverPosition RoverPosition { get; }
        public Identity SizeSurfaceId { get; }
    }
}