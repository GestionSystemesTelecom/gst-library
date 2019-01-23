using GST.Library.Data.Model.DataAnnotations;
using GST.Library.Data.Tests.Model.Stub;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace GST.Library.Data.Tests.Model.DataAnnotations
{

    public class AtLeastOnePropertyAttributeTest    
    {
        [Fact]
        public void FailValidate()
        {

            AtLeastOnePropertyAttribute al = new AtLeastOnePropertyAttribute();

            var stub = new AtLeastStub { };

            ValidationContext context = new ValidationContext(stub, null, null);
            var result = new List<ValidationResult>();

            Validator.TryValidateObject(stub, context, result, true);
            Assert.Equal("You must supply at least one value of ItemA or ItemB", result[0].ErrorMessage);
            Assert.Equal("You must supply at least one value for these fields ItemC, ItemD", result[1].ErrorMessage);
        }

        [Fact]
        public void MustValidate()
        {

            AtLeastOnePropertyAttribute al = new AtLeastOnePropertyAttribute();

            var stub = new AtLeastStub {
                ItemB = "321",
                ItemC = false
            };

            ValidationContext context = new ValidationContext(stub, null, null);
            var result = new List<ValidationResult>();

            Validator.TryValidateObject(stub, context, result, true);

            Assert.True(result.Count == 0);
        }
    }
}
