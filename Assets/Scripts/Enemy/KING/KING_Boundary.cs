using UnityEngine;

public class KING_Boundary : MonoBehaviour
{
    [SerializeField] private string musicGroupName;
    [SerializeField] private GameObject door;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player != null)
        {
            door.SetActive(true);
            AudioManager.instance.StartBGM(musicGroupName);
        }
    }
}
