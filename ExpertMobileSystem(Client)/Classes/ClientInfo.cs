using System;
using System.Collections.Generic;

using System.Text;

namespace ExpertMobileSystem_Client_
{
    public class ClientInfo
    {
        // public int ClientCompanyId { get; set; }

        private int Client_Id = 0;
        public int ClientId
        {
            get { return Client_Id; }
            set { Client_Id = value; }

        }
        private DateTime ClientCreated_Date = DateTime.Today;
        public DateTime ClientCreatedDate
        {
            get { return ClientCreated_Date; }
            set { ClientCreated_Date = value; }

        }

        private String User_Name = "";
        public String UserName
        {
            get { return User_Name; }
            set { User_Name = value; }

        }
        private String Pass_word = "";
        public String Password
        {
            get { return Pass_word; }
            set { Pass_word = value; }

        }

        private String Mobile_No = "";
        public String MobileNo
        {
            get { return Mobile_No; }
            set { Mobile_No = value; }

        }
        private String First_Name = "";
        public String FirstName
        {
            get { return First_Name; }
            set { First_Name = value; }

        }
        private String Last_Name = "";

        public String LastName
        {
            get { return Last_Name; }
            set { Last_Name = value; }

        }
        private String Company_Name = "";
        public String CompanyName
        {
            get { return Company_Name; }
            set { Company_Name = value; }

        }
        private String Company_Address = "";
        public String CompanyAddress
        {
            get { return Company_Address; }
            set { Company_Address = value; }

        }

        private int Cit_y = 0;
        public int City
        {
            get { return Cit_y; }
            set { Cit_y = value; }

        }
        private int Sta_te = 0;
        public int State
        {
            get { return Sta_te; }
            set { Sta_te = value; }

        }
        private int Countr_y = 0;
        public int Country
        {
            get { return Countr_y; }
            set { Countr_y = value; }

        }
        private String Emai_l = "";
        public String Email
        {
            get { return Emai_l; }
            set { Emai_l = value; }

        }
        private int NoOf_Days = 0;
        public int NoOfDays
        {
            get { return NoOf_Days; }
            set { NoOf_Days = value; }

        }
        private int NoOfCompanyPer_User = 0;
        public int NoOfCompanyPerUser
        {
            get { return NoOfCompanyPer_User; }
            set { NoOfCompanyPer_User = value; }

        }

        private int NoOfAccess_User = 0;
        public int NoOfAccessUser
        {
            get { return NoOfAccess_User; }
            set { NoOfAccess_User = value; }

        }

        private int TotalCreated_User = 0;
        public int TotalCreatedUser
        {
            get { return TotalCreated_User; }
            set { TotalCreated_User = value; }

        }
        private int TotalCreated_Company = 0;
        public int TotalCreatedCompany
        {
            get { return TotalCreated_Company; }
            set { TotalCreated_Company = value; }

        }

        private int CreatedAdmin_ID = 0;

        public int CreatedAdminID
        {
            get
            {
                return CreatedAdmin_ID;
            }
            set
            {
                CreatedAdmin_ID = value;
            }
        }

        private DateTime AccountExpired_On = DateTime.Today;
      

        public DateTime AccountExpiredOn
        {
            get
            {
                return AccountExpired_On;
            }
            set
            {
                AccountExpired_On = value;
            }
        }
        private bool QueryRights_ = false;
        public bool QueryRights 
        {
            get { return QueryRights_; }
            set { QueryRights_ = value; }

        }


    }
}
