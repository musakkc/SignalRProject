using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QRCoder;
using SignalRWebUI.Dtos.MenuTabeDtos;
using System.Drawing;
using System.Drawing.Imaging;

namespace SignalRWebUI.Controllers
{
    public class QRCodeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public QRCodeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        { 
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7017/api/MenuTables");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var tables = JsonConvert.DeserializeObject<List<ResultMenuTableDto>>(jsonData);
                ViewBag.Tables = tables;
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(int menuTableId)
        { 
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7017/api/MenuTables");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var tables = JsonConvert.DeserializeObject<List<ResultMenuTableDto>>(jsonData);
                ViewBag.Tables = tables;
            }
             
            string siteUrl = "https://localhost:7295";  
            string menuUrl = $"{siteUrl}/Menu/Index/{menuTableId}";

            using (MemoryStream memoryStream = new MemoryStream())
            {
                QRCodeGenerator createQRCode = new QRCodeGenerator();
                QRCodeGenerator.QRCode squareCode = createQRCode.CreateQrCode(menuUrl, QRCodeGenerator.ECCLevel.Q);
                using (Bitmap image = squareCode.GetGraphic(10))
                {
                    image.Save(memoryStream, ImageFormat.Png);
                    ViewBag.QrCodeImage = "data:image/png;base64," + Convert.ToBase64String(memoryStream.ToArray());
                    ViewBag.SelectedTableId = menuTableId;
                    ViewBag.GeneratedUrl = menuUrl;
                }
            }
            return View();
        }
    }
}
