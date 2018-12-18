using System;

namespace KeyValueProject
{
	/// <summary>
	/// Used for GET operation in In Memory Key value Database.
	/// Can be extended by adding business logic or validations
	/// </summary>
	public class ReadOperation
	{
		IStorage store;
		public ReadOperation(IStorage store)
		{
			this.store = store;
		}
		public string Get(string key)
		{
			if(key == null)
			   return null;
			
			return store.Get(key);			
		}
	}
}
