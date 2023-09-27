using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeederAnalysis.Business
{
    public class FileHelper
    {
        /// <summary>
        /// Chia file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="locations"></param>
        /// <param name="nSize"></param>
        /// <returns></returns>
        public static IEnumerable<List<T>> SplitList<T>(List<T> locations, int nSize = 6)
        {
            for (int i = 0; i < locations.Count; i += nSize)
            {
                var tmp = locations.GetRange(i, Math.Min(nSize, locations.Count - i));
                yield return locations.GetRange(i, Math.Min(nSize, locations.Count - i));
            }
        }
    }
}
