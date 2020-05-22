using Microsoft.EntityFrameworkCore;

namespace EHealth.Api.Contacts.Infrastructure.Repositories
{
    public class BaseRepository
    {

        protected readonly DbContext _apDbContext;

        public BaseRepository(DbContext apDbContext)
        {
            _apDbContext = apDbContext;
        }

        

    }
}