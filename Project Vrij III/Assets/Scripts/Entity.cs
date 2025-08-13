using UnityEngine;

public class Entity : MonoBehaviour
{
    private bool inHitstun = false;

    public bool SetHitstunState(bool state) => inHitstun = state;
}
