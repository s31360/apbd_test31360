using apbd_test31360.DTOs;

namespace apbd_test31360.Services;

public interface IAppointmentService
{
    Task<AppointmentDetailsDTO?> FindAppointmentAsync(int id);
    Task AddAppointmentAsync(NewAppointmentRequestDTO dto); 
}