using UnityEngine;

public class Paint : MonoBehaviour
{
    private void OnEnable() => PaintRegister.Register(this);
}
