using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DefaultNamespace;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class JsonTest
{
    private List<string> lines = new List<string>();
    
    // A Test behaves as an ordinary method
    [Test]
    public void JsonTestSimplePasses()
    {
        // Загрузка данных из текстового файла
        LoadTestFiles();
        
        foreach (var line in lines)
        {
            // Проверка данных на соотвествие
            Assert.Throws<Exception>(()=>CardStatsManager.SetCardData(line));
        }
    }

    public void LoadTestFiles()
    {
        
        string line;

        StreamReader sr = new StreamReader("B:/Unity Projects/Starcraft Card Game/Assets/Tests/EditMode/txtFiles/invalidJSON.txt");
        line = sr.ReadLine();
        // Чтение всех строк
        while (line != null)
        {
            lines.Add(line);
            Debug.Log(line);
            line = sr.ReadLine();
        }
    }
}
