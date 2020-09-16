using System;
using System.ComponentModel.DataAnnotations;

namespace GatewayManagement.Models
{
    public class DeviceVM
    {
        public int Id { get; set; }
        [Required]
        public int UID { get; set; }
        public string Vendor { get; set; }
        public DateTime CreatedDate { get; set; }
        public Status Status { get; set; }
        public int GatewayId { get; set; }
        public string Gateway { get; set; }
    }
}