using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EventHelper
{
    public class WeakActionEvent<T>
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
        }

        private readonly List<WeakAction> _actions = new();
        private readonly object _lock = new();

        public static WeakActionEvent<T> operator +(WeakActionEvent<T> e, Action<T> action)
        {
            lock (e._lock)
            {
                if (!e._actions.Any(a => a.Matches(action)))
                {
                    e._actions.Add(new WeakAction(action));
                }
            }
            return e;
        }

        public static WeakActionEvent<T> operator -(WeakActionEvent<T> e, Action<T> action)
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
