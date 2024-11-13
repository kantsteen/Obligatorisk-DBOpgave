using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obligatorisk_DBOpgave
{
    class DBClient
    {
        string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=HotelDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        
        private int GetMaxFacilityNo(SqlConnection connection)
        {
            Console.WriteLine("Calling -> GetMaxFacilityNo");

            string queryStringMaxFacilityNo = "SELECT MAX(Facility_id) FROM DemoFacility";
            Console.WriteLine($"SQL applied: {queryStringMaxFacilityNo}");

            SqlCommand command = new SqlCommand(queryStringMaxFacilityNo, connection);
            SqlDataReader reader = command.ExecuteReader();

            int MaxFacilityNo = 0;

            if (reader.Read())
            {
                MaxFacilityNo = reader.GetInt32(0);
            }

            reader.Close();
            Console.WriteLine($"Max facility#: {MaxFacilityNo}");
            Console.WriteLine();

            return MaxFacilityNo;
        }

        private int DeleteFacility(SqlConnection connection, int facility_no)
        {
            Console.WriteLine("Calling -> DeleteFacility");

            string deleteCommandString = $"DELETE FROM DemoFacility WHERE Facility_id ={facility_no}";
            Console.WriteLine($"SQL applied: {deleteCommandString}");

            SqlCommand command = new SqlCommand(deleteCommandString, connection);
            Console.WriteLine($"Deleting Facility #{facility_no}");
            int numberOfRowsAffected = command.ExecuteNonQuery();

            Console.WriteLine($"Number of rows affected: {numberOfRowsAffected}");
            Console.WriteLine();

            return numberOfRowsAffected;

        }

        private int UpdateFacility(SqlConnection connection, Facility facility)
        {
            Console.WriteLine("Calling -> UpdateFacility");

            string updateCommandString = $"UPDATE DemoFacility SET Name='{facility.Name}' WHERE Facility_id = {facility.FacilityId}";
            Console.WriteLine($"SQL applied: {updateCommandString}");

            SqlCommand command = new SqlCommand(updateCommandString, connection);
            Console.WriteLine($"Updating facility #{facility.FacilityId}");
            int numberOfRowsAffected = command.ExecuteNonQuery();

            Console.WriteLine($"Number of rows affected: {numberOfRowsAffected}");
            Console.WriteLine();

            return numberOfRowsAffected;
        }

        private int InsertFacility(SqlConnection connection, Facility facility)
        {
            Console.WriteLine("Calling -> InsertFacility");

            string insertCommandString = $"INSERT INTO DemoFacility VALUES({facility.FacilityId}, '{facility.Name}', '{facility.HotelNo}')";
            Console.WriteLine($"SQL applied: {insertCommandString}");

            SqlCommand command = new SqlCommand(insertCommandString, connection);

            Console.WriteLine($"Creating facility #{facility.FacilityId}");
            int numberOfRowsAffected = command.ExecuteNonQuery();

            Console.WriteLine($"Number of rows affected: {numberOfRowsAffected}");
            Console.WriteLine();

            return numberOfRowsAffected;
        }

        private List<Facility> ListAllFacilities(SqlConnection connection)
        {
            Console.WriteLine("Calling -> ListAllFacilities");

            string queryStringAllFacilities = "SELECT * FROM DemoFacility";
            Console.WriteLine($"SQL applied: {queryStringAllFacilities}");

            SqlCommand command = new SqlCommand(queryStringAllFacilities, connection);
            SqlDataReader reader = command.ExecuteReader();

            Console.WriteLine("Listing all facilities:");

            if (!reader.HasRows)
            {
                Console.WriteLine("No facilities in database");
                reader.Close();

                return null;
            }

            List<Facility> facilities = new List<Facility>();
            while (reader.Read())
            {
                Facility nextFacility = new Facility()
                {
                    FacilityId = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    HotelNo = reader.GetInt32(2)
                };

                facilities.Add(nextFacility);

                Console.WriteLine(nextFacility);
            }

            reader.Close();
            Console.WriteLine();

            return facilities;
        }

        private Facility GetFacility(SqlConnection connection, int facility_no)
        {
            Console.WriteLine("Calling -> GetFacility");

            string queryStringOneFacility = $"SELECT * FROM DemoFacility WHERE facility_id = {facility_no}";
            Console.WriteLine($"SQL applied: {queryStringOneFacility}");

            SqlCommand command = new SqlCommand(queryStringOneFacility, connection);
            SqlDataReader reader = command.ExecuteReader();

            Console.WriteLine($"Finding facility#: {facility_no}");

            if (!reader.HasRows)
            {
                Console.WriteLine("No facility in database");
                reader.Close();

                return null;
            }

            Facility facility = null;
            if (reader.Read())
            {
                facility = new Facility()
                {
                    FacilityId = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    HotelNo = reader.GetInt32(2)
                };

                Console.WriteLine(facility);
            }

            reader.Close();
            Console.WriteLine();

            return facility;
        }

        public void Start()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //Open connection
                connection.Open();

                //List all facilities in the database
                ListAllFacilities(connection);

                //Create a new facilities with primary key equal to current max primary key plus 1
                Facility newFacility = new Facility()
                {
                    FacilityId = GetMaxFacilityNo(connection) + 1,
                    Name = "New Facility",
                    HotelNo = 1
                };

                //Insert the facilities into the database
                InsertFacility(connection, newFacility);

                //List all facilities including the the newly inserted one
                ListAllFacilities(connection);

                //Get the newly inserted facilities from the database in order to update it 
                Facility facilityToBeUpdate = GetFacility(connection, newFacility.FacilityId);

                //Alter Name
                facilityToBeUpdate.Name += "(updated)";

                //Update the facility in the database 
                UpdateFacility(connection, facilityToBeUpdate);

                //List all facilities including the updated one
                ListAllFacilities(connection);

                //Get the updated facility in order to delete it
                Facility facilityToBeDeleted = GetFacility(connection, facilityToBeUpdate.FacilityId);

                //Delete the facility
                DeleteFacility(connection, facilityToBeDeleted.FacilityId);

                //List all facilities - now without the deleted one
                ListAllFacilities(connection);
            }

        }
    }
}
