using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Menu;
using UnityEngine;

namespace DefaultNamespace
{
    public class HeroDatabase : MonoBehaviour
    {
        public static List<HeroStats> heroList = new List<HeroStats>();
        [SerializeField] private HeroStats[] _allHeroes;

        public static Dictionary<int, HeroStats> HeroDict = new Dictionary<int, HeroStats>();
        
        private void Awake()
        {
            _allHeroes = Resources.LoadAll<HeroStats>("Heroes/");
            GiveIds();
        }

        private void GiveIds()
        {
            for (int i = 0; i < _allHeroes.Length; i++)
            {
                HeroDict.Add(_allHeroes[i].id, _allHeroes[i]);
            }
        }
    }
}