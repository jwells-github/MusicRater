using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicRater.Areas.Identity.Data
{
    public class RandomString
    {
        public static string Get(int length = 8)
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] stringChars = new char[length];
            Random random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new String(stringChars);
        }
    }
}
