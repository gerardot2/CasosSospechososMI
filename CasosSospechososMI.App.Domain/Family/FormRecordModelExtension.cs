using Refit;
using System;
using System.Collections.Generic;
using System.Text;

namespace CasosSospechososMI.Domain.Family
{
    public class FormRecordModelExtension : FormRecordModel
    {
        public FormRecordModelExtension() { }
        public StreamPart Imagen { get; set; }
        public string Codigo { get; set; }

    }
}
