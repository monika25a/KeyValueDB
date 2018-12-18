using System;

namespace KeyValueProject
{
	/// <summary>
	/// Is used for calling internal GET, PUT, DELETE, Begin Transaction functions
	/// </summary>
	public class InMemoryKeyValueStore
	{
		ReadOperation readOperation;
		PutOperation putOperation;
		DeleteOperation deleteOperation;
		IStorage store;
		InMemoryTransactionStorage transactionStorage;
		
		private InMemoryKeyValueStore(ReadOperation readOperation, 
		                              PutOperation putOperation, DeleteOperation deleteOperation, IStorage store,
		                             InMemoryTransactionStorage transactionStorage)
		{
			this.readOperation = readOperation;
			this.putOperation = putOperation;
			this.deleteOperation = deleteOperation;
			this.store = store;
			this.transactionStorage = transactionStorage;
		}		
		public static InMemoryKeyValueStore createInstance(IStorage store)
		{
			return new InMemoryKeyValueStore(new ReadOperation(store), new PutOperation(store),
			                                 new DeleteOperation(store), store, new InMemoryTransactionStorage());
		}		
		public string Get(string key)
		{
			// Check permissions
			// Check throttling
			return readOperation.Get(key);
		}		
		public void Put(string key, string val)
		{
			// Check permissions
			// Check throttling
			putOperation.Put(key, val);
		}		
		public void Delete(string key)
		{
			// Check permissions
			// Check throttling
			deleteOperation.Delete(key);
		}		
		public Transaction BeginTransaction()
		{
			Int64 id = transactionStorage.beginTransaction();
			return new Transaction(transactionStorage, id, store);
		}
	}
}
