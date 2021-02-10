
1.Open Blank Solution which is name "SocialMediaApp".
2.Open Class Library (.Core) Project which is name "SocialMediaApp.DomainLayer".
	2.1. Open "Enums" Folder. In this Folder, Open "Status.cs" class.	
		 public enum Status { Active=1, Modified=2, Passive=3 }
	2.2. Open "Entities" folder. Create entities in Interface and Concrete folder.
		 P.S : I use "Asp .Net Core Identity" class for users operations.In this context, AppUserRole and AppUser class inhereted from Identity Class. For this, install Microsoft.AspCore.Identity package.  
	2.3. Open "Repository" folder. In this folder, I create methods for CRUD operation properly asynchronous programming .
		 For more asynchronous programming => https://ridvanorun.gitbook.io/asp-net-core/
		 For more Repository Design Pattern => https://ridvanorun.gitbook.io/desing-patterns/
	2.4. Open "UnitOfWork" folder.
		 2.4.1. 
		 2.4.2. 
	     