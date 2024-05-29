using System;
using TMPro;
using UnityEngine;


    public class PlayerManaComponent : MonoBehaviour
    {
        public int currentMana;
        public int MaxMana;
        public TextMeshProUGUI ManaText;
        public Team Team;
        private void Start()
        {
            EventManager.OnBoardInitialized.AddListener(ResetMana);
            if (Team == Team.Player)
            {
                // EventManager.AddPlayerCardPlayed(SpendMana);
                EventManager.OnPlayerCardPlayed.AddListener(SpendMana);
            }
            if (Team == Team.Opponent)
            {
                EventManager.OnOpponentCardPlayed.AddListener(SpendMana);
            }    
        }

        private void ResetMana()
        {
            MaxMana = 1;
            currentMana = 0;
        }

        public void SpendMana(Card card)
        {
            currentMana -= card.ManaComponent.Mana;
            UpdateManaText();
        }

        public void RefreshManaOnTurn()
        {
            if (MaxMana < 10)
            {
                MaxMana += 1;
            }
            
            currentMana = MaxMana;
            UpdateManaText();
        }

        public void AddMana(int amount)
        {
            currentMana += amount;
        }

        public void LoseMana(int amount)
        {
            currentMana -= amount;
        }

        public void UpdateManaText()
        {
            ManaText.text = currentMana + "/" + MaxMana;
        }
    }