using Microsoft.Data.SqlClient;
using Roommates.Models;
using System.Collections.Generic;

namespace Roommates.Repositories
{
    class RoommateRepository : BaseRepository
    {
        public RoommateRepository(string connectionString) : base(connectionString) { }
        public Roommate GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT FirstName, RentPortion, Room.Name
                                        FROM Roommate
                                        LEFT JOIN Room ON Room.Id = Roommate.RoomId
                                        WHERE Roommate.Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    Roommate roommate = null;

                    if (reader.Read())
                    {
                        roommate = new Roommate
                        {
                            Id = id,
                            Firstname = reader.GetString(reader.GetOrdinal("FirstName")),
                            RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                            Room = new Room
                            {
                                Id = id,
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                            }
                        };
                    }
                    reader.Close();
                    return roommate;
                }
            }
        }
    }
}
