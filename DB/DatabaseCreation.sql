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
		Name NVARCHAR(255) NOT NULL,
		FirstName NVARCHAR(255) NOT NULL,
		Email NVARCHAR(255) NOT NULL,
		Age INT NOT NULL,
		CreatedAt DATETIME NOT NULL,
		LastModifiedAt DATETIME NOT NULL
	);
END

-- Insert test users if Users table is empty
IF NOT EXISTS (SELECT 1 FROM Users)
BEGIN
	INSERT INTO Users (UserID, Name, FirstName, Email, Age, CreatedAt, LastModifiedAt)
	VALUES 
	(NEWID(), 'Jackson', 'Michael', 'MichaelJackson@hotmail.com', 67, GETDATE(), GETDATE()),
	(NEWID(), 'Black', 'Jack', 'Jack_Black@gmail.com', 55, GETDATE(), GETDATE()),
	(NEWID(), 'E', 'Eazy', 'EazyE@outlook.com', 56, GETDATE(), GETDATE()),
	(NEWID(), 'Biden', 'Joe', 'Joe.Bidden@gmail.com', 85, GETDATE(), GETDATE()),
	(NEWID(), 'Brown', 'James', 'this_is_a_mans_world@hotmail.com', 90, GETDATE(), GETDATE());
END