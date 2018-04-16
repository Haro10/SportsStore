using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain;

namespace SportsStore.WebUI.Binders
{
    public class CartModelBinder : IModelBinder {

        /* We have removed the GetCart method and added a Cart parameter to each of the action methods.
   When the MVC Framework receives a request that requires, say, the AddToCart method to be invoked, it
   begins by looking at the parameters for the action method. It looks at the list of binders available and tries
   to find one that can create instances of each parameter type. Our custom binder is asked to create a Cart
   object, and it does so by working with the session state feature. Between our binder and the default binder*/
        private const string sessionKey = "Cart";
        public object BindModel(ControllerContext controllerContext,
        ModelBindingContext bindingContext)
        {
            // get the Cart from the session
            Cart cart = (Cart)controllerContext.HttpContext.Session[sessionKey];
            // create the Cart if there wasn't one in the session data
            if (cart == null)
            {
                cart = new Cart();
                controllerContext.HttpContext.Session[sessionKey] = cart;
            }
            // return the cart
            return cart;
        }
    }
}