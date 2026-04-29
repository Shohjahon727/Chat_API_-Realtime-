using System.Collections.Concurrent;

namespace Chat_API.Realtime
{
	public class ConnectionManager
	{
		public static ConcurrentDictionary<int, string> Users = new();
	}
}
