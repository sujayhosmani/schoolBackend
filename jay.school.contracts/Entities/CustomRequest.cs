using System;
using System.Collections.Generic;
using System.Text;

namespace jay.school.contracts.Entities
{
    public class CustomRequest<T>
    {
        public T Data { get; set; }
    }
}
