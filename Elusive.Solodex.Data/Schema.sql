
USE master;
SET NOCOUNT ON;

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'Solodex') BEGIN
	declare @tail int
	declare @basefolder nvarchar(max)
	declare @datafile nvarchar(max)
	declare @logfile nvarchar(max)

	set @tail = (
	  select charindex('\',reverse(physical_name))
	  from master.sys.master_files
	  where name = 'master'
	)

	set @basefolder = (select substring(physical_name,1,len(physical_name)-@tail)
	from master.sys.master_files
	where name = 'master')

	set @datafile = @basefolder + '\Solodex.mdf'
	set @logfile = @basefolder + '\Solodex_log.ldf'

	declare @sql nvarchar(MAX)
	set @sql = 'CREATE DATABASE [Solodex] ON PRIMARY ' +
	'( NAME = N''Solodex'', FILENAME = ''' + @datafile + ''', SIZE = 4096KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB ) ' +
	 ' LOG ON ' + 
	'( NAME = N''Solodex_log'', FILENAME = ''' + @logfile + ''', SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)'
	EXEC(@sql)
	
	ALTER DATABASE Solodex SET COMPATIBILITY_LEVEL = 100;
	ALTER DATABASE Solodex SET ANSI_NULL_DEFAULT OFF;
	ALTER DATABASE Solodex SET ANSI_NULLS OFF;
	ALTER DATABASE Solodex SET ANSI_PADDING OFF; 
	ALTER DATABASE Solodex SET ANSI_WARNINGS OFF; 
	ALTER DATABASE Solodex SET ARITHABORT OFF; 
	ALTER DATABASE Solodex SET AUTO_CLOSE OFF; 
	ALTER DATABASE Solodex SET AUTO_CREATE_STATISTICS ON; 
	ALTER DATABASE Solodex SET AUTO_SHRINK OFF; 
	ALTER DATABASE Solodex SET AUTO_UPDATE_STATISTICS ON; 
	ALTER DATABASE Solodex SET CURSOR_CLOSE_ON_COMMIT OFF; 
	ALTER DATABASE Solodex SET CURSOR_DEFAULT  GLOBAL; 
	ALTER DATABASE Solodex SET CONCAT_NULL_YIELDS_NULL OFF; 
	ALTER DATABASE Solodex SET NUMERIC_ROUNDABORT OFF; 
	ALTER DATABASE Solodex SET QUOTED_IDENTIFIER OFF; 
	ALTER DATABASE Solodex SET RECURSIVE_TRIGGERS OFF; 
	ALTER DATABASE Solodex SET AUTO_UPDATE_STATISTICS_ASYNC OFF; 
	ALTER DATABASE Solodex SET DATE_CORRELATION_OPTIMIZATION OFF; 
	ALTER DATABASE Solodex SET TRUSTWORTHY OFF; 
	ALTER DATABASE Solodex SET ALLOW_SNAPSHOT_ISOLATION OFF; 
	ALTER DATABASE Solodex SET PARAMETERIZATION SIMPLE; 
	ALTER DATABASE Solodex SET READ_WRITE; 
	ALTER DATABASE Solodex SET RECOVERY SIMPLE; 
	ALTER DATABASE Solodex SET MULTI_USER; 
	ALTER DATABASE Solodex SET PAGE_VERIFY CHECKSUM;  
	ALTER DATABASE Solodex SET DB_CHAINING OFF; 
		
     if(@@error <> 0) begin
       RETURN
     end
END
GO

-- Start the main transaction
BEGIN TRANSACTION initialCreate;

	USE Solodex;

	IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[SchemaVersion]')) 
	BEGIN
		CREATE TABLE [dbo].[SchemaVersion](
			[Id] [uniqueidentifier] NOT NULL default newid(),
			[MajorVersion] [int] NOT NULL,
			[MinorVersion] [int] NOT NULL,
			[InstallDate] [datetimeoffset](7) NOT NULL
		 CONSTRAINT [PK_SchemaVersion] PRIMARY KEY CLUSTERED 
		(
			[Id] ASC
		)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
		) ON [PRIMARY]
	END
	
	if(@@error <> 0)
	begin
		ROLLBACK TRANSACTION;
		RETURN;
	end

	declare @majorVersion int;
	declare @minorVersion int;

	set @majorVersion = 1;
	set @minorVersion = 0;
	if(not(exists(select (1) from SchemaVersion where (MajorVersion = @majorVersion) and (MinorVersion = @minorVersion))))
	BEGIN
	
		CREATE TABLE [dbo].[Contacts](
			[Id] [uniqueidentifier] NOT NULL default newid(),
			[Prefix] [nvarchar](10) NULL,
			[First] [nvarchar](30) NULL,
			[Middle] [nvarchar](30) NULL,
			[Last] [nvarchar](30) NULL,
			[Suffix] [nvarchar](10) NULL,
			[MobilePhone] [nvarchar](10) NULL,
			[WorkPhone] [nvarchar](10) NULL,
			[Gender] [char](1) NULL,
			[Dob] [datetime] NULL,
			[Email] [nvarchar](100) NULL,
			[Notes] [nvarchar](250) NULL,
			[rowversion] [timestamp] NOT NULL,
		 CONSTRAINT [PK_Contacts] PRIMARY KEY CLUSTERED 
		(
			[Id] ASC
		)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
		) ON [PRIMARY]

	
		
		CREATE TABLE [dbo].[Addresses](
			[Id] [uniqueidentifier] NOT NULL default newid(),
			[Contact_Id] [uniqueidentifier] NOT NULL,
			[Name] [nvarchar](50) NOT NULL,
			[rowversion] [timestamp] NOT NULL,
		 CONSTRAINT [PK_Addresses] PRIMARY KEY CLUSTERED 
		(
			[Id] ASC
		)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
		) ON [PRIMARY]

		CREATE NONCLUSTERED INDEX [IX_FK_Contacts_Addresses] ON [dbo].[Addresses] 
		(
			[Contact_Id] ASC
		)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

		
		CREATE TABLE [dbo].[ApplicationSettings](
				[Id] [uniqueidentifier] NOT NULL default newid(),
				[Key] [nvarchar](200) NOT NULL,
				[Value] [nvarchar](max) NOT NULL,
				[rowversion] [rowversion] NOT NULL,
			 CONSTRAINT [PK_ApplicationSettings] PRIMARY KEY CLUSTERED 
			(
				[Id] ASC
			)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
			) ON [PRIMARY]
		
		INSERT INTO SchemaVersion values (newid(), @majorVersion, @minorVersion, getutcdate());
	END
	
	if(@@error <> 0)
	begin
		ROLLBACK TRANSACTION;
		RETURN;
	end

COMMIT TRANSACTION initialCreate;


/*
 *	Following is example of how to handle a db migration and inc the schema version
 *
 */
if(not(exists(select (1) from SchemaVersion where (MajorVersion = 1) and (MinorVersion = 1))))
BEGIN
	BEGIN TRANSACTION Version1_1

		-- update cols in address table
		ALTER TABLE [dbo].[Addresses] ADD [Street] [nvarchar](50) NOT NULL
		ALTER TABLE [dbo].[Addresses] ADD [City] [nvarchar](30) NOT NULL
		ALTER TABLE [dbo].[Addresses] ADD [State] [nvarchar](2) NOT NULL
		ALTER TABLE [dbo].[Addresses] ADD [Zip] [nvarchar](10) NOT NULL

		INSERT INTO SchemaVersion values (newid(), 1, 1, getutcdate());
	COMMIT TRANSACTION Version1_1
END



