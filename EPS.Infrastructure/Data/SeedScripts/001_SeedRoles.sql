-- Seed Roles (Essential System Data)
-- This script can be run manually if needed

-- Check if roles table is empty
IF NOT EXISTS (SELECT 1 FROM AspNetRoles)
BEGIN
    -- Insert Roles
    INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
    VALUES 
        (NEWID(), 'Admin', 'ADMIN', NEWID()),
        (NEWID(), 'HR Manager', 'HR MANAGER', NEWID()),
        (NEWID(), 'Manager', 'MANAGER', NEWID()),
        (NEWID(), 'Employee', 'EMPLOYEE', NEWID());

    PRINT 'Roles seeded successfully';
END
ELSE
BEGIN
    PRINT 'Roles already exist';
END