using UnityEngine;

public class Paint : MonoBehaviour
{
    private void OnEnable() => PaintRegister.Instance.Register(this);
}
