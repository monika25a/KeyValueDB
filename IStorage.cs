using System;

namespace KeyValueProject
{
	/// <summary>
	///  Interface defined to handle operations on In Memory key value store.
	///  Can be extended for other functions.
	/// </summary>
	public interface IStorage
	{
		void Put(string key, string val);
		void Delete(string key);
		string Get(string key);
	}
}
