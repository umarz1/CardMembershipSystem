INSERT INTO Employees
           (EmployeeId
           ,FirstName
           ,MiddleName
           ,LastName
           ,Role
           ,StartDate
           ,Grade
           ,Email
           ,Mobile)
     VALUES
           ('1234'
           ,'Muhammad'
           ,'Umar'
           ,'Zahir'
           ,'Software Engineer'
           ,(SELECT CAST( GETDATE() AS Date )) 
           ,'C3'
           ,'muhammad.zahir@sainsburys.co.uk'
           ,'07746314396')
GO

INSERT INTO Employees
           (EmployeeId
           ,FirstName
           ,MiddleName
           ,LastName
           ,Role
           ,StartDate
           ,Grade
           ,Email
           ,Mobile)
     VALUES
           ('1235'
           ,'John'
           ,null
           ,'Doe'
           ,'Product Manager'
           ,(SELECT CAST( GETDATE() AS Date ))
           ,'C4'
           ,'john.doe@sainsburys.co.uk'
           ,'07746314392')
GO


INSERT INTO Employees
           (EmployeeId
           ,FirstName
           ,MiddleName
           ,LastName
           ,Role
           ,StartDate
           ,Grade
           ,Email
           ,Mobile)
     VALUES
           ('1236'
           ,'Laura'
           ,null
           ,'Stevens'
           ,'Delivery Manager'
           ,(SELECT CAST( GETDATE() AS Date ))
           ,'C4'
           ,'laura.stevens@sainsburys.co.uk'
           ,'07746314391')
GO

INSERT INTO CardUsers
           (CardId
           ,EmployeeId
           ,Name
           ,Email
           ,Mobile)
     VALUES
           ('VyDJ0lbYcPkzp2Ju'
           ,'1234'
           ,'Muhammad Umar Zahir'
           ,'muhammad.zahir@sainsburys.co.uk'
           ,'07746314396')
GO
