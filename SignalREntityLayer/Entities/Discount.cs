using System.ComponentModel.DataAnnotations.Schema;

namespace SignalR.EntityLayer.DAL.Entities

{
    public class Discount
    {
        public int DiscountID { get; set; }

        public string Title { get; set; }

        public string Amount { get; set; }

        public string Description { get; set; }
        
        public string ImageUrl { get; set; }

        public bool Status { get; set; }

        public int? CategoryID { get; set; }

        [ForeignKey("CategoryID")]
        public virtual Category Category { get; set; }
    }
}
