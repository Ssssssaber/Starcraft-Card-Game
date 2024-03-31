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
   public static String DeckCode;
   public static bool UseDeckCode = false;
   public static int PlayerHero = 0;
   public static int OpponentHero = 0;
   
   public TMP_InputField deckCodeInputField;
   public Toggle deckCodeToggle;
   public TMP_Dropdown heroDropdown;
   public TMP_Dropdown opponentDropdown;

   private void Start()
   {
      PlayButton.GameStart.AddListener(SetDeckCode);
      PlayButton.GameStart.AddListener(SetUseDeckCode);
      // PlayButton.GameStart.AddListener(SetPlayerHero);
      // PlayButton.GameStart.AddListener(SetOpponentHero);
   }
   
   public void SetDeckCode()
   {
      DeckCode = deckCodeInputField.text;
   }

   public void SetUseDeckCode()
   {
      UseDeckCode = deckCodeToggle;
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
