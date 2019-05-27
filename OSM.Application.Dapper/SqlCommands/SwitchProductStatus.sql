alter PROC SwitchStatus
AS
BEGIN
update Products 
set Status = 0
from(
 select
 p.Id as Id,
 SUM(pq.Quantity) as Quantity
 from ProductQuantities pq
 inner join Products p
 on p.Id = pq.ProductId
 group by p.Id) PQ
where 
Products.Id = PQ.Id
and
PQ.Quantity = 0
END

EXEC dbo.SwitchStatus
