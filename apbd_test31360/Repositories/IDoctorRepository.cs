using apbd_test31360.Models;

namespace apbd_test31360.Repositories;

public interface IDoctorRepository
{
    Task<Doctor?> FindDoctorByIdAsync(int id);
}