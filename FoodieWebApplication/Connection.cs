﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.IO;
using System.Data.SqlClient;

namespace FoodieWebApplication
{
    public class Connection
    {
        public static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;
        }
    }
    public class Utils
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter sda;
        DataTable dt;
        public static bool IsValidExtension(string fileName)
        {
            bool IsValid = false;
            string[] fileExtension = { ".jpg", ".png", ".jpeg" };
            for(int i = 0; i <= fileExtension.Length - 1; i++)
            {
                if (fileName.Contains(fileExtension[i]))
                {
                    IsValid = true;
                    break;
                }
            }
            return IsValid;
        }
       public static string GetImageUrl(Object url)
        {
            string url1 = "";
            if(string.IsNullOrEmpty(url.ToString()) || url == DBNull.Value)
            {
                url1 = "../Images/No_image.png";
            }
            else
            {
                url1 = string.Format("../{0}", url);
            }
            return url1;
        }

        public bool updateCartQuantity(int quantity, int productId, int userId)
        {
            bool isUpdated = false;
            con = new SqlConnection(Connection.GetConnectionString());
            cmd = new SqlCommand("Cart_Crud", con);
            cmd.Parameters.AddWithValue("@Action", "UPDATE");
            cmd.Parameters.AddWithValue("@ProductId", productId);
            cmd.Parameters.AddWithValue("@Quantity", quantity);
            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                isUpdated = true;
            }
            catch (Exception ex)
            {
                isUpdated = false;
                System.Web.HttpContext.Current.Response.Write("<script>alert('Error -" + ex.Message + "');</script>");
            }
            finally
            {
                con.Close();
            }
            return (isUpdated);
        }

        public int cartCount(int userId)
        {
            con = new SqlConnection(Connection.GetConnectionString());
            cmd = new SqlCommand("Cart_Crud", con);
            cmd.Parameters.AddWithValue("@Action", "SELECT");
            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.CommandType = CommandType.StoredProcedure;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            return dt.Rows.Count;
        }

        public static string GetUniqueId()
        {
            Guid guid = Guid.NewGuid();
            string uniqueId = guid.ToString();
            return uniqueId;
        }

    }

    public class DashboardCount
    {
        SqlConnection con;
        SqlDataAdapter sda;
        SqlDataReader sdr;
        SqlCommand cmd;
        DataTable dt;
        public int Count(string name)
        {
            int count = 0;
            con = new SqlConnection(Connection.GetConnectionString());
            cmd = new SqlCommand("Dashboard", con);
            cmd.Parameters.AddWithValue("@Action", name);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                if (sdr[0] == DBNull.Value)
                {
                    count = 0;
                }
                else
                {
                    count = Convert.ToInt32(sdr[0]);
                }
            }
            sdr.Close();
            con.Close();
            return count;
        }
    }
}
