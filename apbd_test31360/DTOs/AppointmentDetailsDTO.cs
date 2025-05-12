namespace apbd_test31360.DTOs;

public class AppointmentDetailsDTO
{
    public DateTime Date { get; set; }
    public PatientDTO Patient { get; set; } = null!;
    public DoctorDTO Doctor { get; set; } = null!;
    public List<AppointmentServiceDTO> AppointmentServices { get; set; } = new();
}