using apbd_test31360.Models;
using Microsoft.Data.SqlClient;

namespace apbd_test31360.Repositories;

public class PatientRepository : IPatientRepository
{
    private readonly IConfiguration _configuration;

    public PatientRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task<Patient?> FindPatientByIdAsync(int id)
    {
        const string query = @"
            SELECT patient_id, first_name, last_name, date_of_birth
            FROM patient
            WHERE patient_id = @Id";
        
        using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        await connection.OpenAsync();
        
        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);
        
        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Patient
            {
                PatientId = reader.GetInt32(0),
                FirstName = reader.GetString(1),
                LastName = reader.GetString(2),
                DateOfBirth = reader.GetDateTime(3),
            };
        }

        return null;
    }
}