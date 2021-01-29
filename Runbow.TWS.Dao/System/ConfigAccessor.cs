using System.Collections.Generic;
using Runbow.TWS.Common;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.System;
using Runbow.TWS.Entity.WMS.Product;
using System.Data;
using Runbow.TWS.Entity.WMS;
namespace Runbow.TWS.Dao
{
    public class ConfigAccessor : BaseAccessor
    {
        public DataTable GetTable_Colums( long? ProjectID ,long? CustomerID, string TableName)
        {
            DbParam[] dbParams = new DbParam[]{
                  new DbParam("@ProjectID", DbType.Int64, ProjectID==null?0:ProjectID, ParameterDirection.Input),
                new DbParam("@CustomerID", DbType.Int64, CustomerID==null?0:CustomerID, ParameterDirection.Input),
                new DbParam("@TableName", DbType.String,TableName ,ParameterDirection.Input)
            };
            return this.ExecuteDataTable("Proc_GetTable_Colums", dbParams);
        }
        public IEnumerable<Config> GetApplicationConfigs()
        {
            return this.ExecuteDataTable("Proc_GetApplicationConfig").ConvertToEntityCollection<Config>();
        }
        public IEnumerable<TableColumn> GetApplicationConfig(long? ProjectID, long? CustomerID)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ProjectID", DbType.Int64, ProjectID==null?0:ProjectID, ParameterDirection.Input),
                new DbParam("@CustomerID", DbType.Int64, CustomerID==null?0:CustomerID, ParameterDirection.Input)              
            };
            return this.ExecuteDataTable("Proc_GetApplicationConfigNew", dbParams).ConvertToEntityCollection<TableColumn>();
        }
        public IEnumerable<Config> GetConfigs()
        {
            return this.ExecuteDataTable("Proc_GetConfig").ConvertToEntityCollection<Config>();
        }

        public IEnumerable<Region> GetRegions()
        {
            return this.ExecuteDataTable("Proc_GetRegions").ConvertToEntityCollection<Region>();
        }

        public IEnumerable<CacheInfo> GetCacheInfo()
        {
            return this.ExecuteDataTable("Proc_WMS_CacheInfo").ConvertToEntityCollection<CacheInfo>();
        }
        public IEnumerable<GoodsShelfInfo> GetGoodsShelfInfo()
        {
            return this.ExecuteDataTable("Proc_WMS_GoodsShelfInfo").ConvertToEntityCollection<GoodsShelfInfo>();
        }
        public IEnumerable<WMSConfig> GetWMSConfig()
        {
            return this.ExecuteDataTable("Proc_WMS_GetWMS_Config").ConvertToEntityCollection<WMSConfig>();
        }
        
        public IEnumerable<WMSConfig> GetWMSCustomerConfig()
        {
            return this.ExecuteDataTable("Proc_WMS_GetWMSCustomer_Config").ConvertToEntityCollection<WMSConfig>();
        }
        public IEnumerable<WMSConfig> GetWMSCustomerConfigByCustomerID(string CustomerID)
        {

            DbParam[] dbParams = new DbParam[]{
                 new DbParam("@CustomerID", DbType.String,CustomerID, ParameterDirection.Input)
            };
            return this.ExecuteDataTable("Proc_WMS_GetWMSCustomer_ConfigByCustomerID", dbParams).ConvertToEntityCollection<WMSConfig>();
        }

        public IEnumerable<WMSConfig> GetWMSCustomerConfig_Return(long? customerID)
        {
            DbParam[] dbParams = new DbParam[]{
                 new DbParam("@CustomerID", DbType.Int64, customerID==null?0:customerID, ParameterDirection.Input)
            };
            if (customerID == 103)
            {
                return this.ExecuteDataTable("Proc_WMS_GetWMSCustomer_Config_103", dbParams).ConvertToEntityCollection<WMSConfig>();
            }
            else
            {
                return this.ExecuteDataTable("Proc_WMS_GetWMSCustomer_Config", dbParams).ConvertToEntityCollection<WMSConfig>();
            }
        }
        public IEnumerable<WMS_UnitAndSpecifications_Config> GetWMS_UnitAndSpecifications_Config(long? ProjectID, long? CustomerID, long? WarehouseID)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ProjectID", DbType.Int64, ProjectID==null?0:ProjectID, ParameterDirection.Input),
                new DbParam("@CustomerID", DbType.Int64, CustomerID==null?0:CustomerID, ParameterDirection.Input),
                new DbParam("@WarehouseID", DbType.Int64, WarehouseID==null?0:WarehouseID, ParameterDirection.Input)
            };
            return this.ExecuteDataTable("Proc_WMS_GetWMS_UnitAndSpecifications_Config", dbParams).ConvertToEntityCollection<WMS_UnitAndSpecifications_Config>();
        }
        
        public IEnumerable<Customer> GetStorerID()
        {

            return this.ExecuteDataTable("Proc_getStorerID").ConvertToEntityCollection<Customer>();
        }
        public IEnumerable<WMS_Customer> GetAllCustomer(long CustomerID)
        {
            DbParam[] dbParams = new DbParam[]{
               new DbParam("@CustomerID", DbType.Int64, CustomerID==null ? 0 : CustomerID, ParameterDirection.Input)
            };
            return this.ExecuteDataTable("Proc_getWMSCustomer", dbParams).ConvertToEntityCollection<WMS_Customer>();
        }
        public IEnumerable<WMS_Config_Type> GetWMSConfigType()
        {
            return this.ExecuteDataTable("Proc_WMS_GetWMS_Config_Type").ConvertToEntityCollection<WMS_Config_Type>();
        }
    }
}