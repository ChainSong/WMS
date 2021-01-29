using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Runbow.TWS.Web.Handler
{
    /// <summary>
    /// Summary description for UploadImgCode
    /// </summary>
    public class UploadImgCode : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string img;//接收经过base64编 之后的字符串    
            context.Response.ContentType = "text/plain";
            try
            {
                img = context.Request["img"].ToString();
                //获取base64字符串          
                byte[] imgBytes = Convert.FromBase64String(img);
                //将base64字符串转换为字节数组            
                System.IO.Stream stream = new System.IO.MemoryStream(imgBytes);
                //将字节数组转换为字节流      
                //将流转回Image，用于将PNG 式照片转为jpg，压缩体积以便保存。         
                System.Drawing.Image imgae = System.Drawing.Image.FromStream(stream);
                imgae.Save(context.Server.MapPath("~/Test/") + Guid.NewGuid().ToString() + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);//保存图片             
                context.Response.Write("OK");//输出调用结果                
            }
            catch (Exception msg)
            {
                img = null;
                context.Response.Write(msg);
                return;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}