using apbd_test31360.DTOs;
using Microsoft.Data.SqlClient;

namespace apbd_test31360.Repositories;

public class AppointmentWriteRepository : IAppointmentWriteRepository
{
    private readonly IConfiguration _configuration;

    public AppointmentWriteRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task AddAppointmentAsync(NewAppointmentRequestDTO dto)
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        await connection.OpenAsync();
        using var transaction = connection.BeginTransaction();

        try
        {
            var checkAppointment = new SqlCommand("SELECT 1 FROM Appointment WHERE appointment_id = @Id", connection, transaction);
            checkAppointment.Parameters.AddWithValue("@Id", dto.AppointmentId);

            if (await checkAppointment.ExecuteScalarAsync() != null)
                throw new Exception("Appointment with given ID already exists.");

            var checkPatient = new SqlCommand("SELECT 1 FROM Patient WHERE patient_id = @Id", connection, transaction);
            checkPatient.Parameters.AddWithValue("@Id", dto.PatientId);

            if (await checkPatient.ExecuteScalarAsync() == null)
                throw new Exception("Patient not found.");

            var getDoctor = new SqlCommand("SELECT doctor_id FROM Doctor WHERE pwz = @Pwz", connection, transaction);
            getDoctor.Parameters.AddWithValue("@Pwz", dto.PWZ);

            var doctorObj = await getDoctor.ExecuteScalarAsync();
            if (doctorObj == null)
                throw new Exception("Doctor with given PWZ not found.");
            var doctorId = (int)doctorObj;

            var insertAppointment = new SqlCommand(@"
                INSERT INTO Appointment (appointment_id, patient_id, doctor_id, date)
                VALUES (@AppointmentId, @PatientId, @DoctorId, GETDATE())", connection, transaction);

            insertAppointment.Parameters.AddWithValue("@AppointmentId", dto.AppointmentId);
            insertAppointment.Parameters.AddWithValue("@PatientId", dto.PatientId);
            insertAppointment.Parameters.AddWithValue("@DoctorId", doctorId);

            await insertAppointment.ExecuteNonQueryAsync();

            foreach (var service in dto.Services)
            {
                var getService = new SqlCommand("SELECT service_id FROM Service WHERE name = @Name", connection, transaction);
                getService.Parameters.AddWithValue("@Name", service.ServiceName);

                var serviceIdObj = await getService.ExecuteScalarAsync();
                if (serviceIdObj == null)
                    throw new Exception($"Service not found: {service.ServiceName}");

                var serviceId = (int)serviceIdObj;

                var insertLink = new SqlCommand(@"
                    INSERT INTO Appointment_Service (appointment_id, service_id, service_fee)
                    VALUES (@AppointmentId, @ServiceId, @ServiceFee)", connection, transaction);

                insertLink.Parameters.AddWithValue("@AppointmentId", dto.AppointmentId);
                insertLink.Parameters.AddWithValue("@ServiceId", serviceId);
                insertLink.Parameters.AddWithValue("@ServiceFee", service.ServiceFee);

                await insertLink.ExecuteNonQueryAsync();
            }

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
