using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using ThreadSafeHelperGenerator.Attributes;


namespace CodeGeneratorTestApp
{
    /// <summary>
    /// Testklasse für den ThreadSafe-Generator
    /// </summary>
    internal static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Test Running");

            // retryAttribute & fallback Test
            var retryService = new RetryServiceExamples();

            // combine retry and fallback
            retryService.UnreliableMethod_RetryFallback();

            Console.WriteLine("Retry/Fallback beendet.");


            Console.ReadLine();
        }

    }

    /// <summary>
    /// Testklasse beinhaltet Methoden, die durch den ThreadSafe-Generator umgewandelt werden.
    /// </summary>
    public partial class ThreadingServiceExamples
    {
        #region CacheAttributeTest
        [TimedExecution(300)]
        public static void Timer()
        {
            Timer_Implementation();
        }
        private static void Timer_Implementation()
        {
            CachedWork();
        }

        [Cache(5)]
        public static string CachedWork()
        {
            return CachedWork_Implementation();
        }

        private static string CachedWork_Implementation()
        {
            string s =  "CacheAttributeTest " + DateTime.Now;

            Console.WriteLine(s);
            return s;
        }
        #endregion

        /// <summary>
        /// Durch den ThreadSafe-Generator wird die Methode so umgewandelt, dass sie thread-sicher ist.
        /// Abhängig von den Parametern des Attributes wird die Methode entweder nur von einem Thread
        /// gleichzeitig ausgeführt oder mehrere Threads können gleichzeitig darauf zugreifen.
        /// Im Falle von SingleExecution wird die Methode nur einmal ausgeführt.
        /// Im Fall von Debounce wird die Methode nur ausgeführt, wenn sie für eine bestimmte Zeit nicht aufgerufen wurde.
        /// Im Fall von ReadWriteLock wird die Methode nur von einem Thread ausgeführt, während andere Threads warten.
        /// Im Fall von TimedExecution wird die Methode in regelmäßigen Abständen ausgeführt.
        /// Im Fall von ThreadSafe wird die Methode thread-sicher gemacht, indem ein Lock verwendet wird.
        /// In diesem Beispiel wird die Methode Work() durch das Attribut ThreadSafe(2) in eine thread-sichere Methode umgewandelt.
        /// Das bedeutet nur zwei Threads können gleichzeitig auf die Methode zugreifen.
        /// Alle anderen Threads warten, bis die Methode verfügbar ist.
        /// </summary>
        // wir benutzen die Originalmethode, um die Thread-Sicherheit zu testen
        [ThreadSafe(2, waitForAvailability: true)]
        public static void Work()
        {
            Work_Implementation();
        }

        private static void Work_Implementation()
        {
            Console.WriteLine($"Die Methode wird von Thread {Environment.CurrentManagedThreadId} ausgeführt.");
            Thread.Sleep(5000); // Simuliert eine langwierige Aufgabe
            Console.WriteLine($"Die Methode ist von Thread {Environment.CurrentManagedThreadId} fertig.");
        }

        /// <summary>
        /// Startet den Timer, der die Methode DoWork() in regelmäßigen Abständen ausführt.
        /// </summary>
        [TimedExecution(100)]
        public void DoWork()
        {
            DoWork_Implementation();
        }

        private void DoWork_Implementation()
        {
            Work_ThreadSafe();
        }
    }

    // Beispielklasse mit Methoden, die Retry und Fallback verwenden
    public partial class RetryServiceExamples
    {
        [Retry(3, 2000)]
        [Fallback(nameof(FallbackMethod_Implementation))]
        public bool UnreliableMethod()
        {
            return UnreliableMethod_Implementation();
        }

        private static bool UnreliableMethod_Implementation()
        {

            // Simulierte unzuverlässige Aktion
            Console.WriteLine("Attempting operation...");
            throw new OperationFailedException("Operation failed");
        }

        public class OperationFailedException(string message) : Exception(message)
        {
        }

        private static bool FallbackMethod_Implementation()
        {
            Console.WriteLine("Executing fallback method...");

            return false;
        }

        public static class RetryHelper
        {
            public static void Retry(Action action, Action fallback, int maxRetries, int delayMilliseconds)
            {
                int attempt = 0;
                while (true)
                {
                    try
                    {
                        action();
                        return;
                    }
                    catch
                    {
                        if (++attempt > maxRetries)
                        {
                            fallback?.Invoke();
                            throw;
                        }
                        Thread.Sleep(delayMilliseconds);
                    }
                }
            }

            public static T Retry<T>(Func<T> action, Func<T> fallback, int maxRetries, int delayMilliseconds)
            {
                int attempt = 0;
                while (true)
                {
                    try
                    {
                        return action();
                    }
                    catch
                    {
                        if (++attempt > maxRetries)
                        {
                            return fallback.Invoke();
                        }
                        Thread.Sleep(delayMilliseconds);
                    }
                }
            }
        }
    }
}