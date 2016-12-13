
This project is based on the awesome ASP.NET Identity 2.0 Extending Users and Roles Example
====================================================================================

The post which accompanies this example can be found at [ASP.NET Identity 2.0: Customizing Users and Roles]
	(http://typecastexception.com/post/2014/06/22/ASPNET-Identity-20-Customizing-Users-and-Roles.aspx)

I started with: https://github.com/TypecastException/AspNet-Identity-2-Extending-Users-And-Roles.git I never did get the Nuget package to run.

Once I installed it I did a rename across the board to GolfDB2 for the model workspace, project files and solution.
I also had to update all of the Nuget packages before it would run.

You should also provide a new COM guid in "AssemblyInfo.cs".

In Web.config you will need to point "DefaultConnection" to a new SQL Server DB unless you are using VS 2012.

<connectionStrings>
	<add name="DefaultConnection" connectionString="data source=DESKTOP-S7JQFF1\SQLEXPRESS;initial catalog=GolfDB20161207-01;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework" providerName="System.Data.SqlClient" />

I changed "DefaultConnection" to point to a new DB. On startup it should initialize the new DB automatically if your DB is set up correctly.
	
I am using SQLEXPRESS (2016) and it is working fine for me.  I believe it needs to be SQL Server 2012 or newer for the sample to work.

I am using VS 2015 community to build this project.


