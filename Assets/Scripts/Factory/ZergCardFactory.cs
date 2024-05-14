
using UnityEngine;

namespace DefaultNamespace.Factory
{
    public class ZergCardFactory : AbstractFactory
    {
        private GameObject creaturePrefab;
        private GameObject spellPrefab;
        
        public ZergCardFactory()
        {
            creaturePrefab = Resources.Load<GameObject>("Prefab/Card/Zerg/ZergCreatureCard");
            spellPrefab = Resources.Load<GameObject>("Prefab/Card/Zerg/ZergSpellCard");

            if ((creaturePrefab == null) || (spellPrefab == null))
            {
                Debug.Log("Hehe-haha");
            }
        }
        
        // // ReSharper disable Unity.PerformanceAnalysis
        // public override CreatureCard CreateCreatureCard()
        // {
        //     var go = Object.Instantiate(creaturePrefab);
        //     var card = go.GetComponent<CreatureCard>();
        //     return card;
        // }

        

        // // ReSharper disable Unity.PerformanceAnalysis
        // public override SpellCard CreateSpellCard()
        // {
        //     var go = Object.Instantiate(spellPrefab);
        //     var card = go.GetComponent<SpellCard>();
        //     return card;
        // }

        public override CreatureCard CreateCreatureCard(PlayerManager player)
        {
            var go = Object.Instantiate(creaturePrefab);
            var card = go.GetComponent<CreatureCard>();
            card.ownerPlayer = player; 
            return card;
        }

        

        // ReSharper disable Unity.PerformanceAnalysis
        public override SpellCard CreateSpellCard(PlayerManager player)
        {
            var go = Object.Instantiate(spellPrefab);
            var card = go.GetComponent<SpellCard>();
            card.ownerPlayer = player; 
            return card;
        }
    }
}