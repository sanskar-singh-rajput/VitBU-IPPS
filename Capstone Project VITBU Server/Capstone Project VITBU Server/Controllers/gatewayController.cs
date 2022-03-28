using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Security.Cryptography;
using System.Text;
using OtpNet;

namespace Capstone_Project_VITBU_Server.Controllers
{
    public class gatewayController : ApiController
    {
        [HttpPost]
        public validate_accounts Post([FromUri] string name, [FromUri] string address,[FromUri] string selected)
        {
            //format string to replace blank spaces
            address = address.Replace(" ", string.Empty);

            validate_accounts acc = new validate_accounts();
            //create a mysql connection and check supplied info
            string connection_s = @"server=localhost;userid=root;password=root;database=capstone";
            var con = new MySqlConnection(connection_s);
            con.Open();

            string query = "", user_salt = "";
            //retrieve the salt value
            query = $"select user_salt from entity where name = \"{name}\"";
            MySqlCommand cmd = new MySqlCommand(query, con);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                user_salt = reader.GetString("user_salt");
            }
            reader.Close();

            //get list
            if (selected == "cc")
                query = $"select id from entity where name = \"{name}\" and  credit_c = \"{ComputeSha256Hash(address + user_salt)}\"";
            if (selected == "dc")
                query = $"select id from entity where name = \"{name}\" and debit_c = \"{ComputeSha256Hash(address + user_salt)}\"";
            string fetched_data = "";
            cmd = new MySqlCommand(query, con);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                fetched_data = reader.GetString("id");
            }
            reader.Close();
            if (!string.IsNullOrEmpty(fetched_data))
                acc.results = fetched_data;
            else
                acc.results = "val_failed";

            return acc;
        }
        public transfer_funds Post([FromUri] string from_id, [FromUri] string from_address, [FromUri] string to_id, [FromUri] string to_address, [FromUri] int transfer_amount, [FromUri] string user_totp, [FromUri] string selected = "cc", [FromUri] string transfer_amount_currency = "INR")
        {
            transfer_funds tf = new transfer_funds();

            //format strings to replace blank spaces
            from_address = from_address.Replace(" ", string.Empty);
            to_address = to_address.Replace(" ", string.Empty);

            string merchant_id = "QuTLhX1tC9", merchant_currency = "USD";

            /*considering a default merchant | THESE VALUES WILL BE CAPTURED FROM WHOEVER INTEGRATES THE PAYMENT GATEWAY IN THEIR WEBSITE/WEB-STORE
            
            merchant_name = "Nanabah Naira", merchant_CC = "4210 6522 7835 8441:211:9/2023", merchant_DC = "3762 4341 9257 482:919:3/2025",
            ID: QuTLhX1tC9
            Name: Nanabah Naira
            CC: 4210 6522 7835 8441:211:9/2023
            DC: 3762 4341 9257 482:919:3/2025
            Currency: USD

             */

            //connect to entity table and get currency type for client
            string connection_s = @"server=localhost;userid=root;password=root;database=capstone";
            var con = new MySqlConnection(connection_s);
            con.Open();

            string query = "";

            //get list
            query = $"select {selected}_balance, default_currency, totp_secret from entity where id = \"{from_id}\"";
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
            Totp totp = new Totp(Base32Encoding.ToBytes(totp_secret));

            if (totp.ComputeTotp() != user_totp)
            {
                tf.results = "Invalid ToTP";
                return tf;
            }

            int balance = Convert.ToInt32(to_fetched_data);

            if (balance < transfer_amount)
                tf.results = "insuf_bal";
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
                    m_balance += transfer_amount / 76;
                else
                    m_balance += transfer_amount;
                query = $"update entity set {selected}_balance = \"{m_balance}\" where id = \"{merchant_id}\"";
                cmd = new MySqlCommand(query, con);
                cmd.ExecuteNonQuery();

                string transaction_token = Guid.NewGuid().ToString();
                //create transaction and update values in entiry
                query = $"insert into transactions values (\"{transaction_token}\", \"{DateTime.UtcNow}\", \"{selected}\", \"{from_id}\", \"{from_address}\", \"{to_id}\", \"{to_address}\", \"complete\")";
                cmd = new MySqlCommand(query, con);
                cmd.ExecuteNonQuery();

                tf.results = $"success:{transaction_token}";
            }

            return tf;

            //inr to usd = INR * 76.18
        }
        string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        #region classes
        public class validate_accounts
        {
            public string results;
        }
        public class transfer_funds
        {
            public string results;
        }
        #endregion
    }
}
