using ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
public class SaveDataAccess
{
    static public SaveData saveData = new SaveData();
    static public List<string> keysToTriggerPopup = new List<string> { "TestingFlag1" };

    static public GameSignal popupUpdateMessageSignal;
    public static void SetFlag(string key, bool value)
    {
        saveData.boolFlags[key] = value;
        Debug.Log(key + " is now "+ saveData.boolFlags[key]);

        // Flags related to completing Francisco's task
        if (saveData.boolFlags["FlowerOne"] && saveData.boolFlags["FlowerTwo"] && saveData.boolFlags["FlowerThree"]) saveData.boolFlags["FranciscoTaskCompleted"] = true;
        if (saveData.boolFlags["FlowerOne"] && saveData.boolFlags["FlowerTwo"] && saveData.boolFlags["FlowerThree"]) saveData.boolFlags["MFFranciscoTC"] = true;
        if (saveData.boolFlags["FlowerOne"] && saveData.boolFlags["FlowerTwo"] && saveData.boolFlags["FlowerThree"]) saveData.boolFlags["HFFranciscoTC"] = true;
        if (saveData.boolFlags["FlowerOne"] && saveData.boolFlags["FlowerTwo"] && saveData.boolFlags["FlowerThree"]) saveData.boolFlags["FranciscoTaskOn"] = false;
        if (saveData.boolFlags["FlowerOne"] && saveData.boolFlags["FlowerTwo"] && saveData.boolFlags["FlowerThree"]) saveData.boolFlags["MFFranciscoTA"] = false;

        // Flags related to completing Yule's task
        if (saveData.boolFlags["YuleString"] && saveData.boolFlags["YuleRod"] && saveData.boolFlags["YuleHook"]) saveData.boolFlags["YuleTaskPartCompleted"] = true;
        if (saveData.boolFlags["YuleString"] && saveData.boolFlags["YuleRod"] && saveData.boolFlags["YuleHook"]) saveData.boolFlags["MFYuleTPC"] = true;
        if (saveData.boolFlags["YuleString"] && saveData.boolFlags["YuleRod"] && saveData.boolFlags["YuleHook"]) saveData.boolFlags["MFYuleTA"] = false;
        if (saveData.boolFlags["YuleString"] && saveData.boolFlags["YuleRod"] && saveData.boolFlags["YuleHook"]) saveData.boolFlags["YuleTaskOneOn"] = false;

        // Flags related to completing Theodore's task
        if (saveData.boolFlags["RubiksCube"] && saveData.boolFlags["PaperClip"] && saveData.boolFlags["Thumbdrive"]) saveData.boolFlags["TheodoreTaskCompleted"] = true;
        if (saveData.boolFlags["RubiksCube"] && saveData.boolFlags["PaperClip"] && saveData.boolFlags["Thumbdrive"]) saveData.boolFlags["MFTheodoreTC"] = true;
        if (saveData.boolFlags["RubiksCube"] && saveData.boolFlags["PaperClip"] && saveData.boolFlags["Thumbdrive"]) saveData.boolFlags["MFTheodoreTA"] = false;
        if (saveData.boolFlags["RubiksCube"] && saveData.boolFlags["PaperClip"] && saveData.boolFlags["Thumbdrive"]) saveData.boolFlags["TheodoreTaskOn"] = false;

        // Flags related to completing Philomena's task
        if (saveData.boolFlags["PhilBatt1"] && saveData.boolFlags["PhilBatt2"] && saveData.boolFlags["PhilCasette"]) saveData.boolFlags["PhilomenaTaskPartCompleted"] = true;
        if (saveData.boolFlags["PhilBatt1"] && saveData.boolFlags["PhilBatt2"] && saveData.boolFlags["PhilCasette"]) saveData.boolFlags["MFPhilomenaTPC"] = true;
        if (saveData.boolFlags["PhilBatt1"] && saveData.boolFlags["PhilBatt2"] && saveData.boolFlags["PhilCasette"]) saveData.boolFlags["MFPhilomenaTA"] = false;
        if (saveData.boolFlags["PhilBatt1"] && saveData.boolFlags["PhilBatt2"] && saveData.boolFlags["PhilCasette"]) saveData.boolFlags["HFPhilomenaTPC"] = true;

        // Flags related to dialogue checks to allow the player to see the game cares who they talked to lol
        if (saveData.boolFlags["BeauBaseCompleted"] && saveData.boolFlags["FranciscoBaseCompleted"]) saveData.boolFlags["ifBeauAndFrancisco"] = true;
        if (saveData.boolFlags["BeauBaseCompleted"] && saveData.boolFlags["AngelBaseCompleted"]) saveData.boolFlags["ifBeauAndAngel"] = true;
        if (saveData.boolFlags["FaridaBaseCompleted"] && saveData.boolFlags["CalebBaseCompleted"]) saveData.boolFlags["ifFaridaAndCaleb"] = true;
        if (saveData.boolFlags["CalebBaseCompleted"] && saveData.boolFlags["AngelBaseCompleted"]) saveData.boolFlags["ifCalebAndAngel"] = true;
        
        // Flags related to combinations for endings
        if (saveData.boolFlags["KeyInformation1"] && saveData.boolFlags["KeyInformation2"] && saveData.boolFlags["KeyInformation3"]) saveData.boolFlags["GoodEnding"] = true;
        if (saveData.boolFlags["KeyInformation1"] && saveData.boolFlags["KeyInformation2"]) saveData.boolFlags["NeutralEnding1"] = true;
        if (saveData.boolFlags["KeyInformation1"] && saveData.boolFlags["KeyInformation3"]) saveData.boolFlags["NeutralEnding2"] = true;
        if (saveData.boolFlags["KeyInformation2"] && saveData.boolFlags["KeyInformation3"]) saveData.boolFlags["NeutralEnding3"] = true;

    }
    public static void SetFlag(string key, int value)
    {
        saveData.intFlags[key] = value;
        if (saveData.intFlags["timeOfDay"] > 3)
        {
            saveData.intFlags["timeOfDay"] = 1;
            saveData.intFlags["day"] += 1;
        }
            
        Debug.Log(key + " is now " + saveData.intFlags[key]);
    }
    public static void SetFlag(string key, string value)
    {
        saveData.stringFlags[key] = value;
        Debug.Log(key + " is now " + saveData.stringFlags[key]);

        if (keysToTriggerPopup.Contains(key) && value== "true")
        {
            popupUpdateMessageSignal.Emit();
        }
    }

    //public static bool GetFlag<Bool>(string key)
    //{
    //    return boolFlags[key];
    //}
    //public static int GetFlag<Int>(string key)
    //{
    //    return intFlags[key];
    //}
    //public static string GetFlag<String>(string key)
    //{
    //    return stringFlags[key];
    //}

    static void writeInt(int integer, FileStream fileStream)
    {
        byte[] valueBytes = BitConverter.GetBytes(integer);
        fileStream.Write(valueBytes, 0, valueBytes.Length);
    }

    public static void WriteDataToDisk()
    {
        switch (saveData.saveDataVersion)
        {
            case 0:
#pragma warning disable CS0162 // Unreachable code detected
                WriteData_V0();
                break;
            default:
                WriteData_V0();
                break;
#pragma warning restore CS0162 // Unreachable code detected
        }

    }
    public static void ReadDataFromDisk()
    {
        ResetSaveData();

        switch (saveData.saveDataVersion) // TODO: Make it so it starts reading, stops after the version number, then calls the correct method using this switch statement.
        {
            case 0:
#pragma warning disable CS0162 // Unreachable code detected
                ReadData_V0();
                break;
            default:
                ReadData_V0();
                break;
#pragma warning restore CS0162 // Unreachable code detected
        }
        Debug.Log("Data read!");
    }

    public static IEnumerator EraseDataFromDisk()
    {
        string filePath = Application.persistentDataPath + "/save.wtcs";

        while (true)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    Debug.Log("File deleted successfully.");
                }
                yield break; // Exit once deletion is successful
            }
            catch (IOException)
            {
                Debug.LogWarning("File is in use by another process, retrying...");
            }
            yield return new WaitForSeconds(0.1f); // Retry after a delay
        }
    }

    public static void ResetSaveData()
    {
        saveData = new SaveData();
    }


    public static bool SavedDataExistsOnDisk()
    {
        if (File.Exists(Application.persistentDataPath + "/save.wtcs"))
        {
            return true;
        }
        Debug.Log("No save data exists on disk!");
        return false;
    }

    static void WriteData_V0()
    {
        //EraseData();

        string filePath = Application.persistentDataPath + "/save.wtcs";

        FileStream fileStream;
        fileStream = new FileStream(filePath, FileMode.Create);

        // Write the save data version.
        writeInt(saveData.saveDataVersion, fileStream);

        foreach (KeyValuePair<string,bool> i in saveData.boolFlags)
        {
            byte valueByte = i.Value ? (byte)1 : (byte)0;
            fileStream.WriteByte(valueByte);
        }
        foreach (KeyValuePair<string,int> i in saveData.intFlags)
        {
            writeInt(i.Value, fileStream);
        }
        foreach (KeyValuePair<string,string> i in saveData.stringFlags)
        {
            byte[] valueBytes = Encoding.UTF8.GetBytes(i.Value); //BitConverter.GetBytes(i.Value);
            byte[] valueBytesLengthBytes = BitConverter.GetBytes(valueBytes.Length);
            fileStream.Write(valueBytesLengthBytes,0, valueBytesLengthBytes.Length);
            fileStream.Write(valueBytes, 0, valueBytes.Length);
        }

        writeInt(saveData.historyEntriesOrder.Count, fileStream);
        foreach (int i in saveData.historyEntriesOrder)
        {
            writeInt(i, fileStream);
        }


        fileStream.Close();
        Debug.Log("File saved to disk!");
    }

    static int ReadInt(FileStream fileStream)
    {
        byte[] intBytes = new byte[4];
        fileStream.Read(intBytes, 0, 4);
        return BitConverter.ToInt32(intBytes, 0);
    }
    static string ReadString(FileStream fileStream)
    {
        int stringLength = ReadInt(fileStream);

        byte[] stringBytes = new byte[stringLength];
        fileStream.Read(stringBytes,0,stringLength);
        return Encoding.UTF8.GetString(stringBytes);
    }
    static void ReadData_V0()
    {
        string filePath = Application.persistentDataPath + "/save.wtcs";
        FileStream fileStream = new FileStream(filePath, FileMode.Open);
        
        int loadedSaveDataVersion = ReadInt(fileStream);

        Dictionary<string,bool> tempBoolFlags = new Dictionary<string,bool>();
        foreach (KeyValuePair<string, bool> i in saveData.boolFlags)
        {
            tempBoolFlags.Add(i.Key, i.Value);
            tempBoolFlags[i.Key] = fileStream.ReadByte() == 1; //BitConverter.ToBoolean(fileBytes, loop);
        }
        saveData.boolFlags = tempBoolFlags;

        Dictionary<string, int> tempIntFlags = new Dictionary<string, int>();
        foreach (KeyValuePair<string,int> i in saveData.intFlags)
        {
            tempIntFlags.Add(i.Key, i.Value);
            tempIntFlags[i.Key] = ReadInt(fileStream);
        }
        saveData.intFlags = tempIntFlags;

        Dictionary<string, string> tempStringFlags = new Dictionary<string, string>();
        foreach (KeyValuePair<string, string> i in saveData.stringFlags)
        {
            tempStringFlags.Add(i.Key, i.Value);
            tempStringFlags[i.Key] = ReadString(fileStream);
        }
        saveData.stringFlags = tempStringFlags;

        // Get the order of journal entries.
        int orderCount = ReadInt(fileStream);
        saveData.historyEntriesOrder = new List<int>();
        for (int i = 0; i < orderCount; i++)
        {
            saveData.historyEntriesOrder.Add(ReadInt(fileStream));
        }

        fileStream.Close();
    }


    private static void PenguinCultAttemptsToScheduleAMeeting()
    {
        SetFlag("penguin_cult",UnityEngine.Random.Range(0,623));
    }
}
