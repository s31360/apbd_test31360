using apbd_test31360.Models;

namespace apbd_test31360.Repositories;

public interface IAppointmentRepository
{
    Task<Appointment?> FindAppointmentByIdAsync(int id);
}