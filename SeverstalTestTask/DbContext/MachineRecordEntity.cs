using SeverstalTestTask.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeverstalTestTask.Db
{
    public class MachineRecordEntity : IMachineRecord
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MachineNumber { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal GrossWeight { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TareWeight { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal NetWeight { get; set; }

        [Required]
        public DateTime TareDate { get; set; }

        [Required]
        public DateTime GrossDate { get; set; }

        public MachineRecordEntity() { }
    }
}
