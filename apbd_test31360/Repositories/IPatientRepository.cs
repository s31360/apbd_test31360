using apbd_test31360.Models;

namespace apbd_test31360.Repositories;

public interface IPatientRepository
{
    Task<Patient?> FindPatientByIdAsync(int id);
}