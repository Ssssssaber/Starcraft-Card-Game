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

    public bool isAlive 
    {
        get { return _isAlive; }
        private set 
        {
            _isAlive = value;
        }
    }

    private bool _isAlive = true;
    
    private void Start()
    {
        player = GetComponentInParent<PlayerManager>();
        _awareness = BoardAwareness.Instance;
        HealthComponent.OnChange.AddListener(UpdateHealth);
        HealthComponent.AddOnDieEffect(HeroDied);
        HealthComponent.AddOnDieEffect(EndGame);

        EventManager.OnBoardInitialized.AddListener(ResetHero);

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

    private void HeroDied()
    {
        isAlive = false;
    }

    private void ResetHero()
    {
        isAlive = true;
        HealthComponent.ResetHealth();
    }

    private void EndGame()
    {
        EventManager.GameEnded();
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
