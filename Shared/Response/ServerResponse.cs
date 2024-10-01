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
    public class ServerResponse<T>
    {
        public ResponseType Flag { get; set; } = ResponseType.Error;
        public string Message { get; set; } = string.Empty;
        public T? Instance { get; set; } = default;

        public ServerResponse(ResponseType flag, string message = "", T? instance = default)
        {
            Flag = flag;
            Message = message;
            Instance = instance;
        }
    }
}
