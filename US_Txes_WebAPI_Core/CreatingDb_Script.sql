USE master
GO

IF DB_ID (N'USDb') IS NULL
BEGIN
CREATE database [USDb];
END
GO

USE [USDb]
GO

drop table if exists [VehicleFees]
GO
drop table if exists [Fees]
GO
drop table if exists [ZipCodes]
GO
drop table if exists [States]
GO
CREATE TABLE [dbo].[States](
	[StateID] [int] IDENTITY(1,1) NOT NULL,
	[Abbreviation] [nvarchar](2) NOT NULL,
	[Name] [nvarchar](100) NOT NULL
) ON [PRIMARY]
GO

ALTER TABLE [States]
   ADD CONSTRAINT PK_States_StateID PRIMARY KEY CLUSTERED (StateID);
   
CREATE TABLE [dbo].[ZipCodes](
	[ZipCodeID] [int] IDENTITY(1,1) NOT NULL,
	[StateID] [int] NOT NULL,
	[Value] [int] NOT NULL
) ON [PRIMARY]
GO

ALTER TABLE [ZipCodes]
   ADD CONSTRAINT PK_ZipCodes_ZipCodeID PRIMARY KEY CLUSTERED (ZipCodeID);

ALTER TABLE ZipCodes
ADD CONSTRAINT FK_ZipCodes_States FOREIGN KEY (StateID)
        REFERENCES States (StateID)

CREATE TABLE [dbo].[Fees](
	[FeeID] [int] IDENTITY(1,1) NOT NULL,
	[ZipCodeID] [int] NOT NULL,
	[Value] [float] NOT NULL
) ON [PRIMARY]
GO

ALTER TABLE [Fees]
   ADD CONSTRAINT PK_Fees_FeeID PRIMARY KEY CLUSTERED (FeeID);

ALTER TABLE [Fees]
ADD CONSTRAINT FK_Fees_ZipCodes FOREIGN KEY (ZipCodeID)
        REFERENCES ZipCodes (ZipCodeID)

CREATE TABLE [dbo].[VehicleFees](
	[VehicleFeeID] [int] IDENTITY(1,1) NOT NULL,
	[StateID] [int] NOT NULL,
	[Type] [tinyint] NOT NULL,
	[Koefficient] [float] null,
	[IsAvailable] [bit] not null,
	[Remarks] nvarchar(max) null
) ON [PRIMARY]
GO

ALTER TABLE [VehicleFees]
   ADD CONSTRAINT PK_VehicleFees_VehicleFeeID PRIMARY KEY CLUSTERED (VehicleFeeID);

ALTER TABLE [VehicleFees]
ADD CONSTRAINT FK_VehicleFees_States FOREIGN KEY (StateID)
        REFERENCES [USDb].[dbo].[States] (StateID)
		GO