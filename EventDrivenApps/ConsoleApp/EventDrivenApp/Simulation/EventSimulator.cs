using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using EventHelper;

namespace EventDrivenApp.Simulation
{
    public class EventSimulator
    {
        private readonly LightweightEventBusAsync _bus;
        private readonly Random _random = new();

        public EventSimulator(LightweightEventBusAsync bus)
        {
            _bus = bus;
        }

        public async Task SimulateEventAsync<T>(T @event, int delayMs = 0)
        {
            if (delayMs > 0)
                await Task.Delay(delayMs);

            await _bus.PublishAsync(@event);
            Console.WriteLine($"Simulated Event: {@event.GetType().Name}");
        }

        public async Task SimulateChainAsync(IEnumerable<object> events, int delayBetweenEventsMs = 0)
        {
            foreach (var e in events)
            {
                await SimulateEventAsync(e, delayBetweenEventsMs);
            }
        }

        public async Task SimulateRandomEventsAsync(IEnumerable<object> possibleEvents, int count, int maxDelayMs = 0)
        {
            var eventList = possibleEvents.ToList();

            for (int i = 0; i < count; i++)
            {
                var randomEvent = eventList[_random.Next(eventList.Count)];
                int delay = maxDelayMs > 0 ? _random.Next(maxDelayMs) : 0;
                await SimulateEventAsync(randomEvent, delay);
            }
        }
    }
}