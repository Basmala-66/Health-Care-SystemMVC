using System.ComponentModel.DataAnnotations;
namespace Health_Care_System.PL.ViewModels;
public class PatientViewModel { public int Id{get;set;} [Required,StringLength(50,MinimumLength=3)] public string Name{get;set;}=string.Empty; [DataType(DataType.Date),Required] public DateTime BirthDate{get;set;} }
