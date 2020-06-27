INSERT INTO Members
           (EmployeeId
           ,Name
           ,Email
           ,Mobile)
     VALUES
           ('1234'
           ,'Umar Zahir'
           ,'muhammad.zahir@sainsburys.co.uk'
           ,'07746314396')
GO

INSERT INTO Cards
           (CardId
           ,EmployeeId
           ,Pin)
     VALUES
           ('VyDJ0lbYcPkzp2Ju'
           ,'1234'
           ,'8881')
GO

INSERT INTO Transactions
           (TransactionId
           ,CardId
           ,Date
           ,Amount
           ,Balance)
     VALUES
           ('a4040c1d-09f5-4dc9-aff7-0cae8747bcfe'
           ,'VyDJ0lbYcPkzp2Ju'
           ,'2020-06-25 00:00:00'
           ,0
           ,0)
GO

