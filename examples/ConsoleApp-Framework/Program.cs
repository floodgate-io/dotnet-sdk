using System;
using System.Collections.Generic;
using FloodGate.SDK;

namespace ConsoleApp_Framework
{
    class Program
    {
        static void Main(string[] args)
        {
            Run();

            Console.ReadLine();
        }

        static void Run()
        {
            try
            {
                FloodGateWrapper floodgate = FloodGateWrapper.Instance;

                var customAttributes = new Dictionary<string, string>() {
                    { "name", "Peter Parker" },
                    { "subscription", "Gold" },
                    { "country", "UK" },
                    { "role", "User" }
                };

                User user = new User("d2405fc0-c9cd-49e7-a07e-bf244d6d360b")
                {
                    Email = "peter@marvel.com",
                    CustomAttributes = customAttributes
                };

                var flag1 = floodgate.Client.GetValue("background-colour", "grey", user);
                Console.WriteLine($"`background-colour` = {flag1}");

                floodgate.Client.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
