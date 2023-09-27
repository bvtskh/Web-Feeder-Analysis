using System;

namespace FeederAnalysis.Entities
{
    public class StaffEntity
    {
        public string StaffCode { get; set; }
        public string FullName { get; set; }
        public string DeptCode { get; set; }
        public DateTime Birthdate { get; set; }
        public string Sex { get; set; }
        public DateTime EntryDate { get; set; }
        public string PosName { get; set; }
        public string Customer { get; set; }
        public string MaritalStatus { get; set; }
        public DateTime? LiquidationDate { get; set; }
        public string EduCode { get; set; }

        public string PermanentAdd { get; set; }
        public double? Distance { get; set; }
    }

}
