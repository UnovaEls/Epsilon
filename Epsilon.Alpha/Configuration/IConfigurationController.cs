using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epsilon.Alpha.Configuration
{
    internal interface IConfigurationController<T>
    {
        Task<T> ReadAsync();

        Task WriteAsync(T config);
    }
}
