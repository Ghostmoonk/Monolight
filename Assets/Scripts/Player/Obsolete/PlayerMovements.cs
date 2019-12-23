using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float velocity;
    SpriteRenderer spriteRenderer;
    Animator animator;
    Collider2D attackZone;

    private float attackOffsetY = 0.5f;
    private float attackOffsetX = 0.3f;

    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();
        attackZone = gameObject.GetComponent<PlayerAttack>().attackZone;
        Debug.Log(attackZone);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        #region Movements
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            animator.SetBool("IsRunning", true);

            Vector3 mov = new Vector3(
                Mathf.Clamp(Input.GetAxis("Horizontal"), 0.8f * Input.GetAxisRaw("Horizontal"), 1f * Input.GetAxisRaw("Horizontal")) * velocity * Time.deltaTime,
                Mathf.Clamp(Input.GetAxis("Vertical"), 0.8f * Input.GetAxisRaw("Vertical"), 1f * Input.GetAxisRaw("Vertical")) * velocity * Time.deltaTime);

            transform.position += mov;

            if (Input.GetAxis("Vertical") > 0)
            {
                //Scope+ : Animation de course vers le bas
                attackZone.offset = new Vector2(0f, attackOffsetY);
            }
            else if (Input.GetAxis("Vertical") < 0)
            {
                attackZone.offset = new Vector2(0f, -attackOffsetY);
            }

            if (Input.GetAxis("Horizontal") < 0)
            {
                spriteRenderer.flipX = true;
                attackZone.offset = new Vector2(-attackOffsetX, 0f);
            }
            else if (Input.GetAxis("Horizontal") > 0)
            {
                spriteRenderer.flipX = false;
                attackZone.offset = new Vector2(attackOffsetX, 0f);
            }
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }
        #endregion
    }
}
