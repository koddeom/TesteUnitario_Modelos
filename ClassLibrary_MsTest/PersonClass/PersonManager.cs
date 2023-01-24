using System.Collections.Generic;

namespace ClassLibrary_MsTest.PersonClass
{
    public class PersonManager
    {
        public Person CreatePerson(string first, string last, bool isSupervisor)
        {
            Person ret = null;

            if (!string.IsNullOrEmpty(first))
            {
                if (isSupervisor)
                    ret = new Supervisor();
                else
                    ret = new Employee();

                ret.Firstname = first;
                ret.Lastname = last;
            }

            return ret;
        }

        public List<Person> GetPeople()
        {
            List<Person> people = new List<Person>();

            people.Add(new Person() { Firstname = "Vinicius", Lastname = "Andrade" });
            people.Add(new Person() { Firstname = "laura", Lastname = "Antonia" });
            people.Add(new Person() { Firstname = "Tiago", Lastname = "Paulo" });

            return people;
        }

        public List<Person> GetSupervisor()
        {
            List<Person> people = new List<Person>();

            people.Add(CreatePerson("Vinicius", "Andrade", true));
            people.Add(CreatePerson("laura", "Antonia", true));

            return people;
        }
    }
}