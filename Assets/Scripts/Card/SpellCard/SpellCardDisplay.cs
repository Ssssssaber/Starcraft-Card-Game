using TMPro;
using UnityEngine;
using System;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SpellCardDisplay : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI _name;
    [SerializeField] protected Image _image;
    public GameObject BackImage;
    
    [FormerlySerializedAs("_card")] [SerializeField] private SpellCard _spellCard;
    [SerializeField] private TextMeshProUGUI _manaCost;

    void Awake()
    {
        _spellCard = GetComponent<SpellCard>();
        _spellCard.ManaComponent.OnChange.AddListener(ApplyMana);
    }
    
    public void DisplayBackImage(bool visible)
    {
        BackImage.SetActive(visible);
    }
    
    public void ApplyStats()
    {
        _name.text = _spellCard.Name;
        _image.sprite = _spellCard.Image;

        _manaCost.text = _spellCard.ManaComponent.Mana.ToString();
    }
    
    private void ApplyMana()
    {
        _manaCost.text = _spellCard.ManaComponent.Mana.ToString();
    }
}