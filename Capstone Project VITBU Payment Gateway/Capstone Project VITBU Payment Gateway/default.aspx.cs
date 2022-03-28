using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using OtpNet;
using RestSharp;

namespace SimpleTabControl
{
    public partial class SimpleTabControl : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Tab1.CssClass = "Clicked";
                MainView.ActiveViewIndex = 0;
            }
            Panel1.Visible = false;
            Panel2.Visible = false;
            Panel3.Visible = false;
            Panel4.Visible = false;
            Panel5.Visible = false;
            Panel6.Visible = false;
        }

        protected void Tab1_Click(object sender, EventArgs e)
        {
            Tab1.CssClass = "Clicked";
            //Tab2.CssClass = "Initial";
            //Tab3.CssClass = "Initial";
            MainView.ActiveViewIndex = 0;
        }

        protected void Tab2_Click(object sender, EventArgs e)
        {
            Tab1.CssClass = "Initial";
            //Tab2.CssClass = "Clicked";
            //Tab3.CssClass = "Initial";
            MainView.ActiveViewIndex = 1;
        }

        protected void Tab3_Click(object sender, EventArgs e)
        {
            Tab1.CssClass = "Initial";
            //Tab2.CssClass = "Initial";
            //Tab3.CssClass = "Clicked";
            MainView.ActiveViewIndex = 2;
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownList1.SelectedIndex == 0)
            {
                Panel1.Visible = false;
                Panel2.Visible = false;
                Panel3.Visible = false;
                Panel4.Visible = false;
                Panel5.Visible = false;
                Panel6.Visible = false;
            }
            if (DropDownList1.SelectedIndex == 1)
            {
                Panel1.Visible = true;
                Panel2.Visible = false;
                Panel3.Visible = false;
                Panel4.Visible = false;
                Panel5.Visible = false;
                Panel6.Visible = false;
            }
            if (DropDownList1.SelectedIndex == 2)
            {
                Panel1.Visible = false;
                Panel2.Visible = true;
                Panel3.Visible = false;
                Panel4.Visible = false;
                Panel5.Visible = false;
                Panel6.Visible = false;
            }
            if (DropDownList1.SelectedIndex == 3)
            {
                Panel1.Visible = false;
                Panel2.Visible = false;
                Panel3.Visible = true;
                Panel4.Visible = false;
                Panel5.Visible = false;
                Panel6.Visible = false;
            }
            if (DropDownList1.SelectedIndex == 4)
            {
                Panel1.Visible = false;
                Panel2.Visible = false;
                Panel3.Visible = false;
                Panel4.Visible = true;
                Panel5.Visible = false;
                Panel6.Visible = false;
            }
            if (DropDownList1.SelectedIndex == 5)
            {
                Panel1.Visible = false;
                Panel2.Visible = false;
                Panel3.Visible = false;
                Panel4.Visible = false;
                Panel5.Visible = true;
                Panel6.Visible = false;
            }
            if (DropDownList1.SelectedIndex == 6)
            {
                Panel1.Visible = false;
                Panel2.Visible = false;
                Panel3.Visible = false;
                Panel4.Visible = false;
                Panel5.Visible = false;
                Panel6.Visible = true;
            }
        }
        string merchant_id = "QuTLhX1tC9", merchant_name = "Nanabah Naira", merchant_CC = "4210 6522 7835 8441:211:9/2023", merchant_DC = "3762 4341 9257 482:919:3/2025", merchant_currency = "USD";
        protected void Button1_Click(object sender, EventArgs e)
        {
            string selected = return_selected();




            //initiating payment

            //validate customer account
            string id = "", name = "", address = "", merchant_address = "";
            if (selected == "cc")
            {
                address = TextBox1.Text + ":" + TextBox3.Text + ":" + TextBox4.Text;
                merchant_address = merchant_CC;
                name = TextBox2.Text;
            }
            if (selected == "dc")
            {
                address = TextBox5.Text + ":" + TextBox7.Text + ":" + TextBox8.Text;
                merchant_address = merchant_DC;
                name = TextBox6.Text;
            }

            
            if(validate_account(name, address, selected) == false)
            {
                string script = $"alert(\"Account Validation Failed!\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
                return;
            }
            if (!string.IsNullOrEmpty(validate_acc_user_id))
            {
                //create transaction and transfer funds
                string message = transfer_funds(validate_acc_user_id, address, merchant_id, merchant_address, 1000, TextBox23.Text, selected);

                if (message.Contains("success"))
                {
                    string[] parsed = message.Split(':');
                    string script = $"alert(\"Successfull transaction! Your transaction ID is: {parsed[1]} and has been copied to clipboard for future references.\");";
                    
                    Thread staThread = new Thread(new ParameterizedThreadStart(setClipboard));
                    staThread.SetApartmentState(ApartmentState.STA);
                    staThread.Start(parsed[1]);
                    
                    ScriptManager.RegisterStartupScript(this, GetType(),
                                          "ServerControlScript", script, true);
                }
                else
                {
                    string script = $"alert(\"{message}!\");";
                    ScriptManager.RegisterStartupScript(this, GetType(),
                                          "ServerControlScript", script, true);
                }
                
            }
            else
            { //server failed validation
                string script = $"alert(\"Server failed validation, a null value was returned!\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);

            }
            string return_selected()
            {
                //detecting payment method through selected panel
                //implementation for cc and dc only

                if (DropDownList1.SelectedValue == "Credit Card")
                    return "cc";
                if (DropDownList1.SelectedValue == "Debit Card")
                    return "dc";
                return "";
            }

            
        }
        public string validate_acc_user_id = "";
        bool validate_account(string name, string address, string selected)
        {
            RestClient client = new RestClient("https://localhost:44344/");
            RestRequest req = new RestRequest("/api/gateway/", Method.POST);
            req.AddQueryParameter("name", name);
            req.AddQueryParameter("address", address);
            req.AddQueryParameter("selected", selected);

            IRestResponse resp = client.Execute(req);
            Json_Result_parse JRP = JsonConvert.DeserializeObject<Json_Result_parse>(resp.Content);
            if (JRP.results == "val_failed")
                return false;
            else
            {
                validate_acc_user_id = JRP.results;
                return true;
            }
        }

        string transfer_funds(string from_id, string from_address, string to_id, string to_address, int transfer_amount, string user_totp, 
            string selected = "cc", string transfer_amount_currency = "INR")
        {
            RestClient client = new RestClient("https://localhost:44344/");
            RestRequest req = new RestRequest("/api/gateway/", Method.POST);
            req.AddQueryParameter("from_id", from_id);
            req.AddQueryParameter("from_address", from_address);
            req.AddQueryParameter("to_id", to_id);
            req.AddQueryParameter("to_address", to_address);
            req.AddQueryParameter("transfer_amount", transfer_amount.ToString());
            req.AddQueryParameter("user_totp", user_totp);
            req.AddQueryParameter("selected", selected);
            req.AddQueryParameter("transfer_amount_currency", transfer_amount_currency);

            IRestResponse resp = client.Execute(req);
            Json_Result_parse JRP = JsonConvert.DeserializeObject<Json_Result_parse>(resp.Content);
            return JRP.results;
        }
        public static void setClipboard(object data)
        {
            Clipboard.SetText(data.ToString());
        }
        /*
        string validate_account(string name, string address, string selected)
        {
            //create a mysql connection and check supplied info
            string connection_s = @"server=localhost;userid=root;password=root;database=capstone";
            var con = new MySqlConnection(connection_s);
            con.Open();

            string query = "";
            //get list
            if(selected == "cc")
                query = $"select id from entity where name = \"{name}\" and  credit_c = \"{address}\"";
            if(selected == "dc")
                query = $"select id from entity where name = \"{name}\" and debit_c = \"{address}\"";
            string fetched_data = "";
            MySqlCommand cmd = new MySqlCommand(query, con);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                fetched_data = reader.GetString("id");
            }
            reader.Close();
            if (!string.IsNullOrEmpty(fetched_data))
                return fetched_data;
            else
            {
                string script = $"alert(\"Account Validation Failed!\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
                return "";
            }
        }
        */
        /*
        string transfer_funds(string from_id, string from_address, string to_id, string to_address, int transfer_amount, string selected = "cc", string transfer_amount_currency = "INR")
        {
            
            /*considering a default merchant

            //ID: QuTLhX1tC9
            //Name: Nanabah Naira
            //CC: 4210 6522 7835 8441:211:9/2023
            //DC: 3762 4341 9257 482:919:3/2025
            //Currency: USD

             

            //connect to entity table and get currency type for client
            string connection_s = @"server=localhost;userid=root;password=root;database=capstone";
            var con = new MySqlConnection(connection_s);
            con.Open();

            //check if amount can be deducted


            //get list
            string query = $"select {selected}_balance, default_currency, totp_secret from entity where id = \"{from_id}\"";
            string to_fetched_data = ""; string default_currency = "", totp_secret = "";
            MySqlCommand cmd = new MySqlCommand(query, con);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                to_fetched_data = reader.GetString($"{selected}_balance");
                default_currency = reader.GetString("default_currency");
                totp_secret = reader.GetString("totp_secret");
            }
            reader.Close();

            //verifying totp validity
            string otp = TextBox23.Text;
            Totp totp = new Totp(Base32Encoding.ToBytes(totp_secret));
            if(Base32Encoding.ToBytes(totp.ComputeTotp()) != Base32Encoding.ToBytes(otp))
            {
                string script = $"alert(\"Time based One Time Password is Invalid!\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
                return "Invalid ToTP";
            }


            int balance = Convert.ToInt32(to_fetched_data);
            
            if (balance < transfer_amount)
            {
                string script = $"alert(\"Insufficient Balance!\");";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                      "ServerControlScript", script, true);
                return "insufficient balance";
            }

            else
            {
                //otherwise no changes required

                int client_amount = Convert.ToInt32(to_fetched_data);
                client_amount = client_amount - transfer_amount;
                query = $"update entity set {selected}_balance = \"{client_amount}\" where id = \"{from_id}\"";
                cmd = new MySqlCommand(query, con);
                cmd.ExecuteNonQuery();

                //add amount to merchant from query
                string merchant_balance = "";
                query = $"select {selected}_balance, default_currency from entity where id = \"{merchant_id}\"";
                cmd = new MySqlCommand(query, con);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    merchant_balance = reader.GetString($"{selected}_balance");
                }
                reader.Close();

                int m_balance = Convert.ToInt32(merchant_balance);
                if (merchant_currency == "USD" && transfer_amount_currency == "INR")
                    m_balance += transfer_amount/76;
                else
                    m_balance += transfer_amount;
                query = $"update entity set {selected}_balance = \"{m_balance}\" where id = \"{merchant_id}\"";
                cmd = new MySqlCommand(query, con);
                cmd.ExecuteNonQuery();

                query = $"insert into transactions values (\"{Guid.NewGuid()}\", \"{selected}\", \"{from_id}\", \"{from_address}\", \"{to_id}\", \"{to_address}\", \"complete\")";
                cmd = new MySqlCommand(query, con);
                cmd.ExecuteNonQuery();

                return "success";
            }

            //create transaction and update values in entiry

            //inr to usd = INR * 76.18
        }
        */
    }
    class Json_Result_parse
    {
        public string results { get; set; }
    }
}



