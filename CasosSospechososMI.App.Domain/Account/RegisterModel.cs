using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CasosSospechososMI.Domain.Account
{
    public class RegisterModel
    {

        [AliasAs("nombre")]
        public string Name  {get; set;}
        [AliasAs("apellido")]
        public string  Surname {get; set;}
        [AliasAs("dni")]
        public string  Dni {get; set;}
        [AliasAs("email")]
        public string Email { get; set; }
        [AliasAs("id_localidad")]
        public int? CityId {get; set;}
        [AliasAs("domicilio")]
        public string  Address {get; set;}
        [AliasAs("cantidad")]
        public int? MembersQty {get; set;}
        [AliasAs("telefono")]
        public string  Phone {get; set;}
        [AliasAs("id_role")]
        public int? RoleId {get; set;}
        [AliasAs("codigo")]
        public string  Code {get; set;}
        [AliasAs("pass")]
        public string Password { get; set; }
        public bool IsFulFilled
        {
            get
            {
                return !string.IsNullOrEmpty(Name)
                    && !string.IsNullOrEmpty(Surname)
                    && CheckDni
                    && CityId.HasValue
                    && !string.IsNullOrEmpty(Address)
                    && MembersQty.HasValue
                    && !string.IsNullOrEmpty(Phone)
                    && RoleId.HasValue
                    && !string.IsNullOrEmpty(Code);
            }
        }
        public bool IsSupervisorFulFilled
        {
            get
            {
                return !string.IsNullOrEmpty(Name)
                    && !string.IsNullOrEmpty(Surname)
                    && CheckDni
                    && CityId.HasValue
                    && !string.IsNullOrEmpty(Phone)
                    && !string.IsNullOrEmpty(Address)
                    && RoleId.HasValue
                    && CheckPassword
                    ;
            }
        }

        public bool CheckPassword { get {
                return !string.IsNullOrEmpty(Password)
                    && Password.Length > 5 && Password.Length < 11;
            } }
        private bool CheckDni
        {
            get
            {
                return !string.IsNullOrEmpty(Dni) && Dni.Length == 8;
            }
        }
    }
}
