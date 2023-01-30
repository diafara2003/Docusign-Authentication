using Model.DTO.Docusign;
using DocuSignBL.Peticion;
using System.Threading.Tasks;

namespace DocuSignBL.Opetations
{
    public class DocuSignBL
    {

        public async Task<EnvelopeSignerDTO> GetRecipentsEnvelope(string envelopes)
        {
            var response = await new PeticionDocusign().peticion<EnvelopeSignerDTO>($"envelopes?envelope_ids={envelopes}&include=recipients", System.Net.Http.HttpMethod.Get);

            return response;
        }

    }
}
