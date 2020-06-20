using CSharpFunctionalExtensions;
using MediatR;
using Ordering.API.Application.DTOs;

namespace Ordering.API.Queries
{
   public class GetOrderByIdQuery : IRequest<Result<OrderQuery>>
    { 
        public GetOrderByIdQuery(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}
