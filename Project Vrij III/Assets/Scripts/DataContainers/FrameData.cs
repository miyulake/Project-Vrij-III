[System.Serializable]
public class FrameData
{
    public int startup;
    public int active;
    public int recovery;
    public FrameRange cancel;

    public int TotalFrames() => startup + active + recovery;
    public bool IsActive(int frame) => frame > startup && frame <= startup + active;
    public bool IsRecovering(int frame) => frame > startup + active;
}

[System.Serializable]
public struct FrameRange
{
    public int start;
    public int end;
}