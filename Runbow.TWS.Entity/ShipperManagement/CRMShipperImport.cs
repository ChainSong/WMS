using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.ShipperManagement
{
    public class CRMShipperImport
    {
        #region Model
        /// <summary>
        /// 自增ID
        /// </summary>
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        [EntityPropertyExtension("Name", "Name")]
        public string Name { get; set; }

        /// <summary>
        /// 运输方式
        /// </summary>
        [EntityPropertyExtension("TransportMode", "TransportMode")]
        public string TransportMode { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [EntityPropertyExtension("Creator", "Creator")]
        public string Creator { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public DateTime CreateTime { get; set; }

        #endregion
    }
}
