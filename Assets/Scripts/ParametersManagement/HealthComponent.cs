using UnityEngine;
using UnityEngine.Serialization;
using System;
using Interfaces;
using UnityEngine.Events;

public class HealthComponent :  MonoBehaviour, IHealth
{
    public UnityEvent OnChange = new UnityEvent();
    public UnityEvent OnDie = new UnityEvent();

    private void Start()
    {
        Health = MaxHealth;
    }
    
    public int Health
    {
        get => _health;
        protected set
        {
            _health = value;
            
            OnChange?.Invoke();
        }
    }

    public int _health;
    
    public int MaxHealth;

    public void SetupHealth(int amount)
    {
        MaxHealth = amount;
        Health = amount;
    }

    public float Ratio => (float) Health / (float) MaxHealth;
    
    public void Heal(int amount)
    {
        Health += amount;

        if (Health > MaxHealth)
        {
            Health = MaxHealth;
        }
    }

    public void IncreaseHealth(int amount)
    {
        MaxHealth += amount;
        Health += amount;
    }

    public void DecreaseHealth(int amount)
    {
        MaxHealth -= amount;
        Health -= amount;

        if (Health <= 0)
        {
            Health = 1;
        }
    }

    public void Damage(int amount)
    {
        Health -= amount;

        if (Health <= 0)
        {
            OnDie?.Invoke();
        }
    }

}
