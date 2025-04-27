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
            private readonly Delegate? OpenDelegate;

            public WeakHandler(T handler)
            {
                TargetRef = handler.Target != null ? new WeakReference(handler.Target) : null;
                Method = handler.Method;

                if (handler.Target == null)
                {
                    OpenDelegate = handler;
                }
                else
                {
                    try
                    {
                        OpenDelegate = Delegate.CreateDelegate(typeof(T), handler.Target, Method);
                    }
                    catch
                    {
                        OpenDelegate = null;
                    }
                }
            }

            public bool Matches(T handler)
            {
                var currentTarget = TargetRef?.Target;
                return handler.Method == Method && currentTarget == handler.Target;
            }

            public bool IsDead => TargetRef != null && TargetRef.Target == null;

            public void Invoke(params object?[] parameters)
            {
                if (IsDead) return;

                try
                {
                    OpenDelegate?.DynamicInvoke(parameters);
                }
                catch (Exception ex)
                {
                    if (BulletproofWeakEventBase<T>.DebugLogging)
                        Console.WriteLine($"[WeakEvent] Fehler beim Invoke: {ex.Message}");
                }
            }
        }

        protected volatile List<WeakHandler> _handlers = new();
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

                    var newHandlers = new List<WeakHandler>(_handlers) { new WeakHandler(handler) };
                    _handlers = newHandlers;
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

                var newHandlers = _handlers.Where(h => !h.Matches(handler)).ToList();
                _handlers = newHandlers;
            }
        }

        protected void CleanupDeadHandlers()
        {
            var newHandlers = _handlers.Where(h => !h.IsDead).ToList();
            if (DebugLogging && newHandlers.Count != _handlers.Count)
                Console.WriteLine($"[WeakEvent] {_handlers.Count - newHandlers.Count} tote Handler entfernt.");
            _handlers = newHandlers;
        }

        protected void InvokeHandlers(params object?[] parameters)
        {
            List<WeakHandler> snapshot = _handlers;

            foreach (var handler in snapshot)
            {
                handler.Invoke(parameters);
            }
        }

        public bool IsEmpty
        {
            get
            {
                CleanupDeadHandlers();
                return _handlers.Count == 0;
            }
        }
    }
}
