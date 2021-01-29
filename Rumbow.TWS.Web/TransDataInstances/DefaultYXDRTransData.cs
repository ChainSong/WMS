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
    public class DefaultYXDRTransData : BaseTransData
    {
        public DefaultYXDRTransData(string TransDataType, long CustomerID, long ProjectID, long WareHouseID,
           DataSet Transdata)
            : base(TransDataType, CustomerID, ProjectID, WareHouseID, Transdata)
        {

        }
        //new PreOrderService().ManualAllocationJson(new ManualAllocationRequest() 
        //{ PodRequest = pod, ID = Convert.ToInt64(ID), CustomerId = CustomerId, Creator = base.UserInfo.Name, Criterion = Criterion });
        public override void CustomerDefinedSettledTransData(ref string message)
        {

            if (TransDataType == "PreOrder")
            {
                #region
                if (WareHouseID == 0)
                {
                    message = "用户没有分配仓库!!";
                    return;
                }
                WarehouseInfo warehouse = ApplicationConfigHelper.GetWarehouseList().Where(m => m.ID == WareHouseID).FirstOrDefault();//customerid获取仓库
                IEnumerable<WMS_Customer> CustomerList = ApplicationConfigHelper.GetAllCustomerByID(CustomerID);
                WMS_UnitAndSpecifications_Config wmsunit = ApplicationConfigHelper.GetWMS_UnitAndSpecifications_Config(ProjectID, CustomerID, warehouse.ID).ToArray()[0];
                DataSet dsResualt = new DataSet();
                DataTable dtpo = new DataTable();
                dtpo.TableName = "预出库单主信息$";
                DataTable dtpodetail = new DataTable();
                dtpodetail.TableName = "预出库单明细信息$";

                DataTable dtNike = Transdata.Tables[0];
                //List<ProductSearch> searprolistfs = new List<ProductSearch>();
                //for (int i = 0; i < dtNike.Rows.Count; i++)
                //{
                //    ProductSearch psfs = new ProductSearch();
                //    psfs.Str10 = dtNike.Rows[i]["Style Color"].ToString();
                //    psfs.Str9 = dtNike.Rows[i]["Size"].ToString();
                //    psfs.Str8 = "01";
                //    searprolistfs.Add(psfs);
                //}
                //IEnumerable<ProductSearch> productListfs = ApplicationConfigHelper.GetSearchProduct(CustomerID, searprolistfs, "Acticle");

                dtpo.Columns.Add("外部单号");
                dtpo.Columns.Add("预出库单类型");
                dtpo.Columns.Add("仓库名称");
                dtpo.Columns.Add("订单日期");
                dtpo.Columns.Add("省");
                dtpo.Columns.Add("市");
                dtpo.Columns.Add("联系人");
                dtpo.Columns.Add("快递公司");
                dtpo.Columns.Add("备注");
                dtpo.Columns.Add("自定义字段2");
                dtpo.Columns.Add("自定义字段3");
                dtpo.Columns.Add("自定义字段4");
                dtpo.Columns.Add("地址");
                dtpo.Columns.Add("联系方式");
                dtpo.Columns.Add("自定义字段5");
                dtpo.Columns.Add("自定义字段6");
                dtpo.Columns.Add("自定义字段7");

                dtpodetail.Columns.Add("外部单号");
                dtpodetail.Columns.Add("仓库");
                dtpodetail.Columns.Add("SKU");
                dtpodetail.Columns.Add("UPC");
                dtpodetail.Columns.Add("产品等级");
                dtpodetail.Columns.Add("产品名称");
                dtpodetail.Columns.Add("批次号");
                dtpodetail.Columns.Add("托号");
                dtpodetail.Columns.Add("备注");
                dtpodetail.Columns.Add("期望数量");
                dtpodetail.Columns.Add("自定义字段4");
                dtpodetail.Columns.Add("自定义字段5");
                dtpodetail.Columns.Add("自定义字段6");
                dtpodetail.Columns.Add("自定义字段7");
                dtpodetail.Columns.Add("单位");
                dtpodetail.Columns.Add("规格");

                for (int i = 0; i < dtNike.Rows.Count; i++)
                {
                    if (dtpo.Select("外部单号='" + dtNike.Rows[i]["出库订单号"].ToString() + "'").Count() <= 0)
                    {
                        DataRow dr = dtpo.NewRow();
                        dr["外部单号"] = dtNike.Rows[i]["出库订单号"].ToString();
                        switch (dtNike.Rows[i]["订单类别"].ToString())
                        {
                            case "2":
                                dr["预出库单类型"] = "门店调拨出库";
                                break;
                            case "6":
                                dr["预出库单类型"] = "退仓出货";
                                break;
                            default:
                                dr["预出库单类型"] = dtNike.Rows[i]["订单类别"].ToString();
                                break;
                        }
                        //dr["预出库单类型"] = dtNike.Rows[i]["订单类别"].ToString() == "2" ? "门店调拨出库" : "退仓出货";//1大仓退货 2门店调拨
                        dr["仓库名称"] = warehouse.WarehouseName;
                        dr["市"] = "";
                        dr["省"] = "";
                        dr["联系人"] = "";
                        dr["订单日期"] = dtNike.Rows[i]["订单日期"].ToString();
                        dr["备注"] = "";
                        dr["自定义字段2"] = dtNike.Rows[i]["门店代码"].ToString();
                        dr["自定义字段3"] = dtNike.Rows[i]["预计发货日期"].ToString();
                        dr["自定义字段4"] = dtNike.Rows[i]["客户要求到货日期"].ToString();
                        dr["地址"] = "";
                        dr["自定义字段5"] = dtNike.Rows[i]["箱号"].ToString();
                        dr["联系方式"] = "";
                        dr["自定义字段6"] = "";
                        dr["自定义字段7"] = "";
                        dtpo.Rows.Add(dr);
                    }

                    DataRow drdetail = dtpodetail.NewRow();
                    drdetail["外部单号"] = dtNike.Rows[i]["出库订单号"].ToString();
                    drdetail["仓库"] = warehouse.WarehouseName;
                    drdetail["SKU"] = dtNike.Rows[i]["SKU"].ToString();
                    drdetail["UPC"] = "";
                    try
                    {
                        drdetail["产品等级"] = dtNike.Rows[i]["产品等级"].ToString();
                    }
                    catch
                    {
                        drdetail["产品等级"] = "A品";
                    }
                    drdetail["产品名称"] = dtNike.Rows[i]["SKU"].ToString();
                    drdetail["批次号"] = "";
                    drdetail["托号"] = "";
                    drdetail["规格"] = "";
                    drdetail["备注"] = "";
                    drdetail["期望数量"] = dtNike.Rows[i]["数量"].ToString();
                    drdetail["自定义字段4"] = dtNike.Rows[i]["箱号"].ToString();
                    drdetail["自定义字段5"] = dtNike.Rows[i]["Article"].ToString();
                    drdetail["自定义字段6"] = dtNike.Rows[i]["Size"].ToString();
                    drdetail["自定义字段7"] = dtNike.Rows[i]["物品类型"].ToString();
                    drdetail["单位"] = wmsunit.Unit;
                    dtpodetail.Rows.Add(drdetail);
                }
                dsResualt.Tables.Add(dtpo);
                dsResualt.Tables.Add(dtpodetail);
                this.AfterData = dsResualt;
                #endregion
            }
            else if (TransDataType == "Asn")
            {
                #region 矩阵导入
                DataTable dtNew = new DataTable();
                DataRow rowNew = dtNew.NewRow();
                DataColumn dc = null;
                //dc = dtNew.Columns.Add("ID", Type.GetType("System.Int32"));
                //dc.AutoIncrement = true;//自动增加 
                //dc.AutoIncrementSeed = 1;//起始为1 
                //dc.AutoIncrementStep = 1;//步长为1 
                //dc.AllowDBNull = false;// 
                dc = dtNew.Columns.Add("类型", Type.GetType("System.String"));
                dc = dtNew.Columns.Add("行号", Type.GetType("System.String"));
                dc = dtNew.Columns.Add("单号", Type.GetType("System.String"));
                dc = dtNew.Columns.Add("日期", Type.GetType("System.DateTime"));
                dc = dtNew.Columns.Add("sold", Type.GetType("System.String"));
                dc = dtNew.Columns.Add("ship", Type.GetType("System.String"));
                dc = dtNew.Columns.Add("客户", Type.GetType("System.String"));
                dc = dtNew.Columns.Add("Article", Type.GetType("System.String"));
                dc = dtNew.Columns.Add("SKU", Type.GetType("System.String"));
                dc = dtNew.Columns.Add("BoxQty", Type.GetType("System.Int32"));
                dc = dtNew.Columns.Add("BoxNumber", Type.GetType("System.String"));
                dc = dtNew.Columns.Add("收货人", Type.GetType("System.String"));
                dc = dtNew.Columns.Add("地址", Type.GetType("System.String"));
                dc = dtNew.Columns.Add("联系方式", Type.GetType("System.String"));
                dc = dtNew.Columns.Add("邮编", Type.GetType("System.String"));
                if (!string.IsNullOrEmpty(Transdata.Tables[0].Rows[0].ToString().Trim()) && Transdata.Tables[0].Rows[0][3].ToString().Trim() == "Z001")
                {
                    #region NIKE装箱单
                    List<ProductSearch> searprolistfs = new List<ProductSearch>();
                    for (int i = 11; i < Transdata.Tables[0].Rows.Count; i++)
                    {
                        ProductSearch psfs = new ProductSearch();
                        if (!string.IsNullOrEmpty(Transdata.Tables[0].Rows[i]["UPC"].ToString().Trim()))
                        {
                            psfs.SKU = Transdata.Tables[0].Rows[i]["UPC"].ToString().Trim();
                            //psfs.Str9 = dtNike.Rows[i]["Size"].ToString();
                            //psfs.Str8 = "01";
                            if (!searprolistfs.Exists(a => a.SKU == psfs.SKU))
                            {
                                searprolistfs.Add(psfs);
                            }
                        }
                    }
                    IEnumerable<ProductSearch> productListfs = ApplicationConfigHelper.GetSearchProductYXDR(CustomerID, searprolistfs, "SKU");

                    for (int j = 0; j < Transdata.Tables[0].Rows.Count; j++)
                    {
                        DataRow rowValue = Transdata.Tables[0].Rows[j];
                        if (string.IsNullOrEmpty(rowValue[0].ToString().Trim()))
                        {
                            continue;
                        }
                        #region 处理数据
                        rowNew["类型"] = rowValue["OrderType"].ToString().Trim();//NIKE
                        rowNew["行号"] = "";
                        rowNew["单号"] = rowValue["ExternOrderKey"].ToString().Trim();
                        rowNew["日期"] = System.DateTime.Now;
                        rowNew["sold"] = "";
                        rowNew["ship"] = "";
                        rowNew["客户"] = "HADDA";
                        rowNew["Article"] = rowValue["Style"].ToString().Trim();
                        rowNew["SKU"] = rowValue["UPC"].ToString().Trim();
                        rowNew["收货人"] = "";
                        rowNew["地址"] = "";
                        rowNew["联系方式"] = "";
                        rowNew["邮编"] = "";
                        rowNew["BoxQty"] = rowValue["QtyShipped"].ToString().Trim();
                        rowNew["BoxNumber"] = rowValue["LabelNo"].ToString().Trim();
                        dtNew.Rows.Add(rowNew.ItemArray);
                        #endregion
                    }
                    #region 转换
                    if (ApplicationConfigHelper.GetWarehouseListByCustomer(CustomerID).Where(p => p.ID == WareHouseID).ToArray().Count() <= 0)
                    {
                        message = "用户没有分配仓库!!";
                        return;
                    }
                    WarehouseInfo warehouse = ApplicationConfigHelper.GetWarehouseListByCustomer(CustomerID).Where(p => p.ID == WareHouseID).ToArray()[0];//
                    IEnumerable<WMS_Customer> CustomerList = ApplicationConfigHelper.GetAllCustomerByID(CustomerID);
                    WMS_UnitAndSpecifications_Config wmsunit = ApplicationConfigHelper.GetWMS_UnitAndSpecifications_Config(ProjectID, CustomerID, warehouse.ID).ToArray()[0];
                    DataSet dsResualt = new DataSet();
                    DataTable dtasn = new DataTable();
                    dtasn.TableName = "预入库单主信息$";
                    DataTable dtasndetail = new DataTable();
                    dtasndetail.TableName = "预入库单明细信息$";

                    DataTable dtNike = dtNew;//

                    dtasn.Columns.Add("外部入库单号");
                    dtasn.Columns.Add("预入库单类型");
                    dtasn.Columns.Add("预入库日期");
                    dtasn.Columns.Add("仓库名称");
                    dtasn.Columns.Add("备注");
                    dtasn.Columns.Add("客户");

                    dtasndetail.Columns.Add("外部入库单号");
                    dtasndetail.Columns.Add("SKU");
                    dtasndetail.Columns.Add("UPC");
                    dtasndetail.Columns.Add("预收数量");
                    dtasndetail.Columns.Add("单位");
                    dtasndetail.Columns.Add("规格");
                    dtasndetail.Columns.Add("批次号");
                    dtasndetail.Columns.Add("托号");

                    for (int i = 0; i < dtNike.Rows.Count; i++)
                    {
                        if (dtNike.Rows[i]["单号"].ToString().Trim() != "" || dtNike.Rows[i]["SKU"].ToString().Trim() != "")
                        {
                            if (dtasn.Select("外部入库单号='" + dtNike.Rows[i]["单号"].ToString().Trim() + "'").Count() <= 0)
                            {
                                DataRow dr = dtasn.NewRow();
                                dr["外部入库单号"] = dtNike.Rows[i]["单号"].ToString().Trim();
                                dr["预入库单类型"] = dtNike.Rows[i]["类型"].ToString().Trim() == "Z001" ? "门店调拨入库" : dtNike.Rows[i][0].ToString().Trim();//入库暂时只有一种类型NIKE入库
                                dr["预入库日期"] = dtNike.Rows[i]["日期"].ToString().Trim();
                                dr["仓库名称"] = warehouse.WarehouseName;
                                dr["备注"] = "";
                                dr["客户"] = dtNike.Rows[i]["客户"].ToString().Trim();
                                dtasn.Rows.Add(dr);
                            }

                            DataRow drdetail = dtasndetail.NewRow();
                            drdetail["外部入库单号"] = dtNike.Rows[i]["单号"].ToString().Trim();
                            drdetail["SKU"] = dtNike.Rows[i]["SKU"].ToString().Trim();
                            drdetail["UPC"] = "";
                            drdetail["预收数量"] = dtNike.Rows[i]["BoxQty"].ToString().Trim();
                            drdetail["单位"] = "";
                            drdetail["规格"] = "";
                            drdetail["批次号"] = "";
                            drdetail["托号"] = dtNike.Rows[i]["BoxNumber"].ToString().Trim();
                            dtasndetail.Rows.Add(drdetail);
                        }
                    }
                    dsResualt.Tables.Add(dtasn);
                    dsResualt.Tables.Add(dtasndetail);
                    this.AfterData = dsResualt;
                    #endregion

                    #endregion
                }
                else
                {
                    #region 普通装箱单
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
                    List<ProductSearch> searprolistfs = new List<ProductSearch>();
                    for (int i = 11; i < Transdata.Tables[0].Rows.Count; i++)
                    {
                        ProductSearch psfs = new ProductSearch();
                        if (!string.IsNullOrEmpty(Transdata.Tables[0].Rows[i][0].ToString().Trim()))
                        {
                            psfs.Str10 = Transdata.Tables[0].Rows[i][0].ToString().Trim();
                            //psfs.Str9 = dtNike.Rows[i]["Size"].ToString();
                            //psfs.Str8 = "01";
                            if (!searprolistfs.Exists(a => a.Str10 == psfs.Str10))
                            {
                                searprolistfs.Add(psfs);
                            }
                        }
                    }
                    IEnumerable<ProductSearch> productListfs = ApplicationConfigHelper.GetSearchProductYXDR(CustomerID, searprolistfs, "Acticle");

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
                            string receiptnumber = Transdata.Tables[0].Rows[1][2].ToString().Trim() + "-";
                            string receipttype = Transdata.Tables[0].Rows[1][1].ToString().Trim();
                            string Article = rowValue[0].ToString().Trim();
                            string Color = rowValue[5].ToString().Trim();
                            int boxNumberStart = Int32.Parse(rowValue[1].ToString().Trim());
                            int boxNumberEnd = Int32.Parse(rowValue[3].ToString().Trim());
                            int boxCount = Int32.Parse(rowValue[4].ToString().Trim());
                            //if ( dtNew.Select("Article='"+Article+"'").Count()<=0 )
                            //{
                            //    rowNew["单号"]=ordernumber + rowValue[0].ToString().Trim().Substring(rowValue[0].ToString().Trim().Length-1,rowValue[0].ToString().Trim().Length-4);
                            //}
                            rowNew["类型"] = receipttype;
                            rowNew["行号"] = "";
                            rowNew["单号"] = receiptnumber + rowValue[0].ToString().Trim().Substring(rowValue[0].ToString().Trim().Length - 3, 3);
                            rowNew["日期"] = System.DateTime.Now;
                            rowNew["sold"] = "";
                            rowNew["ship"] = "";
                            rowNew["客户"] = "HADDA";
                            rowNew["Article"] = "Article";
                            rowNew["收货人"] = "";
                            rowNew["地址"] = "";
                            rowNew["联系方式"] = "";
                            rowNew["邮编"] = "";

                            //string Size = rowValue[0].ToString().Trim().Substring(rowValue[0].ToString().Trim().Length - 3, 3);
                            for (int jc = 6; jc <= 16; jc++)
                            {
                                if (!string.IsNullOrEmpty(rowValue[jc].ToString().Trim()))
                                {
                                    string size = Transdata.Tables[0].Rows[9][jc].ToString().Trim();
                                    if (productListfs.Where(a => a.Str10 == Article & a.Str9 == size & a.Str8 == Color).OrderByDescending(a => a.Str10).Select(a => a.SKU).Count() <= 0)
                                    {
                                        message = "款号" + Article + " 尺码" + size + " 颜色" + Color + " 不存在！！";
                                    }
                                    string SKU = productListfs.Where(a => a.Str10 == Article & a.Str9 == size & a.Str8 == Color).OrderByDescending(a => a.Str10).Select(a => a.SKU).FirstOrDefault().ToString();
                                    rowNew["SKU"] = SKU;
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
                            message = "行" + j + "，款号长度不足或款号有误!!";
                            return;
                        }
                        #endregion
                    }
                    #endregion
                    #region 转换
                    if (ApplicationConfigHelper.GetWarehouseListByCustomer(CustomerID).Where(p => p.ID == WareHouseID).ToArray().Count() <= 0)
                    {
                        message = "用户没有分配仓库!!";
                        return;
                    }
                    WarehouseInfo warehouse = ApplicationConfigHelper.GetWarehouseListByCustomer(CustomerID).Where(p => p.ID == WareHouseID).ToArray()[0];//
                    IEnumerable<WMS_Customer> CustomerList = ApplicationConfigHelper.GetAllCustomerByID(CustomerID);
                    WMS_UnitAndSpecifications_Config wmsunit = ApplicationConfigHelper.GetWMS_UnitAndSpecifications_Config(ProjectID, CustomerID, warehouse.ID).ToArray()[0];
                    DataSet dsResualt = new DataSet();
                    DataTable dtasn = new DataTable();
                    dtasn.TableName = "预入库单主信息$";
                    DataTable dtasndetail = new DataTable();
                    dtasndetail.TableName = "预入库单明细信息$";

                    DataTable dtNike = dtNew;//

                    dtasn.Columns.Add("外部入库单号");
                    dtasn.Columns.Add("预入库单类型");
                    dtasn.Columns.Add("预入库日期");
                    dtasn.Columns.Add("仓库名称");
                    dtasn.Columns.Add("备注");
                    dtasn.Columns.Add("客户");

                    dtasndetail.Columns.Add("外部入库单号");
                    dtasndetail.Columns.Add("SKU");
                    dtasndetail.Columns.Add("UPC");
                    dtasndetail.Columns.Add("预收数量");
                    dtasndetail.Columns.Add("单位");
                    dtasndetail.Columns.Add("规格");
                    dtasndetail.Columns.Add("批次号");
                    dtasndetail.Columns.Add("托号");

                    for (int i = 0; i < dtNike.Rows.Count; i++)
                    {
                        if (dtNike.Rows[i]["单号"].ToString().Trim() != "" || dtNike.Rows[i]["SKU"].ToString().Trim() != "")
                        {
                            if (dtasn.Select("外部入库单号='" + dtNike.Rows[i][2].ToString().Trim() + "'").Count() <= 0)
                            {
                                DataRow dr = dtasn.NewRow();
                                dr["外部入库单号"] = dtNike.Rows[i][2].ToString().Trim();
                                dr["预入库单类型"] = dtNike.Rows[i][0].ToString().Trim() == "1" ? "大仓补货入库" : dtNike.Rows[i][0].ToString().Trim();//入库暂时只有一种类型
                                dr["预入库日期"] = dtNike.Rows[i][3].ToString().Trim();
                                dr["仓库名称"] = warehouse.WarehouseName;
                                dr["备注"] = "";
                                dr["客户"] = dtNike.Rows[i][6].ToString().Trim();
                                dtasn.Rows.Add(dr);
                            }

                            DataRow drdetail = dtasndetail.NewRow();
                            drdetail["外部入库单号"] = dtNike.Rows[i][2].ToString().Trim();
                            drdetail["SKU"] = dtNike.Rows[i][8].ToString().Trim();
                            drdetail["UPC"] = "";
                            drdetail["预收数量"] = dtNike.Rows[i][9].ToString().Trim();
                            drdetail["单位"] = "";
                            drdetail["规格"] = "";
                            drdetail["批次号"] = "";
                            drdetail["托号"] = dtNike.Rows[i][10].ToString().Trim();
                            dtasndetail.Rows.Add(drdetail);
                        }
                    }
                    dsResualt.Tables.Add(dtasn);
                    dsResualt.Tables.Add(dtasndetail);
                    this.AfterData = dsResualt;

                    #endregion
                }
                #endregion
            }
            else
            {
                this.AfterData = Transdata;
            }
        }
    }
}