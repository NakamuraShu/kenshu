using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using ユーザー管理画面.Models;

namespace ユーザー管理画面.Controllers
{
    public class UserManagementController : Controller
    {
        public ActionResult Index(UserManageConditionModel condition)
        {
            UserManagementModel UserManageModel = new UserManagementModel();

            UserManageModel.List(condition);

            Session["UserSerch"] = UserManageModel;
            return View("UserManage", UserManageModel);
        }


        [HttpPost]
        public ActionResult GetPage(int rowCount, int pageNum,string 表示区分)
        {
            UserManagementModel userManagement = (UserManagementModel)Session["UserSerch"];

            if (!string.IsNullOrEmpty(表示区分))
            {
                userManagement.表示区分 = 表示区分;
            }
            userManagement.GetPage(rowCount, pageNum);
            return PartialView("_UserManageList", userManagement);
        }


        [HttpPost]
        public ActionResult Find(UserManageConditionModel condition)
        {
            UserManagementModel userManagement = new UserManagementModel();

            if (Session["UserSerch"] != null)
            {
                userManagement = (UserManagementModel)Session["UserSerch"];
            }
            else
            {
                userManagement.ソート順 = "▲";
                userManagement.ソート列 = "氏名";
            }

            userManagement.List(condition);
            Session["UserSerch"] = userManagement; 

            return PartialView("_UserManageList", userManagement);
        }

        [HttpPost]
        public ActionResult Sort(string colName, string sortOrder)
        {
            UserManagementModel UserManageModel = (UserManagementModel)Session["UserSerch"];
            UserManageModel.Sort(colName, sortOrder);
            return PartialView("_UserManageList", UserManageModel);
        }
        public ActionResult Delete_User(UserManageConditionModel condition, UserManagementModel model)
        {
            UserManagementModel UserManageModel = (UserManagementModel)Session["UserSerch"];

            UserManageModel.選択一覧 = model.選択一覧;

            UserManageModel.DeleteUser(condition);

            Session["UserSerch"] = UserManageModel;
            return PartialView("_UserManageList", UserManageModel);
        }

        public ActionResult Insert_User(UserManageConditionModel condition, UserManagementModel model)
        {
            UserManagementModel UserManageModel = (UserManagementModel)Session["UserSerch"];

            UserManageModel.選択一覧 = model.選択一覧;

            UserManageModel.InsertUser(condition);

            Session["UserSerch"] = UserManageModel;
            return PartialView("_UserManageList", UserManageModel);
        }
    }
}