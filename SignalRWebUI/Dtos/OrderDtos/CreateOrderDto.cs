namespace SignalRWebUI.Dtos.OrderDtos
{
    public class CreateOrderDto
    {
        public string TableNumber { get; set; }
        public string Description { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
    }
}
