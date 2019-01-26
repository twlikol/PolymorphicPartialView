## Polymorphic Partial View for AspNetCore

Testing in AspNetCore 2.2

### Configure Services
```cs
public void ConfigureServices(IServiceCollection services)
{
    services.AddMvc(options =>
    {
        options.ModelBinderProviders.Insert(0, new PolymorphicPartialViewModelBinderProvider());
    });
}
```
### Models
```cs
public class UserExtendModel {
    public virtual string PartialView { get; set; }
}

public class UserExtendModelCollection : List<UserExtendModel> {

}

public class UserBusinessModel : UserExtendModel {
    public override string PartialView => "/Pages/Content/Business.cshtml";

    [Required]
    public string CostID { get; set; }
}

public class UserDepartmentModel : UserExtendModel {
    public override string PartialView => "/Pages/Content/Department.cshtml";

    public string Manager { get; set; }
}
```
### PartialView
```html
@model PolymorphicPartialView.Models.UserBusinessModel

<input type="hidden" asp-for="PartialView" />
<div>
    <div class="header">CostID:</div>
    <div class="item">
        <input type="text" asp-for="CostID" />
        <span asp-validation-for="CostID"></span>
    </div>
</div>
```
```html
@model PolymorphicPartialView.Models.UserDepartmentModel

<input type="hidden" asp-for="PartialView" />
<div>
    <div class="header">Manager:</div>
    <div class="item"><input type="text" asp-for="Manager" /></div>
</div>
```
### PageModel
```cs
public class IndexModel : PageModel {
    private UserModel entity = null;
    private UserExtendModelCollection extendEntities = null;

    [BindProperty]
    public UserModel Entity {
        get {
            if (this.entity == null) {
                UserModel userModel = new UserModel();
                userModel.DisplayName = "Anyone";

                this.entity = userModel;
            }

            return this.entity;
        }
        set {
            this.entity = value;
        }
    }

    [BindProperty]
    public UserExtendModelCollection ExtendEntities {
        get {
            if (this.extendEntities == null) {
                UserBusinessModel userBusinessModel = new UserBusinessModel {
                    CostID = "123456789"
                };

                UserDepartmentModel userDepartmentModel = new UserDepartmentModel {
                    Manager = "Someone"
                };

                UserExtendModelCollection extendModels = new UserExtendModelCollection {
                    userBusinessModel, userDepartmentModel
                };

                this.extendEntities = extendModels;
            }

            return this.extendEntities;
        }
        set {
            this.extendEntities = value;
        }
    }

    public void OnGet() {
            
    }

    public ActionResult OnPost() {
        UserModel userModel = this.Entity;

        List<UserExtendModel> userExtendModel = this.ExtendEntities;

        return Page();
    }
}
```

### PageView
```html
<form method="post">
    <div>
        <div class="header">DisplayName:</div>
        <div class="item"><input type="text" asp-for="Entity.DisplayName" /></div>
    </div>
    @for (int i = 0; i < Model.ExtendEntities.Count; i++)
    {
        ViewContext.ViewData.TemplateInfo.HtmlFieldPrefix = string.Format("ExtendEntities[{0}]", i);

        @await Html.PartialAsync(Model.ExtendEntities[i].PartialView, Model.ExtendEntities[i]);
    }
    <div>
        <input type="submit" value="Post" />
    </div>
</form>
```

### Copyright and License
Copyright © Likol Lee. Licensed under the MIT license.
