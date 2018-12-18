using System;
using System.Collections.Generic;
using System.Threading;

namespace KeyValueProject
{
	/// <summary>
	/// Creates Key value storage type and performs operations on In Memory storage type
	/// Haven't included PUTREF and GETREF as it wasnt clear how to use it so I have concentrated
	/// on main part of this assignment
	/// </summary>
	public class InMemoryStorage : IStorage
	{
		Dictionary<string, string> storage;
		Object lockObject;
		public InMemoryStorage()
		{
			storage = new Dictionary<string, string>();
			lockObject = new Object();
		}		
		public void Put(string key, string val)
		{
			if(storage.ContainsKey(key))
			{
				storage[key]=val;
			}
			else
			{
				storage.Add(key,val);
			}
		}		
		public void Delete(string key)
		{		
			if(storage.ContainsKey(key))
			{
				storage.Remove(key);
			}		
		}		
		public string Get(string key)
		{
			if(storage.ContainsKey(key))
			{
				return storage[key];
			}
			
			return null;
		}		
	}
}
