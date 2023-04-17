using CasosSospechososMI.Domain.Account;
using CasosSospechososMI.Domain.Common;
using CasosSospechososMI.Domain.Family;
using CasosSospechososMI.Domain.Samples;
using CasosSospechososMI.Domain.User;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CasosSospechososMI.Services.ApiDefinitions
{
    public interface IFamilyApiDefinition
    {
        [Get("/api/procev/aspersor/home")]
        Task<HomeDataModel> GetFamilyHomeData(CancellationToken ct);
        [Get("/api/procev/casos/homeAgente")]
        Task<HomeDataSupervisorModel> GetSupervisorHomeData(CancellationToken ct);
        [Get("/api/procev/usuarios/{tipo}/getUserId")]
        Task<OperationResult<FamilyDataModel>> GetFamilyData(CancellationToken ct, int tipo);
        [Get("/api/procev/usuarios/{codigo}/getUserCodigoAsp")]
        Task<OperationResult<FamilyDataModel>> GetFamilyDataByCode(CancellationToken ct, string codigo);

        [Get("/api/procev/casos/{tipo}/preguntas")]
        Task<OperationResult<List<FormModel>>> GetFormItems(CancellationToken ct, int tipo);

        [Multipart]
        [Post("/api/procev/casos/registrar_caso")]
        Task<OperationResult<ResponseGeneric>> RegisterSample([AliasAs("res1")] string Res1,
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
                                                              [AliasAs("resultado")] string resultado,
                                                              [AliasAs("dni")] string dni,
                                                              [AliasAs("apellido")] string apellido,
                                                              [AliasAs("nombre")] string nombre,
                                                              [AliasAs("cantidad")] string cantidad,
                                                              [AliasAs("barrio")] string barrio,
                                                              [AliasAs("domicilio")] string domicilio,
                                                              [AliasAs("id_localidad")] string id_localidad,
                                                              [AliasAs("telefono")] string telefono,
                                                              [AliasAs("email")] string email,
                                                              [AliasAs("imagen")] StreamPart Imagen,
                                                              CancellationToken cancellationToken)
                                                              ;
        
        [Get("/api/procev/casos/getCasosSospechosos")]
        Task<OperationResult<List<SampleModel>>> GetMySamples(CancellationToken cancellationToken);
        [Get("/api/procev/casos/getCasosSospechososCargados")]
        Task<OperationResult<List<SampleModel>>> GetMyLoadedSamples(CancellationToken cancellationToken);
        [Get("/api/procev/aspersor/{id}/muestrasCodigoAspersor")]
        Task<OperationResult<List<SampleModel>>> GetVisitsById(CancellationToken cancellationToken,string id);
        [Post("/api/procev/usuarios/actualizarDatosUsuario")]
        Task<ResponseGeneric> PostSaveDataUpdate([AliasAs("dni")] string Dni,
                                                  [AliasAs("nombre")] string Nombre,
                                                  [AliasAs("apellido")] string Apellido,
                                                  [AliasAs("id_localidad")] string IdLocalidad,
                                                  [AliasAs("domicilio")] string Domicilio,
                                                  [AliasAs("telefono")] string Telefono,
                                                  [AliasAs("cantidad")] string Cantidad,
                                                  [AliasAs("email")] string Email,
                                                  [AliasAs("id_role")] string IdRole,
                                                  [AliasAs("codigo")] string Codigo,
                                                CancellationToken cancellationToken);
    }
    
}
