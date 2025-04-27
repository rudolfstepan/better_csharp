// Ultimate Lightweight Event Bus System with Async Support, Topics, Filters, and Timestamps

using EventHelper;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventHelper
{
    public class EventEnvelope<T>
    {
        public T Payload { get; }
        public DateTimeOffset Timestamp { get; }

        internal EventEnvelope(T payload)
        {
            Payload = payload;
            Timestamp = DateTimeOffset.UtcNow;
        }
    }

    public enum EventAcknowledge
    {
        NotHandled,
        Handled
    }

    public class LightweightEventBusAsync
    {
        private readonly ConcurrentDictionary<Type, IEventGroup> _eventGroups = new();

        public void Subscribe<TEventArgs>(Func<EventEnvelope<TEventArgs>, Task<EventAcknowledge>> handler, Func<TEventArgs, bool>? filter = null)
        {
            var group = (EventGroup<TEventArgs>)_eventGroups.GetOrAdd(typeof(TEventArgs), _ => new EventGroup<TEventArgs>());
            group.Subscribe(handler, filter);
        }

        public void Unsubscribe<TEventArgs>(Func<EventEnvelope<TEventArgs>, Task<EventAcknowledge>> handler)
        {
            if (_eventGroups.TryGetValue(typeof(TEventArgs), out var group))
            {
                ((EventGroup<TEventArgs>)group).Unsubscribe(handler);
            }
        }

        public async Task<List<EventAcknowledge>> PublishAsync<TEventArgs>(TEventArgs args)
        {
            if (!_eventGroups.TryGetValue(typeof(TEventArgs), out var group))
                return new List<EventAcknowledge>();

            return await ((EventGroup<TEventArgs>)group).PublishAsync(new EventEnvelope<TEventArgs>(args));
        }

        private interface IEventGroup { }

        private class EventGroup<TEventArgs> : IEventGroup
        {
            private class Subscription
            {
                public Func<EventEnvelope<TEventArgs>, Task<EventAcknowledge>> Handler { get; }
                public Func<TEventArgs, bool>? Filter { get; }

                public Subscription(Func<EventEnvelope<TEventArgs>, Task<EventAcknowledge>> handler, Func<TEventArgs, bool>? filter)
                {
                    Handler = handler;
                    Filter = filter;
                }
            }

            private readonly ConcurrentDictionary<Guid, Subscription> _subscriptions = new();

            public void Subscribe(Func<EventEnvelope<TEventArgs>, Task<EventAcknowledge>> handler, Func<TEventArgs, bool>? filter = null)
            {
                _subscriptions.TryAdd(Guid.NewGuid(), new Subscription(handler, filter));
            }

            public void Unsubscribe(Func<EventEnvelope<TEventArgs>, Task<EventAcknowledge>> handler)
            {
                foreach (var entry in _subscriptions)
                {
                    if (entry.Value.Handler == handler)
                    {
                        _subscriptions.TryRemove(entry.Key, out _);
                        break;
                    }
                }
            }

            public async Task<List<EventAcknowledge>> PublishAsync(EventEnvelope<TEventArgs> envelope)
            {
                var tasks = _subscriptions.Values
                    .Where(sub => sub.Filter == null || sub.Filter(envelope.Payload))
                    .Select(sub => SafeInvoke(sub.Handler, envelope))
                    .ToList();

                var results = await Task.WhenAll(tasks);
                return new List<EventAcknowledge>(results);
            }

            private async Task<EventAcknowledge> SafeInvoke(Func<EventEnvelope<TEventArgs>, Task<EventAcknowledge>> handler, EventEnvelope<TEventArgs> envelope)
            {
                try
                {
                    return await handler.Invoke(envelope);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[LightweightEventBusAsync] Error during async Publish: {ex.Message}");
                    return EventAcknowledge.NotHandled;
                }
            }
        }
    }

    public static class EventBusExtensions
    {
        public static void SubscribeWithLogging<T>(
            this LightweightEventBusAsync bus,
            Func<EventEnvelope<T>, string>? messageBuilder = null)
            where T : class
        {
            bus.Subscribe<T>(envelope =>
            {
                var message = messageBuilder != null
                    ? messageBuilder(envelope)
                    : $"[Event Received] {typeof(T).Name}";

                Console.WriteLine(message);
                return Task.FromResult(EventAcknowledge.Handled);
            });
        }
    }

    // Example TopicMessage class
    public record TopicMessage(int TopicId);
}
