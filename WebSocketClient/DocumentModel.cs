namespace WebSocketClient
{
	public class TabletDocumentModel
	{
		public int ApplicationId { get; set; }
		public List<DocumentModel> DigitalSignatureDocuments { get; set; }
	}

	public class DocumentModel
	{
		public byte DigitalSignatureDocumentId { get; set; }
		public byte DocumentLanguageId { get; set; }

		public byte? Signature { get; set;}
	}
}
