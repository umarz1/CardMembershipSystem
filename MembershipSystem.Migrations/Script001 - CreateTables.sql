﻿CREATE TABLE Employees (
	EmployeeId VARCHAR(4) NOT NULL CONSTRAINT PK_Employees PRIMARY KEY,
	Name NVARCHAR(200) NOT NULL,
	Email NVARCHAR(200) NOT NULL,
	Mobile NVARCHAR(200) NOT NULL,
)
GO

CREATE TABLE Cards (
	CardId VARCHAR(16) NOT NULL CONSTRAINT PK_CardUsers PRIMARY KEY,
	EmployeeId VARCHAR(4) NOT NULL CONSTRAINT FK_CardUsers FOREIGN KEY (EmployeeId) REFERENCES Employees (EmployeeId),
	Pin  VARCHAR(4)
)
GO

