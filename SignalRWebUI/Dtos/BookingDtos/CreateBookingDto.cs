using System.ComponentModel.DataAnnotations;

namespace SignalRWebUI.Dtos.BookingDtos
{
    public class CreateBookingDto
    {
        [Required(ErrorMessage = "Ad Soyad alanı zorunludur.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Telefon numarası alanı zorunludur.")]
        public string Phone { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Mail adresi alanı zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir mail adresi giriniz.")]
        public string Mail { get; set; }

        [Required(ErrorMessage = "Kişi sayısı seçiniz.")]
        [Range(1, 100, ErrorMessage = "Geçerli bir kişi sayısı seçiniz.")]
        public int PersonCount { get; set; }

        [Required(ErrorMessage = "Tarih alanı zorunludur.")]
        public DateTime? Date { get; set; }
    }
}
