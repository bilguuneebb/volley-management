namespace VolleyManagement.Dal.MsSql
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// DAL user model
    /// </summary>
    [Table("Users")]
    public partial class User
    {
        /// <summary>
        /// Gets or sets id of user
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets login of user
        /// </summary>
        [Required]
        [StringLength(20)]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets password of user
        /// </summary>
        [Required]
        [StringLength(68)]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets full name of user
        /// </summary>
        [StringLength(60)]
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets telephone of user
        /// </summary>
        [StringLength(20)]
        public string CellPhone { get; set; }

        /// <summary>
        /// Gets or sets email of user
        /// </summary>
        [Required]
        [StringLength(80)]
        public string Email { get; set; }
    }
}