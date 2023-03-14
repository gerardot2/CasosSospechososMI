using System;
using System.Collections.Generic;
using System.Text;

namespace CasosSospechososMI.Models
{
    public class User
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public int UserId { get; set; }
        public string FullName { get { return $"{Name} {SurName}"; } }
        public bool Supervisor { get; set; }
        public bool HasCachedForm { get; set; }
        public bool HasCachedSample { get; set; }
    }
}
