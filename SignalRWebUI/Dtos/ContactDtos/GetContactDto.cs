﻿namespace SignalRWebUI.Dtos.ContactDtos
{
    public class GetContactDto
    {
        public int ContactID { get; set; }
        public string Locaiton { get; set; }
        public string Phone { get; set; }
        public string Mail { get; set; }
        public string FooterDescription { get; set; }
    }
}
