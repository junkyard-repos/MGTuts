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
}
