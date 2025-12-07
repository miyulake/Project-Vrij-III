[System.Flags]
public enum MoveFlags
{
    NONE = 0,
    ARMOR = 1 << 0,
    UNBLOCKABLE = 1 << 1,
    METER = 1 << 2
}