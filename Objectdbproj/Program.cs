using Db4objects.Db4o;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Db4objects.Db4o.Query;



namespace Objectdbproj
{
    class Program
    {
        public static void DrawMenu()
        {
            Console.WriteLine("Co chcesz zrobić?");
            Console.WriteLine("1. Lista kontaków");
            Console.WriteLine("2. Statystyki");
            Console.WriteLine("3. Dodaj kontakt");
            Console.WriteLine("4. Edytuj kontakt");
            Console.WriteLine("5. Skasuj kontakt");
            Console.WriteLine("6. Dodaj adres dla osoby");
            Console.WriteLine("7. Edytuj adres dla osoby");
            Console.WriteLine("8. Skasuj adres dla osoby");
            Console.WriteLine("9. Dodaj numer dla osoby");
            Console.WriteLine("10. Edytuj numer dla osoby");
            Console.WriteLine("11. Skasuj numer dla osoby");

        }

        public static void SwitchMenu(string decision)
        {
            switch (decision)
            {
                case "1":
                    DisplayContact();
                    break;
                case "2":
                    DisplayStats();
                    break;
                case "3":
                    AddPerson();
                    break;
                case "4":
                    EditPerson();
                    break;
                case "5":
                    DeletePerson();
                    break;
                case "6":
                    AddAddressForPerson();
                    break;
                case "7":
                    EditAddressForPerson();
                    break;
                case "8":
                    DeleteAddressForPerson();
                    break;
                case "9":
                    AddNumberForPerson();
                    break;
                case "10":
                    EditNumberForPerson();
                    break;
                case "11":
                    DeleteNumberForPerson();
                    break;
                default:
                    break;
            }
        }

        private static void DisplayContact()
        {
            using (IObjectContainer db = Db4oEmbedded.OpenFile("baza.txt"))
            {
                var querry1 = db.Query<Person>(typeof(Person));
                foreach (var itemPerson in querry1)
                {
                    Console.WriteLine("=================================");
                    Console.WriteLine(itemPerson.Name + " " + itemPerson.Lastname);
                    Console.WriteLine(" " + itemPerson.Adress.Street);
                    Console.WriteLine(" " + itemPerson.Adress.PostalCode + " " + itemPerson.Adress.City);
                    if (itemPerson.Contacts != null)
                    {
                        foreach (var itemPhone in itemPerson.Contacts)
                        {
                            if (itemPhone != null)
                            {
                                Console.WriteLine();
                                Console.WriteLine("  " + itemPhone.Number);
                                Console.WriteLine("  " + itemPhone.Provider + " " + itemPhone.Type);
                            }
                            if (itemPerson.Contacts == null)
                            { 
                                Console.WriteLine("ten kontakt nie ma numerów");
                            }
                     
                            
                        }
                    }
                    
                }
            }
        }

        private static void DisplayStats()
        {
            using (IObjectContainer db = Db4oEmbedded.OpenFile("baza.txt"))
            {
                var querry1 = db.Query<Person>(typeof(Person));
                Console.WriteLine("Osoby: " + querry1.Count);
                var querry2 = db.Query<Address>(typeof(Address));
                Console.WriteLine("Adresy: " + querry2.Count);
                var querry3 = db.Query<Phone>(typeof(Phone));
                Console.WriteLine("Telefony: " + querry3.Count);
            }
        }

        private static void AddPerson()
        {
            List<Phone> kontakty = new List<Phone>();
            Console.WriteLine("Podaj imie:");
            var imie = Console.ReadLine();
            Console.WriteLine("Podaj nazwisko:");
            var nazwisko = Console.ReadLine();
            Console.WriteLine("Czy chcesz dodać adres? [t/n]");
            var choice = Console.ReadLine();
            Address adres = null ;
            if (choice.ToLower() == "t")
            {
                Console.WriteLine("Podaj ulicę:");
                var ulica = Console.ReadLine();
                Console.WriteLine("Podaj kod pocztowy:");
                var kod = Console.ReadLine();
                Console.WriteLine("Podaj miasto:");
                var miasto = Console.ReadLine();
                adres = new Address { Street = ulica, PostalCode = kod, City = miasto };
            }
            Console.WriteLine("czy chcesz dodać telefon? [t/n]");
            choice = Console.ReadLine();
            while (choice.ToLower() == "t")
            {
                Console.WriteLine("Podaj numer:");
                var numer = Console.ReadLine();
                Console.WriteLine("Podaj operatora:");
                var operatr = Console.ReadLine();
                Console.WriteLine("Podaj rodzaj:");
                var rodzaj = Console.ReadLine();
                kontakty.Add(new Phone { Number = numer, Provider = operatr, Type = rodzaj });
                Console.WriteLine("czy chcesz dodać kolejny telefon? [t/n]");
                choice = Console.ReadLine();
            }
            Person osoba = new Person { Name = imie, Lastname = nazwisko, Adress = adres, Contacts = kontakty };
            using (IObjectContainer db = Db4oEmbedded.OpenFile("baza.txt"))
            {
                db.Store(osoba);
            }
        }

        private static void EditPerson()
        {
            Console.WriteLine("Podaj imie:");
            var imie = Console.ReadLine();
            Console.WriteLine("Podaj nazwisko:");
            var nazwisko = Console.ReadLine();
            using (IObjectContainer db = Db4oEmbedded.OpenFile("baza.txt"))
            {
                var querry = db.Query<Person>(typeof(Person));
                var result = querry.FirstOrDefault(x => x.Name == imie && x.Lastname == nazwisko);
                if (result != null)
                {
                    
                    Console.WriteLine("Podaj nowe imie");
                    var newName = Console.ReadLine();
                    Console.WriteLine("Podaj nowe nazwisko");
                    var newLastName = Console.ReadLine();
                    result.Name = newName;
                    result.Lastname = newLastName;
                    db.Store(result);
                }
            }
        }

        private static void DeletePerson()
        {
            Console.WriteLine("Podaj imie:");
            var imie = Console.ReadLine();
            Console.WriteLine("Podaj nazwisko:");
            var nazwisko = Console.ReadLine();
            using (IObjectContainer db = Db4oEmbedded.OpenFile("baza.txt"))
            {
                var querry = db.Query<Person>(typeof(Person));
                var result = querry.FirstOrDefault(x => x.Name == imie && x.Lastname == nazwisko);
                if (result != null)
                {
                    if (result.Adress != null)
                    {
                        db.Delete(result.Adress);
                    }
                    if (result.Contacts != null)
                    {
                        foreach (var item in result.Contacts)
                        {
                            db.Delete(item);
                        }
                    }
                    db.Delete(result);
                }
            }
        }

        private static void AddAddressForPerson()
        {
            Console.WriteLine("Podaj imie:");
            var imie = Console.ReadLine();
            Console.WriteLine("Podaj nazwisko:");
            var nazwisko = Console.ReadLine();
            using (IObjectContainer db = Db4oEmbedded.OpenFile("baza.txt"))
            {
                var querry = db.Query<Person>(typeof(Person));
                var result = querry.FirstOrDefault(x => x.Name == imie && x.Lastname == nazwisko);
                if (result != null)
                {
                    Address adres = null;
                    Console.WriteLine("Podaj ulicę:");
                    var ulica = Console.ReadLine();
                    Console.WriteLine("Podaj kod pocztowy:");
                    var kod = Console.ReadLine();
                    Console.WriteLine("Podaj miasto:");
                    var miasto = Console.ReadLine();
                    adres = new Address { Street = ulica, PostalCode = kod, City = miasto };
                    result.Adress = adres;
                    db.Store(result);
                }
            }
        }

        private static void EditAddressForPerson()
        {
            Console.WriteLine("Podaj imie:");
            var imie = Console.ReadLine();
            Console.WriteLine("Podaj nazwisko:");
            var nazwisko = Console.ReadLine();
            using (IObjectContainer db = Db4oEmbedded.OpenFile("baza.txt"))
            {
                var querry = db.Query<Person>(typeof(Person));
                var result = querry.FirstOrDefault(x => x.Name == imie && x.Lastname == nazwisko);
                if (result != null)
                {


                    Console.WriteLine("Podaj nową ulicę:");
                    var newStreet = Console.ReadLine();
                    Console.WriteLine("Podaj nowy kod pocztowy:");
                    var newCode = Console.ReadLine();
                    Console.WriteLine("Podaj nowe miasto:");
                    var newCity = Console.ReadLine();
                    result.Adress.Street = newStreet;
                    result.Adress.PostalCode = newCode;
                    result.Adress.City = newCity;
                    db.Store(result.Adress);
                }
            }
        }

        private static void DeleteAddressForPerson()
        {
            Console.WriteLine("Podaj imie:");
            var imie = Console.ReadLine();
            Console.WriteLine("Podaj nazwisko:");
            var nazwisko = Console.ReadLine();
            using (IObjectContainer db = Db4oEmbedded.OpenFile("baza.txt"))
            {
                var querry = db.Query<Person>(typeof(Person));
                var result = querry.FirstOrDefault(x => x.Name == imie && x.Lastname == nazwisko);
                if (result != null)
                {

                    if (result.Adress != null)
                    {
                        db.Delete(result.Adress);
                    }

                    db.Store(result);
                }
            }
        }

        private static void AddNumberForPerson()
        {
            Console.WriteLine("Podaj imie:");
            var imie = Console.ReadLine();
            Console.WriteLine("Podaj nazwisko:");
            var nazwisko = Console.ReadLine();
            using (IObjectContainer db = Db4oEmbedded.OpenFile("baza.txt"))
            {
                var querry = db.Query<Person>(typeof(Person));
                var result = querry.FirstOrDefault(x => x.Name == imie && x.Lastname == nazwisko);
                if (result != null)
                {
                    Phone phone = null;
                    List<Phone> kontakty = result.Contacts;
                    Console.WriteLine("Podaj numer:");
                    var numer = Console.ReadLine();
                    Console.WriteLine("Podaj operatora:");
                    var operatr = Console.ReadLine();
                    Console.WriteLine("Podaj rodzaj:");
                    var rodzaj = Console.ReadLine();
                    phone = (new Phone { Number = numer, Provider = operatr, Type = rodzaj });
                    kontakty.Add(new Phone { Number = numer, Provider = operatr, Type = rodzaj });
                    db.Store(result.Contacts);
                }
            }
        }

        private static void EditNumberForPerson()
        {
            Console.WriteLine("Podaj imie:");
            var imie = Console.ReadLine();
            Console.WriteLine("Podaj nazwisko:");
            var nazwisko = Console.ReadLine();
            using (IObjectContainer db = Db4oEmbedded.OpenFile("baza.txt"))
            {
                var querry = db.Query<Person>(typeof(Person));
                var result = querry.FirstOrDefault(x => x.Name == imie && x.Lastname == nazwisko);
                if (result != null)
                {


                    if (result != null)
                    {
                        if (result.Contacts != null)
                        {
                            Console.WriteLine("Który numer chcesz edytować?");
                            foreach (var item in result.Contacts)
                            {
                                if (item != null)
                                {

                                    Console.WriteLine(item.Number);
                                }
                                else
                                {
                                    Console.WriteLine("ten kontakt nie ma numerów");
                                }

                            }

                            if (result.Contacts != null)
                            {
                                var choice = Console.ReadLine().ToString();
                                var cos = db.QueryByExample(new Phone { Number = choice })[0] as Phone;
                                foreach (var item in result.Contacts)
                                {
                                    if (item == null)
                                    {
                                        db.Delete(result.Contacts);
                                    }
                                    else
                                    {
                                        if (choice == item.Number)
                                        {
                                            Console.WriteLine("Podaj nowy numer");
                                            var newNumber = Console.ReadLine().ToString();
                                            cos.Number = newNumber;
                                            //var query = result.Contacts.Select(x => { x.Number = choice; return x; });

                                            db.Store(cos);
                                        }
                                    }

                                }
                            }
                            else
                            {
                                Console.WriteLine("ten numer nie ma kontaków");
                            }
                        }
                    }
                }
            }
        }

        private static void DeleteNumberForPerson()
        {
            Console.WriteLine("Podaj imie:");
            var imie = Console.ReadLine();
            Console.WriteLine("Podaj nazwisko:");
            var nazwisko = Console.ReadLine();
            using (IObjectContainer db = Db4oEmbedded.OpenFile("baza.txt"))
            {
                var querry = db.Query<Person>(typeof(Person));
                var result = querry.FirstOrDefault(x => x.Name == imie && x.Lastname == nazwisko);
                if (result != null)
                {
                    if (result.Contacts != null)
                    {
                        Console.WriteLine("Który numer chcesz usunąć?");
                        foreach (var item in result.Contacts)
                        {
                            if (item != null)
                            {
                                
                                Console.WriteLine(item.Number);
                            }
                            else
                            {
                                Console.WriteLine("ten kontakt nie ma numerów");
                            }

                        }

                        if (result.Contacts != null)
                        {
                            var choice = Console.ReadLine().ToString();
                            foreach (var item in result.Contacts)
                            {
                                if (item == null)
                                {
                                    db.Delete(result.Contacts);
                                }
                                else
                                {
                                    if (choice == item.Number)
                                    {
                                        db.Delete(item);
                                    }
                                }

                            }
                        }
                        else
                        {
                            db.Delete(result.Contacts);
                            Console.WriteLine("ten numer nie ma kontaków");
                        }
                    }
                   
                    
                }
            }
        }
        static void Main(string[] args)
        {

            {
                bool work = true;
                while (work)
                {
                    DrawMenu();
                    string cos = Console.ReadLine();
                    SwitchMenu(cos);
                }

            }

        }
    }
}