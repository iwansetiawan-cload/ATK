using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INVENTORYWeb.Utility
{
    public class FormDto<T>
    {
        public bool Success { get; set; }

        public T? Data { get; set; }

        public ErrorMessage? Errormessage { get; set; }

        public FormDto()
        {
        }

        public FormDto(T data)
        {
            Data = data;
            Success = true;
        }

        public FormDto(string errorcode, string errordescription)
        {
            Success = false;
            Errormessage = new ErrorMessage { errorcode = errorcode, errordescription = errordescription };
        }

        public FormDto(bool success, string errorcode, string errordescription)
        {
            Success = true;
            Errormessage = new ErrorMessage { errorcode = errorcode, errordescription = errordescription };
        }

        public FormDto(string errorcode, string errordescription, T data)
        {
            Data = data;
            Success = false;
            Errormessage = new ErrorMessage { errorcode = errorcode, errordescription = errordescription };
        }
    }
}
