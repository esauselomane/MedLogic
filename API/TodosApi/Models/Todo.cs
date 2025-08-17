using System;
using System.Text.Json.Serialization;

namespace TodosApi.Models 
{ 
	public class Todo
	{
		public int Id { get; set; }
		public string Description { get; set; } = string.Empty;
		public string Title { get; set; } = string.Empty;
		public DateTime CreatedDate { get; set; }
		public bool IsCompleted { get; set; }
		public int UserId { get; set; }
		[JsonIgnore]
		public virtual User User { get; set; }
	}
}
