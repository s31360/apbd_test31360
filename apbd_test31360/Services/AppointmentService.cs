using apbd_test31360.DTOs;
using apbd_test31360.Repositories;

namespace apbd_test31360.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IDoctorRepository _doctorRepository;
    private readonly IAppointmentServiceRepository _appointmentServiceRepository;
    private readonly IAppointmentWriteRepository _appointmentWriteRepository;
    
    public AppointmentService(IAppointmentRepository appointmentRepository, IPatientRepository patientRepository, IDoctorRepository doctorRepository, IAppointmentServiceRepository appointmentServiceRepository, IAppointmentWriteRepository appointmentWriteRepository)
    {
        _appointmentRepository = appointmentRepository;
        _patientRepository = patientRepository;
        _doctorRepository = doctorRepository;
        _appointmentServiceRepository = appointmentServiceRepository;
        _appointmentWriteRepository = appointmentWriteRepository;
    }

    public async Task<AppointmentDetailsDTO?> FindAppointmentAsync(int id)
    {
        var appointment = await _appointmentRepository.FindAppointmentByIdAsync(id);
        if (appointment == null)
            return null;

        var patient = await _patientRepository.FindPatientByIdAsync(id);
        if (patient == null)
            return null;
        
        var doctor = await _doctorRepository.FindDoctorByIdAsync(id);
        if (doctor == null)
            return null;
        
        var services = await _appointmentServiceRepository.FindServicesByAppointmentIdAsync(id);
        var serviceDtos = services.Select(s => new AppointmentServiceDTO
        {
            Name = s.Name,
            ServiceFee = s.ServiceFee
        }).ToList();
        
        return new AppointmentDetailsDTO
        {
            Date = appointment.Date,
            Patient = new PatientDTO
            {
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                DateOfBirth = patient.DateOfBirth,
            },
            Doctor = new DoctorDTO
            {
                DoctorId = doctor.DoctorId,
                pwz = doctor.pwz
            },
            AppointmentServices = serviceDtos
        };
    }
    
    public async Task AddAppointmentAsync(NewAppointmentRequestDTO dto)
    {
        await _appointmentWriteRepository.AddAppointmentAsync(dto);
    }
}