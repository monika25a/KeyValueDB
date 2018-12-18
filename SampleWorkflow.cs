using System;

namespace KeyValueProject
{
	class SampleWorkflow
	{
		public static void Main(string[] args)
		{
			InMemoryKeyValueStore dbStore = InMemoryKeyValueStore.createInstance(new InMemoryStorage());
			
			//Test case to check for transaction commit
			Transaction transaction = dbStore.BeginTransaction();
			transaction.Put("A", "1");
			transaction.Put("B", "3");
			transaction.Delete("A");
			transaction.Commit();			
			Console.WriteLine(dbStore.Get("B")=="3");
			Console.WriteLine(dbStore.Get("A")==null);
			
			//Test case to check for transaction Rollback
			Transaction newtransaction = dbStore.BeginTransaction();
			newtransaction.Put("A", "2");
			newtransaction.Put("B", "5");
			newtransaction.Rollback();			
			Console.WriteLine(dbStore.Get("B")=="3");
			Console.WriteLine(dbStore.Get("A")==null);
			
			//Test case to check for nested transactions commit
			Transaction tran = dbStore.BeginTransaction();
			tran.Put("A", "3");
			tran.Put("B", "6");			
			Transaction childTran =	tran.beginTransaction();			
			childTran.Put("C", "1");
			tran.Commit();			
			Console.WriteLine(dbStore.Get("A")=="3");
			Console.WriteLine(dbStore.Get("B")=="6");
			Console.WriteLine(dbStore.Get("C")=="1");
			
			//Test case to check for nested transactions rollback
			Transaction tranRoll = dbStore.BeginTransaction();
			tranRoll.Put("A", "4");
			tranRoll.Put("B", "7");			
			Transaction childTranRoll =	tranRoll.beginTransaction();			
			childTranRoll.Put("C", "8");
			tranRoll.Rollback();			
			Console.WriteLine(dbStore.Get("A")=="3");
			Console.WriteLine(dbStore.Get("B")=="6");
			Console.WriteLine(dbStore.Get("C")=="1");
			
			//Test case to check for inactive transaction
			try
			{
				tran.Commit();
			}
			catch(Exception ex)
			{
				Console.WriteLine("Error occured while commiting transaction" + ex.Message);
			}
			
			//Test case to check for child rollback and parent commit
			Transaction tranParentChild = dbStore.BeginTransaction();
			tranParentChild.Put("A", "4");
			tranParentChild.Put("B", "7");			
			Transaction tranChild =	tranParentChild.beginTransaction();			
			tranChild.Put("C", "8");
			tranChild.Rollback();
			tranParentChild.Commit();	
			
			Console.WriteLine(dbStore.Get("A")=="4");
			Console.WriteLine(dbStore.Get("B")=="7");
			Console.WriteLine(dbStore.Get("C")=="1");
			
			//Test case to check for child commit and parent commit
			Transaction ParentChild = dbStore.BeginTransaction();
			ParentChild.Put("A", "8");
			ParentChild.Put("B", "7");			
			Transaction child =	ParentChild.beginTransaction();			
			child.Put("C", "3");
			child.Delete("A");
			child.Commit();
			ParentChild.Commit();	
			
			Console.WriteLine(dbStore.Get("A")=="8");
			Console.WriteLine(dbStore.Get("B")=="7");
			Console.WriteLine(dbStore.Get("C")=="3");
			
						
			Console.Read();		
			
		}
	}
}