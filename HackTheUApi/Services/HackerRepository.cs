using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HackTheUApi.Models;
using System.Data.SqlClient;
using System.Data;

namespace HackTheUApi.Services
{
    public class HackerRepository
    {
        SqlConnection connection;

        private const string connection_string = "Server=tcp:hacktheu.database.windows.net,1433;Initial Catalog=hacktheudb;Persist Security Info=False;User ID=hacktheu;Password=Augmentation801;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public HackerRepository()
        {
            connection = new SqlConnection(connection_string);
            connection.Open();
        }

        public IEnumerable<Hacker> GetHackers(string team, string checkedin, string present)
        {
            SqlCommand command = new SqlCommand(null, connection);
            command.CommandText = "select * from [dbo].[hackers]";
                        
            if (team != null)
            {
                command.CommandText = "select * from [dbo].[hackers] where [team] like @team";
                AddTeamParam(command, team);
                if (checkedin != null)
                {
                    command.CommandText = command.CommandText + " and [checkedin] = @checkedin";
                    AddCheckedInParam(command, checkedin);
                }
                if (present != null)
                {
                    command.CommandText = command.CommandText + " and [present] = @present";
                    AddPresentParam(command, present);
                }
            }
            else if (checkedin != null)
            {
                command.CommandText = "select * from [dbo].[hackers] where [checkedin] = @checkedin";
                AddCheckedInParam(command, checkedin);
                if (present != null)
                {
                    command.CommandText = command.CommandText + " and [present] = @present";
                    AddPresentParam(command, present);
                }
            }
            else if (present != null)
            {
                command.CommandText = "select * from [dbo].[hackers] where [present] = @present";
                AddPresentParam(command, present);

            }
            command.Prepare();
            SqlDataReader reader = command.ExecuteReader();
            List<Hacker> list = new List<Hacker>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    list.Add(GetHackerFromReader(reader));
                }
            }
            return list;
        }

        internal IEnumerable<Hacker> GetByPresent(bool present)
        {
            string a = (present) ? 1.ToString() : ("0 OR [present] is null");
            SqlCommand command = new SqlCommand(null, connection);
            command.CommandText = "select * from [dbo].[hackers] where [present] = " + a;
            SqlDataReader reader = command.ExecuteReader();
            List<Hacker> list = new List<Hacker>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    list.Add(GetHackerFromReader(reader));
                }
            }
            return list;
        }

        private Hacker GetHackerFromReader(SqlDataReader reader)
        {
            return new Hacker
            {
                Id = (int)reader["id"],
                First = (string)reader["first"],
                Last = (string)reader["last"],
                Email = (string)reader["email"],
                Team = (int)reader["team"],
                CheckedIn = Convert.ToBoolean((byte)reader["checkedin"]),
                Present = Convert.ToBoolean((byte)reader["present"])
            };
        }

        internal IEnumerable<Hacker> GetByCheckedIn(bool checkedIn)
        {
            string a = (checkedIn) ? 1.ToString() : ("0 OR [checkedin] is null");
            SqlCommand command = new SqlCommand(null, connection);
            command.CommandText = "select * from [dbo].[hackers] where [checkedin] = " + a;
            SqlDataReader reader = command.ExecuteReader();
            List<Hacker> list = new List<Hacker>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    list.Add(GetHackerFromReader(reader));
                }
            }
            return list;
        }

        private void AddTeamParam(SqlCommand command, string value)
        {
            SqlParameter teamParam = new SqlParameter("@team", SqlDbType.NVarChar, 100);
            teamParam.Value = value;
            command.Parameters.Add(teamParam);
        }

        private void AddCheckedInParam(SqlCommand command, string value)
        {
            SqlParameter checkedinParam = new SqlParameter("@checkedin", SqlDbType.TinyInt);
            checkedinParam.Value = Convert.ToBoolean(int.Parse(value));
            command.Parameters.Add(checkedinParam);
        }

        private void AddPresentParam(SqlCommand command, string value)
        {
            SqlParameter presentParam = new SqlParameter("@present", SqlDbType.TinyInt);
            presentParam.Value = Convert.ToBoolean(int.Parse(value));
            command.Parameters.Add(presentParam);
        }

        public Hacker GetHacker(int id)
        {
            SqlCommand command = new SqlCommand(null, connection);
            command.CommandText = "select * from [dbo].[hackers] where id = " + id;
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    return GetHackerFromReader(reader);
                }
            }
            return null;
        }

        public bool InsertNewHacker(Hacker hacker)
        {
            SqlCommand command = new SqlCommand(null, connection);
            command.CommandText = "insert into [dbo].[hackers] (first,last,email,team,checkedin,present) VALUES(@first,@last,@email,@team,@checkedin,@present)";
            PrepareHackerParams(command, hacker);
            try
            {
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private void PrepareHackerParams(SqlCommand command, Hacker hacker)
        {
            SqlParameter firstParam = new SqlParameter("@first", SqlDbType.VarChar, 100);
            firstParam.Value = hacker.First;
            SqlParameter lastParam = new SqlParameter("@last", SqlDbType.VarChar, 100);
            lastParam.Value = hacker.Last;
            SqlParameter emailParam = new SqlParameter("@email", SqlDbType.VarChar, 100);
            emailParam.Value = hacker.Email;
            SqlParameter teamParam = new SqlParameter("@team", SqlDbType.VarChar, 100);
            teamParam.Value = hacker.First;
            SqlParameter checkedInParam = new SqlParameter("@checkedin", SqlDbType.TinyInt);
            checkedInParam.Value = hacker.CheckedIn;
            SqlParameter presentParam = new SqlParameter("@present", SqlDbType.TinyInt);
            presentParam.Value = hacker.Present;
            command.Parameters.Add(firstParam);
            command.Parameters.Add(lastParam);
            command.Parameters.Add(emailParam);
            command.Parameters.Add(teamParam);
            command.Parameters.Add(checkedInParam);
            command.Parameters.Add(presentParam);
            command.Prepare();
        }

        public bool UpdateHacker(int id, Hacker hacker)
        {
            SqlCommand command = new SqlCommand(null, connection);
            command.CommandText = "update [dbo].[hackers] set first=@first,last=@last,email=@email,team=@team,checkedin=@checkedin,present=@present where id = " + id;
            PrepareHackerParams(command, hacker);
            try
            {
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool DeleteHacker(int id)
        {
            SqlCommand command = new SqlCommand(null, connection);
            command.CommandText = "delete from [dbo].[hackers] where id = " + id;
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