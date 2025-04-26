using System;

using EventHelper;

namespace EventHelper.Adapters
{
    /// <summary>
    /// Universaladapter, um beliebige Controls mit einem Click- oder EventHandler-basierten Event
    /// an ein BulletproofWeakAction zu koppeln.
    /// </summary>
    /// <typeparam name="TControl">Typ des Controls (z.B. Button, ImageButton)</typeparam>
    public class BulletproofEventAdapter<TControl>
        where TControl : class
    {
        private readonly TControl _control;
        private BulletproofWeakAction<object?, EventArgs> _bulletproofEvent = new();
        private readonly Action<TControl, EventHandler> _subscribe;
        private readonly Action<TControl, EventHandler> _unsubscribe;
        private bool _isHooked;

        public BulletproofEventAdapter(
            TControl control,
            Action<TControl, EventHandler> subscribe,
            Action<TControl, EventHandler> unsubscribe)
        {
            _control = control ?? throw new ArgumentNullException(nameof(control));
            _subscribe = subscribe ?? throw new ArgumentNullException(nameof(subscribe));
            _unsubscribe = unsubscribe ?? throw new ArgumentNullException(nameof(unsubscribe));
        }

        /// <summary>
        /// Exponiertes Event, das bulletproof schwach angebunden wird.
        /// Mehrfachregistrierungen werden automatisch verhindert.
        /// </summary>
        public event EventHandler? Clicked
        {
            add
            {
                if (value != null && !_isHooked)
                {
                    _subscribe(_control, InternalHandler);
                    _bulletproofEvent += value.Invoke;
                    _isHooked = true;
                }
            }
            remove
            {
                if (value != null)
                {
                    _bulletproofEvent -= value.Invoke;
                    if (_bulletproofEvent.IsEmpty && _isHooked)
                    {
                        _unsubscribe(_control, InternalHandler);
                        _isHooked = false;
                    }
                }
            }
        }

        private void InternalHandler(object? sender, EventArgs e)
        {
            _bulletproofEvent.Invoke(sender, e);
        }
    }
}
