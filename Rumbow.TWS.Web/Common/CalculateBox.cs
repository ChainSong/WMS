using Runbow.TWS.Biz;
using Runbow.TWS.Biz.WMS;
using Runbow.TWS.Common;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS;
using Runbow.TWS.Entity.WMS.Box;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.MessageContracts.WMS.PreOrders;
using Runbow.TWS.Web.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Runbow.TWS.Web.Common
{
    public class CalculateBox
    {
        List<BoxTypeModel> boxTypeModels = new List<BoxTypeModel>();
        List<List<CalculateBoxModel>> boxs = new List<List<CalculateBoxModel>>();
        C_LogWriter c_LogWriter = new C_LogWriter();//日志记录

        private string CustomerId = "";
        //声明一个全局的空箱子将装不下的先暂存
        //List<CalculateBoxModel> boxModels = new List<CalculateBoxModel>();
        public bool preOrderDetailsFun(GetOrderByConditionRequest Request)
        {
            c_LogWriter.Init((LogType)Enum.Parse(typeof(LogType), "SysLog"));
            c_LogWriter.Error("发送错误，线程退出，客户端ID  ex.Message");
            CustomerId = Request.SearchCondition.CustomerID.ToString();
            var preOrderDetails = new OrderManagementService().GetOrderDetailBox(Request.SearchCondition).Result.calculateBoxModels;
            boxTypeModels = new PreOrderService().GetBoxType(CustomerId).Result.boxTypeModels.ToList();
            var GetPuductInfo = new ProductService().GetPuductInfo(Request.SearchCondition);
            foreach (var item in GetPuductInfo.Result)
            {
                item.SKUvolume = item.SKUlength * item.SKUwidth * item.SKUhigh;
                RedisCacheHelper.Set(CustomerId + "_" + item.SKU, item, DateTime.Now.AddMonths(3));
            }
            //var preOrderDetailsGroup = preOrderDetails.GroupBy(a => new { a.SKU, a.ExternOrderNumber, a.GoodsName, a.GoodsType, a.PreOrderNumber, a.Warehouse })
            //     .Select(a => new { AllocatedQty = a.Sum(b => b.AllocatedQty), a }).ToList();
            //List<CalculateBoxModel> calculateBoxModels = new List<CalculateBoxModel>();
            //calculateBoxModels.AddRange(preOrderDetails.Where(a => !string.IsNullOrEmpty(a.BoxNumber)).ToList());
            foreach (var item in preOrderDetails.Where(a => !string.IsNullOrEmpty(a.BoxNumber)).ToList())
            {
                boxs.Add(new List<CalculateBoxModel> { item });
            }
            //var wholeBox = preOrderDetails.Where(a => a.AllocatedQty / a.SKUBoxspecifications >= 1).ToList();
            var scatteredBox = preOrderDetails.Where(a => string.IsNullOrEmpty(a.BoxNumber)).ToList();



            //先对SKU分组
            foreach (var item in scatteredBox.GroupBy(a => new { a.ExternOrderNumber, a.SKU }))
            {
                //规格
                //int SKUBoxspecifications = 6;

                geyBox(item.ToList());
                //获取到SKU 
            }
            //boxs
            List<CalculateBoxModel> calculates = new List<CalculateBoxModel>();
            for (int i = 0; i < boxs.Count; i++)
            {
                boxs[i].Each((a, b) =>
                {
                    b.BoxCode = b.ExternOrderNumber + i;
                    calculates.Add(b);
                });
            }
          
            return true;
        }
        public void geyBox(List<CalculateBoxModel> preOrderDetails)
        {
            //创建箱集合
            //List<List<CalculateBoxModel>> calculateBoxModels = new List<List<CalculateBoxModel>>();
            var ProductInfo = RedisCacheHelper.Get<BoxProductInfo>(CustomerId + "_" + preOrderDetails.FirstOrDefault().SKU);

            string boxType = "";
            for (int i = 0; i < boxTypeModels.Count(); i++)
            {
                var boxVolume = preOrderDetails.Sum(c => c.Qty) * ProductInfo.SKUvolume;
                //当箱子可以装下，就先给一个箱类型，继续循环到小箱
                if (boxTypeModels[i].BoxVolume >= boxVolume)
                {
                    //比较箱子能否放下
                    if (ProductInfo.SKUlength <= boxTypeModels[i].BoxLength && ProductInfo.SKUwidth <= boxTypeModels[i].BoxWidth && ProductInfo.SKUhigh <= boxTypeModels[i].BoxHigh)
                    {
                        boxType = boxTypeModels[i].BoxType;
                    }
                }
                else
                {
                    if (i == 0)
                    {

                        //使用最大的箱子还装不下，就拆箱
                        //拆箱的逻辑是一件一件的往外拿
                        //再分配的过程中，会出现同样的数据但是ID不同，为保证数据的一致性和完整性。此处循环带着ID
                        //calculateBoxModels.Add();
                        //boxModels = new List<CalculateBoxModel>();
                        CalculateBoxModels(preOrderDetails);

                        break;
                    }
                    else
                    {
                        preOrderDetails.Each((c, d) =>
                        {
                            d.BoxType = boxType;
                        });
                        boxs.Add(preOrderDetails);
                        //到这里说明这个箱子装不下了，该箱子也不是最大的箱子。那么就取上一次循环中的箱型
                        break;
                    }
                }
            };
        }
        public void CalculateBoxModels(List<CalculateBoxModel> calculates)
        {
            //先做一个箱子
            List<CalculateBoxModel> boxModels = new List<CalculateBoxModel>();
            var ProductInfo = RedisCacheHelper.Get<BoxProductInfo>(CustomerId + "_" + calculates.FirstOrDefault().SKU);

            do
            {
                var model = calculates.Where(a => a.Qty > 0).First();
                //往箱子里面放东西
                boxModels.Add(new CalculateBoxModel(model)
                {
                    Qty = 1
                });
                model.Qty -= 1;

                var boxVolume = boxModels.Where(a => a.Qty > 0).Sum(c => c.Qty) * ProductInfo.SKUvolume;
                string boxType = "";
                if (boxTypeModels[0].BoxVolume <= boxVolume)
                {
                    if (calculates.Sum(a => a.Qty) == 1)
                    {
                        boxModels.Each((c, d) =>
                        {
                            d.BoxType = "异常箱";
                        });
                        boxs.Add(boxModels);
                        boxModels = new List<CalculateBoxModel>();
                        break;
                    }
                    //将最后放进去的东西拿出来
                    model.Qty += 1;
                    boxModels.RemoveAt(boxModels.Count() - 1);

                    //重新计算东西体积
                    boxVolume = boxModels.Where(a => a.Qty > 0).Sum(c => c.Qty) * ProductInfo.SKUvolume;

                    for (int j = 0; j < boxTypeModels.Count; j++)
                    {
                        //当箱子可以装下，就先给一个箱类型，继续循环到小箱
                        if (boxTypeModels[j].BoxVolume >= boxVolume)
                        {
                            boxType = boxTypeModels[j].BoxType;
                        }
                        else if ((boxTypeModels[j].BoxVolume < boxVolume) && j == boxTypeModels.Count)
                        {
                            boxModels.Each((c, d) =>
                            {
                                d.BoxType = boxTypeModels[j - 1].BoxType;
                            });
                            boxs.Add(boxModels);
                            boxModels = new List<CalculateBoxModel>();
                            break;
                        }
                        else
                        {
                            boxModels.Each((c, d) =>
                            {
                                d.BoxType = boxType;
                            });
                            boxs.Add(boxModels);
                            boxModels = new List<CalculateBoxModel>();
                            break;
                        }

                    }
                }
                else if (boxTypeModels[0].BoxVolume > boxVolume && calculates.Sum(a => a.Qty) == 0)
                {
                    //重新计算东西体积
                    boxVolume = boxModels.Where(a => a.Qty > 0).Sum(c => c.Qty) * ProductInfo.SKUvolume;

                    for (int j = 0; j < boxTypeModels.Count; j++)
                    {
                        //当箱子可以装下，就先给一个箱类型，继续循环到小箱
                        if (boxTypeModels[j].BoxVolume >= boxVolume)
                        {
                            boxType = boxTypeModels[j].BoxType;
                        }
                        else if ((boxTypeModels[j].BoxVolume < boxVolume) && j == boxTypeModels.Count)
                        {
                            boxModels.Each((c, d) =>
                            {
                                d.BoxType = boxTypeModels[j - 1].BoxType;
                            });
                            boxs.Add(boxModels);
                            boxModels = new List<CalculateBoxModel>();
                            break;
                        }
                        else
                        {
                            boxModels.Each((c, d) =>
                            {
                                d.BoxType = boxType;
                            });
                            boxs.Add(boxModels);
                            boxModels = new List<CalculateBoxModel>();
                            break;
                        }

                    }
                }
                else if (ProductInfo.SKUlength > boxTypeModels[0].BoxLength || ProductInfo.SKUwidth > boxTypeModels[0].BoxWidth || ProductInfo.SKUhigh > boxTypeModels[0].BoxHigh)
                {
                    boxModels.Each((c, d) =>
                    {
                        d.BoxType = "异常箱";
                    });
                    boxs.Add(boxModels);
                    boxModels = new List<CalculateBoxModel>();
                    break;
                }

            } while (calculates.Sum(a => a.Qty) > 0);
        }



        public bool CalculationBoxType(List<CalculateBoxModel> preOrderDetails)
        {

            //比较SKU能不能放进箱子
            preOrderDetails.GroupBy(a => a.SKU).Select(a => new
            {
                SKUlength = a.Max(b => b.SKUlength),
                SKUwidth = a.Max(b => b.SKUwidth),
                SKUhigh = a.Max(b => b.SKUhigh)
            });

            return true;
        }
    }
}