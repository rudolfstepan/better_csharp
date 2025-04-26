using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EventHelper
{
    public class WeakSingleActionEvent<T>
    {
        private class WeakAction
        {
            public WeakReference TargetRef { get; }
            public MethodInfo Method { get; }

            public WeakAction(Action<T> action)
            {
                TargetRef = new WeakReference(action.Target);
                Method = action.Method;
            }

            public bool Invoke(T arg)
            {
                var target = TargetRef.Target;
                if (target != null || Method.IsStatic)
                {
                    Method.Invoke(target, new object[] { arg });
                    return true;
                }
                return false;
            }

            public bool Matches(Action<T> action)
            {
                return action.Method == Method && action.Target == TargetRef.Target;
            }

            public bool MatchesTarget(object? target)
            {
                return TargetRef.Target == target;
            }
        }

        private readonly List<WeakAction> _actions = new();
        private readonly object _lock = new();

        public static WeakSingleActionEvent<T> operator +(WeakSingleActionEvent<T> e, Action<T> action)
        {
            lock (e._lock)
            {
                var target = action.Target;

                // Bereits ein Handler desselben Objekts registriert?
                if (target != null && e._actions.Any(a => a.MatchesTarget(target)))
                    return e;

                e._actions.Add(new WeakAction(action));
            }
            return e;
        }

        public static WeakSingleActionEvent<T> operator -(WeakSingleActionEvent<T> e, Action<T> action)
        {
            lock (e._lock)
            {
                e._actions.RemoveAll(a => a.Matches(action));
            }
            return e;
        }

        public void Invoke(T arg)
        {
            lock (_lock)
            {
                _actions.RemoveAll(a => !a.Invoke(arg));
            }
        }
    }

}
