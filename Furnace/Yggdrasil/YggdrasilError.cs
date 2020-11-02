using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furnace.Yggdrasil
{
    class YggdrasilError : Exception
    {

        public Furnace.Yggdrasil.Json.Error.YggdrasilError ErrorResponse;
        public override string Message { get; }

        public YggdrasilError(Furnace.Yggdrasil.Json.Error.YggdrasilError errorResponse)
        {
            ErrorResponse = errorResponse;
            Message = $"Response {{ Cause: {errorResponse.Cause ?? "Unspecified"}; Error: {errorResponse.Error}; Message: {errorResponse.ErrorMessage}; }}";
        }


    }
}
