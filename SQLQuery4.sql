
SET XACT_ABORT ON;

BEGIN TRY
    BEGIN TRANSACTION;

    DECLARE @UserId nvarchar(450);

    SELECT @UserId = Id
    FROM dbo.AspNetUsers
    WHERE Email = 'mehak8106@gmail.com'
      AND EmailConfirmed = 1;

    IF @UserId IS NULL
        THROW 50001,
            'Confirmed test login not found.', 1;

    IF EXISTS (
        SELECT 1
        FROM dbo.Borrowers
        WHERE IdentityUserId = @UserId
    )
        THROW 50002,
            'This login already has a membership.', 1;

    IF EXISTS (
        SELECT 1
        FROM dbo.Borrowers
        WHERE MembershipNumber = 'MBR-0002'
           OR Email = 'mehak8106@gmail.com'
    )
        THROW 50003,
            'Test membership or borrower email already exists.', 1;

    INSERT INTO dbo.Borrowers
    (
        MembershipNumber,
        FullName,
        Email,
        Phone,
        Address,
        DateRegistered,
        IsActive,
        IdentityUserId
    )
    VALUES
    (
        'MBR-0002',
        'Kiosk Test Patron',
        'mehak8106@gmail.com',
        '0400000000',
        'Test Record - Darwin City',
        GETUTCDATE(),
        1,
        @UserId
    );

    COMMIT TRANSACTION;

    SELECT
        MembershipNumber,
        FullName,
        Email,
        IsActive,
        CASE
            WHEN IdentityUserId IS NOT NULL
            THEN 'Linked'
            ELSE 'Not linked'
        END AS AccountStatus
    FROM dbo.Borrowers
    WHERE MembershipNumber = 'MBR-0002';

END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;