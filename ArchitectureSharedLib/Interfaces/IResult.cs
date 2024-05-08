using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchitectureSharedLib.Interfaces
{
    public interface IResult<T>
    {
        public List<string> Messages { get; set; }

        public bool Succeeded { get; set; }

        public T Data { get; set; }

        //List<ValidationResult> ValidationErrors { get; set; }

        public int Code { get; set; }
    }
}
