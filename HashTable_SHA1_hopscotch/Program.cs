using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HashTable_SHA1_hopscotch.StudentAdministration;

namespace HashTable_SHA1_hopscotch
{
    class Program
    {
        static void Main(string[] args)
        {
            StudentAdministration sa = new StudentAdministration(520, 10);

            StreamReader sr = new StreamReader("Schülerliste 1-5 Klasse.csv");
            sr.ReadLine();

            Stopwatch sw = new Stopwatch();
            while (!sr.EndOfStream)
            {
                sw.Stop();
                string zeile = sr.ReadLine();
                string[] splitted = zeile.Split(';');
                sw.Start();
                if (!sa.addstudent(new Student(splitted[1], splitted[2], splitted[0])))         //adding student and checking if he has been added correctly
                {
                    Console.WriteLine("Student nicht eingefügt: " + splitted[1] + " " + splitted[2]);       //Student was not inputted correctly, resize array or increase neighbourhood
                }
            }
            sw.Stop();
            Console.WriteLine("Time passed: " + sw.ElapsedMilliseconds + " Milliseconds");

            Console.WriteLine(sa.collisions);               //writing collisions to console
            int checkhelp = sa.checkHashtable();            //checking if all elements are on their position
            Console.WriteLine("Check: " + ((checkhelp == 0) ? "OK" : checkhelp + " Error" + ((checkhelp > 1) ? "" : "s") + " occured"));        //printing that to the console

            List<ArrayItem> students = new List<ArrayItem>();
            students.AddRange(sa.Students.Where(item => item.student != null));         //getting a list of the student array, with all valid students

            int[] collsionioncountperarrayposition = new int[5];                        //creating array for the collsion counting per array index

            foreach (ArrayItem item in students)
            {
                collsionioncountperarrayposition[(item.collisioncount <= 3) ? item.collisioncount : 4]++;   //filling that array
            }

            int count = 0;
            collsionioncountperarrayposition.ToList().ForEach(item => Console.WriteLine(((count != 4) ? count++ + "" : "n") + " Collisions: " + item));       //print it to the console

            //int count = 0;
            //foreach (Tuple<Student,int> item in students)
            //{
            //    if(sa.lookupStudent(item.Item1.GetIdentifier()).GetIdentifier() == item.Item1.GetIdentifier())
            //    {
            //        count++;
            //    }
            //}
            //Console.WriteLine(count);

            Console.ReadKey();
        }
    }
}
