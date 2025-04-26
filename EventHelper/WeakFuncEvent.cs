using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EventHelper
{
    public class WeakFuncEvent<T, TResult>
    {
        private class WeakFunc
        {
            public WeakReference TargetRef { get; }
            public MethodInfo Method { get; }

            public WeakFunc(Func<T, TResult> func)
            {
                TargetRef = new WeakReference(func.Target);
                Method = func.Method;
            }

            public (bool IsAlive, TResult? Result) Invoke(T arg)
            {
                var target = TargetRef.Target;
                if (target != null || Method.IsStatic)
                {
                    var result = Method.Invoke(target, new object[] { arg });
                    return (true, (TResult?)result);
                }
                return (false, default);
            }

            public bool Matches(Func<T, TResult> func)
            {
                return func.Method == Method && func.Target == TargetRef.Target;
            }
        }

        private readonly List<WeakFunc> _functions = new();
        private readonly object _lock = new();

        public static WeakFuncEvent<T, TResult> operator +(WeakFuncEvent<T, TResult> e, Func<T, TResult> func)
        {
            lock (e._lock)
            {
                if (!e._functions.Any(f => f.Matches(func)))
                {
                    e._functions.Add(new WeakFunc(func));
                }
            }
            return e;
        }

        public static WeakFuncEvent<T, TResult> operator -(WeakFuncEvent<T, TResult> e, Func<T, TResult> func)
        {
            lock (e._lock)
            {
                e._functions.RemoveAll(f => f.Matches(func));
            }
            return e;
        }

        public TResult? Invoke(T arg)
        {
            lock (_lock)
            {
                foreach (var f in _functions.ToList())
                {
                    var (isAlive, result) = f.Invoke(arg);
                    if (!isAlive)
                    {
                        _functions.Remove(f);
                    }
                    else
                    {
                        return result; // Rückgabewert des ersten gültigen Funktors
                    }
                }
            }

            return default;
        }
    }
}
