using ClassLibrary_MsTest;
using ClassLibrary_MsTest.PersonClass;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClassLibrary_MsTestTest
{
    [TestClass]
    public class AssertClasTest
    {
        #region "AreEqual/AreNotEqual"

        [TestMethod]
        [Owner("Hal")]
        public void AreEqualTest()
        {
            string str1 = "Vinicius";
            string str2 = "Vinicius";

            Assert.AreEqual(str1, str2);
        }

        //-----------------------------------------------------------------------
        // Espera uma exception o que vai ocasionar um teste ok. pelas definicoes
        //------------------------------------------------------------------------

        [TestMethod]
        [Owner("Hal")]
        [ExpectedException(typeof(AssertFailedException))]
        public void AreEqualTestCaseSensitive()
        {
            string str1 = "Vinicius";
            string str2 = "vinicius";

            //True: Case sensitive comparison
            Assert.AreEqual(str1, str2, false);
        }

        [TestMethod]
        [Owner("Hal")]
        public void AreNotEqualTest()
        {
            string str1 = "Vinicius";
            string str2 = "Thiago";

            Assert.AreNotEqual(str1, str2);
        }

        #endregion "AreEqual/AreNotEqual"

        #region "AreSAme/AreNotSame"

        [TestMethod]
        [Owner("Hal")]
        public void AreSameTest()
        {
            FileProcess x = new FileProcess();
            FileProcess y = x;

            Assert.AreEqual(x, y);
        }

        [TestMethod]
        [Owner("Hal")]
        public void AreNotSameTest()
        {
            FileProcess x = new FileProcess();
            FileProcess y = new FileProcess();

            Assert.AreNotEqual(x, y);
        }

        #endregion "AreSAme/AreNotSame"

        #region "IsInstanceOfType"

        [TestMethod]
        [Owner("Hal")]
        [Description("IsInstanceTypeOfTest")]
        public void IsInstanceTypeOfTest()
        {
            PersonManager personManager = new PersonManager();

            Person person;

            person = personManager.CreatePerson("Vinicius", "Andrade", true);

            Assert.IsInstanceOfType(person, typeof(Supervisor));
        }

        [TestMethod]
        [Owner("Hal")]
        [Description("IsNullObjectTest")]
        public void IsNullObjectTest()
        {
            PersonManager personManager = new PersonManager();

            Person person;

            person = personManager.CreatePerson("", "Andrade", true);

            Assert.IsNotNull(person);
        }

        #endregion "IsInstanceOfType"
    }
}