using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class DeckTest
{
    // Словарь с данными {id, имя карты}: {{1, Dark Templar}, {2, Hydralisk}, {3, Mutalisk}, {4, Queen}, {5, Roach}, {6, Sentry}, {7, Templar}, {8, Ultralisk}, {9, Void Ray}, {10, Zealot}, {11, Zergling}, {12, Storm}, {13, Transfuse}, }
    public static Dictionary<int, string> CardNamesDict = new Dictionary<int, string>()
    {
        {1, "Dark Templar"},
        {2, "Hydralisk"}, 
        {3, "Mutalisk"}, 
        {4, "Queen"}, 
        {5, "Roach"},
        {6, "Sentry"},
        {7, "Templar"},
        {8, "Ultralisk"},
        {9, "Void Ray"},
        {10, "Zealot"},
        {11, "Zergling"},
        {12, "Storm"},
        {13, "Transfuse"} 
    };
    // Код колоды для тестирования
    private string deckString = "MTszOzM7Mzs3Ozc7ODsxMDsxMTs=";

    private List<string> testNames = new List<string>();

    private List<string> resultNames = new List<string>() {"Dark Templar", "Mutalisk","Templar","Ultralisk","Zealot","Zergling" };
    
    // A Test behaves as an ordinary method
    [Test]
    public void DeckTestSimplePasses()
    {
        // Создаем колоду с помощью метода и проверяем на соотвествие
        CreateDeckFromString(deckString);
        Assert.AreEqual(testNames, resultNames);
    }
    
    public void CreateDeckFromString(string deckString)
    {
        // декодирование строки
        deckString = decode(deckString);
        deckString = deckString.Remove(deckString.Length - 1);
        
        
        int[] ints = deckString.Split(';').Select(int.Parse).ToArray();
        // добавление карт в колоду
        foreach (int id in ints)
        {
            if (CardNamesDict.ContainsKey(id))
            {
                if (!testNames.Contains(CardNamesDict[id]))
                {
                    testNames.Add(CardNamesDict[id]);
                }
                // Debug.Log(Team + " Adding");
            }
        }
    }
    
    public string decode(string str)
    { 
        int mod4 = str.Length % 4;
        if (mod4 > 0)
        {
            str += new string('=', 4 - mod4);
        }
        
        byte[] data = Convert.FromBase64String(str);
        string decodedString = Encoding.UTF8.GetString(data);
        return decodedString;
    }
}
