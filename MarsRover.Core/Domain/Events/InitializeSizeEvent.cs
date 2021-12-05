using EventFlow.Aggregates;
using EventFlow.EventStores;
using MarsRover.Core.Domain.Aggregates;
using MarsRover.Core.Domain.ValueTypes;

namespace MarsRover.Core.Domain.Events
{
    [EventVersion("InitializeSize", 1)]
    public class InitializeSizeEvent : AggregateEvent<LocationAggregate, Identity>
    {
        public InitializeSizeEvent(SurfaceSize surfaceSize)
        {
            Size = surfaceSize;
        }

        public SurfaceSize Size { get; set; }
    }
}