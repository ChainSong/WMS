using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Runbow.TWS.Common;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using System.Text;
using System.Linq;

namespace Runbow.TWS.Dao
{
    public class CustomerAccessor : BaseAccessor
    {
        public bool DeleteCustomer(long ID)
        {
            string strSQL = "update dbo.[Customer] set state=0 where [ID] =@ID";
            DbParam[] para = {
                      new DbParam("@ID", global::System.Data.DbType.Int64, ID, global::System.Data.ParameterDirection.Input)
                             };
            try
            {
                if (base.ExecuteScalarBySqlString(strSQL, para).ObjectToBoolean())
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 验证客户名称的唯一性
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public bool CheckNameIsExist(string Name, int? Id, string ProjectID, bool IsEdit)
        {
            string sql = " SELECT COUNT(1) FROM dbo.Customer A INNER JOIN Project_CustomerOrShipper_Mapping  B ON a.ID=b.CustomerOrShipperID AND b.Target=0 WHERE B.ProjectID=@ProjectID AND A.Name=@Name ";
            sql += Id > 0 ? " AND A.ID!=" + Id + " " : string.Empty;

            DbParam[] parms = {
                      new DbParam("@Name", DbType.String, Name, ParameterDirection.Input),
                       new DbParam("@ProjectID", DbType.String, ProjectID, ParameterDirection.Input)
                             };
            return (int)base.ExecuteScalarBySqlString(sql, parms) > 0 ? false : true;
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            string strSQL = "select * from dbo.[Customer] where [state]=1";
            try
            {
                return base.ExecuteDataTableBySqlString(strSQL).ConvertToEntityCollection<Customer>();
            }
            catch
            {
                return null;
            }
        }
        public IEnumerable<Customer> GetAllCustomer()
        {
            string strSQL = "SELECT c.ID,u.UserID,c.Code,c.Name,c.Description FROM dbo.Customer c LEFT JOIN dbo.User_ProjectCustomer_Mapping u ON c.ID=u.CustomerID WHERE  c.[state]=1 and (c.StoreType=2 or c.StoreType=3)";
            try
            {
                return base.ExecuteDataTableBySqlString(strSQL).ConvertToEntityCollection<Customer>();
            }
            catch
            {
                return null;
            }
        }


        public IEnumerable<BoxSize> GetApplicationBox()
        {
            string strSQL = "SELECT Str1+','+Str2+','+Str3 AS Code,Name,Int1,Str5,Code as  Str1 FROM dbo.WMS_Config WHERE Type='BoxSize'";//修改在快递包装时使用，加了一个Str1
            try
            {
                return base.ExecuteDataTableBySqlString(strSQL).ConvertToEntityCollection<BoxSize>();
            }
            catch
            {
                return null;
            }
        }
        public Customer GetCustomerById(long CustomerID)
        {
            string strSQL = "select top 1 * from dbo.[Customer] where [state]=1 and  [ID]=@ID";
            DbParam[] para = {
                      new DbParam("@ID", global::System.Data.DbType.Int64, CustomerID, global::System.Data.ParameterDirection.Input)
                             };
            try
            {
                return base.ExecuteDataTableBySqlString(strSQL, para).ConvertToEntity<Customer>();
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<ProjectCustomer> GetProjectCustomers()
        {
            return this.ExecuteDataTable("Proc_GetProjectCustomer").ConvertToEntityCollection<ProjectCustomer>();
        }

        public int SaveCustomer(Customer customer)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_InsertSaveCustomer", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Code", customer.Code);
                cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters.AddWithValue("@Name", customer.Name);
                cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters.AddWithValue("@Description", customer.Description);
                cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;

                conn.Open();
                try
                {
                    return cmd.ExecuteNonQuery();
                }
                catch
                {
                    throw;
                }
            }
        }

        public int UpdateCustomer(Customer customer)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_UpdateCustomer", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Code", customer.Code);
                cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters.AddWithValue("@Name", customer.Name);
                cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters.AddWithValue("@Description", customer.Description);
                cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters.AddWithValue("@ID", customer.ID);
                cmd.Parameters[3].SqlDbType = SqlDbType.BigInt;
                cmd.Parameters.AddWithValue("@State", customer.State);
                cmd.Parameters[4].SqlDbType = SqlDbType.Bit;

                conn.Open();
                try
                {
                    return cmd.ExecuteNonQuery();
                }
                catch
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// 客户管理新增
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public Customer InsertCustomer(Customer customer)
        {
            Customer Response = new Customer();
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string sql = string.Format("insert into [dbo].[Customer] ( Code, Name, Description, State, CreateDate, Email, LawPerson, PostCode, Address1, Address2, Bank, Account, TaxID, InvoiceTitle, Contactor1, Title1, Phone1, Fax1, Contactor2, Title2, Phone2, Fax2, WebSite, RegistAdd)"
                    + "values('{0}','{1}','{2}','{3}',sysdatetime(),'{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}')"
                    , customer.Code, customer.Name, customer.Description, customer.State, customer.Email, customer.LawPerson, customer.PostCode, customer.Address1, customer.Address2, customer.Bank, customer.Account, customer.TaxID, customer.InvoiceTitle, customer.Contactor1, customer.Title1, customer.Phone1, customer.Fax1, customer.Contactor2, customer.Title2, customer.Phone2, customer.Fax2, customer.WebSite, customer.RegistAdd);
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                Response.ID = cmd.ExecuteNonQuery();

                return Response;
            }

        }
        public IEnumerable<Customer> GetCustomerByConditon(string code, string name, long userId, long projectId, int StoreType, bool state, int pageIndex, int pageSize, out int rowCount)
        {
            int tmpRowCount = 0;
            string Where = CustomerSql(code, name, userId, projectId, StoreType, state);
            DbParam[] dbParams = {
                             new DbParam("@Where", DbType.String, Where, ParameterDirection.Input),
                             new DbParam("@PageIndex", DbType.Int32, pageIndex, ParameterDirection.Input),
                             new DbParam("@PageSize", DbType.Int32, pageSize, ParameterDirection.Input),
                             new DbParam("@RowCount", DbType.Int32, tmpRowCount, ParameterDirection.Output)
                                  };

            IEnumerable<Customer> Customer = (IEnumerable<Customer>)(this.ExecuteDataTable("Proc_GetCustomerByCondition", dbParams).ConvertToEntityCollection<Customer>()).Where(m => m.ProjectID == projectId);
            rowCount = dbParams[3].Value.ObjectToInt32();
            return Customer;
        }

        private string CustomerSql(string code, string name, long userId, long projectId, int StoreType, bool state)
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(code))
            {
                sb.Append("and a.Code ='" + code + "'");
            }
            if (!string.IsNullOrEmpty(name))
            {
                sb.Append("and a.Name='" + name + "'");
            }
            if (StoreType != 99)
            {
                sb.Append("and a.StoreType='" + StoreType + "'");
            }
            if (state)
                sb.Append("and a.State=1");
            else
                sb.Append("and a.State=0");

            sb.Append("AND a.ID IN  ( SELECT CustomerID FROM  User_ProjectCustomer_Mapping  WHERE ProjectID=" + projectId + " AND  UserID=" + userId + " )  ");
            //sb.Append(" AND b.ProjectID=" + projectId + " ");
            return sb.ToString();
        }
        public Customer selectCustomer(int id)
        {
            string sql = "select * from [dbo].[Customer] where id=" + id;
            Customer c = new Customer();
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand comm = new SqlCommand(sql, conn);

                //comm.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sdr = new SqlDataAdapter(comm);
                DataTable dt = new DataTable();
                sdr.Fill(dt);


                foreach (DataRow dr in dt.Rows)
                {

                    c.ID = dr["ID"].ObjectToInt32();
                    c.Code = dr["Code"].ToString();
                    c.Description = dr["Description"].ToString();
                    c.State = (bool)dr["State"];
                    c.CreateDate = dr["CreateDate"].ObjectToDateTime();
                    c.Email = dr["Email"].ToString();
                    c.LawPerson = dr["LawPerson"].ToString();
                    c.PostCode = dr["PostCode"].ToString();
                    c.Address1 = dr["Address1"].ToString();
                    c.Address2 = dr["Address2"].ToString();
                    c.Bank = dr["Bank"].ToString();
                    c.Account = dr["Account"].ToString();
                    c.TaxID = dr["TaxID"].ToString();
                    c.InvoiceTitle = dr["InvoiceTitle"].ToString();
                    c.Contactor1 = dr["Contactor1"].ToString();
                    c.Title1 = dr["Title1"].ToString();
                    c.Phone1 = dr["Phone1"].ToString();
                    c.Fax1 = dr["Fax1"].ToString();
                    c.Contactor2 = dr["Contactor2"].ToString();
                    c.Title2 = dr["Title2"].ToString();
                    c.Phone2 = dr["Phone2"].ToString();
                    c.Fax2 = dr["Fax2"].ToString();
                    c.WebSite = dr["WebSite"].ToString();
                    c.RegistAdd = dr["RegistAdd"].ToString();
                    c.Name = dr["Name"].ToString();
                    c.CreditLine = dr["CreditLine"].ToString();
                    c.ProvinceCity = dr["ProvinceCity"].ToString();
                    c.Remark = dr["Remark"].ToString();
                    c.Types = Convert.ToInt32(dr["Types"].ToString());
                    c.StoreStatus = Convert.ToInt32(dr["StoreStatus"].ToString());
                    c.StoreType = Convert.ToInt32(dr["StoreType"].ToString());
                }
                return c;
            }
        }
        public Customer UpdateCustomers(Customer customer)
        {
            Customer Response = new Customer();
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string sql = string.Format("update  [dbo].[Customer] "
                    + "set Code='{0}', Name='{1}',Description='{2}', State='{3}',Email='{4}',LawPerson='{5}',PostCode='{6}',Address1='{7}',Address2='{8}',Bank='{9}',Account='{10}',TaxID='{11}',InvoiceTitle='{12}',Contactor1='{13}',Title1='{14}',Phone1='{15}',Fax1='{16}',Contactor2='{17}',Title2='{18}',Phone2='{19}',Fax2='{20}',WebSite='{21}',RegistAdd='{22}',StoreType='{23}',[Types]='{24}',StoreStatus='{25}' where ID={26}"
                    , customer.Code, customer.Name, customer.Description, customer.State, customer.Email, customer.LawPerson, customer.PostCode, customer.Address1, customer.Address2, customer.Bank, customer.Account, customer.TaxID, customer.InvoiceTitle, customer.Contactor1, customer.Title1, customer.Phone1, customer.Fax1, customer.Contactor2, customer.Title2, customer.Phone2, customer.Fax2, customer.WebSite, customer.RegistAdd, customer.StoreType, customer.Types, customer.StoreStatus, customer.ID);
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                Response.ID = cmd.ExecuteNonQuery();

                return Response;
            }
        }


        /// <summary>
        /// 货主客户新增编辑
        /// </summary>
        public IEnumerable<Customer> AddCustomers(IEnumerable<Customer> customers, string UId, string name, int projectId, int segmentId)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                IList<Customer> result = new List<Customer>();
                SqlCommand cmd = new SqlCommand("Proc_AddCustomer_bak", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserName", name);
                cmd.Parameters.AddWithValue("@UId", UId);
                cmd.Parameters.AddWithValue("@Customers", customers == null ? null : customers.Select(customer => new CustomerToDb(customer)));
                cmd.Parameters.AddWithValue("@ProjectId", projectId);
                cmd.Parameters.AddWithValue("@SegmentId", segmentId);
                cmd.Parameters[2].SqlDbType = SqlDbType.Structured;

                conn.Open();
                string CustomerId = cmd.ExecuteScalar().ToString();
                conn.Close();


                result.Add(new Customer
                {
                    ID = long.Parse(CustomerId)
                });
                return result;
            }

        }
    }
}