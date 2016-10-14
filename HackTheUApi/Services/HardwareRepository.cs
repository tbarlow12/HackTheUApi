using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HackTheUApi.Models;
using System.Data.SqlClient;
using System.Data;

namespace HackTheUApi.Services
{
    public class HardwareRepository
    {
        SqlConnection connection;

        private const string connection_string = "Server=tcp:hacktheu.database.windows.net,1433;Initial Catalog=hacktheudb;Persist Security Info=False;User ID=hacktheu;Password=Augmentation801;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public HardwareRepository()
        {
            connection = new SqlConnection(connection_string);
            connection.Open();
        }
        public IEnumerable<Hardware> GetAllHardware()
        {
            SqlCommand command = new SqlCommand(null, connection);
            command.CommandText = "select * from [dbo].[hardware]";
            SqlDataReader reader = command.ExecuteReader();
            List<Hardware> list = new List<Hardware>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    list.Add(
                        new Hardware
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Category = reader.GetString(2),
                        Owner = reader.GetString(3)
                    });
                }
            }
            return list;            
        }

        public Hardware GetHardware(int id)
        {
            SqlCommand command = new SqlCommand(null, connection);
            command.CommandText = "select * from [dbo].[hardware] where id = " + id;
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    return
                        new Hardware
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Category = reader.GetString(2),
                            Owner = reader.GetString(3)
                        };
                }
            }
            return null;
        }

        public bool InsertNewHardware(Hardware hardware)
        {
            SqlCommand command = new SqlCommand(null, connection);
            command.CommandText = "insert into [dbo].[hardware] (name, category, owner) VALUES(@name, @category, @owner)";
            PrepareHardwareParams(command, hardware);
            try
            {
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void PrepareHardwareParams(SqlCommand command, Hardware hardware)
        {
            SqlParameter nameParam = new SqlParameter("@name", SqlDbType.NVarChar);
            nameParam.Value = hardware.Name;
            SqlParameter categoryParam = new SqlParameter("@category", SqlDbType.NVarChar);
            categoryParam.Value = hardware.Category;
            SqlParameter ownerParam = new SqlParameter("@owner", SqlDbType.NVarChar);
            ownerParam.Value = hardware.Owner;
            command.Parameters.Add(nameParam);
            command.Parameters.Add(categoryParam);
            command.Parameters.Add(ownerParam);
            command.Prepare();
        }

        public void UpdateHardware(int id, Hardware hardware)
        {
            SqlCommand command = new SqlCommand(null, connection);
            command.CommandText = "update [dbo].[hardware] set name=@name,category=@category,owner=@owner where id = " + id;
            PrepareHardwareParams(command, hardware);
        }

        public bool DeleteHardware(int id)
        {
            SqlCommand command = new SqlCommand(null, connection);
            command.CommandText = "delete from [dbo].[hardware] where id = " + id;
            try
            {
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}