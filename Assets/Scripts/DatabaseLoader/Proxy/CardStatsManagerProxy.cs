using System;
using System.Collections;
using System.Collections.Generic;
using DatabaseLoader.Flyweight;
using UnityEngine;

namespace DatabaseLoader.Proxy
{
    public class CardStatsManagerProxy : MonoBehaviour
    {
        [SerializeField] private CardStatsManager _cardStatsManager;
		private List<int> _loadedCardsId = new List<int>();

		public void InitCardLoader()
		{
			if (_cardStatsManager == null)
			{
				_cardStatsManager = GameObject.Find("GameManager").GetComponent<CardStatsManager>();
			}
			_cardStatsManager.OnCheckCount.AddListener(LoadCards);
			_cardStatsManager.CheckCardsCount();
		}

		public bool allCardsLoaded()
		{
			return _cardStatsManager.getAllCardsLoaded();
		}
		public void LoadCards()
		{
			int cardsCount = _cardStatsManager.GetCardsCount();

			for (int i = 1; i <= cardsCount; i++)
			{
				if (_loadedCardsId.Contains(i))
					continue;
				Debug.Log($"[{DateTime.Now:dd.MM.yyyy}] Loading card with id: {i}"); // Дабвление логирования
				_cardStatsManager.LoadCard(i); // Сохранение загруженных карт
				_loadedCardsId.Add(i);
			}
		}
    }
}