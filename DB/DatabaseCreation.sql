-- DB Creation
IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'PersonnalWebsite')
BEGIN
	CREATE DATABASE PersonnalWebsite;
	END
GO
-- Use DB
USE PersonnalWebsite;
GO
 
-- Table Creation
-- Create Users
IF NOT EXISTS(SELECT * FROM sysobjects WHERE name='Users')
BEGIN
	CREATE TABLE Users (
		UserID UNIQUEIDENTIFIER PRIMARY KEY,
		LastName NVARCHAR(255) NOT NULL,
		FirstName NVARCHAR(255) NOT NULL,
		Username NVARCHAR(255) NOT NULL,
		Email NVARCHAR(255) NOT NULL,
		PasswordHash VARBINARY(MAX) ,
		PasswordSalt VARBINARY(MAX) ,
		Age INT NOT NULL,
		CreatedAt DATETIME2 NOT NULL,
		LastModifiedAt DATETIME2 NOT NULL
	);
END

---- Insert test users if Users table is empty
--IF NOT EXISTS (SELECT 1 FROM Users)
--BEGIN
--	INSERT INTO Users (UserID, LastName, FirstName, Username, Email,/* PasswordHash, PasswordSalt,*/ Age, CreatedAt, LastModifiedAt)
--	VALUES 
--	(NEWID(), 'Jackson', 'Michael', 'MJ', 'MichaelJackson@hotmail.com', 67, GETDATE(), GETDATE()),
--	(NEWID(), 'Black', 'Jack', 'JB_FromD', 'Jack_Black@gmail.com', 55, GETDATE(), GETDATE()),
--	(NEWID(), 'E', 'Eazy', 'EazyMfinE92', 'EazyE@outlook.com', 56, GETDATE(), GETDATE()),
--	(NEWID(), 'Biden', 'Joe', 'Brandon56', 'Joe.Bidden@gmail.com', 85, GETDATE(), GETDATE()),
--	(NEWID(), 'Brown', 'James', 'Brownie69', 'this_is_a_mans_world@hotmail.com', 90, GETDATE(), GETDATE());
--END