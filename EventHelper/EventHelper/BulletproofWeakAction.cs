
namespace EventHelper
{
    public class BulletproofWeakAction : BulletproofWeakEventBase<Action>
    {
        public void Invoke()
        {
            InvokeHandlers();
        }

        public static BulletproofWeakAction operator +(BulletproofWeakAction e, Action handler)
        {
            e.Register(handler);
            return e;
        }

        public static BulletproofWeakAction operator -(BulletproofWeakAction e, Action handler)
        {
            e.Deregister(handler);
            return e;
        }
    }
}
