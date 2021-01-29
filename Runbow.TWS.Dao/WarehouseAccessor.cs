using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Runbow.TWS.Common;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.DataBaseEntity;
using Runbow.TWS.Entity.WMS.Warehouse;
using Runbow.TWS.MessageContracts;

namespace Runbow.TWS.Dao
{
    public class WarehouseAccessor : BaseAccessor
    {
        public IEnumerable<WarehouseInfo> GetWarehouseByCondition(WarehouseSearchCondition SearchCondition, int pageIndex, int pageSize, out int rowCount)
        {
            string sqlWhere = this.GenGetWarehouseWhere(SearchCondition);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, pageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, pageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };

            DataTable dt = this.ExecuteDataTable("Proc_WMS_GetWarehouseByCondition", dbParams);
            rowCount = (int)dbParams[3].Value;
            return dt.ConvertToEntityCollection<WarehouseInfo>();
        }

        public string SaveQRCode(GetQRCodeByConditonRequest request, long? ProjectID, long? WareHouseID, long? Length, long? Width, string Creator, int Flag)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_AddQRCode", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@QRCode", request.QRCode.Select(a => new WMSQRCodeToDb(a)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@ProjectID", ProjectID);
                    cmd.Parameters[1].SqlDbType = SqlDbType.BigInt;
                    cmd.Parameters.AddWithValue("@WareHouseID", WareHouseID);
                    cmd.Parameters[2].SqlDbType = SqlDbType.BigInt;
                    cmd.Parameters.AddWithValue("@Length", Length);
                    cmd.Parameters[3].SqlDbType = SqlDbType.BigInt;
                    cmd.Parameters.AddWithValue("@Width", Width);
                    cmd.Parameters[4].SqlDbType = SqlDbType.BigInt;
                    cmd.Parameters.AddWithValue("@Creator", Creator);
                    cmd.Parameters[5].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[5].Size = 50;
                    cmd.Parameters.AddWithValue("@Flag", Flag);
                    cmd.Parameters[6].SqlDbType = SqlDbType.Int;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[7].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[7].Direction = ParameterDirection.Output;
                    cmd.Parameters[7].Size = 500;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                    conn.Close();
                    return message;
                }
                catch (Exception ex)
                {
                    //return message + "(" + ex.Message + ")";
                    throw ex;
                }
            }
        }

        public string EditQRCode(GetQRCodeByConditonRequest request, long? ProjectID, long? WareHouseID, int Flag)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_EditQRCode", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@QRCode", request.QRCode.Select(a => new WMSQRCodeToDb(a)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@ProjectID", ProjectID);
                    cmd.Parameters[1].SqlDbType = SqlDbType.BigInt;
                    cmd.Parameters.AddWithValue("@WareHouseID", WareHouseID);
                    cmd.Parameters[2].SqlDbType = SqlDbType.BigInt;
                    cmd.Parameters.AddWithValue("@Flag", Flag);
                    cmd.Parameters[3].SqlDbType = SqlDbType.Int;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[4].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[4].Direction = ParameterDirection.Output;
                    cmd.Parameters[4].Size = 500;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                    conn.Close();
                    return message;
                }
                catch (Exception ex)
                {
                    //return message + "(" + ex.Message + ")";
                    throw ex;
                }
            }
        }

        public string ImportGoodsShelf(GetGoodsShelfByConditonRequest request, int ViewType)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_ImportGoodsShelf", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@GoodsShelf", request.GoodsShelf.Select(a => new WMSGoodsShelfToDb(a)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@GoodsShelfRowAndCell", request.GoodsShelfRowAndCell.Select(a => new WMSGoodsShelfRowAndCellToDb(a)));
                    cmd.Parameters[1].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@ViewType", ViewType);
                    cmd.Parameters[2].SqlDbType = SqlDbType.Int;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[3].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[3].Direction = ParameterDirection.Output;
                    cmd.Parameters[3].Size = 500;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                    conn.Close();
                    return message;
                }
                catch (Exception ex)
                {
                    //return message + "(" + ex.Message + ")";
                    throw ex;
                }
            }
        }

        public string ImportGoodsShelfLocation(GetGoodsShelfByConditonRequest request, int ViewType)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_ImportGoodsShelfLocation", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@GoodsShelf", request.GoodsShelf.Select(a => new WMSGoodsShelfToDb(a)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@ViewType", ViewType);
                    cmd.Parameters[1].SqlDbType = SqlDbType.Int;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[2].Direction = ParameterDirection.Output;
                    cmd.Parameters[2].Size = 500;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                    conn.Close();
                    return message;
                }
                catch (Exception ex)
                {
                    //return message + "(" + ex.Message + ")";
                    throw ex;
                }
            }
        }

        public string SaveGoodsShelf(GetGoodsShelfByConditonRequest request, int ViewType)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_AddGoodsShelf", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@GoodsShelf", request.GoodsShelf.Select(a => new WMSGoodsShelfToDb(a)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@GoodsShelfRowAndCell", request.GoodsShelfRowAndCell.Select(a => new WMSGoodsShelfRowAndCellToDb(a)));
                    cmd.Parameters[1].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@ViewType", ViewType);
                    cmd.Parameters[2].SqlDbType = SqlDbType.Int;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[3].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[3].Direction = ParameterDirection.Output;
                    cmd.Parameters[3].Size = 500;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                    conn.Close();
                    return message;
                }
                catch (Exception ex)
                {
                    //return message + "(" + ex.Message + ")";
                    throw ex;
                }
            }
        }

        public string SaveGoodsShelfWithLocation(GetGoodsShelfByConditonRequest request, int ViewType)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_AddGoodsShelfWithLocation", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@GoodsShelf", request.GoodsShelf.Select(a => new WMSGoodsShelfToDb(a)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@GoodsShelfRowAndCell", request.GoodsShelfRowAndCell.Select(a => new WMSGoodsShelfRowAndCellToDb(a)));
                    cmd.Parameters[1].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@GoodsShelfForLocation", request.GoodsShelfForLocation.Select(a => new WMSGoodsShelfToDb(a)));
                    cmd.Parameters[2].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@ViewType", ViewType);
                    cmd.Parameters[3].SqlDbType = SqlDbType.Int;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[4].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[4].Direction = ParameterDirection.Output;
                    cmd.Parameters[4].Size = 500;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                    conn.Close();
                    return message;
                }
                catch (Exception ex)
                {
                    //return message + "(" + ex.Message + ")";
                    throw ex;
                }
            }
        }

        public string ProductWarningAdd(string IDS, string WarehouseID, string CustomerID, string WarehouseName, string MinNumber, string MaxNumber)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_AddProductWarning", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IDS", IDS);
                    cmd.Parameters[0].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[0].Size = 2000;
                    cmd.Parameters.AddWithValue("@WarehouseID", WarehouseID);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
                    cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@WarehouseName", WarehouseName);
                    cmd.Parameters[3].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[3].Size = 50;
                    cmd.Parameters.AddWithValue("@MinNumber", MinNumber);
                    cmd.Parameters[4].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@MaxNumber", MaxNumber);
                    cmd.Parameters[5].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[6].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[6].Direction = ParameterDirection.Output;
                    cmd.Parameters[6].Size = 500;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                    conn.Close();
                    return message;
                }
                catch (Exception ex)
                {
                    //return message + "(" + ex.Message + ")";
                    throw ex;
                }
            }
        }

        public string ProductWarningDelete(string IDS)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_DeleteProductWarning", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IDS", IDS);
                    cmd.Parameters[0].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[0].Size = 2000;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[1].Direction = ParameterDirection.Output;
                    cmd.Parameters[1].Size = 500;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                    conn.Close();
                    return message;
                }
                catch (Exception ex)
                {
                    //return message + "(" + ex.Message + ")";
                    throw ex;
                }
            }
        }

        public string ProductWarningEdit(string ID, string MinNumber, string MaxNumber)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_EditProductWarning", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.Parameters[0].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters.AddWithValue("@MinNumber", MinNumber);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@MaxNumber", MaxNumber);
                    cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[3].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[3].Direction = ParameterDirection.Output;
                    cmd.Parameters[3].Size = 500;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                    conn.Close();
                    return message;
                }
                catch (Exception ex)
                {
                    //return message + "(" + ex.Message + ")";
                    throw ex;
                }
            }
        }

        public string DeleteGoodsShelf(long ID)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_DeleteGoodsShelf", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.Parameters[0].SqlDbType = SqlDbType.BigInt;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[1].Direction = ParameterDirection.Output;
                    cmd.Parameters[1].Size = 500;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                    conn.Close();
                    return message;
                }
                catch (Exception ex)
                {
                    //return message + "(" + ex.Message + ")";
                    throw ex;
                }
            }
        }

        public IEnumerable<GoodsShelfInfo> GetGoodsShelfByCondition(GoodsShelfSearchCondition SearchCondition, int pageIndex, int pageSize, out int rowCount)
        {
            string sqlWhere = this.GenGetGoodsShelfWhere(SearchCondition);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, pageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, pageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };

            DataTable dt = this.ExecuteDataTable("Proc_WMS_GetGoodsShelfByCondition", dbParams);
            rowCount = (int)dbParams[3].Value;
            return dt.ConvertToEntityCollection<GoodsShelfInfo>();
        }

        public IEnumerable<ProductWarningInfo> GetProductWarningByCondition(ProductWarningSearchCondition SearchCondition, int pageIndex, int pageSize, out int rowCount)
        {
            string sqlWhere = this.GenGetProductWarningWhere(SearchCondition);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, pageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, pageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };

            DataTable dt = this.ExecuteDataTable("Proc_WMS_GetProductWarningByCondition", dbParams);
            rowCount = (int)dbParams[3].Value;
            return dt.ConvertToEntityCollection<ProductWarningInfo>();
        }

        public IEnumerable<ProductStorerInfo> GetNoWarningSKUByCondition(ProductWarningSearchCondition SearchCondition, int pageIndex, int pageSize, out int rowCount)
        {
            string sqlWhere = this.GenGetNoWarningSKUWhere(SearchCondition);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, pageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, pageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
            DataTable dt = this.ExecuteDataTable("Proc_WMS_GetNoWarningSKUByCondition", dbParams);
            rowCount = (int)dbParams[3].Value;
            return dt.ConvertToEntityCollection<ProductStorerInfo>();
        }

        public GetGoodsShelfByConditionResponse GetGoodsShelfByID(long? ID)
        {
            GetGoodsShelfByConditionResponse response = new GetGoodsShelfByConditionResponse();
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID", DbType.Int64, ID, ParameterDirection.Input)

            };
            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetGoodsShelfByID", dbParams);
            response.GoodsShelf = ds.Tables[0].ConvertToEntity<GoodsShelfSearchCondition>();
            response.GoodsShelfCollection = ds.Tables[1].ConvertToEntityCollection<GoodsShelfSearchCondition>();
            response.GoodsShelfRowAndCellCollection = ds.Tables[2].ConvertToEntityCollection<GoodsShelfSearchCondition>();
            return response;
        }

        public ProductWarningSearchCondition GetProductWarningByID(long? ID)
        {

            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID", DbType.Int64, ID, ParameterDirection.Input)

            };
            DataTable dt = this.ExecuteDataTable("Proc_WMS_GetProductWarningByID", dbParams);
            return dt.ConvertToEntity<ProductWarningSearchCondition>();
        }

        public GetQRCodeByConditionResponse GetQRCodeByCondition(QRCodeSearchCondition SearchCondition)
        {
            GetQRCodeByConditionResponse response = new GetQRCodeByConditionResponse();
            string sqlWhere = this.GenGetQRCodeWhere(SearchCondition);
            long ProjectID = SearchCondition.ProjectID;
            long WarehouseID = SearchCondition.WarehouseID;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@ProjectID", DbType.Int64, ProjectID, ParameterDirection.Input),
                new DbParam("@WarehouseID", DbType.Int64, WarehouseID, ParameterDirection.Input)

            };
            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetQRCodeByCondition", dbParams);
            response.QRCodeCollection = ds.Tables[0].ConvertToEntityCollection<QRCodeInfo>();
            response.OperationCollection = ds.Tables[1].ConvertToEntityCollection<QRCodeInfo>();
            response.ChargeCollection = ds.Tables[2].ConvertToEntityCollection<QRCodeInfo>();
            return response;
        }

        public IEnumerable<LocationInfo> GetWLocationByCondition(WLocationSearchCondition SearchCondition, int pageIndex, int pageSize, out int rowCount)
        {
            string sqlWhere = this.GenGetWLocationWhere(SearchCondition);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, pageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, pageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };

            DataTable dt = this.ExecuteDataTable("Proc_WMS_GetWLocationByCondition", dbParams);
            rowCount = (int)dbParams[3].Value;
            return dt.ConvertToEntityCollection<LocationInfo>();
        }

        public IEnumerable<WarehouseInfo> GetWarehouseByConditionNoPaging(WarehouseSearchCondition SearchCondition)
        {
            string sqlWhere = this.GenGetWarehouseWhere(SearchCondition);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input)
            };

            return this.ExecuteDataTable("Proc_GetWarehouseByConditionNoPaging", dbParams).ConvertToEntityCollection<WarehouseInfo>();
        }

        public IEnumerable<LocationInfo> GetWLocationByConditionNoPaging(WLocationSearchCondition SearchCondition)
        {
            string sqlWhere = this.GenGetWLocationWhere(SearchCondition);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input)
            };

            return this.ExecuteDataTable("Proc_GetWarehouseByConditionNoPaging", dbParams).ConvertToEntityCollection<LocationInfo>();
        }

        public IEnumerable<WarehouseInfo> GetWarehouse()
        {
            return this.ExecuteDataTable("Proc_WMS_GetWarehouse").ConvertToEntityCollection<WarehouseInfo>();
        }

        private string GenGetWarehouseWhere(WarehouseSearchCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(SearchCondition.Description))
            {
                sb.Append(" AND a.Description like '%").Append(SearchCondition.Description).Append("%' ");
            }

            if (!string.IsNullOrEmpty(SearchCondition.WarehouseStatus))
            {
                sb.Append(" AND a.WarehouseStatus='").Append(SearchCondition.WarehouseStatus).Append("' ");
            }

            if (!string.IsNullOrEmpty(SearchCondition.WarehouseType))
            {
                sb.Append(" AND a.WarehouseType='").Append(SearchCondition.WarehouseType).Append("' ");
            }

            if (!string.IsNullOrEmpty(SearchCondition.WarehouseName))
            {
                sb.Append(" AND a.WarehouseName='").Append(SearchCondition.WarehouseName).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.Address))
            {
                sb.Append(" AND a.Address like '%").Append(SearchCondition.Address).Append("%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.ProvinceCity))
            {
                sb.Append(" AND a.ProvinceCity like '%").Append(SearchCondition.ProvinceCity).Append("%' ");
            }
            if (SearchCondition.ProjectID != 0)
            {
                sb.Append(" AND a.ProjectID=").Append(SearchCondition.ProjectID).Append(" ");
            }
            if (SearchCondition.UserID != 0)
            {
                sb.Append(" AND b.UserID=").Append(SearchCondition.UserID).Append(" ");
            }
            return sb.ToString();
        }

        private string GenGetGoodsShelfWhere(GoodsShelfSearchCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(SearchCondition.GoodsShelvesName))
            {
                sb.Append(" AND GoodsShelvesName like '%").Append(SearchCondition.GoodsShelvesName).Append("%' ");
            }
            if (SearchCondition.ID != 0)
            {
                sb.Append(" AND ID=").Append(SearchCondition.ID).Append(" ");
            }
            if (SearchCondition.CustomerID != 0)
            {
                sb.Append(" AND CustomerID=").Append(SearchCondition.CustomerID).Append(" ");
            }
            if (SearchCondition.WareHouseID != 0)
            {
                sb.Append(" AND WareHouseID=").Append(SearchCondition.WareHouseID).Append(" ");
            }
            return sb.ToString();
        }

        private string GenGetProductWarningWhere(ProductWarningSearchCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();

            if (SearchCondition.WarehouseID != 0)
            {
                sb.Append(" AND WarehouseID= ").Append(SearchCondition.WarehouseID).Append(" ");
            }
            else
            {
                sb.Append(" AND WarehouseID=0 ").Append(" ");
            }
            if (SearchCondition.CustomerID != 0)
            {
                sb.Append(" AND CustomerID= ").Append(SearchCondition.CustomerID).Append(" ");
            }
            else
            {
                sb.Append(" AND CustomerID=0 ").Append(" ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.ProductName))
            {
                sb.Append(" AND ProductName  like '%").Append(SearchCondition.ProductName).Append("%' ");
            }


            return sb.ToString();
        }

        private string GenGetNoWarningSKUWhere(ProductWarningSearchCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();

            if (SearchCondition.CustomerID != 0)
            {
                sb.Append(" AND StorerID= ").Append(SearchCondition.CustomerID).Append(" ");
            }
            else
            {
                sb.Append(" AND StorerID=0 ").Append(" ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.ProductName))
            {
                sb.Append(" AND SKU  like '%").Append(SearchCondition.ProductName).Append("%' ");
            }

            return sb.ToString();
        }

        private string GenGetQRCodeWhere(QRCodeSearchCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();

            if (SearchCondition.ProjectID != 0)
            {
                sb.Append(" AND h.ProjectID= ").Append(SearchCondition.ProjectID).Append(" ");
            }
            if (SearchCondition.WarehouseID != 0)
            {
                sb.Append(" AND h.WarehouseID= ").Append(SearchCondition.WarehouseID).Append(" ");
            }
            if (SearchCondition.WarehouseID == 0)
            {
                sb.Append(" AND h.WarehouseID= 0");
            }
            return sb.ToString();
        }

        private string GenGetWLocationWhere(WLocationSearchCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(SearchCondition.ABCClassification))
            {
                sb.Append(" AND a.ABCClassification='").Append(SearchCondition.ABCClassification).Append("' ");
            }

            if (!string.IsNullOrEmpty(SearchCondition.AreaID))
            {
                sb.Append(" AND a.AreaID='").Append(SearchCondition.AreaID).Append("' ");
            }

            if (!string.IsNullOrEmpty(SearchCondition.Category))
            {
                sb.Append(" AND a.Category='").Append(SearchCondition.Category).Append("' ");
            }

            if (!string.IsNullOrEmpty(SearchCondition.Classification))
            {
                sb.Append(" AND a.Classification='").Append(SearchCondition.Classification).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.Handling))
            {
                sb.Append(" AND a.Handling='").Append(SearchCondition.Handling).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.IsMultiLot))
            {
                sb.Append(" AND a.IsMultiLot='").Append(SearchCondition.IsMultiLot).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.IsMultiSKU))
            {
                sb.Append(" AND a.IsMultiSKU='").Append(SearchCondition.IsMultiSKU).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.Location))
            {
                sb.Append(" AND a.Location='").Append(SearchCondition.Location).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.LocationLevel))
            {
                sb.Append(" AND a.LocationLevel='").Append(SearchCondition.LocationLevel).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.LocationStatus))
            {
                //if (SearchCondition.LocationStatus == true)
                //{ sb.Append(" AND a.LocationStatus='").Append(1).Append("' "); }
                //else
                //{
                sb.Append(" AND a.LocationStatus='").Append(SearchCondition.LocationStatus).Append("' ");
                //}

            }
            if (!string.IsNullOrEmpty(SearchCondition.LocationType))
            {
                sb.Append(" AND a.LocationType='").Append(SearchCondition.LocationType).Append("' ");
            }
            //if (!string.IsNullOrEmpty(SearchCondition.WarehouseID))
            //{
            //    sb.Append(" AND a.WarehouseID='").Append(SearchCondition.WarehouseID).Append("' ");
            //}
            if (!string.IsNullOrEmpty(SearchCondition.WarehouseID))
            {
                IEnumerable<string> numbers = Enumerable.Empty<string>();
                if (SearchCondition.WarehouseID.IndexOf("\n") > 0)
                {
                    numbers = SearchCondition.WarehouseID.Split('\n').Select(s => { return s.Trim(); });
                }
                if (SearchCondition.WarehouseID.IndexOf(',') > 0)
                {
                    numbers = SearchCondition.WarehouseID.Split(',').Select(s => { return s.Trim(); });
                }

                if (numbers != null && numbers.Any())
                {
                    numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
                }

                if (numbers != null && numbers.Any())
                {
                    sb.Append(" and a.WarehouseID in ( ");
                    foreach (string s in numbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" AND a.WarehouseID='").Append(SearchCondition.WarehouseID.Trim()).Append("' ");
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 根据仓库ID获取仓库信息
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="warehouse"></param>
        /// <param name="areas"></param>
        /// <param name="locations"></param>
        /// <returns></returns>
        public string GetWarehouseByID(long? ID, out WarehouseInfo warehouse, out IEnumerable<AreaInfo> areas, out IEnumerable<LocationInfo> locations)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {
                    DbParam[] param = new DbParam[] {
                        new DbParam("@ID", DbType.Int64, ID, ParameterDirection.Input)
                    };
                    DataSet ds = ExecuteDataSet("Proc_WMS_GetWarehouseByID", param);
                    warehouse = ds.Tables[0].ConvertToEntity<WarehouseInfo>() ?? new WarehouseInfo(); //为空则赋值
                    areas = new List<AreaInfo>();// ds.Tables[1].ConvertToEntityCollection<AreaInfo>();
                    locations = new List<LocationInfo>();// ds.Tables[2].ConvertToEntityCollection<LocationInfo>();
                    return "操作成功";
                }
                catch (Exception ex)
                {
                    warehouse = new WarehouseInfo();
                    areas = new List<AreaInfo>();
                    locations = new List<LocationInfo>();
                    return "操作失败(" + ex.Message + ")";
                }
            }
        }

        /// <summary>
        /// 根据仓库ID获取仓库、库区信息
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="warehouse"></param>
        /// <param name="areas"></param>
        /// <param name="locations"></param>
        /// <returns></returns>
        public string GetWarehouseAndAreaByID(long? ID, out WarehouseInfo warehouse, out IEnumerable<AreaInfo> areas)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {

                    DbParam[] param = new DbParam[] {
                        new DbParam("@ID", DbType.Int64, ID, ParameterDirection.Input)
                    };
                    DataSet ds = ExecuteDataSet("Proc_WMS_GetWarehouseAndAreaByID", param);
                    warehouse = ds.Tables[0].ConvertToEntity<WarehouseInfo>() ?? new WarehouseInfo(); //为空则赋值
                    areas = ds.Tables[1].ConvertToEntityCollection<AreaInfo>();
                    return "操作成功";
                }
                catch (Exception ex)
                {
                    warehouse = new WarehouseInfo();
                    areas = new List<AreaInfo>();
                    return "操作失败(" + ex.Message + ")";
                }

            }


        }

        /// <summary>
        /// 新增或修改操作
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="warehouse"></param>
        /// <param name="areas"></param>
        /// <param name="locations"></param>
        /// <returns></returns>
        public string AddOrUpdateWarehouse(DataTable dt, out WarehouseInfo warehouse, out IEnumerable<AreaInfo> areas, out IEnumerable<LocationInfo> locations)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_WarehouseMerge", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@InsertData", dt);
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.Add("@message", SqlDbType.NVarChar, 50);
                    cmd.Parameters[1].Direction = ParameterDirection.Output;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                    conn.Close();
                    warehouse = ds.Tables[0].ConvertToEntity<WarehouseInfo>() ?? new WarehouseInfo();
                    areas = ds.Tables[1].ConvertToEntityCollection<AreaInfo>();
                    locations = ds.Tables[2].ConvertToEntityCollection<LocationInfo>();
                    return message;
                }
                catch (Exception ex)
                {
                    warehouse = new WarehouseInfo();
                    areas = new List<AreaInfo>();
                    locations = new List<LocationInfo>();
                    return message + "(" + ex.Message + ")";
                }
            }
        }

        /// <summary>
        /// 获取仓库下拉列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<WarehouseInfo> GetWarehouseList()
        {
            IEnumerable<WarehouseInfo> list = new List<WarehouseInfo>();   //库区列表
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {

                    DbParam[] param = new DbParam[] {

                    };
                    DataSet ds = ExecuteDataSet("Proc_WMS_GetWarehouseList", param);
                    list = ds.Tables[0].ConvertToEntityCollection<WarehouseInfo>();
                    return list;

                }
                catch (Exception ex)
                {
                    list = new List<WarehouseInfo>();
                    return list;
                }

            }
        }

        /// <summary>
        /// 获取仓库下拉列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<WarehouseInfo> GetWarehouseListByCustomerID(long CustomerID)
        {
            IEnumerable<WarehouseInfo> list = new List<WarehouseInfo>();   //库区列表
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {

                    DbParam[] param = new DbParam[] {
                         new DbParam("@CustomerID", DbType.Int64, CustomerID, ParameterDirection.Input)
                    };
                    DataSet ds = ExecuteDataSet("Proc_WMS_GetWarehouseListByCustomerID", param);
                    list = ds.Tables[0].ConvertToEntityCollection<WarehouseInfo>();
                    return list;

                }
                catch (Exception ex)
                {
                    list = new List<WarehouseInfo>();
                    return list;
                }

            }
        }

        /// <summary>
        /// 获取库区下拉列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AreaInfo> GetWarehouseAreaList(long WarehouseID)
        {
            IEnumerable<AreaInfo> list = new List<AreaInfo>();   //库区列表
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {
                    DbParam[] param = new DbParam[] {
                         new DbParam("@WarehouseID", DbType.Int64, WarehouseID, ParameterDirection.Input)
                    };
                    DataSet ds = ExecuteDataSet("Proc_WMS_GetWarehouseAreaList", param);
                    list = ds.Tables[0].ConvertToEntityCollection<AreaInfo>();
                    return list;
                }
                catch (Exception ex)
                {
                    list = new List<AreaInfo>();
                    return list;
                }
            }
        }

        /// <summary>
        /// 获取库区下拉列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AreaInfo> GetWarehouseAreaListByWarehouseName(string WarehouseName)
        {
            IEnumerable<AreaInfo> list = new List<AreaInfo>();   //库区列表
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {

                    DbParam[] param = new DbParam[] {
                         new DbParam("@WarehouseName", DbType.String, WarehouseName, ParameterDirection.Input)
                    };
                    DataSet ds = ExecuteDataSet("Proc_WMS_GetWarehouseAreaListByWarehouseName", param);
                    list = ds.Tables[0].ConvertToEntityCollection<AreaInfo>();
                    return list;

                }
                catch (Exception ex)
                {
                    list = new List<AreaInfo>();
                    return list;
                }

            }
        }

        public IEnumerable<GoodsShelfInfo> GetGoodsShelfList(long project, long CustomerID, long WarehouseID)
        {
            IEnumerable<GoodsShelfInfo> list = new List<GoodsShelfInfo>();   //库区列表
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {
                    DbParam[] param = new DbParam[] {
                         new DbParam("@project", DbType.Int64, project, ParameterDirection.Input),
                          new DbParam("@CustomerID", DbType.Int64, CustomerID, ParameterDirection.Input),
                           new DbParam("@WarehouseID", DbType.Int64, WarehouseID, ParameterDirection.Input)
                    };
                    DataSet ds = ExecuteDataSet("Proc_WMS_GetGoodsShelfList", param);
                    list = ds.Tables[0].ConvertToEntityCollection<GoodsShelfInfo>();
                    return list;
                }
                catch (Exception ex)
                {
                    list = new List<GoodsShelfInfo>();
                    return list;
                }
            }
        }

        /// <summary>
        /// 获取库位下拉列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LocationInfo> GetWarehouseLocationList(long WarehouseID)
        {
            IEnumerable<LocationInfo> list = new List<LocationInfo>();   //库区列表
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {

                    DbParam[] param = new DbParam[] {
                         new DbParam("@WarehouseID", DbType.Int64, WarehouseID, ParameterDirection.Input)
                    };
                    DataSet ds = ExecuteDataSet("Proc_WMS_GetWarehouseLocationList", param);
                    list = ds.Tables[0].ConvertToEntityCollection<LocationInfo>();
                    return list;

                }
                catch (Exception ex)
                {
                    list = new List<LocationInfo>();
                    return list;
                }

            }
        }
        /// <summary>
        /// 获取库位下拉列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LocationInfo> GetWarehouseLocationListByWCID(long WarehouseID, long CustomerID)
        {
            IEnumerable<LocationInfo> list = new List<LocationInfo>();   //库区列表
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {

                    DbParam[] param = new DbParam[] {
                         new DbParam("@WarehouseID", DbType.Int64, WarehouseID, ParameterDirection.Input),
                         new DbParam("@CustomerID", DbType.Int64, CustomerID, ParameterDirection.Input)
                    };
                    DataSet ds = ExecuteDataSet("Proc_WMS_GetWarehouseLocationListByWCID", param);
                    list = ds.Tables[0].ConvertToEntityCollection<LocationInfo>();
                    return list;

                }
                catch (Exception ex)
                {
                    list = new List<LocationInfo>();
                    return list;
                }

            }
        }
        /// <summary>
        /// 获取库位下拉列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LocationInfo> GetWarehouseLocationListByLocation(string WarehouseID, string Location)
        {
            IEnumerable<LocationInfo> list = new List<LocationInfo>();   //库区列表
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {
                    DbParam[] param = new DbParam[] {
                         new DbParam("@WarehouseID", DbType.Int64, WarehouseID, ParameterDirection.Input),
                         new DbParam("@Location", DbType.String, Location, ParameterDirection.Input)
                    };
                    DataSet ds = ExecuteDataSet("Proc_WMS_GetWarehouseLocationListByLocation", param);
                    list = ds.Tables[0].ConvertToEntityCollection<LocationInfo>();
                    return list;
                }
                catch (Exception ex)
                {
                    list = new List<LocationInfo>();
                    return list;
                }
            }
        }

        /// <summary>
        /// 获取库位下拉列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LocationInfo> GetWarehouseLocationListByWarehouseName(string WarehouseName)
        {
            IEnumerable<LocationInfo> list = new List<LocationInfo>();   //库区列表
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {

                    DbParam[] param = new DbParam[] {
                         new DbParam("@WarehouseName", DbType.String, WarehouseName, ParameterDirection.Input)
                    };
                    DataSet ds = ExecuteDataSet("Proc_WMS_GetWarehouseLocationListByWarehouseName", param);
                    list = ds.Tables[0].ConvertToEntityCollection<LocationInfo>();
                    return list;

                }
                catch (Exception ex)
                {
                    list = new List<LocationInfo>();
                    return list;
                }

            }
        }

        public IEnumerable<LocationInfo> GetLocationGoodShelfList(long WarehouseID)
        {
            IEnumerable<LocationInfo> list = new List<LocationInfo>();   //库区列表
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {

                    DbParam[] param = new DbParam[] {
                         new DbParam("@WarehouseID", DbType.Int64, WarehouseID, ParameterDirection.Input)
                    };
                    DataSet ds = ExecuteDataSet("Proc_WMS_GetLocation_GoodShelf", param);
                    list = ds.Tables[0].ConvertToEntityCollection<LocationInfo>();
                    return list;

                }
                catch (Exception ex)
                {
                    list = new List<LocationInfo>();
                    return list;
                }

            }
        }
        /// <summary>
        /// 库区新增、修改
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="area"></param>
        /// <param name="loactions"></param>
        /// <returns></returns>
        public string AddOrUpdateWarehouseArea(DataTable dt, out WarehouseInfo warehouse, out AreaInfo area, out IEnumerable<LocationInfo> locations)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_WarehouseAreaMerge", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@InsertData", dt);
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[1].Size = 10;
                    cmd.Parameters[1].Direction = ParameterDirection.Output;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                    conn.Close();
                    warehouse = ds.Tables[0].ConvertToEntity<WarehouseInfo>();
                    area = ds.Tables[1].ConvertToEntity<AreaInfo>();
                    locations = ds.Tables[2].ConvertToEntityCollection<LocationInfo>();
                    return message;
                }
                catch (Exception ex)
                {
                    warehouse = new WarehouseInfo();
                    area = new AreaInfo();
                    locations = new List<LocationInfo>();
                    return message + "(" + ex.Message + ")";
                }
            }
        }

        /// <summary>
        /// 根据ID获取库区，以及库区下的所有库位
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="area"></param>
        /// <param name="locations"></param>
        /// <returns></returns>
        public string GetWarehouseAreaByID(long ID, out WarehouseInfo warehouse, out AreaInfo area, out IEnumerable<LocationInfo> locations)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {
                    DbParam[] param = new DbParam[] {
                        new DbParam("@ID", DbType.Int64, ID, ParameterDirection.Input),
                    };
                    DataSet ds = ExecuteDataSet("Proc_WMS_GetWarehouseAreaByID", param);

                    warehouse = ds.Tables[0].Rows.Count > 0 ? ds.Tables[0].ConvertToEntity<WarehouseInfo>() : new WarehouseInfo();
                    area = ds.Tables[1].Rows.Count > 0 ? ds.Tables[1].ConvertToEntity<AreaInfo>() : new AreaInfo();
                    locations = new List<LocationInfo>();// ds.Tables[2].Rows.Count > 0 ? ds.Tables[2].ConvertToEntityCollection<LocationInfo>() : new List<LocationInfo>();
                    return "操作成功";
                }
                catch (Exception ex)
                {
                    warehouse = new WarehouseInfo();
                    area = new AreaInfo();
                    locations = new List<LocationInfo>();
                    return "操作失败(" + ex.Message + ")";
                }

            }
        }

        /// <summary>
        /// 库位新增、修改
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public string AddOrUpdateWarehouseLocation(IEnumerable<LocationInfo> Locations, out WarehouseInfo warehouse, out AreaInfo area, out LocationInfo location, out GoodsShelfInfo goodsShelf)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_WarehouseLocationMerge", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@InsertData", Locations.Select(p => new LocationInfoToDb(p)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[1].Size = 10;
                    cmd.Parameters[1].Direction = ParameterDirection.Output;
                    cmd.CommandTimeout = 300;

                    conn.Open();
                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                    conn.Close();

                    if (message == "操作成功")
                    {
                        warehouse = ds.Tables[0].ConvertToEntity<WarehouseInfo>();
                        area = ds.Tables[1].ConvertToEntity<AreaInfo>();
                        location = ds.Tables[2].ConvertToEntity<LocationInfo>();
                        goodsShelf = ds.Tables[3].ConvertToEntity<GoodsShelfInfo>();
                    }
                    else
                    {
                        warehouse = new WarehouseInfo();  //仓库
                        area = new AreaInfo();            //库区
                        location = new LocationInfo();   //库位
                        goodsShelf = new GoodsShelfInfo();
                    }


                    return message;
                }
                catch (Exception ex)
                {
                    warehouse = new WarehouseInfo();  //仓库
                    area = new AreaInfo();            //库区
                    location = new LocationInfo();    //库位
                    goodsShelf = new GoodsShelfInfo();
                    return message + "(" + ex.Message + ")";
                }
            }
        }

        public string AddLocation(IEnumerable<LocationInfo> Locations, out LocationInfo location)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_WarehouseLocationMerge", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@InsertData", Locations.Select(p => new LocationInfoToDb(p)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[1].DbType = DbType.String;
                    cmd.Parameters[1].Direction = ParameterDirection.Output;
                    cmd.Parameters[1].Size = 20;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                    conn.Close();
                    location = ds.Tables[0].ConvertToEntity<LocationInfo>();
                    return message;
                }
                catch (Exception ex)
                {
                    location = new LocationInfo();
                    return message + "(" + ex.Message + ")";
                }
            }
        }

        public string ImportLocation(IEnumerable<LocationInfo> Locations)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_ImportLocation", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@InsertData", Locations.Select(p => new LocationInfoToDb(p)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[1].DbType = DbType.String;
                    cmd.Parameters[1].Direction = ParameterDirection.Output;
                    cmd.Parameters[1].Size = 20;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                    conn.Close();
                    return message;
                }
                catch (Exception ex)
                {

                    return message + "(" + ex.Message + ")";
                }
            }
        }

        public string ImportLocationAndGoodShelf(IEnumerable<LocationInfo> Locations)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_ImportLocationAndGoodShelf", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@InsertData", Locations.Select(p => new LocationInfoToDb(p)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[1].DbType = DbType.String;
                    cmd.Parameters[1].Direction = ParameterDirection.Output;
                    cmd.Parameters[1].Size = 20;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                    conn.Close();
                    return message;
                }
                catch (Exception ex)
                {

                    return message + "(" + ex.Message + ")";
                }
            }
        }

        public string DeleteLocation(string ID)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_DeleteLocationByID", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.Parameters[0].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[0].Size = 50;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    cmd.ExecuteNonQuery();
                    conn.Close();
                    return "";
                }
                catch (Exception ex)
                {
                    return message + "(" + ex.Message + ")";
                }
            }

        }

        public string WarehouseDelete(string ID)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_DeleteWarehouseByID", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.Parameters[0].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[0].Size = 50;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    cmd.ExecuteNonQuery();
                    conn.Close();
                    return "";
                }
                catch (Exception ex)
                {
                    return message + "(" + ex.Message + ")";
                }
            }

        }

        public string DeleteArea(string ID)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_DeleteAreaByID", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.Parameters[0].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[0].Size = 50;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    cmd.ExecuteNonQuery();
                    conn.Close();
                    return "";
                }
                catch (Exception ex)
                {
                    return message + "(" + ex.Message + ")";
                }
            }

        }

        public string DeleteMap(long WarehouseID)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_DeleteMapByWarehouseID", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@WarehouseID", WarehouseID);
                    cmd.Parameters[0].SqlDbType = SqlDbType.BigInt;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    cmd.ExecuteNonQuery();
                    conn.Close();
                    return "";
                }
                catch (Exception ex)
                {
                    return message + "(" + ex.Message + ")";
                }
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetWarehouseLocationByID(long ID, out WarehouseInfo warehouse, out AreaInfo area, out LocationInfo location, out GoodsShelfInfo goodsShelf)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {

                    DbParam[] param = new DbParam[] {
                        new DbParam("@ID", DbType.Int64, ID, ParameterDirection.Input)
                    };
                    DataSet ds = ExecuteDataSet("Proc_WMS_GetWarehouseLocationByID", param);
                    warehouse = ds.Tables[0].Rows.Count > 0 ? ds.Tables[0].ConvertToEntity<WarehouseInfo>() : new WarehouseInfo();
                    area = ds.Tables[1].Rows.Count > 0 ? ds.Tables[1].ConvertToEntity<AreaInfo>() : new AreaInfo();
                    location = ds.Tables[2].Rows.Count > 0 ? ds.Tables[2].ConvertToEntity<LocationInfo>() : new LocationInfo();
                    goodsShelf = ds.Tables[3].Rows.Count > 0 ? ds.Tables[3].ConvertToEntity<GoodsShelfInfo>() : new GoodsShelfInfo();
                    return "操作成功";
                }
                catch (Exception ex)
                {
                    ///离开前赋值
                    warehouse = new WarehouseInfo();
                    area = new AreaInfo();
                    location = new LocationInfo();
                    goodsShelf = new GoodsShelfInfo();
                    return "操作失败(" + ex.Message + ")";
                }

            }
        }
        /// <summary>
        /// 添加数据 仓库客户mapping表  add by hujiaoqiang  20151027
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="WarehouseID"></param>
        /// <param name="Creator"></param>
        /// <returns></returns>
        public bool SetUserWarehouseAllocate(long customerID, long WarehouseID, string Creator)
        {
            DbParam[] parms = {
                      new DbParam("@CustomerID", DbType.Int64, customerID, ParameterDirection.Input),
                      new DbParam("@WarehouseID", DbType.Int64,WarehouseID, ParameterDirection.Input),
                      new DbParam("@Creator", DbType.String, Creator, ParameterDirection.Input)
                             };
            base.ExecuteNoQuery("Proc_SetCustomerWarehouseAllocate", parms);
            return true;
        }
        /// <summary>
        /// 添加数据 仓库用户mapping表
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="WarehouseID"></param>
        /// <param name="Creator"></param>
        /// <returns></returns>
        public bool SetUserWarehouseAllocates(long userID, long WarehouseID, string Creator)
        {
            DbParam[] parms = {
                      new DbParam("@UserID", DbType.Int64, userID, ParameterDirection.Input),
                      new DbParam("@WarehouseID", DbType.Int64,WarehouseID, ParameterDirection.Input),
                      new DbParam("@Creator", DbType.String, Creator, ParameterDirection.Input)
                             };
            base.ExecuteNoQuery("[Proc_SetUserWarehouseAllocate]", parms);
            return true;
        }
        /// <summary>
        /// 查出当前客户已有的仓库权限  
        /// </summary>add by hujiaoqiang  20151027
        /// <param name="userID"></param>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public IEnumerable<WarehouseAllocate> GetWarehouseAllocate(long customerID)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@CustomerID", DbType.Int64, customerID, ParameterDirection.Input),
            };
            return this.ExecuteDataTable("Proc_WMS_GetWarehouseCustomerAllocate", dbParams).ConvertToEntityCollection<WarehouseAllocate>();
        }
        /// <summary>
        /// 查询当前用户已有仓库
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public IEnumerable<WarehouseAllocate> GetWarehouseAllocates(long useID)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@UserID", DbType.Int64, useID, ParameterDirection.Input),
            };
            return this.ExecuteDataTable("[Proc_WMS_GetWarehouseUserAllocate]", dbParams).ConvertToEntityCollection<WarehouseAllocate>();
        }
        //add by hujiaoqiang  20151027
        public IEnumerable<WarehouseAllocate> GetALLWarehouseAllocate()
        {
            return this.ExecuteDataTable("Proc_WMS_GetAllWarehouseCustomerAllocate").ConvertToEntityCollection<WarehouseAllocate>();
        }
        //2016-3-9 15:27:03  hzf 获取库存盘点盘点明细
        public IEnumerable<WarehouseCheck> GetWarehouseCheck(WarehouseCheckSearchCondition wc, out int rowCount)
        {
            #region 查询条件
            StringBuilder sb = new StringBuilder(); //普通盘点
            StringBuilder sbbd = new StringBuilder(); //变动盘点
            int receipt = 0;
            int order = 0;
            int move = 0;
            int adjust = 0;

            if (!string.IsNullOrEmpty(wc.CustomerID.ToString().Trim()))
            {
                sb.Append(" AND a.CustomerID='").Append(wc.CustomerID.ToString().Trim()).Append("' ");
            }
            if (!string.IsNullOrEmpty(wc.Warehouse.Trim()))
            {
                sb.Append(" AND ww.WarehouseName='").Append(wc.Warehouse.ToString().Trim()).Append("' ");
            }
            if (!string.IsNullOrEmpty(wc.Area) && wc.Area != null)
            {
                if (int.Parse(wc.Area) != 0)
                    sb.Append(" AND area.ID='").Append(wc.Area.ToString().Trim()).Append("' ");
            }
            if (!string.IsNullOrEmpty(wc.Type.ToString()))
            {
                sb.Append(" AND Qty > 0");
                //全部盘点
                if (wc.Type.ToString() == "1")
                {

                }
                //库位盘点
                else if (wc.Type.ToString() == "2")
                {
                    sb.Append(" AND Location >='").Append(wc.str1.Trim()).Append("'");
                    sb.Append(" AND Location <='").Append(wc.str2.Trim()).Append("'");
                }
                //品名盘点
                else if (wc.Type.ToString() == "3")
                {
                    sb.Append(" AND SKU >='").Append(wc.str1.Trim()).Append("'");
                    sb.Append(" AND SKU <='").Append(wc.str2.Trim()).Append("'");
                }
                //小货量盘点
                else if (wc.Type.ToString() == "4")
                {
                    sb.Append(" AND QTY >='").Append(wc.str2.Trim()).Append("'");
                    sb.Append(" AND QTY <='").Append(wc.str1.Trim()).Append("'");
                }
                //变动库位盘点
                else if (wc.Type.ToString() == "5")
                {

                    //判断是否存在盘点类型
                    string[] s = wc.Type_description.ToString().Substring(6, wc.Type_description.Length - 6).Split(',');
                    if (s.Length > 0)
                    {
                        foreach (string ss in s)
                        {
                            if (ss == "1")
                            {
                                receipt = 1;
                            }
                            if (ss == "2")
                            {
                                order = 1;
                            }
                            if (ss == "3")
                            {
                                move = 1;
                            }
                            if (ss == "4")
                            {
                                adjust = 1;
                            }
                        }
                    }
                }
                //手工录入
                else if (wc.Type.ToString() == "6")
                {
                    sb.Append(" AND SKU IN (SELECT SKU FROM dbo.WMS_Order o LEFT JOIN dbo.WMS_OrderDetail d ON o.ID=d.OID WHERE o.Status=9 AND DateDiff(dd,CompleteDate,getdate())<=7)");
                }


            }

            #endregion
            string sqlWhere = sb.ToString();
            int tempRowCount = 0;

            DataTable dt = new DataTable();
            if (wc.Type == 5)
            {
                //差异盘点
                DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@Receipt", DbType.Int32,receipt, ParameterDirection.Input),
                new DbParam("@Order", DbType.Int32,order, ParameterDirection.Input),
                new DbParam("@Move", DbType.Int32, move, ParameterDirection.Input),
                new DbParam("@Adjust", DbType.Int32, adjust, ParameterDirection.Input),
                new DbParam("@BeginTime", DbType.String, wc.str1, ParameterDirection.Input),
                new DbParam("@EndTime", DbType.String, wc.str2, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
                dt = this.ExecuteDataTable("Proc_WMS_GetWarehouseCheckByCondition5", dbParams);
                rowCount = (int)dbParams[1].Value;

            }
            else
            {
                //条件盘点
                DbParam[] dbParams = new DbParam[]{
                new DbParam("@CustomerID", DbType.String, wc.CustomerID, ParameterDirection.Input),
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
                dt = this.ExecuteDataTable("Proc_WMS_GetWarehouseCheckByConditions", dbParams);
                rowCount = (int)dbParams[1].Value;
            }


            return dt.ConvertToEntityCollection<WarehouseCheck>();
        }

        public IEnumerable<WarehouseCheckDetail> GetWarehouseCheckNew(WarehouseCheckSearchCondition wc, out int rowCount)
        {
            #region 查询条件
            StringBuilder sb = new StringBuilder(); //普通盘点
            StringBuilder sbbd = new StringBuilder(); //变动盘点
            int receipt = 0;
            int order = 0;
            int move = 0;
            int adjust = 0;
            int hold = 0;

            if (!string.IsNullOrEmpty(wc.CustomerID.ToString()))
            {
                sb.Append(" AND a.CustomerID='").Append(wc.CustomerID.ToString()).Append("' ");
            }
            if (!string.IsNullOrEmpty(wc.Warehouse))
            {
                sb.Append(" AND a.Warehouse='").Append(wc.Warehouse).Append("' ");
            }
            if (!string.IsNullOrEmpty(wc.Area) && wc.Area != "null")
            {
                sb.Append(" AND Area='").Append(wc.Area).Append("' ");
            }
            if (!string.IsNullOrEmpty(wc.Type.ToString()))
            {
                //全部盘点
                if (wc.Type.ToString() == "1")
                { }
                //库位盘点
                else if (wc.Type.ToString() == "2")
                {
                    if (wc.str1.Trim().Split(',').Length > 1)
                    {
                        var skuList = wc.str1.Trim().Split(',');
                        StringBuilder searchSKU = new StringBuilder();
                        for (int i = 0; i < skuList.Length; i++)
                        {
                            if (!(skuList[i].ToString().Trim().Equals("") || skuList[i].ToString().Trim().Length == 0))
                            {
                                searchSKU.Append("'").Append(skuList[i].ToString().Trim()).Append("'").Append(",");
                            }
                        }
                        sb.Append(" AND Location in (").Append(searchSKU.ToString().Substring(0, searchSKU.Length - 1)).Append(")");
                    }
                    else
                    {
                        sb.Append(" AND Location >='").Append(wc.str1.Trim()).Append("'");
                        sb.Append(" AND Location <='").Append(wc.str2.Trim()).Append("'");
                    }
                }
                //品名盘点 门店及品名盘点
                else if (wc.Type.ToString() == "3"||  wc.Type.ToString() == "10")
                {
                    if (wc.str1.Trim().Split(',').Length > 1)
                    {
                        var skuList = wc.str1.Trim().Split(',');
                        StringBuilder searchSKU = new StringBuilder();
                        for (int i = 0; i < skuList.Length; i++)
                        {
                            if (!(skuList[i].ToString().Trim().Equals("") || skuList[i].ToString().Trim().Length == 0))
                            {
                                searchSKU.Append("'").Append(skuList[i].ToString().Trim()).Append("'").Append(",");
                            }
                        }
                        sb.Append(" AND SKU in (").Append(searchSKU.ToString().Substring(0, searchSKU.Length - 1)).Append(")");
                    }
                    else
                    {
                        sb.Append(" AND SKU >='").Append(wc.str1.Trim()).Append("'");
                        sb.Append(" AND SKU <='").Append(wc.str2.Trim()).Append("'");
                    }
                }
                //小货量盘点
                else if (wc.Type.ToString() == "4")
                {
                    sb.Append(" AND QTY >='").Append(wc.str2.Trim()).Append("'");
                    sb.Append(" AND QTY <='").Append(wc.str1.Trim()).Append("'");
                }
                //变动库位盘点
                else if (wc.Type.ToString() == "5")
                {
                    //判断是否存在盘点类型
                    string[] s = wc.str5.Split(',');//Type_description.ToString().Substring(6, wc.Type_description.Length - 6).Split(',');
                    if (s.Length > 0)
                    {
                        foreach (string ss in s)
                        {
                            if (ss == "1")
                            {
                                receipt = 1;
                            }
                            if (ss == "2")
                            {
                                order = 1;
                            }
                            if (ss == "3")
                            {
                                move = 1;
                            }
                            if (ss == "4")
                            {
                                adjust = 1;
                            }
                            if (ss == "5")
                            {
                                hold = 1;
                            }
                        }
                    }
                }
                //手工录入
                else if (wc.Type.ToString() == "6")
                {
                    sb.Append(" AND SKU IN (SELECT SKU FROM dbo.WMS_Order o LEFT JOIN dbo.WMS_OrderDetail d ON o.ID=d.OID WHERE o.Status=9 AND DateDiff(dd,CompleteDate,getdate())<=7)");
                }
                //else if (wc.Type.ToString() == "8")
                //{
                //    string[] s = wc.str5.Split(',');
                //    if (s.Length > 0)
                //    {
                //        foreach (string ss in s)
                //        {
                //            if (ss == "2")
                //            {
                //                order = 1;
                //            }
                //        }
                //    }
                //}
            }
            #endregion
            string sqlWhere = sb.ToString();
            int tempRowCount = 0;
            DataTable dt = new DataTable();
            if (wc.Type == 5)
            {
                //差异盘点
                DbParam[] dbParams = new DbParam[]{
                new DbParam("@CustomerID", DbType.String, wc.CustomerID, ParameterDirection.Input),
                new DbParam("@Warehouse", DbType.String, wc.Warehouse, ParameterDirection.Input),
                new DbParam("@Area", DbType.String, wc.Area, ParameterDirection.Input),
                new DbParam("@Receipt", DbType.Int32,receipt, ParameterDirection.Input),
                new DbParam("@Order", DbType.Int32,order, ParameterDirection.Input),
                new DbParam("@Move", DbType.Int32, move, ParameterDirection.Input),
                new DbParam("@Adjust", DbType.Int32, adjust, ParameterDirection.Input),
                new DbParam("@Hold", DbType.Int32, hold, ParameterDirection.Input),
                new DbParam("@BeginTime", DbType.String, wc.str1, ParameterDirection.Input),
                new DbParam("@EndTime", DbType.String, wc.str2, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
                    };
                dt = this.ExecuteDataTable("Proc_WMS_GetWarehouseCheckByCondition5", dbParams);
                rowCount = (int)dbParams[10].Value;
            }
            else if (wc.Type == 8)
            {
                //空库位盘点
                DbParam[] dbParams = new DbParam[]{
                new DbParam("@CustomerID", DbType.String, wc.CustomerID, ParameterDirection.Input),//客户Id
                new DbParam("@WarehouseID", DbType.String, wc.WareHouseID, ParameterDirection.Input),//仓库
                new DbParam("@AreaID", DbType.String, wc.AreaID, ParameterDirection.Input),//库区          
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
                    };
                dt = this.ExecuteDataTable("Proc_WMS_GetWarehouseCheckByCondition8", dbParams);
                rowCount = (int)dbParams[3].Value;
            }
            else
            {
                //条件盘点
                DbParam[] dbParams = new DbParam[]{
                new DbParam("@CustomerID", DbType.String, wc.CustomerID, ParameterDirection.Input),
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.String, wc.PageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.String, wc.PageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
                    };
                dt = this.ExecuteDataTable("Proc_WMS_GetWarehouseCheckByConditions03", dbParams);
                rowCount = (int)dbParams[4].Value;
            }
            return dt.ConvertToEntityCollection<WarehouseCheckDetail>();
        }
        //2016-3-9 15:27:03  hzf 保存库存盘点盘点明细
        public string WarehouseCheckSave(WarehouseCheckSearchCondition wc)
        {
            string Mess = string.Empty;
            int receipt = 0;
            int order = 0;
            int move = 0;
            int adjust = 0;
            int hold = 0;
            try
            {
                //保存盘点主子表
                string Roles = string.Empty;
                DataTable dt = new DataTable();
                #region 查询条件
                StringBuilder sb = new StringBuilder(); //普通盘点
                StringBuilder sbbd = new StringBuilder(); //变动盘点

                if (!string.IsNullOrEmpty(wc.CustomerID.ToString()))
                {
                    sb.Append(" AND a.CustomerID='").Append(wc.CustomerID.ToString()).Append("' ");
                }
                if (!string.IsNullOrEmpty(wc.Warehouse))
                {
                    sb.Append(" AND a.Warehouse='").Append(wc.Warehouse).Append("' ");
                }
                if (!string.IsNullOrEmpty(wc.Area) && wc.Area != "null")
                {
                    sb.Append(" AND Area='").Append(wc.Area).Append("' ");
                }
                if (!string.IsNullOrEmpty(wc.Type.ToString()))
                {
                    //全部盘点
                    if (wc.Type.ToString() == "1")
                    { }
                    //库位盘点
                    else if (wc.Type.ToString() == "2")
                    {
                        if (wc.str1.Trim().Split(',').Length > 1)
                        {
                            var skuList = wc.str1.Trim().Split(',');
                            StringBuilder searchSKU = new StringBuilder();
                            for (int i = 0; i < skuList.Length; i++)
                            {
                                if (!(skuList[i].ToString().Trim().Equals("") || skuList[i].ToString().Trim().Length == 0))
                                {
                                    searchSKU.Append("'").Append(skuList[i].ToString().Trim()).Append("'").Append(",");
                                }
                            }
                            sb.Append(" AND Location in (").Append(searchSKU.ToString().Substring(0, searchSKU.Length - 1)).Append(")");
                        }
                        else
                        {
                            sb.Append(" AND Location >='").Append(wc.str1.Trim()).Append("'");
                            sb.Append(" AND Location <='").Append(wc.str2.Trim()).Append("'");
                        }
                    }
                    //品名盘点
                    else if (wc.Type.ToString() == "3"|| wc.Type.ToString() == "10")
                    {
                        if (wc.str1.Trim().Split(',').Length > 1)
                        {
                            var skuList = wc.str1.Trim().Split(',');
                            StringBuilder searchSKU = new StringBuilder();
                            for (int i = 0; i < skuList.Length; i++)
                            {
                                if (!(skuList[i].ToString().Trim().Equals("") || skuList[i].ToString().Trim().Length == 0))
                                {
                                    searchSKU.Append("'").Append(skuList[i].ToString().Trim()).Append("'").Append(",");
                                }
                            }
                            sb.Append(" AND SKU in (").Append(searchSKU.ToString().Substring(0, searchSKU.Length - 1)).Append(")");
                        }
                        else
                        {
                            sb.Append(" AND SKU >='").Append(wc.str1.Trim()).Append("'");
                            sb.Append(" AND SKU <='").Append(wc.str2.Trim()).Append("'");
                        }
                    }
                    //小货量盘点
                    else if (wc.Type.ToString() == "4")
                    {
                        sb.Append(" AND QTY >='").Append(wc.str2.Trim()).Append("'");
                        sb.Append(" AND QTY <='").Append(wc.str1.Trim()).Append("'");
                    }
                    //变动库位盘点
                    else if (wc.Type.ToString() == "5")
                    {
                        //判断是否存在盘点类型
                        string[] s = wc.str5.Split(',');//Type_description.ToString().Substring(6, wc.Type_description.Length - 6).Split(',');
                        if (s.Length > 0)
                        {
                            foreach (string ss in s)
                            {
                                if (ss == "1")
                                {
                                    receipt = 1;
                                }
                                if (ss == "2")
                                {
                                    order = 1;
                                }
                                if (ss == "3")
                                {
                                    move = 1;
                                }
                                if (ss == "4")
                                {
                                    adjust = 1;
                                }
                                if (ss == "5")
                                {
                                    hold = 1;
                                }
                            }
                        }
                    }
                    //手工录入
                    else if (wc.Type.ToString() == "6")
                    {
                        sb.Append(" AND SKU IN (SELECT SKU FROM dbo.WMS_Order o LEFT JOIN dbo.WMS_OrderDetail d ON o.ID=d.OID WHERE o.Status=9 AND DateDiff(dd,CompleteDate,getdate())<=7)");
                    }
                    //else if (wc.Type.ToString() == "8")
                    //{
                    //    string[] s = wc.str5.Split(',');
                    //    if (s.Length > 0)
                    //    {
                    //        foreach (string ss in s)
                    //        {
                    //            if (ss == "2")
                    //            {
                    //                order = 1;
                    //            }
                    //        }
                    //    }
                    //}
                }
                #endregion
                string sqlWhere = sb.ToString();
                int tempRowCount = 0;
                DbParam[] dbParams = new DbParam[]{
                    new DbParam("@CustomerID", DbType.String, wc.CustomerID, ParameterDirection.Input),
                    new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                    new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
                        };
                StringBuilder sbs = new StringBuilder();
                if (wc.Type == 5)
                {
                    //差异盘点
                    DbParam[] dbParamss = new DbParam[]{
                    new DbParam("@CustomerID", DbType.String, wc.CustomerID, ParameterDirection.Input),
                    new DbParam("@Warehouse", DbType.String, wc.Warehouse, ParameterDirection.Input),
                    new DbParam("@Area", DbType.String, wc.Area, ParameterDirection.Input),
                    new DbParam("@Receipt", DbType.Int32,receipt, ParameterDirection.Input),
                    new DbParam("@Order", DbType.Int32,order, ParameterDirection.Input),
                    new DbParam("@Move", DbType.Int32, move, ParameterDirection.Input),
                    new DbParam("@Adjust", DbType.Int32, adjust, ParameterDirection.Input),
                    new DbParam("@Hold", DbType.Int32, hold, ParameterDirection.Input),
                    new DbParam("@BeginTime", DbType.String, wc.str3, ParameterDirection.Input),
                    new DbParam("@EndTime", DbType.String, wc.str4, ParameterDirection.Input),
                    new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
                    };
                    dt = this.ExecuteDataTable("Proc_WMS_GetWarehouseCheckByCondition5", dbParamss);
                }
                else if (wc.Type == 8)
                {
                    //空库位盘点
                    DbParam[] dbParamsss = new DbParam[]{
                        new DbParam("@CustomerID", DbType.String, wc.CustomerID, ParameterDirection.Input),//客户Id
                        new DbParam("@WarehouseID", DbType.String, wc.WareHouseID, ParameterDirection.Input),//仓库
                        new DbParam("@AreaID", DbType.String, wc.AreaID, ParameterDirection.Input),//库区                        
                        new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
                    };
                    dt = this.ExecuteDataTable("Proc_WMS_GetWarehouseCheckByCondition8", dbParamsss);

                }
                else
                {
                    //条件盘点
                    dt = this.ExecuteDataTable("Proc_WMS_GetWarehouseCheckByConditionsNew_NoGoodsType", dbParams);
                    //dt = this.ExecuteDataTable("Proc_WMS_GetWarehouseCheckByConditionsNew", dbParams);
                    
                }

                if (dt.Rows.Count == 0)
                {
                    Mess = "查询数据为空";
                }
                else
                {
                    //主表数据
                    if (wc.Type == 5)//变动库位盘点
                    {
                        string businessType = "";
                        for (int i = 0; i < wc.Roles.Length; i++)
                        {
                            businessType += wc.Roles[i] + ",";
                        }
                        Roles = businessType.Substring(0, businessType.Length - 1);
                        sbs.Append("INSERT INTO WMS_CheckHeader(CheckNumber,ExternNumber,CustomerID,CustomerName,Warehouse,Checkdate,Type,Type_Description,");
                        sbs.Append("Remark,str1,str2,str3,IS_Difference,IS_deal,Creator,CreateTime)VALUES('");
                        sbs.Append(wc.CheckNumber.Trim() + "','" + wc.ExternNumber.Trim() + "'," + wc.CustomerID + ",(SELECT name FROM dbo.Customer where id='" + wc.CustomerID + "'),'" + wc.Warehouse.Trim() + "','" + wc.Checkdate + "',");
                        sbs.Append(wc.Type + ",'" + wc.Type_description + "','" + wc.Remark + "','" + wc.str3 + "','" + wc.str4 + "','" + Roles + "',0,0,'" + wc.Oprer + "',getdate()");
                        sbs.Append(");");
                    }
                    //else if (wc.Type == 8)//空库位盘点
                    //{
                    //    string businessType = "";
                    //    for (int i = 0; i < wc.EmptyLocation.Length; i++)
                    //    {
                    //        businessType += wc.EmptyLocation[i] + ",";
                    //    }
                    //    Roles = businessType.Substring(0, businessType.Length - 1);
                    //    sbs.Append("INSERT INTO WMS_CheckHeader(CheckNumber,ExternNumber,CustomerID,CustomerName,Warehouse,Checkdate,Type,Type_Description,");
                    //    sbs.Append("Remark,str1,str2,str3,IS_Difference,IS_deal,Creator,CreateTime)VALUES('");
                    //    sbs.Append(wc.CheckNumber.Trim() + "','" + wc.ExternNumber.Trim() + "'," + wc.CustomerID + ",(SELECT name FROM dbo.Customer where id='" + wc.CustomerID + "'),'" + wc.Warehouse.Trim() + "','" + wc.Checkdate + "',");
                    //    sbs.Append(wc.Type + ",'" + wc.Type_description + "','" + wc.Remark + "','" + wc.str3 + "','" + wc.str4 + "','" + Roles + "',0,0,'" + wc.Oprer + "',getdate()");
                    //    sbs.Append(");");
                    //}
                    else
                    {
                        sbs.Append("INSERT INTO WMS_CheckHeader(CheckNumber,ExternNumber,CustomerID,CustomerName,Warehouse,Checkdate,Type,Type_Description,");
                        sbs.Append("Remark,str1,str2,str3,IS_Difference,IS_deal,Creator,CreateTime)VALUES('");
                        sbs.Append(wc.CheckNumber.Trim() + "','" + wc.ExternNumber.Trim() + "'," + wc.CustomerID + ",(SELECT name FROM dbo.Customer where id='" + wc.CustomerID + "'),'" + wc.Warehouse.Trim() + "','" + wc.Checkdate + "',");
                        sbs.Append(wc.Type + ",'" + wc.Type_description + "','" + wc.Remark + "','" + wc.str1 + "','" + wc.str2 + "','" + Roles + "',0,0,'" + wc.Oprer + "',getdate()");
                        sbs.Append(");");


                    }
                    //子表数据
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (wc.Type == 5)
                        {
                            sbs.Append("INSERT INTO WMS_CheckDetail(CID,CheckNumber,ExternNumber,CustomerID,CustomerName,warehouse,Area,Location,SKU,UPC,BatchNumber,BoxNumber,Unit,Specifications,");
                            sbs.Append("GoodsType,CheckQty,IS_DIfference,IS_Deal,str5,Creator,CreateTime) VALUES(");
                            sbs.Append("(select ID from WMS_CheckHeader where CheckNumber='" + wc.CheckNumber.Trim() + "'),'" + wc.CheckNumber.Trim() + "','");
                        }
                        else
                        {
                            sbs.Append("INSERT INTO WMS_CheckDetail(CID,CheckNumber,ExternNumber,CustomerID,CustomerName,warehouse,Area,Location,SKU,UPC,BatchNumber,BoxNumber,Unit,Specifications,");
                            sbs.Append("GoodsType,CheckQty,IS_DIfference,IS_Deal,Creator,CreateTime) VALUES(");
                            sbs.Append("(select ID from WMS_CheckHeader where CheckNumber='" + wc.CheckNumber.Trim() + "'),'" + wc.CheckNumber.Trim() + "','");
                        }
                        if (wc.Area == null)
                        {
                            sbs.Append(wc.ExternNumber.Trim() + "'," + wc.CustomerID + ",(SELECT name FROM dbo.Customer where id='" + wc.CustomerID + "'),'" + wc.Warehouse.Trim() + "'," + "''" + ",'");
                        }
                        else
                        {
                            sbs.Append(wc.ExternNumber.Trim() + "'," + wc.CustomerID + ",(SELECT name FROM dbo.Customer where id='" + wc.CustomerID + "'),'" + wc.Warehouse.Trim() + "','" + wc.Area.Trim() + "','");
                        }
                        if (wc.Type == 5)
                        {
                            sbs.Append(dt.Rows[i]["Location"].ToString() + "','" + dt.Rows[i]["SKU"].ToString() + "','" + dt.Rows[i]["UPC"].ToString() + "','" + dt.Rows[i]["BatchNumber"].ToString() + "','" + dt.Rows[i]["BoxNumber"].ToString() + "','" + dt.Rows[i]["Unit"].ToString() + "','" + dt.Rows[i]["Specifications"].ToString() + "','" + dt.Rows[i]["GoodsType"].ToString() + "'," + dt.Rows[i]["CheckQty"].ToString() + ",0,0,'" + Roles + "','");//dt.Rows[i]["BusinessType"].ToString()
                        }
                        else
                        {
                            sbs.Append(dt.Rows[i]["Location"].ToString() + "','" + dt.Rows[i]["SKU"].ToString() + "','" + dt.Rows[i]["UPC"].ToString() + "','" + dt.Rows[i]["BatchNumber"].ToString() + "','" + dt.Rows[i]["BoxNumber"].ToString() + "','" + dt.Rows[i]["Unit"].ToString() + "','" + dt.Rows[i]["Specifications"].ToString() + "','" + dt.Rows[i]["GoodsType"].ToString() + "'," + dt.Rows[i]["CheckQty"].ToString() + ",0,0,'");
                        }
                        sbs.Append(wc.Oprer.Trim() + "',getdate());");
                    }
                    string a = sbs.ToString();
                    int b = a.Length;
                    DbParam[] param = new DbParam[] {
                            new DbParam("@sql", DbType.String, a, ParameterDirection.Input)
                        };
                    DataTable dts = ExecuteDataTable("Proc_WMS_WareHouseCheckSave", param);
                }
                return "操作成功";
            }
            catch (Exception ex)
            {
                return "操作失败(" + ex.Message + ")";
            }
        }
        //2016-3-9 15:27:03  hzf 获取盘点主列表信息
        public GetWarehouseCheckByConditionResponse GetWarehouseCheckList(WarehouseCheckSearchCondition wc, out int rowCount)
        {
            string Mess = string.Empty;
            rowCount = 0;
            GetWarehouseCheckByConditionResponse response = new GetWarehouseCheckByConditionResponse();
            try
            {
                //保存盘点主子表
                DataTable dt = new DataTable();
                #region 查询条件
                //string sb = string.Empty;
                StringBuilder sb = new StringBuilder();
                if (!string.IsNullOrEmpty(wc.CheckNumber))
                {
                    //sb += " and c.CheckNumber like '%" + wc.CheckNumber + "%' ";
                    sb.Append(" and c.CheckNumber like '%" + wc.CheckNumber.Trim() + "%' ");
                }
                if (!string.IsNullOrEmpty(wc.ExternNumber))
                {
                    //sb += " and c.ExternNumber like '%" + wc.ExternNumber + "%' ";
                    sb.Append(" and c.ExternNumber like '%" + wc.ExternNumber.Trim() + "%' ");
                }
                if (wc.StartCheckdate != null)
                {
                    //sb += " and c.Checkdate >='" + wc.Checkdate.ToString() + "'";
                    sb.Append(" AND c.Checkdate >='").Append(wc.StartCheckdate.DateTimeToString("d")).Append(" 00:00:00'");
                }
                if (wc.EndCheckdate != null)
                {
                    //sb += " and c.Checkdate >='" + wc.Checkdate.ToString() + "'";
                    sb.Append(" AND c.Checkdate <='").Append(wc.EndCheckdate.DateTimeToString("d")).Append(" 23:59:59'");
                }
                if (!string.IsNullOrEmpty(wc.CustomerID.ToString()))
                {
                    //sb += " and CustomerID ='" + wc.CustomerID.ToString() + "'";
                    sb.Append(" and CustomerID ='" + wc.CustomerID.ToString().Trim() + "' ");
                }
                if (!string.IsNullOrEmpty(wc.Warehouse) && wc.Warehouse != "==请选择==")
                {
                    //sb += " and c.warehouse= " + wc.Warehouse;
                    //switch (wc.Warehouse)
                    //{
                    //    default:
                    //        break;
                    //    case "20":
                    //        wc.Warehouse = "NIKE-OSR广州仓";
                    //        break;
                    //    case "15":
                    //        wc.Warehouse = "NIKE-OSR北京仓";
                    //        break;
                    //    case "22":
                    //        wc.Warehouse = "NIKE-NFS广州仓";
                    //        break;
                    //    case "21":
                    //        wc.Warehouse = "NIKE-NFS北京仓";
                    //        break;
                    //}
                    sb.Append(" and c.warehouse= '").Append(wc.Warehouse.Trim()).Append("' ");
                }
                if (!string.IsNullOrEmpty(wc.Area) && wc.Area != "==请选择==")
                {
                    //sb += " and ae.id='" + wc.Area + "'";
                    sb.Append(" and ae.ID=" + wc.Area.Trim() + " ");
                }
                if (wc.Type.ToString() != "0")
                {
                    //sb += " and c.Type='" + wc.Type.ToString() + "'";
                    sb.Append(" and c.Type='" + wc.Type.ToString().Trim() + "' ");
                }
                #endregion
                int tempRowCount = 0;
                DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sb.ToString(), ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, wc.PageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, wc.PageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
                DataSet ds = this.ExecuteDataSet("Proc_WMS_GetWarehouseCheckByCondition", dbParams);
                rowCount = (int)dbParams[3].Value;
                response.WarehouseCheckCollection = ds.Tables[0].ConvertToEntityCollection<WarehouseCheck>();
                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        //2016-3-9 15:27:03  hzf 获取盘点单号获取盘点信息
        public GetWarehouseCheckByConditionResponse GetWarehouseCheckByCheckNumber(WarehouseCheckSearchCondition wc)
        {

            string Mess = string.Empty;

            GetWarehouseCheckByConditionResponse response = new GetWarehouseCheckByConditionResponse();
            try
            {
                //保存盘点主子表
                DataTable dt = new DataTable();
                #region 查询条件
                string sb = string.Empty;
                if (!string.IsNullOrEmpty(wc.CheckNumber))
                {
                    sb += " and  CheckNumber='" + wc.CheckNumber.Trim() + "' ";
                }
                #endregion
                DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sb, ParameterDirection.Input)
            };
                DataSet ds = this.ExecuteDataSet("Proc_WMS_GetWarehouseCheckByCheckNumber", dbParams);

                response.WarehouseCheckCollection = ds.Tables[0].ConvertToEntityCollection<WarehouseCheck>();
                response.WarehouseCheckDetailCollection = ds.Tables[1].ConvertToEntityCollection<WarehouseCheckDetail>();
                return response;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public GetWarehouseCheckByConditionResponse ExportWarehouseCheckByCheckNumber(WarehouseCheckSearchCondition wc)
        {
            string Mess = string.Empty;

            GetWarehouseCheckByConditionResponse response = new GetWarehouseCheckByConditionResponse();
            try
            {
                //保存盘点主子表
                DataTable dt = new DataTable();
                #region 查询条件
                string sb = string.Empty;
                if (!string.IsNullOrEmpty(wc.CheckNumber))
                {
                    sb += " and  CheckNumber='" + wc.CheckNumber.Trim() + "' ";
                }
                #endregion
                DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sb, ParameterDirection.Input)
            };
                DataSet ds = this.ExecuteDataSet("Proc_WMS_ExportWarehouseCheckByCheckNumber", dbParams);
                response.WarehouseCheckDetailCollection = ds.Tables[0].ConvertToEntityCollection<WarehouseCheckDetail>();
                return response;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        //2016-3-9 15:27:03  hzf 保存单条盘点信息
        public string SaveWarehouseCheckByCheckNumber(WarehouseCheckSearchCondition wc)
        {
            string Mess = string.Empty;
            GetWarehouseCheckByConditionResponse response = new GetWarehouseCheckByConditionResponse();
            try
            {
                string[] qtysum = wc.ActulQtyargs;
                if (qtysum.Length > 0)
                {
                    string str = string.Empty;
                    str = " and  CheckNumber='" + wc.CheckNumber.Trim() + "' ";

                    DbParam[] dbParams = new DbParam[]{
                            new DbParam("@Where", DbType.String,str,ParameterDirection.Input)
                        };
                    DataTable dt = this.ExecuteDataSet("Proc_WMS_GetWarehouseCheckByCheckNumber", dbParams).Tables[1];
                    if (dt.Rows.Count == qtysum.Length)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DbParam[] dbUpdate = new DbParam[]{
                        new DbParam("@ActualQty", DbType.String,qtysum[i].ToString(),ParameterDirection.Input),
                        new DbParam("@ID", DbType.Int32,int.Parse(dt.Rows[i]["ID"].ToString()),ParameterDirection.Input)

                        };
                            this.ExecuteScalar("Proc_WMS_SaveWarehouseCheckByCheckNumber", dbUpdate);
                        }
                    }

                }
                return "更新成功";


            }
            catch (Exception ex)
            {
                return "更新失败";
            }

        }
        //2016-3-9 15:27:03  hzf 删除单条盘点信息
        public string GetWarehouseCheckDelete(WarehouseCheckSearchCondition wc)
        {
            string Mess = string.Empty;
            try
            {
                DbParam[] dbParams = new DbParam[]{
                new DbParam("@CheckNumber", DbType.String,wc.CheckNumber,ParameterDirection.Input)
            };
                this.ExecuteScalar("Proc_WMS_GetWarehouseCheckDelete", dbParams);
                return "删除成功";
            }
            catch (Exception ex)
            {
                return "删除失败";
            }

        }
        //2016-3-9 15:27:03  hzf 完成单条盘点信息
        public string GetWarehouseCheckDone(WarehouseCheckSearchCondition wc, IEnumerable<WarehouseCheckDetail> Warehousecheckdetails)
        {
            //string Mess = string.Empty;
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_GetWarehouseCheckDone", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CheckNumber", wc.CheckNumber);
                    cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[0].Size = 100;
                    cmd.Parameters.AddWithValue("@Udt", Warehousecheckdetails.Select(receipt => new WMSCheckDetailToDb(receipt)));
                    cmd.Parameters[1].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[2].Direction = ParameterDirection.Output;
                    cmd.Parameters[2].Size = 50;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                    conn.Close();
                    return message;
                }
                catch (Exception ex)
                {
                    return message + "(" + ex.Message + ")";
                }
            }

            //    DbParam[] dbParams = new DbParam[]{
            //    new DbParam("@CheckNumber", DbType.String,wc.CheckNumber,ParameterDirection.Input)
            //};
            //    this.ExecuteScalar("Proc_WMS_GetWarehouseCheckDone", dbParams);
            //    return "删除成功";



        }
        #region
        //public GetWarehouseCheckByConditionResponse PrintWareHouseCheckNumber(string checknumber)
        //{
        //    GetWarehouseCheckByConditionResponse response = new GetWarehouseCheckByConditionResponse();

        //    string str = " AND CheckNumber = '" + checknumber + "'";
        //    DbParam[] dbparams = new DbParam[]{
        //        new DbParam("@Where",DbType.String,str,ParameterDirection.Input)
        //    };
        //    DataSet ds = this.ExecuteDataSet("[Proc_WMS_GetPrintWareHouseByCheckNumber]", dbparams);

        //    response.WarehouseCheckCollection = ds.Tables[0].ConvertToEntityCollection<WarehouseCheck>();
        //    response.WarehouseCheckDetailCollection = ds.Tables[1].ConvertToEntityCollection<WarehouseCheckDetail>();

        //    return response;
        //}
        #endregion
        public GetWarehouseCheckByConditionResponse GetPrintWareHouseCheckByCheckNumber(string checknumber)
        {
            GetWarehouseCheckByConditionResponse response = new GetWarehouseCheckByConditionResponse();

            string str = " AND CheckNumber = '" + checknumber + "'";
            DbParam[] dbparams = new DbParam[]{
                new DbParam("@Where",DbType.String,str,ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("[Proc_WMS_GetPrintWareHouseByCheckNumber]", dbparams);

            response.WarehouseCheck = ds.Tables[0].ConvertToEntity<WarehouseCheck>();
            response.WarehouseCheckDetailCollection = ds.Tables[1].ConvertToEntityCollection<WarehouseCheckDetail>();

            return response;
        }

        public GetWarehouseCheckByConditionResponse GetPrintWareHouseCheckByCheckNumberNike(string checknumber)
        {
            GetWarehouseCheckByConditionResponse response = new GetWarehouseCheckByConditionResponse();

            string str = " AND CheckNumber = '" + checknumber + "'";
            DbParam[] dbparams = new DbParam[]{
                new DbParam("@Where",DbType.String,str,ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("[Proc_WMS_GetPrintWareHouseByCheckNumberNike]", dbparams);

            response.WarehouseCheck = ds.Tables[0].ConvertToEntity<WarehouseCheck>();
            response.WarehouseCheckDetailCollection = ds.Tables[1].ConvertToEntityCollection<WarehouseCheckDetail>();

            return response;
        }
        public GetWarehouseCheckByConditionResponse GetPrintWareHouseCheckByCheckNumberNikeRF(string checknumber)
        {
            GetWarehouseCheckByConditionResponse response = new GetWarehouseCheckByConditionResponse();

         
            DbParam[] dbparams = new DbParam[]{
                new DbParam("@Where",DbType.String,checknumber,ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("[Proc_WMS_GetPrintWareHouseByCheckNumberNikeRF_NoGoodsType]", dbparams);
            //DataSet ds = this.ExecuteDataSet("[Proc_WMS_GetPrintWareHouseByCheckNumberNikeRF]", dbparams);
            
            response.WarehouseCheck = ds.Tables[0].ConvertToEntity<WarehouseCheck>();
            response.WarehouseCheckDetailCollection = ds.Tables[1].ConvertToEntityCollection<WarehouseCheckDetail>();

            return response;
        }
        
        /// <summary>
        /// 获取库位下拉列表（库位变更）
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LocationInfo> ALGetWarehouseLocationListByWarehouseName(string WarehouseName)
        {
            IEnumerable<LocationInfo> list = new List<LocationInfo>();   //库区列表
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {

                    DbParam[] param = new DbParam[] {
                         new DbParam("@WarehouseName", DbType.String, WarehouseName, ParameterDirection.Input)
                    };
                    DataSet ds = ExecuteDataSet("Proc_WMS_GetWarehouseLocationListByWarehouseName_AL", param);
                    list = ds.Tables[0].ConvertToEntityCollection<LocationInfo>();
                    return list;

                }
                catch (Exception ex)
                {
                    list = new List<LocationInfo>();
                    return list;
                }

            }
        }

        /// <summary>
        /// 库区变更方法
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="AreaID"></param>
        /// <param name="ToAreID"></param>
        /// <param name="sqlString"></param>
        /// <param name="LocationCount"></param>
        /// <returns></returns>
        public string AlImportLocationAndGoodShelf(string ID, string AreaID, string ToAreID, string sqlString, int LocationCount)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_ImportLocation_AI", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Where", sqlString);
                    cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@WarehouseID", ID);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@AreaID", AreaID);
                    cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@ToAreID", ToAreID);
                    cmd.Parameters[3].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@LocationCount", LocationCount);
                    cmd.Parameters[4].SqlDbType = SqlDbType.BigInt;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[5].DbType = DbType.String;
                    cmd.Parameters[5].Direction = ParameterDirection.Output;
                    cmd.Parameters[5].Size = 20;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                    conn.Close();
                    return message;
                }
                catch (Exception ex)
                {
                    return message + "(" + ex.Message + ")";
                }
            }
        }

        /// <summary>
        /// 导出库位
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public GetWLocationByConditionResponse GetWLocationInfo(WLocationSearchCondition SearchCondition, int pageIndex, int pageSize, out int rowCount)
        {
            string sqlWhere = "";
            if (SearchCondition != null)
            {
                sqlWhere = GenGetWLocationWhere(SearchCondition);
            }
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, pageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, pageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };

            DataTable dt = this.ExecuteDataTable("[Proc_WMS_GetWLocationInfo]", dbParams);
            rowCount = (int)dbParams[3].Value;
            GetWLocationByConditionResponse response = new GetWLocationByConditionResponse();
            //response.Storer = dt.ConvertToEntityCollection<Storer>();
            response.WLocationCollection = dt.ConvertToEntityCollection<LocationInfo>();
            return response;
        }

    }
}
