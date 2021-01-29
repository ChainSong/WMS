using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.MobileWeb
{
    public class BTBMoneySummaryReport
    {
        [EntityPropertyExtension("Id", "Id")]
        public long Id { get; set; }
        [EntityPropertyExtension("CustomerId", "CustomerId")]
        public long CustomerId { get; set; }
        [EntityPropertyExtension("CustomerName", "CustomerName")]
        public string CustomerName { get; set; }
        [EntityPropertyExtension("CashDeposit", "CashDeposit")]
        public decimal CashDeposit { get; set; }
        [EntityPropertyExtension("Loan", "Loan")]
        public decimal Loan { get; set; }
        [EntityPropertyExtension("Advance", "Advance")]
        public decimal Advance { get; set; }
        [EntityPropertyExtension("ReturnedMoney", "PoReturnedMoneydID")]
        public decimal ReturnedMoney { get; set; }
        [EntityPropertyExtension("MDate", "MDate")]
        public DateTime MDate { get; set; }
        [EntityPropertyExtension("ConventionalDate", "ConventionalDate")]
        public DateTime ConventionalDate { get; set; }
        [EntityPropertyExtension("ActualDate", "ActualDate")]
        public DateTime ActualDate { get; set; }
        [EntityPropertyExtension("CreateDate", "CreateDate")]
        public DateTime CreateDate { get; set; }
        [EntityPropertyExtension("Month", "Month")]
        public DateTime Month { get; set; }

    }
}
