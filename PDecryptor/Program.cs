using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PDecryptor
{
    class Program
    {
        static string contents;
        static void Main(string[] args)
        {
            Console.Title = "PDecryptor";
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.BackgroundColor = ConsoleColor.Black;
            string sgPath = System.IO.Path.Combine(Environment.GetEnvironmentVariable("USERPROFILE"), "saved games");
            
            Console.WriteLine("Pixel Dungeon ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.Write("Decryptor");
            Console.ResetColor();
            Console.Write(" with ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Pretty print");
            Console.ResetColor();
           
            string xorIt(string str)
            {
                int key = 0x1F;
                string str1 = "";
                for (var i = 0; i < str.Length; i++)
                {
                    str1 += Convert.ToChar(((int) str[i]) ^ key);
                }

                return str1;
            }

            if (args.Length != 0)
            {
                if (args.Contains("-d"))
                {
                    Console.WriteLine("Saved games directory: " + sgPath);
                    Console.WriteLine("Detected .dat files: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    foreach (var i in Directory.GetFiles(sgPath, "*.dat"))
                    {
                        Console.WriteLine(Path.GetFileName(i));
                        contents = xorIt(File.ReadAllText(i));

                        if (args.Contains("-pretty"))
                        {
                            var obj = JsonConvert.DeserializeObject(xorIt(File.ReadAllText(i)));
                            contents = JsonConvert.SerializeObject(obj, Formatting.Indented);
                        }

                        File.WriteAllText(Path.ChangeExtension(Path.GetFileName(i), "json"), contents);
                    }
                }

                if (args.Contains("-encrypt"))
                {
                    Console.WriteLine("Detected .json files: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    foreach (var i in Directory.GetFiles(Environment.CurrentDirectory, "*.json"))
                    {
                        Console.WriteLine(Path.GetFileName(i));
                        contents = xorIt(JObject.Parse(File.ReadAllText(i)).ToString(Newtonsoft.Json.Formatting.None));
                        File.WriteAllText(sgPath + "\\" + Path.ChangeExtension(Path.GetFileName(i), "dat"), contents);

                    }
                }

                //Console.ResetColor();
                Console.WriteLine("Done.");
            }
            else
            {
                Console.WriteLine("-d for decrypt\n-encrypt for encrypt\n-pretty for pretty print");
            }

            //string str = File.ReadAllText(@"C:\Users\Admin\Saved Games\warrior.dat"); 
            Console.ReadLine();

            }
        }
    }
