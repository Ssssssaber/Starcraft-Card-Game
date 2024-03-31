using TMPro;
using UnityEngine;
using System;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CreatureCardDisplay : MonoBehaviour
{
    [SerializeField] private CreatureCard _creatureCard;

    [SerializeField] protected TextMeshProUGUI _name;
    [SerializeField] protected Image _image;
    public GameObject BackImage;

    [SerializeField] private TextMeshProUGUI _manaCost;
    [SerializeField] private TextMeshProUGUI _damage;
    [SerializeField] private TextMeshProUGUI _health;

    void Awake()
    {
        _creatureCard = GetComponent<CreatureCard>();
       
        _creatureCard.ManaComponent.OnChange.AddListener(ApplyMana);
        _creatureCard.AttackComponent.OnChange.AddListener(ApplyAttack);
        _creatureCard.HealthComponent.OnChange.AddListener(ApplyHealth);
        
    }
    
    public void DisplayBackImage(bool visible)
    {
        BackImage.SetActive(visible);
    }

    public void ApplyStats()
    {
        _name.text = _creatureCard.Name;
        _image.sprite = _creatureCard.Image;

        _manaCost.text = _creatureCard.ManaComponent.Mana.ToString();
        
        _damage.text = _creatureCard.AttackComponent.Attack.ToString();
        _health.text = _creatureCard.HealthComponent.Health.ToString();
    }

    private void ApplyHealth()
    {
        _health.text = _creatureCard.HealthComponent.Health.ToString();
    }

    private void ApplyAttack()
    {
        _damage.text = _creatureCard.AttackComponent.Attack.ToString();
    }

    private void ApplyMana()
    {
        _manaCost.text = _creatureCard.ManaComponent.Mana.ToString();
    }
}