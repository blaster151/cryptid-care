-- Patients
INSERT INTO Patients (Id, Name, Species, HeadCount) VALUES
    ('a1a1a1a1-0000-0000-0000-000000000001', 'Remus Lupin',    'Werewolf', NULL),
    ('a1a1a1a1-0000-0000-0000-000000000002', 'Fenrir Greyback', 'Werewolf', NULL),
    ('a1a1a1a1-0000-0000-0000-000000000003', 'Lernaean',        'Hydra',    9),
    ('a1a1a1a1-0000-0000-0000-000000000004', 'Hydra Junior',    'Hydra',    3),
    ('a1a1a1a1-0000-0000-0000-000000000005', 'Fawkes',          'Phoenix',  NULL),
    ('a1a1a1a1-0000-0000-0000-000000000006', 'Nessie',          'Other',    NULL);

-- Medicines
INSERT INTO Medicines (Id, Name, ContainsSilver) VALUES
    ('b2b2b2b2-0000-0000-0000-000000000001', 'Silver Sulfadiazine',  1),
    ('b2b2b2b2-0000-0000-0000-000000000002', 'Colloidal Silver',     1),
    ('b2b2b2b2-0000-0000-0000-000000000003', 'Regeneron',            0),
    ('b2b2b2b2-0000-0000-0000-000000000004', 'Amortentia Antidote',  0),
    ('b2b2b2b2-0000-0000-0000-000000000005', 'Phoenix Ash Elixir',   0);
