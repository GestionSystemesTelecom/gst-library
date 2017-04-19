using GST.Library.Data.Model.DataAnnotations;
using System.Collections.Generic;

namespace GST.Library.Data.Tests.Model.DataAnnotations
{
    public class InListObject
    {
        [InList(new[] { "value 1", "value 2", "value 3" })]
        public string someString { get; set; }
    }
}
