using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Extends InteractButton so that you can collect them by pressing 'e'
public class ObjectiveScorer : MonoBehaviour
{    

    [SerializeField] private AudioClip treefx = null;
    [SerializeField] private bool isFinal = false;
    private Collider mainPlayerCollider = null;
    
    void Start()
    {
        mainPlayerCollider = PlayerMove.MainPlayerCollider;

        // this is so the HUDMenuController knows how many objectives there are
        HUDMenuController.addObjective(isFinal);
    }

    void OnTriggerStay(Collider otherCollider)
    {
        // we can use the colliders from the other purposes too
        if (otherCollider.gameObject.tag != "Player" && otherCollider.transform.parent.gameObject.tag != "Player")
            return;
        if (GameInputManager.getKeyDown(GameInputManager.InputType.Interact))
            externalAction();
    }
    
    void OnTriggerEnter(Collider otherCollider)
    {
        if (otherCollider != mainPlayerCollider)
            return;
        externalAction();
    }

    protected void externalAction()
    {
        AudioSource.PlayClipAtPoint(treefx, gameObject.transform.position, 0.1f);
        ScoreManager.instance.incrementScore(isFinal);
        gameObject.SetActive(false);
    }
}
