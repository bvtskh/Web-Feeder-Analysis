using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeederAnalysis.Business
{
    public class Constant
    {

    }
    public enum TokusaiItemHistoryChangeId{

        HongTrang = 0,
        TrangHong = 1,
        ECO = 2,
        MainSubUpdateAlterPart = 3,
        MainSubChangePart = 4,
        MainSubInsertPart = 5,
        TokusaiDMNotAccept = 6
    }
}