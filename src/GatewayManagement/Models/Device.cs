using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GatewayManagement.Models
{
    public enum Status
    {
        Offline,
        Online
    }

    public class Device
    {
        public int Id { get; set; }
        [Required]
        public int UID { get; set; }
        public string Vendor { get; set; }
        public DateTime CreatedDate { get; set; }
        public Status Status { get; set; }
        public int GatewayId { get; set; }

        [JsonIgnore]
        public virtual Gateway Gateway { get; set; }
    }
}