using apbd_test31360.Models;
using Microsoft.Data.SqlClient;

namespace apbd_test31360.Repositories;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly IConfiguration _configuration;

    public AppointmentRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task<Appointment?> FindAppointmentByIdAsync(int id)
    {
        const string query = @"
            SELECT appointment_id, patient_id, doctor_id, date
            FROM appointment
            WHERE appointment_id = @Id";
        
        using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        await connection.OpenAsync();
        
        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);
        
        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Appointment
            {
                AppointmentId = reader.GetInt32(0),
                PatientId = reader.GetInt32(1),
                DoctorId = reader.GetInt32(2),
                Date = reader.GetDateTime(3),
            };
        }

        return null;
    }
}