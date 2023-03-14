using System;
using System.Collections.Generic;
using System.Text;

namespace CasosSospechososMI.Abstractions
{
    public interface IGpsDependencyService
    {
        void OpenSettings();
        bool IsGpsEnable();
    }
}
