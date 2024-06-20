using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

public static class SavedGamesContainer
{
    public static string path = Application.dataPath + "/Resources/SavedData.xml";
    public static string recoveryPath = Application.dataPath + "/Resources/SavedDataRecovery.xml";
    public static string modifyingPath = Application.dataPath + "/Resources/SavedDataModifying.xml";

    [Serializable]
    public struct SavedGame
    {
        public string name;
        public int level;
        public int positionX;
        public int positionY;
        public int positionZ;
    }

    //DATOS
    public static Dictionary<string, SavedGame> savedGames = new Dictionary<string, SavedGame>();
    
    public static string currentGame;

    //UTILS
    private static List<SavedGame> transferGamesList = new List<SavedGame>();

    public static void Save()
    {
        try
        {
            File.Copy(path, modifyingPath);
        }
        catch (Exception e)
        {
            Debug.Log("Failed Saving (Process: Creating copy for writing new data).");
            return;
        }

        transferGamesList.Clear();
        foreach (KeyValuePair<string,SavedGame> savedGame in savedGames)
        {
            transferGamesList.Add(savedGame.Value);
        }

        XmlSerializer serializer = new XmlSerializer(typeof(List<SavedGame>));
        TextWriter writer = new StreamWriter(modifyingPath, false);

        try
        {
            serializer.Serialize(writer, transferGamesList);
        }
        catch (Exception e)
        {
            Debug.Log("Failed Saving (Process: Writing new data).");
            return;
        }

        writer.Close();

        try
        {
            File.Replace(modifyingPath, path, recoveryPath);
        }
        catch
        {
            Debug.Log("Failed Saving (Process: Replacing previous data).");
        }
    }

    public static void Load()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(List<SavedGame>));
        TextReader reader = new StreamReader(path);
        transferGamesList.Clear();

        try
        {
            transferGamesList = (List<SavedGame>)serializer.Deserialize(reader);
        }
        catch (Exception e)
        {
            Debug.Log("Failed loading data (Process: Loading data), loading recovery.");
            reader = new StreamReader(recoveryPath);
            transferGamesList.Clear();

            try
            {
                transferGamesList = (List<SavedGame>)serializer.Deserialize(reader);
            }
            catch
            {
                Debug.Log("Failed loading data (Process: Loading recovery).");
                return;
            }
        }

        savedGames.Clear();

        foreach(SavedGame savedGame in transferGamesList)
        {
            savedGames.Add(savedGame.name, savedGame);
        }

        reader.Close();
    }

    public static void CreateNewGame()
    {
        int i = 0;
        while(i>=0)
        {
            if (savedGames.ContainsKey("Game"+i))
            {
                i++;
            }
            else
            {
                SavedGame newSavedGame = new SavedGame
                {
                    name = "Game" + i,
                    level = 0,
                    positionX = 0,
                    positionY = 0,
                    positionZ = 0,
                };

                savedGames.Add(newSavedGame.name, newSavedGame);
                currentGame = newSavedGame.name;

                i = -1;
            }
        }
    }

    //public static void Load()
    //{
    //    XmlDocument xmlDoc = new XmlDocument();
    //    xmlDoc.Load(path);

    //    XmlNode xmlRootNode;
    //    xmlRootNode = xmlDoc.SelectSingleNode("SavedGamesCollection");

    //    savedGames.Clear();

    //    for (int i = 0; i < xmlRootNode.ChildNodes.Count; i++)
    //    {
    //        SavedGame currentGame = new SavedGame();
    //        currentGame.name = xmlRootNode.ChildNodes[i].Name;
    //        currentGame.level = int.Parse(xmlRootNode.SelectSingleNode(xmlRootNode.ChildNodes[i].Name).SelectSingleNode("level").InnerText);
    //        currentGame.positionX = int.Parse(xmlRootNode.SelectSingleNode(xmlRootNode.ChildNodes[i].Name).SelectSingleNode("positionX").InnerText);
    //        currentGame.positionY = int.Parse(xmlRootNode.SelectSingleNode(xmlRootNode.ChildNodes[i].Name).SelectSingleNode("positionY").InnerText);
    //        currentGame.positionZ = int.Parse(xmlRootNode.SelectSingleNode(xmlRootNode.ChildNodes[i].Name).SelectSingleNode("positionZ").InnerText);

    //        savedGames.Add(currentGame.name, currentGame);
    //    }
    //}

    //public static void Save(SavedGame savedGame)
    //{
    //    XmlDocument xmlDoc = new XmlDocument();
    //    xmlDoc.Load(path);

    //    if (xmlDoc.SelectSingleNode("SavedGamesCollection").SelectSingleNode(savedGame.name)==null)
    //    {
    //        XmlElement Game = xmlDoc.CreateElement(savedGame.name);

    //        XmlElement level = xmlDoc.CreateElement("level");
    //        level.InnerText = savedGame.level.ToString();
    //        Game.AppendChild(level);

    //        XmlElement positionX = xmlDoc.CreateElement("positionX");
    //        positionX.InnerText = savedGame.positionX.ToString();
    //        Game.AppendChild(positionX);

    //        XmlElement positionY = xmlDoc.CreateElement("positionY");
    //        positionY.InnerText = savedGame.positionY.ToString();
    //        Game.AppendChild(positionY);

    //        XmlElement positionZ = xmlDoc.CreateElement("positionZ");
    //        positionZ.InnerText = savedGame.positionZ.ToString();
    //        Game.AppendChild(positionZ);

    //        xmlDoc.SelectSingleNode("SavedGamesCollection").AppendChild(Game);
    //    }
    //    else
    //    {
    //        XmlNode gameNode = xmlDoc.SelectSingleNode("SavedGamesCollection").SelectSingleNode(savedGame.name);

    //        gameNode.SelectSingleNode("level").InnerText = savedGame.level.ToString();

    //        gameNode.SelectSingleNode("positionX").InnerText = savedGame.positionX.ToString();

    //        gameNode.SelectSingleNode("positionY").InnerText = savedGame.positionY.ToString();

    //        gameNode.SelectSingleNode("positionZ").InnerText = savedGame.positionZ.ToString();
    //    }

    //    xmlDoc.Save(path);
    //}
}
