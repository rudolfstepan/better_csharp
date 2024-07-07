namespace OuterClassDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            OuterClass.InnerClass inner = new OuterClass.InnerClass();
            inner.Test();

            Console.ReadLine();
        }
    }

    /// <summary>
    /// Outer class containing an inner class and a member
    /// The inner class can access the outer class's member
    /// </summary>
    public class OuterClass
    {
        public string OuterClassName = "OuterClass";

        // Static instance of the outer class
        public static OuterClass OuterClassInstance;

        // Static constructor to initialize the static instance
        static OuterClass()
        {
            OuterClassInstance = new OuterClass();
        }

        public class InnerClass
        {
            public void Test()
            {
                // Accessing the outer class's member using 'OuterClassInstance'
                Console.WriteLine(OuterClassInstance.OuterClassName);
                Console.WriteLine("Test");
            }
        }
    }
}
