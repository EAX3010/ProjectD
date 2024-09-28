using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Shared.Response.Enums;

namespace Shared.Response
{
    public class Enums
    {
        public enum ResponseType
        {
            Success,
            Error,
            Warning,
            Info
        }
    }
    public record ServicesResponse<T>(ResponseType Flag, string Message, T Instance);
}
