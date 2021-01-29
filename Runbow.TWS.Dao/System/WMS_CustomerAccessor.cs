using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Runbow.TWS.Common;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using System.Text;
using System.Linq;
using Runbow.TWS.Entity.WMS.Product;
using Runbow.TWS.Entity.DataBaseEntity;

namespace Runbow.TWS.Dao
{
    public class WMS_CustomerAccessor : BaseAccessor
    {
        public bool DeleteCustomer(string StorerKey, string CustomerID)
        {
            string strSQL = "delete  dbo.[WMS_Customer]  where [StorerKey] ='" + StorerKey + "'" + " and [CustomerID]='" + CustomerID + "'";
            DbParam[] para = {
                      new DbParam("@ID", global::System.Data.DbType.String, StorerKey, global::System.Data.ParameterDirection.Input),
                      new DbParam("@CustomerID", global::System.Data.DbType.String, CustomerID, global::System.Data.ParameterDirection.Input)
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

        public IEnumerable<WMS_Customer> GetAllWMS_Customers()
        {
            string strSQL = "select * from dbo.[WMS_Customer] ";
            try
            {
                return base.ExecuteDataTableBySqlString(strSQL).ConvertToEntityCollection<WMS_Customer>();
            }
            catch
            {
                return null;
            }
        }
        public IEnumerable<WMS_Customer> GetAllCustomer()
        {
            string strSQL = "select * from dbo.[WMS_Customer] "; 
            try
            {
                return base.ExecuteDataTableBySqlString(strSQL).ConvertToEntityCollection<WMS_Customer>();
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<WMS_Customer> GetWMSCustomerByCustomerID(long customerID)
        {
            string strSQL = "select * from dbo.[WMS_Customer] where CustomerID="+customerID+" ";
            try
            {
                return base.ExecuteDataTableBySqlString(strSQL).ConvertToEntityCollection<WMS_Customer>();
            }
            catch
            {
                return null;
            }
        }


        public IEnumerable<BoxSize> GetApplicationBox()
        {
            string strSQL = "SELECT Str1+','+Str2+','+Str3 AS Code,Name,Int1 FROM dbo.WMS_Config WHERE Type='BoxSize'";
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
        public IEnumerable<WMS_Customer> WMS_GetCustomerByConditon(string customerid,string StorerKey, string Contact1, string PhoneNum, string Company, int pageIndex, int pageSize, out int rowCount)
        {
            int tmpRowCount = 0;
            string Where = CustomerSql(StorerKey, Contact1, PhoneNum, Company, customerid);
            DbParam[] dbParams = {
                             new DbParam("@Where", DbType.String, Where, ParameterDirection.Input),
                             new DbParam("@PageIndex", DbType.Int32, pageIndex, ParameterDirection.Input),
                             new DbParam("@PageSize", DbType.Int32, pageSize, ParameterDirection.Input),
                             new DbParam("@RowCount", DbType.Int32, tmpRowCount, ParameterDirection.Output)
                                  };

            IEnumerable<WMS_Customer> Customer = this.ExecuteDataTable("Proc_WMS_GetCustomerByCondition", dbParams).ConvertToEntityCollection<WMS_Customer>();
            rowCount = dbParams[3].Value.ObjectToInt32();
            return Customer;
        }

        private string CustomerSql(string StorerKey, string Contact1, string PhoneNum, string Company, string customerid)
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(StorerKey.Trim()))
            {
                sb.Append(" and a.StorerKey like '%" + StorerKey.Trim() + "%'");
            }

            if (!string.IsNullOrEmpty(Contact1.Trim()))
            {
                sb.Append(" and a.Contact1 like '%" + Contact1.Trim() + "%'");
                sb.Append(" or a.Contact2 like '%" + Contact1.Trim() + "%'");
            }

            if (!string.IsNullOrEmpty(PhoneNum.Trim()))
            {
                sb.Append("and a.PhoneNum1 like '%" + PhoneNum.Trim() + "%'");
                sb.Append(" or a.PhoneNum2 like '%" + PhoneNum.Trim() + "%'");
            }

            if (!string.IsNullOrEmpty(Company.Trim()))
            {
                sb.Append(" and a.Company like '%" + Company.Trim() + "%'");
            }
            if (!string.IsNullOrEmpty(customerid))
            {
                if (customerid.Contains(','))
                {
                    customerid = customerid.Substring(0, customerid.Length - 1);
                }
                sb.Append(" and a.customerid in (" + customerid + ")");
            }
            sb.Append(" AND active=1 ");

            return sb.ToString();
        }
        public WMS_Customer selectCustomer(string StorerKey,string customerid)
        {
            string sql = "select * from [dbo].[WMS_Customer] where StorerKey='" + StorerKey + "' and customerid='" + customerid + "'";
            WMS_Customer c = new WMS_Customer();
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand comm = new SqlCommand(sql, conn);

                //comm.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sdr = new SqlDataAdapter(comm);
                DataTable dt = new DataTable();
                sdr.Fill(dt);


                foreach (DataRow dr in dt.Rows)
                {
                    c.StorerKey = dr["StorerKey"].ToString();
                    c.City = dr["City"].ToString();
                    c.Company = dr["Company"].ToString();
                    c.AddressLine1 = dr["AddressLine1"].ToString();
                    c.AddressLine2 = dr["AddressLine2"].ToString();
                    c.Contact1 = dr["Contact1"].ToString();
                    c.Contact2 = dr["Contact2"].ToString();
                    c.PhoneNum1 = dr["PhoneNum1"].ToString();
                    c.PhoneNum2 = dr["PhoneNum2"].ToString();
                    c.FaxNum1 = dr["FaxNum1"].ToString();
                    c.FaxNum2 = dr["FaxNum2"].ToString();
                    c.Email1 = dr["Email1"].ToString();
                    c.Email2 = dr["Email2"].ToString();
                    c.ReceiptPrefix = dr["ReceiptPrefix"].ToString();
                    c.OrderPrefix = dr["OrderPrefix"].ToString();
                    c.Country = dr["Country"].ToString();
                    c.State = dr["State"].ToString();
                    c.UserDef10 = dr["UserDef10"].ToString();
                    c.CompanyCode = dr["CompanyCode"].ToString();
                    c.UserDef2 = dr["UserDef2"].ToString();
                    c.UserDef3 = dr["UserDef3"].ToString();
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
        public IEnumerable<WMS_Customer> AddCustomers(IEnumerable<WMS_Customer> customers)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                IList<WMS_Customer> result = new List<WMS_Customer>();
                SqlCommand cmd = new SqlCommand("Proc_AddWMS_Customer_bak", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@WMS_Customers", customers == null ? null : customers.Select(customer => new WMS_CustomerToDb(customer)));

                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;

                conn.Open();
                cmd.ExecuteScalar();
                conn.Close();


                result.Add(new WMS_Customer
                {
                    // ID = long.Parse(CustomerId)
                });
                return result;
            }

        }
    }
}