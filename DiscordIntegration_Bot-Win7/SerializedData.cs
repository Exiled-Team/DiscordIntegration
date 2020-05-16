using System;

namespace SerializedData
{
	[Serializable]
	public class SerializedData
	{
		public string Data;
		public int Port;
		public ulong Channel;
		public string Name;
	}
}