using System;
using System.Collections.Generic;
using FloodGate.SDK;
using WebApplication_Framework.Services;

namespace WebApplication_Framework
{
    public partial class Accounts : System.Web.UI.Page
    {
        FloodGateWrapper floodgate = FloodGateWrapper.Instance;

        protected void Page_Load(object sender, EventArgs e)
        {
            var customAttributes = new Dictionary<string, string>() {
                { "subscription", "Gold" },
                { "role", "User" }
            };

            User user = new User("d2405fc0-c9cd-49e7-a07e-bf244d6d360b")
            {
                Email = "peter@marvel.com",
                Name = "Spiderman",
                Country = "UK",
                CustomAttributes = customAttributes
            };

            var result = floodgate.Client.GetValue("multivariate-flag", "grey", user);

            lblColour.Text = $"The `multivariate-flag` flag is {result}";

            floodgate.Client.FlushEvents();

            // floodgate.Client.Dispose();
        }
    }
}