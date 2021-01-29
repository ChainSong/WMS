using Runbow.TWS.Common;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.Product;
using Runbow.TWS.Web.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Runbow.TWS.Web.TransDataInstances
{
    public class DefaultNikeReturnTransData: BaseTransData
    {
        public DefaultNikeReturnTransData(string TransDataType, long CustomerID, long ProjectID, long WareHouseID,DataSet Transdata)
            : base(TransDataType, CustomerID, ProjectID, WareHouseID, Transdata)
        {
        }
        public override void CustomerDefinedSettledTransData(ref string message)
        {
            if (TransDataType == "PreOrder")
            {
                if (ApplicationConfigHelper.GetWarehouseListByCustomer(CustomerID).Where(p => p.ID == WareHouseID).ToArray().Count() <= 0)
                {
                    message = "用户没有分配仓库!!";
                    return; 
                }
                //正常出货 拆单
                WarehouseInfo warehouse = ApplicationConfigHelper.GetWarehouseListByCustomer(CustomerID).Where(p => p.ID == WareHouseID).ToArray()[0];
                IEnumerable<WMS_Customer> CustomerList = ApplicationConfigHelper.GetAllCustomerByID(CustomerID);
                WMS_UnitAndSpecifications_Config wmsunit = ApplicationConfigHelper.GetWMS_UnitAndSpecifications_Config(ProjectID, CustomerID, warehouse.ID).ToArray()[0];
                DataSet dsResualt = new DataSet();
                DataTable dtpo = new DataTable();
                DataTable dtpodetail = new DataTable();
                dtpo.TableName = "预出库单主信息$";
                dtpodetail.TableName = "预出库单明细信息$";
                DataTable dtpodetailCount = Transdata.Tables[0];//获取第一张表
                DataTable dtpoloadkey = Transdata.Tables[1];  //获取第二张表

                #region 验证空值
                List<ProductSearch> searprolistfs = new List<ProductSearch>();
                string aa = "";
                string bb = "";
                string Size = "";
                string Article = "";
                for (int i = 0; i < dtpodetailCount.Rows.Count; i++)
                {
                    ProductSearch psfs = new ProductSearch();
                    if (dtpodetailCount.Rows[i]["Sku"].ToString().Equals("") || dtpodetailCount.Rows[i]["Sku"].ToString().Length == 0)
                    {
                        message = "第【" + (i + 2) + "】行，" + "【Sku】列" + "有空值或格式不正确（应为文本格式）！";
                        return;
                    }
                    aa = dtpodetailCount.Rows[i]["sku"].ToString();
                    bb = aa.Substring(6, 3);
                    Size = aa.Substring(9);
                    aa = aa.Substring(0, 6);

                    psfs.Str10 = aa + "-" + bb;
                    psfs.Str9 = Size;
                    psfs.Str8 = "01";
                    searprolistfs.Add(psfs);
                }
                #endregion

                #region 获得SKU集合
                IEnumerable<ProductSearch> productListfs;
                productListfs = ApplicationConfigHelper.GetSearchProduct(CustomerID, searprolistfs, "Acticle");
                #endregion

                #region 先验证article存不存在再继续
                for (int i = 0; i < dtpodetailCount.Rows.Count; i++)
                {
                    aa = dtpodetailCount.Rows[i]["sku"].ToString();
                    bb = aa.Substring(6, 3);
                    Size = aa.Substring(9);
                    aa = aa.Substring(0, 6);
                    Article = aa + '-' + bb;
                    if (productListfs.Where(m => m.Str10 == Article).ToArray().Length <= 0)
                    {
                        message = "第【" + (i + 2) + "】行，" + "【sku】列的Article格式不对或Article " + dtpodetailCount.Rows[i]["sku"].ToString() + " 在系统中不存在!";
                        return;
                    }
                    else
                    {
                        if (productListfs.Where(m => m.Str10 == Article && m.Str9 == Size).ToArray().Length <= 0)
                        {
                            message = "第【" + (i + 2) + "】行，" + "【sku】列的尺寸格式不对或 " + dtpodetailCount.Rows[i]["sku"].ToString() + "对应的尺寸" + dtpodetailCount.Rows[i]["sku"].ToString() + " 在系统中不存在!";
                            return;
                        }
                    }
                    //判断是否为单仓订单
                    if (dtpoloadkey.Select("Loadkey='" + dtpodetailCount.Rows[i]["LOAD KEY"].ToString() + "'").Count() <= 0)
                    {
                        dtpodetailCount.Rows[i]["LF出库单号行号"] = "否";
                    }
                    else
                    {
                        dtpodetailCount.Rows[i]["LF出库单号行号"] = "是";
                    }

                    //判断 Division Code
                    if (dtpodetailCount.Rows[i]["Division Code"].ToString() == "10")
                    {
                        dtpodetailCount.Rows[i]["Division Code"] = "APP";
                    }
                    else if (dtpodetailCount.Rows[i]["Division Code"].ToString() == "20" || dtpodetailCount.Rows[i]["Division Code"].ToString() == "40")
                    {
                        dtpodetailCount.Rows[i]["Division Code"] = "FTW";
                    }
                    else
                    {
                        dtpodetailCount.Rows[i]["Division Code"] = "EQP";
                    }
                    

                }
                #endregion

                #region  建列
                dtpo.Columns.Add("外部单号");
                dtpo.Columns.Add("预出库单类型");
                dtpo.Columns.Add("仓库名称");
                dtpo.Columns.Add("订单日期");  
                dtpo.Columns.Add("省");
                dtpo.Columns.Add("市");  //城市
                dtpo.Columns.Add("联系人");
                dtpo.Columns.Add("快递公司");
                dtpo.Columns.Add("备注");
                dtpo.Columns.Add("自定义字段2");//运输时效
                dtpo.Columns.Add("自定义字段3");//Division Code
                dtpo.Columns.Add("自定义字段4");//NFS店铺编码
                dtpo.Columns.Add("地址");      //地址1+地址2+地址3+地址4
                dtpo.Columns.Add("联系方式");
                dtpo.Columns.Add("自定义字段5");//公司名
                dtpo.Columns.Add("自定义字段6");
                dtpo.Columns.Add("自定义字段7");//计划发货时间
                dtpo.Columns.Add("自定义字段8");//VAS Code
                dtpo.Columns.Add("自定义字段9");//LOAD KEY
                dtpo.Columns.Add("自定义字段10");//Nike PO
                dtpo.Columns.Add("自定义字段11");//PACK SLIP NO
                dtpo.Columns.Add("自定义字段12");//LF出库单号行号 (是否单仓)
                dtpo.Columns.Add("自定义字段13");//BU
                dtpo.Columns.Add("自定义字段14");//RP LI 
                dtpo.Columns.Add("自定义字段15");//CRD
                dtpo.Columns.Add("自定义字段16");//预计卸货时间

                dtpodetail.Columns.Add("外部单号");
                dtpodetail.Columns.Add("仓库");
                dtpodetail.Columns.Add("SKU");  //Sku
                dtpodetail.Columns.Add("UPC");  //UPC
                dtpodetail.Columns.Add("产品等级");
                dtpodetail.Columns.Add("产品名称");
                dtpodetail.Columns.Add("批次号");
                dtpodetail.Columns.Add("托号");
                dtpodetail.Columns.Add("备注");
                dtpodetail.Columns.Add("期望数量");  //数量
                dtpodetail.Columns.Add("自定义字段2");
                dtpodetail.Columns.Add("自定义字段3");
                dtpodetail.Columns.Add("自定义字段4");//Sku
                dtpodetail.Columns.Add("自定义字段5");//VAS
                dtpodetail.Columns.Add("自定义字段6");//Article
                dtpodetail.Columns.Add("自定义字段7");//Division Code  BU
                dtpodetail.Columns.Add("自定义字段8");//LF出库单号
                dtpodetail.Columns.Add("自定义字段9");////NIke单据编码

                dtpodetail.Columns.Add("单位");
                dtpodetail.Columns.Add("规格");
                #endregion

                #region 拆单 收货日期+BU+门店代码+Loadkey   计划发货时间+Loadkey+运输方式
                for (int i = 0; i < dtpodetailCount.Rows.Count; i++)
                {
                    aa = dtpodetailCount.Rows[i]["sku"].ToString();
                    bb = aa.Substring(6, 3);
                    Size = aa.Substring(9);
                    aa = aa.Substring(0, 6);
                    Article = aa + '-' + bb;

                    //生成外部单号
                    //string exterNo = dtpodetailCount.Rows[i]["Division Code"].ToString().Substring(0, 1) + '-'
                    //+ dtpodetailCount.Rows[i]["NFS店铺编码"].ToString() + '-' + dtpodetailCount.Rows[i]["LOAD KEY"].ToString();
                    string exterNo = dtpodetailCount.Rows[i]["计划发货时间"].ToString().Replace("-","") + '-' + dtpodetailCount.Rows[i]["LOAD KEY"].ToString() + '-' + dtpodetailCount.Rows[i]["运输方式"].ToString();

                    //判断外部单号是否存在
                    if (dtpo.Select("外部单号='" + exterNo + "'").Count() <= 0)
                    {
                        DataRow dr = dtpo.NewRow();
                        if (productListfs.Where(m => m.Str10 == Article).ToArray().Length <= 0)
                        {
                            message = "第【" + (i + 2) + "】行，" + "【sku】列的Article格式不对或Article " + dtpodetailCount.Rows[i]["sku"].ToString() + "在系统中不存在!";
                            return;
                        }
                        dr["外部单号"] = exterNo;
                        dr["预出库单类型"] = dtpodetailCount.Rows[i]["订单类型"].ToString();//正常出库AB，残次出库C
                        dr["仓库名称"] = warehouse.WarehouseName;
                        dr["市"] = dtpodetailCount.Rows[i]["城市"].ToString();
                        dr["省"] = "";
                        dr["联系人"] = "";
                        dr["订单日期"] = DateTime.Now.ToString();
                        dr["备注"] = "";
                        dr["自定义字段2"] = dtpodetailCount.Rows[i]["运输时效"].ToString();
                        dr["自定义字段3"] = dtpodetailCount.Rows[i]["运输方式"].ToString();
                        //if (CustomerList.Where(m => m.StorerKey == dtpodetailCount.Rows[i]["NFS店铺编码"].ToString()).ToArray().Length <= 0)
                        //{
                        //    message = "第【" + (i + 2) + "】行，" + "【NFS店铺编码】列的店铺编码格式不对或店铺编码 " + dtpodetailCount.Rows[i]["NFS店铺编码"].ToString() + "在系统中不存在!";//dtNike.Rows[i]["Store Code"].ToString() + "不存在!";
                        //    return;
                        //}
                        dr["自定义字段4"] = dtpodetailCount.Rows[i]["NFS店铺编码"].ToString();
                        dr["地址"] = dtpodetailCount.Rows[i]["地址3"].ToString() + '&' + dtpodetailCount.Rows[i]["地址4"].ToString() + '&' + dtpodetailCount.Rows[i]["地址2"].ToString() + '&' + dtpodetailCount.Rows[i]["地址1"].ToString();
                        dr["自定义字段5"] = dtpodetailCount.Rows[i]["公司名"].ToString();
                        dr["联系方式"] = "";
                        dr["自定义字段6"]= productListfs.Where(m => m.Str10 == Article).ToArray().Length > 0 ? productListfs.Where(m => m.Str10 == Article).ToArray()[0].GenderAge.ToString() : "";
                        dr["自定义字段7"] = dtpodetailCount.Rows[i]["计划发货时间"].ToString();
                        dr["自定义字段8"] = dtpodetailCount.Rows[i]["VAS Code"].ToString();
                        dr["自定义字段9"] = dtpodetailCount.Rows[i]["LOAD KEY"].ToString();
                        dr["自定义字段10"] = dtpodetailCount.Rows[i]["Nike PO"].ToString();
                        dr["自定义字段11"] = dtpodetailCount.Rows[i]["PACK SLIP NO"].ToString();
                        dr["自定义字段12"] = dtpodetailCount.Rows[i]["LF出库单号行号"].ToString();
                        dr["自定义字段13"] = dtpodetailCount.Rows[i]["Division Code"].ToString();//BU
                        //判断 Nike PO 长度是否大于9
                        if (dtpodetailCount.Rows[i]["Nike PO"].ToString().Length >=9)
                        {
                            if (dtpodetailCount.Rows[i]["Nike PO"].ToString().Substring(8, 1).Equals("L"))
                            {//1.S列的第9位为L时，显示为LI，为S时，为RP。
                                dr["自定义字段14"] = "LI";//LI
                            }
                            else if (dtpodetailCount.Rows[i]["Nike PO"].ToString().Substring(8, 1).Equals("S"))
                            {
                                dr["自定义字段14"] = "RP";//RP
                            }
                            else
                            {// 2.当非L或S时，用T列判断，RP为RP，LI为LI，NIKECN为空值。
                                if (dtpodetailCount.Rows[i]["VAS Code"].ToString().Equals("NIKECN"))
                                {
                                    dr["自定义字段14"] = "";
                                }
                                else
                                {
                                    dr["自定义字段14"] = dtpodetailCount.Rows[i]["VAS Code"].ToString();//RP为RP，LI为LI
                                }
                            }
                        }
                        else
                        {
                            // 2.当非L或S时，用T列判断，RP为RP，LI为LI，NIKECN为空值。
                                if (dtpodetailCount.Rows[i]["VAS Code"].ToString().Equals("NIKECN"))
                                {
                                    dr["自定义字段14"] = "";
                                }
                                else
                                {
                                    dr["自定义字段14"] = dtpodetailCount.Rows[i]["VAS Code"].ToString();//RP为RP，LI为LI

                                }
                        }
                        //string.Format("{0:d}", dt)
                        //dr["自定义字段15"] = string.Format("{0:d}", DateTime.Parse(dtpodetailCount.Rows[i]["计划发货时间"].ToString())
                        //   .AddDays(int.Parse(dtpodetailCount.Rows[i]["运输时效"].ToString())));
                        dr["自定义字段15"] = (DateTime.Parse(dtpodetailCount.Rows[i]["计划发货时间"].ToString())
                            .AddDays(int.Parse(dtpodetailCount.Rows[i]["运输时效"].ToString()))).ToString("yyyy-MM-dd");
                        try
                        {
                           DateTime time= DateTime.Parse(dtpodetailCount.Rows[i]["Nike PO"].ToString().Substring(0, 8).Insert(4, "-").Insert(7, "-"));

                            if (DateTime.Parse(dtpodetailCount.Rows[i]["计划发货时间"].ToString())
                            .AddDays(int.Parse(dtpodetailCount.Rows[i]["运输时效"].ToString())) >= DateTime.Parse(dtpodetailCount.Rows[i]["Nike PO"].ToString().Substring(0, 8).Insert(4, "-").Insert(7, "-"))
                            )
                            {//1.当H + I》S时，不显示CRD 2.当H + I < S时，显示CRD为S列前8位 运输时效 Nike PO
                                dr["自定义字段15"] = "";//CRD
                            }
                            else
                            {
                                dr["自定义字段15"] = dtpodetailCount.Rows[i]["Nike PO"].ToString().Substring(0, 8).Insert(4, "-").Insert(7, "-");
                            }
                        }
                        catch (Exception e)
                        { }

                        //预计卸货时间
                        dr["自定义字段16"] = (DateTime.Parse(dtpodetailCount.Rows[i]["计划发货时间"].ToString())
                            .AddDays(int.Parse(dtpodetailCount.Rows[i]["运输时效"].ToString()))).ToString("yyyy-MM-dd");
                        try
                        {
                            DateTime time = DateTime.Parse(dtpodetailCount.Rows[i]["Nike PO"].ToString().Substring(0, 8).Insert(4, "-").Insert(7, "-"));
                            dr["自定义字段16"] = dtpodetailCount.Rows[i]["Nike PO"].ToString().Substring(0, 8).Insert(4, "-").Insert(7, "-");
                        }
                        catch (Exception)
                        { }

                        dtpo.Rows.Add(dr);
                    }
                    DataRow drdetail = dtpodetail.NewRow();
                    drdetail["外部单号"] = exterNo;
                    drdetail["仓库"] = warehouse.WarehouseName;
                    drdetail["SKU"]= productListfs.Where(m => m.Str10 == Article.Trim().ToUpper() && m.Str9 == Size.Trim().ToUpper() && m.Str8 == "01").ToArray().Length > 0 ? productListfs.Where(m => m.Str10 == Article.Trim().ToUpper() && m.Str9 ==Size.Trim().ToUpper() && m.Str8 == "01").ToArray()[0].SKU : "";
                    drdetail["UPC"] = "";

                    if (dtpodetailCount.Rows[i]["订单类型"].ToString() == "正常出库")
                    {
                        drdetail["产品等级"] = "A品";
                    }
                    else
                    {
                        drdetail["产品等级"] = "C品";
                    }

                    drdetail["产品名称"] = productListfs.Where(m => m.Str10 == Article).ToArray().Length > 0 ? productListfs.Where(m => m.Str10 == Article).ToArray()[0].LongMaterial.ToString() : "";//产品名称
                    
                    drdetail["批次号"] = "";
                    drdetail["托号"] = "";
                    drdetail["规格"] = "";
                    drdetail["备注"] = "";
                    drdetail["期望数量"] = dtpodetailCount.Rows[i]["数量"].ToString();
                    drdetail["自定义字段2"] = productListfs.Where(m => m.Str10 == Article.Trim().ToUpper() && m.Str9 == Size.Trim().ToUpper() && m.Str8 == "01").ToArray()[0].Price;//价格
                    drdetail["自定义字段3"] = productListfs.Where(m => m.Str10 == Article.Trim().ToUpper() && m.Str9 == Size.Trim().ToUpper() && m.Str8 == "01").ToArray()[0].SafeLock;//安全扣
                    drdetail["自定义字段4"] = productListfs.Where(m => m.Str10 == Article.Trim().ToUpper() && m.Str9 == Size.Trim().ToUpper() && m.Str8 == "01").ToArray()[0].Hanger;//衣架
                    drdetail["自定义字段5"] = dtpodetailCount.Rows[i]["VAS"].ToString();
                    drdetail["自定义字段6"] = Article + '-' + Size;
                    drdetail["自定义字段7"] = dtpodetailCount.Rows[i]["Division Code"];//BU
                    drdetail["自定义字段8"] = dtpodetailCount.Rows[i]["LF出库单号"].ToString();
                    drdetail["自定义字段9"] = dtpodetailCount.Rows[i]["NIke单据编码"].ToString();
                    drdetail["单位"] = wmsunit.Unit;

                    dtpodetail.Rows.Add(drdetail);
                }
                #endregion

                dsResualt.Tables.Add(dtpo);
                dsResualt.Tables.Add(dtpodetail);

                //ModifyForDataTable MFD = new ModifyForDataTable();
                //dsResualt = MFD.AddSerialNumberForNFS(dsResualt);

                this.AfterData = dsResualt;
            }
            else if (TransDataType == "Receiving")
            {
                #region  上架
                DataTable dt = Transdata.Tables[0].Copy();
                foreach (DataRow item in dt.Rows)
                {
                    if (!item["托号"].ToString().Equals(""))
                    {
                        message = "托号列不能有值，请清除后再导入！";
                        return;
                    }
                }

                if (!dt.Columns.Contains("生产日期"))
                {
                    dt.Columns.Add("生产日期");
                }
                if (!dt.Columns.Contains("UPC"))
                {
                    dt.Columns.Add("UPC");
                }
                if (!dt.Columns.Contains("箱内总数"))
                {
                    dt.Columns.Add("箱内总数");
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    try
                    {
                        Convert.ToDateTime(dt.Rows[i]["生产日期"].ToString());
                    }
                    catch
                    {
                        dt.Rows[i]["生产日期"] = System.DateTime.Now.ToString();
                    }
                }
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);

                this.AfterData = ds;
                #endregion
            }
            else if (TransDataType == "Asn")
            {
                try
                {
                    if (ApplicationConfigHelper.GetWarehouseListByCustomer(CustomerID).Where(p => p.ID == WareHouseID).ToArray().Count() <= 0)
                    {
                        message = "用户没有分配仓库!!";
                        return;
                    }
                    //Asn
                    WarehouseInfo warehouse = ApplicationConfigHelper.GetWarehouseListByCustomer(CustomerID).Where(p => p.ID == WareHouseID).ToArray()[0];
                    IEnumerable<WMS_Customer> CustomerList = ApplicationConfigHelper.GetAllCustomerByID(CustomerID);
                    WMS_UnitAndSpecifications_Config wmsunit = ApplicationConfigHelper.GetWMS_UnitAndSpecifications_Config(ProjectID, CustomerID, warehouse.ID).ToArray()[0];
                    DataTable dtpodetailCount = Transdata.Tables[0];//获取第1张表
                    DataTable dtpoloadkey = Transdata.Tables[1];  //获取第2张表


                    #region 验证空值
                    List<ProductSearch> searprolistfs = new List<ProductSearch>();
                    for (int i = 0; i < dtpoloadkey.Rows.Count; i++)
                    {
                        ProductSearch psfs = new ProductSearch();
                        if (dtpoloadkey.Rows[i]["SKU"].ToString().Equals("") || dtpoloadkey.Rows[i]["SKU"].ToString().Length == 0)
                        {
                            message = "第【" + (i + 2) + "】行，" + "【SKU】列" + "有空值或格式不正确（应为文本格式）！";
                            return;
                        }
                        if (dtpoloadkey.Rows[i]["UPC"].ToString().Equals("") || dtpoloadkey.Rows[i]["UPC"].ToString().Length == 0)
                        {
                            message = "第【" + (i + 2) + "】行，" + "【UPC】列" + "有空值或格式不正确（应为文本格式）！";
                            return;
                        }
                        psfs.Str10 = dtpoloadkey.Rows[i]["SKU"].ToString();
                        psfs.Str9 = dtpoloadkey.Rows[i]["UPC"].ToString();
                        psfs.Str8 = "01";
                        searprolistfs.Add(psfs);
                    }
                    #endregion

                    #region 获得SKU集合
                    IEnumerable<ProductSearch> productListfs;
                    productListfs = ApplicationConfigHelper.GetSearchProduct(CustomerID, searprolistfs, "Acticle");
                    #endregion

                    //退货仓填写默认门店代码
                    for (int i = 0; i < dtpodetailCount.Rows.Count; i++)
                    {
                        dtpodetailCount.Rows[i]["门店代码"] = "QQQQQQ";
                    }


                    for (int i = 0; i < dtpoloadkey.Rows.Count; i++)
                    {
                        string Article = dtpoloadkey.Rows[i]["SKU"].ToString();
                        string Sz = dtpoloadkey.Rows[i]["UPC"].ToString();
                        try
                        {
                            dtpoloadkey.Rows[i]["SKU"] = productListfs.Where(m => m.Str10 == Article && m.Str9 == Sz && m.Str8 == "01").ToArray()[0].SKU.ToString();
                            dtpoloadkey.Rows[i]["UPC"] = DBNull.Value;

                        }
                        catch (Exception e)
                        {
                            message = "第" + (i + 1) + "行Article【" + Article + "】，Sz【" + Sz + "】在系统中不存在！";
                            return;
                        }
                    }
                    this.AfterData = Transdata;
                }
                catch (Exception e)
                {

                    throw;
                }
            }
            else
            {
                this.AfterData = Transdata;
            }
        }
    }
}