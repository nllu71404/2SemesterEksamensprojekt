-- 1. Drop DB hvis den findes
------------------------------------------------------------
USE master;
GO

IF DB_ID('StackhouseDB') IS NOT NULL
BEGIN
    ALTER DATABASE StackhouseDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE StackhouseDB;
END
GO

------------------------------------------------------------
-- 2. Opret database (uden lokale paths)
------------------------------------------------------------
CREATE DATABASE StackhouseDB;
GO

USE StackhouseDB;
GO


------------------------------------------------------------
-- 3. TABELLER
------------------------------------------------------------

-- Company
CREATE TABLE dbo.Company
(
    CompanyId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    CompanyName NVARCHAR(100) NOT NULL
);
GO

-- Project
CREATE TABLE dbo.Project
(
    ProjectId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    CompanyId INT NOT NULL,
    Title NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX),

    FOREIGN KEY (CompanyId) REFERENCES dbo.Company(CompanyId)
);
GO

-- Topic
CREATE TABLE dbo.Topic
(
    TopicId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    TopicDescription NVARCHAR(200) NOT NULL
);
GO

-- TimeRecord
CREATE TABLE dbo.TimeRecord
(
    TimerId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    TimerName NVARCHAR(100) NOT NULL,
    ElapsedTime TIME NOT NULL,
    StartTime DATETIME NOT NULL DEFAULT(GETDATE()),
    Note NVARCHAR(200),
    ProjectId INT NULL,
    TopicId INT NULL,

    CONSTRAINT FK_TimeRecord_Project FOREIGN KEY (ProjectId)
        REFERENCES Project(ProjectId),

    CONSTRAINT FK_TimeRecord_Topic FOREIGN KEY (TopicId)
        REFERENCES Topic(TopicId)
);
GO


------------------------------------------------------------
-- 4. VIEWS
------------------------------------------------------------
CREATE VIEW dbo.vwSelectAllCompanies AS
SELECT CompanyId, CompanyName FROM dbo.Company;
GO

CREATE VIEW dbo.vwSelectAllProjects AS
SELECT ProjectId, CompanyId, Title, Description FROM dbo.Project;
GO

CREATE VIEW dbo.vwSelectAllTopics AS
SELECT TopicId, TopicDescription FROM dbo.Topic;
GO

CREATE VIEW dbo.vwSelectAllTimeRecords AS
SELECT 
   tr.TimerId,
   tr.TimerName,
   tr.ElapsedTime,
   tr.StartTime,
   p.CompanyId,    -- hent CompanyId fra Project
   tr.ProjectId,
   tr.TopicId,
   tr.Note
FROM dbo.TimeRecord tr
INNER JOIN dbo.Project p ON tr.ProjectId = p.ProjectId;
GO


------------------------------------------------------------
-- 5. SEED DATA
------------------------------------------------------------

-- Companies
SET IDENTITY_INSERT dbo.Company ON;

INSERT INTO dbo.Company (CompanyId, CompanyName) VALUES
(1, N'Nordic Web Solutions ApS'),
(2, N'DataVision Consulting ret'),
(3, N'GreenTech Innovations'),
(4, N'BlueWave Software'),
(5, N'CloudCore IT Services');

SET IDENTITY_INSERT dbo.Company OFF;
GO


-- Projects
SET IDENTITY_INSERT dbo.Project ON;

INSERT INTO dbo.Project (ProjectId, CompanyId, Title, Description) VALUES
(1, 1, N'Website Redesign', N'Komplet redesign af kundens corporate website.'),
(2, 1, N'E-commerce Platform', N'Udvikling af ny online butik med betalingsintegration.'),
(3, 1, N'SEO Optimization', N'Teknisk optimering og content-analyse.'),

(4, 2, N'Data Warehouse Migration', N'Flytning af legacy data warehouse til Azure SQL.'),
(5, 2, N'BI Dashboard Setup', N'Power BI dashboards til ledelsesrapportering.'),
(6, 2, N'Customer Insights Model', N'Machine learning model for kundeanalyse.'),

(7, 3, N'Solar Monitoring App', N'Mobilapp til overvågning af solcelleproduktion.'),

(8, 4, N'CRM System Upgrade', N'Opgradering af CRM-system til ny version.'),
(9, 4, N'Subscription Billing Module', N'Udvikling af modul til abonnementsbetaling.'),
(10,4, N'User Authentication Rewrite', N'Implementering af OAuth2 + MFA.');

SET IDENTITY_INSERT dbo.Project OFF;
GO


------------------------------------------------------------
-- 6. STORED PROCEDURES
------------------------------------------------------------

-- Create
CREATE PROCEDURE dbo.uspCreateCompany
    @CompanyName NVARCHAR(100)
AS
BEGIN
    INSERT INTO dbo.Company (CompanyName)
    VALUES (@CompanyName);

    SELECT SCOPE_IDENTITY() AS NewCompanyId;
END;
GO

CREATE PROCEDURE dbo.uspCreateProject
    @CompanyId INT,
    @Title NVARCHAR(100),
    @Description NVARCHAR(MAX)
AS
BEGIN
    INSERT INTO dbo.Project (CompanyId, Title, Description)
    VALUES (@CompanyId, @Title, @Description);

    SELECT SCOPE_IDENTITY() AS NewProjectId;
END;
GO

CREATE PROCEDURE dbo.uspCreateTopic
    @TopicDescription NVARCHAR(200)
AS
BEGIN
    INSERT INTO dbo.Topic (TopicDescription)
    VALUES (@TopicDescription);

    SELECT SCOPE_IDENTITY() AS NewTopicId;
END;
GO


-- Delete
CREATE PROCEDURE dbo.uspDeleteCompany
    @CompanyId INT
AS
BEGIN
    DELETE FROM dbo.Company WHERE CompanyId = @CompanyId;
END;
GO

CREATE PROCEDURE dbo.uspDeleteProject
    @ProjectId INT
AS
BEGIN
    DELETE FROM dbo.Project WHERE ProjectId = @ProjectId;
END;
GO

CREATE PROCEDURE dbo.uspDeleteTopic
    @TopicId INT
AS
BEGIN
    DELETE FROM dbo.Topic WHERE TopicId = @TopicId;
END;
GO


-- Update
CREATE PROCEDURE dbo.uspUpdateCompany
    @CompanyId INT,
    @CompanyName NVARCHAR(100)
AS
BEGIN
    UPDATE dbo.Company
    SET CompanyName = @CompanyName
    WHERE CompanyId = @CompanyId;
END;
GO

CREATE PROCEDURE dbo.uspUpdateProject
    @ProjectId INT,
    @CompanyId INT,
    @Title NVARCHAR(100),
    @Description NVARCHAR(MAX)
AS
BEGIN
    UPDATE dbo.Project
    SET CompanyId = @CompanyId,
        Title = @Title,
        Description = @Description
    WHERE ProjectId = @ProjectId;
END;
GO

CREATE PROCEDURE dbo.uspUpdateTopic
    @TopicId INT,
    @TopicDescription NVARCHAR(200)
AS
BEGIN
    UPDATE dbo.Topic
    SET TopicDescription = @TopicDescription
    WHERE TopicId = @TopicId;
END;
GO


-- Utility
CREATE PROCEDURE dbo.uspGetProjectsByCompanyId
    @CompanyId INT
AS
BEGIN
    SELECT ProjectId, CompanyId, Title, Description
    FROM dbo.Project
    WHERE CompanyId = @CompanyId;
END;
GO

------------------------------------------------------------
-- 7. SEED TOPICS
------------------------------------------------------------

INSERT INTO dbo.Topic (TopicDescription)
VALUES
    ('DSU'),
    ('Sprint Planning'),
    ('Refinement'),
    ('Retrospective'),
    ('Code Review'),
    ('IO Weekly'),
    ('Diff. DSU'),
    ('Bugfixes'),
    ('Map Availability'),
    ('Status Meeting'),
    ('Support tickets'),
    ('Testing'),
    ('Onboarding');
GO

CREATE PROCEDURE [dbo].[uspCreateTimeRecord]
    @TimerName NVARCHAR(100),
    @ElapsedTime TIME,
    @StartTime DATETIME,
    @ProjectId INT = NULL,
    @TopicId INT = NULL,
    @Note NVARCHAR(MAX) = NULL  
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.TimeRecord
    (
        TimerName,
        ElapsedTime,
        StartTime,
        ProjectId,
        TopicId,
        Note              
    )
    VALUES
    (
        @TimerName,
        @ElapsedTime,
        @StartTime,
        @ProjectId,
        @TopicId,
        @Note            
    );

    SELECT SCOPE_IDENTITY() AS NewTimeRecordId;
    END;
GO

CREATE PROCEDURE [dbo].[uspFilterTimeRecords]
   @CompanyId INT = NULL,   -- filter på virksomhed
   @ProjectId INT = NULL,   -- filter på projekt
   @TopicId INT = NULL,     -- filter på emne
   @Month INT = NULL,
   @Year INT = NULL
AS
BEGIN
   SET NOCOUNT ON;
 
   SELECT  tr.TimerId,
          tr.TimerName,
          tr.ElapsedTime,
          tr.StartTime,
          p.CompanyId,   -- hent CompanyId via Project
          tr.ProjectId,
          tr.TopicId,
           tr.Note
   FROM dbo.TimeRecord tr
   INNER JOIN dbo.Project p ON tr.ProjectId = p.ProjectId
   WHERE (@CompanyId IS NULL OR p.CompanyId = @CompanyId)   -- filter på virksomhed først
     AND (@ProjectId IS NULL OR tr.ProjectId = @ProjectId)  -- filter på projekt
     AND (@TopicId IS NULL OR tr.TopicId = @TopicId)        -- filter på emne
     AND (@Month IS NULL OR MONTH(tr.StartTime) = @Month)
     AND (@Year IS NULL OR YEAR(tr.StartTime) = @Year)
   ORDER BY tr.StartTime DESC;
END;
GO




