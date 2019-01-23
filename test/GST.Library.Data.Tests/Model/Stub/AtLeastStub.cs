using GST.Library.Data.Model.DataAnnotations;

namespace GST.Library.Data.Tests.Model.Stub
{
    [AtLeastOneProperty("ItemA", "ItemB", ErrorMessage = "You must supply at least one value of ItemA or ItemB")]
    [AtLeastOneProperty("ItemC", "ItemD")]
    public class AtLeastStub
    {
        public string ItemA { get; set; }
        public string ItemB { get; set; }
        public bool? ItemC { get; set; }
        public int? ItemD { get; set; }
    }
}
