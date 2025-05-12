Useful commands for the quick checking

Task_1
link: http://localhost:5044/api/appointments/1

SQL query for checking:
SELECT * FROM Patient WHERE patient_id = 1;

Task_2
link: http://localhost:5044/api/appointments
body: 
{
    "appointmentId": 10,
    "patientId": 1,
    "pwz": "PWZ5678",
    "services": [
      {
          "serviceName": "Consultation",
          "serviceFee": 112.0
      },
      {
         "serviceName": "ECG",
          "serviceFee": 70.00
      }
    ]
}

SQL query for checking:
SELECT * FROM Appointment WHERE patient_id = 1;