using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HackTheUApi.Models;
using System.Data.SqlClient;
using System.Data;

namespace HackTheUApi.Services
{
    public class TeamRepository
    {
        SqlConnection connection;

        private const string connection_string = "Server=tcp:hacktheu.database.windows.net,1433;Initial Catalog=hacktheudb;Persist Security Info=False;User ID=hacktheu;Password=Augmentation801;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public TeamRepository()
        {
            connection = new SqlConnection(connection_string);
            connection.Open();
        }

        internal IEnumerable<Team> GetAllTeams()
        {
            SqlCommand command = new SqlCommand(null, connection);
            command.CommandText = "select * from [dbo].[teams]";
            List<Team> teams = new List<Team>();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        teams.Add(GetTeamFromReader(reader));
                    }
                }
            }
            for(int i = 0; i < teams.Count; i++)
            {
                Team t = teams[i];
                t.Members = GetMembers(t.Id);
            }
            return teams;
        }

        private Team GetTeamFromReader(SqlDataReader reader)
        {
            return new Models.Team
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
            };
        }

        internal Team GetTeam(int id)
        {
            SqlCommand command = new SqlCommand(null, connection);
            command.CommandText = "select * from [dbo].[teams] where [id] = " + id;
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        return GetTeamFromReader(reader);
                    }
                }
            }
            return null;
        }

        internal Team GetTeam(string name)
        {
            SqlCommand command = new SqlCommand(null, connection);
            command.CommandText = "select * from [dbo].[teams] where [name] like @name";
            SqlParameter nameParam = new SqlParameter("@name", SqlDbType.NVarChar, 50);
            nameParam.Value = name;
            command.Parameters.Add(nameParam);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        return GetTeamFromReader(reader);
                    }
                }
            }
            return null;
        }

        internal List<Hacker> GetMembers(int teamId)
        {
            SqlCommand command = new SqlCommand(null, connection);
            command.CommandText = "select * from [dbo].[hackers] where team = " + teamId;
            List<Hacker> list = new List<Hacker>();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        list.Add(GetHackerFromReader(reader));
                    }
                }
            }
            return list;
        }

        private Hacker GetHackerFromReader(SqlDataReader reader)
        {
            return new Hacker
            {
                Id = (int) reader["id"],
                First = (string) reader["first"],
                Last = (string)reader["last"],
                Email = (string)reader["email"],
                Team = (int)reader["team"],
                CheckedIn = Convert.ToBoolean((byte) reader["checkedin"]),
                Present = Convert.ToBoolean((byte)reader["present"])
            };
        }

        internal Team InsertNewTeam(Team value)
        {
            SqlCommand command = new SqlCommand(null, connection);
            command.CommandText = "insert into [dbo].[teams] (name) VALUES(@name)";
            SqlParameter nameParam = new SqlParameter("@name", SqlDbType.NVarChar, 50);
            nameParam.Value = value.Name;
            command.Parameters.Add(nameParam);
            try
            {
                command.ExecuteNonQuery();
                return GetTeam(value.Name);
            }
            catch (Exception)
            {
                return null;
            }
        }

        internal bool UpdateTeam(int id, Team value)
        {
            SqlCommand command = new SqlCommand(null, connection);
            command.CommandText = "update [dbo].[teams] set name=@name where id = " + id;
            command.CommandText = "insert into [dbo].[teams] (name) VALUES(@name)";
            SqlParameter nameParam = new SqlParameter("@name", SqlDbType.NVarChar, 50);
            nameParam.Value = value.Name;
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

        internal bool DeleteTeam(int id)
        {
            SqlCommand command = new SqlCommand(null, connection);
            command.CommandText = "delete from [dbo].[teams] where id = " + id;
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