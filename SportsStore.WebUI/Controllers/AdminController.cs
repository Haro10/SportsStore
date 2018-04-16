using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain;
using SportsStore.Domain.Abstract;

namespace SportsStore.WebUI.Controllers
{
    /*When applied without any parameters, the Authorize attribute grants access to the controller action
    methods if the user is authenticated. This means that if you are authenticated, you are automatically
    authorized to use the administration features. This is fine for SportsStore, where there is only one set of
    restricted action methods and only one user. - Page 285*/

    /*■ Note You can apply filters to an individual action method or to a controller. When you apply a filter to a
    controller, it works as though you had applied it to every action method in the controller class. In Listing 11-2, we
    applied the Authorize filter to the class, so all of the action methods in the Admin controller are available only to
    authenticated users.  - Page 285*/

    /*When you try to access the Index action method of the Admin controller, the MVC Framework detects
    the Authorize filter. Because you have not been authenticated, you are redirected to the URL specified in
     the Web.config forms authentication section: /Account/Login. We have not created the Account controller
    yet—which is what causes the error shown in the figure—but the fact that the MVC Framework has tried
    to instantiate an AccountController class shows us that the Authorize attribute is working. - Page 285*/
    [Authorize]
    public class AdminController : Controller
    {
        private IProductRepository repository;
        public AdminController(IProductRepository repo)
        {
            repository = repo;
        }
        public ViewResult Index()
        {
            return View(repository.Products);
        }

        public ViewResult Edit(int productId)
        {
            Product product = repository.Products
            .FirstOrDefault(p => p.ProductID == productId);
            return View(product);
        }

        /*Notice that we return the ActionResult type from the Edit method. We’ve been using the ViewResult
        type until now. ViewResult is derived from ActionResult, and it is used when you want the framework to
        render a view. However, other types of ActionResults are available, and one of them is returned by the
        RedirectToAction method. We use that in the Edit action method to invoke the Index action method.*/

        // HttpPostedFileBase image We have added a new parameter to the Edit method, which the MVC Framework uses to pass the uploaded file data to us
        [HttpPost]
        public ActionResult Edit(Product product, HttpPostedFileBase image) 
        {

            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    product.ImageMimeType = image.ContentType;
                    product.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(product.ImageData, 0, image.ContentLength);
                }
                repository.SaveProduct(product);

                /*After we have saved the changes in the repository, we store a message using the Temp Data feature.
                This is a key/value dictionary similar to the session data and view bag features we have used previously.
                The key difference from session data is that temp data is deleted at the end of the HTTP request
                 We cannot use ViewBag in this situation because the user is being redirected. ViewBag passes data
                between the controller and view, and it cannot hold data for longer than the current HTTP request. We
                could have used the session data feature, but then the message would be persistent until we explicitly
                removed it, which we would rather not have to do. So, the Temp Data feature is the perfect fit*/
                TempData["message"] = string.Format("{0} has been saved", product.Name);
                return RedirectToAction("Index");
            }
            else
            {
                // there is something wrong with the data values
                return View(product);
            }
        }

        /*The Create method does not render its default view. Instead, it specifies that the Edit view should be
        used. It is perfectly acceptable for one action method to use a view that is usually associated with another
        view. In this case, we inject a new Product object as the view model so that the Edit view is populated with
        empty fields.*/
        public ViewResult Create()
        {
            return View("Edit", new Product());
        }

        [HttpPost]
        public ActionResult Delete(int productId)
        {
            Product deletedProduct = repository.DeleteProduct(productId);
            if (deletedProduct != null)
            {
                TempData["message"] = string.Format("{0} was deleted",
                deletedProduct.Name);
            }
            return RedirectToAction("Index");
        }
    }
}