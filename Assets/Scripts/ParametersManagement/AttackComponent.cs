using Interfaces;
using UnityEngine;
using UnityEngine.Events;


    public class AttackComponent : MonoBehaviour, IAttack
    {
        public UnityEvent OnChange = new UnityEvent();

        public int Attack
        {
            get
            {
                return _attack;
            }
            protected set
            {
                _attack = value;
                OnChange?.Invoke();
            }
        }

        private int _attack;
        
        public void IncreaseAttack(int amount)
        {
            Attack += amount;
        }

        public void DecreaseAttack(int amount)
        {
            Attack -= amount;

            if (Attack < 0)
            {
                Attack = 0;
            }
        }

        public void SetupAttack(int amount)
        {
            Attack = amount;
        }
    }