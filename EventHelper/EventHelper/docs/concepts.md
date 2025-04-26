# Concepts - Why EventHelper?

## Why Traditional Events Are Fragile

In classic C#/.NET:
- Events hold **strong references** to their subscribers.
- Subscribers are often forgotten when a page, viewmodel, or control is disposed.
- This leads to **memory leaks**, dangling events, or multiple invocations per user action.

## The Bulletproof Concept

**EventHelper** solves this elegantly:
- Every event handler is wrapped in a **WeakReference**.
- Duplicate handlers are automatically detected and prevented.
- Dead handlers are cleaned up before every invocation.
- Registration and deregistration happen transparently.

**Key Goals:**
- Safe event handling without developer babysitting.
- Full integration into existing event patterns (`+=`, `-=` syntax).
- Maximum runtime efficiency.
