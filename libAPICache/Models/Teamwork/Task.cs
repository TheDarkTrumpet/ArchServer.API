using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace libAPICache.Models.Teamwork
{
    [Table(name: "Tasks", Schema = "Teamwork")]
    public class Task
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }
        [MaxLength(255)]
        public string ProjectName { get; set; }
        [MaxLength(255)]
        public string CompanyName { get; set; }
        [MaxLength(255)]
        public string Title { get; set; }
        public string Description { get; set; }
        [MaxLength(20)]
        public string Priority { get; set; }
        [MaxLength(50)]
        public string AssignedTo { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? DueDate { get; set; }
        public Boolean Completed { get; set; }
    }
}