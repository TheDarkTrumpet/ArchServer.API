using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace libAPICache.Models.Teamwork
{
    [Table(name: "Person", Schema = "TeamWork")]
    public class Person
    {
        [Key]
        public long Id { get; set; }
        [MaxLength(100)]
        public string UserName { get; set; }
        public DateTime? LastActive { get; set; }
        [MaxLength(100)]
        public string FullName { get; set; }
        [MaxLength(100)]
        public string EmailAddress { get; set; }
        [MaxLength(255)]
        public string CompanyName { get; set; }
        public bool Administrator { get; set; }
    }
}