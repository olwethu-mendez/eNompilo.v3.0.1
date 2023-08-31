using eNompilo.v3._0._1.Models.Counselling;
using eNompilo.v3._0._1.Models.Vaccination;
using eNompilo.v3._0._1.Models.SystemUsers;

namespace eNompilo.v3._0._1.Models.ViewModels
{
    public class PatientDashboardViewModel
    {
        public PatientFile PatientFiles { get; set; }
        public CounsellingAppointment CounsellingAppointments { get; set; }
        public VaccinationAppointment VaccinationAppointments { get; set; }
        public GeneralAppointment GeneralAppointments { get; set; }
    }
}
