
using MockObjects;

public class ProtossTableCreator : Creator
{
    public override InterfaceObject FactoryMethod()
    {
        ProtossTable table = new ProtossTable("Protoss Table", Team.Player);
        return table;
    }
}