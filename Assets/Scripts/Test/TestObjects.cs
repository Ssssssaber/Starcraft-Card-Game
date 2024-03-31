using System;
using System.Collections.Generic;
using UnityEngine;

namespace MockObjects
{
    public class MockCreatureCard
    {
        public string Name;
        public int Health
        {
            get 
            {
                return _health;
            }
            set 
            {
                _health = value;
                if (_health <= 0)
                {
                    OnDie?.Invoke(this);
                }
            }
        }

        private int _health;
        public Action<MockCreatureCard> OnDie;
        public int Attack;
        public bool CanAttack;
        public bool OnTable;
        public Team Team;

        public MockCreatureCard(string name, int health, int attack, Team team)
        {
            Name = name;
            Health = health;
            Attack = attack;
            OnTable = false;
            Team = team;
        }

        public void ReceiveDamage(int amount)
        {
            Health -= amount;
        }

        public void DamageCard(MockCreatureCard otherCard)
        {
            if ((this.OnTable) && (otherCard.OnTable) && (this.Team != otherCard.Team) && this.CanAttack)
            {
                otherCard.ReceiveDamage(this.Attack);
                this.ReceiveDamage(otherCard.Attack);
                // Debug.Log("this " + Health + "; other" + otherCard.Health);
            }
        }

        public void PlayCard(InterfaceObject tableBehavior)
        {
            if (tableBehavior.Team == this.Team)
            {
                tableBehavior.AddCard(this);
                OnTable = true;
                CanAttack = false;
                Debug.Log("Play Card " + this.Name);
            }
        }

        public string PrintStats()
        {
            return $"{Name}: {Health}, {Attack}";
        }
    }

    public class Mediator
    {
        public List<MockTableBehavior> tables = new List<MockTableBehavior>();
        
        public void Notify(string message, MockTableBehavior table)
        {
            foreach (var tbl in tables)
            {
                tbl.ReceiveMessage(message);
            }
        }
    }

    public interface InterfaceObject
    {
        public string Name { get; set; }
        public Team Team { get; set; }
        public void SetMediator(Mediator mediator);
        public List<MockCreatureCard> GetCards();
        public void ReceiveMessage(string message);

        public void AddCard(MockCreatureCard card);

        public void RemoveCard(MockCreatureCard card);
        public void RefreshAttackAbility();

        public void PrintCardStats();

        public void DisableAttackAbility();
    }

    public class MockTableBehavior : InterfaceObject
    {
        public List<MockCreatureCard> CardsList = new List<MockCreatureCard>();
        
        public string Name { get; set; }
        
        public readonly int maxCardsOnTable = 5;
        public Team Team { get; set; }
        private Mediator _mediator;

        public MockTableBehavior(string name, Team team)
        {
            Name = name;
            Team = team;
            
        }

        public void SetCards(List<MockCreatureCard> cards)
        {
            foreach (var card in cards)
            {
                AddCard(card);
            }
        }

        public List<MockCreatureCard> GetCards()
        {
            return CardsList;
        }

        public void SetMediator(Mediator mediator)
        {
            _mediator = mediator;
            mediator.tables.Add(this);
        }

        public void ReceiveMessage(string message)
        {
            Debug.Log($"{Team} table received a message: {message}");
        }

        public void AddCard(MockCreatureCard card)
        {
            if (card != null)
            {
                card.OnDie = RemoveCard;
                card.CanAttack = false;
                GetCards().Add(card);
                if (_mediator != null) _mediator.Notify($"Card with stats: {card.PrintStats()} played on table {this.Team}", this);
            }
        }

        public void RemoveCard(MockCreatureCard card)
        {
            if (card != null && CardsList.Contains(card))
            {
                CardsList.Remove(card);
                Debug.Log("card " + card.Name + " removed");
            }
        }
        public void RefreshAttackAbility()
        {
            foreach (var card in CardsList)
            {
                card.CanAttack = true;
            }
        }

        public void PrintCardStats()
        {
            Debug.Log(Team);
            foreach (var card in CardsList)
            {
                card.PrintStats();
            }
        }
        
        public void DisableAttackAbility()
        {
            foreach (var card in CardsList)
            {
                card.CanAttack = false;
            }
        }
        
    }

    public class ProtossTable : MockTableBehavior
    {
        public ProtossTable(string name, Team team) : base(name, team)
        {
            
        }

        public Race CheckRace()
        {
            return Race.Protoss;
        }
    }

    public class ZergTable : MockTableBehavior
    {
        public ZergTable(string name, Team team) : base(name, team)
        {
            
        }
        
        public Race CheckRace()
        {
            return Race.Zerg;
        }
    }
}