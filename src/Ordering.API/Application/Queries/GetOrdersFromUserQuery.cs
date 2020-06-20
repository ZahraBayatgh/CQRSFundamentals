using CSharpFunctionalExtensions;
using MediatR;
using Ordering.API.Application.DTOs;
using System;
using System.Collections.Generic;

namespace Ordering.API.Application.Queries
{
    public class GetOrdersFromUserQuery : IRequest<Result<IEnumerable<OrderSummary>>>
    {
        public GetOrdersFromUserQuery(int userId)
        {
            this.UserId = userId;
        }

        public int UserId { get; }
    }
}
