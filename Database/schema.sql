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
