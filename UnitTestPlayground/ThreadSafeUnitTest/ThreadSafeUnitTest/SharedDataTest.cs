using System;
using System.Collections.Concurrent;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ThreadSafetyTests
{
    [TestClass]
    public class SharedDataTest
    {
        private int _sharedValue;
        private object _lock = new object();
        private ConcurrentQueue<string> _messageQueue = new ConcurrentQueue<string>();

        // Badly designed method (not thread-safe, resource-intensive)
        [TestMethod]
        public void BadDesignMethod_CausesRaceCondition()
        {
            // Create threads to test race condition
            Thread[] threads = new Thread[10];
            for (int i = 0; i < 10; i++)
            {
                threads[i] = new Thread(BadDesignMethod);
                threads[i].Start();
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }
        }

        private void BadDesignMethod()
        {
            while (true)
            {
                int value = _sharedValue;
                if (_sharedValue > 10)
                {
                    break;
                }

                // Resource-intensive operation
                for (int i = 0; i < 10000000; i++)
                {
                    value++;
                }
                _sharedValue = value;
            }
        }

        // Well-designed method (thread-safe, resource-efficient)
        [TestMethod]
        public void GoodDesignMethod_ScalesOptimally()
        {
            // Create threads to test scaling
            Thread[] threads = new Thread[100];
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(GoodDesignMethod);
                threads[i].Start();
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            // Verify shared data has been updated correctly
            Assert.AreEqual(100000, _messageQueue.Count);
        }

        private void GoodDesignMethod()
        {
            int value = _sharedValue;

            // Resource-efficient operation
            for (int i = 0; i < 1000; i++)
            {
                _messageQueue.Enqueue($"Value {value}");
                value++;
            }
        }

        [TestMethod]
        public void SharedDataUpdate_RaisesErrorWhenNotInitialized()
        {
            try
            {
                // Simulate shared data update
                UpdateSharedData();
                Assert.Fail("Expected exception");
            }
            catch (Exception ex)
            {
                // Expected exception, but it should be a specific message indicating initialization failure
                Assert.AreEqual("Shared data not initialized", ex.Message);
            }

            // Initialize shared data before updating
            _sharedValue = 1;
            UpdateSharedData();
        }

        private void UpdateSharedData()
        {
            if (_sharedValue == 0)
            {
                throw new Exception("Shared data not initialized");
            }
            else
            {
                int value = _sharedValue;
                for (int i = 0; i < 10000000; i++)
                {
                    value++;
                }
                _sharedValue = value;
            }
        }


        [TestMethod]
        public void DeadlockScenario_DoesNotRaiseErrorWhenLockIsAcquired()
        {
            // Create threads that acquire and release locks in a specific order
            Thread[] threads = new Thread[]
            {
                new Thread(() => AcquireAndReleaseLock1(_lock)),
                new Thread(() => AcquireAndReleaseLock2(_lock))
            };

            // Start the threads
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i].Start();
            }

            // Wait for both threads to finish
            foreach (Thread thread in threads)
            {
                thread.Join();
            }
        }

        private void AcquireAndReleaseLock1(object lockObject)
        {
            lock (_lock)
            {
                // Simulate some work
                Thread.Sleep(100);
            }
        }

        private void AcquireAndReleaseLock2(object lockObject)
        {
            lock (lockObject) // Use the same lock object
            {
                // Simulate some work
                Thread.Sleep(100);
            }
        }


        [TestMethod]
        public void GoodDesignMethod_PreventsRaceConditions()
        {
            // Create threads to test concurrent access
            Thread[] threads = new Thread[10];
            for (int i = 0; i < 10; i++)
            {
                int count = i;
                threads[i] = new Thread(() => UpdateSharedData(count));
                threads[i].Start();
            }

            // Wait for all threads to finish
            foreach (Thread thread in threads)
                thread.Join();

            // Verify shared data has been updated correctly
            Assert.AreEqual(45, _sharedValue);
        }

        private void UpdateSharedData(int count)
        {
            lock (_lock)
            {
                _sharedValue += count;
            }
        }

    }
}
