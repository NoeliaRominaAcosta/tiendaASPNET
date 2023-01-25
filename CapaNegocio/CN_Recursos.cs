using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace CapaNegocio
{
   public class CN_Recursos
    {
        //encripted

        public static string ConvertToSha256(string text)
        {
            StringBuilder Sb = new StringBuilder();
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(text));
                foreach (byte b in result)
                {
                    Sb.Append(b.ToString("x2"));
                }
                return Sb.ToString();
            }

        }
    }
}
