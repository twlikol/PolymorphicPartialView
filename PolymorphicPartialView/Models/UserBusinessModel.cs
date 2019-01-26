using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PolymorphicPartialView.Models
{
    public class UserBusinessModel : UserExtendModel
    {
        public override string PartialView => "/Pages/Content/Business.cshtml";

        [Required]
        public string CostID { get; set; }
    }
}
