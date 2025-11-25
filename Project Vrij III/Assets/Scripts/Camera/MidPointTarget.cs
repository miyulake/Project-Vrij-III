using UnityEngine;

public class MidPointTarget : MonoBehaviour
{
    private void Update()
    {
        if (RoundManager.Instance.CurrentState == RoundState.KNOCKOUT)
            transform.position = new Vector3(GetMidPoint().x, GetMidPoint().y, transform.position.z);
    }

    private Vector3 GetMidPoint() => (
        PlayerManager.Instance.playerOne.transform.position +
        PlayerManager.Instance.playerTwo.transform.position
        ) / 2;
}