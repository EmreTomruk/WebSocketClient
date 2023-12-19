using System.ComponentModel.DataAnnotations;

namespace WebSocketClient
{
	public sealed class TabletDocumentModel
	{
		public int ApplicationId { get; set; }
        public int CreatedBy { get; set; }
        public byte MinorApplicantParentTypeId { get; set; }
        public List<DocumentModel> DigitalSignatureDocuments { get; set; }
        public List<MinorApplicantParentApiRequest> MinorApplicantParents { get; set; }
    }

    public sealed class DocumentModel
	{
        [Required]
		public byte DigitalSignatureDocumentId { get; set; }
		public byte DocumentLanguageId { get; set; }

        [Required]
        public string Html { get; set; }
        public List<DocumentSignatureRequestModel> Signatures { get; set; }
    }

    public sealed class MinorApplicantParentApiRequest
    {
        public string Name { get; set; }
    }

    public sealed class DocumentRequestModel 
    {
        public List<DocumentSignatureRequestModel> Signatures { get; set; }
    }

    public sealed class DocumentSignatureRequestModel
    {
        public string Name { get; set; }
        public string Signature { get; set; }
    }

    public sealed class SaveDocumentsRequestModel 
    {
        public int ApplicationId { get; set; }
        public int CreatedBy { get; set; }
        public List<DigitalSignatureDocumentRequestModel> DigitalSignatureDocuments { get; set; }
    }

    public sealed class DigitalSignatureDocumentRequestModel
    {
        public byte DigitalSignatureDocumentId { get; set; }
        public byte DocumentLanguageId { get; set; }
        public string Html { get; set; }

        public List<DocumentSignatureRequestModel> Signatures { get; set; }
    }
}
