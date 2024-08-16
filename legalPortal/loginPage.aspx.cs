using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DocumentFormat.OpenXml.Office2010.Excel;
using Newtonsoft.Json;
using onlineLegalWF.Class; 

namespace onlineLegalWF.legalPortal
{
    public partial class loginPage : System.Web.UI.Page
    {
        #region Public
        public DbControllerBase zdb = new DbControllerBase();
        public string zconnstr = ConfigurationManager.AppSettings["BPMDB"].ToString();
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                if (Session["user_login"] != null)
                {
                    var host_url = ConfigurationManager.AppSettings["host_url"].ToString();
                    Response.Redirect(host_url + "legalportal/legalportal.aspx?m=myrequest");
                }
                //else 
                //{
                //    string Username = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
                //    string user_name = Username.Substring(Username.LastIndexOf("\\") + 1);
                //    string appCode = "EFCY_LGW";

                //    var resUser = GetUserIdentityAsync(user_name, appCode);

                //    if (resUser != null) 
                //    {
                //        // clear session 
                //        addSession(user_name);
                //        var host_url = ConfigurationManager.AppSettings["host_url"].ToString();
                //        Response.Redirect(host_url + "legalportal/legalportal.aspx?m=myrequest");
                //    }
                //}
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            // Check Authen
            //var token = "xxxx";//getAuthen(txtLoginName.Text.Trim(), txtPassword.Text.Trim());
            //if (!string.IsNullOrEmpty(txtPassword.Text.Trim()) && txtPassword.Text.Trim() == "1234")
            //{
            //    if (!String.IsNullOrEmpty(token))
            //    {
            //        // clear session 
            //        addSession(txtLoginName.Text);
            //        var host_url = ConfigurationManager.AppSettings["host_url"].ToString();
            //        Response.Redirect(host_url + "legalportal/legalportal.aspx?m=myrequest");
            //    }
            //}
            //else 
            //{
            //    Response.Write("<script>alert('Error !!! Password incorrect');</script>");
            //}
            var token = checkAuthen(txtLoginName.Text.Trim(), txtPassword.Text.Trim());//getAuthen(txtLoginName.Text.Trim(), txtPassword.Text.Trim());
            if (!String.IsNullOrEmpty(token))
            {
                // clear session 
                addSession(txtLoginName.Text);
                var host_url = ConfigurationManager.AppSettings["host_url"].ToString();
                Response.Redirect(host_url + "legalportal/legalportal.aspx?m=myrequest");
            }
            else
            {
                Response.Write("<script>alert('Error !!! Password incorrect');</script>");
            }

        }
        public string checkAuthen(string xusername, string xpassword) 
        {
            string token = "";

            //string Username = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
            //string user_name = Username.Substring(Username.LastIndexOf("\\") + 1);
            string appCode = "EFCY_LGW";

            //Revoke User
            var status = RevokeUser(xusername);

            //Check Identity User MSAL
            var resUser = GetUserIdentity(xusername, xpassword, appCode);
            // Use Token Data From MSAL
            if (resUser != null)
            {
                token = resUser.refreshToken;
            }
            else 
            {
                //Check Username and Password From li_user if Correct Data set Token Ispass
                //if (!string.IsNullOrEmpty(xusername.Trim()) && !string.IsNullOrEmpty(xpassword.Trim()))
                //var Isdev = ConfigurationManager.AppSettings["isDev"].ToString();
                //if (Isdev == "true") 
                //{
                //if (!string.IsNullOrEmpty(xusername.Trim()) && xpassword.Trim() == "1234")
                //{
                //    token = "IsPass";
                //}
                //}
                //else 
                //{
                //    if (!string.IsNullOrEmpty(xusername.Trim()) && !string.IsNullOrEmpty(xpassword.Trim()))
                //    {
                //        var key = "iJLTaWhyqexThL3Qmj63qA==";
                //        string hashpassword = EmpInfo.DecryptString(key, xpassword.Trim());
                //        string sqlbpm = "select * from li_user where user_login = '" + xusername.Trim() + "' and passwordhash = '"+ hashpassword + "' ";
                //        DataTable dtbpm = zdb.ExecSql_DataTable(sqlbpm, zconnstr);

                //        if (dtbpm.Rows.Count > 0) 
                //        {
                //            token = "IsPass";
                //        }
                //    }
                //}

                if (!string.IsNullOrEmpty(xusername.Trim()) && !string.IsNullOrEmpty(xpassword.Trim()))
                {
                    var key = "iJLTaWhyqexThL3Qmj63qA==";
                    string hashpassword = EmpInfo.EncryptString(key, xpassword);
                    string sqlbpm = "select * from li_user where user_login = '" + xusername.Trim() + "' and passwordhash = '" + hashpassword + "' ";
                    DataTable dtbpm = zdb.ExecSql_DataTable(sqlbpm, zconnstr);

                    if (dtbpm.Rows.Count > 0)
                    {
                        if (dtbpm.Rows[0]["isADAccount"].ToString() == "N")
                        {
                            token = "IsPass";
                        }

                    }
                }


            }

            return token;
        }
        public void addSession(string xusername) 
        {
            var empFunc = new EmpInfo();

            var emp = empFunc.getEmpInfo(xusername);
            Session.Clear();
            Session.Add("is_login", "Y");
            Session.Add("user_login", emp.user_login);
            Session.Add("user_name", emp.full_name_en);
            Session.Add("user_position", emp.position_en);
            Session.Add("division", emp.division);
            Session.Add("department", emp.department);
            Session.Add("bu", emp.bu);

            //var resListEmp = empFunc.getApprovalList(xusername);

        }

        //static async Task<UserIdntityResponse> GetUserIdentityAsync(string identity,string appCode)
        static UserIdntityResponse GetUserIdentityAsync(string identity,string appCode)
        {
            UserIdntityResponse res = null;
            try
            {
                // Create an instance of HttpClient
                using (HttpClient httpClient = new HttpClient())
                {
                    //Prepare content
                    var req = new UserIndentity
                    {
                        identity = identity,
                        applicationCode = appCode
                    };
                    var options = new JsonSerializerSettings
                    {
                        Formatting = Formatting.Indented,
                        DefaultValueHandling = DefaultValueHandling.Ignore
                    };
                    var jsonData = JsonConvert.SerializeObject(req, options);
                    var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");

                    // Define the parameters to include in the URL
                    //string baseurl = $"{_crmSetting.Value.BaseURL}";
                    string baseurl = ConfigurationManager.AppSettings["msal_api_url"].ToString();

                    // Build the URL with the parameter
                    string url = $"{baseurl}/User/identity?identity={identity}&applicationCode={appCode}";
                    httpClient.BaseAddress = new Uri(url);

                    //httpClient.DefaultRequestHeaders.Add("User-Agent", "K2S&L");
                    //httpClient.DefaultRequestHeaders.Add("X-Api-Key", _crmSetting.Value.ApiKey);

                    // Send the GET request and get the response
                    //HttpResponseMessage response = await httpClient.PostAsync(httpClient.BaseAddress, content);
                    HttpResponseMessage response = httpClient.GetAsync(httpClient.BaseAddress).Result;
                    response.EnsureSuccessStatusCode();

                    // Read and display the response content as a string
                    //string responseContent = await response.Content.ReadAsStringAsync();
                    string responseContent = response.Content.ReadAsStringAsync().Result;
                    res = JsonConvert.DeserializeObject<UserIdntityResponse>(responseContent);
                }

            }
            catch (HttpRequestException ex)
            {
                //return BadRequest($"Error: {ex.StatusCode}, {ex.Message}{ex.InnerException}");
                LogHelper.Write($"Error: {ex.Message}{ex.InnerException}");
            }

            return res;
        }
        public UserIdntityResponse GetUserIdentity(string username, string password ,string appCode)
        {
            UserIdntityResponse res = null;
            try
            {
                // Create an instance of HttpClient
                using (HttpClient httpClient = new HttpClient())
                {
                    //Prepare content
                    var req = new UserIndentity
                    {
                        username = username,
                        password = password,
                        applicationCode = appCode
                    };
                    var options = new JsonSerializerSettings
                    {
                        Formatting = Formatting.Indented,
                        DefaultValueHandling = DefaultValueHandling.Ignore
                    };
                    var jsonData = JsonConvert.SerializeObject(req, options);
                    var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");

                    // Define the parameters to include in the URL
                    //string baseurl = $"{_crmSetting.Value.BaseURL}";
                    string baseurl = ConfigurationManager.AppSettings["msal_api_url"].ToString();

                    // Build the URL with the parameter
                    string url = $"{baseurl}/User/identity";
                    httpClient.BaseAddress = new Uri(url);

                    // Send the GET request and get the response
                    //HttpResponseMessage response = await httpClient.PostAsync(httpClient.BaseAddress, content);
                    HttpResponseMessage response = httpClient.PostAsync(httpClient.BaseAddress, content).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        response.EnsureSuccessStatusCode();

                        // Read and display the response content as a string
                        //string responseContent = await response.Content.ReadAsStringAsync();
                        string responseContent = response.Content.ReadAsStringAsync().Result;
                        res = JsonConvert.DeserializeObject<UserIdntityResponse>(responseContent);
                    }
                    else 
                    {
                        res = null;
                    }
                    
                }

            }
            catch (HttpRequestException ex)
            {
                //return BadRequest($"Error: {ex.StatusCode}, {ex.Message}{ex.InnerException}");
                LogHelper.Write($"Error: {ex.Message}{ex.InnerException}");
            }

            return res;
        }
        public bool RevokeUser(string username)
        {
            bool status = false;
            try
            {
                // Create an instance of HttpClient
                using (HttpClient httpClient = new HttpClient())
                {
                    //Prepare content
                    var req = new UserIndentity
                    {
                        identity = username
                    };
                    var options = new JsonSerializerSettings
                    {
                        Formatting = Formatting.Indented,
                        DefaultValueHandling = DefaultValueHandling.Ignore
                    };
                    var jsonData = JsonConvert.SerializeObject(req, options);
                    var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");

                    // Define the parameters to include in the URL
                    //string baseurl = $"{_crmSetting.Value.BaseURL}";
                    string baseurl = ConfigurationManager.AppSettings["msal_api_url"].ToString();

                    // Build the URL with the parameter
                    string url = $"{baseurl}/User/revokeUser";
                    httpClient.BaseAddress = new Uri(url);

                    // Send the GET request and get the response
                    //HttpResponseMessage response = await httpClient.PostAsync(httpClient.BaseAddress, content);
                    HttpResponseMessage response = httpClient.PostAsync(httpClient.BaseAddress, content).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        response.EnsureSuccessStatusCode();

                        status = true;
                    }
                    else
                    {
                        status = false;
                    }

                }

            }
            catch (HttpRequestException ex)
            {
                //return BadRequest($"Error: {ex.StatusCode}, {ex.Message}{ex.InnerException}");
                LogHelper.Write($"Error: {ex.Message}{ex.InnerException}");
            }

            return status;
        }
        public class UserIndentity 
        {
            public string identity { get; set; } 
            public string username { get; set; } 
            public string password { get; set; } 
            public string applicationCode { get; set; }
        }
        public class UserIdntityResponse
        {
            public string displayName { get; set; }
            public string surname { get; set; }
            public string givenName { get; set; }
            public string jobTitle { get; set; }
            public string mail { get; set; }
            public string mobilePhone { get; set; }
            public string officeLocation { get; set; }
            public string userPrincipalName { get; set; }
            public string userId { get; set; }
            public string userGraphId { get; set; }
            public string accessToken { get; set; }
            public string refreshToken { get; set; }
            public string refreshTokenExpiryTime { get; set; }
        }

    }
}