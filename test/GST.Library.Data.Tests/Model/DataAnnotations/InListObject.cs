using GST.Library.Data.Model.DataAnnotations;

namespace GST.Library.Data.Tests.Model.DataAnnotations
{
    public class InListObject
    {
        /// <summary>
        /// SomeString
        /// </summary>
        [InList(new[] { "value 1", "value 2", "value 3" })]
        public string someString { get; set; }
    }
}
