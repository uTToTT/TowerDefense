[System.Serializable]
public class MapObjectPort
{
    public CellOffset Cell;          // локальная клетка формы
    public PortDirection Direction;  // куда "смотрит"
    public PortType Type;
}
