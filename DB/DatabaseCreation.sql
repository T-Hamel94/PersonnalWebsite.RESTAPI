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
		AuthorID UNIQUEIDENTIFIER,
		Content NVARCHAR(MAX) NOT NULL,
		IsApproved BIT, 
		CreatedAt DATETIME2 NOT NULL,
		LastModifiedAt DATETIME2 NOT NULL
	);
END

-- Insert test Articles into the DB if BlogPosts table is empty
IF NOT EXISTS (SELECT 1 FROM BlogPosts)
BEGIN
	-- Insert 5 articles
INSERT INTO BlogPosts (BlogPostID, BlogPostLanguageID, Title, Author, AuthorID, Content, IsApproved, CreatedAt, LastModifiedAt) 
VALUES 
	(NEWID(), 1, 'The future of AI', 'John Doe','F040929F-7A59-4E88-92CC-D6800B365F6B', 'The future of AI is bright. This article explores...', 1,  GETDATE(), GETDATE()),
	(NEWID(), 1, 'Quantum Computing', 'Jane Smith','F040929F-7A59-4E88-92CC-D6800B365F6B', 'Quantum computing is the next frontier...', GETDATE(), 1,  GETDATE()),
	(NEWID(), 1, 'The Rise of Blockchain', 'Alice Johnson','F040929F-7A59-4E88-92CC-D6800B365F6B', 'Blockchain technology is reshaping the world...', 1,  GETDATE(), GETDATE()),
	(NEWID(), 1, 'Big Data Analysis', 'Bob Brown','F040929F-7A59-4E88-92CC-D6800B365F6B', 'Big data is becoming increasingly important...', 1, GETDATE(), GETDATE()),
	(NEWID(), 1, 'Cybersecurity in 2023', 'Charlie Davis','F040929F-7A59-4E88-92CC-D6800B365F6B', 'In the age of digital information, cybersecurity is crucial...', 1, GETDATE(), GETDATE());
END


-- Delete all rows from the Users table
DELETE FROM Users;


-- Delete all rows from Blogpost table
DELETE FROM BlogPosts;


SELECT * FROM BlogPosts;