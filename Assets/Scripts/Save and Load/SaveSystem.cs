using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.InteropServices.ComTypes;


public static class SaveSystem 
{
    public static void SaveGame(GameManager2 gameManager)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = "D:/School_and_Work/Unity Projects/Home - 3d Game/Assets/001_Scripts/Save and Load" + "/HomeData.fun";

        FileStream stream = new FileStream(path, FileMode.Create);

        GameData data = new GameData(gameManager);

        formatter.Serialize(stream, data);

        stream.Close();
    }

    public static GameData LoadGame()
    {
        string path = "D:/School_and_Work/Unity Projects/Home - 3d Game/Assets/001_Scripts/Save and Load" + "/HomeData.fun";

        //FileStream stream = new FileStream(path, FileMode.Open);
        FileStream stream = new FileStream(path, FileMode.Open);
        if (File.Exists(path) && stream.Length > 0)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            GameData data = formatter.Deserialize(stream) as GameData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file was not found in " + path);
            BinaryFormatter formatter = new BinaryFormatter();
            GameData data = new GameData(GameObject.Find("Game Manager 2").GetComponent<GameManager2>());
            formatter.Serialize(stream, data);
            stream.Close();
            return data;
        }
    }

    //public static void SavePlayer (Player player)
    //{
    //    BinaryFormatter formatter = new BinaryFormatter();

    //    string path = "D:/School_and_Work/Unity Projects/Home - 3d Game/Assets/001_Scripts/Save and Load" + "/player.fun";

    //    FileStream stream = new FileStream(path, FileMode.Create);

    //    PlayerData data = new PlayerData(player);

    //    formatter.Serialize(stream, data);

    //    stream.Close();
    //}

    //public static PlayerData LoadPlayer ()
    //{
    //    string path = "D:/School_and_Work/Unity Projects/Home - 3d Game/Assets/001_Scripts/Save and Load" + "/player.fun";

    //    if (File.Exists(path))
    //    {
    //        BinaryFormatter formatter = new BinaryFormatter();
    //        FileStream stream = new FileStream (path, FileMode.Open);

    //        PlayerData data = formatter.Deserialize(stream) as PlayerData;

    //        stream.Close();

    //        return data;

    //    } else
    //    {
    //        Debug.Log("Save file not found in " + path);
    //        return null;
    //    }
    //}
}
