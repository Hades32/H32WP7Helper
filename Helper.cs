using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace H32WP7Helper
{
    public static class Helper
    {
        public static void AddOrSet<TKey, TVal>(this IDictionary<TKey, TVal> dict, TKey key, TVal val)
        {
            if (dict.ContainsKey(key))
                dict[key] = val;
            else
                dict.Add(key, val);
        }

        public static int TryGetHashCode(this object obj)
        {
            if (obj == null)
                return 0;
            else
                return obj.GetHashCode();
        }

        public static TValue getOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
        {
            return dict.getOrDefault(key, default(TValue));
        }

        public static TValue getOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue def)
        {
            if (dict.ContainsKey(key))
            {
                return dict[key];
            }
            else
            {
                return def;
            }
        }

        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> collection)
        {
            var res = new ObservableCollection<T>();

            foreach (var item in collection)
            {
                res.Add(item);
            }

            return res;
        }

        //MD5 implementation from http://dev.mbdnet.hu/2009/12/cmd5-hash-algorithmus/
        public static string MD5Hash(string daten)
        {
            uint[] r = new uint[] { 7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22, 5, 9, 14, 20, 5, 9, 14, 20, 5, 9, 14, 20, 5, 9, 14, 20, 4, 11, 16, 23, 4, 11, 16, 23, 4, 11, 16, 23, 4, 11, 16, 23, 6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21 };
            uint[] k = new uint[64];
            for (int i = 0; i < 64; i++)
            {
                k[i] = Convert.ToUInt32(Math.Floor(Math.Abs(Math.Sin(i + 1)) * Math.Pow(2, 32)));
            }

            uint h0 = 0x67452301;
            uint h1 = 0xefcdab89;
            uint h2 = 0x98badcfe;
            uint h3 = 0x10325476;

            uint laenge = (uint)daten.Length * 8;

            string text = daten + ((char)128).ToString();
            while ((text.Length % 64 != 56))
            {
                text = text + ((char)0).ToString();
            }

            for (int i = 0; i < 4; i++)
            {
                uint tmp;
                tmp = laenge & 0xFF;
                text = text + ((char)tmp).ToString();
                laenge = laenge >> 8;
            }

            text = text + ((char)0).ToString() + ((char)0).ToString() + ((char)0).ToString() + ((char)0).ToString();

            for (int i = 0; i < text.Length / 64; i++)
            {
                uint[] w = new uint[16];
                for (int j = 0; j < 16; j++)
                {
                    string wtmp = "";
                    for (int m = 3; m >= 0; m--)
                    {
                        wtmp = wtmp + ((byte)text[i * 64 + j * 4 + m]).ToString("X2");
                    }
                    w[j] = Convert.ToUInt32(wtmp, 16);
                }

                uint a = h0;
                uint b = h1;
                uint c = h2;
                uint d = h3;

                for (uint j = 0; j < 64; j++)
                {
                    uint f = 0;
                    uint g = 0;

                    if (0 <= j && j <= 15)
                    {
                        f = ((b & c) | ((~b) & d));
                        g = j;
                    }
                    else if (16 <= j && j <= 31)
                    {
                        f = ((d & b) | ((~d) & c));
                        g = (5 * j + 1) % 16;
                    }
                    else if (32 <= j && j <= 47)
                    {
                        f = b ^ c ^ d;
                        g = (3 * j + 5) % 16;
                    }
                    else if (48 <= j && j <= 63)
                    {
                        f = (c ^ (b | (~d)));
                        g = (7 * j) % 16;
                    }
                    uint tmp = d;
                    d = c;
                    c = b;
                    b = RotateLeft((a + f + k[j] + w[g]), r[j]) + b;
                    a = tmp;
                }

                h0 = h0 + a;
                h1 = h1 + b;
                h2 = h2 + c;
                h3 = h3 + d;
            }

            h0 = reverse(h0);
            h1 = reverse(h1);
            h2 = reverse(h2);
            h3 = reverse(h3);

            return h0.ToString("X8") + h1.ToString("X8") + h2.ToString("X8") + h3.ToString("X8");
        }

        private static uint RotateLeft(uint zahl, uint anzahl)
        {
            uint bit;
            for (int i = 0; i < anzahl; i++)
            {
                bit = 0x80000000 & zahl;
                zahl = zahl << 1;
                if (bit == 0x80000000)
                    zahl = zahl + 1;
            }
            return zahl;
        }

        private static uint RotateRight(uint zahl, uint anzahl)
        {
            uint bit;
            for (int i = 0; i < anzahl; i++)
            {
                bit = 0x00000001 & zahl;
                zahl = zahl >> 1;
                if (bit == 0x00000001)
                    zahl = zahl + 0x80000000;
            }
            return zahl;
        }

        private static uint reverse(uint zahl)
        {
            uint tmp;
            uint result = 0;
            for (int i = 0; i < 4; i++)
            {
                result = result << 8;
                tmp = zahl & 0xFF;
                result = result + tmp;
                zahl = zahl >> 8;
            }
            return result;
        }

        public static void networkCheckCallback(Action<bool> hasNetCallback)
        {
            System.Threading.ThreadPool.QueueUserWorkItem(
                (x) =>
                {
                    hasNetCallback(Microsoft.Phone.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable());
                });
        }

        [System.Diagnostics.DebuggerHidden]
        public static void ExitApplication()
        {
            throw new ApplicationExitException();
        }

        /// <summary>
        /// This Exception serves the sole purpose of exiting the application
        /// </summary>
        public class ApplicationExitException : Exception
        {

        }

        /// <summary>
        /// This method replacec the internaltional form of umlauts with the 
        /// correct German one. E.g. ae -> ä
        /// WARNING: This is just a HACK. It might not be very reliable...
        /// </summary>
        /// <param name="s">string to fix</param>
        /// <returns>String with ä,ö,ü letters</returns>
        public static string fixUmlauts(string s)
        {
            var res = s.Replace("ue", "ü").Replace("oe", "ö").Replace("ae", "ä")
                       .Replace("Ue", "Ü").Replace("Oe", "Ö").Replace("Ae", "Ä");
            // fix "Abenteuer"
            res = res.Replace("eü", "eue");
            res = res.Replace("Eü", "Eue");
            // fix "queen"
            res = res.Replace("qü", "que");
            res = res.Replace("Qü", "Que");
            return res;
        }
    }
}
