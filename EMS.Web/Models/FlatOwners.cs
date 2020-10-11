using EMS.Common;
using MicroORM;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace EMS.Web.Models
{
    public class FlatOwners
    {
        public FlatOwnerModel GetItem(int id)
        {
            FlatOwnerModel bll = new FlatOwnerModel();
            if (id == 0)
            {
                bll = new FlatOwnerModel();
            }
            else
            {
                NpgsqlParameter[] parameters = { new NpgsqlParameter("pseqid", id) };

                DataTable DT = DataAccess.ExecuteParaQuery(GetQuery.FLAT_OWNER_DATA_BY_SEQID, parameters);
                if (DT.Rows.Count > 0)
                {
                    List<FlatOwnerModel> userlist = QueryHandler.GetFlatOwnersGridList(DT);
                    if (userlist.Count > 0)
                        bll = userlist[0];
                }
            }
            return bll;
        }
        public List<FlatOwnerModel> LIST_DATA()
        {
            List<FlatOwnerModel> listmodel = new List<FlatOwnerModel>();

            //NpgsqlParameter[] parameters = { new NpgsqlParameter("@seqid", "") };

            DataTable DT = DataAccess.ExecuteParaQuery(GetQuery.FLAT_OWNER_GRID_DATA, null);
            if (DT.Rows.Count > 0)
            {
                listmodel = QueryHandler.GetFlatOwnersGridData(DT);

            }
            return listmodel;
        }
        public string Save(FlatOwnerModel model)
        {
            string Response = string.Empty;
            List<FlatOwnerModel> bll = new List<FlatOwnerModel>();
            DbCommonHelper dbcom = new DbCommonHelper();

            NpgsqlParameter[] Insert_Parameters = {
                                             new NpgsqlParameter("pid",model.seqid),
                                             new NpgsqlParameter("pname",string.IsNullOrEmpty(model.name)?"":model.name),
                                             new NpgsqlParameter("pmobile1",string.IsNullOrEmpty(model.mobile1)?"":model.mobile1),
                                             new NpgsqlParameter("pmobile2",string.IsNullOrEmpty(model.mobile2)?"":model.mobile2),
                                             new NpgsqlParameter("pemail",string.IsNullOrEmpty(model.email)?"":model.email),

                                             new NpgsqlParameter("prenter_seqid",model.renter_seqid),
                                             new NpgsqlParameter("prenter_name",string.IsNullOrEmpty(model.renter_name)?"":model.renter_name),
                                             new NpgsqlParameter("prenter_mobile1",string.IsNullOrEmpty(model.renter_mobile1)?"":model.renter_mobile1),
                                             new NpgsqlParameter("prenter_mobile2",string.IsNullOrEmpty(model.renter_mobile2)?"":model.renter_mobile2),
                                             new NpgsqlParameter("prenter_email",string.IsNullOrEmpty(model.email)?"":model.renter_email),

                                             new NpgsqlParameter("pdeposit",model.deposit),
                                             new NpgsqlParameter("pmonthly_rent",model.monthly_rent),

                                             new NpgsqlParameter("pproperty_type_id",model.property_type_id),
                                             new NpgsqlParameter("plocation_id",model.location_id),
                                             new NpgsqlParameter("pbuilding_id",model.building_id),
                                             new NpgsqlParameter("pflat_no",string.IsNullOrEmpty(model.flat_no)?"":model.flat_no),

                                              new NpgsqlParameter("pduration_from",Convert.ToDateTime(model.duration_from.ToString("yyyy-MM-dd"))),
                                             new NpgsqlParameter("pduration_to",Convert.ToDateTime(model.duration_to.ToString("yyyy-MM-dd"))),
                                              new NpgsqlParameter("premark",string.IsNullOrEmpty(model.remark) ? "":model.remark),
                                              new NpgsqlParameter("plogin_user",Helper.GetUserID()),
                                              new NpgsqlParameter("pcreated_by",model.created_by)
                                             

            };
            //Response = dbcom.ExecuteProcedure(@"select
            //                                       ems_fun_owner_renter_crud_activity
            //                                       (
            //                                           :pid,:pname,:pmobile1,:pmobile2,:pemail,:prenter_seqid,:prenter_name,:prenter_mobile1,:prenter_mobile2,:prenter_email,:pdeposit
            //                                          ,:pmonthly_rent,:pproperty_type_id,:plocation_id,:pbuilding_id,:pflat_no,:pduration_from,:pduration_to,:plogin_user,:premark,:pcreated_by
            //                                       )", Insert_Parameters);

            Response = dbcom.ExecuteProcedure("select ems_fun_owner_renter_crud_activity(:pid,:pname,:pmobile1,:pmobile2,:pemail,:prenter_seqid,:prenter_name,:prenter_mobile1,:prenter_mobile2,:prenter_email,:pdeposit,:pmonthly_rent,:pproperty_type_id,:plocation_id,:pbuilding_id,:pflat_no,:pduration_from,:pduration_to,:premark,:plogin_user,:pcreated_by)", Insert_Parameters);
            return Response;
        }
    }
}