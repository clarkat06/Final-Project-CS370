using UnityEngine;

public class PlayerConvo : MonoBehaviour
{
    [SerializeField] float talkDistance = 2;
    bool inConversation;

    public GameObject pausepanel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }

        else if (Input.GetKeyDown(KeyCode.Escape)){
            PauseToggleActive();
        }
    }

    void PauseToggleActive(){
        if (pausepanel.activeInHierarchy) {
            pausepanel.SetActive(false);
        }
        else {
            pausepanel.SetActive(true);
        }
    }

    void Interact()
    {
        Debug.Log("Interact");
        if (inConversation)
        {
            Debug.Log("Skipping Line");
            GameManager.Instance.SkipLine();
        }
        else
        {
            Debug.Log("Looking for NPC");
            RaycastHit2D hit = Physics2D.CircleCast(transform.position, talkDistance, Vector2.up, 0, LayerMask.GetMask("NPC"));
            if (hit)
            {
                Debug.Log("Hit Something!!" + hit.collider.gameObject.name);

                if (hit.collider.gameObject.TryGetComponent(out NPC npc))
                {
                    GameManager.Instance.StartDialogue(npc.dialogueAsset.dialogue, npc.StartPosition, npc.npcName);
                }
            }
        }
    }

    void JoinConversation()
    {
        inConversation = true;
    }

    void LeaveConversation()
    {
        inConversation = false;
    }

    private void OnEnable()
    {
        GameManager.OnDialogueStarted += JoinConversation;
        GameManager.OnDialogueEnded += LeaveConversation;
    }

    private void OnDisable()
    {
        GameManager.OnDialogueStarted -= JoinConversation;
        GameManager.OnDialogueEnded -= LeaveConversation;
    }
}