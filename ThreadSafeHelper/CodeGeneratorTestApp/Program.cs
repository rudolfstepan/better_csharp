using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using ThreadSafeHelperGenerator.Attributes;


namespace CodeGeneratorTestApp
{
    /// <summary>
    /// Testklasse für den ThreadSafe-Generator
    /// </summary>
    class Program
    {
        static async Task Main(string[] args)
        {
            // -----------------------------------
            // 1. ThreadSafeAttributeTests
            // -----------------------------------
            Console.WriteLine("ThreadSafeAttributeTests");
            Console.WriteLine("");
            var threadSafeAttributeTests = new ThreadSafeAttributeTests();

            // Die Methode wird von mehreren Threads aufgerufen
            //Erstelle mehrere Threads
            Thread[] threads = new Thread[10];
            for (int i = 0; i < threads.Length; i++)
            {
                // Stelle sicher, dass die vom source code generierte Methode aufgerufen wird
                threads[i] = new Thread(threadSafeAttributeTests.ThreadSafeMethod_ThreadSafe);
                threads[i].Start();
            }

            // Warte darauf, dass alle Threads beendet sind
            foreach (var thread in threads)
                thread.Join();

            Console.WriteLine("ThreadSafeAttributeTests beendet. All Threads finished.");


            // -----------------------------------
            // 2. SingleExecutionAttributeTests
            // -----------------------------------
            Console.WriteLine("");
            Console.WriteLine("SingleExecutionAttributeTests");
            Console.WriteLine("");
            var singleExecutionAttributeTests = new SingleExecutionAttributeTests();

            singleExecutionAttributeTests.SingleExecutionMethod_SingleExecution();
            Console.WriteLine("SingleExecutionAttributeTests beendet.");

            // -----------------------------------
            // 3. DebounceAttributeTests
            // -----------------------------------
            Console.WriteLine("");
            Console.WriteLine("DebounceAttributeTests");
            Console.WriteLine("");
            var debounceAttributeTests = new DebounceAttributeTests();

            // Erstelle mehrere Threads
            threads = new Thread[2];
            for (int i = 0; i < threads.Length; i++)
            {
                // Stelle sicher, dass die vom source code generierte Methode aufgerufen wird
                threads[i] = new Thread(debounceAttributeTests.DebounceMethod_Debounce);
                threads[i].Start();
            }
            foreach (var thread in threads)
                thread.Join();

            Console.WriteLine("DebounceAttributeTests beendet. All Threads finished.");

            // -----------------------------------
            // 4. ReadWriteLockAttributeTests
            // -----------------------------------
            Console.WriteLine("");
            Console.WriteLine("ReadWriteLockAttributeTests");
            Console.WriteLine("");
            var readWriteLockAttributeTests = new ReadWriteLockAttributeTests();
            readWriteLockAttributeTests.ReadWriteLockMethod_ReadWriteLock();
            Console.WriteLine("ReadWriteLockAttributeTests beendet.");

            // -----------------------------------
            // 5. TimedExecutionAttributeTests
            // -----------------------------------
            Console.WriteLine("");
            Console.WriteLine("TimedExecutionAttributeTests");
            Console.WriteLine("");
            var timedExecutionAttributeTests = new TimedExecutionAttributeTests();

            timedExecutionAttributeTests.StartTimedExecutionMethodTimer();

            await Task.Delay(1000);

            timedExecutionAttributeTests.StopTimedExecutionMethodTimer();
            Console.WriteLine("TimedExecutionAttributeTests beendet.");

            // -----------------------------------
            // 6. CacheAttributeTests
            // -----------------------------------
            Console.WriteLine("");
            Console.WriteLine("CacheAttributeTests");
            Console.WriteLine("");
            var cacheAttributeTests = new CacheAttributeTests();

            // die Methode wird mehrmals aufgerufen un der Cache wird genutzt
            Console.WriteLine(cacheAttributeTests.CacheMethod_Cached());
            Console.WriteLine(cacheAttributeTests.CacheMethod_Cached());
            Console.WriteLine(cacheAttributeTests.CacheMethod_Cached());

            // warte 5 Sekunden, damit der Cache abläuft
            Console.WriteLine("Warte 5 Sekunden, damit der Cache abläuft");
            await Task.Delay(5000);

            Console.WriteLine(cacheAttributeTests.CacheMethod_Cached());
            Console.WriteLine(cacheAttributeTests.CacheMethod_Cached());
            Console.WriteLine(cacheAttributeTests.CacheMethod_Cached());

            // -----------------------------------
            // 7. RetryAttributeTests
            // -----------------------------------
            Console.WriteLine("");
            Console.WriteLine("RetryAttributeTests");
            Console.WriteLine("");
            var retryAttributeTests = new RetryAttributeTests();
            Console.WriteLine(retryAttributeTests.RetryMethod_Retry());

            // -----------------------------------
            // 8. FallbackAttributeTests
            // -----------------------------------
            Console.WriteLine("");
            Console.WriteLine("FallbackAttributeTests");
            Console.WriteLine("");
            var fallbackAttributeTests = new FallbackAttributeTests();
            fallbackAttributeTests.FallbackMethod_Fallback();
            Console.WriteLine("FallbackAttributeTests beendet.");

            // -----------------------------------
            // 9. RetryFallbackAttributeTests
            // -----------------------------------
            Console.WriteLine("");
            Console.WriteLine("RetryFallbackAttributeTests");
            Console.WriteLine("");
            var retryFallbackAttributeTests = new RetryFallbackAttributeTests();
            retryFallbackAttributeTests.RetryFallbackMethod_RetryFallback();
            Console.WriteLine("RetryFallbackAttributeTests beendet.");

            Console.WriteLine("*** alle Tests beendet ***");
            Console.ReadLine();
        }

    }

    public partial class ThreadSafeAttributeTests
    {
        [ThreadSafe(2)]
        public void ThreadSafeMethod()
        {
            ThreadSafeMethod_Implementation();
        }

        private void ThreadSafeMethod_Implementation()
        {
            Console.WriteLine($"Die Methode wird von Thread {Thread.CurrentThread.ManagedThreadId} ausgeführt.");
            Thread.Sleep(1000); // Simuliert eine langwierige Aufgabe
            Console.WriteLine($"Die Methode ist von Thread {Thread.CurrentThread.ManagedThreadId} fertig.");
        }
    }

    public partial class SingleExecutionAttributeTests
    {
        [SingleExecution]
        public void SingleExecutionMethod()
        {
            SingleExecutionMethod_Implementation();
        }

        private void SingleExecutionMethod_Implementation()
        {
            Console.WriteLine($"Die Methode wird von Thread {Thread.CurrentThread.ManagedThreadId} ausgeführt.");
            Thread.Sleep(1000); // Simuliert eine langwierige Aufgabe
            Console.WriteLine($"Die Methode ist von Thread {Thread.CurrentThread.ManagedThreadId} fertig.");
        }
    }

    public partial class DebounceAttributeTests
    {
        [Debounce(1000)]
        public void DebounceMethod()
        {
            DebounceMethod_Implementation();
        }

        private void DebounceMethod_Implementation()
        {
            Console.WriteLine($"Die Methode wird von Thread {Thread.CurrentThread.ManagedThreadId} ausgeführt.");
            Thread.Sleep(1000); // Simuliert eine langwierige Aufgabe
            Console.WriteLine($"Die Methode ist von Thread {Thread.CurrentThread.ManagedThreadId} fertig.");
        }
    }

    public partial class ReadWriteLockAttributeTests
    {
        [ReadWriteLock(true)]
        public void ReadWriteLockMethod()
        {
            ReadWriteLockMethod_Implementation();
        }

        private void ReadWriteLockMethod_Implementation()
        {
            Console.WriteLine($"Die Methode wird von Thread {Thread.CurrentThread.ManagedThreadId} ausgeführt.");
            Thread.Sleep(1000); // Simuliert eine langwierige Aufgabe
            Console.WriteLine($"Die Methode ist von Thread {Thread.CurrentThread.ManagedThreadId} fertig.");
        }
    }

    public partial class TimedExecutionAttributeTests
    {
        [TimedExecution(300)]
        public void TimedExecutionMethod()
        {
            TimedExecutionMethod_Implementation();
        }

        private void TimedExecutionMethod_Implementation()
        {
            Console.WriteLine($"Die Methode wird von Thread {Thread.CurrentThread.ManagedThreadId} ausgeführt.");
            Thread.Sleep(1000); // Simuliert eine langwierige Aufgabe
            Console.WriteLine($"Die Methode ist von Thread {Thread.CurrentThread.ManagedThreadId} fertig.");
        }
    }

    public partial class CacheAttributeTests
    {
        [Cache(5)]
        public string CacheMethod()
        {
            return CacheMethod_Implementation();
        }

        private string CacheMethod_Implementation()
        {
            Console.WriteLine($"Die Methode wird von Thread {Thread.CurrentThread.ManagedThreadId} ausgeführt.");

            return DateTime.Now.ToString();
        }
    }

    public partial class RetryAttributeTests
    {
        int counter = 0;

        [Retry(3, 2000)]
        public bool RetryMethod()
        {
            return RetryMethod_Implementation();
        }
        // diese methode soll einen Fehler werfen um retry zu testen
        private bool RetryMethod_Implementation()
        {
            counter++;
            Console.WriteLine($"Die Methode wird von Thread {Thread.CurrentThread.ManagedThreadId} ausgeführt. Counter: {counter}");
            throw new Exception("Fehler");
        }
    }

    public partial class FallbackAttributeTests
    {
        [Fallback(nameof(FallbackMethodIfFailed))]
        public bool FallbackMethod()
        {
            return FallbackMethod_Implementation();
        }
        // worker methode die fehlschlägt und fallback aufgerufen wird
        private bool FallbackMethod_Implementation()
        {
            Console.WriteLine($"Die Methode FallbackMethod_Implementation wird von Thread {Thread.CurrentThread.ManagedThreadId} ausgeführt.");
            
            throw new Exception("Fehler");
        }

        private bool FallbackMethodIfFailed()
        {
            Console.WriteLine($"Die Methode MethodIfFailed wird von Thread {Thread.CurrentThread.ManagedThreadId} ausgeführt.");
            Console.WriteLine($"Die Methode MethodIfFailed ist von Thread {Thread.CurrentThread.ManagedThreadId} fertig.");

            return true;
        }
    }

    public partial class RetryFallbackAttributeTests
    {
        int counter = 0;

        [Retry(3, 2000)]
        [Fallback(nameof(FallbackMethod_Implementation))]
        public bool RetryFallbackMethod()
        {
            return RetryFallbackMethod_Implementation();
        }

        // Methode die von Retry solange aufgerufen wird bis sie erfolgreich ist oder die Anzahl der Versuche erreicht ist
        private bool RetryFallbackMethod_Implementation()
        {
            counter++;

            Console.WriteLine($"Die Methode RetryFallbackMethod_Implementation wird von Thread {Thread.CurrentThread.ManagedThreadId} ausgeführt. Counter: {counter}");

            throw new Exception("Fehler");

            Console.WriteLine($"Die Methode RetryFallbackMethod_Implementation ist von Thread {Thread.CurrentThread.ManagedThreadId} fertig.");

            return true;
        }

        // Fallback Methode für Retry wenn Retry fehlschlägt
        private bool FallbackMethod_Implementation()
        {
            Console.WriteLine($"Die Methode FallbackMethod_Implementation wird von Thread {Thread.CurrentThread.ManagedThreadId} ausgeführt.");
            Console.WriteLine($"Die Methode FallbackMethod_Implementation ist von Thread {Thread.CurrentThread.ManagedThreadId} fertig.");

            return true;
        }
    }

}