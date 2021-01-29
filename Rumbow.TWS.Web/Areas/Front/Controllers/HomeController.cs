using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Biz;
using Runbow.TWS.Common;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.Web.Common;
using MyFile = System.IO.File;
using Runbow.TWS.Web.Areas.AMS.Models;
using UtilConstants = Runbow.TWS.Common.Constants;
using System.Net;

namespace Runbow.TWS.Web.Areas.Front.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View(GetMenu());
        }

        public ActionResult Welcome()
        {
            ViewBag.Message = "";
            return View(base.UserInfo);
        }

        public ActionResult LogOff()
        {
            Session.Abandon();

            return RedirectToAction("", "../Login");
        }

        public ActionResult WelcomePage()
        {
            ViewBag.WmsUserName = base.UserInfo.Name;
            //TODO:海润光伏特殊需求
            var customers = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID);
            if (customers != null && customers.Any())
            {
                if (customers.First().CustomerID == 26)
                {
                    var StartTime = DateTime.Parse(DateTime.Now.AddDays(-1).DateTimeToString());
                    var EndTime = DateTime.Parse(DateTime.Now.AddDays(1).DateTimeToString());
                    var currentTime = DateTime.Parse(DateTime.Now.DateTimeToString());
                    var getPodResult = new PodService().QueryPodWithNoPaging(
                        new QueryPodRequest() { SearchCondition = new PodSearchCondition(){
                         ActualDeliveryDate = StartTime,
                         EndActualDeliveryDate = EndTime,
                         CustomerID = 26  
                        }, ProjectID = base.UserInfo.ProjectID });
                    if (getPodResult.IsSuccess)
                    {
                        decimal yesterdayAmt = 0, todayAmt = 0, tomorrowAmt = 0;
                        int yesterdayPodNumber = 0, todayPodNumber = 0, tomorrowPodNumber = 0;
                        StringBuilder errorAmtMessage = new StringBuilder();
                        StringBuilder Messages = new StringBuilder();
                        getPodResult.Result.Each((i, p) => {
                            if (p.ActualDeliveryDate < currentTime)
                            {
                                yesterdayPodNumber++;
                                decimal amtYesterday;
                                if (!string.IsNullOrEmpty(p.Str1) && decimal.TryParse(p.Str1, out amtYesterday))
                                {
                                    yesterdayAmt += amtYesterday;
                                }
                                else
                                {
                                    errorAmtMessage.Append("运单:").Append(p.CustomerOrderNumber).Append("运单价值未填或输入有误").Append("<br/>");
                                }
                            }
                            else if (p.ActualDeliveryDate >= currentTime && p.ActualDeliveryDate < EndTime)
                            {
                                todayPodNumber++;
                                decimal amtToday;
                                if (!string.IsNullOrEmpty(p.Str1) && decimal.TryParse(p.Str1, out amtToday))
                                {
                                    todayAmt += amtToday;
                                }
                                else
                                {
                                    errorAmtMessage.Append("运单:").Append(p.CustomerOrderNumber).Append("运单价值未填或输入有误").Append("<br/>");
                                }
                            }
                            else if (p.ActualDeliveryDate >= EndTime)
                            {
                                tomorrowPodNumber++;
                                decimal amtTomorow;
                                if (!string.IsNullOrEmpty(p.Str1) && decimal.TryParse(p.Str1, out amtTomorow))
                                {
                                    tomorrowAmt += amtTomorow;
                                }
                                else
                                {
                                    errorAmtMessage.Append("运单:").Append(p.CustomerOrderNumber).Append("运单价值未填或输入有误").Append("<br/>");
                                }
                            }
                        });

                        Messages.Append(StartTime.DateTimeToString()).Append(" 运单数为:").Append(yesterdayPodNumber).Append(",运单总价值为:").Append(yesterdayAmt.ToString()).Append("<br/>");
                        Messages.Append(currentTime.DateTimeToString()).Append(" 运单数为:").Append(todayPodNumber).Append(",运单总价值为:").Append(todayAmt.ToString()).Append("<br/>");
                        Messages.Append(EndTime.DateTimeToString()).Append(" 运单数为:").Append(tomorrowPodNumber).Append(",运单总价值为:").Append(tomorrowAmt.ToString()).Append("<br/>");
                        Messages.Append(errorAmtMessage.ToString());
                        ViewBag.Tips = Messages.ToString();
                    }
                }
            }
            return View();
        }

        public ActionResult Content(int? firstMenuID, int? secondMenuID)
        {
            ViewData["menu"] = GetAll3RdMenu(secondMenuID);

            return View();
        }

        [HttpPost]
        public ActionResult Welcome(long?id)
        {
            long ID =long.Parse(Request.Form["ID"].ToString());
            string Password = Request.Form["Psw2"].ToString();
            if (UserInfo.ID != ID)
            {
                return RedirectToAction("Welcome");
            }



            bool result = new UserService().UpdateUserPassword(new UserRequest() { ID = ID, Password = Password }).Result;
                if (result)
                {
                    ViewBag.Message = "保存成功!";
                }
                else
                {
                    ViewBag.Message = "保存失败！";
                }
            
            

            return View(UserInfo);
        }

        [HttpPost]
        public string AjaxUpload(string gid, bool isMultiple, bool isCoverOld)
        {
            string uploadFolderPath = Runbow.TWS.Common.Constants.UPLOAD_FOLDER_PATH;
            string targetPath = Path.Combine(uploadFolderPath, base.UserInfo.ProjectID.ToString(), DateTime.Now.DateTimeToString());
            DateTime createDate;
            string attachmentGroupID = gid, url = string.Empty, actualNameInServer = string.Empty, displayName = string.Empty, ext = string.Empty;
            IList<Attachment> attachments = new List<Attachment>();

            if (string.IsNullOrEmpty(targetPath) || !Path.IsPathRooted(targetPath))
            {
                return new { msg = "程序出错！" }.ToJsonString();
            }

            try
            {
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

                        if (isMultiple && !ext.ToLower().Equals(".zip"))
                        {
                            return new { msg = "批量上传，请用zip格式压缩" }.ToJsonString();
                        }

                        actualNameInServer = displayName.Substring(0, displayName.Length - ext.Length + 1) + DateTime.Now.ToString("yyyyMMddHHmmss") + ext;
                        url = Path.Combine(targetPath, actualNameInServer);
                        hpf.SaveAs(url);
                        hpf.InputStream.Close();
                        if (ext.ToLower().Equals(".zip") && isMultiple)
                        {
                            IList<string> unZipedFileName = new List<string>();
                            ZipHelper.UnZip(url, targetPath, unZipedFileName);
                            MyFile.Delete(url);
                            unZipedFileName.Each((k, fileName) =>
                            {
                                actualNameInServer = Path.GetFileName(fileName);
                                ext = Path.GetExtension(fileName);
                                string groupID = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(actualNameInServer));
                                displayName = groupID + ext;
                                createDate = DateTime.Now;
                                attachments.Add(new Attachment() { ActualNameInServer = actualNameInServer, DisplayName = displayName, Extension = ext, Url = fileName, GroupID = groupID, CreateDate = createDate, CreateUserID = base.UserInfo.ID, Creator = base.UserInfo.Name });
                            });
                        }
                        else
                        {
                            attachments.Add(new Attachment() { ActualNameInServer = actualNameInServer, DisplayName = displayName, Extension = ext, Url = url, GroupID = string.IsNullOrEmpty(attachmentGroupID) ? Guid.NewGuid().ToString() : attachmentGroupID, CreateDate = DateTime.Now, CreateUserID = base.UserInfo.ID, Creator = base.UserInfo.Name });
                        }

                        AttachmentService service = new AttachmentService();
                        Response<IEnumerable<Attachment>> response = service.AddAttachment(new AddAttachmentRequest() { attachments = attachments, IsCoverOld = isCoverOld });

                        if (response.IsSuccess)
                        {
                            if (isMultiple)
                            {
                                return new
                                {
                                    msg = "批量上传文件成功",
                                    aids = response.Result.Select(a => a.ID),
                                    anms = response.Result.Select(a => a.DisplayName),
                                    times = response.Result.Select(a => a.CreateDate),
                                    creators = response.Result.Select(a => a.Creator)
                                }.ToJsonString();
                            }
                            else
                            {
                                return new
                                {
                                    msg = "上传文件成功",
                                    gid = response.Result.First().GroupID,
                                    aid = response.Result.First().ID,
                                    anm = response.Result.First().DisplayName,
                                    time = response.Result.First().CreateDate,
                                    creator = response.Result.First().Creator
                                }.ToJsonString();
                            }
                        }
                        else
                        {
                            return new { msg = "上传文件失败！" }.ToJsonString();
                        }
                    }
                }

                return new { msg = "文件无内容" }.ToJsonString();
            }
            catch (Exception ex)
            {
                return new { msg = ex.Message }.ToJsonString();
            }
        }

        [HttpPost]
        public string AjaxUpload2(string gid, bool isMultiple, bool isCoverOld)
        {//, long customerID, string customerName
            string uploadFolderPath = Runbow.TWS.Common.Constants.UPLOAD_FOLDER_PATH;
            string targetPath = Path.Combine(uploadFolderPath, base.UserInfo.ProjectName, DateTime.Now.ToString("yyyy-MM"));
            DateTime createDate;
            string attachmentGroupID = gid, url = string.Empty, actualNameInServer = string.Empty, displayName = string.Empty, ext = string.Empty;
            IList<AMSUpload> amsUpload = new List<AMSUpload>();
        //    amsUpload.Add(new AMSUpload() { FileName = displayName, FileType = ext, ServerName = actualNameInServer, FilePath = fileName, ProjectID = customer, ProjectName = customerName, OrderNo = "", Creator = UserInfo.Name, CreateTime = createDate, Updator = "", UpdateTime = createDate, Status = false });
            if (string.IsNullOrEmpty(targetPath) || !Path.IsPathRooted(targetPath))
            {
                return new { msg = "程序出错！" }.ToJsonString();
            }
            
            try
            {
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

                        if (isMultiple && !ext.ToLower().Equals(".zip"))
                        {
                            return new { msg = "批量上传，请用zip格式压缩" }.ToJsonString();
                        }

                        //actualNameInServer = displayName.Substring(0, displayName.Length - ext.Length + 1) + DateTime.Now.ToString("yyyyMMddHHmmss") + ext;
                        actualNameInServer = UserInfo.ProjectName + DateTime.Now.ToString("yyyyMMddHHmmss") + ext;
                        url = Path.Combine(targetPath, actualNameInServer);
                        hpf.SaveAs(url);
                        hpf.InputStream.Close();
                        if (ext.ToLower().Equals(".zip") && isMultiple)
                        {
                            IList<string> unZipedFileName = new List<string>();
                            ZipHelper.UnZip(url, targetPath, unZipedFileName);
                            MyFile.Delete(url);
                            unZipedFileName.Each((k, fileName) =>
                            {
                                actualNameInServer = Path.GetFileName(fileName);
                                ext = Path.GetExtension(fileName);                                
                                //string groupID = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(actualNameInServer));
                                displayName = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(actualNameInServer));
                                createDate = DateTime.Now;
                                amsUpload.Add(new AMSUpload() { FileName = displayName, FileType = ext, ServerName = actualNameInServer, FilePath = fileName, ProjectID = UserInfo.ProjectID, ProjectName = UserInfo.ProjectName, OrderNo = "", Creator = UserInfo.Name, CreateTime = createDate, Updator = "", UpdateTime = createDate, Status = false });
                            });
                        }
                        else
                        {
                            amsUpload.Add(new AMSUpload() { FileName = displayName, FileType = ext, ServerName = actualNameInServer, FilePath = url, ProjectID = UserInfo.ProjectID, ProjectName = UserInfo.ProjectName, OrderNo = "", Creator = UserInfo.Name, CreateTime = DateTime.Now, Updator = "", UpdateTime = DateTime.Now, Status = false });
                        }

                        AMSUploadService service = new AMSUploadService();
                        Response<IEnumerable<AMSUpload>> response = service.AddAMSUpload(new AddAMSUploadRequest() { amsUpload = amsUpload, IsCoverOld = isCoverOld });
                        
                        if (response.IsSuccess)
                        {
                            if (isMultiple)
                            {
                                return new
                                {
                                    msg = "批量上传文件成功",
                                    aids = response.Result.Select(a => a.ID),
                                    anms = response.Result.Select(a => a.ServerName),
                                    times = response.Result.Select(a => a.CreateTime),
                                    creators = response.Result.Select(a => a.Creator)
                                }.ToJsonString();
                            }
                            else
                            {
                                return new
                                {
                                    msg = "上传文件成功",
                                    gid = response.Result.First().ID,
                                    aid = response.Result.First().ID,
                                    anm = response.Result.First().ServerName,
                                    time = response.Result.First().CreateTime,
                                    creator = response.Result.First().Creator
                                }.ToJsonString();
                            }
                        }
                        else
                        {
                            return new { msg = "上传文件失败！" }.ToJsonString();
                        }
                    }
                }

                return new { msg = "文件无内容" }.ToJsonString();
            }
            catch (Exception ex)
            {
                return new { msg = ex.Message }.ToJsonString();
            }
        }

        public ActionResult GetAttachment(long? id)
        {
            if (id.HasValue)
            {
                AttachmentService service = new AttachmentService();
                Response<Attachment> resp = service.GetAttachmentByID(new GetAttachmentByIDRequest() { ID = id.Value });

                if (resp.IsSuccess)
                {
                    string encode = string.Empty;
                    if (resp.Result.DisplayName.EndsWith("xlsx", StringComparison.OrdinalIgnoreCase) || resp.Result.DisplayName.EndsWith("xls", StringComparison.OrdinalIgnoreCase))
                    {
                        encode = "application/vnd.ms-excel";
                    }
                    else if (resp.Result.DisplayName.EndsWith("bmp", StringComparison.OrdinalIgnoreCase))
                    {
                        encode = "image/x-ms-bmp";
                    }
                    else if (resp.Result.DisplayName.EndsWith("jpeg", StringComparison.OrdinalIgnoreCase) || resp.Result.DisplayName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) || resp.Result.DisplayName.EndsWith("jpe", StringComparison.OrdinalIgnoreCase))
                    {
                        encode = "image/jpeg";
                    }
                    else
                    {
                        encode = "application";
                    }
                    return File(resp.Result.Url, encode, resp.Result.DisplayName.ToUtf8String());
                }
            }

            return new EmptyResult();
        }

        [HttpPost]
        public JsonResult GetAttachments(string gid)
        {
            AttachmentService service = new AttachmentService();
            Response<IEnumerable<Attachment>> resp = service.GetAttachmentsByGroupID(new GetAttachmentsByGroupIDRequest() { GroupID = gid });
            if (resp.IsSuccess)
            {
                return Json(resp.Result);
            }
            else
            {
                return Json(new { msg = "没有对应附件" });
            }
        }

        [HttpPost]
        public JsonResult getTips()
        {
            long DeliverID = 1;
            AttachmentService service = new AttachmentService();
            var response = service.getTips(DeliverID);
            return Json(response);
        }

        [HttpPost]
        public JsonResult AjaxDeleteAttachment(long aid)
        {
            AttachmentService service = new AttachmentService();
            Response<Attachment> resp = service.DeleteAttachment(new DeleteAttachmentRequest() { ID = aid });
            if (resp.IsSuccess)
            {
                try
                {
                    MyFile.Delete(resp.Result.Url);
                }
                catch (Exception ex)
                {
                    return Json(new { success = true, msg = "删除文件成功,但没有删除服务器垃圾文件" });
                }

                return Json(new { success = true, msg = "删除文件成功" });
            }
            else
            {
                return Json(new { msg = resp.Exception.Message });
            }
        }

        private IEnumerable<Menu> GetMenu()
        {
            //var menuDic = new MenuService().GetMenuInfo(new GetMenuByProjectRoleIDRequest() { ProjectRoleID = base.UserInfo.ProjectRoleID }).Result.Where(m => m.Scenarios == 1).Select(m => (Menu)m).GroupBy(m => m.SuperID).ToDictionary(g => g.Key);

            //return this.GetMenus(0, menuDic).OrderBy(m => m.DisplayOrder);
            IEnumerable<Menu> menus = new MenuService().GetMenuInfo(new GetMenuByProjectRoleIDRequest() { ProjectRoleID = base.UserInfo.ProjectRoleID }).Result.Where(m => m.Scenarios == 1).Select(m => (Menu)m);
            ViewData["Menu"] = menus;
            return menus;
        }

        private IEnumerable<Menu> GetMenus(int superID, Dictionary<int, IGrouping<int, Menu>> menus)
        {
            if (menus.ContainsKey(superID))
            {
                var list = new List<Menu>();
                foreach (var m in menus[superID])
                {
                    var chi = GetMenus(m.ID, menus);
                    list.Add(new Menu() { ID = m.ID, SuperID = m.SuperID, Link = m.Link, Name = m.Name, DisplayOrder = m.DisplayOrder, Glyphicon = m.Glyphicon, Children = GetMenus(m.ID, menus) });
                }

                return list.OrderBy(m => m.DisplayOrder);
            }

            return null;
        }

        private IEnumerable<Menu> GetAll3RdMenu(int? secondMenuID)
        {
            if (secondMenuID.HasValue)
            {
                return new MenuService().GetMenuInfo(new GetMenuByProjectRoleIDRequest() { ProjectRoleID = base.UserInfo.ProjectRoleID }).Result.Where(pm => (pm.SuperID == secondMenuID) && pm.Scenarios == 1).OrderBy(m => m.DisplayOrder);
            }

            return null;
        }

        /// <summary>
        /// 微信注册用户列表
        /// </summary>
        [HttpGet]
        public ActionResult wxcustomer()
        {
            QueryWXCustomerViewModel vm = new QueryWXCustomerViewModel();
            vm.SearchCondition = new WXCustomerSearchCondition();
            vm.PageIndex = 0;
            vm.PageCount = 0;
            vm.Customers = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID)
                                        .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            return View(vm);
        }

        /// <summary>
        /// 查询微信注册用户
        /// </summary>
        [HttpPost]
        public ActionResult wxcustomer(QueryWXCustomerViewModel vm, int? PageIndex)
        {
            vm.Customers = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID)
                                         .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });

            var response = (new WXCustomerService()).GetQueryWXCustomer(new QueryWXCustomerRequests()
            {
                PageIndex = PageIndex ?? 0,
                PageSize = UtilConstants.PAGESIZE,
                SearchCondition = vm.SearchCondition,
                //Customers = sb.ToString().Substring(0, sb.Length - 1).ToString()
            });

            if (response.IsSuccess)
            {
                vm.WXCustomerCollection = response.Result.WXCustomerCollection;
                vm.PageCount = response.Result.PageCount;
                vm.PageIndex = response.Result.PageIndex;
            }
            return View(vm);
        }

        /// <summary>
        /// 审核用户
        /// </summary>
        [HttpPost]
        public JsonResult UpdateWXCustomer(string id)
        {
            string[] wx = id.Split('|');
            var response = (new WXCustomerService()).UploadWXCustomer(new QueryWXCustomerRequests() { ID = Convert.ToInt64(wx[0]) });
            if (response.IsSuccess)
            {
                GetPage(wx[1]);
                return Json(new { Message = "审核用户成功", IsSuccess = true });
            }
            else
            {
                if (response.Result == 0)
                {
                    return Json(new { Message = "审核用户失败！", IsSuccess = false });
                }

                return Json(new { Message = "审核用户失败！", IsSuccess = false });
            }
        }

        /// <summary>
        /// POST提交
        /// </summary>
        private string GetPage(string weixinName)
        {
            string posturl = string.Format("https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token={0}", IsExistAccess_Token());
            string postData = "{\"touser\": \"" + weixinName + "\",\"msgtype\": \"text\", \"text\": {\"content\": \"恭喜您注册已通过审核\"}}";
            Stream outstream = null;
            Stream instream = null;
            StreamReader sr = null;
            HttpWebResponse response = null;
            HttpWebRequest request = null;
            Encoding encoding = Encoding.GetEncoding("utf-8");
            byte[] data = encoding.GetBytes(postData);
            // 准备请求...
            try
            {
                // 设置参数
                request = WebRequest.Create(posturl) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                outstream = request.GetRequestStream();
                outstream.Write(data, 0, data.Length);
                outstream.Close();
                //发送请求并获取相应回应数据
                response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                instream = response.GetResponseStream();
                sr = new StreamReader(instream, encoding);
                //返回结果网页（html）代码
                string content = sr.ReadToEnd();
                string err = string.Empty;
                return content;
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return string.Empty;
            }
        }

        /// <summary>  
        /// 根据当前日期 判断Access_Token 是否超期  
        /// 如果超期返回新的Access_Token  否则返回之前的Access_Token  
        /// </summary>  
        public string IsExistAccess_Token()
        {
            string Token = string.Empty;
            WXCustomerService ws = new WXCustomerService();
            //查询已存在的记录
            WXAccessToken at = ws.GetQueryWXAccessToken();
            Token = at.Access_token;
            if (at.Expires_in <= DateTime.Now)
            {
                WebClient wc = new WebClient();
                wc.Encoding = Encoding.UTF8;

                var url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type={0}&appid={1}&secret={2}",
                                    "client_credential", "wx1a4fb84b6f156f44", "2b72be8ef2978b11a34fa9ecfadf313b");
                string josn_Token = wc.DownloadString(url);
                if (!josn_Token.Contains("errcode"))
                {
                    string[] test = josn_Token.Split(',');
                    test[0] = test[0].Replace("\"", "");
                    Token = test[0].Substring(14);
                }
                //修改WXAccessToken表
                WXAccessToken wx = new WXAccessToken();
                wx.Access_token = Token;
                wx.Expires_in = DateTime.Now.AddSeconds(7000);                
                ws.UploadWXAccessToken(wx);
            }

            return Token;
        }

        /// <summary>
        /// 鑫浣微信客服管理
        /// </summary>
        [HttpGet]
        public ActionResult xhcustomer()
        {
            QueryWXCustomerViewModel vm = new QueryWXCustomerViewModel();
            vm.SearchCondition = new WXCustomerSearchCondition();
            vm.PageIndex = 0;
            vm.PageCount = 0;
            vm.Customers = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID)
                                        .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            return View(vm);
        }


        /// <summary>
        /// 鑫浣微信在线活动管理
        /// </summary>
        [HttpGet]
        public ActionResult xhActivity()
        {
            QueryWXCustomerViewModel vm = new QueryWXCustomerViewModel();
            vm.SearchCondition = new WXCustomerSearchCondition();
            vm.PageIndex = 0;
            vm.PageCount = 0;
            vm.Customers = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID)
                                        .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            return View(vm);
        }

        /// <summary>
        /// 鑫浣新增微信在线活动
        /// </summary>
        [HttpGet]
        public ActionResult xhAddActivity()
        {
            QueryWXCustomerViewModel vm = new QueryWXCustomerViewModel();
            vm.SearchCondition = new WXCustomerSearchCondition();
            vm.PageIndex = 0;
            vm.PageCount = 0;
            vm.Customers = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID)
                                        .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            return View(vm);
        }

        /// <summary>
        /// 鑫浣抽奖信息
        /// </summary>
        [HttpGet]
        public ActionResult xhprizeInfo()
        {
            QueryWXCustomerViewModel vm = new QueryWXCustomerViewModel();
            vm.SearchCondition = new WXCustomerSearchCondition();
            vm.PageIndex = 0;
            vm.PageCount = 0;
            vm.Customers = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID)
                                        .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            return View(vm);
        }

        public ActionResult WelcomeChart()
        {
            return View();
        }
    }
}