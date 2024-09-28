using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Response
{
    public record ServicesResponse<T>(bool Flag, string Message, T Instance);
}
