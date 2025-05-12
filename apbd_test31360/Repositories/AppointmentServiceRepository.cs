using Microsoft.Data.SqlClient;

namespace apbd_test31360.Repositories;

public class AppointmentServiceRepository : IAppointmentServiceRepository
{
    private readonly IConfiguration _configuration;

    public AppointmentServiceRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task<List<(string Name, decimal ServiceFee)>> FindServicesByAppointmentIdAsync(int appointmentId)
    {
        var result = new List<(string, decimal)>();
        
        const string query = @"
            SELECT s.name, aps.service_fee
            FROM Appointment_Service aps
            Join Service s on s.service_id = aps.service_id
            WHERE aps.appointment_id = @Id";
        
        using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        await connection.OpenAsync();
        
        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", appointmentId);
        
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var name = reader.GetString(0);
            var servicefee = reader.GetDecimal(1);
            result.Add((name, servicefee));
        }

        return result;
    }
}