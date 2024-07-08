using Microsoft.AspNetCore.Identity;

namespace TaskManager.Models
{
	public class User : IdentityUser<Guid>
	{

		public string Name { get; set; }
		public List<TodoTask> Tasks { get; set; }
	}
}
