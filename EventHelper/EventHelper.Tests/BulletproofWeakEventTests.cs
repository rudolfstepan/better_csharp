using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

namespace EventHelper.Tests
{
    [TestClass]
    public class BulletproofWeakEventTests
    {
        private class TestSubscriber
        {
            public int Count { get; private set; }

            public void OnAction()
            {
                Count++;
            }

            public void OnActionWithParam(string value)
            {
                Count += value.Length;
            }

            public int OnFunc(int input)
            {
                return input * 2;
            }
        }

        [TestMethod]
        public void BulletproofWeakAction_InvokeOnce()
        {
            var evt = new BulletproofWeakAction();
            var subscriber = new TestSubscriber();

            evt += subscriber.OnAction;
            evt.Invoke();

            Assert.AreEqual(1, subscriber.Count);
        }

        [TestMethod]
        public void BulletproofWeakAction_PreventsDuplicate()
        {
            var evt = new BulletproofWeakAction();
            var subscriber = new TestSubscriber();

            evt += subscriber.OnAction;
            evt += subscriber.OnAction;

            evt.Invoke();

            Assert.AreEqual(1, subscriber.Count);
        }

        [TestMethod]
        public void BulletproofWeakActionT_InvokeOnce()
        {
            var evt = new BulletproofWeakAction<string>();
            var subscriber = new TestSubscriber();

            evt += subscriber.OnActionWithParam;
            evt.Invoke("Test");

            Assert.AreEqual(4, subscriber.Count);
        }

        [TestMethod]
        public void BulletproofWeakFunc_InvokeReturnsCorrectValue()
        {
            var evt = new BulletproofWeakFunc<int, int>();
            var subscriber = new TestSubscriber();

            evt += subscriber.OnFunc;
            int result = evt.Invoke(5);

            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public void BulletproofWeakAction_RemovesDeadHandlers()
        {
            var evt = new BulletproofWeakAction();

            void AddDeadHandler()
            {
                var subscriber = new TestSubscriber();
                evt += subscriber.OnAction;
            }

            AddDeadHandler();

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            evt.Invoke();

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void BulletproofWeakAction_DeregisterHandler()
        {
            var evt = new BulletproofWeakAction();
            var subscriber = new TestSubscriber();

            evt += subscriber.OnAction;
            evt -= subscriber.OnAction;

            evt.Invoke();

            Assert.AreEqual(0, subscriber.Count);
        }

        [TestMethod]
        public void BulletproofWeakAction_HandlesExceptions()
        {
            var evt = new BulletproofWeakAction();
            var subscriber = new TestSubscriber();

            evt += subscriber.OnAction;
            evt += () => throw new InvalidOperationException("Absichtlich!");

            evt.Invoke();

            Assert.AreEqual(1, subscriber.Count);
        }

        [TestMethod]
        public void BulletproofWeakAction_MassiveInvocationTest()
        {
            var evt = new BulletproofWeakAction();
            const int subscriberCount = 10000;
            var subscribers = new TestSubscriber[subscriberCount];

            for (int i = 0; i < subscriberCount; i++)
            {
                subscribers[i] = new TestSubscriber();
                evt += subscribers[i].OnAction;
            }

            evt.Invoke();

            for (int i = 0; i < subscriberCount; i++)
            {
                Assert.AreEqual(1, subscribers[i].Count, $"Subscriber {i} wurde nicht korrekt aufgerufen.");
            }
        }

        [TestMethod]
        public void BulletproofWeakAction_RapidMultipleInvocations()
        {
            var evt = new BulletproofWeakAction();
            var subscriber = new TestSubscriber();
            evt += subscriber.OnAction;

            const int invocationCount = 10000;

            for (int i = 0; i < invocationCount; i++)
            {
                evt.Invoke();
            }

            Assert.AreEqual(invocationCount, subscriber.Count);
        }
    }
}
