﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.Contributors
{
    public class GetAllContributors
    {
        public class Request : IRequest<List<ContributorDto>>
        {
            // no params
        }

        public class Handler : IRequestHandler<Request, List<ContributorDto>>
        {
            private readonly IQueryObject _query;

            public Handler(IQueryObject query)
            {
                _query = query;
            }

            public Task<List<ContributorDto>> Handle(Request request)
            {
                return _query.Execute(Unit.Value);
            }
        }

        public interface IQueryObject : IQuery<Unit, List<ContributorDto>>
        {

        }
    }
}
