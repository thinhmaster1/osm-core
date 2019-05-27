alter PROC GetBill
AS
BEGIN
		  SELECT
          COUNT(Id) AS Bill,
		  BillStatus 
          FROM Bills
		  GROUP BY BillStatus
END


EXEC dbo.GetBill


