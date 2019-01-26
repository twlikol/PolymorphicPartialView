using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolymorphicPartialView.Models
{
    public class UserDepartmentModel : UserExtendModel
    {
        public override string PartialView => "/Pages/Content/Department.cshtml";

        public string Manager { get; set; }
    }
}
