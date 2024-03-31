
using MockObjects;
using UnityEngine;

public class ZergTableCreator : Creator
{
    public override InterfaceObject FactoryMethod()
    {
        ZergTable table = new ZergTable("Zerg Table", Team.Opponent);
        return table;
    }
}