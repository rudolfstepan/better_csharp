
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EventHelper
{
    public abstract class BulletproofWeakEventBase<T> where T : Delegate
    {
        protected class WeakHandler
        {
            public readonly WeakReference? TargetRef;
            public readonly MethodInfo Method;

            public WeakHandler(T handler)
            {
                TargetRef = handler.Target != null ? new WeakReference(handler.Target) : null;
                Method = handler.Method;
            }

            public bool Matches(T handler)
            {
                var currentTarget = TargetRef?.Target;
                return handler.Method == Method && currentTarget == handler.Target;
            }

            public bool IsDead => TargetRef != null && TargetRef.Target == null;
        }

        protected readonly List<WeakHandler> _handlers = new();
        protected readonly object _lock = new();

        public static bool DebugLogging { get; set; } = false;

        public static BulletproofWeakEventBase<T> operator +(BulletproofWeakEventBase<T> e, T handler)
        {
            e.Register(handler);
            return e;
        }

        public static BulletproofWeakEventBase<T> operator -(BulletproofWeakEventBase<T> e, T handler)
        {
            e.Deregister(handler);
            return e;
        }

        protected void Register(T handler)
        {
            lock (_lock)
            {
                CleanupDeadHandlers();
                if (!_handlers.Any(h => h.Matches(handler)))
                {
                    if (DebugLogging)
                        Console.WriteLine($"[WeakEvent] Registriert: {handler.Method.DeclaringType?.Name}.{handler.Method.Name}");

                    _handlers.Add(new WeakHandler(handler));
                }
                else
                {
                    if (DebugLogging)
                        Console.WriteLine($"[WeakEvent] Handler bereits vorhanden: {handler.Method.DeclaringType?.Name}.{handler.Method.Name}");
                }
            }
        }

        protected void Deregister(T handler)
        {
            lock (_lock)
            {
                if (DebugLogging)
                    Console.WriteLine($"[WeakEvent] Deregistriert: {handler.Method.DeclaringType?.Name}.{handler.Method.Name}");

                _handlers.RemoveAll(h => h.Matches(handler));
            }
        }

        protected void CleanupDeadHandlers()
        {
            int removed = _handlers.RemoveAll(h => h.IsDead);

            if (DebugLogging && removed > 0)
                Console.WriteLine($"[WeakEvent] {removed} tote Handler entfernt.");
        }

        protected void InvokeHandlers(params object?[] parameters)
        {
            lock (_lock)
            {
                foreach (var handler in _handlers.ToList())
                {
                    try
                    {
                        if (!handler.IsDead)
                        {
                            var target = handler.TargetRef?.Target;
                            handler.Method.Invoke(target, parameters);
                        }
                    }
                    catch (Exception ex)
                    {
                        if (DebugLogging)
                            Console.WriteLine($"[WeakEvent] Fehler beim Aufruf: {ex.Message}");
                    }
                }

                CleanupDeadHandlers();
            }
        }
    }
}
