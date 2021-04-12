using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text.Json;

namespace netrans
{
    class Program
    {       
        static void Main(string[] args)
        {
            start:
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Введите слово");
            Console.ForegroundColor = ConsoleColor.White;
            string text = Convert.ToString(Console.ReadLine());
            string entering_text;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Обращаемся к серверам yandex, подождите\n");
            Console.ForegroundColor = ConsoleColor.White;
            WebRequest request = WebRequest.Create("https://dictionary.yandex.net/api/v1/dicservice.json/lookup?" + "key=dict.1.1.20210412T095511Z.4c083894f80b8bad.7fac82fc335897d2f205324d2ea51a26000111bd" + "&lang=" + "ru-en" + "&text=" + text);
            WebResponse response = request.GetResponse();

            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string added_string = reader.ReadToEnd();                

                Check_Dictionary dict_yandex = JsonConvert.DeserializeObject<Check_Dictionary>(added_string);

                if (dict_yandex.Def.Length == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ничего не нашел :с");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    foreach (var elem in dict_yandex.Def)
                    {


                        foreach (var elemy in elem.Tr)
                        {
                            Console.WriteLine("перевод = " + elemy.Text);
                        }
                        Console.WriteLine("род = " + elem.Gen);
                        Console.WriteLine("живой? = " + elem.Anm + "\n");
                    }
                }
            }
            goto start;
        }
    }
    [Serializable]
    public class Check_Dictionary
    {
        public Head Head { get; set; }
        public Def[] Def { get; set; }
    }
    public class Head
    {

    }
    public class Def
    {
        public string Anm { get; set; }
        public string Gen { get; set; }
        public string Pos { get; set; }
        public string Text { get; set; }
        public Tr[] Tr { get; set; }
    }
    public class Tr
    {
        public string Pos { get; set; }
        public string Text { get; set; }
        public string Fr { get; set; }
    }
}
