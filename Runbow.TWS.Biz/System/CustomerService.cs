using System;
using System.Collections.Generic;
using Runbow.TWS.Common;
using Runbow.TWS.Dao;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;

namespace Runbow.TWS.Biz
{
    public class CustomerService : BaseService
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

            CustomerAccessor accessor = new CustomerAccessor();

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
            CustomerAccessor accessor = new CustomerAccessor();
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

        public Response<IEnumerable<Customer>> GetAllCustomers()
        {
            Response<IEnumerable<Customer>> response = new Response<IEnumerable<Customer>>();

            CustomerAccessor accessor = new CustomerAccessor();

            try
            {
                response.Result = accessor.GetAllCustomers();
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

        public Response<IEnumerable<Customer>> GetAllCustomer()
        {
            Response<IEnumerable<Customer>> response = new Response<IEnumerable<Customer>>();

            CustomerAccessor accessor = new CustomerAccessor();

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
        public Response<IEnumerable<BoxSize>> GetApplicationBox()
        {
            Response<IEnumerable<BoxSize>> response = new Response<IEnumerable<BoxSize>>();

            CustomerAccessor accessor = new CustomerAccessor();

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
            CustomerAccessor accessor = new CustomerAccessor();
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

        public Response<Customer> DeleteCustomer(long ID)
        {
            CustomerAccessor accessor = new CustomerAccessor();
            Response<Customer> response = new Response<Customer>();
            try
            {
                if (accessor.DeleteCustomer(ID))
                {
                    response.IsSuccess = true;
                    response.SuccessMessage= "删除成功!";
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
                response.SuccessMessage= "删除失败!" + ex.Message;
            }
            return response;
        }

        public string UpdateCustomer(Customer customer)
        {
            CustomerAccessor accessor = new CustomerAccessor();
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
                CustomerAccessor accessor = new CustomerAccessor();
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
                CustomerAccessor CustomerAccessor = new CustomerAccessor();
                response.Result = CustomerAccessor.InsertCustomer(request.Customer);
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
                CustomerAccessor CustomerAccessor = new CustomerAccessor();
                response.Result = CustomerAccessor.UpdateCustomers(request.Customer);
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
                response.Result.Shippers = accessor.GetShippersByConditon(request.Code, request.Name, request.EnglishName, request.State, request.PageIndex, request.PageSize,request.ProjectId, out rowCount);
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

        public Response<GetCustomerByConditionResponse> GetCustomerByConditon(GetCustomerByConditionRequest request)
        {
            Response<GetCustomerByConditionResponse> response = new Response<GetCustomerByConditionResponse>() { Result = new GetCustomerByConditionResponse() };
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetCustomerByConditon request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                CustomerAccessor accessor = new CustomerAccessor();
                int rowCount;
                response.Result.Customer = accessor.GetCustomerByConditon(request.Code, request.Name,request.UserId,request.ProjectId,request.StoreType , request.State, request.PageIndex, request.PageSize, out rowCount);
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

        public Customer selectCustomer(int id)
        {
            try
            {
                 CustomerAccessor accessor = new CustomerAccessor();
                 return accessor.selectCustomer(id);
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        /// <summary>
        /// 货主客户新增编辑
        /// </summary>
        public Response<IEnumerable<Customer>> AddCustomer(AddCustomerRequest request, string ID, string UserName, int projectId, int segmentId)
        {
            Response<IEnumerable<Customer>> response = new Response<IEnumerable<Customer>>();

            if (request == null || request.customers == null)
            {
                ArgumentNullException ex = new ArgumentNullException("AddCustomer request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                CustomerAccessor accessor = new CustomerAccessor();
                response.Result = accessor.AddCustomers(request.customers, ID, UserName, projectId, segmentId);
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