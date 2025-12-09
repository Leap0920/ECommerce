using System;
using System.Web.Mvc;
using ECommerce.Data;

namespace ECommerce.Controllers
{
    public class TestController : Controller
    {
        // GET: Test/DatabaseConnection
        public ActionResult DatabaseConnection()
        {
            try
            {
                using (var connection = DatabaseConfig.GetConnection())
                {
                    connection.Open();
                    
                    // Test the connection
                    using (var cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = "SELECT 1";
                        var result = cmd.ExecuteScalar();
                        
                        ViewBag.Message = "✅ MySQL Connection Successful!";
                        ViewBag.Status = "Connected";
                        ViewBag.Server = connection.DataSource;
                        ViewBag.Database = connection.Database;
                    }
                    
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "❌ MySQL Connection Failed!";
                ViewBag.Status = "Error";
                ViewBag.Error = ex.Message;
            }

            return View();
        }

        // GET: Test/CheckTables
        public ActionResult CheckTables()
        {
            try
            {
                using (var connection = DatabaseConfig.GetConnection())
                {
                    connection.Open();
                    
                    using (var cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = @"
                            SELECT TABLE_NAME 
                            FROM INFORMATION_SCHEMA.TABLES 
                            WHERE TABLE_SCHEMA = 'ecommerce_db'";
                        
                        var tables = new System.Collections.Generic.List<string>();
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                tables.Add(reader.GetString(0));
                            }
                        }
                        
                        ViewBag.Tables = tables;
                        ViewBag.Status = "Success";
                    }
                    
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Status = "Error";
                ViewBag.Error = ex.Message;
            }

            return View();
        }
    }
}
