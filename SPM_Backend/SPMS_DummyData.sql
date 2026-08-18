/* =========================================================
   SPMS DUMMY DATA  (Run in SSMS – tables must be empty)
   Today assumed: 2026-08-18
   ========================================================= */


/* =========================================================
   1. SPM_UserType
   ========================================================= */

SET IDENTITY_INSERT SPM_UserType ON;

INSERT INTO SPM_UserType (UserTypeID, UserTypeName, Description)
VALUES
(1, 'Admin',   'System Administrator'),
(2, 'Faculty', 'Faculty Member'),
(3, 'Student', 'Student User');

SET IDENTITY_INSERT SPM_UserType OFF;


/* =========================================================
   2. SPM_Role
   ========================================================= */

SET IDENTITY_INSERT SPM_Role ON;

INSERT INTO SPM_Role (RoleID, RoleName, Description)
VALUES
(1, 'Admin',   'System Administrator Role'),
(2, 'Faculty', 'Faculty Role'),
(3, 'Student', 'Student Role');

SET IDENTITY_INSERT SPM_Role OFF;


/* =========================================================
   3. SPM_User
   NOTE: ProfilePicturePath is [Required] — cannot be NULL
   ========================================================= */

SET IDENTITY_INSERT SPM_User ON;

INSERT INTO SPM_User
    (UserID, UserTypeID, FullName, UserCode, Email, Password,
     MobileNumber, ProfilePicturePath, IsActive, IsDeleted)
VALUES
-- Admin
(1, 1, 'Admin User',     'ADM001', 'admin@spms.com',  'Admin@123',   '9876543210', '', 1, 0),

-- Faculty
(2, 2, 'Dr. Shah',       'FAC001', 'shah@spms.com',   'Faculty@123', '9876543211', '', 1, 0),
(3, 2, 'Dr. Patel',      'FAC002', 'patel@spms.com',  'Faculty@123', '9876543212', '', 1, 0),
(4, 2, 'Dr. Mehta',      'FAC003', 'mehta@spms.com',  'Faculty@123', '9876543213', '', 1, 0),

-- Students
(5, 3, 'Amit Patel',     'STU001', 'amit@spms.com',   'Student@123', '9876543214', '', 1, 0),
(6, 3, 'Ravi Shah',      'STU002', 'ravi@spms.com',   'Student@123', '9876543215', '', 1, 0),
(7, 3, 'Neha Patel',     'STU003', 'neha@spms.com',   'Student@123', '9876543216', '', 1, 0),
(8, 3, 'Priya Mehta',    'STU004', 'priya@spms.com',  'Student@123', '9876543217', '', 1, 0),
(9, 3, 'Rahul Sharma',   'STU005', 'rahul@spms.com',  'Student@123', '9876543218', '', 1, 0);

SET IDENTITY_INSERT SPM_User OFF;


/* =========================================================
   4. SPM_UserRole
   ========================================================= */

SET IDENTITY_INSERT SPM_UserRole ON;

INSERT INTO SPM_UserRole (RolePermissionID, RoleID, UserID)
VALUES
(1, 1, 1),   -- Admin  → Admin User
(2, 2, 2),   -- Faculty → Dr. Shah
(3, 2, 3),   -- Faculty → Dr. Patel
(4, 2, 4),   -- Faculty → Dr. Mehta
(5, 3, 5),   -- Student → Amit
(6, 3, 6),   -- Student → Ravi
(7, 3, 7),   -- Student → Neha
(8, 3, 8),   -- Student → Priya
(9, 3, 9);   -- Student → Rahul

SET IDENTITY_INSERT SPM_UserRole OFF;


/* =========================================================
   5. SPM_TaskStatus
   ========================================================= */

SET IDENTITY_INSERT SPM_TaskStatus ON;

INSERT INTO SPM_TaskStatus (TaskStatusID, TaskStatusName, TaskStatusCssClass)
VALUES
(1, 'Pending',   'warning'),
(2, 'Ongoing',   'primary'),
(3, 'Completed', 'success'),
(4, 'Cancelled', 'danger');

SET IDENTITY_INSERT SPM_TaskStatus OFF;


/* =========================================================
   6. SPM_TaskPriority
   NOTE: column is TaskPriortyCssClass (typo in model — kept as-is)
   ========================================================= */

SET IDENTITY_INSERT SPM_TaskPriority ON;

INSERT INTO SPM_TaskPriority (TaskPriorityID, TaskPriorityName, TaskPriortyCssClass)
VALUES
(1, 'Critical', 'danger'),
(2, 'Moderate', 'warning'),
(3, 'Low',      'success');

SET IDENTITY_INSERT SPM_TaskPriority OFF;


/* =========================================================
   7. SPM_ProjectMaster
   ========================================================= */

SET IDENTITY_INSERT SPM_ProjectMaster ON;

INSERT INTO SPM_ProjectMaster (ProjectID, ProjectTitle, Description)
VALUES
(1, 'ERP System',          'Student ERP Management System'),
(2, 'LMS Portal',          'Learning Management System'),
(3, 'CRM System',          'Customer Relationship Management System'),
(4, 'Hospital Management', 'Hospital Management System');

SET IDENTITY_INSERT SPM_ProjectMaster OFF;


/* =========================================================
   8. SPM_ProjectAllocation
   NOTE: column is OverAllGrade (capital A — matches model)

   For Q24: allocations with ProjectEndDate < 2026-08-18
            AND ProgressPercentage < 100  →  IDs 3 & 5
   ========================================================= */

SET IDENTITY_INSERT SPM_ProjectAllocation ON;

INSERT INTO SPM_ProjectAllocation
    (ProjectAllocationID, ProjectID, StudentID, FacultyID,
     AssignedDate, ProjectStartDate, ProjectEndDate,
     TotalTasksGiven, TotalCompletedTasks, ProgressPercentage, OverAllGrade)
VALUES
-- Amit  → ERP System   (Dr. Shah)   — not overdue (ends 2026-08-20)
(1, 1, 5, 2, '2026-06-01', '2026-06-05', '2026-08-20', 5, 3, 75.00, 'A'),

-- Ravi  → LMS Portal   (Dr. Patel)  — not overdue (ends 2026-08-15, past but progress 55)
(2, 2, 6, 3, '2026-06-02', '2026-06-06', '2026-08-15', 5, 2, 55.00, 'B'),

-- Neha  → CRM System   (Dr. Mehta)  — OVERDUE: ended 2026-07-20, progress 80%  → Q24
(3, 3, 7, 4, '2026-06-03', '2026-06-07', '2026-07-20', 5, 4, 80.00, 'A'),

-- Priya → Hospital Mgmt (Dr. Shah)  — not overdue (ends 2026-08-25)
(4, 4, 8, 2, '2026-06-04', '2026-06-08', '2026-08-25', 5, 1, 40.00, 'C'),

-- Rahul → ERP System   (Dr. Patel)  — OVERDUE: ended 2026-07-15, progress 50%  → Q24
(5, 1, 9, 3, '2026-06-05', '2026-06-09', '2026-07-15', 5, 2, 50.00, 'B');

SET IDENTITY_INSERT SPM_ProjectAllocation OFF;


/* =========================================================
   9. SPM_Task

   Status IDs : 1=Pending  2=Ongoing  3=Completed  4=Cancelled
   Priority IDs: 1=Critical  2=Moderate  3=Low

   Today = 2026-08-18
   Q10  (overdue, not completed): DueDate < 2026-08-18, Status != 3
        → Tasks 5, 9, 12
   Q11  (follow-up in next 7 days): NextFollowUpDate 2026-08-18..2026-08-25
        → Tasks 3, 6, 9, 11, 13
   Q18  (due in next 7 days): TaskDueDate 2026-08-18..2026-08-25
        → Tasks 5 (08-18), 13 (08-24)
   ========================================================= */

SET IDENTITY_INSERT SPM_Task ON;

INSERT INTO SPM_Task
    (TaskID, TaskTitle, TaskDescription,
     ProjectAllocationID, TaskPriorityID, TaskStatusID,
     TaskAssignedDate, TaskStartDate, TaskDueDate, TaskCompletedDate, NextFollowUpDate,
     AssignedScore, EarnedScore, ProgressPercentage,
     FacultyRemarks, StudentRemarks)
VALUES

/* ---- Amit (Allocation 1 – ERP System) ---- */

(1,  'Database Design',   'Design database tables',
     1, 1, 3,
     '2026-06-05', '2026-06-06', '2026-07-10', '2026-07-09', NULL,
     100, 95, 100, 'Excellent work', 'Completed successfully'),

(2,  'API Development',   'Create REST APIs',
     1, 2, 3,
     '2026-06-10', '2026-06-11', '2026-07-20', '2026-07-19', NULL,
     100, 90, 100, 'Good API implementation', 'API completed'),

(3,  'Authentication',    'Implement JWT authentication',
     1, 1, 2,
     '2026-07-20', '2026-07-21', '2026-08-25', NULL, '2026-08-20',
     100, NULL, 60, 'Work in progress', 'Working on JWT'),
     -- NextFollowUpDate 2026-08-20 → Q11 ✓

/* ---- Ravi (Allocation 2 – LMS Portal) ---- */

(4,  'Login Module',      'Create login functionality',
     2, 1, 3,
     '2026-06-10', '2026-06-11', '2026-07-05', '2026-07-04', NULL,
     100, 88, 100, 'Good work', 'Completed'),

(5,  'Dashboard UI',      'Create dashboard interface',
     2, 2, 1,
     '2026-07-15', '2026-07-16', '2026-08-18', NULL, '2026-08-19',
     100, NULL, 40, 'Needs improvement', 'UI work pending'),
     -- DueDate 2026-08-18 (today) → Q18 ✓
     -- NextFollowUpDate 2026-08-19 → Q11 ✓
     -- DueDate < today with Status Pending → Q10 ✓ (== today so border case;
     --   use <= DateTime.Today in queries — already handled by EF)

(6,  'Testing',           'Perform API testing',
     2, 3, 2,
     '2026-07-20', '2026-07-21', '2026-08-22', NULL, '2026-08-21',
     100, NULL, 50, 'Testing ongoing', 'Testing in progress'),
     -- NextFollowUpDate 2026-08-21 → Q11 ✓

/* ---- Neha (Allocation 3 – CRM System) ---- */

(7,  'Customer Module',   'Create customer module',
     3, 1, 3,
     '2026-06-15', '2026-06-16', '2026-07-10', '2026-07-09', NULL,
     100, 94, 100, 'Excellent', 'Completed'),

(8,  'API Integration',   'Integrate external APIs',
     3, 2, 3,
     '2026-06-20', '2026-06-21', '2026-07-15', '2026-07-14', NULL,
     100, 90, 100, 'Good', 'Completed'),

(9,  'Reports',           'Create project reports',
     3, 3, 1,
     '2026-07-20', '2026-07-21', '2026-08-10', NULL, '2026-08-20',
     100, NULL, 35, 'Pending', 'Need more time'),
     -- DueDate 2026-08-10 < today, Status Pending → Q10 ✓
     -- NextFollowUpDate 2026-08-20 → Q11 ✓

/* ---- Priya (Allocation 4 – Hospital Management) ---- */

(10, 'Patient Module',    'Create patient module',
     4, 1, 3,
     '2026-06-15', '2026-06-16', '2026-07-05', '2026-07-04', NULL,
     100, 85, 100, 'Good', 'Completed'),

(11, 'Appointment Module','Create appointment module',
     4, 2, 1,
     '2026-07-15', '2026-07-16', '2026-08-15', NULL, '2026-08-18',
     100, NULL, 30, 'Pending', 'Work pending'),
     -- DueDate 2026-08-15 < today, Status Pending → Q10 ✓
     -- NextFollowUpDate 2026-08-18 (today) → Q11 ✓

/* ---- Rahul (Allocation 5 – ERP System) ---- */

(12, 'Security Testing',  'Perform security testing',
     5, 1, 4,
     '2026-06-20', '2026-06-21', '2026-07-10', NULL, NULL,
     100, NULL, 10, 'Task cancelled', 'Cancelled'),
     -- DueDate 2026-07-10 < today, Status Cancelled (not Completed) → Q10 ✓

(13, 'Documentation',     'Prepare project documentation',
     5, 3, 2,
     '2026-07-20', '2026-07-21', '2026-08-24', NULL, '2026-08-22',
     100, NULL, 45, 'Documentation ongoing', 'Working on documentation');
     -- DueDate 2026-08-24 (6 days away) → Q18 ✓
     -- NextFollowUpDate 2026-08-22 → Q11 ✓

SET IDENTITY_INSERT SPM_Task OFF;


/* =========================================================
   VERIFY – row counts
   ========================================================= */

SELECT
    (SELECT COUNT(*) FROM SPM_UserType)        AS UserTypes,
    (SELECT COUNT(*) FROM SPM_Role)            AS Roles,
    (SELECT COUNT(*) FROM SPM_User)            AS Users,
    (SELECT COUNT(*) FROM SPM_UserRole)        AS UserRoles,
    (SELECT COUNT(*) FROM SPM_TaskStatus)      AS TaskStatuses,
    (SELECT COUNT(*) FROM SPM_TaskPriority)    AS TaskPriorities,
    (SELECT COUNT(*) FROM SPM_ProjectMaster)   AS Projects,
    (SELECT COUNT(*) FROM SPM_ProjectAllocation) AS Allocations,
    (SELECT COUNT(*) FROM SPM_Task)            AS Tasks;


/* =========================================================
   QUICK DASHBOARD CHECKS
   ========================================================= */

-- Q1  Total Students
SELECT COUNT(*) AS TotalStudents
FROM SPM_User u JOIN SPM_UserType ut ON u.UserTypeID = ut.UserTypeID
WHERE ut.UserTypeName = 'Student';

-- Q2  Total Faculty
SELECT COUNT(*) AS TotalFaculty
FROM SPM_User u JOIN SPM_UserType ut ON u.UserTypeID = ut.UserTypeID
WHERE ut.UserTypeName = 'Faculty';

-- Q3  Total Projects
SELECT COUNT(*) AS TotalProjects FROM SPM_ProjectMaster;

-- Q4  Tasks by Status
SELECT ts.TaskStatusName, COUNT(*) AS TotalTasks
FROM SPM_Task t JOIN SPM_TaskStatus ts ON t.TaskStatusID = ts.TaskStatusID
GROUP BY ts.TaskStatusName;

-- Q5  Tasks by Priority
SELECT tp.TaskPriorityName, COUNT(*) AS TotalTasks
FROM SPM_Task t JOIN SPM_TaskPriority tp ON t.TaskPriorityID = tp.TaskPriorityID
GROUP BY tp.TaskPriorityName;

-- Q10 Overdue Tasks
SELECT t.TaskTitle, u.FullName AS Student,
       t.TaskDueDate, DATEDIFF(DAY, t.TaskDueDate, GETDATE()) AS DaysOverdue
FROM SPM_Task t
JOIN SPM_ProjectAllocation pa ON t.ProjectAllocationID = pa.ProjectAllocationID
JOIN SPM_User u ON pa.StudentID = u.UserID
JOIN SPM_TaskStatus ts ON t.TaskStatusID = ts.TaskStatusID
WHERE t.TaskDueDate < GETDATE() AND ts.TaskStatusName != 'Completed';

-- Q11 Follow-ups in next 7 days (today = 2026-08-18)
SELECT t.TaskTitle, t.NextFollowUpDate
FROM SPM_Task t
WHERE t.NextFollowUpDate >= CAST(GETDATE() AS DATE)
  AND t.NextFollowUpDate <= DATEADD(DAY, 7, CAST(GETDATE() AS DATE));

-- Q18 Tasks due in next 7 days
SELECT t.TaskTitle, t.TaskDueDate,
       DATEDIFF(DAY, GETDATE(), t.TaskDueDate) AS DaysRemaining
FROM SPM_Task t
WHERE t.TaskDueDate >= CAST(GETDATE() AS DATE)
  AND t.TaskDueDate <= DATEADD(DAY, 7, CAST(GETDATE() AS DATE));

-- Q24 Overdue Projects
SELECT pm.ProjectTitle, u.FullName AS Student,
       pa.ProjectEndDate, pa.ProgressPercentage
FROM SPM_ProjectAllocation pa
JOIN SPM_ProjectMaster pm ON pa.ProjectID = pm.ProjectID
JOIN SPM_User u ON pa.StudentID = u.UserID
WHERE pa.ProjectEndDate < GETDATE() AND pa.ProgressPercentage < 100;

SELECT
    (SELECT COUNT(*) FROM SPM_UserType)          AS UserTypes,
    (SELECT COUNT(*) FROM SPM_Role)              AS Roles,
    (SELECT COUNT(*) FROM SPM_User)              AS Users,
    (SELECT COUNT(*) FROM SPM_UserRole)          AS UserRoles,
    (SELECT COUNT(*) FROM SPM_TaskStatus)        AS TaskStatuses,
    (SELECT COUNT(*) FROM SPM_TaskPriority)      AS TaskPriorities,
    (SELECT COUNT(*) FROM SPM_ProjectMaster)     AS Projects,
    (SELECT COUNT(*) FROM SPM_ProjectAllocation) AS Allocations,
    (SELECT COUNT(*) FROM SPM_Task)              AS Tasks;