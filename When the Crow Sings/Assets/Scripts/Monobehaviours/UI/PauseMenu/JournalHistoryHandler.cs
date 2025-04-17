using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class JournalHistoryHandler : MonoBehaviour
{
    public GameObject contentHolder;
    public GameObject templateEntriesHolder;

    [HideInInspector]
    public List<HistoryEntry> historyEntries = new List<HistoryEntry>();

    Dictionary<string,bool> _associatedData = new Dictionary<string,bool>();

    private void Awake() // Using awake because this needs to happen regardless of being Enabled.
    {
        GetHistoryEntriesFromTemplate();

        foreach (int i in SaveDataAccess.saveData.historyEntriesOrder)
        {
            AddNextHistoryEntry(i, false);
        }
    }
    private void Update()
    {
        CheckEntriesAgainstFlags();
    }

    private void CheckEntriesAgainstFlags()
    {
        for(int i = 0; i < historyEntries.Count; i++)
        {
            if (SaveDataAccess.saveData.historyEntriesOrder.Contains(i) == false &&
                SaveDataAccess.saveData.boolFlags[historyEntries[i].associatedDataKey_EnableEntry] == true)
            {
                AddNextHistoryEntry(i);
            }
        }
    }


    private void GetHistoryEntriesFromTemplate()
    {
        historyEntries = templateEntriesHolder.GetComponentsInChildren<HistoryEntry>(true).ToList();
    }

    void AddNextHistoryEntry(int nextEntryIndex, bool updateSaveData = true)
    {
        // Simply changing the transform. No alterations to the original list are being made.
        historyEntries[nextEntryIndex].transform.SetParent(contentHolder.transform);
        historyEntries[nextEntryIndex].transform.SetAsFirstSibling();

        if (updateSaveData ) SaveDataAccess.saveData.historyEntriesOrder.Add(nextEntryIndex);
        
    }
}
