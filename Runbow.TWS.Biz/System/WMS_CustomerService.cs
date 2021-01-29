using System;
using System.Collections.Generic;
using Runbow.TWS.Common;
using Runbow.TWS.Dao;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.MessageContracts.System;
using Runbow.TWS.Entity.WMS.Product;

namespace Runbow.TWS.Biz
{
    public class WMS_CustomerService : BaseService
    {
        public Response<Customer> GetCustomerInfo(GetCustomerByCustomerIdRequest request)
        {
            Response<Customer> response = new Response<Customer>();
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetCustomerInfo request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            WMS_CustomerAccessor accessor = new WMS_CustomerAccessor();

            try
            {
                response.Result = accessor.GetCustomerById(request.CustomerID);
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
        /// 验证公司编号 唯一性
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public string CheckNameIsExist(string Name, int? Id, string ProjectID, bool IsEdit)
        {
            WMS_CustomerAccessor accessor = new WMS_CustomerAccessor();
            string ReturnValue = string.Empty;
            try
            {
                if (!accessor.CheckNameIsExist(Name, Id, ProjectID, IsEdit))
                {
                    ReturnValue = "该客户名称已存在！";
                }
            }
            catch (Exception ex)
            {
                ReturnValue = ex.Message;
                LogError(ex);
            }
            return ReturnValue;
        }

        public Response<IEnumerable<WMS_Customer>> GetAllWMS_Customers()
        {
            Response<IEnumerable<WMS_Customer>> response = new Response<IEnumerable<WMS_Customer>>();

            WMS_CustomerAccessor accessor = new WMS_CustomerAccessor();

            try
            {
                response.Result = accessor.GetAllWMS_Customers();
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

        public Response<IEnumerable<WMS_Customer>> GetAllCustomer()
        {
            Response<IEnumerable<WMS_Customer>> response = new Response<IEnumerable<WMS_Customer>>();

            WMS_CustomerAccessor accessor = new WMS_CustomerAccessor();

            try
            {
                response.Result = accessor.GetAllCustomer();
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

        public IEnumerable<WMS_Customer> GetWMSCustomerByCustomerID(long customerid)
        {
            return new WMS_CustomerAccessor().GetWMSCustomerByCustomerID(customerid);
        }



        public Response<IEnumerable<BoxSize>> GetApplicationBox()
        {
            Response<IEnumerable<BoxSize>> response = new Response<IEnumerable<BoxSize>>();

            WMS_CustomerAccessor accessor = new WMS_CustomerAccessor();

            try
            {
                response.Result = accessor.GetApplicationBox();
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
        public string SaveCustomer(SaveCustomer request)
        {
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetCustomer request");
                LogError(ex);
                return "获得保存数据失败!";
            }
            WMS_CustomerAccessor accessor = new WMS_CustomerAccessor();
            Customer customer = new Customer();
            customer.Code = request.Code;
            customer.Name = request.Name;
            customer.Description = request.Description;
            try
            {
                if (accessor.SaveCustomer(customer) == -1)
                {
                    return "项目名称在数据中已存在!";
                }
                else
                {
                    return "保存成功!";
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                return "保存失败!" + ex.Message;
            }
        }

        public Response<WMS_Customer> DeleteWMS_Customer(string StorerKey, string CustomerID)
        {
            WMS_CustomerAccessor accessor = new WMS_CustomerAccessor();
            Response<WMS_Customer> response = new Response<WMS_Customer>();
            try
            {
                if (accessor.DeleteCustomer(StorerKey, CustomerID))
                {
                    response.IsSuccess = true;
                    response.SuccessMessage = "删除成功!";
                }
                else
                {
                    response.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.SuccessMessage = "删除失败!" + ex.Message;
            }
            return response;
        }

        public string UpdateCustomer(Customer customer)
        {
            WMS_CustomerAccessor accessor = new WMS_CustomerAccessor();
            try
            {
                return accessor.UpdateCustomer(customer) == -1 ? "项目名称在数据中已存在!" : "保存成功!";
            }
            catch (Exception ex)
            {
                LogError(ex);
                return "保存失败!" + ex.Message;
            }
        }

        public Response<IEnumerable<ProjectCustomer>> GetProjectCustomers()
        {
            Response<IEnumerable<ProjectCustomer>> response = new Response<IEnumerable<ProjectCustomer>>();

            try
            {
                WMS_CustomerAccessor accessor = new WMS_CustomerAccessor();
                response.Result = accessor.GetProjectCustomers();
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

        public Response<Customer> OperateCustomer(AddCustomerRequest request)
        {
            Response<Customer> response = new Response<Customer>();

            if (request == null || request.Customer == null)
            {
                ArgumentNullException ex = new ArgumentNullException("OperateCustomer request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                WMS_CustomerAccessor WMS_CustomerAccessor = new WMS_CustomerAccessor();
                response.Result = WMS_CustomerAccessor.InsertCustomer(request.Customer);
                if (response.Result.ID > 0)
                {
                    response.IsSuccess = true;
                }
                else
                {
                    response.IsSuccess = false;
                    response.ErrorCode = ErrorCode.Technical;
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.Exception = ex;
                response.ErrorCode = ErrorCode.Technical;
                throw;
            }
            return response;
        }

        public Response<Customer> UpdateCustomer(AddCustomerRequest request)
        {
            Response<Customer> response = new Response<Customer>();

            if (request == null || request.Customer == null)
            {
                ArgumentNullException ex = new ArgumentNullException("OperateCustomer request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                WMS_CustomerAccessor WMS_CustomerAccessor = new WMS_CustomerAccessor();
                response.Result = WMS_CustomerAccessor.UpdateCustomers(request.Customer);
                if (response.Result.ID > 0)
                {
                    response.IsSuccess = true;
                }
                else
                {
                    response.IsSuccess = false;
                    response.ErrorCode = ErrorCode.Technical;
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.Exception = ex;
                response.ErrorCode = ErrorCode.Technical;
                throw;
            }
            return response;
        }



        public Response<GetShippersByConditionResponse> GetShippersByConditon(GetShippersByConditionRequest request)
        {
            Response<GetShippersByConditionResponse> response = new Response<GetShippersByConditionResponse>() { Result = new GetShippersByConditionResponse() };
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetShippersByConditon request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                ShipperAccessor accessor = new ShipperAccessor();
                int rowCount;
                response.Result.Shippers = accessor.GetShippersByConditon(request.Code, request.Name, request.EnglishName, request.State, request.PageIndex, request.PageSize, request.ProjectId, out rowCount);
                response.Result.PageIndex = request.PageIndex;
                response.Result.PageCount = rowCount % request.PageSize == 0 ? rowCount / request.PageSize : rowCount / request.PageSize + 1;
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

        public Response<GetWMS_CustomerByConditionResponse> GetCustomerByConditon(GetWMS_CustomerByConditionRequest request)
        {
            Response<GetWMS_CustomerByConditionResponse> response = new Response<GetWMS_CustomerByConditionResponse>() { Result = new GetWMS_CustomerByConditionResponse() };
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetWMS_CustomerByConditon request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                WMS_CustomerAccessor accessor = new WMS_CustomerAccessor();
                int rowCount;
                response.Result.Customer = accessor.WMS_GetCustomerByConditon(request.CustomerID, request.StorerKey, request.Contact1, request.PhoneNum, request.Company, request.PageIndex, request.PageSize, out rowCount);
                response.Result.PageIndex = request.PageIndex;
                response.Result.PageCount = rowCount % request.PageSize == 0 ? rowCount / request.PageSize : rowCount / request.PageSize + 1;
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

        public WMS_Customer selectCustomer(string id, string customerid)
        {
            try
            {
                WMS_CustomerAccessor accessor = new WMS_CustomerAccessor();
                return accessor.selectCustomer(id, customerid);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 货主客户新增编辑
        /// </summary>
        public Response<IEnumerable<WMS_Customer>> AddCustomer(AddWMS_CustomerRequest request)
        {
            Response<IEnumerable<WMS_Customer>> response = new Response<IEnumerable<WMS_Customer>>();


            if (request == null || request.customers == null)
            {
                ArgumentNullException ex = new ArgumentNullException("AddWMS_Customer request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                WMS_CustomerAccessor accessor = new WMS_CustomerAccessor();
                response.Result = accessor.AddCustomers(request.customers);
                response.SuccessMessage = "保存成功!";
                response.IsSuccess = true;

            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
                response.SuccessMessage = "保存失败!" + ex.Message;
            }

            return response;
        }

    }
}