
using DefaultNamespace;
using UnityEngine;
[CreateAssetMenu(menuName = "Hero", fileName = "Hero")]
public class HeroStats : ScriptableObject
{
    public int id;
    public new string name;
    public int Health;
    public Sprite Image;

    public Race Race;
    
}
