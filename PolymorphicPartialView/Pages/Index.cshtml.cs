using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PolymorphicPartialView.Models;

namespace PolymorphicPartialView.Pages
{
    public class IndexModel : PageModel
    {

        private UserModel entity = null;
        private UserExtendModelCollection extendEntities = null;

        [BindProperty]
        public UserModel Entity
        {
            get
            {
                if (this.entity == null)
                {
                    UserModel userModel = new UserModel();
                    userModel.DisplayName = "Anyone";

                    this.entity = userModel;
                }

                return this.entity;
            }
            set
            {
                this.entity = value;
            }
        }

        [BindProperty]
        public UserExtendModelCollection ExtendEntities
        {
            get
            {
                if (this.extendEntities == null)
                {
                    UserBusinessModel userBusinessModel = new UserBusinessModel
                    {
                        CostID = "123456789"
                    };

                    UserDepartmentModel userDepartmentModel = new UserDepartmentModel
                    {
                        Manager = "Someone"
                    };

                    UserExtendModelCollection extendModels = new UserExtendModelCollection
                    {
                        userBusinessModel,
                        userDepartmentModel
                    };

                    this.extendEntities = extendModels;
                }

                return this.extendEntities;
            }
            set
            {
                this.extendEntities = value;
            }
        }

        public void OnGet()
        {
            
        }

        public ActionResult OnPost()
        {
            UserModel userModel = this.Entity;

            List<UserExtendModel> userExtendModel = this.ExtendEntities;

            return Page();
        }
    }
}