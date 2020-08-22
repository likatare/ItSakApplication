using Newtonsoft.Json;
using Repository;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ITSakApp
{
    class Program
    {
        private static readonly Random random = new Random();
        const string BACKUP_FILE_PATH = @"C:\Users\toffa\Documents\It och säkerhet\Backup\";
        const string KEYS_SAVE_PATH = @"C:\Users\toffa\Documents\It och säkerhet\ITSakAppKeys\";
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
                    case 7:
                        CreateBackup();
                        break;
                    case 8:
                        RestoreBackup();
                        break;
                    case 9:
                        CreateKeys();
                        break;
                    case 10:
                        EncryptMessage();
                        break;
                    case 11:
                        DecryptMessage();
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

                string hashedPassword = GetUserByUsername(username);

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

                    string[] chosenFile = File.ReadAllLines(files[index]);
                    foreach (string file in chosenFile)
                    {
                        Console.WriteLine(file);
                    }

                }
                else
                    Console.WriteLine("Bad input. Repeat please.");
            }

        }

        private static void CreateBackup()
        {
            List<User> users = UserRepository.GetUsers();
            string usersJson = JsonConvert.SerializeObject(users);

            Console.Write("Enter name on backup: ");
            string input = Console.ReadLine();

            using (StreamWriter file =
            new StreamWriter($"{BACKUP_FILE_PATH}{input}.json", false))
            {
                file.Write(usersJson);
            }

            Console.WriteLine("Backup created");
        }

        private static void RestoreBackup()
        {
            string usersJson = "";

            string dir = SelectFile();

            using (var reader = new StreamReader(dir))
            {
                usersJson = reader.ReadToEnd();
            }

            var user = JsonConvert.DeserializeObject<List<User>>(usersJson);

            UserRepository.DeleteAllUsers();
            UserRepository.SaveManyUsers(user);
        }

        private static void CreateKeys()
        {
            var enc = new Cryptography();
            string publicKey = enc.GetPublicKey();
            string privateKey = enc.GetPrivateKey();

            using (StreamWriter file =
                new StreamWriter($"{KEYS_SAVE_PATH}private.key", false))
            {
                file.Write(privateKey);
            }

            using (StreamWriter file =
                new StreamWriter($"{KEYS_SAVE_PATH}public.key", false))
            {
                file.Write(publicKey);
            }


            Console.WriteLine("keys saved");
            Console.ReadLine();
        }

        private static void EncryptMessage()
        {

            var enc = new Cryptography();


            string inputText = "";

            using (var reader = new StreamReader($"{KEYS_SAVE_PATH}messageToEncrypt.txt"))
            {
                string file = reader.ReadToEnd();
                string[] splittedFile = file.Split('^');
                string publicKey = splittedFile[0];
                inputText = splittedFile[1];


                enc.SetKey(publicKey);
            }

            string encryptedMessage = enc.Encrypt(inputText);
            Console.WriteLine($"Encrypted: {encryptedMessage}");
            Console.ReadLine();
        }

        private static void DecryptMessage()
        {
            var enc = new Cryptography();


            string inputText = "";

            using (var reader = new StreamReader($"{KEYS_SAVE_PATH}messageToDecrypt.txt"))
            {
                string file = reader.ReadToEnd();
                string[] splittedFile = file.Split('~');
                string privateKey = splittedFile[0];
                inputText = splittedFile[1];


                enc.SetKey(privateKey);
            }

            string decryptedMessage = enc.Decrypt(inputText);
            Console.WriteLine($"decrypted: {decryptedMessage}");
            Console.ReadLine();
        }

        private static string SelectFile()
        {
            const string dir = @"C:\Users\toffa\Documents\It och säkerhet\backup";  // your folder here
            var files = Directory.GetFiles(dir);

            Console.WriteLine("Id name - description");

            for (int i = 0; i < files.Length; i++)
            {
                Console.WriteLine($"{i + 1}) {Path.GetFileName(files[i])}");
            }

            Console.Write("select a file: ");
            string input = Console.ReadLine();
            int selectedNumber = int.Parse(input);
            return files[selectedNumber - 1];
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
            MD5 mD5 = MD5.Create();
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



        private static string GetUserByUsername(string username)
        {


            User user = UserRepository.GetUserByUsername(username);

            string password = "";

            if (user != null)
            {
                password = user.Password;

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
            Console.WriteLine("7. Create backup");
            Console.WriteLine("8. Restore backup");
            Console.WriteLine("9. Create keys");
            Console.WriteLine("10. Encrypt message");
            Console.WriteLine("11. Decrypt message");

            Console.Write("Input selection: ");

            string input = Console.ReadLine();
            int.TryParse(input, out int selectedOption);
            return selectedOption;

        }
    }
}
