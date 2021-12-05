using EventFlow;
using EventFlow.Aggregates;
using EventFlow.Commands;
using EventFlow.Extensions;
using MarsRover.Application;
using MarsRover.Core.Domain.Aggregates;
using MarsRover.Core.Domain.Commands;
using MarsRover.Core.Domain.ValueTypes;
using MarsRover.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Threading;

namespace MarsRover
{
    static class Program
    {
        static void Main(string[] args)
        {
            var roverIdList = new List<Identity>();

            using (var resolver = EventFlowOptions.New
                                                  .RegisterModule<RoverModule>()
                                                  .UseNullLog()
                                                  .CreateResolver())
            {
                Helpers.RootResolver = resolver;

                var commandBus = resolver.Resolve<ICommandBus>();

                Console.WriteLine("Size :");

                var sizeInput = Console.ReadLine();

                var sizeAggregateId = Identity.New;
                var createsizeSurfaceCommand = new CreateSizeCommand(sizeAggregateId, sizeInput);

                var commands = new List<ICommand>
                {
                    createsizeSurfaceCommand,
                };

                while (true)
                {
                    Console.WriteLine("Location :");
                    string roverPositionInput = Console.ReadLine();

                    Console.WriteLine("Command :");
                    var roverCommandInput = Console.ReadLine();

                    var roverId = Identity.New;
                    roverIdList.Add(roverId);

                    var deployRoverCommand = new DeployRoverCommand(roverId, sizeAggregateId, roverPositionInput);

                    commands.Add(deployRoverCommand);
                    commands.AddRange(roverCommandInput.ToRoverCommands(roverId));

                    Console.WriteLine("Add new rover ? (Y/N)");
                    var addRoverInput = Console.ReadLine();

                    if (!addRoverInput.Equals("Y", StringComparison.InvariantCultureIgnoreCase))
                    {
                        break;
                    }
                }

                commandBus
                    .PublishMultipleAsync(commands.ToArray())
                    .GetAwaiter()
                    .GetResult();

                Console.WriteLine("Expected Output :");

                IAggregateStore aggregateStore = resolver.Resolve<IAggregateStore>();

                foreach (Identity roverId in roverIdList)
                {
                    var rover = aggregateStore.LoadAsync<RoverAggregate, Identity>(roverId, CancellationToken.None).Result;

                    Console.WriteLine($"{rover.RoverPosition.X} " +
                      $"{rover.RoverPosition.Y} " +
                      $"{rover.RoverPosition.Orientation}");
                }

                Console.ReadLine();
            }
        }
    }
}