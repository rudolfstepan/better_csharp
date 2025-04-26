# Frequently Asked Questions

## Does BulletproofEventAdapter slow down my app?

No. The overhead is negligible, especially compared to the cost of memory leaks or duplicated event firing.

## Is BulletproofEventAdapter compatible with MAUI Hot Reload?

Yes. No limitations for debugging or reloading views.

## Can I use BulletproofEventAdapter outside of MAUI?

Absolutely. Any .NET event that uses `EventHandler` can benefit from it.

## What if the event uses custom EventArgs?

Currently only `EventHandler` (object?, EventArgs) signatures are supported, but extensions are planned.
