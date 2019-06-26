using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace HashTable_SHA1_hopscotch
{
    class StudentAdministration
    {
        public ArrayItem[] Students { get; set; }
        private SHA1 sha = new SHA1CryptoServiceProvider();
        public int neighbourhood { get; set; }
        public int collisions { get; set; }
        public int entryNr { get; set; }

        public struct ArrayItem
        {
            public Student student;
            public int actualbucket;
            public int collisioncount;
        }

        public StudentAdministration(int EntryNr, int H)
        {
            entryNr = EntryNr;
            neighbourhood = H;
            Students = new ArrayItem[EntryNr];
            collisions = 0;
        }

        public bool addstudent(Student st)
        {
            int key = getkey(st.GetIdentifier());

            if (Students[key].student == null)      //no collision
            {
                ArrayItem help = new ArrayItem();
                help.student = st;
                help.actualbucket = key;
                help.collisioncount = 0;
                Students[key] = help;
                return true;
            }
            else
            {                               //collision
                collisions++;
                Students[key].collisioncount++;
                int actualkey = key;
                do
                {
                    key++;
                } while (key < Students.Length - 1 && Students[key].student != null);       //getting next empty bucket

                if (key <= actualkey + neighbourhood - 1)   //looking if next empty bucket is in neighbourhood
                {
                    if (key >= Students.Length)
                    {
                        return false;
                        //Array has to be resized, due to the last positions being occupied
                    }

                    ArrayItem help = new ArrayItem();
                    help.student = st;
                    help.actualbucket = actualkey;
                    help.collisioncount = 0;

                    Students[key] = help;

                    return true;
                }
                else
                {                           //he is not
                    int nextemptybucket = key;
                    while (true)  //swapping until empty bucket is in neighbourhood
                    {
                        nextemptybucket = swap(nextemptybucket);
                        if (nextemptybucket == int.MaxValue)        //swap method returns int.MaxValue if something went wrong
                        {
                            return false;
                        }

                        if (nextemptybucket <= actualkey + neighbourhood - 1)       //greak if swapping has finished and the bucket is finally in the neighbourhood
                        {
                            break;
                        }
                    }

                    ArrayItem help = new ArrayItem();
                    help.student = st;
                    help.actualbucket = actualkey;
                    help.collisioncount = 0;

                    Students[nextemptybucket] = help;
                    return true;
                }
            }
        }

        public Student lookupStudent(String Identifier)
        {
            int key = getkey(Identifier);       //getting initial key

            for (int i = key; i < key + neighbourhood; i++)     //look for that student in the neighbourhood
            {
                if (Students[i].student != null && Students[i].student.GetIdentifier() == Identifier)
                {
                    return Students[i].student;
                }
            }

            return null;
        }

        public int checkHashtable()
        {
            int count = 0;
            for (int i = 0; i < Students.Length; i++)       //going through every student to check the hashtable
            {
                if (Students[i].student != null && i > Students[i].actualbucket + neighbourhood - 1)
                {
                    count++;
                    Console.WriteLine(Students[i].actualbucket + " " + i);      //printing students whose bucjets are not in their neighbourhood
                }
            }
            return count;
        }







        private int swap(int pointtoswapfrom)
        {
            int[] candidates = new int[neighbourhood - 1];

            int count = 0;
            for (int i = pointtoswapfrom - 1; i > pointtoswapfrom - neighbourhood; i--)  //getting swapcandidates
            {
                if (i < 0)
                    break;
                if (Students[i].student == null)
                {
                    return i;
                }
                candidates[count] = i;
                count++;
            }

            bool all = true;
            for (int i = 0; i < candidates.Length; i++)
            {
                if (pointtoswapfrom >= Students[candidates[i]].actualbucket + neighbourhood - 1)
                {
                    all = false;
                    break;
                    //check if there are any candidates who are allowed to swap
                    //if not, return and resize array or increase neighbourhood
                }
            }
            if (all)
                return int.MaxValue;




            candidates = candidates.Reverse().ToArray();




            for (int i = 0; i < candidates.Length; i++) //finding first candidate who is allowed to swap
            {
                if (pointtoswapfrom <= Students[candidates[i]].actualbucket + neighbourhood - 1)
                {
                    Students[pointtoswapfrom] = Students[candidates[i]];        //found him
                    Students[candidates[i]].student = null;                             //making the bucket empty
                    return candidates[i];
                }
            }
            return int.MaxValue;
        }






        private int getkey(string data)
        {
            byte[] result = sha.ComputeHash(Encoding.ASCII.GetBytes(data));

            return Math.Abs(int.Parse((new BigInteger(result) % Students.Length).ToString()));
        }
    }
}
