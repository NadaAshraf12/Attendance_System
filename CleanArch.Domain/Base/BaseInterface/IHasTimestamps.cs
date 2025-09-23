using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.Domain.Base.BaseInterface
{
    public interface IHasTimestamps
    {
        string? CreatedBy { get; set; }
        DateTime CreatedOn { get; set; }
        string? UpdatedBy { get; set; }
        DateTime? UpdatedOn { get; set; }
    }
}
