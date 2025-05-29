CREATE DATABASE LibraryDB;
GO

USE LibraryDB;
GO

CREATE TABLE Authors (
    AuthorID INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Biography NVARCHAR(MAX),
    BirthDate DATE
);

CREATE TABLE Books (
    BookID INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(100) NOT NULL,
    AuthorID INT NOT NULL,
    PublicationYear INT CHECK (PublicationYear > 0 AND PublicationYear <= YEAR(GETDATE())),
    AvailableCopies INT NOT NULL DEFAULT 0 CHECK (AvailableCopies >= 0),
    ISBN NVARCHAR(20),
    Description NVARCHAR(MAX),
    FOREIGN KEY (AuthorID) REFERENCES Authors(AuthorID) ON DELETE CASCADE
);

CREATE TABLE Readers (
    ReaderID INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) UNIQUE,
    RegistrationDate DATE NOT NULL DEFAULT GETDATE(),
    Phone NVARCHAR(20)
);

CREATE TABLE Orders (
    OrderID INT PRIMARY KEY IDENTITY(1,1),
    BookID INT NOT NULL,
    ReaderID INT NOT NULL,
    IssueDate DATETIME NOT NULL DEFAULT GETDATE(),
    ReturnDate DATETIME NULL,
    ExpectedReturnDate DATETIME NOT NULL DEFAULT DATEADD(DAY, 30, GETDATE()),
    FOREIGN KEY (BookID) REFERENCES Books(BookID),
    FOREIGN KEY (ReaderID) REFERENCES Readers(ReaderID),
    CHECK (ReturnDate IS NULL OR ReturnDate >= IssueDate)
);

CREATE TABLE Reviews (
    ReviewID INT PRIMARY KEY IDENTITY(1,1),
    BookID INT NOT NULL,
    ReaderID INT NOT NULL,
    Rating INT NOT NULL CHECK (Rating BETWEEN 1 AND 5),
    Comment NVARCHAR(500),
    ReviewDate DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (BookID) REFERENCES Books(BookID) ON DELETE CASCADE,
    FOREIGN KEY (ReaderID) REFERENCES Readers(ReaderID),
    UNIQUE (BookID, ReaderID)
);

-- Индексы для улучшения производительности
CREATE INDEX IX_Books_AuthorID ON Books(AuthorID);
CREATE INDEX IX_Orders_BookID ON Orders(BookID);
CREATE INDEX IX_Orders_ReaderID ON Orders(ReaderID);
CREATE INDEX IX_Reviews_BookID ON Reviews(BookID);
CREATE INDEX IX_Readers_Email ON Readers(Email);