using CasosSospechososMI.Domain.Common;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace CasosSospechososMI.Services.ApiDefinitions
{
    public interface ISupervisorApiDefinition
    {
        [Multipart]
        [Post("/api/procev/aspersor/registrar_visita")]
        Task<OperationResult<ResponseGeneric>> RegisterVisit([AliasAs("codigo")] string Codigo,
                                                              [AliasAs("res1")] string Res1,
                                                              [AliasAs("res2")] string Res2,
                                                              [AliasAs("res3")] string Res3,
                                                              [AliasAs("res4")] string Res4,
                                                              [AliasAs("preg1")] string Preg1,
                                                              [AliasAs("preg2")] string Preg2,
                                                              [AliasAs("preg3")] string Preg3,
                                                              [AliasAs("preg4")] string Preg4,
                                                              [AliasAs("comentario")] string Comentario,
                                                              [AliasAs("fecha_carga")] string FechaCarga,
                                                              [AliasAs("latitud")] string Latitud,
                                                              [AliasAs("longitud")] string Longitud,
                                                              [AliasAs("imagen")] StreamPart Imagen,
                                                              CancellationToken cancellationToken)
                                                              ;
    }
}
