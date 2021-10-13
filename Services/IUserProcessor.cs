namespace PSDIPortal
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using PSDIPortal.Models;

    public interface IUserProcessor
    {
        Task AddDefaultCurrentUserDetails();
        Task<bool> checkIsCurrentUserNew();

        Task<User> getUserDetails();
    }
}