using Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.Events;


    public class ManaComponent :  MonoBehaviour, IMana
    {
        public UnityEvent OnChange = new UnityEvent();
        
        public int Mana
        {
            get
            {
                return _mana;
            }
            protected set
            {
                _mana = value;
                OnChange?.Invoke();
            }
        }

        public void SetupMana(int amount)
        {
            Mana = amount;
        }

        private int _mana;
        
        public void IncreaseManaCost(int amount)
        {
            Mana += amount;
        }

        public void DecreaseManaCost(int amount)
        {
            Mana -= amount;

            if (Mana < 0)
            {
                Mana = 0;
            }
        }
    }
