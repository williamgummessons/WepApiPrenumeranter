using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace WepApiPrenumeranter.Models
{
    public class PrenumeranterMethods
    {
        private readonly string _connectionString;

        public PrenumeranterMethods(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }



        public PrenumerantDetails? GetPrenumerant(int prennr, out string errormsg)
        {
            errormsg = string.Empty;
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM tbl_prenumeranter WHERE pr_prennr = @Prennr";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Prennr", prennr);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new PrenumerantDetails
                                {
                                    Prennr = reader.GetInt32(0),
                                    Namn = reader.GetString(1),
                                    Telenr = reader.GetString(2),
                                    Utadress = reader.GetString(3),
                                    Postnr = reader.GetString(4),
                                    Ort = reader.GetString(5)
                                };
                            }
                            else
                            {
                                errormsg = "Prenumerant not found.";
                                return null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errormsg = $"Error retrieving prenumerant: {ex.Message}";
                return null;
            }
        }

        public PrenumerantDetails? EditPrenumerant(PrenumerantDetails prenumerant, out string errormsg)
        {
            errormsg = string.Empty;
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE tbl_prenumeranter SET pr_namn = @Namn, pr_telenr = @Telenr, pr_utadress = @Utadress, pr_postnr = @Postnr, pr_ort = @Ort WHERE pr_prennr = @Prennr";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Namn", prenumerant.Namn);
                        command.Parameters.AddWithValue("@Telenr", prenumerant.Telenr);
                        command.Parameters.AddWithValue("@Utadress", prenumerant.Utadress);
                        command.Parameters.AddWithValue("@Postnr", prenumerant.Postnr);
                        command.Parameters.AddWithValue("@Ort", prenumerant.Ort);
                        command.Parameters.AddWithValue("@Prennr", prenumerant.Prennr);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected == 0)
                        {
                            errormsg = "Prenumerant not found or data is unchanged.";
                            return null;
                        }
                        return prenumerant;
                    }
                }
            }
            catch (Exception ex)
            {
                errormsg = $"Error updating prenumerant: {ex.Message}";
                return null;
            }
        }
    }
}
