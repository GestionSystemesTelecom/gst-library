using System;
using System.Collections.Generic;
using Xunit;

namespace GST.Library.String.Tests
{
    public class Base64UrlTest
    {
        [Theory]
        [MemberData(nameof(Base64SafeForUrlData))]
        public void MustEncodeInBase64SafeForUrl(string expected, string texte)
        {
            byte[] encodedBytes = System.Text.Encoding.Unicode.GetBytes(texte);

            string txtBase64 = Base64Url.Encode(encodedBytes);

            Assert.Equal(expected, txtBase64);
        }

        [Theory]
        [MemberData(nameof(Base64SafeForUrlData))]
        public void MustDecodeInBase64SafeForUrl(string base64String, string expected)
        {
            byte[] encodedBytes = Base64Url.Decode(base64String);

            string txt = System.Text.Encoding.Unicode.GetString(encodedBytes);

            Assert.Equal(expected, txt);
        }

        [Theory]
        [MemberData(nameof(Base64SafeForUrlData))]
        public void MustMakeBase64StringSafeForUrl(string base64String, string texte)
        {

            byte[] encodedBytes = System.Text.Encoding.Unicode.GetBytes(texte);

            string txtBase64 = Convert.ToBase64String(encodedBytes);
            string txtBase64Url = Base64Url.SafeBase64StringForUrl(txtBase64);
            Assert.Equal(base64String, txtBase64Url);

            Assert.Equal(txtBase64, Base64Url.UnSafeBase64StringForUrl(txtBase64Url));
        }

        public static IEnumerable<object[]> Base64SafeForUrlData()
        {
            yield return new object[] { "dABlAHMAdAA", "test" };
            yield return new object[] { "TABlAHMAIABzAGEAbgBnAGwAbwB0AHMAIABsAG8AbgBnAHMAIABEAGUAcwAgAHYAaQBvAGwAbwBuAHMAIABEAGUAIABsACcAYQB1AHQAbwBtAG4AZQAgAEIAbABlAHMAcwBlAG4AdAAgAG0AbwBuACAAYwBvAGUAdQByACAARAAnAHUAbgBlACAAbABhAG4AZwB1AGUAdQByACAATQBvAG4AbwB0AG8AbgBlAC4A", "Les sanglots longs Des violons De l'automne Blessent mon coeur D'une langueur Monotone." };
            yield return new object[] { "TABlAHMAIABzAGEAbgBnAGwAbwB0AHMAIABsAG8AbgBnAHMADQAKAEQAZQBzACAAdgBpAG8AbABvAG4AcwANAAoARABlACAAbAAnAGEAdQB0AG8AbQBuAGUADQAKAEIAbABlAHMAcwBlAG4AdAAgAG0AbwBuACAAYwBvAGUAdQByAA0ACgBEACcAdQBuAGUAIABsAGEAbgBnAHUAZQB1AHIADQAKAE0AbwBuAG8AdABvAG4AZQAuAA0ACgANAAoAVABvAHUAdAAgAHMAdQBmAGYAbwBjAGEAbgB0AA0ACgBFAHQAIABiAGwA6gBtAGUALAAgAHEAdQBhAG4AZAANAAoAUwBvAG4AbgBlACAAbAAnAGgAZQB1AHIAZQAsAA0ACgBKAGUAIABtAGUAIABzAG8AdQB2AGkAZQBuAHMADQAKAEQAZQBzACAAagBvAHUAcgBzACAAYQBuAGMAaQBlAG4AcwANAAoARQB0ACAAagBlACAAcABsAGUAdQByAGUADQAKAA0ACgBFAHQAIABqAGUAIABtACcAZQBuACAAdgBhAGkAcwANAAoAQQB1ACAAdgBlAG4AdAAgAG0AYQB1AHYAYQBpAHMADQAKAFEAdQBpACAAbQAnAGUAbQBwAG8AcgB0AGUADQAKAEQAZQDnAOAALAAgAGQAZQBsAOAALAANAAoAUABhAHIAZQBpAGwAIADgACAAbABhAA0ACgBGAGUAdQBpAGwAbABlACAAbQBvAHIAdABlAC4A", "Les sanglots longs\r\nDes violons\r\nDe l'automne\r\nBlessent mon coeur\r\nD'une langueur\r\nMonotone.\r\n\r\nTout suffocant\r\nEt blême, quand\r\nSonne l'heure,\r\nJe me souviens\r\nDes jours anciens\r\nEt je pleure\r\n\r\nEt je m'en vais\r\nAu vent mauvais\r\nQui m'emporte\r\nDeçà, delà,\r\nPareil à la\r\nFeuille morte." };
        }

    }
}
