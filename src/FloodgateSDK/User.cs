using System;
using System.Collections.Generic;
using System.Linq;

namespace FloodGate.SDK
{
    /// <summary>
    /// User object containing data specific to the user.
    /// The default properties link directly to properties which are available for filtering in the floodgate.io management console.
    /// </summary>
    public class User
    {

        /// <summary>
        /// Default: Unique identifier of the current user
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Default: Current users email address
        /// </summary>
        public string Email { get; set; }

        public string Name { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Country { get; set; }

        /// <summary>
        /// Disctionary containing custom attributes of the user
        /// </summary>
        public Dictionary<string, string> CustomAttributes { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Creates a new user for targeting
        /// </summary>
        /// <param name="id">Unique identifier of the user</param>
        public User(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("UserId", "Every user must have a unique user id");
            }

            Id = id;
        }

        public string GetAttributeValue(string key)
        {
            if (key == Consts.USER_ATTRIBUTE_ID)
                return Id;

            if (key == Consts.USER_ATTRIBUTE_EMAIL)
                return Email;

            // Check custom attributes
            var attribute = CustomAttributes.Where(q => q.Key.ToLower() == key.ToLower()).FirstOrDefault();
            if (!string.IsNullOrEmpty(attribute.Value))
            {
                return attribute.Value.ToLower();
            }

            return null;
        }
    }
}
