using System;
using MyClasses.Languages;
using System.IO;
using System.Runtime.Serialization.Json;

namespace Lang_Compiler
{
    class Program
    {
        static void Serialize(Language language, string name)
        {
            try
            {
                FileStream stream = new FileStream(name, FileMode.Create);
                DataContractJsonSerializer serializator = new DataContractJsonSerializer(typeof(Language));
                serializator.WriteObject(stream, language);
                stream.Close();
                Console.WriteLine("Udało się!");
            }
            catch
            {
                Console.WriteLine("Nie udało się!");
            }
        }
        static void CreateList()
        {
            LangList list = new LangList();
            list.SetNames(new object[] { "English", "Polski" , "Español" });
            list.SetPaths(new object[] { "langs\\Lang_en.json", "langs\\Lang_pl.json", "langs\\Lang_es.json" });
            try
            {
                FileStream stream = new FileStream("langs\\List_languages.json", FileMode.Create);
                DataContractJsonSerializer serializator = new DataContractJsonSerializer(typeof(LangList));
                serializator.WriteObject(stream, list);
                stream.Close();
                Console.WriteLine("Udało się!");
            }
            catch(Exception e)
            {
                Console.WriteLine("Nie udało się!");
                Console.WriteLine(e.Message);
            }
        }
        static void Main(string[] args)
        {
            CreateList();

            Language language1 = new Language();
            English(language1);
            Serialize(language1, "langs\\Lang_en.json");

            Language language2 = new Language();
            Polish(language2);
            Serialize(language2, "langs\\Lang_pl.json");

            Language language3 = new Language();
            Spanish(language3);
            Serialize(language3, "langs\\Lang_es.json");
        }
        static void Polish(Language lang)
        {
            lang.play = "Graj";
            lang.load = "Wczytaj";
            lang.options = "Ustawienia";
            lang.exit = "Wyjdź";
            lang.save = "Zapisz";

            lang.lang = "Język";
            lang.time = "Czas";
            lang.noTime = "Brak limitu";
            lang.minute = "minut";
            lang.back = "Powrót";

            lang.version = "Wersja: ";
            lang.author = "Autor: ";
            lang.turn[0] = "Tura Białych";
            lang.turn[1] = "Tura Czarnych";

            lang.ask = "Czy na pewno chcesz przerwać tę rozgrywkę?";
            lang.answers[0] = "Tak";
            lang.answers[1] = "Nie";
            lang.answers[2] = "Zapisz i wyjdź";
        }
        static void English(Language lang)
        {
            lang.play = "Play";
            lang.load = "Load";
            lang.options = "Options";
            lang.exit = "Exit";
            lang.save = "Save";

            lang.lang = "Language";
            lang.time = "Time";
            lang.noTime = "No time limit";
            lang.minute = "minutes";
            lang.back = "Back";

            lang.version = "Version: ";
            lang.author = "Author: ";
            lang.turn[0] = "white turn";
            lang.turn[1] = "Black turn";

            lang.ask = "Are you sure you wont to leave this game?";
            lang.answers[0] = "Yes";
            lang.answers[1] = "No";
            lang.answers[2] = "Save and quit";
        }
        static void Spanish(Language lang)
        {
            lang.play = "Jugar";
            lang.load = "Cargar";
            lang.options = "Ajustes";
            lang.exit = "Salir";
            lang.save = "Guardar";

            lang.lang = "Idioma";
            lang.time = "Tiempo";
            lang.noTime = "Sin límite";
            lang.minute = "minutos";
            lang.back = "Devolver";

            lang.version = "Versión: ";
            lang.author = "Author: ";
            lang.turn[0] = "El turno de las blancas";
            lang.turn[1] = "El turno de las negras";

            lang.ask = "¿Estás seguro de que quieres detener este juego?";
            lang.answers[0] = "Si";
            lang.answers[1] = "No";
            lang.answers[2] = "Guardar y salir";
        }
    }
}
