using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MainManager : MonoBehaviour
{
    //values stored in the MainManager class will be shared by all instances of this class.
    //ie: if there were 10 instances of MainManager for some reason, and a value was changed in one, it would be changed for the other 9.
    public static MainManager Instance;

    public Color TeamColor;

    private void Awake()
    {
        //if the Instance variable is not null...
        //ie: it has already been assigned by a previous instance of this script
        if (Instance != null)
        {
            //then destroy this gameObject
            Destroy(gameObject);
            //and return
            return;
            //This is called a SINGLETON. it is used to ensure that only a single instance of the MainManager can ever exist, so it acts as a central point of access.
        }

        //Stores the current instance of MainManager in the class member Instance.
        //Allows you to call MainManager.Instance from any other script, and get a link to that specific instance of it.
        //ie: don't need a reference to it, like you do when assigning GameObjects to script properties in the Inspector.
        Instance = this;
        //Marks this GameObject attached to this script to not be destroyed when the scene changes.
        DontDestroyOnLoad(gameObject);

        LoadColor();
        Debug.Log(Application.persistentDataPath);
    }

    [System.Serializable]
    class SaveData
    {
        public Color TeamColor;
    }

    public void SaveColor()
    {
        // creates a new instance of the save data
        SaveData data = new SaveData();
        // fills that instance's team color class member with the TeamColor variable saved to the MainManager
        data.TeamColor = TeamColor;

        // transforms that instance (data) to JSON using JsonUtility
        string json = JsonUtility.ToJson(data);

        //uses the special method File.WriteAllText to write a string to a file
        //the first parameter is the path to the file. Application.persistentDataPath gives a foldder that will survive between application reinstall or update.
        // We then append the file name (/savefile.json)
        // The second parameter is the text we want written to that file. In this case, the string saved to the variable named "json".
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadColor()
    {
        // make a new string called path and set it to the path of the save file.
        string path = Application.persistentDataPath + "/savefile.json";

        // Check whether a file actually exists at the designated path before continuing
        if (File.Exists(path))
        {
            // creates a string (jason) and saves all text in the file at (path) to that string
            string json = File.ReadAllText(path);

            // takes the string and converts it back into an instance of the SaveData class.
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            // sets the TeamColor to the color defined in the save data.
            TeamColor = data.TeamColor;
        }
    }
}
