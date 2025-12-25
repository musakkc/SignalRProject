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
                return View(values);
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
    }
}
