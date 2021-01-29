using Runbow.TWS.Common;
using Runbow.TWS.Dao;
using Runbow.TWS.Dao.POD;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.MessageContracts.POD.Adidas;
using Runbow.TWS.MessageContracts.POD.AKZO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Runbow.TWS.Biz
{
    public class ContractService : BaseService
    {
        /// <summary>
        /// 根据合同ID得到合同
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Contract GetContractByID(GetContractRequest request)
        {

            return new ContractAccessor().GetContractByID(request.ContractID);
        }

        /// <summary>
        /// 根据查询条件分页得到合同
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<GetContractByConditionResponse> GetContractByCondition(GetContractByConditionRequest request)
        {
            int count = 0;

            ///定义表，用于存储过程调用
            DataTable dt = new DataTable();
            //增加列

            dt.Columns.Add("CompanyCode");
            dt.Columns.Add("BusinessCode");
            dt.Columns.Add("DepartmentCode");
            dt.Columns.Add("ContractTypeCode");
            dt.Columns.Add("ContractStartDate_start");
            dt.Columns.Add("ContractStartDate_end");
            dt.Columns.Add("IsContractExtension");
            dt.Columns.Add("IsContractExpired");
            dt.Columns.Add("StampTax");
            dt.Columns.Add("Contract");
            dt.Columns.Add("SearchType1");
            dt.Columns.Add("SearchType2");

            //增加行
            dt.Rows.Add(request.SearchCondition.SCCompany, request.SearchCondition.SCBusiness, request.SearchCondition.SCDepartment,
                request.SearchCondition.SCContractType,
                //对日期类型的处理
                request.SearchCondition.SCContractStart_start == new DateTime(0001, 1, 1) ? new DateTime(1753, 1, 1) : request.SearchCondition.SCContractStart_start,
                request.SearchCondition.SCContractStart_end == new DateTime(0001, 1, 1) ? new DateTime(1753, 1, 1) : request.SearchCondition.SCContractStart_end,
                request.SearchCondition.SCExtended, request.SearchCondition.SCExpired, request.SearchCondition.StampTax, request.SearchCondition.SCContract, request.SearchType1, request.SearchType2);

            ///初始化响应类
            Response<GetContractByConditionResponse> response = new Response<GetContractByConditionResponse>();
            response.Result = new GetContractByConditionResponse();

            //为响应类赋值
            response.Result.ContractCollection = new ContractAccessor().GetContractByCondition(dt, request.PageIndex, request.PageSize, out count);
            response.Result.PageIndex = request.PageIndex;
            response.Result.PageCount = count;

            if (response.Result.ContractCollection.Count() > 0) 
            {
                response.IsSuccess = true;
            }

            return response;
        }

        /// <summary>
        /// 新增或者修改合同
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Contract AddOrUpdateContract(AddOrUpdateContractRequest request, out string message)
        {
            ///定义表数据，作为参数传给存储过程
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("CompanyCode");
            dt.Columns.Add("CompanyName");
            dt.Columns.Add("BusinessCode");
            dt.Columns.Add("BusinessName");
            dt.Columns.Add("DepartmentCode");
            dt.Columns.Add("DepartmentName");
            dt.Columns.Add("ContractTypeCode");
            dt.Columns.Add("ContractTypeName");
            dt.Columns.Add("ContractStartDate");
            dt.Columns.Add("ContractNumber");
            dt.Columns.Add("ContractContent");
            dt.Columns.Add("BusinessPartnerName");
            dt.Columns.Add("IsContractExtension");
            dt.Columns.Add("ContractExpireDate");
            dt.Columns.Add("IsContractExpired");
            dt.Columns.Add("StampTax");
            dt.Columns.Add("OldContractNumber");
            dt.Columns.Add("QualificationCertificate");
            dt.Columns.Add("PolStartDate");
            dt.Columns.Add("PolEndDate");
            dt.Columns.Add("IsPolExpired");
            dt.Columns.Add("AttachmentGroupID");
            dt.Columns.Add("Remark");
            dt.Columns.Add("Creator");
            dt.Columns.Add("CreateTime");
            dt.Columns.Add("Updator");
            dt.Columns.Add("UpdateTime");

            dt.Rows.Add(request.Contract.ID, request.Contract.CompanyCode, request.Contract.CompanyName, request.Contract.BusinessCode, request.Contract.BusinessName
                , request.Contract.DepartmentCode, request.Contract.DepartmentName, request.Contract.ContractTypeCode, request.Contract.ContractTypeName
                , request.Contract.ContractStartDate == new DateTime(0001, 1, 1) ? new DateTime(1753, 1, 1) : request.Contract.ContractStartDate
                , request.Contract.ContractNumber, request.Contract.ContractContent, request.Contract.BusinessPartnerName
                , request.Contract.IsContractExtension,
                request.Contract.ContractExpireDate == new DateTime(0001, 1, 1) ? new DateTime(1753, 1, 1) : request.Contract.ContractExpireDate,
                request.Contract.IsContractExpired, request.Contract.StampTax
                , request.Contract.OldContractNumber, request.Contract.QualificationCertificate,
                request.Contract.PolStartDate == new DateTime(0001, 1, 1) ? new DateTime(1753, 1, 1) : request.Contract.PolStartDate,
                request.Contract.PolEndDate == new DateTime(0001, 1, 1) ? new DateTime(1753, 1, 1) : request.Contract.PolEndDate
                , request.Contract.IsPolExpired, request.Contract.AttachmentGroupID, request.Contract.Remark, request.Contract.Creator,
                request.Contract.CreateTime == new DateTime(0001, 1, 1) ? new DateTime(1753, 1, 1) : request.Contract.CreateTime,
                 request.Contract.Updator, 
                request.Contract.UpdateTime==new DateTime(0001,1,1) ? new DateTime(1753,1,1):request.Contract.UpdateTime
                );

            return new ContractAccessor().AddOrUpdateContract(dt, out message);
        }

        /// <summary>
        /// 删除合同
        /// 根据ID删除合同
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteContract(long id)
        {
            return new ContractAccessor().DeleteContract(id);
        }

        /// <summary>
        /// 顺延合同
        /// 根据ID顺延合同
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ExtendContract(long id) 
        {
            return new ContractAccessor().ExtendContract(id);
        }

        /// <summary>
        /// 根绝类型取得到期或者即将到期合同
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<IEnumerable<Contract>> GetExpireContracts(GetExpireContractsReqeust request)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 合同延期
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<bool> ContractExtension(ContractExtensionRequest request)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 根据类型获取合同公参数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Response<CommonParamResponse> GetContractListByType(CommonParamRequest request)
        {
           Response<CommonParamResponse> response = new Response<CommonParamResponse>();
           response.Result = new CommonParamResponse();

            try
            {
                response.Result.configList = new ContractAccessor().GetContractListByType(request.ParamType);
            }
            catch (Exception ex) 
            {
                response.Exception=ex;
            }
            return response;
        }

         
    }       
}
