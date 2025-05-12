using apbd_test31360.Models;
using Microsoft.Data.SqlClient;

namespace apbd_test31360.Repositories;

public class DoctorRepository : IDoctorRepository
{
    private readonly IConfiguration _configuration;

    public DoctorRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task<Doctor?> FindDoctorByIdAsync(int id)
    {
        const string query = @"
            SELECT doctor_id, first_name, last_name, pwz
            FROM doctor
            WHERE doctor_id = @Id";
        
        using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        await connection.OpenAsync();
        
        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);
        
        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Doctor
            {
                DoctorId = reader.GetInt32(0),
                FirstName = reader.GetString(1),
                LastName = reader.GetString(2),
                pwz = reader.GetString(3),
            };
        }

        return null;
    }
}