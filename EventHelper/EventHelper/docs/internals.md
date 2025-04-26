# Internals of BulletproofWeakAction

- **WeakReference Targets**: We store both the target object and its method.
- **Cleanup**: On every invocation or registration, dead references are automatically removed.
- **Thread Safety**: Every internal list operation is protected by a `lock`.
- **Operator Overloading**: We use `+=` and `-=` operators to make the API feel natural.

## Lifespan

- A handler lives only as long as the target instance exists.
- If the subscriber is garbage collected, the event no longer references it.
