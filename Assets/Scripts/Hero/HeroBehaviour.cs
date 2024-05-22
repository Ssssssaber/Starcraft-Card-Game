using System;
using DefaultNamespace;
using Interfaces;
using Menu;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HeroBehaviour : MonoBehaviour
{
    public HealthComponent HealthComponent;
    [SerializeField] TextMeshProUGUI _heroName;

    [SerializeField] private Image _heroImage;
    [SerializeField] private Image _healthBar;

    private BoardAwareness _awareness;

    public PlayerManager player;

    public Team Team;
    
    private void Start()
    {
        player = GetComponentInParent<PlayerManager>();
        _awareness = BoardAwareness.Instance;
        HealthComponent.OnChange.AddListener(UpdateHealth);

        if (Team == Team.Opponent)
        {
            // ApplyStats(BoardBehaviour.instance.OpponentHeroStats);
            ApplyStats(HeroDatabase.HeroDict[OptionStats.OpponentHero + 1]);
        }
        else
        {
            ApplyStats(HeroDatabase.HeroDict[OptionStats.PlayerHero + 1]);
            // ApplyStats(BoardBehaviour.instance.PlayerHeroStats);
        }
    }
    
    private void UpdateHealth()
    {
        _healthBar.fillAmount = HealthComponent.Ratio;
    }

    private void ApplyStats(HeroStats heroStats)
    {
        _heroName.text = heroStats.name;
        _heroImage.sprite = heroStats.Image;
        player.HeroStats = heroStats;
        player.Race = heroStats.Race;
        // if (Team == Team.Opponent)
        // {
        //     _awareness.OpponentHeroStats = heroStats;
        //     _awareness.OpponentRace = heroStats.Race;
        // }
        // else
        // {
        //     _awareness.PlayerHeroStats = heroStats;
        //     _awareness.PlayerRace = heroStats.Race;
        // }

        HealthComponent.SetupHealth(heroStats.Health);
    }
}
