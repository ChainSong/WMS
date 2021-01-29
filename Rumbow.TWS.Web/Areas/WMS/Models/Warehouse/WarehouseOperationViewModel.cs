using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Runbow.TWS.Entity;
using System.Web.Mvc;
using Runbow.TWS.Web.Common;

namespace Runbow.TWS.Web.Areas.WMS.Models.Warehouse
{
    public class WarehouseOperationViewModel
    {

        //0 View 1 Create 2 Edit
        public int? ViewType { get; set; }            //页面类型
        public int? PageIndex { get; set; }           //页码
        public int? PageCount { get; set; }           //页数



        public IEnumerable<SelectListItem> LocationStatus
        {
            get
            {
                IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("LocationStatus");
                List<SelectListItem> st = new List<SelectListItem>();
                foreach (WMSConfig w in wms)
                {
                    st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
                }

                return st;
            }
        }


        /// <summary>
        /// 仓库类型
        /// </summary>
        public IEnumerable<SelectListItem> WarehouseTypeList
        {
            get
            {
                IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("WarehouseType");
                List<SelectListItem> st = new List<SelectListItem>();
                foreach (WMSConfig w in wms)
                {
                    st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
                }

                return st;
            }
        }

        public IEnumerable<SelectListItem> AreaTypeList
        {
            get
            {
                IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("AreaType");
                List<SelectListItem> st = new List<SelectListItem>();
                st.Add(new SelectListItem() { Value = "", Text = "==请选择==" });
                foreach (WMSConfig w in wms)
                {
                    st.Add(new SelectListItem() { Value = w.Name, Text = w.Name });
                }

                return st;
            }
        }

        /// <summary>
        /// 仓库状态
        /// </summary>
        public IEnumerable<SelectListItem> WarehouseStatusList
        {
            get
            {
                IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("WarehouseStatus");
                List<SelectListItem> st = new List<SelectListItem>();
                foreach (WMSConfig w in wms)
                {
                    if (w.Code == "1")
                    {
                        st.Add(new SelectListItem() { Value = w.Code, Text = w.Name, Selected = true });
                    }
                    else
                    {
                        st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });

                    }
                }

                return st;
            }
        }

        /// <summary>
        /// 仓库列表
        /// </summary>
        public IEnumerable<SelectListItem> WarehouseList
        {
            get;
            set;
            //{
            //    IEnumerable<WarehouseInfo> list=ApplicationConfigHelper.GetWarehouseList();
            //    List<SelectListItem> st=new List<SelectListItem>();
            //    foreach(WarehouseInfo warehouse in list)
            //    {
            //        st.Add(new SelectListItem(){Value=warehouse.ID.ToString(),Text=warehouse.WarehouseName});
            //    }
            //     return st;
            //}
        }
        public IEnumerable<SelectListItem> GoodsShelfList
        {
            get;
            set;

        }
        public IEnumerable<SelectListItem> LevelsList
        {
            get
            {
                IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("LevelsList");
                List<SelectListItem> st = new List<SelectListItem>();
                foreach (WMSConfig w in wms)
                {
                    st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
                }
                return st;
            }
        }

        public IEnumerable<SelectListItem> SerialNumberList
        {
            get
            {
                IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("SerialNumberList");
                List<SelectListItem> st = new List<SelectListItem>();
                foreach (WMSConfig w in wms)
                {
                    st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
                }
                return st;
            }

        }
        /// <summary>
        /// 库区列表
        /// </summary>
        public IEnumerable<SelectListItem> WarehouseAreaList
        {
            get
            {
                long WarehouseID = 0;
                IEnumerable<AreaInfo> list = ApplicationConfigHelper.GetWarehouseAreaList(WarehouseID);
                List<SelectListItem> st = new List<SelectListItem>();
                foreach (AreaInfo warehouse in list)
                {
                    st.Add(new SelectListItem() { Value = warehouse.ID.ToString(), Text = warehouse.AreaName });
                }
                return st;
            }

        }

        /// <summary>
        /// 库位类型 
        /// </summary>
        public IEnumerable<SelectListItem> ParaLocationTypeList
        {
            get;
            set;
            //{
            //    IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("ParaLocationType");
            //    List<SelectListItem> st = new List<SelectListItem>();
            //    foreach (WMSConfig w in wms)
            //    {
            //        st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
            //    }
            //    return st;
            //}
        }

        /// <summary>
        /// 库位分类
        /// </summary>
        public IEnumerable<SelectListItem> ParaLocationClassifyList
        {
            get
            {
                IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("ParaLocationClassify");
                List<SelectListItem> st = new List<SelectListItem>();
                foreach (WMSConfig w in wms)
                {
                    st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
                }
                return st;
            }
        }

        /// <summary>
        /// ABCClassification
        /// </summary>
        public IEnumerable<SelectListItem> ABCClassificationList
        {
            get
            {
                IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("ABCClassification");
                List<SelectListItem> st = new List<SelectListItem>();
                foreach (WMSConfig w in wms)
                {
                    st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
                }

                return st;
            }
        }


        /// <summary>
        /// 库位Handing
        /// </summary>
        public IEnumerable<SelectListItem> HandlingList
        {
            get
            {
                IEnumerable<WMSConfig> wms = ApplicationConfigHelper.GetWMS_Config("Handling");
                List<SelectListItem> st = new List<SelectListItem>();
                foreach (WMSConfig w in wms)
                {
                    st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
                }

                return st;
            }
        }

        /// <summary>
        /// 是否整箱
        /// </summary>
        public IEnumerable<SelectListItem> IsWholeCase
        {
            get
            {
                List<SelectListItem> st = new List<SelectListItem>();
                st.Add(new SelectListItem() { Value = "0", Text = "零散" });
                st.Add(new SelectListItem() { Value = "1", Text = "整箱" });
                return st;
            }
        }



        public WarehouseInfo Warehouse { get; set; }  //仓库信息
        public AreaInfo Area { get; set; }            //库区信息
        public LocationInfo Location { get; set; }    //库位信息
        public GoodsShelfInfo GoodsShelf { get; set; }    //货架信息

    }
}