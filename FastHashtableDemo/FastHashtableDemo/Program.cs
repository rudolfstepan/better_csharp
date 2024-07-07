using System.Diagnostics;

namespace FastHashtableDemo
{
    internal class Program
    {
        static long GetElapsedNanoseconds(Stopwatch stopwatch)
        {
            return (stopwatch.ElapsedTicks * 1_000_000_000) / Stopwatch.Frequency;
        }

        static unsafe void Main()
        {
            int numElements = 1000000;

            // Test with multiple instances to ensure memory isolation
            var table1 = new FastHashtable<int, string>(1000000);
            var table2 = new FastHashtable<int, string>(1000000);

            // Benchmark Add for table1
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < numElements; i++)
            {
                table1.Add(i, "Value" + i);
            }
            stopwatch.Stop();
            Console.WriteLine($"Table1 - Time to add {numElements} elements: {GetElapsedNanoseconds(stopwatch)} ns");

            // Benchmark Add for table2
            stopwatch.Reset();
            stopwatch.Start();
            for (int i = 0; i < numElements; i++)
            {
                table2.Add(i, "Value" + i);
            }
            stopwatch.Stop();
            Console.WriteLine($"Table2 - Time to add {numElements} elements: {GetElapsedNanoseconds(stopwatch)} ns");

            // Benchmark TryGetValue for table1
            stopwatch.Reset();
            stopwatch.Start();
            for (int i = 0; i < numElements; i++)
            {
                table1.TryGetValue(i, out string value);
            }
            stopwatch.Stop();
            Console.WriteLine($"Table1 - Time to lookup {numElements} elements: {GetElapsedNanoseconds(stopwatch)} ns");

            // Benchmark TryGetValue for table2
            stopwatch.Reset();
            stopwatch.Start();
            for (int i = 0; i < numElements; i++)
            {
                table2.TryGetValue(i, out string value);
            }
            stopwatch.Stop();
            Console.WriteLine($"Table2 - Time to lookup {numElements} elements: {GetElapsedNanoseconds(stopwatch)} ns");

            // Benchmark Remove for table1
            stopwatch.Reset();
            stopwatch.Start();
            for (int i = 0; i < numElements; i++)
            {
                table1.Remove(i);
            }
            stopwatch.Stop();
            Console.WriteLine($"Table1 - Time to remove {numElements} elements: {GetElapsedNanoseconds(stopwatch)} ns");

            // Benchmark Remove for table2
            stopwatch.Reset();
            stopwatch.Start();
            for (int i = 0; i < numElements; i++)
            {
                table2.Remove(i);
            }
            stopwatch.Stop();
            Console.WriteLine($"Table2 - Time to remove {numElements} elements: {GetElapsedNanoseconds(stopwatch)} ns");

            table1.Dispose();
            table2.Dispose();


            Console.ReadKey();
        }
    }
}
