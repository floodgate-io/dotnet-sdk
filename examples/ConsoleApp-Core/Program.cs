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
                    Name = "Spiderman",
                    Country = "UK",
                    CustomAttributes = customAttributes
                };

                string flagKey, flagResult;

                flagKey = "boolean-flag";
                flagResult = floodgate.Client.GetValue(flagKey, false, user).ToString();
                Console.WriteLine($"`{flagKey}` = {flagResult}");

                flagKey = "multivariate-flag";
                flagResult = floodgate.Client.GetValue(flagKey, "black", user);
                Console.WriteLine($"`{flagKey}` = {flagResult}");

                flagKey = "multivariate-flag";
                flagResult = floodgate.Client.GetValue(flagKey, "black");
                Console.WriteLine($"`{flagKey}` = {flagResult}");

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
