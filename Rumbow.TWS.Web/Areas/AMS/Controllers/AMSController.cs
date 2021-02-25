using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Biz;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.Web.Areas.Finance.Models;
using Runbow.TWS.Web.Common;
using UtilConstants = Runbow.TWS.Common.Constants;
using WebConstants = Runbow.TWS.Web.Common.Constants;
using Runbow.TWS.Common;
using System.IO;
using MyFile = System.IO.File;
using System.Data;
using Runbow.TWS.MessageContracts.AMS;
using Runbow.TWS.Web.Areas.AMS.Models;
using System.Text;
using Runbow.TWS.Common;
namespace Runbow.TWS.Web.Areas.AMS.Controllers
{
    public class AMSController : BaseController
    {
        //上传回单
        [HttpGet]
        public ActionResult ReplyDocument_Upload()
        {
            ViewBag.Customers = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID)
                                       .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            return View();
        }
        [HttpPost]
        public string ReplyDocument_Upload(long customer, string customerName)
        {
            //新路径
            string uploadFolderPath = Runbow.TWS.Common.Constants.UPLOAD_AMS_PATH;
            string targetPath = Path.Combine(uploadFolderPath, customerName, DateTime.Now.ToString("yyyy-MM"));
            DateTime createDate;
            string url = string.Empty, actualNameInServer = string.Empty, displayName = string.Empty, ext = string.Empty;

            IList<AMSUpload> amsUpload = new List<AMSUpload>();

            if (string.IsNullOrEmpty(targetPath) || !Path.IsPathRooted(targetPath))
            {
                return new { msg = "程序出错！" }.ToJsonString();
            }
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                if (hpf.ContentLength > 0)
                {
                    if (!Directory.Exists(targetPath))
                    {
                        Directory.CreateDirectory(targetPath);
                    }
                    displayName = Path.GetFileName(hpf.FileName);
                    ext = Path.GetExtension(hpf.FileName);

                    if (!ext.ToLower().Equals(".zip"))
                    {
                        return new { msg = "批量上传，请用zip格式压缩" }.ToJsonString();
                    }
                    actualNameInServer = UserInfo.ProjectName + DateTime.Now.ToString("yyyyMMddHHmmss") + ext;
                    url = Path.Combine(targetPath, actualNameInServer);
                    hpf.SaveAs(url);
                    hpf.InputStream.Close();
                    if (ext.ToLower().Equals(".zip"))
                    {
                        IList<string> unZipedFileName = new List<string>();
                        ZipHelper.UnZip(url, targetPath, unZipedFileName);
                        MyFile.Delete(url);
                        unZipedFileName.Each((k, fileName) =>
                        {
                            actualNameInServer = Path.GetFileName(fileName);
                            ext = Path.GetExtension(fileName);
                            displayName = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(actualNameInServer));
                            createDate = DateTime.Now;
                            amsUpload.Add(new AMSUpload() { FileName = displayName, FileType = ext, ServerName = actualNameInServer, FilePath = fileName, ProjectID = customer, ProjectName = customerName, OrderNo = "", Creator = UserInfo.Name, CreateTime = createDate, Updator = "", UpdateTime = createDate, Status = false });
                        });
                    }
                    else
                    {
                        amsUpload.Add(new AMSUpload() { FileName = displayName, FileType = ext, ServerName = actualNameInServer, FilePath = url, ProjectID = UserInfo.ProjectID, ProjectName = UserInfo.ProjectName, OrderNo = "", Creator = UserInfo.Name, CreateTime = DateTime.Now, Updator = "", UpdateTime = DateTime.Now, Status = false });
                    }

                    AMSUploadService service = new AMSUploadService();
                    //查询已存在的记录
                    Response<IEnumerable<AMSUpload>> resams = service.GetAMSUpload(new AddAMSUploadRequest() { amsUpload = amsUpload });                   

                    //执行新增修改操作
                    Response<IEnumerable<AMSUpload>> response = service.AddAMSUpload(new AddAMSUploadRequest() { amsUpload = amsUpload });
                    if (response.IsSuccess)
                    {
                        #region 删除已存在记录的图片
                        if (resams.IsSuccess)
                        {
                            if (resams.Result != null)
                            {
                                foreach (AMSUpload a in resams.Result)
                                {
                                    if (MyFile.Exists(a.FilePath)) MyFile.Delete(a.FilePath);
                                }
                            }
                        } 
                        #endregion
                        return new { result = "批量上传文件成功!", IsSuccess = true }.ToJsonString();
                    }
                }
                return new { result = "文件内容为空", IsSuccess = false }.ToJsonString();
            }

            return new { result = "请选择文件", IsSuccess = false }.ToJsonString();
        }

        //type=0 项目回单查询  | type =1 EHS回单查询   |  type=2  装箱单查询 |  type=3 生成装箱单页面的查询
        [HttpGet]
        public ActionResult QueryReplyDocument(int type)
        {
            QueryReplyDocumentViewModel vm = new QueryReplyDocumentViewModel();
            vm.SearchCondition = new AMSSearchCondition();
            vm.Type = type;
            vm.PageIndex = 0;
            vm.PageCount = 0;
            vm.Customers = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID)
                                        .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            return View(vm);
        }

        [HttpPost]
        public ActionResult QueryReplyDocument(QueryReplyDocumentViewModel vm, int? PageIndex)
        {
            vm.Customers = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID)
                                         .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            
            StringBuilder sb = new StringBuilder();
            foreach (var i in vm.Customers)
            {
                sb.Append("'" + i.Text + "',");
            }

            var response = new AMSUploadService().QueryAMSUpload(new QueryAMSUploadRequests()
            {
                PageIndex = PageIndex ?? 0,
                PageSize = UtilConstants.PAGESIZE,
                SearchCondition = vm.SearchCondition,
                Customers = sb.ToString().Substring(0, sb.Length - 1).ToString()
            });

            if (response.IsSuccess)
            {
                vm.AMSUploadCollection = response.Result.AMSUploadCollection;
                vm.PageCount = response.Result.PageCount;
                vm.PageIndex = response.Result.PageIndex;
            }
            return View(vm);
        }

        //Excel上传验证
        [HttpGet]
        public ActionResult ExcelCheck()
        {
            return View();
        }
        [HttpPost]
        public string ExcelCheck(string customer)
        {
            string uploadFolderPath = Runbow.TWS.Common.Constants.UPLOAD_FOLDER_PATH;
            string targetPath = Path.Combine(uploadFolderPath, "AMSTempFile");
            string url = string.Empty, actualNameInServer = string.Empty, ext = string.Empty;
            DateTime amsDate = new DateTime();

            if (string.IsNullOrEmpty(targetPath) || !Path.IsPathRooted(targetPath))
            {
                return new { msg = "程序出错！" }.ToJsonString();
            }

            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                if (hpf.ContentLength > 0)
                {
                    if (!Directory.Exists(targetPath))
                    {
                        Directory.CreateDirectory(targetPath);
                    }
                    ext = Path.GetExtension(hpf.FileName);
                    if (!ext.ToLower().Equals(".xls") && !ext.ToLower().Equals(".xlsx"))
                    {
                        return new { msg = "请选择Excel格式的文件" }.ToJsonString();
                    }

                    actualNameInServer = DateTime.Now.ToString("yyyyMMddHHmmss") + ext;
                    url = Path.Combine(targetPath, actualNameInServer);
                    hpf.SaveAs(url);
                    hpf.InputStream.Close();
                    Runbow.TWS.Common.ExcelHelper excelHelper = new Runbow.TWS.Common.ExcelHelper(url);
                    DataSet ds = excelHelper.GetAllDataFromAllSheets();
                    excelHelper.Dispose();
                    MyFile.Delete(url);
                    AMSUploadService service = new AMSUploadService();
                    Response<IEnumerable<AMSUpload>> response = service.GetAMSUpload(new AddAMSUploadRequest() { amsUpload = null });
                    if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0 && response.Result.Count() > 0)
                    {
                        StringBuilder results = new StringBuilder();
                        StringBuilder ids = new StringBuilder();
                        string projectName, fileName;
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            DataRow row = ds.Tables[0].Rows[i];
                            projectName = row[0].ToString().Trim().ToUpper();
                            fileName = row[1].ToString().Trim().ToUpper();
                            if (projectName == "AMS_永兴东润")
                            {
                                amsDate = Convert.ToDateTime(row[2].ToString());
                                fileName = amsDate.ToString("yyyy-MM-") + fileName;
                            }
                            
                            bool isExist = false; 
                            foreach (AMSUpload ams in response.Result)
                            {
                                if (projectName == ams.ProjectName.Trim().ToUpper() && fileName == ams.FileName.Trim().ToUpper())
                                {
                                    ids.Append("'" + ams.ID.ToString() + "',");
                                    isExist = true;
                                    //if (projectName != "AMS_永兴东润") //AMS_永兴东润
                                    //{
                                    //    break;
                                    //}
                                }
                            }

                            if (!isExist)
                            {
                                results.Append("[" + row[0].ToString() + "]客户中[" + row[1].ToString() + "]运单号  未上传回单图片<br />");
                            }
                        }
                        if (ids.Length > 0)
                        {
                            service.UpdateAMSUploadStatus(new AddAMSUploadRequest() { Ids = ids.ToString() });
                        }
                        if (results.Length <= 0)
                        {
                            results.Append("回单图片都已上传.");
                        }
                        return new { result = results.ToString(), IsSuccess = true }.ToJsonString();
                    }
                }
                return new { result = "文件内容为空", IsSuccess = false }.ToJsonString();
            }
            return new { result = "请选择文件", IsSuccess = false }.ToJsonString();
        }

        //手动导入回单
        [HttpGet]
        public ActionResult ManualImportReplyDocument()
        {
            //ViewData["Mess"] = this.GetImportReplyDocument();
            ViewBag.PODInfo = this.GetImportReplyDocument();
            return View();
        }
        //手动导入回单
        [HttpPost]
        public string SetImportReplyDocument()
        {
            string uploadFolderPath = Runbow.TWS.Common.Constants.UPLOAD_AMS_PATH;//新路径
            AMSUploadService service = new AMSUploadService();
            Response<IEnumerable<Attachment>> response = service.GetAttachmentOrAMS();
            if (response.IsSuccess)
            {
                string destFileName = string.Empty;
                try
                {
                    foreach (Attachment at in response.Result)
                    {
                        destFileName = Path.Combine(uploadFolderPath, at.Creator, DateTime.Now.ToString("yyyy-MM"));
                        if (!Directory.Exists(destFileName))
                        {
                            Directory.CreateDirectory(destFileName);
                        }
                        destFileName = Path.Combine(destFileName, at.ActualNameInServer);
                        if (MyFile.Exists(at.Url)) MyFile.Copy(at.Url, destFileName, true);
                    }

                    service.InsertAMSUpload();
                    return new { result = "导入成功", IsSuccess = true }.ToJsonString();
                }
                catch
                {
                    return new { result = "导入失败", IsSuccess = false }.ToJsonString();
                }
            }

            return new { result = "手动导入完成", IsSuccess = true }.ToJsonString();
        }
        private string GetImportReplyDocument()
        {
            StringBuilder result = new StringBuilder();
            AMSUploadService service = new AMSUploadService();
            Response<IEnumerable<AMSUpload>> respose = service.GetAMSUploadOrPODInfo();
            string amsTime = DateTime.Now.AddMonths(-2).ToString("yyyy-MM") + "-01";
            result.Append("(以下为" + amsTime + "至今天所有运单的回单明细，如果AMS当前同步数不等于TMS已上传附件数，请点击 [手动导入] 按钮)<br />");
            if (respose.IsSuccess)
            {
                foreach (AMSUpload ams in respose.Result)
                {
                    result.Append(ams.ProjectName + "全部运单总数：" + ams.ProjectID);
                    result.Append(",TMS中已上传附件数：" + ams.ID);
                    result.Append(",AMS中当前同步数：" + ams.FileType + "<br />");
                }
            }

            return result.ToString();
        }

        //生成装箱单页面查询
        [HttpGet]
        public ActionResult GenBoxNumber()
        {
            GenBoxNumberViewModel vm = new GenBoxNumberViewModel();
            vm.SearchCondition = new AMSSearchCondition();
            vm.PageIndex = 0;
            vm.PageCount = 0;
            vm.Customers = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID)
                .Select(c => new SelectListItem { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            vm.SearchCondition.StatUpLoadTime = DateTime.Now.AddDays(-7);
            vm.SearchCondition.StateID = 0;
            return View(vm);
        }
        [HttpPost]
        public ActionResult GenBoxNumber(GenBoxNumberViewModel vm, int? PageIndex)
        {

            vm.Customers = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID)
                .Select(c => new SelectListItem { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            //var CustomerText = Json(vm.Customers.Select(p => new { p.Text})).ToJsonString();
            StringBuilder sb = new StringBuilder();
            foreach (var i in vm.Customers)
            {
                sb.Append("'" + i.Text + "',");
            }
            var response = new AMSUploadService().QueryGenBoxNumber(new GenBoxNumberRequest()
            {
                PageIndex = PageIndex ?? 0,
                PageSize = UtilConstants.PAGESIZE,
                SearchCondition = vm.SearchCondition,
                Customers =sb.ToString().Substring(0, sb.Length - 1).ToString(),
            });
            if (response.IsSuccess)
            {
                vm.AMSUploadCollection = response.Result.AMSUploadCollection;
            }
            return View(vm);
        }

        public JsonResult GenBoxNumbers(string sql)
        {
            GenBoxNumberViewModel gb = new GenBoxNumberViewModel();
            var response = new AMSUploadService().AddGenBoxNumber(new GenBoxNumberRequest()
            {
                Check = sql,
            });
            if (response.IsSuccess)
            {
                return Json(response.Result.AMSUploadCollection.Select(p => new{ p.ID,p.OrderNo }));
                
            }
            throw response.Exception;
        }

        /// <summary>
        /// 点击查看图片
        /// </summary>
        public ActionResult GetAttachment(long? id)
        {
            if (id.HasValue)
            {
                AMSUploadService service = new AMSUploadService();
                Response<AMSUpload> resp = service.GetAttachmentByID(new GetAMSUPLOADByIDRequest() { ID = id.Value });

                if (resp.IsSuccess)
                {
                    string encode = string.Empty;
                    if (resp.Result.ServerName.EndsWith("xlsx", StringComparison.OrdinalIgnoreCase) || resp.Result.FileType.EndsWith("xls", StringComparison.OrdinalIgnoreCase))
                    {
                        encode = "application/vnd.ms-excel";
                    }
                    else if (resp.Result.ServerName.EndsWith("bmp", StringComparison.OrdinalIgnoreCase))
                    {
                        encode = "image/x-ms-bmp";
                    }
                    else if (resp.Result.ServerName.EndsWith("jpeg", StringComparison.OrdinalIgnoreCase) || resp.Result.ServerName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) || resp.Result.ServerName.EndsWith("jpe", StringComparison.OrdinalIgnoreCase))
                    {
                        encode = "image/jpeg";
                    }
                    else
                    {
                        encode = "application";
                    }
                    return File(resp.Result.FilePath, encode, resp.Result.ServerName.ToUtf8String());
                }
            }

            return new EmptyResult();
        }

        /// <summary>
        /// 线路管理
        /// </summary>
        [HttpGet]
        public ActionResult Line()
        {
            GenBoxNumberViewModel vm = new GenBoxNumberViewModel();
            vm.SearchCondition = new AMSSearchCondition();
            vm.PageIndex = 0;
            vm.PageCount = 0;
            vm.Customers = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID)
                .Select(c => new SelectListItem { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            vm.SearchCondition.StatUpLoadTime = DateTime.Now.AddDays(-7);
            vm.SearchCondition.StateID = 0;
            return View(vm);
        }
        [HttpPost]
        public ActionResult Line(GenBoxNumberViewModel vm, int? PageIndex)
        {

            vm.Customers = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID)
                .Select(c => new SelectListItem { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            //var CustomerText = Json(vm.Customers.Select(p => new { p.Text})).ToJsonString();
            StringBuilder sb = new StringBuilder();
            foreach (var i in vm.Customers)
            {
                sb.Append("'" + i.Text + "',");
            }
            var response = new AMSUploadService().QueryGenBoxNumber(new GenBoxNumberRequest()
            {
                PageIndex = PageIndex ?? 0,
                PageSize = UtilConstants.PAGESIZE,
                SearchCondition = vm.SearchCondition,
                Customers = sb.ToString().Substring(0, sb.Length - 1).ToString(),
            });
            if (response.IsSuccess)
            {
                vm.AMSUploadCollection = response.Result.AMSUploadCollection;
            }
            return View(vm);
        }


        /// <summary>
        /// 快递分配
        /// </summary>
        [HttpGet]
        public ActionResult PodAssign()
        {
            QueryReplyDocumentViewModel vm = new QueryReplyDocumentViewModel();
            vm.SearchCondition = new AMSSearchCondition();
            vm.Type = 0;
            vm.PageIndex = 0;
            vm.PageCount = 0;
            vm.Customers = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID)
                                        .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            return View(vm);
        }
        [HttpPost]
        public ActionResult PodAssign(QueryReplyDocumentViewModel vm, int? PageIndex)
        {
            vm.Customers = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID)
                                         .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });

            StringBuilder sb = new StringBuilder();
            foreach (var i in vm.Customers)
            {
                sb.Append("'" + i.Text + "',");
            }

            var response = new AMSUploadService().QueryAMSUpload(new QueryAMSUploadRequests()
            {
                PageIndex = PageIndex ?? 0,
                PageSize = UtilConstants.PAGESIZE,
                SearchCondition = vm.SearchCondition,
                Customers = sb.ToString().Substring(0, sb.Length - 1).ToString()
            });

            if (response.IsSuccess)
            {
                vm.AMSUploadCollection = response.Result.AMSUploadCollection;
                vm.PageCount = response.Result.PageCount;
                vm.PageIndex = response.Result.PageIndex;
            }
            return View(vm);
        }

        /// <summary>
        /// 快递交接
        /// </summary>
        [HttpGet]
        public ActionResult AddExpress()
        {
            QueryReplyDocumentViewModel vm = new QueryReplyDocumentViewModel();
            vm.SearchCondition = new AMSSearchCondition();
            vm.Type = 0;
            vm.PageIndex = 0;
            vm.PageCount = 0;
            vm.Customers = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID)
                                        .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            return View(vm);
        }
        [HttpPost]
        public ActionResult AddExpress(QueryReplyDocumentViewModel vm, int? PageIndex)
        {
            vm.Customers = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID)
                                         .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });

            StringBuilder sb = new StringBuilder();
            foreach (var i in vm.Customers)
            {
                sb.Append("'" + i.Text + "',");
            }

            var response = new AMSUploadService().QueryAMSUpload(new QueryAMSUploadRequests()
            {
                PageIndex = PageIndex ?? 0,
                PageSize = UtilConstants.PAGESIZE,
                SearchCondition = vm.SearchCondition,
                Customers = sb.ToString().Substring(0, sb.Length - 1).ToString()
            });

            if (response.IsSuccess)
            {
                vm.AMSUploadCollection = response.Result.AMSUploadCollection;
                vm.PageCount = response.Result.PageCount;
                vm.PageIndex = response.Result.PageIndex;
            }
            return View(vm);
        }

        /// <summary>
        /// 快递跟踪
        /// </summary>
        [HttpGet]
        public ActionResult ExpressTrack()
        {
            QueryReplyDocumentViewModel vm = new QueryReplyDocumentViewModel();
            vm.SearchCondition = new AMSSearchCondition();
            vm.Type = 0;
            vm.PageIndex = 0;
            vm.PageCount = 0;
            vm.Customers = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID)
                                        .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            return View(vm);
        }
        [HttpPost]
        public ActionResult ExpressTrack(QueryReplyDocumentViewModel vm, int? PageIndex)
        {
            vm.Customers = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID)
                                         .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });

            StringBuilder sb = new StringBuilder();
            foreach (var i in vm.Customers)
            {
                sb.Append("'" + i.Text + "',");
            }

            var response = new AMSUploadService().QueryAMSUpload(new QueryAMSUploadRequests()
            {
                PageIndex = PageIndex ?? 0,
                PageSize = UtilConstants.PAGESIZE,
                SearchCondition = vm.SearchCondition,
                Customers = sb.ToString().Substring(0, sb.Length - 1).ToString()
            });

            if (response.IsSuccess)
            {
                vm.AMSUploadCollection = response.Result.AMSUploadCollection;
                vm.PageCount = response.Result.PageCount;
                vm.PageIndex = response.Result.PageIndex;
            }
            return View(vm);
        }

    }
}
