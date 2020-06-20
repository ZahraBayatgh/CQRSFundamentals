using CSharpFunctionalExtensions;
using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using Ordering.API.Application.DTOs;
using Ordering.API.Application.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.API.Queries
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, Result<OrderQuery>>
    {
        private readonly ConnectionString _connectionString;

        public GetOrderByIdQueryHandler(ConnectionString connectionString)
        {
            _connectionString = connectionString;
        }


        public async Task<Result<OrderQuery>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {

            using (var connection = new SqlConnection(_connectionString.Value))
            {
                connection.Open();

                var result = await connection.QueryAsync<dynamic>(
@"select o.[Id] as ordernumber,o.OrderDate as date, o.Description as description,
o.Address_City as city, o.Address_Country as country, o.Address_Street as street, o.Address_ZipCode as zipcode,os.Name as status, oi.ProductName as productname, oi.Units as units, oi.UnitPrice as unitprice, oi.PictureUrl as pictureurl
       FROM ordering.Orders o
              LEFT JOIN ordering.Orderitems oi ON o.Id = oi.orderid 
    LEFT JOIN ordering.orderstatus os on o.OrderStatusId  = os.Id
           WHERE o.Id=@id"
                        , new { request.Id }
                    );

                if (result.AsList().Count == 0)
                    throw new KeyNotFoundException();

                return MapOrderItems(result);
            }
        }

        private Result<OrderQuery> MapOrderItems(dynamic result)
        {
            var order = new OrderQuery
            {
                Ordernumber = result[0].ordernumber,
                Date = result[0].date,
                Status = result[0].status,
                Description = result[0].description,
                Street = result[0].street,
                City = result[0].city,
                Zipcode = result[0].zipcode,
                Country = result[0].country,
                Orderitems = new List<Orderitem>(),
                Total = 0
            };

            foreach (dynamic item in result)
            {
                var orderitem = new Orderitem
                {
                    Productname = item.productname,
                    Units = item.units,
                    Unitprice = (double)item.unitprice,
                    Pictureurl = item.pictureurl
                };

                order.Total += item.units * item.unitprice;
                order.Orderitems.Add(orderitem);
            }

            return Result.Success(order);
        }

    }
}
