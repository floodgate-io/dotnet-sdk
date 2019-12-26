using System.Security.Cryptography;
using System.Text;

namespace FloodGate.SDK.Evaluators
{
    public class EvaluatorHelper
    {
        public static int GetScale(string key, string id)
        {
            var hashString = key + id;

            var hash = HashString(id).Substring(0, 7);

            var scale = int.Parse(hash, System.Globalization.NumberStyles.HexNumber) % 100;

            return scale;
        }

        static string HashString(string uniqueIdentifier)
        {
            using (var hash = SHA1.Create())
            {
                var hb = hash.ComputeHash(Encoding.UTF8.GetBytes(uniqueIdentifier));

                var sb = new StringBuilder();

                foreach (byte b in hb)
                {
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }
        }

        //static string HexStringFromBytes(byte[] bytes)
        //{
        //    var sb = new StringBuilder();
        //    foreach (byte b in bytes)
        //    {
        //        var hex = b.ToString("x2");
        //        sb.Append(hex);
        //    }
        //    return sb.ToString();
        //}
    }
}
