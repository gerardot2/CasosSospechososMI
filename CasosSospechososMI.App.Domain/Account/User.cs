using CasosSospechososMI.Domain.Account.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CasosSospechososMI.Domain.Account
{
    public class User
    {
        public string Password { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public UserTypeEnum UserType { get; set; }
        public int? DocumentNumber { get; set; }
        public int? Role { get; set; }
        public int? UserId { get; set; }
        public bool Supervisor { get; set; }
        public bool HasCachedForm { get; set; }
        public bool HasCachedRecord { get; set; }
        public string FullName
        {
            get
            {
                if (!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(SurName))
                {
                    return $"{Name} {SurName}";
                }
                else if (!string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(SurName))
                {
                    return Name;
                }
                else if (string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(SurName))
                {
                    return SurName;
                }
                else
                {
                    return string.Empty;
                }

            }
        }
        public string MenuName
        {
            get
            {
                return "Hola " + Name;
            }
        }

    }
}
