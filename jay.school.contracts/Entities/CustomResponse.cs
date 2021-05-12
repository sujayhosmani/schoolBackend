using System;
using System.Collections.Generic;
using System.Text;

namespace jay.school.contracts.Entities
{
    public class CustomResponse<T>
    {
        public int Status { get; set; }

        public T Data { get; set; }

        public string Error { get; set; }

        //public List<ErrorModel> Error { get; set;  }

        public CustomResponse(int Status, T Data, string Error) {
            
            this.Status = Status;

            this.Data = Data;

            this.Error = Error;
        
        }

    }

    public class ErrorModel 
    {
        public string ErrMsg { get; set; }
    }
}
