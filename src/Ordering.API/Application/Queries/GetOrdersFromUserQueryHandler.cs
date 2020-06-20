using CSharpFunctionalExtensions;
using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using Ordering.API.Application.DTOs;
using Ordering.API.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.API.Application.Queries
{
    public class GetOrdersFromUserQueryHandler : IRequestHandler<GetOrdersFromUserQuery, Result<IEnumerable<OrderSummary>>>
    {
        private readonly ConnectionString _connectionString;

        public GetOrdersFromUserQueryHandler(ConnectionString connectionString)
        {
            _connectionString = connectionString;
        }


        public async Task<Result<IEnumerable<OrderSummary>>> Handle(GetOrdersFromUserQuery request, CancellationToken cancellationToken)
        {
            using (var connection = new SqlConnection(_connectionString.Value))
            {
                connection.Open();

                var result= await connection.QueryAsync<OrderSummary>(@"SELECT o.[Id] as ordernumber,o.[OrderDate] as [date],os.[Name] as [status], SUM(oi.units*oi.unitprice) as total
                     FROM [ordering].[Orders] o
                     LEFT JOIN[ordering].[orderitems] oi ON  o.Id = oi.orderid 
                     LEFT JOIN[ordering].[orderstatus] os on o.OrderStatusId = os.Id                     
                     LEFT JOIN[ordering].[buyers] ob on o.BuyerId = ob.Id
                     WHERE ob.IdentityGuid = @userId
                     GROUP BY o.[Id], o.[OrderDate], os.[Name] 
                     ORDER BY o.[Id]", new { request.UserId });
                return Result.Success(result);
            }
        }
    }

}
