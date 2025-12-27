using SignalR.EntityLayer.DAL.Entities;

namespace SignalR.BusinessLayer.Abstract
{
    public interface IProductService:IGenericService<Product>
    {

        List<Product> TGetProductsWithCategories();
        int TProductCount();

        int TProductCountByCategoryNameHamburger();
        int TProductCountByCategoryNameDrink();
        decimal TProductPriceAvg();

        string TProductNameByMaxPrice();
        string TProductNameByMinPrice();
        decimal TProductAvgPriceByHamburger();
        List<Product> TGetLast9Products();
    }
}
