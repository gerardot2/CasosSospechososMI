using Refit;
using System;
using System.Collections.Generic;
using System.Text;

namespace CasosSospechososMI.Domain.Family
{
    public class FormRecordModel
    {
        public FormRecordModel() { }
        public int FormItemsNumber { get; set; }

        [AliasAs("res1")]
        public string Res1 { get; set; }
        [AliasAs("res2")]
        public string Res2 { get; set; }
        [AliasAs("res3")]
        public string Res3 { get; set; }
        [AliasAs("res4")]
        public string Res4 { get; set; }
        [AliasAs("preg1")]public string Preg1 { get; set; }
        [AliasAs("preg2")]public string Preg2 { get; set; }
        [AliasAs("preg3")]public string Preg3 { get; set; }
        [AliasAs("preg4")]public string Preg4 { get; set; }
        [AliasAs("comentario")]
        public string Comment { get; set; }
        [AliasAs("fecha_carga")]
        public string Date { get; set; }
        [AliasAs("imagen")]
        public string Image { get; set; }
        [AliasAs("latitud")]
        public string Latitude { get; set; }
        [AliasAs("longitud")]
        public string Longitude { get; set; }
        public bool DataLoaded
        {
            get {
                return !string.IsNullOrEmpty(Res1)
                      || !string.IsNullOrEmpty(Res2)
                      || !string.IsNullOrEmpty(Res3)
                      || !string.IsNullOrEmpty(Res4)
                      || !string.IsNullOrEmpty(Comment)
                      || !string.IsNullOrEmpty(Date)
                      || Image != null
                      ;
            }
        }
        public bool FormFulfilled
        {
            get
            {
                return ResponsesCompleted(FormItemsNumber)
                      && !string.IsNullOrEmpty(Date)
                      && Image != null
                      && !string.IsNullOrEmpty(Latitude)
                      && !string.IsNullOrEmpty(Longitude)
                      ;
            }
        }
        public bool FormFulfilledNoLocation
        {
            get
            {
                return ResponsesCompleted(FormItemsNumber)
                      && Image != null
                      ;
            }
        }
        private bool ResponsesCompleted(int cant)
        {
            switch (cant)
            {
                case 1:
                    return !string.IsNullOrEmpty(Res1);
                case 2:
                    return !string.IsNullOrEmpty(Res1)
                        && !string.IsNullOrEmpty(Res2);
                case 3:
                    return !string.IsNullOrEmpty(Res1)
                        && !string.IsNullOrEmpty(Res2)
                        && !string.IsNullOrEmpty(Res3);
                case 4:
                    return !string.IsNullOrEmpty(Res1)
                        && !string.IsNullOrEmpty(Res2)
                        && !string.IsNullOrEmpty(Res3)
                        && !string.IsNullOrEmpty(Res4);
                default:
                    return false;
            }
        }
    }
}
