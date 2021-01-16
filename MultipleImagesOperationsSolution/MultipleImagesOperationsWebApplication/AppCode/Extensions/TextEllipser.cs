using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultipleImagesOperationsWebApplication.AppCode.Extensions
{
    public static partial class Extension
    {
        public  static string ToEllipseText(string ourSentence, int ellipseLength = 100)
        {
            if (ourSentence == null)
            {
                return ourSentence;
            }
            else if(ourSentence.Length <= ellipseLength)
            {
                return ourSentence;
            }
            else
            {
                return $"{ourSentence.Substring(0, ellipseLength)}...";
            }
        }
    }
}
