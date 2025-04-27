using EventDrivenApp.ConsoleTools;
using EventDrivenApp.Demo;
using EventDrivenApp.Simulation;
using EventDrivenApp.Topics;

using EventHelper;

using System;

namespace ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DemoRunner demoRunner = new DemoRunner();
            demoRunner.RunAsync().GetAwaiter().GetResult();
        }
    }
}
