using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SignalR.BusinessLayer.Abstract;
using SignalR.DtoLayer.OrderDto;
using SignalR.EntityLayer.Entities;

namespace SignalRApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrderController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult OrderList()
        {
            var values = _orderService.TGetListAll();
            return Ok(_mapper.Map<List<ResultOrderDto>>(values));
        }

        [HttpPost]
        public IActionResult CreateOrder(CreateOrderDto createOrderDto)
        {
            var order = _mapper.Map<Order>(createOrderDto);
            order.OrderDate = DateTime.Now;
            _orderService.TAdd(order);
            return Ok("Sipariş oluşturuldu");
        }

        [HttpGet("GetByTableNumber/{tableNumber}")]
        public IActionResult GetByTableNumber(string tableNumber)
        {
            var orders = _orderService.TGetListAll()
                .Where(x => x.TableNumber == tableNumber)
                .ToList();
            return Ok(_mapper.Map<List<ResultOrderDto>>(orders));
        }

        [HttpDelete("DeleteByTableNumber/{tableNumber}")]
        public IActionResult DeleteByTableNumber(string tableNumber)
        {
            var orders = _orderService.TGetListAll()
                .Where(x => x.TableNumber == tableNumber)
                .ToList();
            
            foreach (var order in orders)
            {
                _orderService.TDelete(order);
            }
            
            return Ok("Sipariş geçmişi silindi");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(int id)
        {
            var value = _orderService.TGetById(id);
            _orderService.TDelete(value);
            return Ok("Sipariş silindi");
        }

        // YENİ: Sipariş durumunu "Onaylandı" yap
        [HttpGet("ChangeStatusToApproved/{id}")]
        public IActionResult ChangeStatusToApproved(int id)
        {
            var order = _orderService.TGetById(id);
            order.Status = "Onaylandı";
            _orderService.TUpdate(order);
            return Ok("Sipariş onaylandı");
        }

        // YENİ: Sipariş durumunu "Teslim Edildi" yap
        [HttpGet("ChangeStatusToDelivered/{id}")]
        public IActionResult ChangeStatusToDelivered(int id)
        {
            var order = _orderService.TGetById(id);
            order.Status = "Teslim Edildi";
            _orderService.TUpdate(order);
            return Ok("Sipariş teslim edildi");
        }
    }
}
