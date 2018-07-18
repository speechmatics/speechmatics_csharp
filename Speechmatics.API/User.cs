using System;

namespace Speechmatics.API
{
    /// <summary>
    /// Class representing a user of the speechmatics API
    /// </summary>
    public class User
    {
        /// <summary>
        /// Constructor from API response
        /// </summary>
        /// <param name="id">Unique user Id</param>
        /// <param name="email">User's email address</param>
        /// <param name="balance">User's remaining balance (in pence)</param>
        public User(int id, string email, int balance)
        {
            Id = id;
            Email = email;
            Balance = balance;                
        }

        /// <summary>
        /// Unique user Id
        /// </summary>
        public int Id { get; }
        /// <summary>
        /// User's email address
        /// </summary>
        public string Email { get; }
        /// <summary>
        /// User's remaining balance (in pence)
        /// </summary>
        public int Balance { get; }
    }
}
