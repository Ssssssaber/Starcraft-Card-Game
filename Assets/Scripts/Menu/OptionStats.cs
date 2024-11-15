using System;
using System.Collections;
using System.Collections.Generic;
using Menu;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionStats : MonoBehaviour
{
   // think of bug: heroes selected independently of stats changed
   public static bool shuffleDecks = false;
   public static bool encoded = true;
   public static string PlayerDeckCode;
   public static string OpponentDeckCode;
   public static int PlayerHero = 0;
   public static int OpponentHero = 0;
   
   public TMP_InputField deckCodeInputField;
   public Toggle deckCodeToggle;
   public TMP_Dropdown heroDropdown;
   public TMP_Dropdown opponentDropdown;

   private void Start()
   {
      PlayButton.GameStart.AddListener(SetPlayerDeckCode);
      // PlayButton.GameStart.AddListener(SetPlayerHero);
      // PlayButton.GameStart.AddListener(SetOpponentHero);
   }
   
   public void SetPlayerDeckCode()
   {
      PlayerDeckCode = deckCodeInputField.text;
   }

   public void SetPlayerHero()
   {
      PlayerHero = heroDropdown.value;
      Debug.Log(heroDropdown.value);
      Debug.Log("Player" + heroDropdown.options[heroDropdown.value].text);
   }

   public void SetOpponentHero()
   {
      OpponentHero = opponentDropdown.value;
      Debug.Log(opponentDropdown.value);
      Debug.Log("Enemy" + opponentDropdown.options[opponentDropdown.value].text);
   }
}
