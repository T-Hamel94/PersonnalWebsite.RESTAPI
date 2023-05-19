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
		Birthdate DATETIME2 NOT NULL,
		IsAdmin BIT, 
		CreatedAt DATETIME2 NOT NULL,
		LastModifiedAt DATETIME2 NOT NULL
	);
END

-- Create BlogPosts
IF NOT EXISTS(SELECT * FROM sysobjects WHERE name='BlogPosts')
BEGIN
	CREATE TABLE BlogPosts (
		BlogPostID UNIQUEIDENTIFIER PRIMARY KEY,
		BlogPostLanguageID INT NOT NULL,
		Title NVARCHAR(255) NOT NULL,
		Author NVARCHAR(255) NOT NULL,
		Content NVARCHAR(MAX) NOT NULL,
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

-- Insert test Articles into the DB if BlogPosts table is empty
IF NOT EXISTS (SELECT 1 FROM BlogPosts)
BEGIN
	-- Insert 5 articles
INSERT INTO BlogPosts (BlogPostID, BlogPostLanguageID, Title, Author, Content, CreatedAt, LastModifiedAt) 
VALUES 
	(NEWID(), 1, 'The future of AI', 'John Doe', 'The future of AI is bright. This article explores...', GETDATE(), GETDATE()),
	(NEWID(), 1, 'Quantum Computing', 'Jane Smith', 'Quantum computing is the next frontier...', GETDATE(), GETDATE()),
	(NEWID(), 1, 'The Rise of Blockchain', 'Alice Johnson', 'Blockchain technology is reshaping the world...', GETDATE(), GETDATE()),
	(NEWID(), 1, 'Big Data Analysis', 'Bob Brown', 'Big data is becoming increasingly important...', GETDATE(), GETDATE()),
	(NEWID(), 1, 'Cybersecurity in 2023', 'Charlie Davis', 'In the age of digital information, cybersecurity is crucial...', GETDATE(), GETDATE());
END