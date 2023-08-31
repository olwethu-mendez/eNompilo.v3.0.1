using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eNompilo.v3._0._1.Models
{
    public class SessionNotes
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Practitioner's Notes")]
        public string PractitionerNotes { get; set; }

        [Required]
        [Display(Name = "Does patient indicate any signs for a potential condition?")]
        public bool ConditionIndication { get; set; }

        [Display(Name = "What potential condition may the patient have?")]
        public string? PotentialCondition { get; set; }

        public List<PrescriptionMeds>? PrescriptionMeds { get; set; }

        public int SessionId { get; set; }
        [ForeignKey("SessionId")]
        public Session Session { get; set; }

        [Required]
        public bool Archived { get; set; }
    }
}
