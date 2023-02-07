using Model.DTO.Docusign;
using DocuSignBL.Peticion;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DocuSignBL.Opetations
{
    public class DocuSignBL
    {
        private IHttpContextAccessor _httpContextAccessor { get; }


        public DocuSignBL(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<EnvelopeSignerDTO> GetRecipentsEnvelope(string envelopes)
        {
            var response = await new PeticionDocusign(_httpContextAccessor).peticion<EnvelopeSignerDTO>($"envelopes?envelope_ids={envelopes}&include=recipients", System.Net.Http.HttpMethod.Get);

            return response;
        }

    }
}
