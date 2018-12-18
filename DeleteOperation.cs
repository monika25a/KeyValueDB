using System;

namespace KeyValueProject
{
	/// <summary>
	/// Used for delete operation in In Memory Key value Database.
	/// Can be extended by adding business logic  or validations
	/// </summary>
	public class DeleteOperation
	{
		IStorage store;
		public DeleteOperation(IStorage store)
		{
			this.store = store;
		}		
		public void Delete(string key)
		{
			if(key == null)
				return;
			
			store.Delete(key);
		}
	}
}
