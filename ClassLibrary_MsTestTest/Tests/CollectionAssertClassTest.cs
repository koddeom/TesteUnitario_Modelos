using ClassLibrary_MsTest.PersonClass;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ClassLibrary_MsTestTest.Tests
{
    //===================================================================================
    // Teste com coleção
    //===================================================================================
    [TestClass]
    public class CollectionAssertClassTest
    {
        [TestMethod]
        [Owner("Hal")]
        public void AreCollectionEqualFailsBecauseNoCompreTest()
        {
            PersonManager personManager = new PersonManager();

            List<Person> peopleExpected = new List<Person>();

            List<Person> peopleActual = new List<Person>();

            peopleExpected.Add(new Person() { Firstname = "Vinicius", Lastname = "Andrade" });
            peopleExpected.Add(new Person() { Firstname = "laura", Lastname = "Antonia" });
            peopleExpected.Add(new Person() { Firstname = "Tiago", Lastname = "Paulo" });

            //you shall not pass It´s not the same object
            //peopleActual = personManager.GetPeople();

            //It´s pass!
            peopleActual = peopleExpected;

            CollectionAssert.AreEqual(peopleExpected, peopleActual);
        }

        [TestMethod]
        [Owner("Hal")]
        public void AreCollectionEqualWithCompreTest()
        {
            PersonManager personManager = new PersonManager();

            List<Person> peopleExpected = new List<Person>();

            List<Person> peopleActual = new List<Person>();

            peopleExpected.Add(new Person() { Firstname = "Vinicius", Lastname = "Andrade" });
            peopleExpected.Add(new Person() { Firstname = "laura", Lastname = "Antonia" });
            peopleExpected.Add(new Person() { Firstname = "Tiago", Lastname = "Paulo" });

            peopleActual = personManager.GetPeople();

            //comparacao item a item, então apesar de serem objetos diferente ele valida.

            CollectionAssert.AreEqual(peopleExpected, peopleActual,
                Comparer<Person>.Create((x, y) => x.Firstname == y.Firstname
                                             && x.Lastname == y.Lastname ? 0 : 1));
        }

        [TestMethod]
        [Owner("Hal")]
        public void AreCollectionEquivalentTest()
        {
            PersonManager personManager = new PersonManager();

            List<Person> peopleExpected = new List<Person>();
            List<Person> peopleActual = new List<Person>();

            peopleActual = personManager.GetPeople();

            //Ordem diferente
            peopleExpected.Add(peopleActual[1]);
            peopleExpected.Add(peopleActual[2]);
            peopleExpected.Add(peopleActual[0]);

            //Com  Equivalent ele não diferencia a Ordem
            CollectionAssert.AreEquivalent(peopleExpected, peopleActual);
        }

        [TestMethod]
        [Owner("Hal")]
        public void IsCollectionTypeTest()
        {
            PersonManager personManager = new PersonManager();
            List<Person> peopleActual = new List<Person>();

            peopleActual = personManager.GetSupervisor();

            CollectionAssert.AllItemsAreInstancesOfType(peopleActual, typeof(Supervisor));
        }
    }
}