using UnityEngine;
using DefaultNamespace;

public abstract class AbstractFactory
{
    public abstract CreatureCard CreateCreatureCard(PlayerManager player);
    public abstract SpellCard CreateSpellCard(PlayerManager player);
}