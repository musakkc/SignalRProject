using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SignalRWebUI.Dtos.OrderDtos;

namespace SignalRWebUI.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public OrdersController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // Admin: Tüm siparişleri listele
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7017/api/Order");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultOrderDto>>(jsonData);
                
                // Yeni den eskiye sırala
                var sortedValues = values.OrderByDescending(x => x.OrderDate).ToList();
                
                return View(sortedValues);
            }
            return View();
        }

        // Siparişi onayla
        public async Task<IActionResult> ApproveOrder(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7017/api/Order/ChangeStatusToApproved/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Sipariş onaylandı!";
            }
            else
            {
                TempData["ErrorMessage"] = "Sipariş onaylanırken bir hata oluştu.";
            }
            
            return RedirectToAction("Index");
        }

        // Siparişi teslim et
        public async Task<IActionResult> DeliverOrder(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7017/api/Order/ChangeStatusToDelivered/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Sipariş teslim edildi!";
            }
            else
            {
                TempData["ErrorMessage"] = "Sipariş teslim edilirken bir hata oluştu.";
            }
            
            return RedirectToAction("Index");
        }

        // Siparişi iptal et (sil)
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.DeleteAsync($"https://localhost:7017/api/Order/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Sipariş iptal edildi!";
            }
            else
            {
                TempData["ErrorMessage"] = "Sipariş iptal edilirken bir hata oluştu.";
            }
            
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
