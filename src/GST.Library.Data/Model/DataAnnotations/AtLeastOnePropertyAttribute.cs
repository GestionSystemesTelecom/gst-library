using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;

namespace GST.Library.Data.Model.DataAnnotations
{
    /// <summary>
    /// Validate that at least one of the properties is set
    /// </summary>
    /// <remarks>
    /// 
    /// Code source From https://stackoverflow.com/questions/2712511/data-annotations-for-validation-at-least-one-required-field/26424791#26424791
    /// 
    /// ```C#
    /// How to use it
    /// [AtLeastOneProperty("StringProp", "Id", ErrorMessage="You must supply at least one value")]
    /// [AtLeastOneProperty("BoolProp", "BoolPropNew", ErrorMessage = "You must supply at least one value")]
    /// public class SimpleTest
    /// {
    ///     public string StringProp { get; set; }
    ///     public int? Id { get; set; }
    ///     public bool? BoolProp { get; set; }
    ///     public bool? BoolPropNew { get; set; }
    /// }
    /// ```
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class AtLeastOnePropertyAttribute : ValidationAttribute
    {
        private string[] PropertyList { get; set; }

        /// <summary>
        /// AtLeastOnePropertyAttribute
        /// </summary>
        /// <param name="propertyList"></param>
        public AtLeastOnePropertyAttribute(params string[] propertyList) : base(() => "You must supply at least one value for these fields {0}")
        {
            this.PropertyList = propertyList;
        }

        private AtLeastOnePropertyAttribute() : base(() => "You must supply at least one value for these fields {0}")
        {
        }

        /// <summary>
        /// see http://stackoverflow.com/a/1365669
        /// </summary>
        public override object TypeId
        {
            get
            {
                return this;
            }
        }

        /// <inheritdoc />
        public override bool IsValid(object value)
        {
            PropertyInfo propertyInfo;
            foreach (string propertyName in PropertyList)
            {
                propertyInfo = value.GetType().GetProperty(propertyName);

                if (propertyInfo != null && propertyInfo.GetValue(value, null) != null)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Override of <see cref="ValidationAttribute.FormatErrorMessage"/>
        /// </summary>
        /// <param name="name">The name to include in the formatted string</param>
        /// <returns>A localized string to describe the error</returns>
        /// <exception cref="InvalidOperationException"> is thrown if the current attribute is ill-formed.</exception>
        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentCulture, this.ErrorMessageString, string.Join(", ", PropertyList));
        }
    }
}
