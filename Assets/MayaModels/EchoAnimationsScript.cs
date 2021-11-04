using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoAnimationsScript : MonoBehaviour
{
    [SerializeField]
    Animator EchoAnimations;
    [SerializeField]
    PlayerController _player;

    public bool jumpTrigger = false;
    public bool moveTrigger = false;
    public bool playingAnimation = false;
    bool playOnce = false;

    // Start is called before the first frame update
    void Start()
    {
        EchoAnimations = GetComponent<Animator>();
    }

    private void Awake()
    {
        EchoAnimations.SetTrigger("IdleState");
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.anyKeyDown)
        {
            EchoAnimations.SetBool("Moving", false);
            if (jumpTrigger == false)
            {
                EchoAnimations.SetBool("Jumping", false);
            }
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            if (jumpTrigger == false)
            {
                EchoAnimations.SetTrigger("MoveState");
                StartCoroutine(DelayAnimationReplay(0.5f));
            }
            if (_player._PlayerGrounded == false)
            {
                if (playOnce == false)
                {
                    EchoAnimations.SetTrigger("JumpState");
                    playOnce = true;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) && jumpTrigger == false)
        {
            EchoAnimations.SetTrigger("JumpState");
            StartCoroutine(DelayAnimationReplay(0.5f));
        }
        if (_player._PlayerGrounded == true && playingAnimation == false)
        {
            jumpTrigger = false;
            EchoAnimations.SetTrigger("IdleState");
            playOnce = false;
        }
    }

    IEnumerator DelayAnimationReplay(float value)
    {
        playingAnimation = true;
        jumpTrigger = true;
        moveTrigger = true;
        yield return new WaitForSeconds(value);
        moveTrigger = false;
        playingAnimation = false;
    }

    IEnumerator PlayOnce()
    {
        yield return new WaitForSeconds(0.5f);
    }

}
