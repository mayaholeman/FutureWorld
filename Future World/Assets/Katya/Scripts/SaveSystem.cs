using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public static class SaveSystem
{
    public static void SavePlayer(Target target, Vector3 positionVec)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/Player.fun";
        try
        {
            FileStream stream = new FileStream(path, FileMode.Create);

            TargetData data = new TargetData(target, positionVec);
            formatter.Serialize(stream, data);
            stream.Close();
        }
        catch
        {
            Debug.Log("Could not make stream");
        }

    }

    public static void SaveEnemies(Target[] targetList, GameObject[] enemyGO)
    {
        for (int i = 0; i < targetList.Length; i++)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/" + targetList[i].Level + "/Enemy" + i + ".fun";
            try
            {
                FileStream stream = new FileStream(path, FileMode.Create);

                TargetData data = new TargetData(targetList[i], enemyGO[i].transform.position);
                formatter.Serialize(stream, data);
                stream.Close();
            }
            catch
            {
                Debug.Log("Could not make stream");
            }
        }


    }

    public static TargetData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/Player.fun";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                FileStream stream = new FileStream(path, FileMode.Open);
                TargetData data = formatter.Deserialize(stream) as TargetData;
                stream.Close();
                return data;
            }
            catch
            {
                Debug.Log("Could not make stream");
                return null;
            }

        }
        else
        {
            Debug.Log("Save File not found in " + path);
            return null;
        }
    }

    public static TargetData[] LoadEnemies(int level)
    {
        
        string path = Application.persistentDataPath + "/" + level;
        string[] dirs = Directory.GetDirectories(path);
        TargetData[] toReturn = new TargetData[dirs.Length];
        for (int i = 0; i < dirs.Length; i++)
        {
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                try
                {
                    FileStream stream = new FileStream(path, FileMode.Open);
                    TargetData data = formatter.Deserialize(stream) as TargetData;
                    stream.Close();
                    toReturn[i] = data;
                }
                catch
                {
                    Debug.Log("Could not make stream");
                    break;
                }

            }
            else
            {
                Debug.Log("Save File not found in " + path);
                break;
            }
        }
        return toReturn;
    }
}
