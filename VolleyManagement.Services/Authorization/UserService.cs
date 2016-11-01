﻿namespace VolleyManagement.Services.Authorization
{
    using System.Collections.Generic;
    using Contracts;
    using Data.Contracts;
    using Data.Queries.Common;
    using Domain.UsersAggregate;

    /// <summary>
    /// Provides the way to get specified information about user.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IQuery<User, FindByIdCriteria> _getUserByIdQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="getUserByIdQuery">Query for getting User by Id.</param>
        public UserService(IQuery<User, FindByIdCriteria> getUserByIdQuery)
        {
            this._getUserByIdQuery = getUserByIdQuery;
        }

        /// <summary>
        /// Gets User entity by Id.
        /// </summary>
        /// <param name="userId">User Id.</param>
        /// <returns>User entity.</returns>
        public User GetUser(int userId)
        {
            return this._getUserByIdQuery.Execute(
                    new FindByIdCriteria { Id = userId });
        }

        /// <summary>
        /// Gets list of users which role is Admin.
        /// </summary>
        /// <returns>List of User entities.</returns>
        public IList<User> GetAdminsList()
        {
            throw new System.NotImplementedException();
        }
    }
}