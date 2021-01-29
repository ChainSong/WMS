using System.Data;
using Microsoft.SqlServer.Server;

namespace Runbow.TWS.Entity
{
    public class ProjectCustomerOrShipperToDb : SqlDataRecord
    {
        public ProjectCustomerOrShipperToDb(ProjectCustomersOrShippers projectCustomerOrShippers)
            : base(s_metadata)
        {
            SetSqlInt64(0, projectCustomerOrShippers.ProjectShipperOrCustomerID);
            SetSqlInt64(1, projectCustomerOrShippers.ProjectID);
            SetSqlInt32(2, projectCustomerOrShippers.Target);
            SetSqlInt64(3, projectCustomerOrShippers.CustomerOrShipperID);
            SetSqlBoolean(4, projectCustomerOrShippers.IsDefault);
            SetSqlBoolean(5, projectCustomerOrShippers.State);
            SetSqlInt64(6, projectCustomerOrShippers.SegmentID ?? 0);
        }

        private static readonly SqlMetaData[] s_metadata =
        {
            new SqlMetaData("ID", SqlDbType.BigInt),
            new SqlMetaData("ProjectID", SqlDbType.BigInt),
            new SqlMetaData("Target", SqlDbType.Int),
            new SqlMetaData("CustomerOrShipperID", SqlDbType.BigInt),
            new SqlMetaData("IsDefault", SqlDbType.Bit),
            new SqlMetaData("State", SqlDbType.Bit),
            new SqlMetaData("SegmentID", SqlDbType.BigInt)
        };
    }
}