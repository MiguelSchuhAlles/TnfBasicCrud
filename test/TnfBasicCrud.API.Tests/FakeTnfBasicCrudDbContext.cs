using TnfBasicCrud.Infra.Context;
using Microsoft.EntityFrameworkCore;
using Tnf.Runtime.Session;

namespace TnfBasicCrud.API.Tests
{
    public class FakeCrudDbContext : TnfBasicCrudContext
    {
        public FakeCrudDbContext(DbContextOptions<TnfBasicCrudContext> options, ITnfSession session)
            : base(options, session)
        {
        }
    }
}
