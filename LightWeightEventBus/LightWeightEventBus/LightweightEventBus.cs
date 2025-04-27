using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace LightweightEventBus
{
    public class LightweightEventBus
    {
        private readonly ConcurrentDictionary<Type, IEventGroup> _eventGroups = new();

        public void Subscribe<TEventArgs>(Action<TEventArgs> handler)
        {
            var group = (EventGroup<TEventArgs>)_eventGroups.GetOrAdd(typeof(TEventArgs), _ => new EventGroup<TEventArgs>());
            group.Subscribe(handler);
        }

        public void Unsubscribe<TEventArgs>(Action<TEventArgs> handler)
        {
            if (_eventGroups.TryGetValue(typeof(TEventArgs), out var group))
            {
                ((EventGroup<TEventArgs>)group).Unsubscribe(handler);
            }
        }

        public void Publish<TEventArgs>(TEventArgs args)
        {
            if (_eventGroups.TryGetValue(typeof(TEventArgs), out var group))
            {
                ((EventGroup<TEventArgs>)group).Publish(args);
            }
        }

        private interface IEventGroup { }

        private class EventGroup<TEventArgs> : IEventGroup
        {
            private readonly List<WeakReference<Action<TEventArgs>>> _handlers = new();
            private readonly object _lock = new();

            public void Subscribe(Action<TEventArgs> handler)
            {
                lock (_lock)
                {
                    _handlers.Add(new WeakReference<Action<TEventArgs>>(handler));
                }
            }

            public void Unsubscribe(Action<TEventArgs> handler)
            {
                lock (_lock)
                {
                    _handlers.RemoveAll(wr =>
                    {
                        return wr.TryGetTarget(out var existingHandler) && existingHandler == handler;
                    });
                }
            }

            public void Publish(TEventArgs args)
            {
                List<Action<TEventArgs>> toInvoke = new();
                lock (_lock)
                {
                    _handlers.RemoveAll(wr => !wr.TryGetTarget(out _));
                    foreach (var wr in _handlers)
                    {
                        if (wr.TryGetTarget(out var handler))
                        {
                            toInvoke.Add(handler);
                        }
                    }
                }

                foreach (var handler in toInvoke)
                {
                    try
                    {
                        handler.Invoke(args);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[LightweightEventBus] Fehler beim Publish: {ex.Message}");
                    }
                }
            }
        }
    }
}
