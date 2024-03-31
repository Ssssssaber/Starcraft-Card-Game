using System;
using Interfaces;
using UnityEngine;
using UnityEngine.Events;

    public abstract class StatsManager : MonoBehaviour
    {
        // public static UnityEvent<Card> OnDie = new UnityEvent<Card>();
        //
        // public int MaxHealth;
        // public int Health
        // {
        //     get => _health;
        //     set
        //     {
        //         _health = value;
        //         CardDisplay.OnChange?.Invoke();
        //     }
        // }
        //
        // private int _health;
        //
        // public void Heal(int amount)
        // {
        //     Health += amount;
        //
        //     if (Health > MaxHealth)
        //     {
        //         Health = MaxHealth;
        //     }
        // }
        //
        // public void Damage(int amount)
        // {
        //     Health -= amount;
        //
        //     if (Health <= 0)
        //     {
        //         OnDie?.Invoke(this.GetComponent<Card>());
        //         Destroy(gameObject, 0.2f);
        //     }
        // }
        //
        // public int Attack
        // {
        //     get => _attack;
        //     set
        //     {
        //         _attack = value;
        //         CardDisplay.OnChange?.Invoke();
        //     }
        // }
        //
        // private int _attack;
        //
        // public void BuffHealth(int amount)
        // {
        //     throw new NotImplementedException();
        // }
        //
        // public void DebuffHealth(int amount)
        // {
        //     throw new NotImplementedException();
        // }
        //
        // public void BuffAttack(int amount)
        // {
        //     Attack += amount;
        // }
        //
        // public void DebuffAttack(int amount)
        // {
        //     Attack -= amount;
        //     if (Attack < 0)
        //     {
        //         Attack = 0;
        //     }
        // }
        //
        // public int ManaCost
        // {
        //     get => _attack;
        //     set
        //     {
        //         _attack = value;
        //         CardDisplay.OnChange?.Invoke();
        //     }
        // }
        //
        // public void IncreaseManaCost(int amount)
        // {
        //     ManaCost += amount;
        // }
        //
        // public void DecreaseManaCost(int amount)
        // {
        //     ManaCost -= amount;
        //
        //     if (ManaCost < 0)
        //     {
        //         ManaCost = 0;
        //     }
        // }
    }