
namespace EventHelper
{
    public class BulletproofWeakAction<T> : BulletproofWeakEventBase<Action<T>>
    {
        public void Invoke(T arg)
        {
            InvokeHandlers(arg);
        }

        public static BulletproofWeakAction<T> operator +(BulletproofWeakAction<T> e, Action<T> handler)
        {
            e.Register(handler);
            return e;
        }

        public static BulletproofWeakAction<T> operator -(BulletproofWeakAction<T> e, Action<T> handler)
        {
            e.Deregister(handler);
            return e;
        }
    }
}
