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
        public IEnumerable<Hardware> GetHardware(string name, string category, string owner)
        {
            SqlCommand command = new SqlCommand(null, connection);
            command.CommandText = "select * from [dbo].[hardware]";

            SqlParameter nameParam = new SqlParameter("@name", SqlDbType.NVarChar, 50);
            nameParam.Value = name;
            SqlParameter categoryParam = new SqlParameter("@category", SqlDbType.NVarChar, 50);
            categoryParam.Value = category;
            SqlParameter ownerParam = new SqlParameter("@owner", SqlDbType.NVarChar, 50);
            ownerParam.Value = owner;
            if (name != null)
            {
                command.CommandText = "select * from [dbo].[hardware] where name like @name";
                command.Parameters.Add(nameParam);
                if(category != null)
                {
                    command.CommandText = command.CommandText + " and category like @category";
                    command.Parameters.Add(categoryParam);
                }
                if (owner != null)
                {
                    command.CommandText = command.CommandText + " and owner like @owner";
                    command.Parameters.Add(ownerParam);
                }
            }
            else if(category != null)
            {
                command.CommandText = "select * from [dbo].[hardware] where category like @category";
                command.Parameters.Add(categoryParam);
                if (owner != null)
                {
                    command.CommandText = command.CommandText + " and owner like @owner";
                    command.Parameters.Add(ownerParam);
                }
            }
            else if (owner != null)
            {
                command.CommandText = "select * from [dbo].[hardware] where owner like @owner";
                command.Parameters.Add(ownerParam);

            }
            command.Prepare();
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

        public IEnumerable<Hardware> GetByAvailability(bool available)
        {
            string a = (available) ? 1.ToString() : ("0 OR [available] is null");
            SqlCommand command = new SqlCommand(null, connection);
            command.CommandText = "select * from [dbo].[hardware] where [available] = " + a;
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
            SqlParameter nameParam = new SqlParameter("@name", SqlDbType.NVarChar,50);
            nameParam.Value = hardware.Name;
            SqlParameter categoryParam = new SqlParameter("@category", SqlDbType.NVarChar,50);
            categoryParam.Value = hardware.Category;
            SqlParameter ownerParam = new SqlParameter("@owner", SqlDbType.NVarChar,50);
            ownerParam.Value = hardware.Owner;
            command.Parameters.Add(nameParam);
            command.Parameters.Add(categoryParam);
            command.Parameters.Add(ownerParam);
            command.Prepare();
        }

        public bool UpdateHardware(int id, Hardware hardware)
        {
            SqlCommand command = new SqlCommand(null, connection);
            command.CommandText = "update [dbo].[hardware] set name=@name,category=@category,owner=@owner where id = " + id;
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