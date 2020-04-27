using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace libAPICache.Models.Teamwork
{
    [Table(name: "People", Schema = "TeamWork")]
    public class Person : Base
    {
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