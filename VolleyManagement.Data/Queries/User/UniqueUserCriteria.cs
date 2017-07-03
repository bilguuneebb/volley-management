﻿namespace VolleyManagement.Data.Queries.User
{
    using Contracts;

    /// <summary>
    /// Unique criteria to find users assigned to specific role
    /// </summary>
    public class UniqueUserCriteria : IQueryCriteria
    {
        /// <summary>
        /// Id of the role to search
        /// </summary>
        public int RoleId { get; set; }
    }
}
