using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using GST.Library.Helper.Type;

namespace GST.Library.Data.Model.DataAnnotations
{
    /// <summary>
    /// Validate that the data is in the list
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class InListAttribute : ValidationAttribute
    {
        /// <summary>
        /// List of allowed string
        /// </summary>
        public string[] AllowedValue { get; private set; }

        /// <summary>
        /// List of things
        /// </summary>
        /// <param name="AllowedValue"></param>
        public InListAttribute(string[] AllowedValue) : this()
        {
            this.AllowedValue = AllowedValue;
        }

        private InListAttribute() : base(() => "The field {0} is not an allowed value")
        {
        }

        /// <summary>
        /// Override of <see cref="ValidationAttribute.IsValid(object)"/>
        /// </summary>
        /// <remarks>
        /// This method returns <c>true</c> if the <paramref name="value"/> is null.
        /// </remarks>
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            if (!value.IsString())
            {
                throw new InvalidCastException();
            }

            return this.AllowedValue.Any(el => el == value.ToString());
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
