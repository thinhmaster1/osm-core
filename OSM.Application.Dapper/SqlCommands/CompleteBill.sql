alter PROC CompleteBill
AS
BEGIN
UPDATE Bills
SET Status = 0
WHERE
BillStatus = 4 and GETDATE() - cast(DateModified as datetime) >= 2 and Status = 1
END

EXEC dbo.CompleteBill
