using Runbow.TWS.Entity.WMS.UnitAndSpecifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Dao.UnitAndSpecifications
{
    public class UnitAndSpecificationsAccessor : BaseAccessor
    {

        public IEnumerable<UnitAndSpecificationsInfo> GetUnitAndSpecifications(UnitAndSpecificationsInfo unitAndSpecificationsInfo)
        {
            try
            {
                string sql = " select (select top 1 Name from Customer where id =a.Customerid) CustomerName, * from [dbo].[WMS_UnitAndSpecifications_Config] a where 1=1 ";
                if (unitAndSpecificationsInfo.CustomerID != 0)
                {
                    sql += " and  CustomerId=" + unitAndSpecificationsInfo.CustomerID;
                }

                if (!string.IsNullOrEmpty(unitAndSpecificationsInfo.CustomerIDs))
                {

                    sql += " and  CustomerId in  (" + unitAndSpecificationsInfo.CustomerIDs + ")";
                }
                return this.ExecuteDataTableBySqlString(sql).ConvertToEntityCollection<UnitAndSpecificationsInfo>();

            }
            catch (Exception e)
            {

                return null;
            }
        }
        public bool AddUnitAndSpecifications(UnitAndSpecificationsInfo unitAndSpecificationsInfo)
        {
            try
            {
                string sql = @" insert into  [WMS_UnitAndSpecifications_Config] (ProjectID,CustomerID,Unit,Specifications,Status)
                    values (
                    '" + unitAndSpecificationsInfo.ProjectID + @"',
                    '" + unitAndSpecificationsInfo.CustomerID + @"',
                    '" + unitAndSpecificationsInfo.Unit + @"',  
                    '" + unitAndSpecificationsInfo.Specifications + @"',  
                     1
                    )";
                this.ExecuteDataTableBySqlString(sql);
                return true;

            }
            catch (Exception e)
            {
                return false;
            }
        }
        public bool DeleteUnitAndSpecifications(int Id)
        {
            try
            {
                string sql = @" delete  [WMS_UnitAndSpecifications_Config]  where ID=" + Id;
                this.ExecuteDataTableBySqlString(sql);
                return true;

            }
            catch (Exception e)
            {
                return false;
            }
        }

    }
}
