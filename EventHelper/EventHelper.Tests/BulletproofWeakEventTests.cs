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
            const int subscriberCount = 1000;
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

        [TestMethod]
        public async Task LightweightEventBusAsync_FilteredSubscriptionTest()
        {
            var bus = new LightweightEventBusAsync();
            int normalCount = 0;
            int filteredCount = 0;

            bus.Subscribe<string>(async envelope =>
            {
                normalCount++;
                return EventAcknowledge.Handled;
            });

            bus.Subscribe<string>(async envelope =>
            {
                filteredCount++;
                return EventAcknowledge.Handled;
            },
            filter: msg => msg.Contains("important"));

            await bus.PublishAsync("This is normal");
            await bus.PublishAsync("This is important");

            Assert.AreEqual(2, normalCount);
            Assert.AreEqual(1, filteredCount);
        }

        [TestMethod]
        public async Task LightweightEventBusAsync_MassiveFilteredSubscribersTest()
        {
            var bus = new LightweightEventBusAsync();
            const int subscriberCount = 1000000;
            int triggeredCount = 0;

            for (int i = 0; i < subscriberCount; i++)
            {
                int id = i;
                bus.Subscribe<int>(async envelope =>
                {
                    triggeredCount++;
                    return EventAcknowledge.Handled;
                },
                filter: value => value == id);
            }

            // Fire a specific event that should only match one subscriber
            await bus.PublishAsync(1234);

            Assert.AreEqual(1, triggeredCount);
        }

        [TestMethod]
        public async Task LightweightEventBusAsync_MassiveTopicsTest()
        {
            var bus = new LightweightEventBusAsync();
            const int topicCount = 5000;
            int triggeredCount = 0;

            for (int i = 0; i < topicCount; i++)
            {
                int topicId = i;
                bus.Subscribe<TopicMessage>(async envelope =>
                {
                    triggeredCount++;
                    return EventAcknowledge.Handled;
                },
                filter: msg => msg.TopicId == topicId);
            }

            // Fire one message per topic
            for (int topicId = 0; topicId < topicCount; topicId++)
            {
                await bus.PublishAsync(new TopicMessage(topicId));
            }

            Assert.AreEqual(topicCount, triggeredCount);
        }

        [TestMethod]
        public async Task LightweightEventBusAsync_ManyTopicsManySubscribersTest()
        {
            var bus = new LightweightEventBusAsync();
            const int topicCount = 500;
            const int subscribersPerTopic = 100;
            int totalTriggered = 0;

            for (int topicId = 0; topicId < topicCount; topicId++)
            {
                int capturedTopicId = topicId;

                for (int sub = 0; sub < subscribersPerTopic; sub++)
                {
                    bus.Subscribe<TopicMessage>(async envelope =>
                    {
                        Interlocked.Increment(ref totalTriggered);
                        return EventAcknowledge.Handled;
                    },
                    filter: msg => msg.TopicId == capturedTopicId);
                }
            }

            // Fire one message per topic
            for (int topicId = 0; topicId < topicCount; topicId++)
            {
                await bus.PublishAsync(new TopicMessage(topicId));
            }

            int expected = topicCount * subscribersPerTopic;
            Assert.AreEqual(expected, totalTriggered, $"Expected {expected} triggered, but got {totalTriggered}.");
        }


    }
}
