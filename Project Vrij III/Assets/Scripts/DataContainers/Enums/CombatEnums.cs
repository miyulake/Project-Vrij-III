public class CombatEnums
{
    public enum AttackInput
    {
        JAB,
        FORWARD,
        DOWNWARD,
        UPWARD,
        GRAB,
        SNAP,
        PUSH,
        TAUNT,
        SUPER
    }

    public enum MoveType
    {
        NORMAL,
        SPECIAL,
        SUPER,
        GRAB
    }

    public enum SuperType
    {
        NONE,
        CHAIN,
        TIME,
        TAUNT
    }

    public enum ContactType
    {
        NORMAL,
        BLOCK,
        COUNTER,
        PUNISH
    }

    [System.Flags]
    public enum MoveFlags
    {
        NONE = 0,
        ARMOR = 1 << 0,
        UNBLOCKABLE = 1 << 1
    }

    public enum SoundType
    {
        ThrowCaught,
        ThrowBreak,
        TauntActive,
        TauntInactive
    }
}
