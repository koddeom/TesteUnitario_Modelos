using System.Collections.Generic;

namespace ClassLibrary_MsTest.PersonClass
{
    public class Supervisor : Person
    {
        public List<Employee> Employees { get; set; }
    }
}