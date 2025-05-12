using apbd_test31360.Models;

namespace apbd_test31360.Repositories;

public interface IAppointmentServiceRepository
{
    Task<List<(string Name, decimal ServiceFee)>> FindServicesByAppointmentIdAsync(int appointmentId);
}