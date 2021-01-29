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
    public class DefaultYXDRSHHTransData : BaseTransData
    {

        //由BaseTransData 传来的参数
        public DefaultYXDRSHHTransData(string TransDataType, long CustomerID, long ProjectID, long WareHouseID,
           DataSet Transdata)
            : base(TransDataType, CustomerID, ProjectID, WareHouseID, Transdata)
        {

        }
        public override void CustomerDefinedSettledTransData(ref string message)
        {

            if (TransDataType == "PreOrder")
            {
                if (WareHouseID == 0)
                {
                    message = "用户没有分配仓库!!";
                    return;
                }
                List<ProductSearch> productLists1 = new List<ProductSearch>();
                IEnumerable<ProductSearch> productList1;//获取系统根据条件查询之后的SKU信息
                WarehouseInfo warehouse = ApplicationConfigHelper.GetWarehouseList().Where(m => m.ID == WareHouseID).FirstOrDefault();//customerid获取仓库
                IEnumerable<WMS_Customer> customerlist = ApplicationConfigHelper.GetAllCustomerByID(CustomerID);//店铺
                IEnumerable<WMS_UnitAndSpecifications_Config> unit = ApplicationConfigHelper.GetWMS_UnitAndSpecifications_Config(ProjectID, CustomerID, warehouse.ID);//货品单位信息
                DataSet dsresult = new DataSet();
                DataTable dtYXDR = this.Transdata.Tables[0];

                DataTable dtpre = new DataTable();
                DataTable dtpredetail = new DataTable();

                dtpre.TableName = "预出库单主信息$";
                dtpredetail.TableName = "预出库单明细信息$";
                //子表字段
                dtpre.Columns.Add("外部单号");
                dtpre.Columns.Add("预出库单类型");
                dtpre.Columns.Add("仓库名称");
                dtpre.Columns.Add("订单日期");
                dtpre.Columns.Add("收货公司");
                dtpre.Columns.Add("省");
                dtpre.Columns.Add("市");
                dtpre.Columns.Add("联系人");
                dtpre.Columns.Add("备注");
                dtpre.Columns.Add("地址");
                dtpre.Columns.Add("联系方式");
                dtpre.Columns.Add("自定义字段2");
                dtpre.Columns.Add("自定义字段3");
                dtpre.Columns.Add("自定义字段4");

                //子表

                //子表
                dtpredetail.Columns.Add("外部单号");
                dtpredetail.Columns.Add("仓库");
                dtpredetail.Columns.Add("SKU");
                dtpredetail.Columns.Add("UPC");
                dtpredetail.Columns.Add("产品名称");
                dtpredetail.Columns.Add("产品等级");
                dtpredetail.Columns.Add("批次号");
                dtpredetail.Columns.Add("箱号");//代表整箱箱号
                dtpredetail.Columns.Add("备注");
                dtpredetail.Columns.Add("期望数量");
                dtpredetail.Columns.Add("单位");
                dtpredetail.Columns.Add("规格");
                dtpredetail.Columns.Add("自定义字段4");
                dtpredetail.Columns.Add("自定义字段5");
                dtpredetail.Columns.Add("自定义字段6");

                //一个datatable转换成主表和子表
                for (int i = 0; i < dtYXDR.Rows.Count; i++)
                {
                    if (dtpre.Select("外部单号='" + dtYXDR.Rows[i]["订单号"].ToString().Trim() + "'").Count() <= 0)
                    {
                        DataRow drpre = dtpre.NewRow();
                        drpre["外部单号"] = dtYXDR.Rows[i]["订单号"].ToString().Trim();
                        drpre["预出库单类型"] = dtYXDR.Rows[i]["订单类型"].ToString().Trim();
                        drpre["仓库名称"] = warehouse.WarehouseName;
                        drpre["订单日期"] = dtYXDR.Rows[i]["订单日期"].ToString().Trim();
                        if (customerlist.Where(a => a.StorerKey == dtYXDR.Rows[i]["货主"].ToString()).ToArray().Length <= 0)
                        {
                            message = "Excel中第：" + (i + 2) + "行的" + dtYXDR.Rows[i]["货主"].ToString() + "货主不存在！！";
                            return;
                        }
                        drpre["自定义字段4"] = dtYXDR.Rows[i]["货主"].ToString().Trim();
                        drpre["收货公司"] = customerlist.Where(c => c.StorerKey == dtYXDR.Rows[i]["货主"].ToString().Trim()).ToArray().Length > 0 ? customerlist.Where(c => c.StorerKey == dtYXDR.Rows[i]["货主"].ToString().Trim()).ToArray()[0].Company : "";
                       
                        drpre["备注"] = dtYXDR.Rows[i]["备注1"].ToString();
                        drpre["自定义字段2"] = dtYXDR.Rows[i]["备注2"].ToString();
                        drpre["自定义字段3"] = dtYXDR.Rows[i]["备注3"].ToString();

                      
                        if (string.IsNullOrEmpty(dtYXDR.Rows[i]["联系人"].ToString().Trim()))
                        {
                            message = "Excel中第：" + (i + 2) + "的 联系人 不能为空，请检查";
                            return;
                        }
                        if (string.IsNullOrEmpty(dtYXDR.Rows[i]["联系方式"].ToString().Trim()))
                        {
                            message = "Excel中第：" + (i + 2) + "的 联系方式 不能为空，请检查";
                            return;
                        }
                        if (string.IsNullOrEmpty(dtYXDR.Rows[i]["地址"].ToString().Trim()))
                        {
                            message = "Excel中第：" + (i + 2) + "的 地址 不能为空，请检查";
                            return;
                        }
                        drpre["市"] = dtYXDR.Rows[i]["市"].ToString().Trim();
                        drpre["联系人"] = dtYXDR.Rows[i]["联系人"].ToString().Trim();
                        drpre["联系方式"] = dtYXDR.Rows[i]["联系方式"].ToString().Trim();
                        drpre["地址"] = dtYXDR.Rows[i]["地址"].ToString().Trim();

                         //drpre["省"] = "";
                        //drpre["市"] = customerlist.Where(c => c.StorerKey == dtYXDR.Rows[i]["货主"].ToString().Trim()).ToArray().Length > 0 ? customerlist.Where(c => c.StorerKey == dtYXDR.Rows[i]["货主"].ToString().Trim()).ToArray()[0].City : "";
                        //drpre["联系人"] = customerlist.Where(c => c.StorerKey == dtYXDR.Rows[i]["货主"].ToString().Trim()).ToArray().Length > 0 ? customerlist.Where(c => c.StorerKey == dtYXDR.Rows[i]["货主"].ToString().Trim()).ToArray()[0].Contact1 : "";
                        //drpre["地址"] = customerlist.Where(c => c.StorerKey == dtYXDR.Rows[i]["货主"].ToString().Trim()).ToArray().Length > 0 ? customerlist.Where(c => c.StorerKey == dtYXDR.Rows[i]["货主"].ToString().Trim()).ToArray()[0].AddressLine1 : "";
                        //drpre["联系方式"] = customerlist.Where(c => c.StorerKey == dtYXDR.Rows[i]["货主"].ToString().Trim()).ToArray().Length > 0 ? customerlist.Where(c => c.StorerKey == dtYXDR.Rows[i]["货主"].ToString().Trim()).ToArray()[0].PhoneNum1 : "";
                        dtpre.Rows.Add(drpre);
                    }
                    DataRow drpredetail = dtpredetail.NewRow();
                    drpredetail["外部单号"] = dtYXDR.Rows[i]["订单号"].ToString().Trim();
                    drpredetail["仓库"] = warehouse.WarehouseName;

                    ProductSearch ps = new ProductSearch();
                    ps.SKU = dtYXDR.Rows[i]["SKU"].ToString().Trim();
                    productLists1.Add(ps);

                    drpredetail["UPC"] = "";
                    try
                    {
                        drpredetail["产品等级"] = dtYXDR.Rows[i]["产品等级"].ToString().Trim();
                    }
                    catch (Exception)
                    {
                        drpredetail["产品等级"] = "A品";
                    }

                    drpredetail["批次号"] = "";
                    drpredetail["箱号"] = dtYXDR.Rows[i]["箱号"].ToString().Trim();
                    drpredetail["规格"] = "";
                    drpredetail["备注"] = "";
                    drpredetail["期望数量"] = dtYXDR.Rows[i]["数量"].ToString();
                    drpredetail["自定义字段4"] = dtYXDR.Rows[i]["备注4"].ToString();
                    drpredetail["自定义字段5"] = dtYXDR.Rows[i]["备注5"].ToString();
                    drpredetail["自定义字段6"] = dtYXDR.Rows[i]["备注6"].ToString();
                    drpredetail["单位"] = dtYXDR.Rows[i]["单位"].ToString();
                    dtpredetail.Rows.Add(drpredetail);
                }


                productList1 = ApplicationConfigHelper.GetSearchProductYXDR(CustomerID, productLists1, "SKU");
                #region 验证SKU是否存在
                for (int i = 0; i < dtYXDR.Rows.Count; i++)
                {
                    if (productList1.ToArray().Length <= 0)
                    {
                        message = "excel中的SKU在系统中不存在,请检查excel数据！";
                        return;
                    }
                    string sku = productList1.Where(p => p.SKU == dtYXDR.Rows[i]["SKU"].ToString()).ToArray().Length > 0 ? productList1.Where(p => p.SKU == dtYXDR.Rows[i]["SKU"].ToString()).ToArray()[0].SKU : "";
                    if (string.IsNullOrEmpty(sku))
                    {
                        message = "excel中的SKU=" + dtYXDR.Rows[i]["SKU"].ToString() + "在系统中不存在，请检查数据！";
                        return;
                    }
                    dtpredetail.Rows[i]["SKU"] = sku;
                    dtpredetail.Rows[i]["产品名称"] = productList1.Where(p => p.SKU == sku).Select(p => p.GoodsName).FirstOrDefault();


                }
                #endregion

                #region 验证同一外部单号是否有相同SKU
                //string msg = "";
                //for (int x = 0; x < dtpredetail.Rows.Count; x++)
                //{
                //    msg += dtpredetail.Rows[x]["托号"].ToString().Trim();

                //}
                //整箱货
                //if (msg.Length > 0 && !string.IsNullOrEmpty(msg))
                //{
                //    var dd1 = from t in dtpredetail.AsEnumerable()
                //              group t by new { t1 = t.Field<string>("外部单号"), t2 = t.Field<string>("SKU"), t3 = t.Field<string>("托号") } into m
                //              select new
                //              {
                //                  外部单号 = m.Select(p => p.Field<string>("外部单号")).First(),
                //                  SKU = m.Select(p => p.Field<string>("SKU")).First(),
                //                  箱号 = m.Select(p => p.Field<string>("托号")).First(),//整箱箱号
                //                  count = m.Count(),

                //              };
                //    var dr1 = dd1.Where(d => d.count > 1);
                //    if (dr1.Count() > 0)
                //    {
                //        foreach (var item in dr1)
                //        {
                //            message += "<p><font color='#FF0000'>外部单号" + item.外部单号 + "中的SKU：" + item.SKU + "和箱号：" + item.箱号 + "存在重复值,请检查！" + "</font></p>";
                //            return;
                //        }
                //    }
                //}
                ////非整箱
                //else
                //{
                //    var dd1 = from t in dtpredetail.AsEnumerable()
                //              group t by new { t1 = t.Field<string>("外部单号"), t2 = t.Field<string>("SKU") } into m
                //              select new
                //              {
                //                  外部单号 = m.Select(p => p.Field<string>("外部单号")).First(),
                //                  SKU = m.Select(p => p.Field<string>("SKU")).First(),
                //                  count = m.Count(),

                //              };
                //    var dr1 = dd1.Where(d => d.count > 1);
                //    if (dr1.Count() > 0)
                //    {
                //        foreach (var item in dr1)
                //        {
                //            message += "<p><font color='#FF0000'>外部单号" + item.外部单号 + "中的SKU：" + item.SKU + "存在重复值,请检查！" + "</font></p>";
                //            return;
                //        }
                //    }
                //}
                #endregion



                dsresult.Tables.Add(dtpre);
                dsresult.Tables.Add(dtpredetail);
                this.AfterData = dsresult;

            }
            else if (TransDataType == "Asn")//预入库单导入
            {

                #region 使用矩阵转换数据
                DataTable dtNew = new DataTable();
                DataRow rowNew = dtNew.NewRow();
                DataColumn dc = null;

                dc = dtNew.Columns.Add("预入库单类型", Type.GetType("System.String"));
                dc = dtNew.Columns.Add("行号", Type.GetType("System.String"));
                dc = dtNew.Columns.Add("订单号", Type.GetType("System.String"));
                dc = dtNew.Columns.Add("预入库日期", Type.GetType("System.DateTime"));
                dc = dtNew.Columns.Add("Article", Type.GetType("System.String"));
                dc = dtNew.Columns.Add("SKU", Type.GetType("System.String"));
                dc = dtNew.Columns.Add("产品名称", Type.GetType("System.String"));
                dc = dtNew.Columns.Add("BoxQty", Type.GetType("System.Int32"));
                dc = dtNew.Columns.Add("BoxNumber", Type.GetType("System.String"));//最终是asndetail表str2字段 
                dc = dtNew.Columns.Add("收货人", Type.GetType("System.String"));
                dc = dtNew.Columns.Add("地址", Type.GetType("System.String"));
                dc = dtNew.Columns.Add("联系方式", Type.GetType("System.String"));
                dc = dtNew.Columns.Add("邮编", Type.GetType("System.String"));
                #endregion

                //鞋子装箱单转换导入，系统模板导入
                if (Transdata.Tables[0].Columns[0].ColumnName == "预入库单类型" && Transdata.Tables[0].Columns[2].ColumnName == "订单号")
                {
                    if (ApplicationConfigHelper.GetWarehouseListByCustomer(CustomerID).Where(w => w.ID == WareHouseID).ToArray().Count() <= 0)//仓库不存在
                    {
                        message = "用户没有分配仓库!!";
                        return;
                    }
                    List<ProductSearch> productLists = new List<ProductSearch>();
                    IEnumerable<ProductSearch> productList;
                    WarehouseInfo warehouse = ApplicationConfigHelper.GetWarehouseListByCustomer(CustomerID).Where(m => m.ID == WareHouseID).ToArray()[0];//仓库
                    IEnumerable<WMS_Customer> customerlist = ApplicationConfigHelper.GetAllCustomerByID(CustomerID);//店铺
                    IEnumerable<WMS_UnitAndSpecifications_Config> wmsunit = ApplicationConfigHelper.GetWMS_UnitAndSpecifications_Config(ProjectID, CustomerID, warehouse.ID);//单位

                    DataSet dsresult = new DataSet();
                    DataTable dtYXDR = Transdata.Tables[0];
                    DataTable dtasn = new DataTable();
                    DataTable dtasndetail = new DataTable();
                    dtasn.TableName = "预入库单主信息$";
                    dtasndetail.TableName = "预入库单明细信息$";

                    //主表
                    dtasn.Columns.Add("外部入库单号");
                    dtasn.Columns.Add("预入库单类型");
                    dtasn.Columns.Add("预入库日期");
                    dtasn.Columns.Add("仓库名称");
                    dtasn.Columns.Add("客户");
                    dtasn.Columns.Add("备注");
                    dtasn.Columns.Add("自定义字段1");
                    dtasn.Columns.Add("自定义字段2");
                    dtasn.Columns.Add("自定义字段3");

                    //子表
                    dtasndetail.Columns.Add("外部入库单号");
                    dtasndetail.Columns.Add("仓库");
                    dtasndetail.Columns.Add("SKU");
                    dtasndetail.Columns.Add("产品名称");
                    dtasndetail.Columns.Add("UPC");
                    dtasndetail.Columns.Add("规格");
                    dtasndetail.Columns.Add("预收数量");
                    dtasndetail.Columns.Add("单位");
                    dtasndetail.Columns.Add("产品等级");
                    dtasndetail.Columns.Add("箱号");
                    dtasndetail.Columns.Add("备注");
                    dtasndetail.Columns.Add("自定义字段4");
                    dtasndetail.Columns.Add("自定义字段5");
                    dtasndetail.Columns.Add("自定义字段6");
                    for (int i = 0; i < dtYXDR.Rows.Count; i++)
                    {
                        if (dtasn.Select("外部入库单号='" + dtYXDR.Rows[i]["订单号"].ToString() + "'").Count() <= 0)//相同单号
                        {
                            DataRow dr = dtasn.NewRow();
                            dr["外部入库单号"] = dtYXDR.Rows[i]["订单号"].ToString().Trim();
                            dr["预入库单类型"] = dtYXDR.Rows[i]["预入库单类型"].ToString().Trim();
                            dr["预入库日期"] = dtYXDR.Rows[i]["预入库日期"].ToString();
                            dr["仓库名称"] = warehouse.WarehouseName;
                            dr["客户"] = dtYXDR.Rows[i]["客户"].ToString().Trim().Trim();
                            dr["备注"] = dtYXDR.Rows[i]["备注1"].ToString();
                            dr["自定义字段1"] = dtYXDR.Rows[i]["备注1"].ToString();
                            dr["自定义字段2"] = dtYXDR.Rows[i]["备注2"].ToString();
                            dr["自定义字段3"] = dtYXDR.Rows[i]["备注3"].ToString();
                            dtasn.Rows.Add(dr);
                        }

                        DataRow drdetail = dtasndetail.NewRow();
                        drdetail["外部入库单号"] = dtYXDR.Rows[i]["订单号"].ToString().Trim();
                        drdetail["仓库"] = warehouse.WarehouseName;
                        ProductSearch ps = new ProductSearch();
                        ps.SKU = dtYXDR.Rows[i]["SKU"].ToString().Trim();
                        productLists.Add(ps);
                        drdetail["UPC"] = "";
                        drdetail["规格"] = "";
                        drdetail["预收数量"] = dtYXDR.Rows[i]["预收数量"].ToString();
                        drdetail["箱号"] = dtYXDR.Rows[i]["箱号"].ToString().Trim();

                        if (wmsunit.Where(u => u.Unit == dtYXDR.Rows[i]["单位"].ToString()).Count() > 0)
                        {
                            drdetail["单位"] = dtYXDR.Rows[i]["单位"].ToString();
                        }
                        else
                        {
                            message = "excel中第：" + (i + 1) + "行的单位在系统中不存在，请检查！";
                            return;
                        }

                        try
                        {
                            drdetail["产品等级"] = dtYXDR.Rows[i]["产品等级"].ToString();
                        }
                        catch (Exception)
                        {
                            drdetail["产品等级"] = "A品";//默认A
                        }
                        drdetail["自定义字段4"] = dtYXDR.Rows[i]["备注4"].ToString();
                        drdetail["自定义字段5"] = dtYXDR.Rows[i]["备注5"].ToString();
                        drdetail["自定义字段6"] = dtYXDR.Rows[i]["备注6"].ToString();
                        dtasndetail.Rows.Add(drdetail);
                    }
                    productList = ApplicationConfigHelper.GetSearchProductYXDR(CustomerID, productLists, "SKU");
                    for (int i = 0; i < dtYXDR.Rows.Count; i++)//校验SKU是否存在
                    {
                        if (productList.ToArray().Length <= 0)
                        {
                            message = "excel中的SKU在系统中不存在,请检查excel数据！";
                            return;
                        }

                        string sku = productList.Where(p => p.SKU == dtYXDR.Rows[i]["SKU"].ToString()).ToArray().Length > 0 ? productList.Where(p => p.SKU == dtYXDR.Rows[i]["SKU"].ToString()).ToArray()[0].SKU : "";
                        if (string.IsNullOrEmpty(sku))
                        {
                            message = "excel中的SKU=" + dtYXDR.Rows[i]["SKU"].ToString() + "在系统中不存在，请检查数据！";
                            return;
                        }
                        dtasndetail.Rows[i]["SKU"] = sku;
                        dtasndetail.Rows[i]["产品名称"] = productList.Where(p => p.SKU == sku).Select(p => p.GoodsName).FirstOrDefault();

                    }

                    //验证同一外部单号是否存在相同SKU和箱号
                    //var dd = from t in dtasndetail.AsEnumerable()
                    //         group t by new { t1 = t.Field<string>("外部入库单号"), t2 = t.Field<string>("SKU"), t3 = t.Field<string>("箱号") } into m
                    //         select new
                    //         {
                    //             外部单号 = m.Select(p => p.Field<string>("外部入库单号")).First(),
                    //             SKU = m.Select(p => p.Field<string>("SKU")).First(),
                    //             箱号 = m.Select(p => p.Field<string>("箱号")).First(),
                    //             count = m.Count()
                    //         };
                    //var dr1 = dd.Where(d => d.count > 1);
                    //if (dr1.Count() > 0)
                    //{
                    //    foreach (var item in dr1)
                    //    {
                    //        message += "<p><font color='#FF0000'>外部入库单号" + item.外部单号 + "中的SKU:" + item.SKU + "和箱号:" + item.箱号 + "存在重复值，请检查！" + "</font></p>";
                    //        return;
                    //    }
                    //}
                    dsresult.Tables.Add(dtasn);
                    dsresult.Tables.Add(dtasndetail);
                    this.AfterData = dsresult;
                }
                else//普通装箱单
                {
                    for (int r = 11; r < Transdata.Tables[0].Rows.Count; r++)
                    {
                        if (!string.IsNullOrEmpty(Transdata.Tables[0].Rows[r][0].ToString()))
                        {
                            for (int c = 1; c < 6; c++)
                            {
                                if (string.IsNullOrEmpty(Transdata.Tables[0].Rows[r][c].ToString()))
                                {
                                    message = (r + 2) + "行" + (c + 1) + "列有空值！";
                                    return;
                                }
                            }
                        }
                    }
                    List<ProductSearch> searprolistfs = new List<ProductSearch>();//存放装箱单里的款号，用于获取数据库中SKU基本信息
                    for (int i = 11; i < Transdata.Tables[0].Rows.Count; i++)
                    {
                        ProductSearch psfs = new ProductSearch();
                        if (!string.IsNullOrEmpty(Transdata.Tables[0].Rows[i][0].ToString().Trim()))
                        {
                            psfs.Str10 = Transdata.Tables[0].Rows[i][0].ToString().Trim();//装箱单里的款号

                            if (!searprolistfs.Exists(a => a.Str10 == psfs.Str10))
                            {
                                searprolistfs.Add(psfs);
                            }
                        }
                    }
                    //获取外部单号
                    string externReceiptNumber = "";
                    if (!string.IsNullOrEmpty(Transdata.Tables[0].Rows[1][1].ToString().Trim()))
                    {
                        externReceiptNumber = Transdata.Tables[0].Rows[1][1].ToString().Trim();
                    }
                    else
                    {
                        message = "Excel中第三行第二列需要有外部单号，请检查后在导入！";
                        return;
                    }

                    IEnumerable<ProductSearch> productListfs = ApplicationConfigHelper.GetSearchProductYXDR(CustomerID, searprolistfs, "Acticle");//根据款号查询所有SKU基本信息
                    for (int j = 11; j < Transdata.Tables[0].Rows.Count; j++)
                    {
                        DataRow rowValue = Transdata.Tables[0].Rows[j];
                        if (string.IsNullOrEmpty(rowValue[0].ToString().Trim()))
                        {
                            continue;
                        }
                        #region 处理数据
                        if (rowValue[0].ToString().Trim().Length >= 3)
                        {
                            string Article = rowValue[0].ToString().Trim();//装箱单里的款号
                            string Color = rowValue[5].ToString().Trim();//颜色
                            int boxNumberStart = Int32.Parse(rowValue[1].ToString().Trim());//开始箱号
                            int boxNumberEnd = Int32.Parse(rowValue[3].ToString().Trim());//结束箱号
                            int boxCount = Int32.Parse(rowValue[4].ToString().Trim());//装箱单总箱数

                            rowNew["预入库单类型"] = "";
                            rowNew["行号"] = "";
                            rowNew["订单号"] = externReceiptNumber;
                            rowNew["预入库日期"] = System.DateTime.Now;
                            rowNew["Article"] = "Article";
                            rowNew["收货人"] = "";
                            rowNew["地址"] = "";
                            rowNew["联系方式"] = "";
                            rowNew["邮编"] = "";

                            for (int jc = 6; jc <= 13; jc++)//获取数量6-13列，代表模板中每箱数量
                            {
                                if (!string.IsNullOrEmpty(rowValue[jc].ToString().Trim()))
                                {
                                    string size = Transdata.Tables[0].Rows[9][jc].ToString().Trim();
                                    if (productListfs.Where(a => a.Str10 == Article & a.Str9 == size & a.Str8 == Color).OrderByDescending(a => a.Str10).Select(a => a.SKU).Count() <= 0)//是否匹配到SKU
                                    {
                                        message = "款号" + Article + " 尺码" + size + " 颜色" + Color + " 不存在！！";
                                    }
                                    string sku = productListfs.Where(a => a.Str10 == Article && a.Str9 == size && a.Str8 == Color).OrderByDescending(a => a.Str10).Select(a => a.SKU).FirstOrDefault().ToString();//SKU
                                    string goodsname = productListfs.Where(g => g.SKU == sku).ToArray()[0].GoodsName;//产品名称
                                    rowNew["SKU"] = sku;
                                    rowNew["产品名称"] = goodsname;
                                    if (boxCount == 1)
                                    {
                                        rowNew["BoxQty"] = Int32.Parse(rowValue[jc].ToString().Trim());
                                        rowNew["BoxNumber"] = boxNumberStart.ToString();
                                        dtNew.Rows.Add(rowNew.ItemArray);
                                    }
                                    else
                                    {
                                        for (int i = boxNumberStart; i <= boxNumberEnd; i++)
                                        {
                                            rowNew["BoxQty"] = Int32.Parse(rowValue[jc].ToString().Trim());
                                            rowNew["BoxNumber"] = i.ToString();
                                            dtNew.Rows.Add(rowNew.ItemArray);
                                        }
                                    }

                                }
                            }

                        }
                        else
                        {
                            message = "第" + j + "行款号长度不足或款号有误，请检查！";
                        }

                        #endregion


                    }

                    #region 转换两个datatable
                    if (ApplicationConfigHelper.GetWarehouseListByCustomer(CustomerID).Where(w => w.ID == WareHouseID).ToArray().Count() <= 0)//仓库不存在
                    {
                        message = "用户没有分配仓库!!";
                        return;
                    }
                    List<ProductSearch> productLists = new List<ProductSearch>();
                    IEnumerable<ProductSearch> productList;
                    WarehouseInfo warehouse = ApplicationConfigHelper.GetWarehouseListByCustomer(CustomerID).Where(m => m.ID == WareHouseID).ToArray()[0];//仓库
                    IEnumerable<WMS_Customer> customerlist = ApplicationConfigHelper.GetAllCustomerByID(CustomerID);//店铺
                    IEnumerable<WMS_UnitAndSpecifications_Config> wmsunit = ApplicationConfigHelper.GetWMS_UnitAndSpecifications_Config(ProjectID, CustomerID, warehouse.ID);//单位

                    DataSet dsresult = new DataSet();
                    DataTable dtYXDR = dtNew;
                    DataTable dtasn = new DataTable();
                    DataTable dtasndetail = new DataTable();
                    dtasn.TableName = "预入库单主信息$";
                    dtasndetail.TableName = "预入库单明细信息$";

                    //主表
                    dtasn.Columns.Add("外部入库单号");
                    dtasn.Columns.Add("预入库单类型");
                    dtasn.Columns.Add("预入库日期");
                    dtasn.Columns.Add("仓库名称");
                    dtasn.Columns.Add("客户");
                    dtasn.Columns.Add("备注");
                    dtasn.Columns.Add("自定义字段1");
                    dtasn.Columns.Add("自定义字段2");
                    dtasn.Columns.Add("自定义字段3");

                    //子表
                    dtasndetail.Columns.Add("外部入库单号");
                    dtasndetail.Columns.Add("仓库");
                    dtasndetail.Columns.Add("SKU");
                    dtasndetail.Columns.Add("产品名称");
                    dtasndetail.Columns.Add("UPC");
                    dtasndetail.Columns.Add("规格");
                    dtasndetail.Columns.Add("预收数量");
                    dtasndetail.Columns.Add("单位");
                    dtasndetail.Columns.Add("产品等级");
                    dtasndetail.Columns.Add("箱号");
                    dtasndetail.Columns.Add("备注");
                    dtasndetail.Columns.Add("自定义字段4");
                    dtasndetail.Columns.Add("自定义字段5");
                    dtasndetail.Columns.Add("自定义字段6");

                    for (int i = 0; i < dtYXDR.Rows.Count; i++)
                    {
                        if (dtasn.Select("外部入库单号='" + dtYXDR.Rows[i]["订单号"].ToString().Trim() + "'").Count() <= 0)
                        {
                            DataRow dr = dtasn.NewRow();
                            dr["外部入库单号"] = dtYXDR.Rows[i]["订单号"].ToString().Trim();
                            dr["预入库单类型"] = "大仓补货入库";
                            dr["预入库日期"] = dtYXDR.Rows[i]["预入库日期"].ToString();
                            dr["仓库名称"] = warehouse.WarehouseName;
                            dr["客户"] = "YXDRSHH";
                            dr["备注"] = "";
                            dr["自定义字段1"] = "";
                            dr["自定义字段2"] = "";
                            dr["自定义字段3"] = "";
                            dtasn.Rows.Add(dr);

                        }
                        DataRow drdetail = dtasndetail.NewRow();
                        drdetail["外部入库单号"] = dtYXDR.Rows[i]["订单号"].ToString().Trim();
                        drdetail["仓库"] = warehouse.WarehouseName;
                        drdetail["SKU"] = dtYXDR.Rows[i]["SKU"].ToString().Trim();
                        drdetail["产品名称"] = dtYXDR.Rows[i]["产品名称"].ToString().Trim();
                        drdetail["UPC"] = "";
                        drdetail["规格"] = "";
                        drdetail["预收数量"] = dtYXDR.Rows[i]["BoxQty"].ToString().Trim();
                        drdetail["单位"] = "件";
                        drdetail["产品等级"] = "A品";
                        drdetail["箱号"] = dtYXDR.Rows[i]["BoxNumber"].ToString();
                        drdetail["备注"] = "";
                        drdetail["自定义字段4"] = "";
                        drdetail["自定义字段5"] = "";
                        drdetail["自定义字段6"] = "";
                        dtasndetail.Rows.Add(drdetail);
                    }
                    //验证同一外部单号是否存在相同SKU和箱号
                    //var dd = from t in dtasndetail.AsEnumerable()
                    //         group t by new { t1 = t.Field<string>("外部入库单号"), t2 = t.Field<string>("SKU"), t3 = t.Field<string>("箱号") } into m
                    //         select new
                    //         {
                    //             外部单号 = m.Select(p => p.Field<string>("外部入库单号")).First(),
                    //             SKU = m.Select(p => p.Field<string>("SKU")).First(),
                    //             箱号 = m.Select(p => p.Field<string>("箱号")).First(),
                    //             count = m.Count()
                    //         };
                    //var dr1 = dd.Where(d => d.count > 1);
                    //if (dr1.Count() > 0)
                    //{
                    //    foreach (var item in dr1)
                    //    {
                    //        message += "<p><font color='#FF0000'>外部入库单号" + item.外部单号 + "中的SKU:" + item.SKU + "和箱号:" + item.箱号 + "存在重复值！" + "</font></p>";
                    //        return;
                    //    }
                    //}
                    dsresult.Tables.Add(dtasn);
                    dsresult.Tables.Add(dtasndetail);
                    this.AfterData = dsresult;

                    #endregion
                }
            }
            else if (TransDataType == "Receiving")
            {
                DataTable dt = this.Transdata.Tables[0].Copy();
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

                //判断是不是模板中有没有箱号字段

                if (dt.Columns.Contains("箱号"))
                {
                    dt.Columns.Add("托号");
                }
                else
                {
                    message = "模板中没有箱号这一列，请检查！";
                    return;
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    try
                    {
                        dt.Rows[i]["托号"] = dt.Rows[i]["箱号"].ToString().Trim();
                    }
                    catch
                    {
                        dt.Rows[i]["托号"] = "";
                    }

                }
                dt.Columns.Remove("箱号");


                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                this.AfterData = ds;
            }
            else
            {
                this.AfterData = Transdata;
            }
        }



    }
}