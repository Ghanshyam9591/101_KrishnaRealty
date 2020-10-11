using MicroORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using EMS.Common;
using Npgsql;

namespace EMS.Web.Models
{
    public class Common2 : EntityBusinessLogicBase<Common2>
    {
        DbCommonHelper bll = new DbCommonHelper();
        List<DDLModel> ListModel = new List<DDLModel>();
        public List<DDLModel> GetPropertyTypes()
        {
            ListModel = new List<DDLModel>();
            DDLModel ddlobj = new DDLModel();
            ListModel = bll.FILE_DROPDOWN_LIST(GetQuery.DDL_GET_PROPERTY_TYPES, null);
            return ListModel;
        }
        public List<DDLModel> GetEnquiryTypes()
        {
            ListModel = new List<DDLModel>();
            DDLModel ddlobj = new DDLModel();
            ListModel = bll.FILE_DROPDOWN_LIST(GetQuery.DDL_GET_ENQUIRY_TYPES, null);
            return ListModel;
        }
        public List<DDLModel> GetLovCategory()
        {
            ListModel = new List<DDLModel>();
            DDLModel ddlobj = new DDLModel();
            ListModel = bll.FILE_DROPDOWN_LIST(GetQuery.DDL_GET_LOV_CATEGORY, null);
            return ListModel;
        }
        public List<DDLModel> GetActionTypes()
        {
            ListModel = new List<DDLModel>();
            DDLModel ddlobj = new DDLModel();
            ListModel = bll.FILE_DROPDOWN_LIST(GetQuery.DDL_GET_ACTION_TYPES, null);
            return ListModel;
        }
        
        public List<DDLModel> GetEnquirySouces()// approved/reject
        {
            ListModel = new List<DDLModel>();
            DDLModel ddlobj = new DDLModel();
            ListModel = bll.FILE_DROPDOWN_LIST(GetQuery.DDL_GET_ENQUIRY_SOURCES, null);
            return ListModel;
        }
        public List<DDLModel> GetLocations()// approved/reject
        {
            ListModel = new List<DDLModel>();
            DDLModel ddlobj = new DDLModel();
            ListModel = bll.FILE_DROPDOWN_LIST(GetQuery.DDL_GET_LOCATIONS, null);
            return ListModel;
        }
        public List<MenusModel> GetMenus()// approved/reject
        {
            List<MenusModel> menulist = new List<MenusModel>();


            NpgsqlParameter[] param = {
                new NpgsqlParameter("@userid",Helper.GetUserID())

            };
            DDLModel ddlobj = new DDLModel();
            DataTable dt = bll.ExecuteParaQuery(GetQuery.DDL_GET_MENUS, param);

            Helper.WriteLog("menu Count : " + dt.Rows.Count);

            foreach (DataRow drow in dt.Rows)
            {
                MenusModel mmobj = new MenusModel();

                mmobj.SeqID = string.IsNullOrEmpty(drow["seqid"].ToString()) ? 0 : Convert.ToInt32(drow["seqid"].ToString());
                mmobj.Link = string.IsNullOrEmpty(drow["link"].ToString()) ? "" : drow["link"].ToString();
                mmobj.Title = string.IsNullOrEmpty(drow["title"].ToString()) ? "" : drow["title"].ToString();
                mmobj.css_class = string.IsNullOrEmpty(drow["css_class"].ToString()) ? "" : drow["css_class"].ToString();
                mmobj.is_active = string.IsNullOrEmpty(drow["is_active"].ToString()) ? 0 : Convert.ToInt32(drow["is_active"].ToString());
                menulist.Add(mmobj);

            }
            return menulist;
        }

        public List<DDLModel> GetRoles()
        {
            ListModel = new List<DDLModel>();
            DDLModel ddlobj = new DDLModel();
            ListModel = bll.FILE_DROPDOWN_LIST(GetQuery.DDL_GET_ROLES, null);
            return ListModel;
        }
        public List<DDLModel> GetUsers()
        {
            ListModel = new List<DDLModel>();
            DDLModel ddlobj = new DDLModel();
            ListModel = bll.FILE_DROPDOWN_LIST(GetQuery.DDL_GET_USERS, null);
            return ListModel;
        }
        public List<DDLModel> GetBuildings()
        {
            ListModel = new List<DDLModel>();
            DDLModel ddlobj = new DDLModel();
            ListModel = bll.FILE_DROPDOWN_LIST(GetQuery.DDL_GET_BUILDINGS, null);
            return ListModel;
        }
        public List<DDLModel2> GetCustomers(string q)
        {
            NpgsqlParameter[] param = {
                new NpgsqlParameter("@emp_name",q)
            };
            List<DDLModel2> ListModel2 = new List<DDLModel2>();
            DDLModel2 ddlobj = new DDLModel2();
            ListModel2 = bll.FEEL_DROPDOWNLIST_FOR_EMP("select distinct seqid, name from ems_tbl_enquiry_trans where lower(name) like lower('%" + q + "%')", "name", null);
            return ListModel2;
        }

        public List<DDLModel2> GetOwners(string q)
        {
            NpgsqlParameter[] param = {
                new NpgsqlParameter("@emp_name",q)
            };
            List<DDLModel2> ListModel2 = new List<DDLModel2>();
            DDLModel2 ddlobj = new DDLModel2();
            ListModel2 = bll.FEEL_DROPDOWNLIST_FOR_EMP("select distinct seqid, name from ems_tbl_flatowners where lower(name) like lower('%" + q + "%')", "name", null);
            return ListModel2;
        }

        public List<DDLModel2> GetRenters(string q)
        {
            NpgsqlParameter[] param = {
                new NpgsqlParameter("@emp_name",q)
            };
            List<DDLModel2> ListModel2 = new List<DDLModel2>();
            DDLModel2 ddlobj = new DDLModel2();
            ListModel2 = bll.FEEL_DROPDOWNLIST_FOR_EMP("select distinct seqid, name from rms_tbl_renter_trans where lower(name) like lower('%" + q + "%')", "name", null);
            return ListModel2;
        }

    }
}