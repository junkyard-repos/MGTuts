using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace RPGMadeEasy
{
    public class JsonManager<T>
    {
        public Type Type;

        public T Load(string path)
        {
            T instance;

            using (TextReader reader = new StreamReader(path))
            {
                string jsonValue = reader.ReadToEnd();

                instance = (T)JsonConvert.DeserializeObject(jsonValue, Type);

            }

            return instance;
        }
    }


    //public class Rootobject
    //{
    //    public Image Image { get; set; }
    //}

    //public class Image
    //{
    //    public string Path { get; set; }
    //    public Position Position { get; set; }
    //}

    //public class Position
    //{
    //    public int X { get; set; }
    //    public int Y { get; set; }
    //}

}
