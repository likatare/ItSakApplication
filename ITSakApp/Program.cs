using LibraryITSak;
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

        static void Main(string[] args)
        {
            ITSakLibraryApp iTSakLibraryApp = new ITSakLibraryApp();
            iTSakLibraryApp.Start();


        }
    }
}
