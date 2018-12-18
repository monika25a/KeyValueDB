using System;

namespace KeyValueProject
{
	/// <summary>
	/// Used for PUT operation in In Memory Key value Database.
	/// Can be extended by adding business logic or validations
	/// </summary>
	public class PutOperation
	{
		IStorage store;
		public PutOperation(IStorage store)
		{
			this.store = store;
		}		
		public void Put(string key, string val)
		{
			if(key == null)
				return;
			
			store.Put(key,val);
		}
	}
}
