using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    [Serializable]
    public class TableColumn
    {
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }

        [EntityPropertyExtension("ProjectID", "ProjectID")]
        public long ProjectID { get; set; }

        [EntityPropertyExtension("ProjectName", "ProjectName")]
        public string ProjectName { get; set; }

        [EntityPropertyExtension("CustomerID", "CustomerID")]
        public long CustomerID { get; set; }

        [EntityPropertyExtension("CustomerName", "CustomerName")]
        public string CustomerName { get; set; }

        [EntityPropertyExtension("TableName", "TableName")]
        public string TableName { get; set; }

        [EntityPropertyExtension("TableNameCH", "TableNameCH")]
        public string TableNameCH { get; set; }

        [EntityPropertyExtension("DisplayName", "DisplayName")]
        public string DisplayName { get; set; }


        [EntityPropertyExtension("DbColumnName", "DbColumnName")]
        public string DbColumnName { get; set; }

        [EntityPropertyExtension("IsKey", "IsKey")]
        public bool IsKey { get; set; }

        [EntityPropertyExtension("IsSearchCondition", "IsSearchCondition")]
        public bool IsSearchCondition { get; set; }

        [EntityPropertyExtension("IsHide", "IsHide")]
        public bool IsHide { get; set; }

        [EntityPropertyExtension("IsReadOnly", "IsReadOnly")]
        public bool IsReadOnly { get; set; }

        [EntityPropertyExtension("Group", "Group")]
        public string Group { get; set; }

        [EntityPropertyExtension("Type", "Type")]
        public string Type { get; set; }

        [EntityPropertyExtension("DefaultValue", "DefaultValue")]
        public string DefaultValue { get; set; }

        [EntityPropertyExtension("Order", "Order")]
        public int Order { get; set; }

        [EntityPropertyExtension("IsShowInList", "IsShowInList")]
        public bool IsShowInList { get; set; }

        [EntityPropertyExtension("IsImportColumn", "IsImportColumn")]
        public bool IsImportColumn { get; set; }

        [EntityPropertyExtension("SearchConditionOrder", "SearchConditionOrder")]
        public int SearchConditionOrder { get; set; }

        [EntityPropertyExtension("ForView", "ForView")]
        public bool ForView { get; set; }

        [EntityPropertyExtension("Module", "Module")]
        public string Module { get; set; }

        [EntityPropertyExtension("IsInnerColumn", "IsInnerColumn")]
        public int IsInnerColumn { get; set; }
    }
}