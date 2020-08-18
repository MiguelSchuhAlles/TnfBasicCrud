using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tnf.Runtime.Session;
using TnfBasicCrud.Infra.Context;

namespace TnfBasicCrud.Infra.SqlServer.Context
{
    public class SqlServerTnfBasicCrudContext : TnfBasicCrudContext
    {
        public SqlServerTnfBasicCrudContext(DbContextOptions<TnfBasicCrudContext> options, ITnfSession session)
            : base(options, session)
        {
        }
    }
}
