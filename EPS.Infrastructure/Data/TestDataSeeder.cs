using EPS.Domain.Entities;
using EPS.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace EPS.Infrastructure.Data;

/// <summary>
/// Seeds TEST/SAMPLE data - ONLY runs in Development environment
/// Provides realistic data for testing and demonstration
/// </summary>
public static class TestDataSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        // Skip if data already exists
        if (await context.Departments.AnyAsync())
        {
            return;
        }

        // Seed Departments
        var departments = new List<Department>
        {
            new Department { Name = "Information Technology", Code = "IT", Description = "IT and Software Development", Location = "Building A", IsActive = true },
            new Department { Name = "Human Resources", Code = "HR", Description = "Human Resources and Administration", Location = "Building B", IsActive = true },
            new Department { Name = "Finance", Code = "FIN", Description = "Finance and Accounting", Location = "Building B", IsActive = true },
            new Department { Name = "Sales", Code = "SAL", Description = "Sales and Marketing", Location = "Building C", IsActive = true },
            new Department { Name = "Operations", Code = "OPS", Description = "Operations and Support", Location = "Building A", IsActive = true }
        };

        await context.Departments.AddRangeAsync(departments);
        await context.SaveChangesAsync();

        // Seed Designations
        var designations = new List<Designation>
        {
            new Designation { Title = "Chief Executive Officer", Code = "CEO", Level = 1, MinSalary = 150000, MaxSalary = 250000, IsActive = true },
            new Designation { Title = "Manager", Code = "MGR", Level = 2, MinSalary = 80000, MaxSalary = 120000, IsActive = true },
            new Designation { Title = "Senior Developer", Code = "SRDEV", Level = 3, MinSalary = 70000, MaxSalary = 100000, IsActive = true },
            new Designation { Title = "Developer", Code = "DEV", Level = 4, MinSalary = 50000, MaxSalary = 75000, IsActive = true },
            new Designation { Title = "HR Specialist", Code = "HRSP", Level = 3, MinSalary = 45000, MaxSalary = 65000, IsActive = true },
            new Designation { Title = "Accountant", Code = "ACC", Level = 3, MinSalary = 45000, MaxSalary = 70000, IsActive = true },
            new Designation { Title = "Sales Executive", Code = "SALEXEC", Level = 4, MinSalary = 40000, MaxSalary = 60000, IsActive = true },
            new Designation { Title = "Support Specialist", Code = "SUPSP", Level = 4, MinSalary = 35000, MaxSalary = 50000, IsActive = true }
        };

        await context.Designations.AddRangeAsync(designations);
        await context.SaveChangesAsync();

        // Seed Employees
        var employees = new List<Employee>
        {
            new Employee { EmployeeId = "EMP-0001", FirstName = "John", LastName = "Smith", Email = "john.smith@eps.com", Phone = "555-0101", DateOfBirth = new DateTime(1985, 5, 15), Gender = "Male", Address = "123 Main St", City = "New York", State = "NY", ZipCode = "10001", Country = "USA", DepartmentId = 1, DesignationId = 1, HireDate = new DateTime(2020, 1, 15), Status = EmployeeStatus.Active, Salary = 180000, EmergencyContactName = "Jane Smith", EmergencyContactPhone = "555-0102" },
            new Employee { EmployeeId = "EMP-0002", FirstName = "Sarah", LastName = "Johnson", Email = "sarah.johnson@eps.com", Phone = "555-0201", DateOfBirth = new DateTime(1988, 8, 22), Gender = "Female", Address = "456 Oak Ave", City = "New York", State = "NY", ZipCode = "10002", Country = "USA", DepartmentId = 2, DesignationId = 2, HireDate = new DateTime(2020, 3, 10), Status = EmployeeStatus.Active, Salary = 95000, ManagerId = 1, EmergencyContactName = "Mike Johnson", EmergencyContactPhone = "555-0202" },
            new Employee { EmployeeId = "EMP-0003", FirstName = "Michael", LastName = "Brown", Email = "michael.brown@eps.com", Phone = "555-0301", DateOfBirth = new DateTime(1990, 11, 5), Gender = "Male", Address = "789 Pine Rd", City = "New York", State = "NY", ZipCode = "10003", Country = "USA", DepartmentId = 1, DesignationId = 2, HireDate = new DateTime(2020, 6, 1), Status = EmployeeStatus.Active, Salary = 105000, ManagerId = 1, EmergencyContactName = "Lisa Brown", EmergencyContactPhone = "555-0302" },
            new Employee { EmployeeId = "EMP-0004", FirstName = "Emily", LastName = "Davis", Email = "emily.davis@eps.com", Phone = "555-0401", DateOfBirth = new DateTime(1992, 3, 18), Gender = "Female", Address = "321 Elm St", City = "New York", State = "NY", ZipCode = "10004", Country = "USA", DepartmentId = 1, DesignationId = 3, HireDate = new DateTime(2021, 2, 15), Status = EmployeeStatus.Active, Salary = 85000, ManagerId = 3, EmergencyContactName = "Tom Davis", EmergencyContactPhone = "555-0402" },
            new Employee { EmployeeId = "EMP-0005", FirstName = "David", LastName = "Wilson", Email = "david.wilson@eps.com", Phone = "555-0501", DateOfBirth = new DateTime(1993, 7, 25), Gender = "Male", Address = "654 Maple Dr", City = "New York", State = "NY", ZipCode = "10005", Country = "USA", DepartmentId = 1, DesignationId = 4, HireDate = new DateTime(2021, 9, 1), Status = EmployeeStatus.Active, Salary = 62000, ManagerId = 3, EmergencyContactName = "Amy Wilson", EmergencyContactPhone = "555-0502" },
            new Employee { EmployeeId = "EMP-0006", FirstName = "Jessica", LastName = "Martinez", Email = "jessica.martinez@eps.com", Phone = "555-0601", DateOfBirth = new DateTime(1991, 12, 8), Gender = "Female", Address = "987 Cedar Ln", City = "New York", State = "NY", ZipCode = "10006", Country = "USA", DepartmentId = 2, DesignationId = 5, HireDate = new DateTime(2021, 4, 20), Status = EmployeeStatus.Active, Salary = 55000, ManagerId = 2, EmergencyContactName = "Carlos Martinez", EmergencyContactPhone = "555-0602" },
            new Employee { EmployeeId = "EMP-0007", FirstName = "Robert", LastName = "Garcia", Email = "robert.garcia@eps.com", Phone = "555-0701", DateOfBirth = new DateTime(1989, 4, 14), Gender = "Male", Address = "147 Birch Ave", City = "New York", State = "NY", ZipCode = "10007", Country = "USA", DepartmentId = 3, DesignationId = 2, HireDate = new DateTime(2020, 8, 5), Status = EmployeeStatus.Active, Salary = 98000, ManagerId = 1, EmergencyContactName = "Maria Garcia", EmergencyContactPhone = "555-0702" },
            new Employee { EmployeeId = "EMP-0008", FirstName = "Amanda", LastName = "Rodriguez", Email = "amanda.rodriguez@eps.com", Phone = "555-0801", DateOfBirth = new DateTime(1994, 9, 30), Gender = "Female", Address = "258 Spruce St", City = "New York", State = "NY", ZipCode = "10008", Country = "USA", DepartmentId = 3, DesignationId = 6, HireDate = new DateTime(2022, 1, 10), Status = EmployeeStatus.Active, Salary = 58000, ManagerId = 7, EmergencyContactName = "Juan Rodriguez", EmergencyContactPhone = "555-0802" },
            new Employee { EmployeeId = "EMP-0009", FirstName = "Christopher", LastName = "Lee", Email = "christopher.lee@eps.com", Phone = "555-0901", DateOfBirth = new DateTime(1987, 6, 20), Gender = "Male", Address = "369 Willow Way", City = "New York", State = "NY", ZipCode = "10009", Country = "USA", DepartmentId = 4, DesignationId = 2, HireDate = new DateTime(2020, 11, 15), Status = EmployeeStatus.Active, Salary = 92000, ManagerId = 1, EmergencyContactName = "Jennifer Lee", EmergencyContactPhone = "555-0902" },
            new Employee { EmployeeId = "EMP-0010", FirstName = "Michelle", LastName = "Taylor", Email = "michelle.taylor@eps.com", Phone = "555-1001", DateOfBirth = new DateTime(1995, 2, 11), Gender = "Female", Address = "741 Poplar Pl", City = "New York", State = "NY", ZipCode = "10010", Country = "USA", DepartmentId = 4, DesignationId = 7, HireDate = new DateTime(2022, 5, 1), Status = EmployeeStatus.Active, Salary = 48000, ManagerId = 9, EmergencyContactName = "Brian Taylor", EmergencyContactPhone = "555-1002" }
        };

        await context.Employees.AddRangeAsync(employees);
        await context.SaveChangesAsync();

        // Seed Leave Requests
        var leaves = new List<Leave>
        {
            new Leave { EmployeeId = 4, LeaveType = LeaveType.Annual, StartDate = DateTime.Today.AddDays(5), EndDate = DateTime.Today.AddDays(7), Reason = "Family vacation", Status = LeaveStatus.Pending, CreatedAt = DateTime.UtcNow },
            new Leave { EmployeeId = 5, LeaveType = LeaveType.Sick, StartDate = DateTime.Today.AddDays(-2), EndDate = DateTime.Today.AddDays(-1), Reason = "Flu", Status = LeaveStatus.Approved, ApprovedBy = 3, ApprovedDate = DateTime.UtcNow.AddDays(-3), CreatedAt = DateTime.UtcNow.AddDays(-3) },
            new Leave { EmployeeId = 6, LeaveType = LeaveType.Casual, StartDate = DateTime.Today.AddDays(10), EndDate = DateTime.Today.AddDays(12), Reason = "Personal matters", Status = LeaveStatus.Pending, CreatedAt = DateTime.UtcNow },
            new Leave { EmployeeId = 8, LeaveType = LeaveType.Annual, StartDate = DateTime.Today.AddDays(15), EndDate = DateTime.Today.AddDays(20), Reason = "Summer vacation", Status = LeaveStatus.Approved, ApprovedBy = 7, ApprovedDate = DateTime.UtcNow.AddDays(-1), CreatedAt = DateTime.UtcNow.AddDays(-2) },
            new Leave { EmployeeId = 10, LeaveType = LeaveType.Sick, StartDate = DateTime.Today.AddDays(3), EndDate = DateTime.Today.AddDays(3), Reason = "Medical appointment", Status = LeaveStatus.Pending, CreatedAt = DateTime.UtcNow }
        };

        await context.Leaves.AddRangeAsync(leaves);
        await context.SaveChangesAsync();

        // Seed Attendance Records (Last 30 days for first 10 employees)
        var attendances = new List<Attendance>();
        var random = new Random();

        for (int day = 30; day >= 1; day--)
        {
            var date = DateTime.Today.AddDays(-day);

            // Skip weekends
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                continue;

            for (int empId = 1; empId <= 10; empId++)
            {
                var status = AttendanceStatus.Present;
                DateTime? checkInTime = date.AddHours(9).AddMinutes(random.Next(-15, 30));
                DateTime? checkOutTime = date.AddHours(18).AddMinutes(random.Next(-30, 30));

                // Randomly make some employees late or absent
                var chance = random.Next(100);
                if (chance < 5) // 5% absent
                {
                    status = AttendanceStatus.Absent;
                    checkInTime = null;
                    checkOutTime = null;
                }
                else if (chance < 15) // 10% late
                {
                    status = AttendanceStatus.Late;
                    checkInTime = date.AddHours(9).AddMinutes(random.Next(30, 90));
                }

                attendances.Add(new Attendance
                {
                    EmployeeId = empId,
                    Date = date,
                    CheckInTime = checkInTime,
                    CheckOutTime = checkOutTime,
                    Status = status,
                    CreatedAt = date
                });
            }
        }

        await context.Attendances.AddRangeAsync(attendances);
        await context.SaveChangesAsync();
    }
}