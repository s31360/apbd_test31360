using apbd_test31360.DTOs;

namespace apbd_test31360.Repositories;

public interface IAppointmentWriteRepository
{
    Task AddAppointmentAsync(NewAppointmentRequestDTO dto);
}