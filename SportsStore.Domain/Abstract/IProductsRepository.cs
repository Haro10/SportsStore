using System.CodeDom;
using System.Linq;
using System.Runtime.InteropServices;
using SportsStore.Domain.Entities;

namespace SportsStore.Domain.Abstract
{
    public interface IProductsRepository
    {
        IQueryable<Product> Products { get; }
    }

//    class MyClass2 : Interface1
//    {
//        public void Print()
//        {
//            
//        }
//    }
//    class MyClass
//    {
//        
//        Interface1 returnInteerface()
//        {
//            Interface1 inter = new MyClass2();
//            inter.Print();
//            return inter;
//        }
//    }
}