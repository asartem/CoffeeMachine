using System;
using Cm.Domain.Users;
using Newtonsoft.Json;

namespace Cm.Api.Api.Users.Models
{
    /// <summary>
    /// Product for coffee machine
    /// </summary>
    public class UserDto : UserDtoBase
    {
        /// <summary>
        /// Id of the product
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the user
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Creates instance of the dto
        /// </summary>
        [JsonConstructor]
        protected UserDto() { }

        /// <summary>
        /// Converts entity to DTO
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public UserDto(User entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Id = entity.Id;
            UserName = entity.Name;
            Id = entity.Id;
            Deposit = entity.Deposit;
        }
    }
}
