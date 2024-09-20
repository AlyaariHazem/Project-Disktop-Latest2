using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows.Forms;
using System.Linq;
using System.Xml;

namespace Myschool.Helpers
{
    public class DatabaseHelper : IDisposable
    {
        private readonly SqlConnection _connection;

        public DatabaseHelper()
        {
            var connectionString = GetConnectionString("SchoolDB");
            _connection = new SqlConnection(connectionString);
        }
        public static string GetConnectionString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

       

        public SqlConnection OpenConnection()
        {
            try
            {
                if (_connection.State == ConnectionState.Closed)
                {
                    _connection.Open();
                }
                return _connection;
            }
            catch (Exception ex)
            {
                throw new Exception("Error opening connection: " + ex.Message);
            }
        }



        public void CloseConnection()
        {
            try
            {
                if (_connection.State == ConnectionState.Open)
                {
                    _connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error closing connection: " + ex.Message);
            }
        }



        public void Insert(string tableName, Dictionary<string, object> values)
        {
            if (values == null || values.Count == 0)
            {
                throw new ArgumentException("Values dictionary cannot be null or empty.", nameof(values));
            }

            try
            {
                OpenConnection();
                string columns = string.Join(", ", values.Keys);
                string parameters = string.Join(", ", values.Keys.Select(k => "@" + k));

                string query = $"INSERT INTO {tableName} ({columns}) VALUES ({parameters})";
                using (SqlCommand cmd = new SqlCommand(query, _connection))
                {
                    foreach (var kvp in values)
                    {
                        cmd.Parameters.Add(new SqlParameter("@" + kvp.Key, kvp.Value ?? DBNull.Value));
                    }
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting data: " + ex.Message);
            }
        }

        public void Update(string tableName, Dictionary<string, object> values, string whereClause)
        {
            if (values == null || values.Count == 0)
            {
                throw new ArgumentException("Values dictionary cannot be null or empty.", nameof(values));
            }

            try
            {
                OpenConnection();
                string setClause = string.Join(", ", values.Keys.Select(k => $"{k} = @{k}"));
                string query = $"UPDATE {tableName} SET {setClause} WHERE {whereClause}";

                using (SqlCommand cmd = new SqlCommand(query, _connection))
                {
                    foreach (var kvp in values)
                    {
                        cmd.Parameters.Add(new SqlParameter("@" + kvp.Key, kvp.Value ?? DBNull.Value));
                    }
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating data: " + ex.Message);
            }
        }
       

     

        public void Delete(string tableName, string whereClause)
        {
            try
            {
                OpenConnection();
                string query = $"DELETE FROM {tableName} WHERE {whereClause}";
                using (SqlCommand cmd = new SqlCommand(query, _connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting data: " + ex.Message);
            }
        }
      


        public void Dispose()
        {
            CloseConnection();
            _connection.Dispose();
        }









    }
}