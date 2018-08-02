using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MXJ.Core.Utility.Helper;

namespace MXJ.Core.Utility.Extensions
{
    public static class GuidExtension
    {
        
        public static string Shrink(this Guid target)
        {
            ArgumentChecker.IsNotEmpty(target, "target");

            string base64 = Convert.ToBase64String(target.ToByteArray());

            string encoded = base64.Replace("/", "_").Replace("+", "-");

            return encoded.Substring(0, 22);
        }

        
        public static bool IsEmpty(this Guid target)
        {
            return target == Guid.Empty;
        }
    }
}