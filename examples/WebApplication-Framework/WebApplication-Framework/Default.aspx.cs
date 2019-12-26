using System;
using System.Collections.Generic;
using FloodGate.SDK;
using WebApplication_Framework.Services;

namespace WebApplication_Framework
{
    public partial class Default : System.Web.UI.Page
    {
        FloodGateWrapper floodgate = FloodGateWrapper.Instance;

        protected void Page_Load(object sender, EventArgs e)
        {
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

            var result = floodgate.Client.GetValue("background-colour", "grey", user);

            lblColour.Text = $"The `background-colour` flag is {result}";

            floodgate.Client.Dispose();
        }
    }
}