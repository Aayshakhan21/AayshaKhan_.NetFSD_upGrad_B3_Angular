

CREATE DATABASE EventDb;
USE EventDb;

---- first query ----

CREATE TABLE UserInfo(
     EmailId VARCHAR(50) PRIMARY KEY,

     UserName VARCHAR(50) NOT NULL
      CHECK (LEN(UserName) BETWEEN 1 AND 50), 
      
     Role VARCHAR(25) NOT NULL
      CHECK (Role IN('Admin','Participant')) ,

     Password VARCHAR(20) NOT NULL
       CHECK (LEN(Password) BETWEEN 6 AND 20)

);

INSERT INTO UserInfo VALUES
('admin@gmail.com','AdminUser','Admin','admin123'),
('aaysha@gmail.com','Aaysha','Participant','pass123'),
('john@gmail.com','John','Participant','john456');

SELECT * FROM UserInfo;

----- Second Query -----

CREATE TABLE EventDetails (
   EventId INT PRIMARY KEY,

   EventName VARCHAR(50) NOT NULL
      CHECK (LEN(EventName) BETWEEN 1 AND 50),

   EventCategory VARCHAR(50) NOT NULL
      CHECK (LEN(EventCategory) BETWEEN 1 AND 50), 

   EventDate DATETIME NOT NULL, 
   
   Description VARCHAR(255) NULL,
    
   Status VARCHAR(15) NOT NULL DEFAULT 'Active' 
        CHECK (Status IN ('Active','In-Active'))
);

INSERT INTO EventDetails VALUES
(1,'Tech Conference','Technology','2026-04-10','Annual tech event','Active'),
(2,'AI Summit','Artificial Intelligence','2026-05-15','AI focused event','Active');

SELECT * FROM EventDetails;

---- third query ----

CREATE TABLE SpeakersDetails
(
    SpeakerId INT PRIMARY KEY,
    
    SpeakerName VARCHAR(50) NOT NULL
      CHECK (LEN(SpeakerName) BETWEEN 1 AND 50)

);

INSERT INTO SpeakersDetails VALUES
(1,'Aman Dhattarwal'),
(2,'Code with Harry');

SELECT * FROM SpeakersDetails;

------ four query -----


CREATE TABLE SessionInfo(
   
   SessionId INT PRIMARY KEY,

   EventId INT FOREIGN KEY REFERENCES EventDetails(EventId),

   SessionTitle VARCHAR(50) NOT NULL
      CHECK (LEN(SessionTitle) BETWEEN 1 AND 50),

   SpeakerId INT  FOREIGN KEY REFERENCES SpeakersDetails(SpeakerId),
   
   Description VARCHAR(255) NULL, 

   SessionStart DATETIME NOT NULL,

   SessionEnd DATETIME NOT NULL
     CHECK (SessionEnd > SessionStart),

   SessionUrl VARCHAR(255)  
);
SELECT * FROM SessionInfo;

------ query five ------

CREATE TABLE ParticipantEventDetails (
   Id INT PRIMARY KEY,
 
   ParticipantEmailId VARCHAR(50) NOT NULL FOREIGN KEY REFERENCES UserInfo(EmailId),
  
   EventId INT FOREIGN KEY REFERENCES EventDetails(EventId),

   SpeakerId INT  FOREIGN KEY REFERENCES SpeakersDetails(SpeakerId)
);
SELECT * FROM ParticipantEventDetails;