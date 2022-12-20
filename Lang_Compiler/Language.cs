using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace MyClasses
{
    [DataContract]
    class Language
    {
        //Menu główne
        [DataMember]
        public string play;
        [DataMember]
        public string load;
        [DataMember]
        public string options;
        [DataMember]
        public string exit;
        [DataMember]
        public string save;
        //ustawienia
        [DataMember]
        public string lang;
        [DataMember]
        public string time;
        [DataMember]
        public string noTime;
        [DataMember]
        public string back;
        //stopka
        [DataMember]
        public string version;
        [DataMember]
        public string author;
        //w grze
        [DataMember]
        public string[] turn = new string[2];
        //pytanie powrót do menu
        [DataMember]
        public string ask;
        [DataMember]
        public string[] answers = new string[3];

        //funkcje
        public static Language Load(string path)
        {
            Language lang;
            try
            {
                FileStream stream = new FileStream(path, FileMode.Open);
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Language));
                lang = (Language)serializer.ReadObject(stream);
                stream.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
            return lang;
        }
    }
    [DataContract]
    class LangList
    {
        [DataMember]
        public string[] names;
        [DataMember]
        public string[] paths;
        public void SetNames(Array array)
        {
            names = new string[array.Length];
            int i = 0;
            foreach (string a in array)
            {
                names[i] = a;
                i++;
            }
        }
        public void SetPaths(Array array)
        {
            paths = new string[array.Length];
            int i = 0;
            foreach (string a in array)
            {
                paths[i] = a;
                i++;
            }
        }
        public static LangList Load(string path)
        {
            LangList lang;
            try
            {
                FileStream stream = new FileStream(path, FileMode.Open);
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(LangList));
                lang = (LangList)serializer.ReadObject(stream);
                stream.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
            return lang;
        }
    }
}
