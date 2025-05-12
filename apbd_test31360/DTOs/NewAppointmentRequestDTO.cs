namespace apbd_test31360.DTOs;

public class NewAppointmentRequestDTO
{
    public int AppointmentId { get; set; }
    public int PatientId { get; set; }
    public string PWZ { get; set; } = null!;
    public List<ServiceInAppointmentDTO> Services { get; set; } = new();
}