using System;
using System.Collections.Generic;
using System.Threading;

namespace KeyValueProject
{
	/// <summary>
	/// Handles transaction commands and stores them in Dictionary with transaction id as key and 
	/// value as list of all commands.
	/// </summary>
	public class InMemoryTransactionStorage
	{
		Dictionary<Int64, List<string>> commandStorage;
		long nextTransactionId;
		Object lockObject;
		public InMemoryTransactionStorage()
		{
			commandStorage = new Dictionary<long, List<string>>();
			nextTransactionId = 1;
			lockObject = new Object();
		}		
		public void Put(long transactionId, string key, string val)
		{
			lock(lockObject)
			{
				string command = "PUT"+ " " + key + " " + val;
				commandStorage[transactionId].Add(command);
			}
		}			
		public void Delete(long transactionId, string key)
		{
			lock(lockObject)
			{
				string command = "DELETE" + " " + key;
				commandStorage[transactionId].Add(command);
			}
		}		
		public Int64 beginTransaction(){
			lock(lockObject)
			{
				List<string> commands = new List<string>();
				Int64 id = nextTransactionId;
				commandStorage.Add(id, commands);
				nextTransactionId += 1;
				return id;
			}
		}		
		public void RemoveId(long transactionId)
		{
			if(commandStorage.ContainsKey(transactionId))
				commandStorage.Remove(transactionId);
		}		
		public List<string> GetCommands(long transactionId)
		{
			if (commandStorage.ContainsKey(transactionId)) {
				return commandStorage[transactionId];
			}
			return new List<string>();
		}
	}
}
