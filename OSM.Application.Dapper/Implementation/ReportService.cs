using Dapper;
using Microsoft.Extensions.Configuration;
using OSM.Application.Dapper.Interface;
using OSM.Application.Dapper.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace OSM.Application.Dapper.Implementation
{
    public class ReportService : IReportService
    {
        private readonly IConfiguration _configuration;

        public ReportService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task AutoCompleteBillAsync()
        {
            using (var sqlConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await sqlConnection.OpenAsync();
                try
                {
                    SqlCommand sqlCommand = new SqlCommand("CompleteBill", sqlConnection);
                    await sqlCommand.ExecuteNonQueryAsync();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public async Task DeActiveProductAsync()
        {
            using (var sqlConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await sqlConnection.OpenAsync();
                try
                {
                    SqlCommand sqlCommand = new SqlCommand("SwitchStatus", sqlConnection);
                    await sqlCommand.ExecuteNonQueryAsync();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public async Task<IEnumerable<BillViewModel>> GetBillAsync()
        {
            using (var sqlConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await sqlConnection.OpenAsync();
                try
                {
                    return await sqlConnection.QueryAsync<BillViewModel>("GetBill", CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public async Task<IEnumerable<RevenueReportViewModel>> GetReportAsync(string fromDate, string toDate)
        {
            using (var sqlConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await sqlConnection.OpenAsync();
                var dynamicParameters = new DynamicParameters();
                var now = DateTime.Now;

                var firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                dynamicParameters.Add("@fromDate", string.IsNullOrEmpty(fromDate) ? firstDayOfMonth.ToString("yyyy/MM/dd") : fromDate);
                dynamicParameters.Add("@toDate", string.IsNullOrEmpty(toDate) ? lastDayOfMonth.ToString("yyyy/MM/dd") : toDate);

                try
                {
                    return await sqlConnection.QueryAsync<RevenueReportViewModel>(
                        "GetRevenueDaily", dynamicParameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}
