using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolymorphicPartialView.Models
{
    public class UserExtendModel
    {
        public virtual string PartialView { get; set; }
    }

    public class UserExtendModelCollection : List<UserExtendModel>
    {

    }
}
