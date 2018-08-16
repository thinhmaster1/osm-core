using System;
using System.Collections.Generic;
using System.Text;

namespace OSM.Infrastructure.SharedKernel
{
    public abstract class DomainEntity<T>
    {
        public T Id { get; set; }
        public bool IsTransient() {
            return Id.Equals(default(T));
        }
    }
}
