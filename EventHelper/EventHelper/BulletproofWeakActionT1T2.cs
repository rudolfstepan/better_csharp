using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventHelper
{
    public class BulletproofWeakAction<T1, T2> : BulletproofWeakEventBase<Action<T1, T2>>
    {
        public void Invoke(T1 arg1, T2 arg2)
        {
            InvokeHandlers(arg1, arg2);
        }

        public static BulletproofWeakAction<T1, T2> operator +(BulletproofWeakAction<T1, T2> e, Action<T1, T2> handler)
        {
            e.Register(handler);
            return e;
        }

        public static BulletproofWeakAction<T1, T2> operator -(BulletproofWeakAction<T1, T2> e, Action<T1, T2> handler)
        {
            e.Deregister(handler);
            return e;
        }
    }
}
