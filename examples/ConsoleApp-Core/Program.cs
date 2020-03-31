using System;
using System.Collections.Generic;
using FloodGate.SDK;

namespace ConsoleApp_Core
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
                    { "SubscriptionPlan", "Gold" },
                    { "Role", "User" }
                };

                User user = new User("d2405fc0-c9cd-49e7-a07e-bf244d6d360b")
                {
                    Email = "peter@marvel.com",
                    Name = "Peter Parker",
                    Country = "UK",
                    CustomAttributes = customAttributes
                };

                var flag1 = floodgate.Client.GetValue("enable-team-feature", false, user);
                Console.WriteLine($"`enable-team-feature` = {flag1}");

                Console.ReadLine();
                
                floodgate.Client.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
