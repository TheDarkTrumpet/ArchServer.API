using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace libAPICache.Models.Toggl
{
    [Table(name: "Workspaces", Schema = "Toggl")]
    public class Workspace : Base
    {
        [MaxLength(100)]
        public string Name { get; set; }
        public bool Premium { get; set; }
    }
}