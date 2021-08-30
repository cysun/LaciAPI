using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Laci.Models
{
    public enum ShowType
    {
        Silent = 0,
        MessageWarn = 1,
        MessageError = 2,
        Notification = 4,
        Page = 9
    }

    public enum ErrorType
    {
        InvalidRequest,
        NotFound,
        ClientError,
        ServerError
    }

    // The ErrorInfoStructure expected by @umijs/plugin-request. See more at
    // https://umijs.org/plugins/plugin-request
    public class ResponseStructure
    {
        public bool Success { get; set; } = true;

        public object Data { get; set; }

        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public ShowType? ShowType { get; set; }

        public string TraceId { get; set; }

        public string Host { get; set; }

        public ResponseStructure() { }

        public static ResponseStructure Result(object data = null)
        {
            return new ResponseStructure {
                Data = data
            };
        }

        public static ResponseStructure Error(ErrorType errorType)
        {
            return new ResponseStructure {
                Success = false,
                ErrorCode = errorType.ToString(),
                ErrorMessage = errorType.ToString(),
                ShowType = Models.ShowType.MessageWarn
            };
        }
    }
}
