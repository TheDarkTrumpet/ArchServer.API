using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using libAPICache.Abstract;

namespace libAPICache.Models
{
    public abstract class Base
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }
    }
}