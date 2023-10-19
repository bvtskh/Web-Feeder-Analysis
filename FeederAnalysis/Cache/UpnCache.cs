using System;
using System.Collections.Generic;
using System.Diagnostics;
using FeederAnalysis.Business;

namespace FeederAnalysis.Cache
{
    public static class UpnCache
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static Dictionary<string, UpnEntity> _cache = new Dictionary<string, UpnEntity>();
        private const int cacheLengt = 10000;
        private static readonly object  ob = new object();
        private static void AddCache(string bcNo)
        {
            ClearCache();
            lock (ob)
            {
                if (!_cache.ContainsKey(bcNo))
                {
                    try
                    {
                        var entity = SingletonHelper.UsapInstance.GetByBcNo(bcNo);
                        if (entity != null)
                        {
                            var cache = new UpnEntity()
                            {
                                upnID = bcNo,
                                partNo = entity.PART_NO
                            };
                            if (string.Equals(entity.BC_TYPE, "EM", StringComparison.OrdinalIgnoreCase))
                            {
                                var bcTokusai = SingletonHelper.UsapInstance.GetBcTokusai(bcNo);
                                if (bcTokusai != null)
                                {
                                    cache.emNo = bcTokusai.EM_NO;
                                    cache.partFm = bcTokusai.PART_FM;
                                    cache.partTo = bcTokusai.PART_TO;

                                }
                            }
                            _cache.Add(bcNo, cache);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
            }
           

        }

        private static void ClearCache()
        {
            if (_cache.Count >= cacheLengt)
            {
                _cache.Clear();
                log.DebugFormat("{0} clear cache.", Environment.MachineName);
            }
        }

        public static UpnEntity FindBc(string bcNo)
        {
            AddCache(bcNo);
            UpnEntity value = new UpnEntity();
            if (_cache.TryGetValue(bcNo, out value))
            {
                return value;
            }
            else
            {
                return null;
            }
        }
    }
}
