using System;

namespace GST.Library.String
{
    /// <summary>
    /// Encode and decode Base64String Safe for URL
    /// From https://brockallen.com/2014/10/17/base64url-encoding/
    /// </summary>
    public class Base64Url
    {
        /// <summary>
        /// Encode byte in Base64String Url safe
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static string Encode(byte[] arg)
        {
            string s = Convert.ToBase64String(arg); // Standard base64 encoder

            return SafeBase64StringForUrl(s);
        }

        /// <summary>
        /// Decode Base64String Url safe into byte
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static byte[] Decode(string arg)
        {
            string s = UnSafeBase64StringForUrl(arg);

            return Convert.FromBase64String(s); // Standard base64 decoder
        }

        /// <summary>
        /// Encode a Base64 string to be passed in a URL
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string SafeBase64StringForUrl(string s)
        {
            s = s.Split('=')[0]; // Remove any trailing '='s
            s = s.Replace('+', '-'); // 62nd char of encoding
            s = s.Replace('/', '_'); // 63rd char of encoding

            return s;
        }

        /// <summary>
        /// Decode a Base64 string that has been encoded with <see cref="SafeBase64StringForUrl"/>
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string UnSafeBase64StringForUrl(string s)
        {
            s = s.Replace('-', '+'); // 62nd char of encoding
            s = s.Replace('_', '/'); // 63rd char of encoding

            switch (s.Length % 4) // Pad with trailing '='s
            {
                case 0: break; // No pad chars in this case
                case 2: s += "=="; break; // Two pad chars
                case 3: s += "="; break; // One pad char
                default: throw new Exception("Illegal base64url string!");
            }

            return s;
        }
    }
}
