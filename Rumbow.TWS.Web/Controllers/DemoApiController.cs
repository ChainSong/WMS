using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;

namespace Runbow.TWS.Web.Controllers
{
    //This is a Demo
    public class DemoApiController : ApiController
    {
        public List<DemoUsers> GetUsers()
        {
            var userList = new List<DemoUsers>();
            userList.Add(new DemoUsers() { ID = 1, Name = "Aden" });
            userList.Add(new DemoUsers() { ID = 2, Name = "Yunyun" });

            return userList;
        }

        public List<DemoUsers> GetUsers(int id)
        {
            var userList = new List<DemoUsers>();
            userList.Add(new DemoUsers() { ID = 1, Name = "Aden" });
            return userList;
        }

    }

    //DemoModel
    public class DemoUsers
    {
        public int ID { get; set; }

        public string Name { get; set; }

    }
}
