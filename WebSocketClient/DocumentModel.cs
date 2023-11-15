namespace WebSocketClient
{
	public class TabletDocumentModel
	{
		public int ApplicationId { get; set; }
        public int CreatedBy { get; set; }
        public List<DocumentModel> DigitalSignatureDocuments { get; set; }
	}

	public class DocumentModel
	{
		public byte DigitalSignatureDocumentId { get; set; }
		public byte DocumentLanguageId { get; set; }
        public string Html { get; set; }
        public string Signature { get; set; }
    }
}
