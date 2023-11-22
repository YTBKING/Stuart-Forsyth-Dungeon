using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Tracing;
using System.Dynamic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using Spectre.Console;

namespace Dungeon
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game dungeon = new Game();
            //instantiation - creating an object (instance of a class)
            //declaration - declaring the existence of a variable and it's type
            //initialisation - giving a variable it's initial or first value
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(@"
                                                                                
▀██▀▀█▄   ▀██▀  ▀█▀ ▀█▄   ▀█▀  ▄▄█▀▀▀▄█  ▀██▀▀▀▀█   ▄▄█▀▀██   ▀█▄   ▀█▀ 
 ██   ██   ██    █   █▀█   █  ▄█▀     ▀   ██  ▄    ▄█▀    ██   █▀█   █  
 ██    ██  ██    █   █ ▀█▄ █  ██    ▄▄▄▄  ██▀▀█    ██      ██  █ ▀█▄ █  
 ██    ██  ██    █   █   ███  ▀█▄    ██   ██       ▀█▄     ██  █   ███  
▄██▄▄▄█▀    ▀█▄▄▀   ▄█▄   ▀█   ▀▀█▄▄▄▀█  ▄██▄▄▄▄▄█  ▀▀█▄▄▄█▀  ▄█▄   ▀█  ");
            Console.ForegroundColor = ConsoleColor.DarkGray;

            Console.WriteLine("\x1B[3m");
            dungeon.PlayGame();
            Console.WriteLine("\x1B[0m");
        }
    }

}
