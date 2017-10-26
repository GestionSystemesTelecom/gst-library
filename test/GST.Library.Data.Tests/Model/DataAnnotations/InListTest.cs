using GST.Library.Data.Model.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace GST.Library.Data.Tests.Model.DataAnnotations
{

    public class InListTest
    {
        [Fact]
        public void MustValidData()
        {
            InListAttribute il = new InListAttribute(new[] {
                "value 1",
                "value 2",
                "value 3"
            });

            Assert.True(il.IsValid("value 1"));
            Assert.True(il.IsValid("value 2"));
            Assert.True(il.IsValid("value 3"));

            Assert.False(il.IsValid("value 4"));
        }

        [Fact]
        public void MustFormatErrorMessage()
        {
            InListAttribute il = new InListAttribute(new[] {
                "value 1",
                "value 2",
                "value 3"
            });

            Assert.Equal("The field aFieldName is not an allowed value", il.FormatErrorMessage("aFieldName"));
        }

        [Fact]
        public void MustSucceedWithAnnotation()
        {
            InListObject ilo = new InListObject();
            ilo.someString = "mustRaisedError";

            ValidationContext context = new ValidationContext(ilo, null, null);
            var result = new List<ValidationResult>();

            Validator.TryValidateObject(ilo, context, result, true);
            Assert.Equal("The field someString is not an allowed value", result.FirstOrDefault().ErrorMessage);
        }
    }
}
