namespace WebSocketClient
{
	public class TabletDocumentModel
	{
		public string EncryptedApplicationId { get; set; }
		public List<DocumentModel> DigitalSignatureDocuments { get; set; }
	}

	public class DocumentModel
	{
		public byte DigitalSignatureDocumentId { get; set; }
		public byte DocumentLanguageId { get; set; }
	}
}
