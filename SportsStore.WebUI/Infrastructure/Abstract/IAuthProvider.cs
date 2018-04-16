namespace SportsStore.WebUI.Infrastructure.Abstract
{
    /*The problem with calling static methods from within action methods is that it makes unit testing the
    controller difficult. Mocking frameworks such as Moq can mock only instance members. This problem
    arises because the FormsAuthentication class predates the unit-testing-friendly design of MVC.
     
      We can now create an implementation of this interface that acts as a wrapper around the static
    methods of the FormsAuthentication class. -- Page 286
     
     HaKM:  static methods from within action methods is that it makes unit testing the
    controller difficult, Mocking frameworks such as Moq can mock only instance members
     => use AddBindings method of the NinjectControllerFactory class
     to keep the static FormsAuthentication methods that out of the controller. - Page 287
     */

    public interface IAuthProvider
    {
        bool Authenticate(string username, string password);
    }
}