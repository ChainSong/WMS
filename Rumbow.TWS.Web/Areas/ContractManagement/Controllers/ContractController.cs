using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Web.Areas.ContractManagement.Models;
using Runbow.TWS.Biz;
using System.Data;
using UtilConstants = Runbow.TWS.Common.Constants;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.Web.Common;
using Runbow.TWS.Entity;
using Runbow.TWS.Common;
using System.Text;

namespace Runbow.TWS.Web.Areas.ContractManagement.Controllers
{
    public class ContractController : BaseController
    {
        /// <summary>
        /// 合同查询主界面
        /// </summary>
        /// <param name="SearchType1">合同类型</param>
        /// <param name="SearchType2">保单类型</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index(int? SearchType1,int? SearchType2)
        {
            


            ///合同管理的ViewModel
            QueryContractViewModel vm = new QueryContractViewModel();

            if (base.UserInfo.ProjectRoleID == 49)
            {///合同查看角色，需要去掉编辑、删除、顺延按钮
                vm.ShowEditButton = false;
            }
            else
            {
                vm.ShowEditButton = true;
            }


            BindDropDownList(ref vm);   //绑定下拉列表
            
            ///                                               
            vm.PageCount = 0;
            vm.PageIndex = 0;
            vm.SearchType1 = SearchType1 ?? 0;
            vm.SearchType2 = SearchType2 ?? 0;

            //增加逻辑

            return View(vm);
        }

        /// <summary>
        /// 点击查询按钮后，会触发此Action
        /// </summary>
        /// <param name="vm">页面ViewModel</param>
        /// <param name="PageIndex">分页当前页</param>
        /// <param name="Action">查询或者导出</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(QueryContractViewModel vm, int? PageIndex, string Action)
        {
            BindDropDownList(ref vm);   //重新绑定下拉


            ///合同的查询功能
            ///定义表数据，作为参数传给存储过程
            GetContractByConditionRequest request = new GetContractByConditionRequest();
            request.PageIndex = PageIndex ?? 0;
            request.SearchType1 = vm.SearchType1 ?? 0;         //合同类型
            request.SearchType2 = vm.SearchType2 ?? 0;        //保单类型
            request.PageSize = UtilConstants.PAGESIZE;        //页数量 
            request.SearchCondition = vm.SearchCondition;
            var response = new ContractService().GetContractByCondition(request);
            vm.ContractCollection = response.Result.ContractCollection;
            vm.PageCount = response.Result.PageCount / UtilConstants.PAGESIZE + ((response.Result.PageCount % UtilConstants.PAGESIZE) == 0 ? 0 : 1);
            vm.PageIndex = response.Result.PageIndex;


            if (response.IsSuccess && Action == "导出")
            {
                DataTable dt = new DataTable();
                dt = EnumerableExtension.ToDataTable<Contract>(response.Result.ContractCollection.ToList());             //List转化为Datatable

                ///对查询出来的列进行处理
                dt.Columns.Remove("ID");
                dt.Columns.Remove("CompanyCode");
                dt.Columns.Remove("BusinessCode");
                dt.Columns.Remove("DepartmentCode");
                dt.Columns.Remove("ContractTypeCode");
                dt.Columns.Remove("AttachmentGroupID");
                dt.Columns.Remove("IsPolExpired");
                dt.Columns.Remove("IsContractExpired");
                dt.Columns.Remove("Creator");
                dt.Columns.Remove("CreateTime");
                dt.Columns.Remove("Updator");
                dt.Columns.Remove("UpdateTime");
           

                dt.Columns["CompanyName"].ColumnName = "公司名称";
                dt.Columns["BusinessName"].ColumnName = "业务大类";
                dt.Columns["DepartmentName"].ColumnName = "部门名称";
                dt.Columns["ContractTypeName"].ColumnName = "合同种类";
                dt.Columns["ContractStartDate"].ColumnName = "合同起始日期";
                dt.Columns["ContractNumber"].ColumnName = "合同编号";
                dt.Columns["ContractContent"].ColumnName = "合同内容";
                dt.Columns["BusinessPartnerName"].ColumnName = "业务对方名称";
                dt.Columns["IsContractExtension"].ColumnName = "是否顺延";
                dt.Columns["ContractExpireDate"].ColumnName = "实际到期日期";
                dt.Columns["StampTax"].ColumnName = "是否含印花税";
                dt.Columns["OldContractNumber"].ColumnName = "老合同编号";
                dt.Columns["PolStartDate"].ColumnName = "保单起始日期";
                dt.Columns["PolEndDate"].ColumnName = "保单到期日期";
                dt.Columns["Remark"].ColumnName = "备注";
                dt.Columns["QualificationCertificate"].ColumnName = "证书";
  
 

                this.WriteExcel(dt, "Contract_" + UserInfo.Name + "_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");  //生成Excel
                return new EmptyResult();

            }
            return View(vm);
        }

        /// <summary>
        /// 新增编辑查询合同明细主界面
        /// ViewType=0  查询页面  ViewType=1 新增  ViewType=2 编辑
        /// </summary>
        /// <param name="id">id 有值，编辑  id无值， 新增</param>
        /// <param name="ViewType"> 页面类型</param>
        /// <param name="type"> 查询类型</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create(long? id, int? ViewType)
        {
            ContractOperationViewModel vm = new ContractOperationViewModel();
            BindDropDownList(ref vm);
            vm.Contract = new Contract();
            vm.Contract.AttachmentGroupID = Guid.NewGuid().ToString();  //新建一个GUID

            ///根据ID判断编辑修改状态
            if (id == null)
            {
                vm.ViewType = 1;   //新增操作
            }
            else
            {
                if (ViewType == 0)
                {//若为查看模式，则更新数据
                    vm.ViewType = 0;
                }
                else
                {//否则认为是修改模式
                    vm.ViewType = 2;   //修改操作
                    
                }

                GetContractRequest request1 = new GetContractRequest();
                request1.ContractID = id ?? 0;
                vm.Contract = new ContractService().GetContractByID(request1);
                vm.CertificateCodes= vm.Contract.QualificationCertificate.TrimEnd('|').Split('|').ToArray();
                vm.SelectedCertificates = new List<SelectListItem>();

                IList<SelectListItem> temp = new List<SelectListItem>();

                foreach (var item in vm.CertificateCodes) 
                {
                    temp.Add(new SelectListItem { Value = item, Text = item });
                }
                vm.SelectedCertificates = temp;
            }
            
            return View(vm);
        }

        /// <summary>
        /// 编辑/新增 合同相应Action
        /// </summary>
        /// <param name="vm">合同明细信息</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(ContractOperationViewModel vm)
        {
            AddOrUpdateContractRequest request = new AddOrUpdateContractRequest();
            GetContractRequest request1 = new GetContractRequest();
            string message="";
            request.Contract = vm.Contract;
            switch (vm.ViewType)
            {
                case 0: //查看
                    vm.Contract = new ContractService().GetContractByID(request1);
                    break;
                case 1:
                    //新增操作
                    request.Contract.Creator = base.UserInfo.Name;
                    request.Contract.CreateTime = DateTime.Now;

                    if (vm.CertificateCodes != null)
                    {///若有选中证书，则导入
                        ///证书导入
                        foreach (var item in vm.CertificateCodes)
                        {
                            request.Contract.QualificationCertificate += item + "|";
                        }
                    }

                    ///获取到更新的那条数据
                    vm.Contract = new ContractService().AddOrUpdateContract(request, out message);
             
                    break;
                case 2:
                    //修改操作
                    request.Contract.Updator = base.UserInfo.Name;
                    request.Contract.UpdateTime = DateTime.Now;


                    if (vm.CertificateCodes != null)
                    {///若有选中证书，则导入
                        ///证书导入
                        foreach (var item in vm.CertificateCodes)
                        {
                            request.Contract.QualificationCertificate += item + "|";
                        }
                    }

                    ///获取到更新的那条数据
                    vm.Contract = new ContractService().AddOrUpdateContract(request, out message);

                    break;
                default:
                    break;
            }

        

           
            vm.ViewType = 0;  //ViewType=0, 编辑或者新增后，直接进入查看页面
            return View(vm);
        }

        /// <summary>
        /// 查看合同是否到期和即将到期Action
        /// </summary>
        /// <param name="ExpireType"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ContractExpires(int ExpireType)
        {
            ContractExpiresViewModel vm = new ContractExpiresViewModel();
            vm.ContractExpireType = ExpireType;
            
            //增加逻辑
            return View(vm);
        }

        /// <summary>
        /// 合同延期，点击合同延期按钮，合同到期日期自动增加364天.已Ajax方式调用
        /// </summary>
        /// <param name="id">合同ID</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ContractExtension(long id)
        {
            if (new ContractService().ExtendContract(id))
            {//延迟成功
                return Json(new { Message = "延迟成功", IsSuccess = true });
            }
            else
            {//延迟失败
                return Json(new { Message = "延迟失败！", IsSuccess = false });
            }
            
        } 

        /// <summary>
        /// 删除合同Action
        /// </summary>
        /// <param name="id">合同ID</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteContract(long id)
        {
            if (new ContractService().DeleteContract(id)) 
            {//删除成功
                return Json(new { Message = "删除成功", IsSuccess = true });
            }
            else
            {//删除失败
                return Json(new { Message = "删除失败！", IsSuccess = false });
            }
            
        }

        /// <summary>
        /// 绑定下拉列表
        /// 查询阶段
        /// </summary>
        public void BindDropDownList(ref QueryContractViewModel vm)
        {
            vm.YesNOList = new List<SelectListItem>()
            { 
                new SelectListItem(){Value="Y",Text="是"},
                new SelectListItem(){Value="N",Text="否"}
            };
            CommonParamRequest request = new CommonParamRequest();
            Response<CommonParamResponse> response = new Response<CommonParamResponse>();
            response.Result = new Response<CommonParamResponse>().Result;
            IList<SelectListItem> list = new List<SelectListItem>();

            //所属公司
            request.ParamType = "CompanyOfAffiliation";
            response = new ContractService().GetContractListByType(request);
            foreach(var item in response.Result.configList){
                list.Add(new SelectListItem { Value = item.Code, Text = item.Name });
            }
            vm.CompanyList = list; ;

            ///清空值
            request = new CommonParamRequest();
            response = new Response<CommonParamResponse>();
            list = new List<SelectListItem>();

            ///业务大类
            request.ParamType = "Business";
            response = new ContractService().GetContractListByType(request);
            foreach (var item in response.Result.configList)
            {
                list.Add(new SelectListItem { Value = item.Code, Text = item.Name });
            }
            vm.BusinessList = list;

            ///清空值
            request = new CommonParamRequest();
            response = new Response<CommonParamResponse>();
            list = new List<SelectListItem>();

            //合同类型
            request.ParamType = "ContractType";
            response = new ContractService().GetContractListByType(request);
            foreach (var item in response.Result.configList)
            {
                list.Add(new SelectListItem { Value = item.Code, Text = item.Name });
            }
            vm.ContractTypeList =list;

            ///清空值
            request = new CommonParamRequest();
            response = new Response<CommonParamResponse>();
            list = new List<SelectListItem>();

            //部门列表
            request.ParamType = "Department";
            response = new ContractService().GetContractListByType(request);
            foreach (var item in response.Result.configList)
            {
                list.Add(new SelectListItem { Value = item.Code, Text = item.Name });
            }
            vm.DepartmentList = list;
         
            
        }

        /// <summary>
        /// 绑定下拉列表
        /// 新增或修改阶段
        /// </summary>
        /// <param name="vm"></param>
        public void BindDropDownList(ref ContractOperationViewModel vm) 
        {
            vm = new ContractOperationViewModel();
            vm.YesNOList = new List<SelectListItem>()
            { 
                new SelectListItem(){Value="Y",Text="是"},
                new SelectListItem(){Value="N",Text="否"}
            };

            CommonParamRequest request = new CommonParamRequest();
            Response<CommonParamResponse> response = new Response<CommonParamResponse>();
            response.Result = new Response<CommonParamResponse>().Result;
            IList<SelectListItem> list = new List<SelectListItem>();

            //所属公司
            request.ParamType = "CompanyOfAffiliation";
            response = new ContractService().GetContractListByType(request);
            foreach (var item in response.Result.configList)
            {
                list.Add(new SelectListItem { Value = item.Code, Text = item.Name });
            }
            vm.CompanyList = list;

            ///清空值
             request = new CommonParamRequest();
             response = new Response<CommonParamResponse>();
             list = new List<SelectListItem>();

            ///业务大类
            request.ParamType = "Business";
            response = new ContractService().GetContractListByType(request);
            foreach (var item in response.Result.configList)
            {
                list.Add(new SelectListItem { Value = item.Code, Text = item.Name });
            }
            vm.BusinessList = list;

            ///清空值
            request = new CommonParamRequest();
            response = new Response<CommonParamResponse>();
            list = new List<SelectListItem>();

            //合同类型
            request.ParamType = "ContractType";
            response = new ContractService().GetContractListByType(request);
            foreach (var item in response.Result.configList)
            {
                list.Add(new SelectListItem { Value = item.Code, Text = item.Name });
            }
            vm.ContractTypeList = list;

            ///清空值
            request = new CommonParamRequest();
            response = new Response<CommonParamResponse>();
            list = new List<SelectListItem>();

            //部门列表
            request.ParamType = "Department";
            response = new ContractService().GetContractListByType(request);
            foreach (var item in response.Result.configList)
            {
                list.Add(new SelectListItem { Value = item.Code, Text = item.Name });
            }
            vm.DepartmentList = list;



            ///清空值
            request = new CommonParamRequest();
            response = new Response<CommonParamResponse>();
            list = new List<SelectListItem>();

            //证件
            request.ParamType = "Certificate";
            response = new ContractService().GetContractListByType(request);
            foreach (var item in response.Result.configList)
            {
                list.Add(new SelectListItem { Value = item.Code, Text = item.Name });
            }
            vm.AvaliableCertificates = list;

  
        }


        /// <summary>
        /// 根据datatable生成Excel
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="path"></param>
        public void WriteExcel(DataTable dt, string fileName)
        {
            try
            {
                var sbHtml = new StringBuilder();
                sbHtml.Append("<style>td{mso-number-format:\"\\@\";}</style>");
                sbHtml.Append("<table border='1' cellspacing='0' cellpadding='0'>");
                sbHtml.Append("<tr>");
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sbHtml.AppendFormat("<td style='font-size: 14px;text-align:center;background-color: #DCE0E2; font-weight:bold;' height='25'>{0}</td>", dt.Columns[i].ColumnName);
                }

                sbHtml.Append("</tr>");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sbHtml.Append("<tr>");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", dt.Rows[i][j].ToString());
                    }
                    sbHtml.Append("</tr>");
                }

                sbHtml.Append("</table>");
                Response.Charset = "UTF-8";
                Response.HeaderEncoding = Encoding.UTF8;
                Response.AppendHeader("content-disposition", "attachment;filename=" + fileName);
                Response.ContentEncoding = Encoding.UTF8;
                Response.ContentType = "application/ms-excel";
                Response.Write("<meta http-equiv='content-type' content='application/ms-excel; charset=UTF-8'/>" + sbHtml.ToString());
                Response.Flush();
                Response.End();

            }
            catch (Exception ex)
            {

            }
        }


    }
}
