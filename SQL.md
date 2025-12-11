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
    p.CompanyId,
    tr.ProjectId,
    tr.TopicId,
    tr.Note,
    c.CompanyName,           -- Kolonne 8
    p.Title AS ProjectTitle, -- Kolonne 9
    t.TopicDescription       -- Kolonne 10
FROM dbo.TimeRecord tr
LEFT JOIN dbo.Project p ON tr.ProjectId = p.ProjectId
LEFT JOIN dbo.Company c ON p.CompanyId = c.CompanyId
LEFT JOIN dbo.Topic t ON tr.TopicId = t.TopicId;
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

-- TimeRecords
INSERT INTO dbo.TimeRecord (TimerName, ElapsedTime, StartTime, ProjectId, TopicId, Note)
VALUES
    ('Sprint planning', '02:30:00', '2025-01-15 10:00:00', 1, 2, ''),
    ('Bug fixing', '03:45:00', '2025-01-16 09:00:00', 4, 8, ''),
    ('Code review', '01:15:00', '2025-01-17 14:00:00', 7, 5, ''),

    -- Nordic Web Solutions ApS (Company 1) - Website Redesign (Project 1)
    ('Morgen standup', '00:15:00', '2024-11-15 09:00:00', 1, 1, ''),
    ('Sprint planning', '02:30:00', '2024-11-18 10:00:00', 1, 2, ''),
    ('Bug fixing header', '03:45:00', '2024-11-20 09:00:00', 1, 8, ''),
    ('Code review', '01:15:00', '2024-11-22 14:00:00', 1, 5, ''),
    ('Testing responsive', '02:00:00', '2024-11-25 13:00:00', 1, 12, ''),
    
    -- Nordic Web Solutions ApS - E-commerce Platform (Project 2)
    ('Refinement session', '02:00:00', '2024-12-02 10:00:00', 2, 3, ''),
    ('Implementering stripe', '04:30:00', '2024-12-05 08:30:00', 2, 8, ''),
    ('Support tickets', '01:45:00', '2024-12-10 15:00:00', 2, 11, ''),
    
    -- DataVision Consulting (Company 2) - Data Warehouse Migration (Project 4)
    ('Daily standup', '00:15:00', '2024-12-03 09:00:00', 4, 1, 'Team sync'),
    ('Migration script', '05:00:00', '2024-12-04 10:00:00', 4, 8, ''),
    ('Status meeting', '01:00:00', '2024-12-06 14:00:00', 4, 1, 'Status med kunde'),
    ('Testing migration', '03:30:00', '2024-12-09 09:00:00', 4, 12, 'Arbejd videre'),
    
    -- DataVision Consulting - BI Dashboard Setup (Project 5)
    ('Retrospective', '01:30:00', '2025-01-08 13:00:00', 5, 4, 'Sprint retrospektiv'),
    ('Dashboard design', '04:00:00', '2025-01-10 09:00:00', 5, 8, 'Power BI dashboard setup'),
    ('Code review', '01:00:00', '2025-01-13 15:00:00', 5, 5, 'Review af DAX queries'),
    
    -- GreenTech Innovations (Company 3) - Solar Monitoring App (Project 7)
    ('Sprint planning', '02:15:00', '2025-01-15 10:00:00', 7, 2, 'Sprint 3 planning'),
    ('Onboarding', '03:00:00', '2025-01-16 09:00:00', 7, 13, 'Onboarding af ny udvikler'),
    ('Feature udvikling', '05:30:00', '2025-01-20 08:00:00', 7, 8, ''),
    ('Testing app', '02:45:00', '2025-01-22 13:00:00', 7, 12, 'Integration tests'),
    
    -- BlueWave Software (Company 4) - CRM System Upgrade (Project 8)
    ('Daily standup', '00:15:00', '2025-02-03 09:00:00', 8, 1, 'Quick team sync'),
    ('Refinement', '02:00:00', '2025-02-05 10:00:00', 8, 3, 'User story refinement'),
    ('Upgrade implementation', '06:00:00', '2025-02-07 08:00:00', 8, 8, 'CRM version upgrade'),
    ('Support tickets', '02:30:00', '2025-02-10 14:00:00', 8, 11, 'Post-upgrade support'),
    
    -- BlueWave Software - Subscription Billing Module (Project 9)
    ('IO Weekly', '01:00:00', '2025-02-12 15:00:00', 9, 6, 'Sank med Henrik'),
    ('Billing logic', '04:45:00', '2025-02-14 09:00:00', 9, 8, ''),
    ('Code review', '01:30:00', '2025-02-17 14:00:00', 9, 5, '');    
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

-- TimeRecord Stored Procedures

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
           tr.Note,
           c.CompanyName,
           p.Title AS ProjectTitle,
           t.TopicDescription

           
   FROM dbo.TimeRecord tr
   LEFT JOIN dbo.Project p ON tr.ProjectId = p.ProjectId
   LEFT JOIN dbo.Company c ON p.CompanyId = c.CompanyId
   LEFT JOIN dbo.Topic t ON tr.TopicId = t.TopicId
   WHERE (@CompanyId IS NULL OR p.CompanyId = @CompanyId)   -- filter på virksomhed først
     AND (@ProjectId IS NULL OR tr.ProjectId = @ProjectId)  -- filter på projekt
     AND (@TopicId IS NULL OR tr.TopicId = @TopicId)        -- filter på emne
     AND (@Month IS NULL OR MONTH(tr.StartTime) = @Month)
     AND (@Year IS NULL OR YEAR(tr.StartTime) = @Year)
   ORDER BY tr.StartTime DESC;
END;
GO




