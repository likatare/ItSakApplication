using Repository;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ITSakApp
{
    class Program
    {
        private static readonly Random random = new Random();

        static void Main(string[] args)
        {
            Console.WriteLine("This is itsäk test app!");



            while (true)
            {
                bool exit = false;

                int selection = ShowMenu();

                switch (selection)
                {
                    case 1:
                        CreateUser();
                        break;
                    case 2:
                        DeleteUser();
                        break;
                    case 3:
                        EditUser();
                        break;
                    case 4:
                        TestLogin();
                        break;
                    case 5:
                        CreateNote();
                        break;
                    case 6:
                        ReadFile();
                        break;
                    default:
                        exit = true;
                        break;
                }

                if (exit)
                {
                    break;
                }
            }
        }



        private static void CreateUser()
        {
            Console.Clear();
            Console.WriteLine("Create a user");

            Console.Write("Enter username: ");
            string username = Console.ReadLine();

            Console.Write("Enter password: ");
            string password = Console.ReadLine();

            string passwordHash = HashPassword(password);

            Console.Write("Enter description: ");
            string description = Console.ReadLine();

            UserRepository.CreateUser(username, passwordHash, description);
        }

        private static void DeleteUser()
        {
            Console.Clear();
            Console.WriteLine("Delete User");

            User userToDelete = SelectUser();

            UserRepository.DeleteUserById(userToDelete.Id);

            Console.Clear();

        }

        private static void EditUser()
        {
            Console.Clear();

            Console.WriteLine("Edit User");

            User userToEdit = SelectUser();

            Console.Write("Enter a description: ");
            userToEdit.Description = Console.ReadLine();

            UserRepository.EditUserById(userToEdit.Id, userToEdit);

        }


        private static void TestLogin()
        {
            Console.Clear();
            Console.WriteLine("Lets test");


            while (true)
            {

                Console.Write("Enter username: ");
                string username = Console.ReadLine();

                Console.Write("Enter password: ");
                string inputPassword = Console.ReadLine();

                string hashedPassword = GetPassword(username);

                if (hashedPassword.Length > 0)
                {

                    string[] splittedInput = hashedPassword.Split(':');
                    string salt = splittedInput[1];

                    string combinedPasswordSalt = $"{inputPassword}:{salt}";
                    string hashedResult = CreateMd5(combinedPasswordSalt);

                    if (hashedResult == splittedInput[0])
                    {
                        Console.WriteLine("its a match");
                        Console.ReadLine();
                        Console.Clear();
                        break;
                    }
                    else
                    {
                        Console.WriteLine("It doesn`t match");
                    }
                }

                else
                {
                    Console.WriteLine("User doesn't exist");
                }
            }


        }

        private static void CreateNote()
        {
            Console.Clear();
            Console.WriteLine("Lets note");
            User userToNote = SelectUser();


            Console.Write("Enter a note: ");
            string notes = Console.ReadLine();

            if (userToNote.Notes == null)
            {
                userToNote.Notes = new List<string>();
            }

            userToNote.Notes.Add(notes);

            UserRepository.CreateNote(userToNote.Id, userToNote);
        }

        private static void ReadFile()
        {
            // string text = System.IO.File.ReadAllText(@"C:\Users\toffa\Documents\It och säkerhet\Kalles kaviar.txt");
            // System.Console.WriteLine("Contents of Kalles kaviar.txt = {0}", text);

            const string dir = @"C:\Users\toffa\Documents\It och säkerhet";  // your folder here
            var files = Directory.GetFiles(dir);
            for (var i = 0; i < files.Length; i++)
            {
                Console.WriteLine($"{i}) {Path.GetFileName(files[i])}");
            }

            while (true)
            {
                Console.WriteLine("Enter the number of the file you would like to see or -1 to exit");
                var input = Console.ReadLine();

                if (int.TryParse(input, out var index) && index > -2 && index < files.Length)
                {
                    if (index == -1)
                        return;

                    string[] chosenFile = System.IO.File.ReadAllLines(files[index]);
                    foreach (string file in chosenFile)
                    {
                        Console.WriteLine(file);
                    }

                }
                else
                    Console.WriteLine("Bad input. Repeat please.");
            }

        }

        static string HashPassword(string password)
        {
            string salt = RandomString(25);
            string saltedPassword = $"{password}:{salt}";

            string md5 = CreateMd5(saltedPassword);

            return $"{md5}:{salt}";
        }

        private static string CreateMd5(string input)
        {
            MD5 mD5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = mD5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        private static string GetPassword(string username)
        {
            List<User> users = UserRepository.GetUsers();

            string password = "";
            for (int i = 0; i < users.Count; i++)
            {

                if (username == users[i].UserName)
                {
                    password = users[i].Password;
                }
               
            }

            return password;
        }

        private static User SelectUser()
        {
            List<User> users = UserRepository.GetUsers();

            Console.WriteLine("Id name - description");

            for (int i = 0; i < users.Count; i++)
            {
                Console.WriteLine($"{i + 1}:{users[i].UserName} {users[i].Description} ");
            }

            Console.Write("select a user: ");
            string input = Console.ReadLine();
            int selectedNumber = int.Parse(input);
            return users[selectedNumber - 1];
        }

        private static int ShowMenu()
        {
            Console.WriteLine("Menu");
            Console.WriteLine("1. Create User");
            Console.WriteLine("2. Delete User");
            Console.WriteLine("3. Edit User");
            Console.WriteLine("4. Test Login");
            Console.WriteLine("5. Create note");
            Console.WriteLine("6. Read file");

            Console.Write("Input selection: ");

            string input = Console.ReadLine();
            int.TryParse(input, out int selectedOption);
            return selectedOption;

        }
    }
}
