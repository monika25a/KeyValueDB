using System;
using System.Collections.Generic;

namespace KeyValueProject
{
	/// <summary>
	/// Transaction class handles transaction by calling InMemoryTransactionStorage class.
	/// Commit operation commits all commands, if any error is encountered it rollsback In Memory Database 
	/// to its previous version before Begin Transaction. Also handles nested transaction.
	/// </summary>
	public class Transaction
	{
		InMemoryTransactionStorage tranStore;
		Int64 transactionId;
		IStorage store;
		List<Transaction> childTransactions;
		public Transaction(InMemoryTransactionStorage tranStore, Int64 transactionId, IStorage store)
		{
			this.tranStore = tranStore;
			this.transactionId = transactionId;
			this.store = store;
			this.childTransactions = new List<Transaction>();
		}		
		public void Put(string key, string val)
		{
			tranStore.Put(transactionId, key, val);
		}		
		public void Delete(string key)
		{
			tranStore.Delete(transactionId, key);
		}		
		public void Rollback()
		{
			foreach (Transaction childTransaction in childTransactions) 
			{
				childTransaction.Rollback();
			}
			tranStore.RemoveId(transactionId);
		}
		public void Commit()
		{
			foreach (Transaction childTransaction in childTransactions) 
			{
				childTransaction.Commit();
			}			
			List<string> commands = tranStore.GetCommands(transactionId);
			Dictionary<string, string> tempStorage = new Dictionary<string, string>();
			
			for(int commandIndex = 0 ; commandIndex < commands.Count ; commandIndex++)
			{
				int index = commands[commandIndex].IndexOf(" ");
				string action = commands[commandIndex].Substring(0,index);
				string keyCommand = commands[commandIndex].Substring(index+1);
				string key = string.Empty;
				if(action.ToUpper() == "PUT")
				{
					int keyIndex = keyCommand.IndexOf(" ");
					key = keyCommand.Substring(0,keyIndex);
				}
				else if(action.ToUpper() == "DELETE")
				{
					key = keyCommand.Substring(0);
				}
								
				if(!tempStorage.ContainsKey(key))
				{
					string getValue = store.Get(key);					
					tempStorage.Add(key,getValue);
				}
			}			
			
			try
			{
				for(int commandIndex = 0 ; commandIndex < commands.Count ; commandIndex++)
				{
					int index = commands[commandIndex].IndexOf(" ");
					string action = commands[commandIndex].Substring(0,index);
					string keyCommand = commands[commandIndex].Substring(index+1);
					string key = string.Empty;				
					if(action.ToUpper() == "PUT")
					{
						int keyIndex = keyCommand.IndexOf(" ");
						key = keyCommand.Substring(0,keyIndex);
						string valCommand = keyCommand.Substring(keyIndex+1);
						string val = valCommand.Substring(0);		
						store.Put(key, val);
					}
					else if(action.ToUpper() == "DELETE")
					{
						key = keyCommand.Substring(0);
						store.Delete(key);
					}	
				}							
			}
			catch(Exception ex)
			{
				foreach(KeyValuePair<string, string> values in tempStorage)
				{
					if(values.Value == null)
					{
						store.Delete(values.Key);
					}
					else
					{
						store.Put(values.Key,values.Value);
					}
				}
			}
			finally
			{
				tranStore.RemoveId(transactionId);
			}	
		}
		public Transaction beginTransaction(){
			Int64 id = tranStore.beginTransaction();
			Transaction newTransaction = new Transaction(tranStore, id, store);
			childTransactions.Add(newTransaction);
			return newTransaction;
		}
	}
}
