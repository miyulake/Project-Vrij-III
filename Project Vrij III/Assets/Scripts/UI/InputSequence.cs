using UnityEngine;
using UnityEngine.UI;

public class InputSequence : MonoBehaviour
{
    public enum InputType
    {
        NONE,
        JAB,
        FORWARD,
        DOWNWARD,
        UPWARD,
        GRAB,
        SNAP,
        PUSH,
        TAUNT,
        BLOCK,
        HOLD
    }

    public RawImage[] slots;

    [Header("Sequence Data")]
    public InputType[] controllerTypes;
    public InputType[] keyboardTypes;
}
