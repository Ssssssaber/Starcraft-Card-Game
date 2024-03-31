using UnityEngine;


namespace DefaultNamespace.Factory
{
    public class ProtossCardFactory : AbstractFactory
    {
        private GameObject creaturePrefab;
        private GameObject spellPrefab;
        
        public ProtossCardFactory()
        {
            creaturePrefab = Resources.Load<GameObject>("Prefab/Card/Protoss/ProtossCreatureCard");
            spellPrefab = Resources.Load<GameObject>("Prefab/Card/Protoss/ProtossSpellCard");

            if ((creaturePrefab == null) || (spellPrefab == null))
            {
                Debug.Log("Hehe-haha");
            }
        }
        
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