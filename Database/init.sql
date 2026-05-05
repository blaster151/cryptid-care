-- Wait for SQL Server to be ready (handled by healthcheck in docker-compose).
-- This script runs once when the container is first created.

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'CryptidCare')
BEGIN
    CREATE DATABASE CryptidCare;
END
GO

USE CryptidCare;
GO

-- Schema
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Patients')
BEGIN
    CREATE TABLE Patients (
        Id              UNIQUEIDENTIFIER    NOT NULL DEFAULT NEWSEQUENTIALID() PRIMARY KEY,
        Name            NVARCHAR(200)       NOT NULL,
        Species         NVARCHAR(50)        NOT NULL,
        HeadCount       INT                 NULL
    );

    CREATE TABLE Medicines (
        Id              UNIQUEIDENTIFIER    NOT NULL DEFAULT NEWSEQUENTIALID() PRIMARY KEY,
        Name            NVARCHAR(200)       NOT NULL,
        ContainsSilver  BIT                 NOT NULL DEFAULT 0
    );

    CREATE TABLE Claims (
        Id                  UNIQUEIDENTIFIER    NOT NULL DEFAULT NEWSEQUENTIALID() PRIMARY KEY,
        PatientId           UNIQUEIDENTIFIER    NOT NULL REFERENCES Patients(Id),
        MedicineId          UNIQUEIDENTIFIER    NOT NULL REFERENCES Medicines(Id),
        ExternalReferenceId NVARCHAR(100)       NULL,
        RequestedQuantity   INT                 NOT NULL,
        DispensedQuantity   INT                 NOT NULL,
        Status              NVARCHAR(20)        NOT NULL,
        RejectionReason     NVARCHAR(500)       NULL,
        CreatedAt           DATETIME2           NOT NULL DEFAULT SYSUTCDATETIME()
    );

    -- Seed data
    INSERT INTO Patients (Id, Name, Species, HeadCount) VALUES
        ('a1a1a1a1-0000-0000-0000-000000000001', 'Remus Lupin',     'Werewolf', NULL),
        ('a1a1a1a1-0000-0000-0000-000000000002', 'Fenrir Greyback', 'Werewolf', NULL),
        ('a1a1a1a1-0000-0000-0000-000000000003', 'Lernaean',        'Hydra',    9),
        ('a1a1a1a1-0000-0000-0000-000000000004', 'Hydra Junior',    'Hydra',    3),
        ('a1a1a1a1-0000-0000-0000-000000000005', 'Fawkes',          'Phoenix',  NULL),
        ('a1a1a1a1-0000-0000-0000-000000000006', 'Nessie',          'Other',    NULL);

    INSERT INTO Medicines (Id, Name, ContainsSilver) VALUES
        ('b2b2b2b2-0000-0000-0000-000000000001', 'Silver Sulfadiazine', 1),
        ('b2b2b2b2-0000-0000-0000-000000000002', 'Colloidal Silver',    1),
        ('b2b2b2b2-0000-0000-0000-000000000003', 'Regeneron',           0),
        ('b2b2b2b2-0000-0000-0000-000000000004', 'Amortentia Antidote', 0),
        ('b2b2b2b2-0000-0000-0000-000000000005', 'Phoenix Ash Elixir',  0);
END
GO
