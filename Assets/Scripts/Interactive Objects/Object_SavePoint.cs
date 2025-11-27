using UnityEngine;

public class Object_SavePoint : MonoBehaviour, ISaveable
{
    [SerializeField] private string checkpointID;
    [SerializeField] private Transform respawnPoint;

    public bool isActive { get; private set; }
    private Animator animator;
    private AudioSource fireAudioSource;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        fireAudioSource = GetComponent<AudioSource>();
    }
    public string GetCheckpointID() => checkpointID;
    public Vector3 GetPosition() => respawnPoint == null ? transform.position : respawnPoint.position;

    public void ActivateSavepoint(bool activate)
    {
        isActive = activate;
        animator.SetBool("Active", activate);
        if (isActive && fireAudioSource.isPlaying == false)
            fireAudioSource.Play();
        if (isActive == false)
            fireAudioSource.Stop();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ActivateSavepoint(true);
    }

    public void LoadData(GameData data)
    {
        bool active = data.unlockedCheckpoints.TryGetValue(checkpointID, out active);
        ActivateSavepoint(active);
    }

    public void SaveData(ref GameData data)
    {
        if (isActive == false) return;

        if (data.unlockedCheckpoints.ContainsKey(checkpointID) == false)
            data.unlockedCheckpoints.Add(checkpointID, true);
    }
    private void OnValidate()
    {
#if UNITY_EDITOR
        if (string.IsNullOrEmpty(checkpointID))
        {
            checkpointID = System.Guid.NewGuid().ToString();
        }
#endif
    }

}
