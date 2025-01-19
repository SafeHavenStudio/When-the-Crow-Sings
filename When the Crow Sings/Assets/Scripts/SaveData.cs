using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    public int saveDataVersion = 1;
    public Dictionary<string, bool> boolFlags = new Dictionary<string, bool>()
    {
        { "TestingFlag1",false },
        { "TestingFlag2",true },
        { "TestingFlag3",false },
        { "TestingFlag4",true },
        { "SceneStructureFlag", false },

        { "AngelTaskOffered",false },
        { "AngelTaskAccepted",false },
        { "AngelTaskCompleted",false },
        { "AngelTaskOn",false },
        { "AngelBaseCompleted",false },
        { "AngelExhausted",false },

        { "BeauTaskOffered",false },
        { "BeauTaskAccepted",false },
        { "BeauTaskCompleted",false },
        { "BeauTaskOn",false },
        { "BeauBaseCompleted",false },
        { "BeauExhausted",false },

        { "CalebTaskOffered",false },
        { "CalebTaskAccepted",false },
        { "CalebTaskCompleted",false },
        { "CalebTaskOn",false },
        { "CalebBaseCompleted",false },
        { "CalebExhausted",false },

        { "FaridaIntroduction",false },
        { "FaridaTaskOffered",false },
        { "FaridaTaskAccepted",false },
        { "FaridaTaskCompleted",false },
        { "FaridaTaskOn",false },
        { "FaridaBaseCompleted",false },
        { "FaridaExhausted",false },

        { "QuinnTaskOffered",false },
        { "QuinnTaskAccepted",false },
        { "QuinnTaskCompleted",false },
        { "QuinnTaskOn",false },
        { "QuinnBaseCompleted",false },
        { "QuinnExhausted",false },

        { "JazmyneTaskOffered",false },
        { "JazmyneTaskAccepted",false },
        { "JazmyneTaskCompleted",false },
        { "JazmyneTaskOn",false },
        { "PapersUp",false },
        { "PapersDown",false },
        { "QTEOn", false},
        { "JazmyneBaseCompleted",false },
        { "JazmyneExhausted",false },

        { "FranciscoTaskOffered",false },
        { "FranciscoTaskAccepted",false },
        { "FranciscoTaskCompleted",false },
        { "FranciscoTaskOn",false },
        { "FlowerOne",false },
        { "FlowerTwo",false },
        { "FlowerThree",false },
        { "FranciscoBaseCompleted",false },
        { "FranciscoExhausted",false },

        { "YuleTaskOffered",false },
        { "YuleTaskAccepted",false },
        { "YuleTaskCompleted",false },
        { "YuleTaskOn",false },
        { "YuleBaseCompleted",false },
        { "YuleExhausted",false },

        { "PhilomenaTaskOffered",false },
        { "PhilomenaTaskAccepted",false },
        { "PhilomenaTaskCompleted",false },
        { "PhilomenaTaskOn",false },
        { "PhilomenaBaseCompleted",false },
        { "PhilomenaExhausted",false },

        { "TheodoreIntroduction",false },
        { "TheodoreTaskOffered",false },
        { "TheodoreTaskAccepted",false },
        { "TheodoreTaskCompleted",false },
        { "TheodoreTaskOn",false },
        { "TheodoreBaseCompleted",false },
        { "TheodoreExhausted",false },

        { "ifBeauAndFrancisco",false },
        { "ifBeauAndAngel",false },
        { "ifFaridaAndCaleb",false },
        { "ifCalebAndAngel",false },

        { "SeenBrokenGauges",false },

        { "RecCenterDoorUnlocked",false },
        { "GreenhouseDoorUnlocked",false },
        { "EnergyHQDoorUnlocked",false },
        { "ClinicDoorUnlocked",false },
        { "Zone1DoorUnlocked",false },
        { "WoodPileMovable",false },
        { "Zone2DoorUnlocked",false },
        { "Zone3DoorUnlocked",false },
        { "Zone4DoorUnlocked",false },

        { "KeyInformation1",false },
        { "KeyInformation2",false },
        { "KeyInformation3",false },

        { "IntroContextSeen", false},
    };

    public Dictionary<string, int> intFlags = new Dictionary<string, int>()
    {
        { "TestingFlag1",0 },
        { "TestingFlag2",1 },
        { "TestingFlag3",2 },
        { "TestingFlag4",3 },

        {"day",1 },
        {"timeOfDay",1 },
        {"numberOfTasksCompleted",0 },
        {"TBD_OnLoadSpawnPoint_OrSomething",0 },

        {"penguin_cult",623 },
        {"benchcounter",0 },
        {"PhilTalk", 0},

        {"levelDataIndex", 1},
    };

    public Dictionary<string, string> stringFlags = new Dictionary<string, string>()
    {
        { "TestingFlag1","firstString" },
        { "TestingFlag2","secondString" },
        { "TestingFlag3","thirdString" },
        { "TestingFlag4","fourthString" },

        {"exampleString","myString" },
    };
}
