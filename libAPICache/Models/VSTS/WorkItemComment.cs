using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace libAPICache.Models.VSTS
{
    [Table(name: "WorkItemComment", Schema = "VSTS")]
    public class WorkItemComment
    {
        [Key]
        public int id { get; set; }
        [MaxLength(200)]
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Comment { get; set; }
    }
}