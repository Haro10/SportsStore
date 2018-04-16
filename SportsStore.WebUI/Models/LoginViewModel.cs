using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SportsStore.WebUI.Models
{
    /*Given that there are only two properties, you might be tempted to do without a view model and rely
on the ViewBag to pass data to the view. However, it is good practice to define view models so that the data
passed from the controller to the view and from the model binder to the action method is typed
consistently. This allows us to use template view helpers more easily. - page 288*/

    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; }

        /*The DataType attribute has led the MVC Framework to render the editor for the Password property as
        an HTML password-input element, which means that the characters in the password are not visible. */
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}