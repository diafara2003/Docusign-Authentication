using Docusign.Repository.Peticion;
using Docusign.Services;
using Model.DTO.Docusign;
using Model.DTO;
using Model.DTO.Users;
using System.IO.Compression;

namespace API.Routes.MapDocusign
{
    public static class MapDocusignRoutes
    {
        public static void RegisterDocusign(this IEndpointRouteBuilder app)
        {
            app.MapGet("/Docusign/userInfo", async (IDocusignService _docusignService) =>
            {
                try
                {
                    userDTO _user = await _docusignService.peticion<userDTO>("users", MethodRequest.GET);
                    return Results.Ok(_user);
                }
                catch (Exception e)
                {
                    return Results.Ok(e.Message);
                }
            }).WithTags("Docusign");

            app.MapGet("/Docusign/Templates", async (IDocusignService _docusignService) =>
            {
                try
                {
                    var _result = await _docusignService.GetTemplates(); ;
                    return Results.Ok(_result);
                }
                catch (Exception e)
                {
                    return Results.Ok(e.Message);
                }
            }).WithTags("Docusign");

            app.MapGet("/Docusign/TemplatesSigners", async (IDocusignService _docusignService) =>
            {
                try
                {
                    var _result = await _docusignService.GetTemplatesSigners();
                    return Results.Ok(_result);
                }
                catch (Exception e)
                {
                    return Results.Ok(e.Message);
                }
            }).WithTags("Docusign");

            app.MapGet("/Docusign/envelopes/recipents", async (IDocusignService _docusignService, string envelope) =>
            {
                try
                {
                    var _result = await _docusignService.GetRecipentsEnvelope(envelope);
                    return Results.Ok(_result);
                }
                catch (Exception e)
                {
                    return Results.Ok(e.Message);
                }
            }).WithTags("Docusign");

            app.MapGet("/Docusign/SignersByTemplete", async (IDocusignService _docusignService, string idTemplate, string contrato = "") =>
            {
                try
                {
                    var _result = await _docusignService.GetSignersByTemplete(idTemplate, contrato);
                    return Results.Ok(_result);
                }
                catch (Exception e)
                {
                    return Results.Ok(e.Message);
                }
            }).WithTags("Docusign");

            app.MapPost("/Docusign/envelopes/send", async (IDocusignService _docusignService, EnvelopeSendDTO envelope) =>
            {
                try
                {
                    var _result = await _docusignService.SendEnvelope(envelope);
                    return Results.Ok(_result);
                }
                catch (Exception e)
                {
                    return Results.Ok(e.Message);
                }
            }).WithTags("Docusign");

            app.MapGet("/Docusign/envelopes/status", async (IDocusignService _docusignService, string idenvelope) =>
            {
                var response = await _docusignService.peticion<DocusignAuditoriaDTO>($"envelopes/{idenvelope}?include=recipients,documents", MethodRequest.GET);


                return Results.Ok(new Tuple<AuthenticationDTO, DocusignAuditoriaDTO>(new AuthenticationDTO()
                {
                    isAuthenticated = true,
                }, response));
            }).WithTags("Docusign");

            app.MapGet("/Docusign/envelopes/history", async (IDocusignService _docusignService, string idenvelope) =>
            {
                try
                {
                    return Results.Ok(await _docusignService.GetEnvelopeHistory(idenvelope));
                }
                catch (Exception e)
                {
                    return Results.Ok(new Tuple<AuthenticationDTO, ResponseDocusignAuditoriaDTO>(new AuthenticationDTO() { isAuthenticated = true }, new ResponseDocusignAuditoriaDTO()));
                }
            }).WithTags("Docusign");

            app.MapGet("/Docusign/envelopes/documents", async (IDocusignService _docusignService, string idenvelope) =>
            {
                try
                {
                    return Results.Ok(await _docusignService.GetEnvelopeDocuments(idenvelope));
                }
                catch (Exception e)
                {
                    return Results.Ok(new Tuple<AuthenticationDTO, ResponseEnvelopeDTO>(new AuthenticationDTO() { isAuthenticated = true }, new ResponseEnvelopeDTO()));
                }
            }).WithTags("Docusign");

            app.MapGet("/Docusign/envelopes/documents/zip", async (IDocusignService _docusignService, string idenvelope) =>
            {
                Tuple<AuthenticationDTO, ResponseEnvelopeDTO> respuesta = await _docusignService.GetEnvelopeDocuments(idenvelope);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (ZipArchive zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, false))
                    {
                        foreach (var item in respuesta.Item2.envelopeDocuments)
                        {
                            ZipArchiveEntry fileInArchive = zipArchive.CreateEntry(item.name + ".pdf", CompressionLevel.Optimal);

                            using (var entryStream = fileInArchive.Open())
                            {
                                using (var ms = new MemoryStream(item.file))
                                {
                                    await ms.CopyToAsync(entryStream);
                                }
                            }
                        }
                    }

                    return Results.File(memoryStream.ToArray(), "application/x-zip-compressed", "test-txt-files.zip");
                }
            }).WithTags("Docusign");

            app.MapGet("/Docusign/state/token",  (IDocusignService _docusignService, IWebHostEnvironment _webHostEnvironment) =>
            {
                try
                {
                    return Results.Ok(_docusignService.StateToken(_webHostEnvironment.ContentRootPath));
                }
                catch (Exception e)
                {
                    return Results.Ok(new ResposeStateTokenDTO());
                }
            }).WithTags("Docusign");

            app.MapGet("Docusign/validarsesion",  (IDocusignService _docusignService) =>
            {
                return Results.Ok(new Tuple<AuthenticationDTO, string>(_docusignService.validationAuthentication(), string.Empty));
            }).WithTags("Docusign");
            app.MapGet("Docusign", (IDocusignService _docusignService) =>
            {
                return Results.Ok("hola");
            }).WithTags("Docusign");

        }
    }
}
