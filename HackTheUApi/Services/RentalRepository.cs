using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HackTheUApi.Models;
using System.Data.SqlClient;
using System.Data;

namespace HackTheUApi.Services
{
    public class RentalRepository
    {
        SqlConnection connection;

        private const string connection_string = "Server=tcp:hacktheu.database.windows.net,1433;Initial Catalog=hacktheudb;Persist Security Info=False;User ID=hacktheu;Password=Augmentation801;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        private const string allRentalsQuery = "select * from rentals r " +
                                   " left outer join " +
                                   " ( select id as hackId, first, last, email, team, checkedin, present " +
                                    " from hackers ) as hack on(r.hacker = hack.hackId) " +
                                    "left outer join " +
                                   " ( select id as hardId, name, category, owner, available " +
                                    " from hardware ) as hard on(r.hardware = hard.hardId) ";
        public RentalRepository()
        {
            connection = new SqlConnection(connection_string);
            connection.Open();
        }
        internal IEnumerable<Rental> GetAllRentals()
        {
            SqlCommand command = new SqlCommand(null, connection);
            command.CommandText = allRentalsQuery;
            List<Rental> list = new List<Rental>();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        list.Add(GetRentalFromReader(reader));
                    }
                }
            }
            return list;
        }

        public Rental GetRentalFromReader(SqlDataReader reader)
        {
            Rental r = new Rental();
            r.Hacker = GetHackerFromReader(reader);
            r.Hardware = GetHardwareFromReader(reader);
            try
            {
                r.CheckOut = reader.GetDateTime(2);
            }
            catch (Exception) { }
            try
            {
                r.CheckIn = reader.GetDateTime(3);
            }
            catch (Exception) { }
            return r;            
        }

        private Hardware GetHardwareFromReader(SqlDataReader reader)
        {
            return new Hardware
            {
                Id = (int)reader["hardId"],
                Name = (string)reader["name"],
                Category = (string)reader["category"],
                Owner = (string)reader["owner"]
            };
        }

        private Hacker GetHackerFromReader(SqlDataReader reader)
        {
            return new Hacker
            {
                Id = (int)reader["hackId"],
                First = (string)reader["first"],
                Last = (string)reader["last"],
                Email = (string)reader["email"],
                Team = (int)reader["team"],
                CheckedIn = Convert.ToBoolean((byte)reader["checkedin"]),
                Present = Convert.ToBoolean((byte)reader["present"])
            };
        }

        internal Rental GetHardwareRental(int id)
        {
            throw new NotImplementedException();
        }

        internal Rental GetHackerRental(int id)
        {
            throw new NotImplementedException();
        }

        internal Rental GetCurrentRentals()
        {
            throw new NotImplementedException();
        }

        internal bool CheckOut(int hardware, int hacker)
        {
            SqlCommand command = new SqlCommand(null, connection);
            command.CommandText = "insert into [dbo].[rentals] (hardware,hacker,checkout) VALUES (@hardware,@hacker,@checkout)";

            SqlParameter hardwareParam = new SqlParameter("@hardware",SqlDbType.Int);
            hardwareParam.Value = hardware;
            command.Parameters.Add(hardwareParam);

            SqlParameter hackerParam = new SqlParameter("@hacker", SqlDbType.Int);
            hackerParam.Value = hacker;
            command.Parameters.Add(hackerParam);

            SqlParameter checkOutParam = new SqlParameter("@checkout", SqlDbType.DateTime);
            checkOutParam.Value = DateTime.Now;
            command.Parameters.Add(checkOutParam);

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

        internal bool CheckIn(int hardware, int hacker)
        {
            throw new NotImplementedException();
        }
    }
}