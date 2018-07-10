using System;
using System.Collections.Generic;
using System.Text;

namespace OSM.Infrastructure.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
    }
}
