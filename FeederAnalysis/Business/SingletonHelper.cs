using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeederAnalysis.Business
{
    public class SingletonHelper
    {
        private static volatile UsapService.USAPWebServiceSoapClient _usap_service;
        private static volatile ErpService.ERPWebServiceSoapClient _erp_service;
        private static object sync = new object();

        public static UsapService.USAPWebServiceSoapClient UsapInstance
        {
            get
            {
                if (_usap_service == null)
                {
                    lock (sync)
                    {
                        if (_usap_service == null)
                        {
                            _usap_service = new UsapService.USAPWebServiceSoapClient();
                        }
                    }
                }
                return _usap_service;
            }
        }
        public static ErpService.ERPWebServiceSoapClient ErpInstance
        {
            get
            {
                if (_erp_service == null)
                {
                    lock (sync)
                    {
                        if (_erp_service == null)
                        {
                            _erp_service = new ErpService.ERPWebServiceSoapClient();
                        }
                    }
                }
                return _erp_service;
            }
        }
    }
}
