using AutoMapper; using Health_Care_System.Entities; using Health_Care_System.PL.ViewModels;
namespace Health_Care_System.PL.Helper;
public class HealthCareProfile:Profile { public HealthCareProfile(){CreateMap<Patient,PatientViewModel>().ReverseMap();CreateMap<Doctor,DoctorViewModel>().ReverseMap();CreateMap<Appointment,AppointmentViewModel>().ForMember(d=>d.PatientName,o=>o.MapFrom(s=>s.Patient.Name)).ForMember(d=>d.DoctorName,o=>o.MapFrom(s=>s.Doctor.Name));CreateMap<AppointmentViewModel,Appointment>();} }
