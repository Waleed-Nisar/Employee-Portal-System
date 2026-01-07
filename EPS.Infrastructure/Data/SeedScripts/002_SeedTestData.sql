-- Seed Test Data (Development Only)
-- This script provides sample data for testing

-- Check if test data already exists
IF NOT EXISTS (SELECT 1 FROM Departments)
BEGIN
    -- Seed Departments
    INSERT INTO Departments (Name, Code, Description, Location, IsActive, CreatedAt, UpdatedAt)
    VALUES 
        ('Information Technology', 'IT', 'IT and Software Development', 'Building A', 1, GETUTCDATE(), GETUTCDATE()),
        ('Human Resources', 'HR', 'Human Resources and Administration', 'Building B', 1, GETUTCDATE(), GETUTCDATE()),
        ('Finance', 'FIN', 'Finance and Accounting', 'Building B', 1, GETUTCDATE(), GETUTCDATE()),
        ('Sales', 'SAL', 'Sales and Marketing', 'Building C', 1, GETUTCDATE(), GETUTCDATE()),
        ('Operations', 'OPS', 'Operations and Support', 'Building A', 1, GETUTCDATE(), GETUTCDATE());

    -- Seed Designations
    INSERT INTO Designations (Title, Code, Description, Level, MinSalary, MaxSalary, IsActive, CreatedAt, UpdatedAt)
    VALUES 
        ('Chief Executive Officer', 'CEO', NULL, 1, 150000, 250000, 1, GETUTCDATE(), GETUTCDATE()),
        ('Manager', 'MGR', NULL, 2, 80000, 120000, 1, GETUTCDATE(), GETUTCDATE()),
        ('Senior Developer', 'SRDEV', NULL, 3, 70000, 100000, 1, GETUTCDATE(), GETUTCDATE()),
        ('Developer', 'DEV', NULL, 4, 50000, 75000, 1, GETUTCDATE(), GETUTCDATE()),
        ('HR Specialist', 'HRSP', NULL, 3, 45000, 65000, 1, GETUTCDATE(), GETUTCDATE()),
        ('Accountant', 'ACC', NULL, 3, 45000, 70000, 1, GETUTCDATE(), GETUTCDATE()),
        ('Sales Executive', 'SALEXEC', NULL, 4, 40000, 60000, 1, GETUTCDATE(), GETUTCDATE()),
        ('Support Specialist', 'SUPSP', NULL, 4, 35000, 50000, 1, GETUTCDATE(), GETUTCDATE());

    PRINT 'Test data seeded successfully';
END
ELSE
BEGIN
    PRINT 'Test data already exists';
END