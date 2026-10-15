
SELECT
    Id,
    BranchCode,
    Name,
    IsActive
FROM dbo.Branches;

SELECT
    Id,
    LibraryCode,
    Name,
    BranchId,
    Status
FROM dbo.Items
ORDER BY Id;