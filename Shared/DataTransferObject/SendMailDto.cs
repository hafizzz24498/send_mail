using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObject
{
    public record SendMaiDto
    {
        [Required(ErrorMessage = "Name id is required")]
        public required string SmtpServer { get; init; }
        [Required(ErrorMessage = "Name id is required")]
        public int Port { get; init; }
        [Required(ErrorMessage = "Name id is required")]
        public required string Username { get; init; }
        [Required(ErrorMessage = "Name id is required")]
        public required string Password { get; init; }
        [Required(ErrorMessage = "Name id is required")]
        public required string Message { get; init; }
        [Required(ErrorMessage = "Name id is required")]
        public required string Receiver { get; init; }
        public IFormFile? File { get; init; }
    }
}
