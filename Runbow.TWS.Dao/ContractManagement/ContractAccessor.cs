using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Runbow.TWS.Common;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;

namespace Runbow.TWS.Dao
{
    public class ContractAccessor : BaseAccessor
    {
        /// <summary>
        /// 根据合同ID得到合同
        /// </summary>
        /// <param name="ContractID"></param>
        /// <returns></returns>
        public Contract GetContractByID(long ContractID)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {
                    DbParam[] param = new DbParam[] { new DbParam("@id", DbType.Int64, ContractID, ParameterDirection.Input) };
                    return ExecuteDataTable("Proc_Contract_GetContractByID", param).ConvertToEntity<Contract>();
                }
                catch (Exception ex)
                {
                    return null;
                }

            }
        }

        /// <summary>
        /// 根据查询条件分页得到合同
        /// </summary>
        /// <param name="SearchtionCondition"></param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <param name="rowCount">总条数</param>
        /// <returns></returns>
        public IEnumerable<Contract> GetContractByCondition(DataTable dt, int pageIndex, int pageSize, out int rowCount)
        {

            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_Contract_Search", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@InsertData", dt);
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@pagesize", pageSize);
                    cmd.Parameters.AddWithValue("@pageindex", pageIndex);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                    conn.Open();
                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    rowCount = Convert.ToInt32(ds.Tables[1].Rows[0][0]); //获取数据总数
                    return ds.Tables[0].ConvertToEntityCollection<Contract>();
                }
                catch (Exception ex)
                {
                    rowCount = 0;
                    return null;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// 新增或者编辑合同
        /// </summary>
        /// <param name="contractCollection">新增或编辑的合同列表</param>
        /// <returns>合同ID集合</returns>
        public Contract AddOrUpdateContract(DataTable dt, out string message)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_Contract_Merge", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@InsertData", dt);
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.Add("@message", SqlDbType.NVarChar, 50);
                cmd.Parameters[1].Direction = ParameterDirection.Output;
                conn.Open();
                DataSet ds = new DataSet();

                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(ds);
                message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                conn.Close();

                ///定义合同类
                Contract contract = new Contract();
                return ds.Tables[0].ConvertToEntity<Contract>();
            }
        }

        /// <summary>
        /// 根据ID删除合同
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteContract(long id)
        {
            try
            {
                //定义合同管理公参
                IList<ContractConfig> list = new List<ContractConfig>();
                DbParam[] param = new DbParam[] { new DbParam("@id", DbType.Int64, id, ParameterDirection.Input) };

                ///从数据库读取到datatable上，再存到list上
                ExecuteNoQuery("pro_contract_delete", param);
                return true;
            }
            catch (Exception ex) 
            {
                return false;
            }
        }

        /// <summary>
        /// 根据ID顺延合同
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ExtendContract(long id) 
        {
            try
            {
                //定义合同管理公参
                IList<ContractConfig> list = new List<ContractConfig>();
                DbParam[] param = new DbParam[] { new DbParam("@id", DbType.Int64, id, ParameterDirection.Input) };

                ///从数据库读取到datatable上，再存到list上
                ExecuteNoQuery("pro_contract_extend", param);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 根绝类型取得到期或者即将到期合同
        /// </summary>
        /// <param name="ContractExpireType"></param>
        /// <returns></returns>
        public IEnumerable<Contract> GetExpireContracts(int ContractExpireType)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 合同延期
        /// </summary>
        /// <param name="ContractID"></param>
        /// <returns></returns>
        public bool ContractExtension(long ContractID)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 根据类型获取合同公参数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IEnumerable<ContractConfig> GetContractListByType(string type)
        {
            //定义合同管理公参
            IList<ContractConfig> list = new List<ContractConfig>();
            DbParam[] param = new DbParam[] { new DbParam("@type", DbType.String, type, ParameterDirection.Input) };

            ///从数据库读取到datatable上，再存到list上
            DataTable dt = ExecuteDataTable("Proc_ContractConfig_Search", param);
            return dt.ConvertToEntityCollection<ContractConfig>();
        }
    }
}
