using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Common
{
    public class GetQuery
    {
        //Query for user screen
        public static string USER_INSERT_QUERY
        {
            get
            {
                return @"insert into ems_tbl_user_master(employee_code,employee_name,email_address,role_id,is_active,created_date)
                                                    values(@employee_code, @employee_name, @email_address, @role_id, @is_active, @created_date)";
            }
        }
        public static string ROLE_RECORD_UPDATE_QUERY
        {
            get
            {
                return @"update ems_tbl_role_master 
                        set role_name=@role_name,is_active=@is_active where seqid=@seqid";
            }
        }
        public static string USER_RECORD_UPDATION_QUERY
        {
            get
            {
                return @"update ems_tbl_user_master 
                          set employee_name=@employee_name,email_address=@email_address,role_id=@role_id where seqid=@seqid";
            }
        }
        public static string MENU_LIST_DATA
        {
            get
            {
                return @"select * from ems_tbl_menu_master where is_active=1";
            }
        }
        public static string ROLE_DATA_SEQID
        {
            get
            {
                return @"select * from ems_tbl_role_master where seqid=@seqid";
            }
        }
        public static string LOV_CATEGORY_DATA_ID
        {
            get
            {
                return @"select * from ems_tbl_lov_category where id=@id";
            }
        }
        public static string LOV_DATA_ID
        {
            get
            {
                return @"select * from ems_tbl_lov where id=@id";
            }
        }
        public static string GET_ASSIGNED_MENU_TO_ROLE
        {
            get
            {
                return @"select * from ems_tbl_role_menu_mapping where  is_active=true and role_id=@role_id ";
            }
        }
        public static string DELETE_OLD_MENU_ROLE_MAPPING_QUERY
        {
            get
            {
                return @"delete from ems_tbl_role_menu_mapping where role_id=@role_id";
            }
        }
        public static string MENU_ROLE_MAPPING_INSERT_QUERY
        {
            get
            {
                return @"insert into ems_tbl_role_menu_mapping(menu_id,role_id,created_date,is_active)
                                                    values(@menu_id,@role_id,@created_date,@is_active)";
            }
        }
        public static string ENQUIRY_INSERT_QUERY
        {
            get
            {
                return @"insert into ems_tbl_enquiry_trans
                        (
                            name, mobile1, mobile2, email, location_id, cost_upto, property_type_id, enquiry_source_id,
                             remark, created_by, created_date, enquiry_date
                        )
                        values
                        (
                            @name, @mobile1, @mobile2, @email, @location_id, @cost_upto, @property_type_id, @enquiry_source_id,
                             @remark, @created_by, @created_date, @enquiry_date
                        )";
            }
        }
        public static string ENQUIRY_DATA_ENTRY_UPDATE_QUERY
        {
            get
            {
                return @"update ems_tbl_enquiry_trans set
                            name=@name, mobile1=@mobile1, mobile2=@mobile2,
							email=@email, location_id=@location_id, cost_upto=@cost_upto,
							property_type_id=@property_type_id, enquiry_source_id=@enquiry_source_id,
                             remark=@remark, enquiry_date=@enquiry_date where seqid=@seqid ";
            }
        }
        public static string FLAT_OWNER_INSERT_QUERY
        {
            get
            {
                return @"insert into ems_tbl_flatowners
                        (
                            name, mobile1, mobile2, email, location_id, deposit,monthly_rent, property_type_id,
                             remark, created_by, created_date
                        )
                        values
                        (
                            @name, @mobile1, @mobile2, @email, @location_id, @deposit,@monthly_rent,@property_type_id,
                             @remark, @created_by, @created_date
                        )";
            }
        }

        public static string FLATE_OWNER_UPDATE_QUERY
        {
            get
            {
                return @"update ems_tbl_flatowners
                        set
                            name=@name, mobile1=@mobile1, mobile2=@mobile2, email=@email, location_id=@location_id
							, deposit=@deposit,monthly_rent=@monthly_rent, property_type_id=@property_type_id,
                             remark=@remark where seqid=@seqid ";
            }
        }
        public static string ASSIGN_ENQUIRY_TO_AGENT
        {
            get
            {
                return @"update ems_tbl_enquiry_trans set assign_to_id=@assign_to_id,assign_to_date=@assign_to_date where seqid=@seqid";
            }
        }
        public static string ADD_ENQUIRY_CALL_UP_COMMENTS
        {
            get
            {
                return @"insert into ems_tbl_enquiry_status
                                    (enquiry_no,action_taken_by,action_type_id,remark,action_date,created_by,created_date)
	                        values(@enquiry_no,@action_taken_by,@action_type_id,@remark,@action_date,@created_by,@created_date)";
            }
        }

        public static string ENQUIRY_RECORD_CHECK_QUERY
        {
            get
            {
                return @"select * from ems_tbl_enquiry_trans where seqid=@seqid";
            }
        }
        public static string FLAT_OWNER_RECORD_CHECK_QUERY
        {
            get
            {
                return @"select * from ems_tbl_flatowners where seqid=@seqid";
            }
        }

        public static string USER_RECORD_CHECK_QUERY
        {
            get
            {
                return @"select * from ems_tbl_user_master where employee_code=@employee_code";
            }
        }



        // Query for role screen
        public static string ROLE_INSERT_QUERY
        {
            get
            {
                return @"insert into ems_tbl_role_master(role_code,role_name,is_active,created_date)
                                                    values(@role_code, @role_name, @is_active, @created_date)";
            }
        }
        public static string ROLE_RECORD_CHECK_QUERY
        {
            get
            {
                return @"select * from ems_tbl_role_master where role_code=@role_code";
            }
        }

        public static string DDL_GET_PROPERTY_TYPES
        {
            get
            {
                return @"select lov.id,lov.name  from ems_tbl_lov_category lc
                            inner join ems_tbl_lov lov on lov.category_id =lc.id  where lc.code='#PRPTTY#' and lc.is_active=true";
            }
        }
        public static string DDL_GET_LOV_CATEGORY
        {
            get
            {
                return @"select id,name from ems_tbl_lov_category where is_active=true";
            }
        }
        public static string DDL_GET_ACTION_TYPES
        {
            get
            {
                return @"select lov.id,lov.name  from ems_tbl_lov_category lc
                            inner join ems_tbl_lov lov on lov.category_id =lc.id  where lc.code='#ACTTYPE#' and lc.is_active=true";
            }
        }
        public static string DDL_GET_ENQUIRY_SOURCES
        {
            get
            {
                return @"select lov.id,lov.name from ems_tbl_lov_category lc
                         inner join ems_tbl_lov lov on lov.category_id =lc.id where lc.code='#ENQSRC#' and lc.is_active=true";
            }
        }

        public static string DDL_GET_LOCATIONS
        {
            get
            {
                return @"select seqid as id,name from ems_tbl_location_master where is_active=true";
            }
        }
        public static string DDL_GET_RECORD_STATUS
        {
            get
            {
                return @"select 'Select' as claim_status 
                                                        union all
                                               select distinct claim_status from ems_tbl_record_waiting_for_payment";
            }
        }

        public static string DDL_GET_MENUS
        {
            get
            {
                return @"select 
                                                      mm2.seqid
                                                	  ,mm2.link
                                                	  ,mm2.title
                                                	  ,mm2.css_class
                                                	  ,mm2.is_active 
                                                from ems_tbl_user_master um
                                                inner join ems_tbl_role_master rm on um.role_id = rm.seqid
                                                inner join ems_tbl_role_menu_mapping mm on mm.role_id =rm.seqid
                                                inner join ems_tbl_menu_master mm2 on mm2.seqid=mm.menu_id
                                                where mm2.is_active=1 and um.employee_code=@userid
												order by menu_order asc";
            }
        }

        public static string DDL_GET_ROLES
        {
            get
            {
                return @"select seqid as id,role_name as name from ems_tbl_role_master where is_active=true";
            }
        }
        public static string DDL_GET_USERS
        {
            get
            {
                return @"select seqid as id,employee_name as name from ems_tbl_user_master where is_active=true";
            }
        }
        public static string DDL_GET_EMPLOYEES
        {
            get
            {
                return @"select distinct emp_code,emp_name from ems_tbl_record_waiting_for_payment where lower(emp_name) like lower('%@emp_name%')";
            }
        }
        public static string ROLE_GRID_DATA
        {
            get
            {
                return @"select * from ems_tbl_role_master";
            }
        }
        public static string LOVCATEGORY_GRID_DATA
        {
            get
            {
                return @"select * from ems_tbl_lov_category";
            }
        }
        public static string LOV_GRID_DATA
        {
            get
            {
                return @"select 
                              lov.id,lov.name,lov.code,lovc.name category,lov.category_id,lov.is_active 
                         from ems_tbl_lov lov
                        left join ems_tbl_lov_category lovc on lovc.id=lov.category_id";
            }
        }

        public static string USER_GRID_DATA
        {
            get
            {
                return @"select * from ems_tbl_user_master";
            }
        }

        public static string USER_DATA_BY_ID
        {
            get
            {
                return @"select * from ems_tbl_user_master where seqid=@seqid";
            }
        }

        public static string ENQUIRY_GRID_DATA
        {
            get
            {
                return @"select * from ems_tbl_enquiry_trans order by 1 desc";
            }
        }
        public static string FLAT_OWNER_GRID_DATA
        {
            get
            {
                return @"select * from ems_tbl_flatowners order by 1 desc";
            }
        }
        public static string ENQUIRY_DATA_BY_SEQID
        {
            get
            {
                return @"select * from ems_tbl_enquiry_trans where seqid=@seqid";
            }
        }
        public static string FLAT_OWNER_DATA_BY_SEQID
        {
            get
            {
                return @"select * from ems_fn_get_owner_renter_detail(:pseqid)";
            }
        }
        public static string QUERY_MODULE_GRID_DATA
        {
            get
            {
                return @"select 
                                  et.seqid,et.name Customer,et.enquiry_date,lv.name propertytype,et.mobile1
                                  ,et.mobile2,et.email,lm.name as location,et.cost_upto
								  ,(select remark from ems_tbl_enquiry_status where enquiry_no=et.seqid order by 1 desc limit 1) lastremark
								  ,(select action_type_id from ems_tbl_enquiry_status where enquiry_no=et.seqid order by 1 desc limit 1) action_type_id
                            from ems_tbl_enquiry_trans et
                            left join ems_tbl_enquiry_status es on es.enquiry_no=et.seqid
                            left join ems_tbl_location_master lm on lm.seqid=et.location_id
                            left join ems_tbl_lov lv on lv.id=et.property_type_id
                            left join ems_tbl_lov_category lc on lc.id=lv.category_id and lc.code='#PRPTTY#'
                          where 1=1  ";
            }
        }
        public static string QUERY_MODULE_GRID_GROUP_BY
        {
            get
            {
                return @"group by 
                                    et.seqid, et.name, et.enquiry_date, lv.name, et.mobile1
                                    , et.mobile2, et.email, lm.name, et.cost_upto";
            }
        }

        public static string CALL_HISTRY_CUSTOMER_GRID
        {
            get
            {
                return @"select 
                                enquiry_no
                        	   ,es.action_date as call_date
                        	   ,es.remark as call_response 
                        	   ,lov.name as action_type
                        	   ,um.employee_name
                        from ems_tbl_enquiry_status es
                        inner join ems_tbl_enquiry_trans et on et.seqid=es.enquiry_no
                        inner join ems_tbl_lov lov on lov.id=es.action_type_id
                        inner join ems_tbl_lov_category lc on lc.id=lov.category_id and lc.code='#ACTTYPE#'
                        inner join ems_tbl_user_master um on um.employee_code=es.created_by
                        where es.enquiry_no=@enquiry_no ";
            }
        }

    }
}
