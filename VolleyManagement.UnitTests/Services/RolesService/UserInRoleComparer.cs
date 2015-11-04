﻿namespace VolleyManagement.UnitTests.Services.RolesService
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using VolleyManagement.Domain.Dto;

    /// <summary>
    /// Compares Role instances
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class UserInRoleComparer : IComparer<UserInRoleDto>, IComparer
    {
        public int Compare(UserInRoleDto x, UserInRoleDto y)
        {
            if (x == null && y == null)
            {
                return 0;
            }

            if (x == null)
            {
                return -1;
            }

            if (y == null)
            {
                return 1;
            }

            return this.CompareInternal(x, y);
        }

        public int Compare(object x, object y)
        {
            return this.Compare(x as UserInRoleDto, y as UserInRoleDto);
        }

        private int CompareInternal(UserInRoleDto x, UserInRoleDto y)
        {
            var result = y.UserId - x.UserId;
            if (result != 0)
            {
                return result;
            }

            result = string.CompareOrdinal(x.UserName, y.UserName);
            if (result != 0)
            {
                return result;
            }

            result = y.RoleIds.Count - x.RoleIds.Count;
            if (result != 0)
            {
                return result;
            }

            for (int i = 0; i < x.RoleIds.Count; i++)
            {
                result = y.RoleIds[i] - x.RoleIds[i];
                if (result != 0)
                {
                    break;
                }
            }

            return result;
        }
    }
}