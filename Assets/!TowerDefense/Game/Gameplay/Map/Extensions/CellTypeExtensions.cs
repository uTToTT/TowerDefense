public static class CellTypeExtensions 
{
    public static bool IsBlocked(this CellType type) => type switch
    {
        CellType.Path => true,
        CellType.Entrance => true,
        CellType.Exit => true,
        CellType.Blocked => true,
        _ => false
    };
}
