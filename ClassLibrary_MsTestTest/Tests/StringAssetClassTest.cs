using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;

namespace ClassLibrary_MsTestTest
{
    [TestClass]
    public class StringAssetClassTest
    {
        [TestMethod]
        [Owner("Hal")]
        public void ContainsTest()
        {
            string str1 = "Vinicius Andrade";
            string str2 = "Andrade";

            StringAssert.Contains(str1, str2);
        }

        [TestMethod]
        [Owner("Hal")]
        public void StartsWithTest()
        {
            string str1 = "Todos Caixa Alta";
            string str2 = "Todos Caixa";

            StringAssert.StartsWith(str1, str2);
        }

        //-------------------------------------------------------
        // USO DE REGEX
        //-------------------------------------------------------
        [TestMethod]
        [Owner("Hal")]
        public void IsAllowerCaseTest()
        {
            Regex reg = new Regex(@"^([^A-z])+$");

            StringAssert.DoesNotMatch("Todos caixa", reg);
        }
    }
}