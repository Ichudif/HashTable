using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashTable_SHA1_hopscotch
{
    class Student
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Class { get; set; }

        public Student(string name, string surname, string @class)
        {
            Name = name;
            Surname = surname;
            Class = @class;
        }

        public string GetIdentifier()
        {
            return Name + Surname + Class;
        }
    }
}
