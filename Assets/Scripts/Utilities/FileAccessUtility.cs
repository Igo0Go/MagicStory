using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;

public static class FileAccessUtility
{
    public static string stringSeparator = "\n";
    public static string stringPartSeparator = "><";
    public static string propertyPartSeparator = "_";

    public static void SaveSpellInTheFile(Spell spell, string fileName)
    {
        string saveString = spell.GetDataString();

        string filePath = Path.Combine(Application.streamingAssetsPath, "Saves", "Spells", fileName + ".txt");

        Debug.Log(filePath);

        if (!File.Exists(filePath))
        {
            File.Create(filePath).Close();
        }

        using (TextWriter writer = new StreamWriter(filePath, false))
        {
            writer.Write(saveString);
        }
    }
    public static List<string> GetAllSpellFilesNames()
    {
        List<string> list = new List<string>();

        string directoryPath = Path.Combine(Application.streamingAssetsPath, "Saves", "Spells");

        DirectoryInfo di = new DirectoryInfo(directoryPath);
        FileInfo[] files = di.GetFiles("*.txt");
        foreach (FileInfo file in files)
        {
            list.Add(file.FullName);
        }

        return list;
    }
    public static Spell LoadSpellFromTheFile(string fileName, bool nameOnly)
    {
        if(nameOnly)
        {
            fileName = Path.Combine(Application.streamingAssetsPath, "Saves", "Spells", fileName + ".txt");
        }

        string info = string.Empty;
        using (StreamReader reader = new StreamReader(fileName))
        {
            info = reader.ReadToEnd();
        }
        return Spell.GetSpellByInfo(info);
    }

    public static List<string> GetAllMagicansFilesNames()
    {
        List<string> list = new List<string>();

        string directoryPath = Path.Combine(Application.streamingAssetsPath, "Saves", "Magicans");

        DirectoryInfo di = new DirectoryInfo(directoryPath);
        FileInfo[] files = di.GetFiles("*.txt");
        foreach (FileInfo file in files)
        {
            list.Add(file.FullName);
        }

        return list;
    }
    public static void SaveMagicanInTheFile(Magican magican, string fileName)
    {
        string saveString = magican.GetSaveString();

        string filePath = Path.Combine(Application.streamingAssetsPath, "Saves", "Magicans", fileName + ".txt");

        Debug.Log(filePath);

        if (!File.Exists(filePath))
        {
            File.Create(filePath).Close();
        }

        using (TextWriter writer = new StreamWriter(filePath, false))
        {
            writer.Write(saveString);
        }
    }

    public static Magican LoadMagicanFromFile(string fileName, bool nameOnly)
    {
        if (nameOnly)
        {
            fileName = Path.Combine(Application.streamingAssetsPath, "Saves", "Magicans", fileName + ".txt");
        }

        string info = string.Empty;
        using (StreamReader reader = new StreamReader(fileName))
        {
            info = reader.ReadToEnd();
        }

        return Magican.GetMagican(info);
    }

}
