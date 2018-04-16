using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain;
using SportsStore.Domain.Abstract;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.Controllers
{
    public class CartController : Controller
    {
        //
        // GET: /Cart/

        private IProductRepository repository;
        private IOrderProcessor orderProcessor;

        public CartController(IProductRepository repo, IOrderProcessor proc)
        {
            repository = repo;
            orderProcessor = proc;
        }

        public ViewResult Index(Cart cart, string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }

        /* Once again, we are relying on the model binder system, both for the ShippingDetails parameter (which is
        created automatically using the HTTP form data) and the Cart parameter (which is created using our custom binder)*/

        [HttpPost]
        public ViewResult Checkout(Cart cart, ShippingDetails shippingDetails)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            }
            if (ModelState.IsValid)
            {
                orderProcessor.ProcessOrder(cart, shippingDetails);
                cart.Clear();
                return View("Completed");
            }
            else
            {
                return View(shippingDetails);
            }
        }

        // HaKM: used parameter names that match the input
        //elements in the HTML forms we created in the ProductSummary.cshtml view. This allows the MVC
        //Framework to associate incoming form POST variables with those parameters.

        /* We have removed the GetCart method and added a Cart parameter to each of the action methods.
        When the MVC Framework receives a request that requires, say, the AddToCart method to be invoked, it
        begins by looking at the parameters for the action method. It looks at the list of binders available and tries
        to find one that can create instances of each parameter type. Our custom binder is asked to create a Cart
        object, and it does so by working with the session state feature. Between our binder and the default binder
         
          The MVC Framework is able to create the set of parameters required to call the action method,
        allowing us to refactor the controller so that it has no knowledge of how Cart objects are created when
        requests are received.*/

        //HaKM: When the MVC Framework receives a request AddToCart  => only in this actual case the binder is called 
        //=> If test project call controller => binder isn't called => It is reason why if we can use it to Test, 
        //because when test it does not call binder it means that it doesn't call Session["Cart"] => not have null error case
        //like when we use GetCart() to run test
        public RedirectToRouteResult AddToCart(Cart cart, int productId, string returnUrl)
        {
            Product product = repository.Products
                .FirstOrDefault(p => p.ProductID == productId);
            if (product != null)
            {
//                Cart cart = GetCart();
                cart.AddItem(product, 1);
            }
            return RedirectToAction("Index", new {returnUrl});
        }

        // HaKM: used parameter names that match the input
        //elements in the HTML forms we created in the ProductSummary.cshtml view. This allows the MVC
        //Framework to associate incoming form POST variables with those parameters.
        public RedirectToRouteResult RemoveFromCart(Cart cart, int productId, string returnUrl)
        {
            Product product = repository.Products
                .FirstOrDefault(p => p.ProductID == productId);
            if (product != null)
            {
                cart.RemoveLine(product);
            }
            return RedirectToAction("Index", new {returnUrl});
        }

        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }

        public ViewResult Checkout()
        {
            return View(new ShippingDetails());
        }

        // removed because now rely on our model binder called CartModelBinder in  global.asax
//        public Cart GetCart(){
//            Cart cart = (Cart) Session["Cart"];
//            if (cart == null)
//            {
//                cart = new Cart();
//                Session["Cart"] = cart;
//            }
//            return cart;
//        }
    }
}
