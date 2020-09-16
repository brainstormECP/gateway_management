using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace GatewayManagement.Models
{
    public class Gateway
    {
        private const string ipAdressRegEx = @"\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b";
        public int Id { get; set; }

        [Required]
        public string SerialNumber { get; set; }

        [Required]
        public string Name { get; set; }

        [RegularExpression(ipAdressRegEx, ErrorMessage = "IPv4 Invalid Format")]
        [Required]
        public string IPv4 { get; set; }
        public virtual ICollection<Device> Devices { get; set; }

        public Gateway()
        {
            Devices = new HashSet<Device>();
        }
    }
}