/*
 * S0001 - Environment Setup 
 * Description: Creation of the Database and the basic tables (user, profiles, permissions, appliedScript)
 */

 /**************************** DATABASE: Creation  ******************************/
USE master
GO

IF  EXISTS (
	SELECT name 
		FROM sys.databases 
		WHERE name = 'PlataformaZ2'

)
DROP DATABASE PlataformaZ2
GO

CREATE DATABASE PlataformaZ2
GO

/**************************** DATABASE: User Creation  ******************************/
USE [PlataformaZ2]
GO

IF NOT EXISTS (SELECT loginname FROM syslogins WHERE name = 'platUser')
BEGIN
	CREATE LOGIN platUser WITH PASSWORD = 'platPass'
END;
GO

IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'platUser')
BEGIN
    CREATE USER [platUser] FOR LOGIN [platUser]
    EXEC sp_addrolemember N'db_owner', N'platUser'
END;
GO
  
/**************************** DATABASE: Usage  ******************************/  
USE [PlataformaZ2]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/********** TABLE: AppliedScript **********/
CREATE TABLE [dbo].[AppliedScript](
	[ScriptNumber] [int] NOT NULL,
	[ScriptName] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK_AppliedScript] PRIMARY KEY CLUSTERED 
(
	[ScriptNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/********** TABLE: Profile **********/
CREATE TABLE [dbo].[Profile](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](250) NULL,
 CONSTRAINT [PK_Profile] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/********** TABLE: Permission **********/
CREATE TABLE [dbo].[Permission](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](250) NULL,
 CONSTRAINT [PK_Permission] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/********** TABLE: ProfilePermission **********/
CREATE TABLE [dbo].[ProfilePermission](
	[IdProfile] [int] NOT NULL,
	[IdPermission] [int] NOT NULL,
 CONSTRAINT [PK_ProfilePermission] PRIMARY KEY CLUSTERED 
   ([IdProfile],[IdPermission] ASC)
   WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ProfilePermission] WITH CHECK ADD CONSTRAINT [Fk1_ProfilePermission] FOREIGN KEY([IdProfile])
REFERENCES [dbo].[Profile] ([Id])
GO

ALTER TABLE [dbo].[ProfilePermission] WITH CHECK ADD CONSTRAINT [Fk2_ProfilePermission] FOREIGN KEY([IdPermission])
REFERENCES [dbo].[Permission] ([Id])
GO

/********** TABLE: File **********/
CREATE TABLE [dbo].[File](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[RealName] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_File] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/********** TABLE: User **********/
CREATE TABLE [dbo].[User](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](100) NOT NULL,
	[Password] [nvarchar](100) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Nickname] [nvarchar](25) NOT NULL,
	[Cpf] [nvarchar](11) NOT NULL,	
	[IdPhoto] [int] NULL,
	[IdProfile] [int] NOT NULL,	
	[AccessToken] [nvarchar](250) NULL,
	[AccessTokenCreationDate] [datetime] NULL,
	[Active] [bit] NOT NULL, 
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[User] WITH CHECK ADD CONSTRAINT [Fk1_User] FOREIGN KEY([IdPhoto])
REFERENCES [dbo].[File] ([Id])
GO

ALTER TABLE [dbo].[User] WITH CHECK ADD CONSTRAINT [Fk2_User] FOREIGN KEY([IdProfile])
REFERENCES [dbo].[Profile] ([Id])
GO


/********** TABLE: PasswordDefinition **********/
CREATE TABLE [dbo].[PasswordDefinition](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdUser] [int] NOT NULL,
	[Token] [nvarchar](250) NOT NULL,
	[CreationDate] [datetime] NOT NULL,
	[ExpirationDate] [datetime] NULL,
 CONSTRAINT [PK_PasswordDefinition] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[PasswordDefinition] WITH CHECK ADD CONSTRAINT [Fk1_PasswordDefinition] FOREIGN KEY([IdUser])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[PasswordDefinition] CHECK CONSTRAINT [Fk1_PasswordDefinition]
GO

--Register the script as applied
INSERT INTO [dbo].[AppliedScript] ([ScriptNumber], [ScriptName]) VALUES (1, 'Environment Setup')