using System;
using System.Linq;
using UnityEngine;
using System.Text;



public class GameUtilities : MonoBehaviour
{
    public static Sprite BASE_CARD_IMAGE
    {
        get 
        {
            if (_baseCardImage == null)
            {
                _baseCardImage = Resources.Load<Sprite>("Images/base");
            }

            return _baseCardImage;
        }
        set 
        {
            _baseCardImage = value;
        }
    }
    private static Sprite _baseCardImage;
    public static int CARD_WEIGHT = 5;
    public static int MAX_HAND_CAPACITY = 7;
    public static int START_CARD_COUNT = 3;


    public static string CapitalizeRemoveSpaces(string line)
    {
        string[] words = line.Split(" ");
        words = words.Select(word => char.ToUpper(word[0]) + word.Substring(1)).ToArray();
        string result = String.Join("", words);
        // Debug.Log(result);
        return result;
    }

    public static string DecodeAsci(string str)
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
