using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeederAnalysis.Business
{
    public static class Utility
    {
        public static string MakePCB(string inputText)
        {
            string[] remove = { "SMT", "SMTA", "SMTB" };
            foreach (string item in remove)
            {
                if (inputText.EndsWith(item))
                {
                    inputText = inputText.Substring(0, inputText.LastIndexOf(item));
                    break; //only allow one match at most
                }
            }
            inputText = inputText.Replace("-", "");
            return inputText;
        }
    }
}
