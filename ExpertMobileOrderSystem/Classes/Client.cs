using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ExpertMobileOrderSystem
{
    public class Client
    {
        public Client() { }
        public Client(DataRow dr)
        {
            AccountExpiredOn = Convert.ToDateTime(dr["AccountExpiredOn"]);
            Id = Convert.ToInt32(dr["Id"]);
            CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
            CompanyAddress = dr["CompanyAddress"].ToString();
            CompanyName = dr["CompanyName"].ToString();
            CreatedAdminID = Convert.ToInt32(dr["CreatedAdminID"]);
            Email = dr["Email"].ToString();
            FirstName = dr["FirstName"].ToString();
            LastName = dr["LastName"].ToString();
            MobileNo = dr["MobileNo"].ToString();
            NoOfAccessUsers = Convert.ToInt32(dr["NoOfAccessUsers"]);
            NoOfCompanyPerUser = Convert.ToInt32(dr["NoOfCompanyPerUser"]);
            NoOfDays = Convert.ToInt32(dr["NoOfDays"]);
            Password = dr["Password"].ToString();
            UserName = dr["UserName"].ToString();
            TotalCreatedUser = Convert.ToInt32(dr["TotalCreatedUser"]);
            TotalCreatedCompany = Convert.ToInt32(dr["TotalCreatedCompany"]);
            QueryRights = Convert.ToBoolean(dr["QueryRights"]);
            IsWithout = Convert.ToBoolean(dr["IsWithout"]);
        }
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string MobileNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public int NoOfDays { get; set; }
        public int NoOfCompanyPerUser { get; set; }
        public int NoOfAccessUsers { get; set; }
        public int TotalCreatedUser { get; set; }
        public int TotalCreatedCompany { get; set; }
        public DateTime AccountExpiredOn { get; set; }
        public string IsUploadingProcessStart { get; set; }
        public int CreatedAdminID { get; set; }
        public string ClientMastercol { get; set; }
        public bool QueryRights { get; set; }
        public string DataUploadedOn { get; set; }
        public int CreatedDistributorId { get; set; }
        public bool IsWithout { get; set; }
        private DataRowCollection _billableCompany;
        private DataRow _withoutCompany;
        public DataRowCollection BillableCompanies
        {
            get
            {
                if (_billableCompany == null)
                {
                    var dt = Operation.GetDataTable("select * from [Order.ClientCompanyMaster] where ClientId=" + Id + " and IsWithout=0 order by [Order.ClientCompanyMaster].IsDefault desc", Operation.Conn);
                    if (dt != null && dt.Rows.Count > 0)
                        _billableCompany = dt.Rows;
                }
                return _billableCompany;
            }
            set
            {
                _billableCompany = value;
            }
        }

        public DataRow WithoutCompany
        {
            get
            {
                if (_withoutCompany == null)
                {
                    var dt = Operation.GetDataTable("select * from [Order.ClientCompanyMaster] where ClientId=" + Id + " and IsWithout=1", Operation.Conn);
                    if (dt != null && dt.Rows.Count > 0)
                        _withoutCompany = dt.Rows[0];
                }
                return _withoutCompany;
            }
            set
            {
                _withoutCompany = value;
            }
        }
    }
}
