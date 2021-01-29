using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Runbow.TWS.Common;
using Runbow.TWS.Entity;
using System.Linq;

namespace Runbow.TWS.Dao
{
    public class ShipperAccessor : BaseAccessor
    {
        public IEnumerable<Shipper> GetShippers()
        {
            return base.ExecuteDataTable("Proc_GetShippers").ConvertToEntityCollection<Shipper>();
        }

        public IEnumerable<Shipper> GetShippersByConditon(string code, string name, string englishName, bool state, int pageIndex, int pageSize, long projectID, out int rowCount)
        {
            int tmpRowCount = 0;
            DbParam[] dbParams = {
                             new DbParam("@Code", DbType.String, code, ParameterDirection.Input),
                             new DbParam("@Name", DbType.String, name, ParameterDirection.Input),
                             new DbParam("@EnglishName", DbType.String, englishName, ParameterDirection.Input),
                             new DbParam("@State", DbType.Boolean, state, ParameterDirection.Input),
                             new DbParam("@PageIndex", DbType.Int32, pageIndex, ParameterDirection.Input),
                             new DbParam("@PageSize", DbType.Int32, pageSize, ParameterDirection.Input),
                             new DbParam("@RowCount", DbType.Int32, tmpRowCount, ParameterDirection.Output),
                             new DbParam("@ProjectID", DbType.Int32, projectID, ParameterDirection.Input)
                                  };

            IEnumerable<Shipper> shippers = this.ExecuteDataTable("Proc_GetShippersByCondition", dbParams).ConvertToEntityCollection<Shipper>();
            rowCount = dbParams[6].Value.ObjectToInt32();
            return shippers;
        }

        /// <summary>
        /// 验证承运商名称的唯一性
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public bool CheckNameIsExist(string Name, int? Id, string ProjectID, bool IsEdit)
        {
            string sql = " SELECT COUNT(1) FROM dbo.Shipper A INNER JOIN Project_CustomerOrShipper_Mapping  B ON a.ID=b.CustomerOrShipperID AND b.Target=1 WHERE B.ProjectID=@ProjectID AND A.Name=@Name ";
            sql += Id > 0 ? " AND A.ID!=" + Id + " " : string.Empty;

            DbParam[] parms = {
                      new DbParam("@Name", DbType.String, Name, ParameterDirection.Input),
                       new DbParam("@ProjectID", DbType.String, ProjectID, ParameterDirection.Input)
                             };
            return (int)base.ExecuteScalarBySqlString(sql, parms) > 0 ? false : true;
        }

        public Vehicle AddOrUpdateVehicle(Vehicle vehicle)
        {
            DbParam[] dbParams = {
                             new DbParam("@ID", DbType.Int64, vehicle.ID, ParameterDirection.Input),
                             new DbParam("@ShipperID", DbType.Int64, vehicle.ShipperID, ParameterDirection.Input),
                             new DbParam("@PlateNumber", DbType.String, vehicle.PlateNumber, ParameterDirection.Input),
                             new DbParam("@Pilot", DbType.String, vehicle.Pilot, ParameterDirection.Input),
                             new DbParam("@JobNumber", DbType.String, vehicle.JobNumber, ParameterDirection.Input),
                             new DbParam("@Contract", DbType.String, vehicle.Contract, ParameterDirection.Input),
                             new DbParam("@Str1", DbType.String, vehicle.Str1, ParameterDirection.Input),
                             new DbParam("@Str2", DbType.String, vehicle.Str2, ParameterDirection.Input),
                             new DbParam("@Str3", DbType.String, vehicle.Str3, ParameterDirection.Input),
                             new DbParam("@Str4", DbType.String, vehicle.Str4, ParameterDirection.Input),
                             new DbParam("@Str5", DbType.String, vehicle.Str5, ParameterDirection.Input),
                             new DbParam("@Str6", DbType.String, vehicle.Str6, ParameterDirection.Input),
                             new DbParam("@Str7", DbType.String, vehicle.Str7, ParameterDirection.Input),
                             new DbParam("@Str8", DbType.String, vehicle.Str8, ParameterDirection.Input),
                             new DbParam("@DateTime1", DbType.DateTime, vehicle.DateTime1.HasValue ? vehicle.DateTime1.Value : System.Data.SqlTypes.SqlDateTime.Null, ParameterDirection.Input),
                             new DbParam("@DateTime2", DbType.DateTime, vehicle.DateTime2.HasValue ? vehicle.DateTime2.Value : System.Data.SqlTypes.SqlDateTime.Null, ParameterDirection.Input),
                             new DbParam("@Decimal1", DbType.Decimal, vehicle.Decimal1.HasValue ? vehicle.Decimal1.Value : System.Data.SqlTypes.SqlDecimal.Null, ParameterDirection.Input),
                             new DbParam("@State", DbType.Boolean, vehicle.State, ParameterDirection.Input),
                             new DbParam("@Creator", DbType.String, vehicle.Creator, ParameterDirection.Input),
                             new DbParam("@CreateTime", DbType.DateTime, vehicle.CreateTime.HasValue ? vehicle.CreateTime.Value : DateTime.Now, ParameterDirection.Input)
                                  };

            return this.ExecuteDataTable("Proc_AddOrUpdateVehicle", dbParams).ConvertToEntity<Vehicle>();
        }

        public long AddOrUpdateShipper(Shipper shipper, long ProjectId)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_AddOrUpdateShippers", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Shippers", new ShipperToDb[] { new ShipperToDb(shipper) });
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@ProjectID", ProjectId);
                cmd.Parameters[1].SqlDbType = SqlDbType.Int;
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return reader.GetInt64(0);
                }

                return 0;
            }
        }

        public IEnumerable<Shipper> GetShipperList(string SqlWhere)
        {
            string cmdText = "select * from [Shipper] where 1 = 1" + SqlWhere;
            return base.ExecuteDataTableBySqlString(cmdText).ConvertToEntityCollection<Shipper>();
        }

        public Shipper GetShipperById(long ID)
        {
            DbParam[] para = {
                      new DbParam("@ID", DbType.Int64, ID, ParameterDirection.Input)
                             };
            return base.ExecuteDataTable("Proc_GetShipperByID", para).ConvertToEntity<Shipper>();
        }

        public IEnumerable<Vehicle> GetShipperVehicle(long ID)
        {
            DbParam[] para = {
                      new DbParam("@ID", DbType.Int64, ID, ParameterDirection.Input)
                             };
            return base.ExecuteDataTable("Proc_GetShipperVehicle", para).ConvertToEntityCollection<Vehicle>();
        }

        public IEnumerable<ProjectShipper> GetProjectShippers()
        {
            return this.ExecuteDataTable("Proc_GetProjectShippers").ConvertToEntityCollection<ProjectShipper>();
        }

        public bool DeleteVehicle(long id)
        {
            DbParam[] para = {
                      new DbParam("@ID", DbType.Int64, id, ParameterDirection.Input)
                             };
            base.ExecuteNoQuery("Proc_DeleteShipperVehicle", para);

            return true;
        }

        public ShipperAllInfo GetShipperAllInfo(long ShipperID, long? ProjectID, long? RelatedCustomerID)
        {
            ShipperAllInfo shipperAllInfo = new ShipperAllInfo();
            shipperAllInfo.Shipper = this.GetShipperById(ShipperID);
            if (ProjectID != null && RelatedCustomerID != null)
            {
                DbParam[] para = {
                      new DbParam("@ProjectID", DbType.Int64, ProjectID.Value, ParameterDirection.Input),
                      new DbParam("@RelatedCustomerID", DbType.Int64, RelatedCustomerID.Value, ParameterDirection.Input),
                      new DbParam("@ShipperID", DbType.Int64, ShipperID, ParameterDirection.Input)
                             };
                shipperAllInfo.ShipperRelatedInfo = base.ExecuteDataTable("Proc_GetShipperRelatedInfo", para).ConvertToEntity<ShipperRelatedInfo>();
                shipperAllInfo.ShipperRegionCoveredCollection = base.ExecuteDataTable("Proc_GetShipperRegionCovered", para).ConvertToEntityCollection<ShipperRegionCovered>();

            }
            else
            {
                shipperAllInfo.ShipperRegionCoveredCollection = null;
                shipperAllInfo.ShipperRelatedInfo = null;
            }

            return shipperAllInfo;
        }

        public void ManageShipperEmailInfo(long ProjectID, long RelatedCustomerID, long ShipperID, string ShipperName, string EmailAddress, string EmailContent, int Type)
        {
            DbParam[] para = {
                      new DbParam("@ProjectID", DbType.Int64, ProjectID, ParameterDirection.Input),
                      new DbParam("@RelatedCustomerID", DbType.Int64, RelatedCustomerID, ParameterDirection.Input),
                      new DbParam("@ShipperID", DbType.Int64, ShipperID, ParameterDirection.Input),
                      new DbParam("@ShipperName", DbType.String, ShipperName, ParameterDirection.Input),
                      new DbParam("@EmailAddress", DbType.String, EmailAddress, ParameterDirection.Input),
                      new DbParam("@EmailContent", DbType.String, EmailContent, ParameterDirection.Input),
                      new DbParam("@Type", DbType.String, Type, ParameterDirection.Input)
                             };
            base.ExecuteNoQuery("Proc_ManageShipperEmailInfo", para);
        }

        public void ManageShipperRegionCovered(long ProjectID, long RelatedCustomerID, long ShipperID, string ShipperName, long StartCityID, string StartCityName, long EndCityID, string EndCityName)
        {
            DbParam[] para = {
                      new DbParam("@ProjectID", DbType.Int64, ProjectID, ParameterDirection.Input),
                      new DbParam("@RelatedCustomerID", DbType.Int64, RelatedCustomerID, ParameterDirection.Input),
                      new DbParam("@ShipperID", DbType.Int64, ShipperID, ParameterDirection.Input),
                      new DbParam("@ShipperName", DbType.String, ShipperName, ParameterDirection.Input),
                      new DbParam("@StartCityID", DbType.Int64, StartCityID, ParameterDirection.Input),
                      new DbParam("@StartCityName", DbType.String, StartCityName, ParameterDirection.Input),
                      new DbParam("@EndCityID", DbType.Int64, EndCityID, ParameterDirection.Input),
                      new DbParam("@EndCityName", DbType.String, EndCityName, ParameterDirection.Input)
                             };
            base.ExecuteNoQuery("Proc_ManageShipperRegionCovered", para);
        }

        public void DeleteShipperRegionCovered(long ProjectID, long RelatedCustomerID, long ShipperID, long StartCityID, long EndCityID)
        {
            DbParam[] para = {
                      new DbParam("@ProjectID", DbType.Int64, ProjectID, ParameterDirection.Input),
                      new DbParam("@RelatedCustomerID", DbType.Int64, RelatedCustomerID, ParameterDirection.Input),
                      new DbParam("@ShipperID", DbType.Int64, ShipperID, ParameterDirection.Input),
                      new DbParam("@StartCityID", DbType.Int64, StartCityID, ParameterDirection.Input),
                      new DbParam("@EndCityID", DbType.Int64, EndCityID, ParameterDirection.Input)
                             };
            base.ExecuteNoQuery("Proc_DeleteShipperRegionCovered", para);
        }
    }
}