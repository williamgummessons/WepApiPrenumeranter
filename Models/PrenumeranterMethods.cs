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



        public PrenumerantDetails? GetPrenumerant(int preNr, out string errormsg)
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
                        command.Parameters.AddWithValue("@Prennr", preNr);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new PrenumerantDetails
                                {
                                    pr_preNr = reader.GetInt32(0),
                                    pr_namn = reader.GetString(1),
                                    pr_teleNr = reader.GetString(2),
                                    pr_utAdress = reader.GetString(3),
                                    pr_postNr = reader.GetString(4),
                                    pr_ort = reader.GetString(5)
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
                        command.Parameters.AddWithValue("@Namn", prenumerant.pr_namn);
                        command.Parameters.AddWithValue("@Telenr", prenumerant.pr_teleNr);
                        command.Parameters.AddWithValue("@Utadress", prenumerant.pr_utAdress);
                        command.Parameters.AddWithValue("@Postnr", prenumerant.pr_postNr);
                        command.Parameters.AddWithValue("@Ort", prenumerant.pr_ort);
                        command.Parameters.AddWithValue("@Prennr", prenumerant.pr_preNr);

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
