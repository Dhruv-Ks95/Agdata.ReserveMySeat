CREATE TABLE [dbo].[Employees] (
    [EmployeeId] INT IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (100) NOT NULL,
    [Email] NVARCHAR (100) NOT NULL UNIQUE,
    [Role] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Employees] PRIMARY KEY CLUSTERED ([EmployeeId] ASC)
);
