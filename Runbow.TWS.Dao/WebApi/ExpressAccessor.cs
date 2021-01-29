using Runbow.TWS.Common;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Runbow.TWS.MessageContracts.WebApi.Express;
using Runbow.TWS.Entity.WebApi.Express;

namespace Runbow.TWS.Dao.WebApi
{
    public class ExpressAccessor : BaseAccessor
    {
        public YtoRequest GetExpressNumYto(string PackageNumber)
        {
            YtoRequest ytoRequest = new YtoRequest();
            try
            {
                StringBuilder sb = new StringBuilder();
                //先获取订单信息
                sb.Append(@" select  top 1 *,(select top 1 ID from WMS_Warehouse where WMS_Warehouse.WarehouseName=WMS_Order.Warehouse ) WarehouseID from WMS_Package 
                left join 
                WMS_Order
                on WMS_Package.OID=WMS_Order.ID
                where  WMS_Package.PackageNumber='" + PackageNumber + "'");
                //获取订单明细
                sb.Append(@" select  * from WMS_PackageDetail where PackageNumber='" + PackageNumber + "'");
                //获取发货地址
                sb.Append(@" select top 1 * from WMS_Warehouse 
                where WarehouseName =(
                select  top 1 Warehouse from WMS_Package where PackageNumber='" + PackageNumber + @"')
                ");
                var data = this.ScanDataSet(sb.ToString());
                ytoRequest.orderInfo = data.Tables[0].ConvertToEntity<OrderInfo>();
                ytoRequest.orderDetailInfos = data.Tables[1].ConvertToEntityCollection<OrderDetailInfo>().ToList();
                ytoRequest.warehouseInfo = data.Tables[2].ConvertToEntity<WarehouseInfo>();

            }
            catch (Exception)
            {

            }
            return ytoRequest;
        }

        public int InsExpressNumYto(YtoRequest ytoRequest)
        {
            //YtoRequest ytoRequest = new YtoRequest();
            try
            {
                StringBuilder sb = new StringBuilder();
                //先获取订单信息
                sb.Append(@"  
                        insert into  WMS_ExpressDelivery (
                             [CustomerID],[CustomerName],[WarehouseID],[WarehouseName]
                             ,[OID],[OrderNumber],[ExternOrderNumber],[ExpressNumber],[ExpressCompany],[PackageNumber]
                             ,[Status],[success],[code],[logisticProviderID],[mailNo]
                             ,[txLogisticID],[clientID],[shortAddress],[consigneeBranchCode],[packageCenterCode] ,[packageCenterName],[arrivedOrgSimpleName],[uniquerRequestNumber],[parentMailNo],[reason],originateOrgCode,printKeyWord
                             ,[mn],[pcn],[rbc],[sbc],[ssc],[tsc],[Remark],[Creator],[CreateTime]
                        ) values (
                 '" + ytoRequest.expressDelivery.CustomerID + @"',
                 '" + ytoRequest.expressDelivery.CustomerName + @"',
                 '" + ytoRequest.expressDelivery.WarehouseID + @"',
                 '" + ytoRequest.expressDelivery.WarehouseName + @"',
                 '" + ytoRequest.expressDelivery.OID + @"',
                 '" + ytoRequest.expressDelivery.OrderNumber + @"',
                 '" + ytoRequest.expressDelivery.ExternOrderNumber + @"',
                 '" + ytoRequest.expressDelivery.ExpressNumber + @"',
                 '" + ytoRequest.expressDelivery.ExpressCompany + @"',
                 '" + ytoRequest.expressDelivery.PackageNumber + @"',
                 '" + ytoRequest.expressDelivery.Status + @"',
                 '" + ytoRequest.expressDelivery.success + @"',
                 '" + ytoRequest.expressDelivery.code + @"',
                 '" + ytoRequest.expressDelivery.logisticProviderID + @"',
                 '" + ytoRequest.expressDelivery.mailNo + @"',
                 '" + ytoRequest.expressDelivery.txLogisticID + @"',
                 '" + ytoRequest.expressDelivery.clientID + @"',
                 '" + ytoRequest.expressDelivery.shortAddress + @"',
                 '" + ytoRequest.expressDelivery.consigneeBranchCode + @"',
                 '" + ytoRequest.expressDelivery.packageCenterCode + @"',
                 '" + ytoRequest.expressDelivery.packageCenterName + @"',
                 '" + ytoRequest.expressDelivery.arrivedOrgSimpleName + @"',
                 '" + ytoRequest.expressDelivery.uniquerRequestNumber + @"',
                 '" + ytoRequest.expressDelivery.parentMailNo + @"',
                 '" + ytoRequest.expressDelivery.reason + @"',
                 '" + ytoRequest.expressDelivery.originateOrgCode + @"',
                 '" + ytoRequest.expressDelivery.printKeyWord + @"',
                 '" + ytoRequest.expressDelivery.mn + @"','" + ytoRequest.expressDelivery.pcn + @"','" + ytoRequest.expressDelivery.rbc + @"','" + ytoRequest.expressDelivery.sbc + @"','" + ytoRequest.expressDelivery.ssc + @"','" + ytoRequest.expressDelivery.tsc + @"','" + ytoRequest.expressDelivery.Remark + @"','" + ytoRequest.expressDelivery.Creator + @"','" + ytoRequest.expressDelivery.CreateTime + @"');
                   --更新包装快递信息
                   UPDATE dbo.WMS_Package SET ExpressCompany='" + ytoRequest.expressDelivery.ExpressCompany + @"',ExpressNumber='" + ytoRequest.expressDelivery.ExpressNumber + @"' WHERE PackageNumber='" + ytoRequest.expressDelivery.PackageNumber + @"';

                    --更新订单主表快递
                   UPDATE o SET o.ExpressCompany=p.ExpressCompany,o.ExpressNumber=p.ExpressNumber
                   FROM dbo.WMS_Order o,
                   (SELECT TOP 1 * FROM dbo.WMS_Package WHERE OrderNumber =(SELECT TOP 1 OrderNumber FROM dbo.WMS_Package WHERE PackageNumber='" + ytoRequest.expressDelivery.PackageNumber + @"')) p
                   WHERE o.ID=p.OID;

                   SELECT TOP 1000 * FROM dbo.WMS_ExpressDelivery WHERE ID=@@IDENTITY;");
                var data = this.ScanExecuteNonQuery(sb.ToString());
                if (data > 0)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }

            }
            catch (Exception)
            {

            }
            return 0;
        }



        public ExpressResponse GetOrder(string PackageNumber, string OrderType)
        {
            ExpressResponse response = new ExpressResponse();
            DbParam[] dbParams = new DbParam[] {
                new DbParam("@PackageNumber",DbType.String,PackageNumber,ParameterDirection.Input),
                new DbParam("@OrderType",DbType.String,OrderType,ParameterDirection.Input)
            };
            DataSet ds = base.ExecuteDataSet("Proc_GetOrderExpressJT", dbParams);
            response.orderInfo = ds.Tables[0].ConvertToEntity<OrderInfo>();
            response.packageInfos = ds.Tables[1].ConvertToEntityCollection<Entity.PackageInfo>().ToList();
            response.warehouseInfo = ds.Tables[2].ConvertToEntity<WarehouseInfo>();

            return response;
        }

        /// <summary>
        /// 获取需要申请快递的订单，订单明细，包装，包装明细，仓库信息
        /// </summary>
        /// <param name="PackageNumber"></param>
        /// <param name="OrderType"></param>
        /// <returns></returns>
        public ExpressResponse GetOrderByExpress(string PackageNumber, string OrderType)
        {
            ExpressResponse response = new ExpressResponse();
            DbParam[] dbParams = new DbParam[] {
                new DbParam("@PackageNumber",DbType.String,PackageNumber,ParameterDirection.Input),
                new DbParam("@OrderType",DbType.String,OrderType,ParameterDirection.Input)
            };
            DataSet ds = base.ExecuteDataSet("Proc_GetOrderByExpress", dbParams);
            response.orderInfo = ds.Tables[0].ConvertToEntity<Entity.OrderInfo>();
            response.orderDetailInfos = ds.Tables[1].ConvertToEntityCollection<Entity.OrderDetailInfo>().ToList();
            response.packageInfos = ds.Tables[2].ConvertToEntityCollection<Entity.PackageInfo>().ToList();
            response.packageDetailInfos = ds.Tables[3].ConvertToEntityCollection<Entity.PackageDetailInfo>().ToList();
            response.warehouseInfo = ds.Tables[4].ConvertToEntity<WarehouseInfo>();

            return response;
        }


        public void AddExpressAndUpdatePackage(DPCreateOrderResponse response, Entity.PackageInfo package)
        {
            string sql = string.Format(@"INSERT INTO dbo.WMS_ExpressDelivery
                                            (
                                                CustomerID,CustomerName,WarehouseID,WarehouseName,OID,OrderNumber,ExternOrderNumber,
                                                ExpressNumber,ExpressCompany,PackageNumber,
                                                Status,
                                                success,code,mailNo,txLogisticID,arrivedOrgSimpleName,reason,uniquerRequestNumber,parentMailNo,
                                                Creator,CreateTime
                                            )
                                            SELECT 
                                            CustomerID,CustomerName,3,Warehouse,OID,OrderNumber,ExternOrderNumber,
                                            '{0}','德邦',PackageNumber,
                                            0,
                                            '{1}','{2}','{0}','{3}','{4}','{5}','{6}','{7}',
                                            '德邦',GETDATE()
                                            FROM dbo.WMS_Package WHERE PackageNumber='{8}';
                                            --更新包装快递信息
                                            UPDATE dbo.WMS_Package SET ExpressCompany='德邦',ExpressNumber='{0}' WHERE PackageNumber='{8}';

                                            --更新订单主表快递
                                            UPDATE o SET o.ExpressCompany=p.ExpressCompany,o.ExpressNumber=p.ExpressNumber
                                            FROM dbo.WMS_Order o,
                                             (SELECT TOP 1 * FROM dbo.WMS_Package WHERE OrderNumber =(SELECT TOP 1 OrderNumber FROM dbo.WMS_Package WHERE PackageNumber='{8}')) p
                                            WHERE o.ID=p.OID;

                                            SELECT TOP 1000 * FROM dbo.WMS_ExpressDelivery WHERE ID=@@IDENTITY;",
                                        package.ExpressNumber, response.result, response.resultCode, response.logisticID, response.arrivedOrgSimpleName,
                                        response.reason, response.uniquerRequestNumber, response.parentMailNo, package.PackageNumber);
            base.ExecuteNoQueryBySqlString(sql);
        }

        public ExpressResponse GetOrderByYD(string PackageNumber)
        {
            ExpressResponse response = new ExpressResponse();

            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT TOP 1 * FROM dbo.WMS_Order WHERE ID IN (SELECT OID FROM dbo.WMS_Package WHERE PackageNumber='" + PackageNumber + "')");
            sb.Append("SELECT TOP 1 * FROM dbo.WMS_Package WHERE PackageNumber='" + PackageNumber + "'");
            sb.Append("SELECT * FROM dbo.WMS_PackageDetail WHERE PackageNumber ='" + PackageNumber + "'");
            sb.Append("SELECT TOP 1 * FROM dbo.WMS_Warehouse WHERE WarehouseName =(SELECT TOP 1 Warehouse FROM dbo.WMS_Package WHERE PackageNumber='" + PackageNumber + "')");

            DataSet ds = base.ExecuteDataSetBySqlString(sb.ToString());
            response.orderInfo = ds.Tables[0].ConvertToEntity<Entity.OrderInfo>();
            response.packageInfos = ds.Tables[1].ConvertToEntityCollection<Entity.PackageInfo>().ToList();
            response.packageDetailInfos = ds.Tables[2].ConvertToEntityCollection<Entity.PackageDetailInfo>().ToList();
            response.warehouseInfo = ds.Tables[3].ConvertToEntity<Entity.WarehouseInfo>();
            return response;
        }

        public string AddExpressAndUpdatePackageYD(YdResponseParam responseParam, Entity.PackageInfo package, PdfInfoObj obj)
        {
            try
            {
                string sql = string.Format(@"INSERT INTO dbo.WMS_ExpressDelivery
                                                (CustomerID,CustomerName,WarehouseID,WarehouseName,OID,OrderNumber,ExternOrderNumber,
                                                ExpressNumber,ExpressCompany,PackageNumber,
                                                Status,success,code,orderSerialNo,mailNo,pdfInfo,msg,
                                                position,position_no,four_code,package_wdjc,cus_area1,
                                                Creator,CreateTime)
                                                SELECT  CustomerID,CustomerName,
                                                (SELECT TOP 1 w.ID FROM dbo.WMS_Warehouse w WHERE w.WarehouseName =p.Warehouse) WarehouseID,Warehouse,OID,OrderNumber,ExternOrderNumber,
                                                {2},'韵达',PackageNumber,
                                                0,'true','200','{1}','{2}','{3}','{4}',
                                                '{5}','{6}','{7}','{8}','{9}',
                                                '韵达',GETDATE() FROM dbo.WMS_Package p WHERE p.PackageNumber='{0}';

                                                --更新包装快递信息
                                                UPDATE dbo.WMS_Package SET ExpressCompany='韵达',ExpressNumber='{2}' WHERE PackageNumber='{0}';

                                                 --更新订单主表快递
                                                UPDATE o SET o.ExpressCompany=p.ExpressCompany,o.ExpressNumber=p.ExpressNumber
                                                FROM dbo.WMS_Order o,
                                                (SELECT TOP 1 * FROM dbo.WMS_Package WHERE OrderNumber =(SELECT TOP 1 OrderNumber FROM dbo.WMS_Package WHERE PackageNumber='{0}')) p
                                                WHERE o.ID=p.OID;

                                                SELECT TOP 1000 * FROM dbo.WMS_ExpressDelivery WHERE ID=@@IDENTITY;",
                                            package.PackageNumber, responseParam.order_serial_no, responseParam.mail_no, responseParam.pdf_info, responseParam.msg,
                                            obj.position, obj.position_no, obj.four_code, obj.package_wdjc, obj.cus_area1);
                base.ExecuteNoQueryBySqlString(sql);
                return "";
            }
            catch (Exception ex)
            {
                return "新增失败";
            }
        }
    }
}
