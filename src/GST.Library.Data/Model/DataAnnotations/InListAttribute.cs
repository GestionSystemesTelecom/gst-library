using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;

namespace GST.Library.Data.Model.DataAnnotations
{
    /// <summary>
    /// Validate that the data is in the list
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class InListAttribute : ValidationAttribute
    {
        private IEnumerable<object> expectedList;

        /// <summary>
        /// List of things
        /// </summary>
        /// <param name="expectedList"></param>
        public InListAttribute(IEnumerable<object> expectedList) : base(() => "The field {0} is not an allowed value")
        {
            this.expectedList = expectedList;
        }

        /// <summary>
        /// Override of <see cref="ValidationAttribute.IsValid(object)"/>
        /// </summary>
        /// <remarks>This method returns <c>true</c> if the <paramref name="value"/> is null.
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            return this.expectedList.Any(el => el == value);
        }

        /// <summary>
        /// Override of <see cref="ValidationAttribute.FormatErrorMessage"/>
        /// </summary>
        /// <param name="name">The name to include in the formatted string</param>
        /// <returns>A localized string to describe the error</returns>
        /// <exception cref="InvalidOperationException"> is thrown if the current attribute is ill-formed.</exception>
        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentCulture, this.ErrorMessageString, name);
        }
    }
}
