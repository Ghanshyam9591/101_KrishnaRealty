using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMS.Web.Models;
using EMS.Common;
using ClosedXML.Excel;
using System.IO;
using Npgsql;
using System.Text;
using System.Data;
using MicroORM;

namespace EMS.Web.Controllers
{
    [CustomAuthorize]
    public class EnquiryController : Controller
    {
        // GET: Enquiry
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetGridDetail(DateTime AsOnDate,int location_id)
        {
            Equiry bll = new Equiry();
            return new JsonNetResult(bll.GetGridDetail(AsOnDate, location_id));
        }
        //public ActionResult AssignBatchNumber(List<EnquiryModel> model)
        //{
        //    Equiry bll = new Equiry();
        //    return new JsonNetResult(bll.AssignBatchNumber(model));
        //}
        public ActionResult GetExportToExcel(ExportParaModel epmodel)
        {
            NpgsqlParameter[] parameters = {
                new NpgsqlParameter("@activity_date",epmodel.AsOnDate.ToString("yyyy-MM-dd"))

            };
            DataTable DT = new DataTable("Grid");
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@"SELECT activity_number, activity_name, activity_creation_date, emp_code, emp_name, activity_date, meeting_duration
                                                , mode_of_activity, mode_of_conveyance, claim_conveyance_expense, conveyance_expense_amount
                                                , claim_con_exp_for_ret_trip, return_trip_amount, additional_disributors, product_type, supervisor_id
                                                , supervisor_name, supervisor_approval_date, claim_amt, partner_code, partner_name, emp_ufc_code, emp_bank_acc_number
                                                , activity_description, claim_type, approved_rejected, batch_number, batch_number_date, data_upload_date, created_date, runtime_status
                                    FROM ems_tbl_record_waiting_for_payment
                                    where batch_number = 0 and activity_date <='" + epmodel.AsOnDate.ToString("yyyy-MM-dd") + "'");

            if (!string.IsNullOrWhiteSpace(epmodel.ClaimType))
            {
                sb.AppendLine("and claim_type='" + epmodel.ClaimType + "'");
            }
            DT = DataAccess.ExecuteQuery(sb.ToString());

            using (XLWorkbook wb = new XLWorkbook())
            {
                try
                {
                    wb.Worksheets.Add(DT, "sheet1");
                }
                catch (Exception exx)
                {
                    Helper.WriteLog("error export to excel :" + exx);
                }
                using (MemoryStream stream = new MemoryStream())
                {

                    wb.SaveAs(stream);

                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", DateTime.Now.ToString("dd_MM_yyyy") + "_data.xlsx");
                    //return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileNm + ".xlsx");
                }
            }
        }
    }
}