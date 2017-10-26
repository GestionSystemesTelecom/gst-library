using System;
using System.Linq;
using System.Reflection;
using Xunit;
using GST.Library.Shared.Mock;
using GST.Library.Helper.Type;

namespace GST.Library.Helper.Tests.Type
{
    public class IsTypeHelperTest
    {
        [Fact]
        public void MustAssertString()
        {
            Assert.True("This Is A String".IsString());

            String isAString = "This Is A String";
            Assert.True(isAString.IsString());

            System.Type type = typeof(CheckType);
            PropertyInfo[] genericClassProperties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            Assert.True(genericClassProperties.First(c => c.Name == "isAString").IsString());
        }

        [Fact]
        public void MustAssertNumeric()
        {
            Assert.True(12.IsNumeric());

            int isAInt = 32132132;
            Assert.True(isAInt.IsNumeric());

            System.Type type = typeof(CheckType);
            PropertyInfo[] genericClassProperties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            Assert.True(genericClassProperties.First(c => c.Name == "isAInt").IsNumeric());
            Assert.True(genericClassProperties.First(c => c.Name == "isAInt16").IsNumeric());
            Assert.True(genericClassProperties.First(c => c.Name == "isAInt32").IsNumeric());
            Assert.True(genericClassProperties.First(c => c.Name == "isAInt64").IsNumeric());
            Assert.True(genericClassProperties.First(c => c.Name == "isADouble").IsNumeric());
            Assert.True(genericClassProperties.First(c => c.Name == "isAFloat").IsNumeric());
        }

        [Fact]
        public void MustAssertDateTime()
        {
            Assert.True((new DateTime()).IsDate());

            string isADate = "2017-01-20";
            Assert.True(isADate.IsDate());

            System.Type type = typeof(CheckType);
            PropertyInfo[] genericClassProperties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            Assert.True(genericClassProperties.First(c => c.Name == "isADateTime").IsDate());
        }
    }
}
