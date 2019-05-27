alter PROC GetRevenueDaily
	@fromDate VARCHAR(10),
	@toDate VARCHAR(10)
AS
BEGIN
		  select
                CAST(b.DateCreated AS DATE) as Date,
				sum(bd.Quantity*p.OriginalPrice) as Fund,
                sum(bd.Quantity*bd.Price) as Revenue,
                sum((bd.Quantity*bd.Price)-(bd.Quantity * p.OriginalPrice)) as Profit
                from Bills b
                inner join dbo.BillDetails bd
                on b.Id = bd.BillId
                inner join Products p
                on bd.ProductId  = p.Id
                where b.DateCreated <= cast(dateadd(day, 1,@toDate) as date) 
				AND b.DateCreated >= cast(@fromDate as date)
				AND	b.BillStatus = 4
                group by CAST(b.DateCreated AS DATE)
END


EXEC dbo.GetRevenueDaily @fromDate = '2019/5/20',
                         @toDate = '2019/5/20' 