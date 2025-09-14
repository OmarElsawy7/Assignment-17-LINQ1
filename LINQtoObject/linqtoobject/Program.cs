using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using static LINQtoObject.SampleData;

namespace LINQtoObject
{
    class Program
    {
        static void Main(string[] args)
        {
            //1 - Display book title and its ISBN.

            Console.WriteLine(" Q1 ");

            var res1 = Books.Select(x => new { x.Title, x.Isbn }).ToList();

            foreach (var x in res1)
                Console.WriteLine(x);

            //2- Display the first 3 books with price more than 25.

            Console.WriteLine(" Q2 ");

            var res2 = (from B in Books
                        where B.Price > 25M
                        select new {B.Title, B.Price} ).Take(3);

            foreach(var i in res2)
                Console.WriteLine(i);

            //3- Display Book title along with its publisher name. (Using 2 methods).


            Console.WriteLine(" Q3 ");

            //1-
            var res3 = from B in Books
                        select new { B.Title, B.Publisher };

            foreach(var i in res3)
                Console.WriteLine(i);
            //2-

            res3 = Books.Select(B => new { B.Title, B.Publisher });

            foreach (var i in res3)
                Console.WriteLine(i);

            //4 Find the number of books which cost more than 20.

            Console.WriteLine(" Q4 ");

            var res4 = (from B in Books
                        where B.Price > 40M
                        select B).Count();

           
                Console.WriteLine(res4);

            // 5 - Display book title, price and subject name sorted
            // by its subject name ascending and by its price descending.

            Console.WriteLine(" Q5 ");
            //   query
            var res5 = from B in Books
                       orderby B.Subject.Name ascending, B.Price descending
                       select new { B.Title, B.Price, B.Subject };
            //// operation
            res5 = Books.OrderBy(B => B.Subject.Name).ThenByDescending(B => B.Price)
               .Select(B => new { B.Title, B.Price, B.Subject});


            foreach (var i in res5)
                Console.WriteLine(i);



            //6 - Display All subjects with books related to this subject. (Using 2 methods).

            Console.WriteLine(" Q6 ");
            var res6 = from B in Books
                       group B by B.Subject into g
                       select new { Subject = g.Key.Name, Books = g };

            foreach (var s in res6)
            {
                Console.WriteLine($"Subject: {s.Subject}");
                foreach (var b1 in s.Books)
                    Console.WriteLine($"   {b1.Title}");
            }



            //7 - Try to display book title &price(from book objects)
            //returned from GetBooks Function.


            // casting
          
            var res7 = GetBooks().Cast<Book>().Select(B=> new { B.Title, B.Price });

            Console.WriteLine(" Q7 ");
            foreach(var s in res7)
                Console.WriteLine(s);



            //8 - Display books grouped by publisher &Subject.

            Console.WriteLine(" Q8 ");
            var res8 = from B in Books
                       group B by new { 
                           Publisher = B.Publisher.Name,
                           Subject = B.Subject.Name } 
                       into g
                       select new
                       {
                           g.Key.Publisher,
                           g.Key.Subject,
                           Books = g
                       };

            Console.WriteLine("------------------------------------------");


            foreach (var grp in res8)
            {
                Console.WriteLine($"Publisher: {grp.Publisher}, Subject: {grp.Subject}");
                foreach (var b in grp.Books)
                    Console.WriteLine($"   {b.Title} - {b.Price}");
            }


            Console.WriteLine(" Bouns ");


            Console.Write("Enter publisher name: ");
            string pubName = Console.ReadLine();

            Console.Write("Enter sorting criteria (Title / Price / Subject): ");
            string criteria = Console.ReadLine();

            Console.Write("Enter sorting way (ASC / DESC): ");
            string way = Console.ReadLine();

            FindBooksSorted(pubName, criteria, way);
        }

        static void FindBooksSorted(string publisherName, string criteria, string way)
        {
            
            var query = Books
          .Where(b => b.Publisher.Name.Equals(publisherName, StringComparison.OrdinalIgnoreCase));

           
            switch (criteria.ToLower())
            {
                case "title":
                    query = (way.ToUpper() == "DESC") ? query.OrderByDescending(b => b.Title)
                                                      : query.OrderBy(b => b.Title);
                    break;

                case "price":
                    query = (way.ToUpper() == "DESC") ? query.OrderByDescending(b => b.Price)
                                                      : query.OrderBy(b => b.Price);
                    break;

                case "subject":
                    query = (way.ToUpper() == "DESC") ? query.OrderByDescending(b => b.Subject.Name)
                                                      : query.OrderBy(b => b.Subject.Name);
                    break;

                default:
                    Console.WriteLine("Invalid sorting criteria. Using default (Title ASC).");
                    query = query.OrderBy(b => b.Title);
                    break;
            }

           
            foreach (var b in query)
            {
                Console.WriteLine($"{b.Title} | {b.Price} | {b.Subject.Name} | {b.Publisher.Name}");
            }










        }
    }
}
