using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SignalRWebUI.Dtos.BasketDtos;
using SignalRWebUI.Dtos.OrderDtos;
using System.Text;

namespace SignalRWebUI.Controllers
{
    [AllowAnonymous]
    public class BasketsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BasketsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index(int id)
        {
            TempData["id"] = id;
            var client = _httpClientFactory.CreateClient();
             
            var responseMessage = await client.GetAsync("https://localhost:7017/api/Basket/BasketListByMenuTableWithProductName?id="+id);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultBasketDto>>(jsonData);
                 
                var orderResponse = await client.GetAsync($"https://localhost:7017/api/Order/GetByTableNumber/{id}");
                if (orderResponse.IsSuccessStatusCode)
                {
                    var orderJsonData = await orderResponse.Content.ReadAsStringAsync();
                    var orders = JsonConvert.DeserializeObject<List<ResultOrderDto>>(orderJsonData);
                     
                    ViewBag.Orders = orders?.OrderByDescending(o => o.OrderDate).ToList();
                }
                
                return View(values);
            }
            return View();
        }
        public async Task<IActionResult> DeleteBasket(int id)
        { 
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.DeleteAsync($"https://localhost:7017/api/Basket/{id}");
            
            // TempData'dan menuTableId'yi al (null kontrolü ile)
            var menuTableId = TempData["id"];
            TempData.Keep("id"); // TempData'yı bir sonraki request için koru
            
            if (responseMessage.IsSuccessStatusCode)
            {
                if (menuTableId != null)
                {
                    return RedirectToAction("Index", new { id = menuTableId });
                }
                return RedirectToAction("Index");
            }
            return NoContent();
        }
         
        public async Task<IActionResult> CompleteOrder(int id)
        {
            var client = _httpClientFactory.CreateClient();
             
            var basketResponse = await client.GetAsync($"https://localhost:7017/api/Basket/BasketListByMenuTableWithProductName?id={id}");
            decimal totalAmount = 0;
            
            if (basketResponse.IsSuccessStatusCode)
            {
                var jsonData = await basketResponse.Content.ReadAsStringAsync();
                var baskets = JsonConvert.DeserializeObject<List<ResultBasketDto>>(jsonData);
                
                if (baskets != null && baskets.Count > 0)
                {
                    totalAmount = baskets.Sum(b => b.Price * b.Count);
                     
                    await client.PostAsync($"https://localhost:7017/api/MoneyCases?totalAmount={totalAmount}", null);
                     
                    foreach (var basket in baskets)
                    {
                        await client.DeleteAsync($"https://localhost:7017/api/Basket/{basket.BasketID}");
                    }
                }
            }
             
            await client.DeleteAsync($"https://localhost:7017/api/Order/DeleteByTableNumber/{id}");
             
            await client.GetAsync($"https://localhost:7017/api/MenuTables/ChangeMenuTableStatusToFalse?id={id}");
            
            // Ana sayfaya yönlendir
            return RedirectToAction("Index", "Default");
        } 
        public async Task<IActionResult> ConfirmOrder(int id)
        {
            var client = _httpClientFactory.CreateClient();
             
            var basketResponse = await client.GetAsync($"https://localhost:7017/api/Basket/BasketListByMenuTableWithProductName?id={id}");
            if (basketResponse.IsSuccessStatusCode)
            {
                var jsonData = await basketResponse.Content.ReadAsStringAsync();
                var baskets = JsonConvert.DeserializeObject<List<ResultBasketDto>>(jsonData);
                
                if (baskets != null && baskets.Count > 0)
                { 
                    var description = string.Join(", ", baskets.Select(b => $"{b.ProductName} x{b.Count}"));
                     
                    var totalPrice = baskets.Sum(b => b.Price * b.Count);
                     
                    var orderDto = new CreateOrderDto
                    {
                        TableNumber = id.ToString(),
                        Description = description,
                        TotalPrice = totalPrice,
                        Status = "Beklemede"  
                    };
                    
                    var orderJson = JsonConvert.SerializeObject(orderDto);
                    var orderContent = new StringContent(orderJson, Encoding.UTF8, "application/json");
                    await client.PostAsync("https://localhost:7017/api/Order", orderContent);
                    
                    // Sepeti temizle
                    foreach (var basket in baskets)
                    {
                        await client.DeleteAsync($"https://localhost:7017/api/Basket/{basket.BasketID}");
                    }
                }
            }
            
            TempData["SuccessMessage"] = "Siparişiniz mutfağa gönderildi!";
            return RedirectToAction("Index", new { id = id });
        } 

    }
}
