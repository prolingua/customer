The solution has two projects: Customer and UnitTestCustomer

Project Customer has a controller, CustomerController has five actions which should be self-explanatory.
The controller has a constructor with a parameter of type ICustomerDb. When the IIS creates an instance of CustomerController,
an instance of CustomerDb is injected to the constructer as specified in Startup.cs line 31 as follows:
	services.AddTransient<ICustomerDb>(f => new CustomerDb(Configuration));

It's assumed that a database called customer exists on a local running sqlexpress. Please see the connection string in appsettings.json:
	"CustomersConnectionString": "Server=.\\SQLExpress;Database=CustomerDb;Trusted_Connection=True;"

Also the following SQL statements should be run against the database to create the necessary table:

USE [CustomerDb]
GO

/****** Object:  Table [dbo].[Customer]    Script Date: 01/03/2020 20:37:30 ******/
DROP TABLE [dbo].[Customer]
GO

/****** Object:  Table [dbo].[Customer]    Script Date: 01/03/2020 20:37:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Customer](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](100) NOT NULL,
	[LastName] [varchar](100) NOT NULL,
	[Password] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

Project UnitTestCustomer contains the unit tests for the database and the controller. The unit tests for the controller use both the
database and the mock database or in-memory database.

At this stage all the actions in the controller are not secured by [Authorize] attribute. Thus, at this stage anybody can access all
the actions. To restrict access to certain actions, those actions will need [Authorize] attribute. This will need token-based authorization.
This kind of security has to be addressed before deploying the api to live. 

When the volume of the request is high, there is a high possibility of concurrency. For example two persons update the same records.
At the moment, this is dealt with. To deal with this concurrency, lock should be implemented.

Another thing to consider is what to do in case the access to the actual database fails. One thing that can be considered is create a
mechanisme to store the data or data changes into in-memory database and try to save the data or the data changes to the actual database
when the database is available again, or to serialize the data or data changes and save it into a file.

SSL should be used as standard for the live api access.


