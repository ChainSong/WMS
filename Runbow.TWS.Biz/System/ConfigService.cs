using System;
using System.Collections.Generic;
using Runbow.TWS.Common;
using Runbow.TWS.Dao;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.System;
using Runbow.TWS.Entity.WMS;
using Runbow.TWS.Entity.WMS.Product;
using Runbow.TWS.MessageContracts;

namespace Runbow.TWS.Biz
{
    public class ConfigService : BaseService
    {
        public Response<IEnumerable<Config>> GetConfigs()
        {
            Response<IEnumerable<Config>> response = new Response<IEnumerable<Config>>();

            try
            {
                ConfigAccessor accessor = new ConfigAccessor();
                response.Result = accessor.GetConfigs();
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }

        public Response<IEnumerable<Region>> GetRegions()
        {
            Response<IEnumerable<Region>> response = new Response<IEnumerable<Region>>();

            try
            {
                ConfigAccessor accessor = new ConfigAccessor();
                response.Result = accessor.GetRegions();
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }
        public Response<IEnumerable<CacheInfo>> GetCacheInfo()
        {
            Response<IEnumerable<CacheInfo>> response = new Response<IEnumerable<CacheInfo>>();

            try
            {
                ConfigAccessor accessor = new ConfigAccessor();
                response.Result = accessor.GetCacheInfo();
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }
        public Response<IEnumerable<GoodsShelfInfo>> GetGoodsShelfInfo()
        {
            Response<IEnumerable<GoodsShelfInfo>> response = new Response<IEnumerable<GoodsShelfInfo>>();

            try
            {
                ConfigAccessor accessor = new ConfigAccessor();
                response.Result = accessor.GetGoodsShelfInfo();
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }
        public Response<IEnumerable<TableColumn>> GetApplicationConfig()
        {
            Response<IEnumerable<TableColumn>> response = new Response<IEnumerable<TableColumn>>();

            try
            {
                ConfigAccessor accessor = new ConfigAccessor();
                response.Result = accessor.GetApplicationConfig(null, null);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }

        public Response<IEnumerable<TableColumn>> GetApplicationConfigNew(long? ProjectID, long? CustomerID)
        {
            Response<IEnumerable<TableColumn>> response = new Response<IEnumerable<TableColumn>>();

            try
            {
                ConfigAccessor accessor = new ConfigAccessor();
                response.Result = accessor.GetApplicationConfig(ProjectID, CustomerID);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }

        public Response<IEnumerable<Config>> GetApplicationConfigs()
        {
            Response<IEnumerable<Config>> response = new Response<IEnumerable<Config>>();

            try
            {
                ConfigAccessor accessor = new ConfigAccessor();
                response.Result = accessor.GetApplicationConfigs();
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }

        public Response<IEnumerable<WMSConfig>> GetWMSConfig()
        {
            Response<IEnumerable<WMSConfig>> response = new Response<IEnumerable<WMSConfig>>();

            try
            {

                ConfigAccessor accessor = new ConfigAccessor();
                response.Result = accessor.GetWMSConfig();
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }
        /// <summary>
        /// 获取门店
        /// </summary>
        /// <returns></returns>
        public Response<IEnumerable<WMSConfig>> GetWMSCustomerConfig()
        {
            Response<IEnumerable<WMSConfig>> response = new Response<IEnumerable<WMSConfig>>();

            try
            {

                ConfigAccessor accessor = new ConfigAccessor();
                response.Result = accessor.GetWMSCustomerConfig();
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }
        /// <summary>
        /// 获取门店
        /// </summary>
        /// <returns></returns>
        public Response<IEnumerable<WMSConfig>> GetWMSCustomerConfigByCustomerID(string CustomerID)
        {
            Response<IEnumerable<WMSConfig>> response = new Response<IEnumerable<WMSConfig>>();

            try
            {

                ConfigAccessor accessor = new ConfigAccessor();
                response.Result = accessor.GetWMSCustomerConfigByCustomerID(CustomerID);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }
        public Response<IEnumerable<WMSConfig>> GetWMSCustomerConfig_Return(long? customerID)
        {
            Response<IEnumerable<WMSConfig>> response = new Response<IEnumerable<WMSConfig>>();

            try
            {

                ConfigAccessor accessor = new ConfigAccessor();
                response.Result = accessor.GetWMSCustomerConfig_Return(customerID);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }
        public Response<IEnumerable<WMS_UnitAndSpecifications_Config>> GetWMS_UnitAndSpecifications_Config(long? ProjectID, long? CustomerID, long? WarehouseID)
        {
            Response<IEnumerable<WMS_UnitAndSpecifications_Config>> response = new Response<IEnumerable<WMS_UnitAndSpecifications_Config>>();

            try
            {
                ConfigAccessor accessor = new ConfigAccessor();
                response.Result = accessor.GetWMS_UnitAndSpecifications_Config(ProjectID, CustomerID, WarehouseID);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }
        public Response<IEnumerable<Customer>> GetStorerID()
        {
            Response<IEnumerable<Customer>> response = new Response<IEnumerable<Customer>>();

            try
            {
                ConfigAccessor accessor = new ConfigAccessor();
                response.Result = accessor.GetStorerID();
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }
        public Response<IEnumerable<WMS_Customer>> GetAllCustomer(long CustomerID)
        {
            Response<IEnumerable<WMS_Customer>> response = new Response<IEnumerable<WMS_Customer>>();

            try
            {
                ConfigAccessor accessor = new ConfigAccessor();
                response.Result = accessor.GetAllCustomer(CustomerID);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }

        public Response<IEnumerable<WMS_Config_Type>> GetWMSConfigType()
        {
            Response<IEnumerable<WMS_Config_Type>> response = new Response<IEnumerable<WMS_Config_Type>>();

            try
            {

                ConfigAccessor accessor = new ConfigAccessor();
                response.Result = accessor.GetWMSConfigType();
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }
    }
}