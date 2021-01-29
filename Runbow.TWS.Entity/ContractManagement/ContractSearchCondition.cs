using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity
{
    [Serializable]
    public class ContractSearchCondition
    {
        //Contract 查询条件
        public string SCCompany { get; set; }                 //所属公司查询
        public string SCBusiness { get; set; }                //业务大类查询
        public string SCDepartment { get; set; }              //部门查询
        public string SCContractType { get; set; }            //合同类型查询
        public DateTime SCContractStart_start { get; set; }   //合同日期查询 开始
        public DateTime SCContractStart_end { get; set; }     //合同日期查询 终止
        public string SCContract { get; set; }                //合同查询 合同编号、名称、内容、老合同编号、业务对方名称
        public string SCExtended { get; set; }                //合同是否能顺延
        public string SCExpired { get; set; }                 //合同是否截止
        public string StampTax { get; set; }                  //是否有印花税

       

    }
}
