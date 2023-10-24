namespace WebSocketClient
{
	public class ApiResponse<T>
	{
		public bool IsSuccess { get; set; }
		public string Code { get; set; }
		public string Message { get; set; }
		public T Data { get; set; }
	}

	public class ApplicationEntryFormApiResponse
	{
		public int Id { get; set; }
		public string PassportNumber { get; set; }
		public string Name { get; set; }
		public string Surname { get; set; }
		public DateTime BirthDate { get; set; }
		public int NationalityId { get; set; }
		public string Nationality { get; set; }
		public DateTime ArrivalDate { get; set; }
		public int BranchId { get; set; }

        public string Html { get; set; }
        //public byte[] Html { get; set; }
    }
}
