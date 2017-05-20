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
            TotalCreatedUser = (string.IsNullOrEmpty(Convert.ToString(dr["TotalCreatedUser"])) ? 0 : Convert.ToInt32(dr["TotalCreatedUser"]));
            TotalCreatedCompany = (string.IsNullOrEmpty(Convert.ToString(dr["TotalCreatedCompany"])) ? 0 : Convert.ToInt32(dr["TotalCreatedCompany"]));
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
        private DataRowCollection _companies;
        private DataRow _withoutCompany;
        private DataRow _defaultCompany;
        public DataRowCollection Companies
        {
            get
            {
                if (_companies == null)
                {
                    var dt = Operation.GetDataTable("select * from [Order.ClientCompanyMaster] where ClientId=" + Id + " order by [Order.ClientCompanyMaster].IsDefault desc", Operation.Conn);
                    if (dt != null && dt.Rows.Count > 0)
                        _companies = dt.Rows;
                }
                return _companies;
            }
        }
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
        public DataRow DefaultCompany
        {
            get
            {
                if (_defaultCompany == null)
                {
                    var dt = Operation.GetDataTable("select * from [Order.ClientCompanyMaster] where ClientId=" + Id + " and IsDefault=1", Operation.Conn);
                    if (dt != null && dt.Rows.Count > 0)
                        _defaultCompany = dt.Rows[0];
                }
                return _defaultCompany;
            }
            set
            {
                _defaultCompany = value;
            }
        }

        public void Refresh()
        {
            var dt = Operation.GetDataTable("select * from [Order.ClientMaster] where ClientId=" + Id, Operation.Conn);
            if (dt != null && dt.Rows.Count > 0)
            {
                AccountExpiredOn = Convert.ToDateTime(dt.Rows[0]["AccountExpiredOn"]);
                Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                CreatedDate = Convert.ToDateTime(dt.Rows[0]["CreatedDate"]);
                CompanyAddress = dt.Rows[0]["CompanyAddress"].ToString();
                CompanyName = dt.Rows[0]["CompanyName"].ToString();
                CreatedAdminID = Convert.ToInt32(dt.Rows[0]["CreatedAdminID"]);
                Email = dt.Rows[0]["Email"].ToString();
                FirstName = dt.Rows[0]["FirstName"].ToString();
                LastName = dt.Rows[0]["LastName"].ToString();
                MobileNo = dt.Rows[0]["MobileNo"].ToString();
                NoOfAccessUsers = Convert.ToInt32(dt.Rows[0]["NoOfAccessUsers"]);
                NoOfCompanyPerUser = Convert.ToInt32(dt.Rows[0]["NoOfCompanyPerUser"]);
                NoOfDays = Convert.ToInt32(dt.Rows[0]["NoOfDays"]);
                Password = dt.Rows[0]["Password"].ToString();
                UserName = dt.Rows[0]["UserName"].ToString();
                TotalCreatedUser = (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["TotalCreatedUser"])) ? 0 : Convert.ToInt32(dt.Rows[0]["TotalCreatedUser"]));
                TotalCreatedCompany = (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["TotalCreatedCompany"])) ? 0 : Convert.ToInt32(dt.Rows[0]["TotalCreatedCompany"]));
                QueryRights = Convert.ToBoolean(dt.Rows[0]["QueryRights"]);
                IsWithout = Convert.ToBoolean(dt.Rows[0]["IsWithout"]);
            }
        }
    }
}
