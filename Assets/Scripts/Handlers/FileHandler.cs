using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Handlers
{
    /// <summary>
    /// Provides static methods for saving and loading data in various formats (Raw, List, JSON).
    /// </summary>
    public static class FileHandler
    {
        /// <summary> Saves a raw string to a file. </summary>
        public static void SaveData(string name, string data)
        {
            WriteFile(GetPath(name), data);
        }

        /// <summary> Loads raw text content from a file. </summary>
        public static string LoadData(string name)
        {
            return ReadFile(GetPath(name));
        }

        /// <summary> Saves a list of strings, each on a new line. </summary>
        public static void SaveList(List<string> list, string filename)
        {
            File.WriteAllLines(GetPath(filename), list);
        }

        /// <summary> Reads a file and converts its lines into a List of strings. </summary>
        public static List<string> ReadList(string filename)
        {
            return File.ReadAllLines(GetPath(filename)).ToList();
        }

        /// <summary> Serializes a list of objects into a JSON array and saves it to a file. </summary>
        public static void SaveToJSON<T>(List<T> toSave, string filename)
        {
            Debug.Log(GetPath(filename));
            var content = ToJson(toSave.ToArray());
            WriteFile(GetPath(filename), content);
        }

        /// <summary> Serializes a single object into JSON format and saves it to a file. </summary>
        public static void SaveToJSON<T>(T toSave, string filename)
        {
            var content = JsonUtility.ToJson(toSave);
            WriteFile(GetPath(filename), content);
        }

        /// <summary> Helper: Wraps an array into a JSON wrapper string. </summary>
        private static string ToJson<T>(T[] array)
        {
            var wrapper = new Wrapper<T>
            {
                Items = array
            };
            return JsonUtility.ToJson(wrapper);
        }

        /// <summary> Helper: Deserializes a JSON wrapper string back into an array. </summary>
        private static T[] FromJson<T>(string json)
        {
            var wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items;
        }

        [Serializable]
        private class Wrapper<T>
        {
            public T[] Items;
        }

        /// <summary> Reads a JSON file and returns its content as a List of objects. </summary>
        public static List<T> ReadListFromJSON<T>(string filename)
        {
            var content = ReadFile(GetPath(filename));

            if (string.IsNullOrEmpty(content) || content == "{}")
            {
                return new List<T>();
            }

            var res = FromJson<T>(content).ToList();
            return res;
        }

        /// <summary> Parses a raw JSON string into a List of objects. </summary>
        public static List<T> ReadListFromJsonString<T>(string content)
        {
            if (string.IsNullOrEmpty(content) || content == "{}")
            {
                return new List<T>();
            }

            var res = FromJson<T>(content).ToList();
            return res;
        }

        /// <summary> Reads a JSON file and deserializes it into a single object of type T. </summary>
        public static T ReadFromJSON<T>(string filename)
        {
            var content = ReadFile(GetPath(filename));

            if (string.IsNullOrEmpty(content) || content == "{}")
            {
                return default(T);
            }

            var res = JsonUtility.FromJson<T>(content);
            return res;
        }

        /// <summary> Returns the full absolute path for a filename in the persistent data path. </summary>
        public static string GetPath(string filename)
        {
            return Application.persistentDataPath + "/" + filename;
        }

        /// <summary> Returns the full path including a specific sub-folder. </summary>
        public static string GetPath(string folder, string filename)
        {
            return Application.persistentDataPath + "/" + folder + "/" + filename;
        }

        /// <summary> Checks if a file exists at the persistent data path. </summary>
        public static bool Exists(string filename)
        {
            return File.Exists(GetPath(filename));
        }

        /// <summary> Internal method to write string content to a specified file path. </summary>
        private static void WriteFile(string path, string content)
        {
            var fileStream = new FileStream(path, FileMode.Create);
            using var writer = new StreamWriter(fileStream);
            writer.Write(content);
        }

        /// <summary> Internal method to read string content from a specified file path. </summary>
        private static string ReadFile(string path)
        {
            if (!File.Exists(path)) return "";
            using var reader = new StreamReader(path);
            var content = reader.ReadToEnd();
            return content;
        }
    }
}