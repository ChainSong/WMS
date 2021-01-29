using Runbow.TWS.Biz;
using Runbow.TWS.Biz.WMS;
using Runbow.TWS.Common;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.Log;
using Runbow.TWS.Entity.WMS.Order;
using Runbow.TWS.MessageContracts.WMS.PreOrders;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NikeGetB2COrder
{
    public class OrderManage
    {

        /// <summary>
        /// 同步2c订单
        /// </summary>
        public void SyncB2COrder()
        {
            List<NikeCrodOrderLog> logs = new List<NikeCrodOrderLog>();

            //读取配置文件                 
            XmlSerializerHelper<OrderConfig> config = new XmlSerializerHelper<OrderConfig>();
            config.Load(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "StoreConfig.xml"));
            if (config != null && config.Value != null && config.Value.customerStores != null && config.Value.customerStores.Any() && config.Value.orderTypes != null && config.Value.orderTypes.Any())
            {
                List<CustomerStore> storeConfig = config.Value.customerStores.ToList();
                List<OrderTypeConfig> orderTypeConfig = config.Value.orderTypes.ToList();
                //获取中间表的订单信息         
                OrderECModel model = new OrderECManagementService().GetNikeOrderB2C(1);
                if (model != null && model.Order_Headers != null && model.Order_Headers.Any() && model.Order_Details != null && model.Order_Details.Any())
                {
                    //PreOrderRequest request = new PreOrderRequest();
                    IList<PreOrder> preorders = new List<PreOrder>();
                    IList<PreOrderDetail> preorderDetails = new List<PreOrderDetail>();
                    foreach (var item in model.Order_Headers)
                    {
                        if (string.IsNullOrEmpty(item.orderCode))
                        {
                            continue;
                        }
                        List<Order_Detail> details = model.Order_Details.Where(m => m.orderCode == item.orderCode).ToList();
                        //没有明细
                        if (details == null || !details.Any())
                        {
                            logs.Add(new NikeCrodOrderLog()
                            {
                                OrderCode = item.orderCode,
                                Type = "GetB2COrder",
                                Operation = "抓取B2C订单",
                                Remark = "抓取失败：该订单没有明细信息",
                                Creator = "sysService",
                                Str1 = "",
                                Str2 = "",
                                Str3 = "",
                                Str4 = "",
                                Str5 = ""
                            });
                            continue;
                        }
                        CustomerStore _store = storeConfig.Where(m => m.StoreKey == item.appointStcode).FirstOrDefault();
                        OrderTypeConfig _ordertype = orderTypeConfig.Where(m => m.OrderTypeCode.ToString() == item.platType).FirstOrDefault();
                        //没有找到这家门店 ，log     
                        if (_store == null || _ordertype == null)
                        {
                            logs.Add(new NikeCrodOrderLog()
                            {
                                OrderCode = item.orderCode,
                                Type = "GetB2COrder",
                                Operation = "抓取B2C订单",
                                Remark = "抓取失败：该订单对应的门店代码或者订单类型未配置",
                                Creator = "sysService",
                                Str1 = "",
                                Str2 = "",
                                Str3 = "",
                                Str4 = "",
                                Str5 = ""
                            });
                            continue;
                        }


                        #region 头信息
                        PreOrder order = new PreOrder();
                        order.ExternOrderNumber = item.orderCode;
                        order.CustomerID = _store.CustomerID;
                        order.CustomerName = _store.CustomerName;
                        order.Warehouse = _store.WarehouseName;
                        order.OrderType = _ordertype.OrderType;
                        order.Status = 1;
                        order.OrderTime = item.pushTime.ObjectToNullableDateTime();
                        order.Province = item.province;
                        order.City = item.city;
                        order.District = item.district;
                        order.Address = item.address;
                        order.Consignee = item.receiver;
                        order.Contact = item.receiverPhone;
                        order.ExpressCompany = "顺丰快递";//默认顺丰
                        order.DetailCount = details.Count;
                        order.Creator = "sysService";
                        order.str4 = item.appointStcode;//门店代码
                        order.str2 = item.platType == "31" ? "3721" : item.appointStcode;
                        order.str13 = "cord";//订单打个标记，代表cord订单

                        //为了方便知道往哪个库里面插入，打个标记
                        order.str20 = _store.DBType;
                        preorders.Add(order);
                        #endregion

                        #region 明细
                        foreach (var detailitem in details)
                        {
                            PreOrderDetail detail = new PreOrderDetail();
                            detail.ExternOrderNumber = item.orderCode;
                            detail.CustomerID = _store.CustomerID;
                            detail.CustomerName = _store.CustomerName;
                            detail.Warehouse = _store.WarehouseName;
                            detail.WarehouseId = _store.WarehouseID;
                            detail.LineNumber = detailitem.lineId;
                            detail.SKU = detailitem.barCode;
                            detail.GoodsName = detailitem.skuName;
                            detail.GoodsType = "A品";
                            detail.OriginalQty = detailitem.quantity;
                            detail.Creator = order.Creator;
                            detail.str6 = detailitem.style_color + "-" + detailitem.size;

                            detail.str20 = _store.DBType;
                            preorderDetails.Add(detail);
                        }
                        #endregion
                    }
                    if (preorders != null && preorders.Any() && preorderDetails != null && preorderDetails.Any())
                    {
                        List<string> dbs = preorders.Select(m => m.str20).Distinct().ToList();
                        //不同数据库的订单
                        foreach (var dbitem in dbs)
                        {
                            List<PreOrder> preodertowms = preorders.Where(m => m.str20 == dbitem).ToList();//得到本库的订单
                            List<PreOrderDetail> predetailtowms = preorderDetails.Where(m => m.str20 == dbitem).ToList();

                            //request.PreOrderList = preorders.Where(m => m.str20 == dbitem).ToList();
                            //request.PreOd = preorderDetails.Where(m => m.str20 == dbitem).ToList();
                            //插入数据库
                            string dbconnStr = "";
                            if (dbitem == "1")//1.上海数据库
                            {
                                dbconnStr = Common.SHWMSSqlConnection;
                            }
                            else if (dbitem == "2")//2.成都武汉西安那个数据库
                            {
                                dbconnStr = Common.CDWMSSSqlConnection;
                            }

                            IEnumerable<PreOrder> wmspreorders = new PreOrderService().GetWMSPreOrdersByExternNumber(preodertowms, dbconnStr);
                            if (wmspreorders != null && wmspreorders.Any())
                            {
                                foreach (var item in wmspreorders)
                                {
                                    preodertowms.Remove(preodertowms.Where(m => m.ExternOrderNumber == item.ExternOrderNumber).FirstOrDefault());
                                    predetailtowms.Remove(predetailtowms.Where(m => m.ExternOrderNumber == item.ExternOrderNumber).FirstOrDefault());
                                }
                            }
                            if (!preodertowms.Any())//本库的没有订单了，下一个库
                            {
                                continue;
                            }

                            preodertowms.Where(m => m.str20 == dbitem).ToList().ForEach(m => m.str20 = null);
                            predetailtowms.Where(m => m.str20 == dbitem).ToList().ForEach(m => m.str20 = null);


                            var response = new PreOrderService().AddPreOrderAndPreOrderDetailDynamicConn(preodertowms, predetailtowms, "sysService", dbconnStr);
                            //成功
                            if (response.IsSuccess && response.Result != null && response.Result.PreOrderList != null && response.Result.PreOrderList.Any())
                            {
                                foreach (var item in preodertowms)
                                {
                                    logs.Add(new NikeCrodOrderLog()
                                    {
                                        OrderCode = item.ExternOrderNumber,
                                        Type = "GetB2COrder",
                                        Operation = "抓取B2C订单",
                                        Remark = "抓取成功",
                                        Creator = "sysService",
                                        Str1 = "",
                                        Str2 = "",
                                        Str3 = "",
                                        Str4 = "",
                                        Str5 = ""
                                    });
                                }
                                //更新中间表状态
                                List<string> Numbers = preodertowms.Select(m => m.ExternOrderNumber).Distinct().ToList();
                                new OrderECManagementService().UpdateNikeOrderStatus(Numbers);
                            }
                            else
                            {
                                foreach (var item in preodertowms)
                                {
                                    logs.Add(new NikeCrodOrderLog()
                                    {
                                        OrderCode = item.ExternOrderNumber,
                                        Type = "GetB2COrder",
                                        Operation = "抓取B2C订单",
                                        Remark = "抓取失败：订单数据库插入失败",
                                        Creator = "sysService",
                                        Str1 = "",
                                        Str2 = "",
                                        Str3 = "",
                                        Str4 = "",
                                        Str5 = ""
                                    });
                                }
                            }
                        }
                    }
                    else
                    {
                        //没有需要插入的订单
                    }
                }
                else
                {
                    //中间表没查到数据                        
                }
            }
            else
            {
                logs.Add(new NikeCrodOrderLog()
                {
                    OrderCode = "",
                    Type = "GetB2COrder",
                    Operation = "抓取B2C订单",
                    Remark = "抓取失败：未读取到门店和客户仓库配置文件",
                    Creator = "sysService",
                    Str1 = "",
                    Str2 = "",
                    Str3 = "",
                    Str4 = "",
                    Str5 = ""
                });
            }
            //日志 
            if (logs != null && logs.Any())
            {
                new LogOperationService().AddNikeCordOrderLog(logs);
            }
        }
    }
}
