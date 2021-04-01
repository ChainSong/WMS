using System;
using System.Collections.Generic;
using System.Linq;
using Runbow.TWS.Biz;
using Runbow.TWS.Biz.ShipperManagement;
using Runbow.TWS.Common;
using Runbow.TWS.Dao;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.ShipperManagement.DriverManagement;
using Runbow.TWS.Entity.ShipperManagement.VehicleManagement;
using Runbow.TWS.Entity.WMS.Product;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.MessageContracts.WMS.ASNs;
using Runbow.TWS.Biz.WMS;
using Runbow.TWS.Entity.System;
using Runbow.TWS.Entity.ShipperManagement;
using System.Text;
using System.Data;
using Runbow.TWS.Entity.WMS;
using Runbow.TWS.Entity.WMS.Warehouse;

namespace Runbow.TWS.Web.Common
{
    public class ApplicationConfigHelper
    {
        private static object lockobject = new object();

        public static Projects GetApplicationConfig()
        {

            return (Projects)CacheHelper.Get("Projects", () =>
            {
                XmlSerializerHelper<Projects> helper = new XmlSerializerHelper<Projects>();
                helper.Load(Constants.APPLICATIONCONFIGPATH);
                Projects projects = helper.Value;

                if (projects == null)
                {
                    throw new Exception("不能加载系统配置文件");
                }
                lock (lockobject)
                {
                    CacheHelper.Insert("Projects", projects, new System.Web.Caching.CacheDependency(Constants.APPLICATIONCONFIGPATH));
                }
            });
        }

        public static void RefreshGetApplicationConfigNew(long? ProjectID, long? CustomerID)
        {
            ConfigService service = new ConfigService();
            var rg = service.GetApplicationConfigNew(ProjectID, CustomerID).Result;
            List<Column> co = new List<Column>();
            co = rg.Where(q => q.IsInnerColumn == 0).Select(a => new Column
            {
                DisplayName = a.DisplayName,
                DbColumnName = a.DbColumnName,
                IsKey = a.IsKey,
                IsSearchCondition = a.IsSearchCondition,
                IsHide = a.IsHide,
                IsReadOnly = a.IsReadOnly,
                Group = a.Group,
                Type = a.Type,
                DefaultValue = a.DefaultValue,
                Order = a.Order,
                IsShowInList = a.IsShowInList,
                IsImportColumn = a.IsImportColumn,
                SearchConditionOrder = a.SearchConditionOrder,
                ForView = a.ForView,
                CustomerID = a.CustomerID,
                TableName = a.TableName,
                InnerColumns = null
                //rg.Where(b => b.IsInnerColumn == 1 && a.TableName == b.TableName && b.DbColumnName == a.DbColumnName)
                //&& b.ProjectID == a.ProjectID && a.ProjectID == ProjectID.Value && a.CustomerID == CustomerID.Value
                //.Select(b => new Column
                //{
                //    DisplayName = b.DisplayName,
                //    DbColumnName = b.DbColumnName,
                //    IsKey = b.IsKey,
                //    IsSearchCondition = b.IsSearchCondition,
                //    IsHide = b.IsHide,
                //    IsReadOnly = b.IsReadOnly,
                //    Group = b.Group,
                //    Type = b.Type,
                //    DefaultValue = b.DefaultValue,
                //    Order = b.Order,
                //    IsShowInList = b.IsShowInList,
                //    IsImportColumn = b.IsImportColumn,
                //    SearchConditionOrder = b.SearchConditionOrder,
                //    ForView = b.ForView,
                //    CustomerID = b.CustomerID,
                //    TableName = b.TableName
                //}).ToList()
            }).ToList();
            List<Table> tb = new List<Table>();
            tb = rg.Where(a => a.IsInnerColumn == 0).GroupBy(q => q.TableName).Select(a => new Table
            {
                Name = a.Key,
                ColumnCollection = co.Where(b => b.TableName == a.Key).ToList()
            }).ToList();
            Tables tbs = new Tables();
            tbs.TableCollection = tb;
            List<Module> mo = new List<Module>();
            mo = rg.Where(a => a.IsInnerColumn == 0).GroupBy(q => q.Module).Select(a => new Module
            {
                Id = a.Key,
                Tables = tbs
            }).ToList();
            List<Runbow.TWS.Entity.Application.Project> pjc = new List<Runbow.TWS.Entity.Application.Project>();
            pjc = rg.Where(a => a.IsInnerColumn == 0).GroupBy(q => q.ProjectID).Select(
                a => new Runbow.TWS.Entity.Application.Project
                {
                    Id = a.Key.ToString(),
                    ModuleCollection = mo
                }
                ).ToList();

            Projects pojs = new Projects();
            pojs.ProjectCollection = pjc;
            lock (pojs)
            {
                CacheHelper.Remove("ApplicationConfigNewInfo" + ProjectID + (CustomerID == null ? 0 : CustomerID));
                CacheHelper.Insert("ApplicationConfigNewInfo" + ProjectID + (CustomerID == null ? 0 : CustomerID), pojs);
            }
        }

        public static DataTable GetTable_Colums(long? ProjectID, long? CustomerID, string TableName)
        {
            DataTable colums = new DataTable();
            ConfigAccessor accessor = new ConfigAccessor();
            return accessor.GetTable_Colums(ProjectID, CustomerID, TableName);
        }

        public static Projects GetApplicationConfigNew(long? ProjectID, long? CustomerID)
        {
            var projects = (Projects)CacheHelper.Get("ApplicationConfigNewInfo" + ProjectID + (CustomerID == null ? 0 : CustomerID), () =>
            {
                ConfigService service = new ConfigService();
                var rg = service.GetApplicationConfigNew(ProjectID, CustomerID).Result;

                if (rg == null)
                {

                    throw new Exception("请联系IT配置系统");
                }

                lock (lockobject)
                {
                    //&& q.ProjectID == ProjectID.Value && q.CustomerID == CustomerID.Value
                    List<Column> co = new List<Column>();
                    co = rg.Where(q => q.IsInnerColumn == 0).Select(a => new Column
                    {
                        DisplayName = a.DisplayName,
                        DbColumnName = a.DbColumnName,
                        IsKey = a.IsKey,
                        IsSearchCondition = a.IsSearchCondition,
                        IsHide = a.IsHide,
                        IsReadOnly = a.IsReadOnly,
                        Group = a.Group,
                        Type = a.Type,
                        DefaultValue = a.DefaultValue,
                        Order = a.Order,
                        IsShowInList = a.IsShowInList,
                        IsImportColumn = a.IsImportColumn,
                        SearchConditionOrder = a.SearchConditionOrder,
                        ForView = a.ForView,
                        CustomerID = a.CustomerID,
                        TableName = a.TableName,
                        InnerColumns = null
                    }).ToList();
                    List<Table> tb = new List<Table>();
                    tb = rg.Where(a => a.IsInnerColumn == 0).GroupBy(q => q.TableName).Select(a => new Table
                    {
                        Name = a.Key,
                        ColumnCollection = co.Where(b => b.TableName == a.Key).ToList()
                    }).ToList();
                    Tables tbs = new Tables();
                    tbs.TableCollection = tb;
                    List<Module> mo = new List<Module>();
                    mo = rg.Where(a => a.IsInnerColumn == 0).GroupBy(q => q.Module).Select(a => new Module
                    {
                        Id = a.Key,
                        Tables = tbs
                    }).ToList();
                    List<Runbow.TWS.Entity.Application.Project> pjc = new List<Runbow.TWS.Entity.Application.Project>();
                    pjc = rg.Where(a => a.IsInnerColumn == 0).GroupBy(q => q.ProjectID).Select(
                        a => new Runbow.TWS.Entity.Application.Project
                        {
                            Id = a.Key.ToString(),
                            ModuleCollection = mo
                        }
                        ).ToList();

                    Projects pojs = new Projects();
                    pojs.ProjectCollection = pjc;
                    CacheHelper.Insert("ApplicationConfigNewInfo" + ProjectID + (CustomerID == null ? 0 : CustomerID), pojs);
                }
            });

            return projects;
        }

        public static IEnumerable<Config> GetApplicationConfigs(string identifyType)
        {
            var ApplicationConfigs = (IEnumerable<Config>)CacheHelper.Get("ApplicationConfigs", () =>
            {
                ConfigService service = new ConfigService();
                var configs = service.GetApplicationConfigs().Result;

                if (configs == null || !configs.Any())
                {
                    throw new Exception("请联系IT配置系统");
                }

                lock (lockobject)
                {
                    CacheHelper.Insert("ApplicationConfigs", configs);
                }
            });

            return ApplicationConfigs.Where(c => string.Equals(c.IdentifyType, identifyType, StringComparison.OrdinalIgnoreCase));
        }

        public static IEnumerable<Config> GetSystemConfigs(long projectID, string identifyType)
        {
            var SystemConfigs = (IEnumerable<Config>)CacheHelper.Get("Configs", () =>
            {
                ConfigService service = new ConfigService();
                var configs = service.GetConfigs().Result;

                if (configs == null || !configs.Any())
                {
                    throw new Exception("请联系IT配置系统");
                }

                lock (lockobject)
                {
                    CacheHelper.Insert("Configs", configs);
                }
            });

            var returnConfig = SystemConfigs.Where(c => c.ProjectID == projectID && string.Equals(c.IdentifyType, identifyType, StringComparison.OrdinalIgnoreCase));

            if (returnConfig == null || !returnConfig.Any())
            {
                returnConfig = SystemConfigs.Where(c => string.Equals(c.IdentifyType, identifyType, StringComparison.OrdinalIgnoreCase) && c.AsDefault);
            }

            return returnConfig;
        }
        /// <summary>
        /// 刷新下拉列表框
        /// </summary>
        public static void RefreshGetGetWMS_UnitAndSpecifications_Config(long? ProjectID, long? CustomerID, long? WarehouseID)
        {
            ConfigService service = new ConfigService();
            var config = service.GetWMS_UnitAndSpecifications_Config(ProjectID, CustomerID, WarehouseID).Result;
            lock (lockobject)
            {
                CacheHelper.Remove("WMS_UnitAndSpecifications_Config" + ProjectID + CustomerID + WarehouseID);
                CacheHelper.Insert("WMS_UnitAndSpecifications_Config" + ProjectID + CustomerID + WarehouseID, config);
            }
        }
        public static Region GetParentRegionByChildRegion(long regionID, int TargetGrade)
        {
            var regions = GetRegions();
            var currentRegion = regions.FirstOrDefault(r => r.ID == regionID);

            if (currentRegion == null)
            {
                throw new Exception("错误的RegionID");
            }

            if (currentRegion.Grade < TargetGrade)
            {
                throw new Exception("错误的TargetGrade");
            }

            if (currentRegion.Grade == TargetGrade)
            {
                return currentRegion;
            }

            return GetParentRegionByChildRegion(currentRegion.SupperID, TargetGrade);
        }

        public static IEnumerable<Region> GetChildRegion(long supperID)
        {
            var regions = (IEnumerable<Region>)CacheHelper.Get("Regions", () =>
            {
                ConfigService service = new ConfigService();
                var rg = service.GetRegions().Result;

                if (rg == null || !rg.Any())
                {
                    throw new Exception("请联系IT配置系统");
                }

                lock (lockobject)
                {
                    CacheHelper.Insert("Regions", rg);
                }
            });

            return regions.Where(r => r.SupperID == supperID);
        }

        public static IEnumerable<Region> GetRegions()
        {
            var regions = (IEnumerable<Region>)CacheHelper.Get("Regions", () =>
            {
                ConfigService service = new ConfigService();
                var rg = service.GetRegions().Result;

                if (rg == null || !rg.Any())
                {
                    throw new Exception("请联系IT配置系统");
                }

                lock (lockobject)
                {
                    CacheHelper.Insert("Regions", rg);
                }
            });

            return regions;
        }

        public static void RefreshRegions()
        {
            ConfigService service = new ConfigService();
            var rg = service.GetRegions().Result;
            lock (lockobject)
            {
                CacheHelper.Remove("Regions");
                CacheHelper.Insert("Regions", rg);
            }
        }

        public static IEnumerable<CacheInfo> GetCacheInfo()
        {
            var regions = (IEnumerable<CacheInfo>)CacheHelper.Get("CacheInfo", () =>
            {
                ConfigService service = new ConfigService();
                var rg = service.GetCacheInfo().Result;

                //if (rg == null || !rg.Any())
                //{
                //    throw new Exception("请联系IT配置系统");
                //}

                lock (lockobject)
                {
                    CacheHelper.Insert("CacheInfo", rg);
                }
            });

            return regions;
        }
        /// <summary>
        /// 刷新缓存
        /// </summary>
        public static void RefreshCacheInfo(string key, string WarehouseName)
        {
            //ConfigService service = new ConfigService();
            //var rg = service.GetCacheInfo().Result;
            lock (lockobject)
            {
                CacheHelper.Remove(key);
                //CacheHelper.Insert(key, rg);
            }
            GetWarehouseLocationListByWarehouseName(WarehouseName);
        }
        public static void RefreshCacheInfo()
        {
            ConfigService service = new ConfigService();
            var rg = service.GetCacheInfo().Result;
            lock (lockobject)
            {
                CacheHelper.Remove("CacheInfo");
                CacheHelper.Insert("CacheInfo", rg);
            }
        }

        public static IEnumerable<GoodsShelfInfo> GetGoodsShelfInfo()
        {
            var regions = (IEnumerable<GoodsShelfInfo>)CacheHelper.Get("GoodsShelfInfo", () =>
            {
                ConfigService service = new ConfigService();
                var rg = service.GetGoodsShelfInfo().Result;

                //if (rg == null || !rg.Any())
                //{
                //    throw new Exception("请联系IT配置系统");
                //}

                lock (lockobject)
                {
                    CacheHelper.Insert("GoodsShelfInfo", rg);
                }
            });

            return regions;
        }

        public static void RefreshGoodsShelfInfoInfo()
        {
            ConfigService service = new ConfigService();
            var rg = service.GetGoodsShelfInfo().Result;
            lock (lockobject)
            {
                CacheHelper.Remove("GoodsShelfInfo");
                CacheHelper.Insert("GoodsShelfInfo", rg);
            }
        }

        public static IEnumerable<VehicleToDriver> GetCarInfo()
        {
            var regions = (IEnumerable<VehicleToDriver>)CacheHelper.Get("GetCarInfo", () =>
            {
                DriverManagementService service = new DriverManagementService();
                var rg = service.GetCarCacheInfo("");

                if (rg == null || !rg.Any())
                {

                    throw new Exception("请联系IT配置系统");
                }

                lock (lockobject)
                {
                    CacheHelper.Insert("GetCarInfo", rg);
                }
            });

            return regions;
        }

        public static void RefreshGetCarInfo()
        {
            DriverManagementService service = new DriverManagementService();
            var rg = service.GetCarCacheInfo("");
            lock (lockobject)
            {
                CacheHelper.Remove("GetCarInfo");
                CacheHelper.Insert("GetCarInfo", rg);
            }
        }

        public static IEnumerable<ProjectCustomer> GetProjectCustomers(long projectID)
        {
            var customers = (IEnumerable<ProjectCustomer>)CacheHelper.Get("Customers", () =>
            {
                CustomerService service = new CustomerService();
                var cus = service.GetProjectCustomers().Result;

                if (cus == null || !cus.Any())
                {
                    //throw new Exception("请联系IT配置系统客户");
                }
                lock (lockobject)
                {
                    CacheHelper.Insert("Customers", cus);
                }
            });

            var returnCustomers = customers.Where(c => c.ProjectID == projectID);

            if (returnCustomers == null || !returnCustomers.Any())
            {
                //throw new Exception("请联系IT配置此项目客户");
            }

            return returnCustomers;
        }

        public static IEnumerable<ProjectShipper> GetProjectShippers(long projectID)
        {
            var shippers = (IEnumerable<ProjectShipper>)CacheHelper.Get("Shippers", () =>
            {
                ShipperService service = new ShipperService();
                var ship = service.GetProjectShippers().Result;

                if (ship == null || !ship.Any())
                {
                    //throw new Exception("请联系IT配置系统承运商");
                }
                lock (lockobject)
                {
                    CacheHelper.Insert("Shippers", ship);
                }
            });

            var returnShippers = shippers.Where(s => s.ProjectID == projectID);

            if (returnShippers == null || !returnShippers.Any())
            {
                //throw new Exception("请联系IT配置此项目承运商");
            }

            return returnShippers;
        }

        public static IEnumerable<Role> GetApplicationRoles()
        {
            var shippers = (IEnumerable<Role>)CacheHelper.Get("ApplicationRoles", () =>
            {
                RoleService service = new RoleService();
                var ship = service.GetAllProjectRoles().Result;

                if (ship == null || !ship.Any())
                {
                    throw new Exception("请联系IT配置系统承运商");
                }
                lock (lockobject)
                {
                    CacheHelper.Insert("ApplicationRoles", ship);
                }
            });

            return shippers;
        }

        public static void RefreshShippers()
        {
            ShipperService service = new ShipperService();
            var ship = service.GetProjectShippers().Result;
            lock (lockobject)
            {
                CacheHelper.Remove("shippers");
                CacheHelper.Insert("Shippers", ship);
            }
        }

        public static void RefreshCustomers()
        {
            CustomerService service = new CustomerService();
            var cus = service.GetProjectCustomers().Result;
            lock (lockobject)
            {
                CacheHelper.Remove("Customers");
                CacheHelper.Insert("Customers", cus);
            }
        }

        /// <summary>
        /// 刷新角色信息
        /// </summary>
        public static void RefreshRole(int pId)
        {
            RoleService service = new RoleService();
            var role = service.GetRoleInfo(new RoleRequest() { PageSize = 15, PageIndex = 0, ProjectId = pId }).Result;
            lock (lockobject)
            {
                CacheHelper.Remove("roles");
                CacheHelper.Insert("roles", role);
            }
        }

        public static void RefreshApplicationShippers()
        {
            ShipperService service = new ShipperService();
            var ship = service.GetShippers().Result;
            lock (lockobject)
            {
                CacheHelper.Remove("ApplicationShippers");
                CacheHelper.Insert("ApplicationShippers", ship);
            }
        }

        public static void RefreshApplicationCustomers()
        {
            CustomerService service = new CustomerService();
            var cus = service.GetAllCustomers().Result;
            lock (lockobject)
            {
                CacheHelper.Remove("ApplicationCustomers");
                CacheHelper.Insert("ApplicationCustomers", cus);
            }
        }

        public static void RefreshApplicationWMS_Customers()
        {
            WMS_CustomerService service = new WMS_CustomerService();
            var cus = service.GetAllWMS_Customers().Result;
            lock (lockobject)
            {
                CacheHelper.Remove("ApplicationWMS_Customers");
                CacheHelper.Insert("ApplicationWMS_Customers", cus);
            }
        }

        public static void RefreshApplicationCustomer()
        {
            CustomerService service = new CustomerService();
            var cus = service.GetAllCustomer().Result;
            lock (lockobject)
            {
                CacheHelper.Remove("ApplicationCustomer");
                CacheHelper.Insert("ApplicationCustomer", cus);
            }
        }

        public static void RefreshProjectCustomerOrShiperSegment()
        {
            ProjectService service = new ProjectService();
            var response = service.GetAllProjectCustomerOrShipperSegments();
            if (!response.IsSuccess)
            {
                throw new Exception("获取系统数据失败");
            }
            lock (lockobject)
            {
                CacheHelper.Remove("ProjectCustomerOrShipperSegment");
                CacheHelper.Insert("ProjectCustomerOrShipperSegment", response.Result);
            }
        }

        public static IEnumerable<Customer> GetApplicationCustomers()
        {
            var customers = (IEnumerable<Customer>)CacheHelper.Get("ApplicationCustomers", () =>
            {
                CustomerService service = new CustomerService();
                var cus = service.GetAllCustomers().Result;

                if (cus == null || !cus.Any())
                {
                    throw new Exception("请联系IT配置系统客户");
                }
                lock (lockobject)
                {
                    CacheHelper.Insert("ApplicationCustomers", cus);
                }
            });

            return customers;
        }

        public static IEnumerable<Customer> GetApplicationCustomer()
        {
            var customers = (IEnumerable<Customer>)CacheHelper.Get("ApplicationCustomer", () =>
            {
                CustomerService service = new CustomerService();
                var cus = service.GetAllCustomer().Result;

                if (cus == null || !cus.Any())
                {
                    throw new Exception("请联系IT配置系统客户");
                }
                lock (lockobject)
                {
                    CacheHelper.Insert("ApplicationCustomer", cus);
                }
            });

            return customers;
        }

        public static void RefreshGetApplicationCustomer()
        {
            CustomerService service = new CustomerService();
            var cus = service.GetAllCustomer().Result;
            lock (lockobject)
            {
                CacheHelper.Remove("ApplicationCustomer");
                CacheHelper.Insert("ApplicationCustomer", cus);
            }
        }

        public static void RefreshGetApplicationWMS_Customer()
        {
            WMS_CustomerService service = new WMS_CustomerService();
            var cus = service.GetAllCustomer().Result;
            lock (lockobject)
            {
                CacheHelper.Remove("ApplicationWMS_Customer");
                CacheHelper.Insert("ApplicationWMS_Customer", cus);
            }
        }
        //用户
        public static IEnumerable<User> GetApplicationUser()
        {
            var users = (IEnumerable<User>)CacheHelper.Get("ApplicationUser", () =>
            {
                UserService service = new UserService();
                var us = service.GetAllUser().Result;

                if (us == null || !us.Any())
                {
                    throw new Exception("请联系IT配置系统客户");
                }
                lock (lockobject)
                {
                    CacheHelper.Insert("ApplicationUser", us);
                }
            });

            return users;
        }
        //项目
        public static IEnumerable<Project> GetAllProjects()
        {
            var projects = (IEnumerable<Project>)CacheHelper.Get("Project", () =>
            {
                ProjectService service = new ProjectService();
                var cus = service.GetAllProjects().Result;

                if (cus == null || !cus.Any())
                {
                    throw new Exception("请联系IT配置系统客户");
                }
                lock (lockobject)
                {
                    CacheHelper.Insert("Project", cus);
                }
            });

            return projects;
        }
        #region /***************************刷新缓存区域（wlq）********************/

        #region 刷新项目缓存
        /// <summary>
        /// 刷新项目缓存
        /// </summary>
        /// <returns></returns>
        public static void RefreshProject()
        {
            ProjectService service = new ProjectService();
            var cus = service.GetAllProjects();

            lock (lockobject)
            {
                CacheHelper.Remove("Project");
                CacheHelper.Insert("Project", cus);
            }

        }
        #endregion

        #region 刷新缓存取得所有项目角色
        /// <summary>
        /// 刷新缓存取得所有项目角色
        /// </summary>
        public static void RefreshProjectRoles()
        {
            RoleService service = new RoleService();
            var prs = service.GetAllProjectRoles().Result;
            lock (lockobject)
            {
                CacheHelper.Remove("ProjectRoles");
                CacheHelper.Insert("ProjectRoles", prs);
            }
        }
        #endregion

        #region  刷新某个项目下的角色
        /// <summary>
        /// 刷新某个项目下的角色
        /// </summary>
        /// <param name="projectID"></param>
        public static void RefreshProjectRole(GetRoleByProjectIDRequest request)
        {
            RoleService service = new RoleService();
            var prs = service.GetProjectRoles(request).Result;
            lock (lockobject)
            {
                CacheHelper.Remove("ProjectRoles");
                CacheHelper.Insert("ProjectRoles", prs);
            }
        }
        #endregion

        #region 刷新某个项目下的用户
        /// <summary>
        ///刷新某个项目下的用户
        /// </summary>
        /// <param name="request"></param>
        public static void RefreshUserByProjectId(UserRequest request)
        {
            UserService service = new UserService();
            var prs = service.GetUserByProjetId(request);
            lock (lockobject)
            {
                CacheHelper.Remove("UserByProjectId");
                CacheHelper.Insert("UserByProjectId", prs);
            }
        }
        #endregion

        #region 刷新项目下的角色用户
        /// <summary>
        /// 刷新项目下想角色用户
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public static IEnumerable<ProjectUserRole> GetProjectUserRoles(long projectID)
        {
            var projectUserRoles = (IEnumerable<ProjectUserRole>)CacheHelper.Get("ProjectUserRoles", () =>
            {
                UserService service = new UserService();
                var pur = service.GetAllProjectUserRoles().Result;

                if (pur == null || !pur.Any())
                {
                    //throw new Exception("请联系IT配置项目用户角色");
                }

                lock (lockobject)
                {
                    CacheHelper.Insert("ProjectUserRoles", pur);
                }
            });

            var returnprojectUserRoles = projectUserRoles.Where(c => c.ProjectID == projectID);

            if (returnprojectUserRoles == null || !returnprojectUserRoles.Any())
            {
                throw new Exception("请联系IT配置此项目用户角色");
            }

            return returnprojectUserRoles;
        }
        #endregion

        #region 获取某个角色的菜单
        /// <summary>
        /// 获取某个角色的菜单
        /// </summary>
        /// <param name="projectRoleID"></param>
        /// <returns></returns>
        public static IEnumerable<ProjectRoleMenu> RefreshProjectRoleMenu(long projectRoleID)
        {
            var projectRoleMenu = (IEnumerable<ProjectRoleMenu>)CacheHelper.Get("ProjectRoleMenu", () =>
            {
                RoleService service = new RoleService();
                var pur = service.GetRoleMenu(new RoleRequest { ProjectRoleID = projectRoleID }).Result;

                lock (lockobject)
                {
                    CacheHelper.Insert("ProjectRoleMenu", pur);
                }
            });

            var returnprojectRoleMenu = projectRoleMenu.Where(c => c.ProjectRoleId == projectRoleID);

            if (returnprojectRoleMenu == null || !returnprojectRoleMenu.Any())
            {
                throw new Exception("请联系IT配置此项目用户角色");
            }

            return returnprojectRoleMenu;
        }
        #endregion

        #endregion


        /// <summary>
        /// 获取不同项目的包装箱类型
        /// </summary>
        /// <param name="PorjectName">wms_config.str5</param>
        /// <returns></returns>
        public static IEnumerable<BoxSize> GetApplicationBox(string PorjectName)
        {
            var boxs = (IEnumerable<BoxSize>)CacheHelper.Get("GetApplicationBox" + PorjectName, () =>
            {
                CustomerService service = new CustomerService();

                var cus = service.GetApplicationBox().Result.Where(m => m.Str5 == PorjectName);
                if (cus.Count() <= 0)
                {
                    cus = service.GetApplicationBox().Result.Where(m => m.Str5 == "");
                }
                if (cus == null || !cus.Any())
                {
                    throw new Exception("请联系IT配置系统客户");
                }
                lock (lockobject)
                {
                    CacheHelper.Insert("GetApplicationBox" + PorjectName, cus);
                }
            });

            return boxs;
        }

        public static IEnumerable<ProjectCustomerOrShipperSegment> GetProjectCustomerOrShiperSegment()
        {
            var projectCustomerOrShipperSegment = (IEnumerable<ProjectCustomerOrShipperSegment>)CacheHelper.Get("ProjectCustomerOrShipperSegment", () =>
                {
                    ProjectService service = new ProjectService();
                    var response = service.GetAllProjectCustomerOrShipperSegments();
                    if (!response.IsSuccess)
                    {
                        throw new Exception("获取系统数据失败");
                    }

                    lock (lockobject)
                    {
                        CacheHelper.Insert("ProjectCustomerOrShipperSegment", response.Result);
                    }
                });
            return projectCustomerOrShipperSegment;
        }

        public static IEnumerable<Shipper> GetApplicationShippers()
        {
            var shippers = (IEnumerable<Shipper>)CacheHelper.Get("ApplicationShippers", () =>
            {
                ShipperService service = new ShipperService();
                var ship = service.GetShippers().Result;

                if (ship == null || !ship.Any())
                {
                    throw new Exception("请联系IT配置系统承运商");
                }
                lock (lockobject)
                {
                    CacheHelper.Insert("ApplicationShippers", ship);
                }
            });

            return shippers;
        }

        public static IEnumerable<User> GetApplicationUsers()
        {
            var appUsers = (IEnumerable<User>)CacheHelper.Get("ApplicationUsers", () =>
            {
                UserService service = new UserService();
                var users = service.GetAllUsers().Result;

                if (users == null || !users.Any())
                {
                    throw new Exception("请联系IT配置系统用户");
                }
                lock (lockobject)
                {
                    CacheHelper.Insert("ApplicationUsers", users);
                }
            });

            return appUsers;
        }

        public static void RefreshApplicationUsers()
        {
            UserService service = new UserService();
            var users = service.GetAllUsers().Result;
            lock (lockobject)
            {
                CacheHelper.Remove("ApplicationUsers");
                CacheHelper.Insert("ApplicationUsers", users);
            }
        }

        public static IEnumerable<ProjectRole> GetProjectRoles()
        {
            var projectRoles = (IEnumerable<ProjectRole>)CacheHelper.Get("ProjectRoles", () =>
            {
                RoleService service = new RoleService();
                var prs = service.GetAllProjectRoles().Result;

                if (prs == null || !prs.Any())
                {
                    throw new Exception("请联系IT配置系统项目角色");
                }
                lock (lockobject)
                {
                    CacheHelper.Insert("ProjectRoles", prs);
                }
            });

            return projectRoles;
        }

        public static IEnumerable<Role> GetRoles()
        {
            var projectRoles = (IEnumerable<Role>)CacheHelper.Get("Roles", () =>
            {
                RoleService service = new RoleService();
                var prs = service.GetRole().Result;

                if (prs == null || !prs.Any())
                {
                    throw new Exception("请联系IT配置系统项目角色");
                }
                lock (lockobject)
                {
                    CacheHelper.Insert("Roles", prs);
                }
            });

            return projectRoles;
        }

        /// <summary>
        /// 刷新项目缓存
        /// </summary>
        /// <param name="id">项目Id</param>
        public static void RefreshProject(long id)
        {
            ProjectService service = new ProjectService();
            var prs = service.GetProjectInfo(new GetProjectByProjectIdRequest { ProjectID = id }).Result;
            lock (lockobject)
            {
                CacheHelper.Remove("Project");
                CacheHelper.Insert("Project", prs);
            }
        }

        public static IEnumerable<TransportationLine> GetTransportationLine()
        {
            var transportationLines = (IEnumerable<TransportationLine>)CacheHelper.Get("TransportationLines", () =>
            {
                var trans = new TransportationLineService().GetTransportationLines();

                if (!trans.IsSuccess || trans.Result == null || !trans.Result.Any())
                {
                    throw new Exception("请先配置系统路径");
                }
                lock (lockobject)
                {
                    CacheHelper.Insert("TransportationLines", trans.Result);
                }
            });

            return transportationLines;
        }

        public static void RefreshTransportationLines()
        {
            var trans = new TransportationLineService().GetTransportationLines();
            if (trans.IsSuccess)
            {
                lock (lockobject)
                {
                    CacheHelper.Remove("TransportationLines");
                    CacheHelper.Insert("TransportationLines", trans.Result);
                }
            }
            else
            {
                throw new Exception("更新系统内存路径失败");
            }
        }
        
        //出库单管理
        public static IEnumerable<ProjectUserCustomer> GetProjectUserCustomers(long projectID, long userID)
        {
            var projectUserCustomers = (IEnumerable<ProjectUserCustomer>)CacheHelper.Get("ProjectUserCustomers", () =>
            {
                ProjectService service = new ProjectService();
                var pus = service.GetAllProjectUserCustomers().Result;
                lock (lockobject)
                {
                    CacheHelper.Insert("ProjectUserCustomers", pus);
                }
            });

            var returnprojectUserCustomers = projectUserCustomers.Where(c => c.ProjectID == projectID && c.UserID == userID);
            return returnprojectUserCustomers;
        }

        public static IEnumerable<ProjectCustomerWarehouse> GetAllProjectCustomersWarehouse(long? CustomerID)
        {
            var projectCustomersWareHouse = (IEnumerable<ProjectCustomerWarehouse>)CacheHelper.Get("ProjectCustomerWarehouse", () =>
            {
                ProjectService service = new ProjectService();
                var pus = service.GetAllProjectCustomersWarehouse().Result;
                lock (lockobject)
                {
                    CacheHelper.Insert("ProjectCustomerWarehouse", pus);
                }
            });

            var returnprojectCustomersWarehouse = projectCustomersWareHouse.Where(c => c.CustomerID == CustomerID);
            return returnprojectCustomersWarehouse;
        }

        public static IEnumerable<ProjectCustomerWarehouse> GetAllProjectCustomersWarehouse()
        {
            var projectCustomersWareHouse = (IEnumerable<ProjectCustomerWarehouse>)CacheHelper.Get("ProjectCustomerWarehouse", () =>
            {
                ProjectService service = new ProjectService();
                var pus = service.GetAllProjectCustomersWarehouse().Result;
                lock (lockobject)
                {
                    CacheHelper.Insert("ProjectCustomerWarehouse", pus);
                }
            });

            var returnprojectCustomersWarehouse = projectCustomersWareHouse;
            return returnprojectCustomersWarehouse;
        }
        //库区
        public static IEnumerable<AreaInfo> GetAllProjectCustomersWarehouse_Area()
        {
            var projectCustomersWareHouse = (IEnumerable<AreaInfo>)CacheHelper.Get("AreaInfo", () =>
            {
                ProjectService service = new ProjectService();
                var pus = service.GetAllProjectCustomersWarehouse_Area().Result;
                lock (lockobject)
                {
                    CacheHelper.Insert("AreaInfo", pus);
                }
            });
            var returnprojectCustomersWarehouse = projectCustomersWareHouse;
            return returnprojectCustomersWarehouse;
        }

        //用户与库位关联
        public static IEnumerable<WMS_User_Area_Mapping> GetAllUser_Area()
        {
            var projectCustomersWareHouse = (IEnumerable<WMS_User_Area_Mapping>)CacheHelper.Get("User_AreaInfo", () =>
            {
                ProjectService service = new ProjectService();
                var pus = service.GetAllUser_Area().Result;
                lock (lockobject)
                {
                    CacheHelper.Insert("User_AreaInfo", pus);
                }
            });
            var returnprojectCustomersWarehouse = projectCustomersWareHouse;
            return returnprojectCustomersWarehouse;
        }

        //库位
        public static IEnumerable<LocationInfo> GetAllProjectCustomersWarehouse_Location()
        {
            var projectCustomersWareHouse = (IEnumerable<LocationInfo>)CacheHelper.Get("LocationInfo", () =>
            {
                ProjectService service = new ProjectService();
                var pus = service.GetAllProjectCustomersWarehouse_Location().Result;
                lock (lockobject)
                {
                    CacheHelper.Insert("LocationInfo", pus);
                }
            });

            var returnprojectCustomersWarehouse = projectCustomersWareHouse;
            return returnprojectCustomersWarehouse;
        }
        //库位
        public static IEnumerable<LocationInfo> GetAllProjectCustomersWarehouse_Location(long Customer)
        {
            var projectCustomersWareHouse = (IEnumerable<LocationInfo>)CacheHelper.Get("LocationInfo_" + Customer.ToString(), () =>
            {
                ProjectService service = new ProjectService();
                var pus = service.GetAllProjectCustomersWarehouse_Location(Customer).Result;
                lock (lockobject)
                {
                    CacheHelper.Insert("LocationInfo_" + Customer.ToString(), pus);
                }
            });

            var returnprojectCustomersWarehouse = projectCustomersWareHouse;
            return returnprojectCustomersWarehouse;
        }

        public static void RefreshProjectUserCustomers()
        {
            ProjectService service = new ProjectService();
            var pus = service.GetAllProjectUserCustomers().Result;
            lock (lockobject)
            {
                CacheHelper.Remove("ProjectUserCustomers");
                CacheHelper.Insert("ProjectUserCustomers", pus);
            }
        }

        public static void RefreshProjectCustomersWarehouse()
        {
            ProjectService service = new ProjectService();
            var pus = service.GetAllProjectCustomersWarehouse().Result;
            lock (lockobject)
            {
                CacheHelper.Remove("ProjectCustomerWarehouse");
                CacheHelper.Insert("ProjectCustomerWarehouse", pus);
            }
        }

        /// <summary>
        /// 油价
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public static IEnumerable<QueryBAFPrice> GetBAFPrice(long projectID)
        {
            var projectQuotedPrice = (IEnumerable<QueryBAFPrice>)CacheHelper.Get("GetBAFPrice", () =>
            {
                QuotedPriceService service = new QuotedPriceService();
                var allBAFPrice = service.GetBAFQuotedPrice().Result;
                if (allBAFPrice == null || !allBAFPrice.Any())
                {
                    throw new Exception("请配置系统报价");
                }

                lock (lockobject)
                {
                    CacheHelper.Insert("GetBAFPrice", allBAFPrice);
                }
            });

            var returnProjectQuotedPrice = projectQuotedPrice.Where(p => p.ProjectID == projectID);

            if (returnProjectQuotedPrice == null || !returnProjectQuotedPrice.Any())
            {
                throw new Exception("请先配置项目报价");
            }

            return returnProjectQuotedPrice;
        }

        //change by cyf
        public static IEnumerable<QuotedPrice> GetProjectQuotedPrice(long projectID, int target, long targetID, long relatedCustomerID)
        {
            string cacheQuotedPriceKey = Constants.GenApplicationQuotedPriceKey(projectID, target, targetID, relatedCustomerID);

            var projectQuotedPrice = (IEnumerable<QuotedPrice>)CacheHelper.Get(cacheQuotedPriceKey, () =>
            {
                QuotedPriceService service = new QuotedPriceService();
                var allQuotedPrice = service.GetQuotedPrice(projectID, target, targetID, relatedCustomerID).Result;

                lock (lockobject)
                {
                    CacheHelper.Insert(cacheQuotedPriceKey, allQuotedPrice);
                }
            });

            return projectQuotedPrice;

        }

        public static void RefreshProjectQuotedPrice(long projectID, int target, long targetID, long relatedCustomerID)
        {
            string key = Constants.GenApplicationQuotedPriceKey(projectID, target, targetID, relatedCustomerID);

            QuotedPriceService service = new QuotedPriceService();
            var allQuotedPrice = service.GetQuotedPrice(projectID, target, targetID, relatedCustomerID).Result;

            lock (lockobject)
            {
                CacheHelper.Remove(key);
                CacheHelper.Insert(key, allQuotedPrice);
            }
        }

        public static IEnumerable<QuotedPrice> GetProjectQuotedPrice(long projectID)
        {
            var projectQuotedPrice = (IEnumerable<QuotedPrice>)CacheHelper.Get("ProjectQuotedPrices", () =>
                {
                    QuotedPriceService service = new QuotedPriceService();
                    var allQuotedPrice = service.GetAllQuotedPrice().Result;
                    if (allQuotedPrice == null || !allQuotedPrice.Any())
                    {
                        throw new Exception("请配置系统报价");
                    }

                    lock (lockobject)
                    {
                        CacheHelper.Insert("ProjectQuotedPrices", allQuotedPrice);
                    }
                });

            var returnProjectQuotedPrice = projectQuotedPrice.Where(p => p.ProjectID == projectID);

            if (returnProjectQuotedPrice == null || !returnProjectQuotedPrice.Any())
            {
                throw new Exception("请先配置项目报价");
            }

            return returnProjectQuotedPrice;
        }

        public static ServicePeriod GetSerivePeriod(long projectID, long customerID, long startCityID, long endCityID)
        {
            var servicePeriods = (IEnumerable<ServicePeriod>)CacheHelper.Get("ServicePeriods", () =>
                {
                    HiltiService service = new HiltiService();
                    var response = service.GetServicePeriod();

                    lock (lockobject)
                    {
                        CacheHelper.Insert("ServicePeriods", response.Result);
                    }
                });

            if (servicePeriods != null && servicePeriods.Any())
            {
                return servicePeriods.FirstOrDefault(s => s.ProjectID == projectID && s.CustomerID == customerID && s.StartCityID == startCityID && s.EndCityID == endCityID);
            }

            return null;
        }

        public static void RefreshProjectQuotedPrice()
        {

            QuotedPriceService service = new QuotedPriceService();
            var allQuotedPrice = service.GetAllQuotedPrice().Result;
            lock (lockobject)
            {
                CacheHelper.Remove("ProjectQuotedPrices");
                CacheHelper.Insert("ProjectQuotedPrices", allQuotedPrice);
            }
        }

        public static void RefreshProjectUserRole()
        {
            UserService service = new UserService();
            var pur = service.GetAllProjectUserRoles().Result;
            lock (lockobject)
            {
                CacheHelper.Remove("ProjectUserRoles");
                CacheHelper.Insert("ProjectUserRoles", pur);
            }
        }

        /// <summary>
        /// 获取WMS中DropDownList配置缓存
        /// </summary>
        public static IEnumerable<WMSConfig> GetWMS_Config(string types)
        {
            var configs = (IEnumerable<WMSConfig>)CacheHelper.Get("WMSConfig", () =>
            {
                ConfigService service = new ConfigService();
                var config = service.GetWMSConfig().Result;

                if (config == null || !config.Any())
                {
                    throw new Exception("请联系IT配置系统缓存");
                }
                lock (lockobject)
                {
                    CacheHelper.Insert("WMSConfig", config);
                }
            });

            var returnConfig = configs.Where(s => s.Type == types);

            if (returnConfig == null || !returnConfig.Any())
            {
                throw new Exception("请联系IT配置系统缓存");
            }

            return returnConfig;
        }

        /// <summary>
        /// 获取WMS中存储过程 配置缓存
        /// </summary>
        public static IEnumerable<WMS_Config_Type> GetWMS_ConfigType(string types, long ProjectID, long CustomerID, long WarehouseID)
        {
            var configs = (IEnumerable<WMS_Config_Type>)CacheHelper.Get("WMSConfigType", () =>
            {
                ConfigService service = new ConfigService();
                var config = service.GetWMSConfigType().Result;

                if (config == null || !config.Any())
                {
                    throw new Exception("请联系IT配置系统缓存");
                }
                lock (lockobject)
                {
                    CacheHelper.Insert("WMSConfigType", config);
                }
            });
            var allConfig = configs.Where(s => s.Type == types);
            var returnConfig = allConfig.Where(s => s.ProjectID == 0 && s.CustomerID == 0 && s.WarehouseID == 0);//默认配置

            if (allConfig.Where(s => s.ProjectID == ProjectID && s.CustomerID == 0 && s.WarehouseID == 0).Count() > 0)//按照projectid取配置
                returnConfig = allConfig.Where(s => s.ProjectID == ProjectID && s.CustomerID == 0 && s.WarehouseID == 0);

            if (allConfig.Where(s => s.ProjectID == ProjectID && s.CustomerID == CustomerID && s.WarehouseID == 0).Count() > 0)//按照projectid和customerid取配置
                returnConfig = allConfig.Where(s => s.ProjectID == ProjectID && s.CustomerID == CustomerID && s.WarehouseID == 0);

            if (allConfig.Where(s => s.ProjectID == ProjectID && s.CustomerID == CustomerID && s.WarehouseID == WarehouseID).Count() > 0)//按照projectid、customerid、warehouseid取配置
                returnConfig = allConfig.Where(s => s.ProjectID == ProjectID && s.CustomerID == CustomerID && s.WarehouseID == WarehouseID);


            if (returnConfig == null || !returnConfig.Any())
            {
                throw new Exception("请联系IT配置系统缓存");
            }

            return returnConfig;
        }

        public static IEnumerable<WMS_UnitAndSpecifications_Config> GetWMS_UnitAndSpecifications_Config(long? ProjectID, long? CustomerID, long? WarehouseID)
        {
            var configs = (IEnumerable<WMS_UnitAndSpecifications_Config>)CacheHelper.Get("WMS_UnitAndSpecifications_Config" + ProjectID + CustomerID + WarehouseID, () =>
            {
                ConfigService service = new ConfigService();
                var config = service.GetWMS_UnitAndSpecifications_Config(ProjectID, CustomerID, WarehouseID).Result;

                if (config == null || !config.Any())
                {
                    throw new Exception("请联系IT配置系统缓存");
                }
                lock (lockobject)
                {
                    CacheHelper.Insert("WMS_UnitAndSpecifications_Config" + ProjectID + CustomerID + WarehouseID, config);
                }
            });

            var returnConfig = configs;

            if (returnConfig == null || !returnConfig.Any())
            {
                throw new Exception("请联系IT配置系统缓存");
            }

            return returnConfig;
        }

        /// <summary>
        ///获取货主
        /// </summary>
        public static IEnumerable<Customer> GetStorerID(long id)
        {
            var configs = (IEnumerable<Customer>)CacheHelper.Get("GetCustomerID", () =>
            {
                ConfigService service = new ConfigService();
                var config = service.GetStorerID().Result;

                if (config == null || !config.Any())
                {
                    new List<Customer>();  //为空
                }
                lock (lockobject)
                {
                    CacheHelper.Insert("GetCustomerID", config);
                }
            });

            var returnConfig = configs.Where(s => s.ID == id);

            if (returnConfig == null || !returnConfig.Any())
            {
                new List<Customer>();  //为空
            }

            return returnConfig;
        }

        public static IEnumerable<ProductSearch> GetSearchProduct(long CustomerID, List<ProductSearch> ProductSearch, string type)
        {
            ProductService service = new ProductService();
            var config = service.GetSearchProduct(CustomerID, ProductSearch, type);

            if (config == null || !config.Any())
            {
                new List<ProductSearch>();  //为空
            }
            return config;
        }

        public static IEnumerable<ProductSearch> GetSearchProductYXDR(long CustomerID, List<ProductSearch> ProductSearch, string type)
        {
            ProductService service = new ProductService();
            var config = service.GetSearchProductYXDR(CustomerID, ProductSearch, type);

            if (config == null || !config.Any())
            {
                new List<ProductSearch>();  //为空
            }
            return config;
        }

        public static string GetQueryDetail(string p, string querykey)
        {
            try
            {
                OrderManagementService cls = new OrderManagementService();
                return cls.GetQueryDetail(p, querykey);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static IEnumerable<ArticleSearch> GetSearchActicle(long CustomerID, List<ArticleSearch> ArticleSearch, string type)
        {
            ProductService service = new ProductService();
            var config = service.GetSearchArticle(CustomerID, ArticleSearch, type);

            if (config == null || !config.Any())
            {
                new List<ArticleSearch>();  //为空
            }
            return config;
        }

        /// <summary>
        /// 获取仓库下拉列表数据
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<WarehouseInfo> GetWarehouseList()
        {

            var list = (IEnumerable<WarehouseInfo>)CacheHelper.Get("WarehouseList", () =>
            {
                WarehouseService service = new WarehouseService();
                var WarehouseList = service.GetWarehouseList().Result;

                if (WarehouseList == null || !WarehouseList.Any())
                {
                    new List<WarehouseInfo>();  //为空
                }
                lock (lockobject)
                {
                    CacheHelper.Insert("WarehouseList", WarehouseList);
                }
            });

            return list;
        }

        //public static void RefreshGetWarehouseList()
        //{
        //    WarehouseService service = new WarehouseService();
        //    var WarehouseList = service.GetWarehouseList().Result;
        //    lock (lockobject)
        //    {
        //        CacheHelper.Remove("WarehouseList");
        //        CacheHelper.Insert("WarehouseList", WarehouseList);
        //    }
        //}
        public static IEnumerable<OrderInfo> GetProvinceList()
        {
            var list = (IEnumerable<OrderInfo>)CacheHelper.Get("ProvinceList", () =>
            {
                OrderManagementService service = new OrderManagementService();
                var ProvinceList = service.GetProvinceList().Result;

                if (ProvinceList == null || !ProvinceList.Any())
                {
                    new List<OrderInfo>();  //为空
                }
                lock (lockobject)
                {
                    CacheHelper.Insert("ProvinceList", ProvinceList);
                }
            });

            return list;
        }

        public static IEnumerable<OrderInfo> GetCityList(string province)
        {
            var list = (IEnumerable<OrderInfo>)CacheHelper.Get("GetCityList", () =>
            {
                OrderManagementService service = new OrderManagementService();
                var GetCityList = service.GetCityList(province).Result;

                if (GetCityList == null || !GetCityList.Any())
                {
                    new List<OrderInfo>();  //为空
                }
                lock (lockobject)
                {
                    CacheHelper.Insert("GetCityList", GetCityList);
                }
            });

            return list;
        }

        public static IEnumerable<OrderInfo> GetDistrictList(string city)
        {
            var list = (IEnumerable<OrderInfo>)CacheHelper.Get("GetDistrictList", () =>
            {
                OrderManagementService service = new OrderManagementService();
                var GetDistrictList = service.GetDistrictList(city).Result;

                if (GetDistrictList == null || !GetDistrictList.Any())
                {
                    new List<OrderInfo>();  //为空
                }
                lock (lockobject)
                {
                    CacheHelper.Insert("GetDistrictList", GetDistrictList);
                }
            });

            return list;
        }

        public static void RefreshGetProvinceList()
        {
            OrderManagementService service = new OrderManagementService();
            var ProvinceList = service.GetProvinceList().Result;
            lock (lockobject)
            {
                CacheHelper.Remove("ProvinceList");
                CacheHelper.Insert("ProvinceList", ProvinceList);
            }
        }

        public static void RefreshGetCityList(string province)
        {
            OrderManagementService service = new OrderManagementService();
            var GetCityList = service.GetCityList(province).Result;
            lock (lockobject)
            {
                CacheHelper.Remove("GetCityList");
                CacheHelper.Insert("GetCityList", GetCityList);
            }
        }

        public static void RefreshGetDistrictList(string city)
        {
            OrderManagementService service = new OrderManagementService();
            var GetDistrictList = service.GetDistrictList(city).Result;
            lock (lockobject)
            {
                CacheHelper.Remove("GetDistrictList");
                CacheHelper.Insert("GetDistrictList", GetDistrictList);
            }
        }

        /// <summary>
        /// 刷新仓库下拉列表
        /// </summary>
        public static void RefreshGetWarehouseList()
        {
            WarehouseService service = new WarehouseService();
            var WarehouseList = service.GetWarehouseList().Result;
            lock (lockobject)
            {
                CacheHelper.Remove("WarehouseList");
                CacheHelper.Insert("WarehouseList", WarehouseList);
            }
        }

        /// <summary>
        /// 根据CustomerID获取仓库 hzf
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<WarehouseInfo> GetWarehouseListByCustomer(long CustomerID)
        {
            long a = 0;
            var list = (IEnumerable<WarehouseInfo>)CacheHelper.Get("WarehouseListByCustomerID", () =>
            {
                WarehouseService service = new WarehouseService();
                var AreaList = service.GetWarehouseListByCustomerID(a).Result;

                if (AreaList == null || !AreaList.Any())
                {
                    new List<WarehouseInfo>();
                }
                lock (lockobject)
                {
                    CacheHelper.Insert("WarehouseListByCustomerID", AreaList);
                }
            });
            if (CustomerID == 0)
            {
                return list;
            }
            else
            {
                var returnConfig = list.Where(s => s.CustomerID == CustomerID);
                if (returnConfig == null || !returnConfig.Any())
                {
                    new List<WarehouseInfo>();  //为空
                }

                return returnConfig;
            }
        }
         /// <summary>
        /// 根据仓库ID获取库区 实时
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<AreaInfo> GetWarehouseAreaListByWID(long WarehouseID)
        {
            WarehouseService service = new WarehouseService();
            var AreaList = service.GetWarehouseAreaList(WarehouseID).Result;

            if (AreaList == null || !AreaList.Any())
            {
                new List<AreaInfo>();
            }
            return AreaList;
        }
        /// <summary>
        /// 根据仓库ID获取库区 缓存
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<AreaInfo> GetWarehouseAreaList(long WarehouseID)
        {
            long a = 0;
            var list = (IEnumerable<AreaInfo>)CacheHelper.Get("WarehouseAreaList", () =>
            {
                WarehouseService service = new WarehouseService();
                var AreaList = service.GetWarehouseAreaList(a).Result;

                if (AreaList == null || !AreaList.Any())
                {
                    new List<AreaInfo>();
                }
                lock (lockobject)
                {
                    //CacheHelper.Insert("WarehouseAreaList", AreaList);
                    CacheHelper.Insert("WarehouseAreaList", AreaList, null, DateTime.Now.AddSeconds(20), TimeSpan.Zero);
                }
            });
            if (WarehouseID == 0)
            {
                return list;
            }
            else
            {
                var returnConfig = list.Where(s => s.WarehouseID == WarehouseID);
                if (returnConfig == null || !returnConfig.Any())
                {
                    new List<AreaInfo>();  //为空
                }

                return returnConfig;
            }
        }

        /// <summary>
        /// 根据仓库Name获取库区
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<AreaInfo> GetWarehouseAreaListByWarehouseName(string WarehouseName)
        {
            var list = (IEnumerable<AreaInfo>)CacheHelper.Get("WarehouseAreaList", () =>
            {
                WarehouseService service = new WarehouseService();
                var AreaList = service.GetWarehouseAreaListByWarehouseName(WarehouseName).Result;

                if (AreaList == null || !AreaList.Any())
                {
                    new List<AreaInfo>();
                }
                lock (lockobject)
                {
                    CacheHelper.Insert("WarehouseAreaList", AreaList);
                }
            });
            if (WarehouseName == "")
            {
                return list;
            }
            else
            {
                var returnConfig = list.Where(s => s.WarehouseName == WarehouseName);
                if (returnConfig == null || !returnConfig.Any())
                {
                    new List<AreaInfo>();  //为空
                }

                return returnConfig;
            }
        }

        public static IEnumerable<GoodsShelfInfo> GetGoodsShelfList(long project, long CustomerID, long WarehouseID)
        {

            var list = (IEnumerable<GoodsShelfInfo>)CacheHelper.Get("GoodsShelfList", () =>
            {
                WarehouseService service = new WarehouseService();
                var GoodsShelfList = service.GetGoodsShelfList(project, CustomerID, WarehouseID).Result;

                if (GoodsShelfList == null || !GoodsShelfList.Any())
                {
                    new List<GoodsShelfInfo>();
                }
                lock (lockobject)
                {
                    CacheHelper.Insert("GoodsShelfList", GoodsShelfList);
                }
            });
            return list;
        }

        public static void RefreshGetGoodsShelfList(long project, long CustomerID, long WarehouseID)
        {
            long a = 0;
            WarehouseService service = new WarehouseService();
            var GoodsShelfList = service.GetGoodsShelfList(project, CustomerID, WarehouseID).Result;
            lock (lockobject)
            {
                CacheHelper.Remove("GoodsShelfList");
                CacheHelper.Insert("GoodsShelfList", GoodsShelfList);
            }
        }

        /// <summary>
        /// 根据仓库ID获取库区
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<LocationInfo> GetWarehouseLocationList()//long WarehouseID, string WarehouseName
        {
            long a = 0;
            var list = (IEnumerable<LocationInfo>)CacheHelper.Get("WarehouseLocationList", () =>
            {
                WarehouseService service = new WarehouseService();
                var AreaList = service.GetWarehouseLocationList(a).Result;

                if (AreaList == null || !AreaList.Any())
                {
                    new List<LocationInfo>();
                }
                lock (lockobject)
                {
                    CacheHelper.Insert("WarehouseLocationList", AreaList);
                }
            });
            if (list == null || !list.Any())
            {
                new List<LocationInfo>();  //为空
            }

            return list;

        }

        /// <summary>
        /// 根据仓库ID获取库位
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<LocationInfo> GetWarehouseLocationListByWarehouseName(string WarehouseName)//long WarehouseID, string WarehouseName
        {
            long a = 0;
            var list = (IEnumerable<LocationInfo>)CacheHelper.Get("WarehouseLocationList_" + WarehouseName, () =>
            {
                WarehouseService service = new WarehouseService();
                var AreaList = service.GetWarehouseLocationListByWarehouseName(WarehouseName).Result;

                if (AreaList == null || !AreaList.Any())
                {
                    new List<LocationInfo>();
                }
                lock (lockobject)
                {
                    CacheHelper.Insert("WarehouseLocationList_" + WarehouseName, AreaList);
                }
            });
            if (list == null || !list.Any())
            {
                new List<LocationInfo>();  //为空
            }

            return list;

        }
        /// <summary>
        /// 根据仓库ID获取库区
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<LocationInfo> GetWarehouseLocationList(long WarehouseID)//long WarehouseID, string WarehouseName
        {
            //long a = 0;
            var list = (IEnumerable<LocationInfo>)CacheHelper.Get("WarehouseLocationList" + WarehouseID, () =>
            {
                WarehouseService service = new WarehouseService();
                var AreaList = service.GetWarehouseLocationList(WarehouseID).Result;

                if (AreaList == null || !AreaList.Any())
                {
                    new List<LocationInfo>();
                }
                lock (lockobject)
                {
                    CacheHelper.Insert("WarehouseLocationList" + WarehouseID, AreaList);
                }
            });
            if (list == null || !list.Any())
            {
                new List<LocationInfo>();  //为空
            }

            return list;

        }

        public static void RefreshGetWarehouseLocationList(long WarehouseID)
        {
            long a = 0;
            WarehouseService service = new WarehouseService();
            var AreaList = service.GetWarehouseLocationList(WarehouseID).Result;
            lock (lockobject)
            {
                CacheHelper.Remove("WarehouseLocationList" + WarehouseID);
                CacheHelper.Insert("WarehouseLocationList" + WarehouseID, AreaList);
            }
        }
        //库位货架对应关系
        public static IEnumerable<LocationInfo> GetLocationGoodShelfList(long WarehouseID)//long WarehouseID, string WarehouseName
        {
            //long a = 0;
            var list = (IEnumerable<LocationInfo>)CacheHelper.Get("LocationGoodShelfList" + WarehouseID, () =>
            {
                WarehouseService service = new WarehouseService();
                var Lists = service.GetLocationGoodShelfList(WarehouseID).Result;

                if (Lists == null || !Lists.Any())
                {
                    new List<LocationInfo>();
                }
                lock (lockobject)
                {
                    CacheHelper.Insert("LocationGoodShelfList" + WarehouseID, Lists);
                }
            });
            if (list == null || !list.Any())
            {
                new List<LocationInfo>();  //为空
            }

            return list;

        }

        public static void RefreshGetLocationGoodShelfList(long WarehouseID)
        {

            WarehouseService service = new WarehouseService();
            var Lists = service.GetLocationGoodShelfList(WarehouseID).Result;
            lock (lockobject)
            {
                CacheHelper.Remove("LocationGoodShelfList" + WarehouseID);
                CacheHelper.Insert("LocationGoodShelfList" + WarehouseID, Lists);
            }
        }

        /// <summary>
        /// 将ProductStorer表缓存起来
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        //public static IEnumerable<ProductStorer> GetProductList()//long ID
        //{
        //    string SqlWhere = "";
        //    var list = (IEnumerable<ProductStorer>)CacheHelper.Get("ProductStorerList", () =>
        //    {
        //        ProductService service = new ProductService();
        //        var AreaList = service.GetProductStorerList(SqlWhere).Result;

        //        if (AreaList == null || !AreaList.Any())
        //        {
        //            new List<ProductStorer>();
        //        }
        //        lock (lockobject)
        //        {
        //            CacheHelper.Insert("ProductStorerList", AreaList);
        //        }
        //    });
        //    return list;
        //}

        /// <summary>
        /// 将ProductStorer表缓存起来
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static IEnumerable<ProductStorer> GetProductStorerList(long? CustomerID)
        {
            string SqlWhere = " and p.StorerID=" + CustomerID.Value;
            var list = (IEnumerable<ProductStorer>)CacheHelper.Get("ProductStorerList" + CustomerID.Value, () =>
            {
                ProductService service = new ProductService();
                var AreaList = service.GetProductStorerList(SqlWhere).Result;

                if (AreaList == null || !AreaList.Any())
                {
                    new List<ProductStorer>();
                }
                lock (lockobject)
                {
                    CacheHelper.Insert("ProductStorerList" + CustomerID.Value, AreaList);
                }
            });
            return list;
        }

        public static IEnumerable<ProductStorer> GetSKUListBySKU(long Customer, string SKU)
        {
            ProductService service = new ProductService();
            var AreaList = service.GetSKUListBySKU(Customer, SKU).Result;

            if (AreaList == null || !AreaList.Any())
            {
                new List<ProductStorer>();
            }
            return AreaList;
        }

        /// <summary>
        /// 刷新产品表
        /// </summary>
        public static void RefreshGetProductStorerList(string CustomerID)
        {
            string SqlWhere = " and p.StorerID=" + CustomerID;
            ProductService service = new ProductService();
            var AreaList = service.GetProductStorerList(CustomerID).Result;
            lock (lockobject)
            {
                CacheHelper.Remove("ProductStorerList" + CustomerID);
                CacheHelper.Insert("ProductStorerList" + CustomerID, AreaList);
            }
        }

        //将库位库区缓存起来
        //public static IEnumerable<LocationInfo> GetLocationList()
        //{
        //    var list = (IEnumerable<LocationInfo>)CacheHelper.Get("GetLocationList", () =>
        //    {
        //        AdjustmentManagementService service = new AdjustmentManagementService();
        //        var AreaList = service.GetLocationList().Result;

        //        if (AreaList == null || !AreaList.Any())
        //        {
        //            new List<LocationInfo>();
        //        }
        //        lock (lockobject)
        //        {
        //            CacheHelper.Insert("GetLocationList", AreaList);
        //        }
        //    });
        //    return list;
        //}

        //将sku 数量 描述 品名缓存起来
        //public static IEnumerable<Inventorys> GetInventoryLocationList(string location)
        //{
        //    var list = (IEnumerable<Inventorys>)CacheHelper.Get("GetInventoryLocationList", () =>
        //    {
        //        AdjustmentManagementService service = new AdjustmentManagementService();
        //        var AreaList = service.GetInventoryLocationList(location).Result;

        //        if (AreaList == null || !AreaList.Any())
        //        {
        //            new List<Inventorys>();
        //        }
        //        lock (lockobject)
        //        {
        //            CacheHelper.Insert("GetInventoryLocationList", AreaList);
        //        }
        //    });
        //    return list;
        //}
        public static void RefreshInventoryLocationList(string location)
        {
            AdjustmentManagementService service = new AdjustmentManagementService();
            var WarehouseList = service.GetInventoryLocationList(location, "", "","").Result;
            lock (lockobject)
            {
                CacheHelper.Remove("GetInventoryLocationList");
                CacheHelper.Insert("GetInventoryLocationList", WarehouseList);
            }
        }

        public static IEnumerable<ProductStorer> GetALLProductStorerList(string CustomerID)
        {
            var list = (IEnumerable<ProductStorer>)CacheHelper.Get("ProductStorerList" + CustomerID, () =>
            {
                ProductService service = new ProductService();
                var AreaList = service.GetProductStorerList(CustomerID).Result;

                if (AreaList == null || !AreaList.Any())
                {
                    new List<ProductStorer>();
                }
                lock (lockobject)
                {
                    CacheHelper.Insert("ProductStorerList" + CustomerID, AreaList);
                }
            });
            return list;
        }

        public static IEnumerable<ProductStorer> GetALLProductByCustomerIDList(long CustomerID)
        {
            var list = (IEnumerable<ProductStorer>)CacheHelper.Get(CustomerID + "ProductList", () =>
            {
                ProductService service = new ProductService();
                var AreaList = service.GetProductByCustomerIDList(CustomerID).Result;

                if (AreaList == null || !AreaList.Any())
                {
                    new List<ProductStorer>();
                }
                lock (lockobject)
                {
                    CacheHelper.Insert(CustomerID + "ProductList", AreaList);
                }
            });
            return list;
        }

        ///// <summary>
        ///// 刷新仓库下拉列表
        ///// </summary>
        ///// <param name="CustomerID">客户ID</param>
        //public static void RefreshGetWarehouseListByCustomerID(long CustomerID)
        //{
        //    WarehouseService service = new WarehouseService();
        //    var AreaList = service.GetWarehouseListByCustomerID(CustomerID).Result;
        //    lock (lockobject)
        //    {
        //        CacheHelper.Remove("WarehouseListByCustomerID");
        //        CacheHelper.Insert("WarehouseListByCustomerID", AreaList);
        //    }
        //}

        /// <summary>
        /// 刷新仓库库区下拉列表
        /// </summary>
        /// <param name="WarehouseID"></param>
        public static void RefreshGetWarehouseAreaList(long WarehouseID)
        {
            WarehouseService service = new WarehouseService();
            var AreaList = service.GetWarehouseAreaList(WarehouseID).Result;
            lock (lockobject)
            {
                CacheHelper.Remove("WarehouseAreaList");
                CacheHelper.Insert("WarehouseAreaList", AreaList);
            }
        }

        /// <summary>
        /// /// <summary>
        /// 刷新WMS中DropDownList配置缓存
        /// </summary>
        /// </summary>
        public static void RefreshWMS_Config()
        {
            ConfigService service = new ConfigService();
            var config = service.GetWMSConfig().Result;
            lock (lockobject)
            {
                CacheHelper.Remove("WMSConfig");
                CacheHelper.Insert("WMSConfig", config);
            }
        }
        public static void RefreshWarehouseAllocate()
        {
            WarehouseService service = new WarehouseService();
            var pur = service.GetAllWarehouseAllocate().Result;
            lock (lockobject)
            {
                CacheHelper.Remove("GetAllWarehouseAllocate");
                CacheHelper.Insert("GetAllWarehouseAllocate", pur);
            }
        }

        /// <summary>
        /// 通过ID查询到承运商
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<CRMShipper> GetShipperList()
        {

            var list = (IEnumerable<CRMShipper>)CacheHelper.Get("ShipperList", () =>
            {
                ShipperManagementService service = new ShipperManagementService();
                var ShipperList = service.GetShipperList().Result;

                if (ShipperList == null || !ShipperList.Any())
                {
                    new List<CRMShipper>();  //为空
                }
                lock (lockobject)
                {
                    CacheHelper.Insert("ShipperList", ShipperList);
                }
            });

            return list;
        }

        /// <summary>
        /// 通过ID查询到wms_customer
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<WMS_Customer> GetAllCustomerByID(long CustomerID)
        {

            ConfigService service = new ConfigService();
            var CustomerList = service.GetAllCustomer(CustomerID).Result;

            if (CustomerList == null || !CustomerList.Any())
            {
                return new List<WMS_Customer>();  //为空
            }


            return CustomerList;
        }

        /// <summary>
        /// 查询已选择车辆下拉列表数据
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<CRMVehicle> GetVehicleList()
        {

            var list = (IEnumerable<CRMVehicle>)CacheHelper.Get("VehicleList", () =>
            {
                VehicleManagementService service = new VehicleManagementService();
                var VehicleList = service.GetVehicleList().Result;

                if (VehicleList == null || !VehicleList.Any())
                {
                    new List<CRMVehicle>();  //为空
                }
                lock (lockobject)
                {
                    CacheHelper.Insert("VehicleList", VehicleList);
                }
            });

            return list;
        }

        /// <summary>
        /// 所有的司机
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<CRMDriver> GetDriverList()
        {

            var list = (IEnumerable<CRMDriver>)CacheHelper.Get("DriverList", () =>
            {
                DriverManagementService service = new DriverManagementService();
                var DriverList = service.GetDriverList().Result;

                if (DriverList == null || !DriverList.Any())
                {
                    new List<CRMDriver>();  //为空
                }
                lock (lockobject)
                {
                    CacheHelper.Insert("DriverList", DriverList);
                }
            });

            return list;
        }

        public static IEnumerable<ProductStorer> GetSKUlist()
        {
            var list = (IEnumerable<ProductStorer>)CacheHelper.Get("goodsnameandgoodtypeList", () =>
            {
                ProductService service = new ProductService();
                var goodsnameandgoodtypeList = service.QuerySKUProduct(new GetProductByConditionRequest()).Result;

                if (goodsnameandgoodtypeList == null)
                {
                    new List<ProductStorer>();
                }
                lock (lockobject)
                {
                    CacheHelper.Insert("goodsnameandgoodtypeList", goodsnameandgoodtypeList);
                }
            });
            return list;
        }

        //得到ASN表缓存起来
        public static IEnumerable<ASN> GetASNInfo()
        {
            var list = (IEnumerable<ASN>)CacheHelper.Get("ASNList", () =>
            {
                ASNManagementService service = new ASNManagementService();
                var ASNList = service.GetASNInfo().Result;

                if (ASNList == null || !ASNList.Any())
                {
                    new List<WarehouseInfo>();  //为空
                }
                lock (lockobject)
                {
                    CacheHelper.Insert("ASNList", ASNList);
                }
            });
            return list;
        }

        public static void RefreshASNInfo()
        {
            ASNManagementService service = new ASNManagementService();
            var ASNList = service.GetASNInfo().Result;
            lock (ASNList)
            {
                CacheHelper.Remove("ASNList");
                CacheHelper.Insert("ASNList", ASNList);
            }
        }

        public static void RefreshUserWarehouseMapping()
        {
        }

        /// <summary>
        /// 根据仓库名称查询库区信息（库区变更）
        /// </summary>
        /// <param name="WarehouseName"></param>
        /// <returns></returns>
        public static IEnumerable<LocationInfo> ALGetWarehouseLocationListByWarehouseName(string WarehouseName)
        {
            long a = 0;
            var list = (IEnumerable<LocationInfo>)CacheHelper.Get("WarehouseLocationList_" + WarehouseName, () =>
            {
                WarehouseService service = new WarehouseService();
                var AreaList = service.ALGetWarehouseLocationListByWarehouseName(WarehouseName).Result;
                if (AreaList == null || !AreaList.Any())
                {
                    new List<LocationInfo>();
                }
                lock (lockobject)
                {
                    CacheHelper.Insert("WarehouseLocationList_" + WarehouseName, AreaList);
                }
            });
            if (list == null || !list.Any())
            {
                new List<LocationInfo>();  //为空
            }

            return list;

        }

        

    }
}