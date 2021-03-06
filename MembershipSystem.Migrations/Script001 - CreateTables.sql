﻿CREATE TABLE Members (
	MemberId VARCHAR(4) NOT NULL CONSTRAINT PK_Members PRIMARY KEY,
	Name NVARCHAR(200) NOT NULL,
	Email NVARCHAR(200) NOT NULL,
	Mobile NVARCHAR(200) NOT NULL,
)
GO

CREATE TABLE Cards (
	CardId VARCHAR(16) NOT NULL CONSTRAINT PK_Cards PRIMARY KEY,
	MemberId VARCHAR(4) NOT NULL CONSTRAINT FK_Cards FOREIGN KEY (MemberId) REFERENCES Members (MemberId),
	Pin  VARCHAR(4)
)
GO

CREATE INDEX IX_Cards_MemberId ON Cards (MemberId);

CREATE TABLE Transactions (
	TransactionId UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_Transactions PRIMARY KEY,
	CardId VARCHAR(16) NOT NULL CONSTRAINT FK_Transactions FOREIGN KEY (CardId) REFERENCES Cards (CardId),
	Date DATETIME NOT NULL,
	Amount DECIMAL(10,2) NOT NULL DEFAULT 0, 
	Balance DECIMAL(10,2) NOT NULL DEFAULT 0, 
)
GO

CREATE INDEX IX_Transactions_CardId ON Transactions (CardId);
