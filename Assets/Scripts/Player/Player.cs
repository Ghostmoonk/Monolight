using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update

    #region Bindings
    public string harvestControl;
    [SerializeField] private string attackControl;
    public string buildControl;
    #endregion
    #region Components
    SpriteRenderer spriteRenderer;
    Animator animator;

    Monolithe monolithe;

    [SerializeField] GameObject attackObject;

    /*[Tooltip("Le collider de la zone d'attaque")]
    [SerializeField] Collider2D attackZone;
    [Tooltip("Le sprite de la zone d'attaque")]
    [SerializeField] SpriteRenderer attackSprite;*/

    #endregion

    #region Stats
    [SerializeField] float velocity;
    public int damages;
    //Cooldown entre les attaques
    [SerializeField] float attackDuration;
    [SerializeField] float waitBeforeAttack;
    public int maxLife;
    public int lifePoints;
    public int aggressionAmount;
    public bool attackActivate;
    private bool invulnerable = false;
    [SerializeField] private float invulnerabilityPeriod;
    [SerializeField] private float flashDelay;
    #endregion

    private float attackOffsetY = 1f;
    private float attackOffsetX = 1f;

    #region UI
    [SerializeField] private Image healthAmountImage;
    #endregion

    private void Awake()
    {
        /* 
        attackZone = transform.GetComponentInChildren<CapsuleCollider2D>();
        attackSprite = transform.GetComponentInChildren<SpriteRenderer>();
        */
    }

    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();
        lifePoints = maxLife;
        attackObject.SetActive(false);
        attackActivate = false;

        if (harvestControl == "")
        {
            harvestControl = "Harvest";
        }
        if (attackControl == "")
        {
            attackControl = "Attack";
        }
        if (buildControl == "")
        {
            buildControl = "Build";
        }
    }

    private void FixedUpdate()
    {

        #region Movements
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            if (!animator.GetBool("IsRunning"))
                animator.SetBool("IsRunning", true);
            Vector3 mov = new Vector3(
                Mathf.Clamp(Input.GetAxis("Horizontal"), 0.8f * Input.GetAxisRaw("Horizontal"), 1f * Input.GetAxisRaw("Horizontal")) * velocity * Time.deltaTime,
                Mathf.Clamp(Input.GetAxis("Vertical"), 0.8f * Input.GetAxisRaw("Vertical"), 1f * Input.GetAxisRaw("Vertical")) * velocity * Time.deltaTime);

            transform.position += mov;
            //Adapte le sortingLayer quand le joueur bouge
            _MGR_Layer.Instance.SetOrderInLayer(spriteRenderer);
            DeplaceAttackZone();
        }
        else
        {
            if (animator.GetBool("IsRunning"))
                animator.SetBool("IsRunning", false);
        }
        #endregion

    }

    //Deplace la zone d'attaque et pivote le sprite verticalement.
    void DeplaceAttackZone()
    {
        if (attackActivate == false)
        {
            if (Input.GetAxis("Vertical") > 0)
            {
                if (!animator.GetBool("IsBack"))
                {
                    animator.SetBool("IsBack", true);
                }
                attackObject.gameObject.transform.localPosition = new Vector2(0f, attackOffsetY);
                attackObject.gameObject.transform.eulerAngles = new Vector3(0f, 0f, 90f);
            }
            else if (Input.GetAxis("Vertical") < 0)
            {
                //On descend
                if (animator.GetBool("IsBack"))
                {
                    animator.SetBool("IsBack", false);
                }
                attackObject.gameObject.transform.localPosition = new Vector2(0f, -attackOffsetY);
                attackObject.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, -90f));
                attackObject.gameObject.transform.eulerAngles = new Vector3(0f, 0f, 270f);
            }

            if (Input.GetAxis("Horizontal") < 0)
            {
                spriteRenderer.flipX = false;
                attackObject.gameObject.transform.localPosition = new Vector2(-attackOffsetX, 0f);
                attackObject.gameObject.transform.eulerAngles = new Vector3(0f, 0f, 180f);
            }
            else if (Input.GetAxis("Horizontal") > 0)
            {
                spriteRenderer.flipX = true;
                attackObject.gameObject.transform.localPosition = new Vector2(attackOffsetX, 0f);
                attackObject.gameObject.transform.eulerAngles = new Vector3(0f, 0f, 0f);
            }
        }
    }
    private void Update()
    {

        if (Input.GetButtonDown(attackControl))
        {
            Attack();
        }
    }

    //private void DealDamages(Enemy target)
    //{
    //    target.TakesDamages(damages, gameObject);
    //}

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (Input.GetButtonDown("Jump"))
    //    {
    //        Debug.Log("et dis donc là");
    //        if (other.gameObject.tag == "Enemy")
    //        {
    //            Enemy target = other.gameObject.GetComponentInParent<Enemy>();
    //            DealDamages(target);
    //        }
    //    }
    //}

    private void Attack()
    {

        //animator.SetBool("IsAttacking", true);
        if (!attackObject.activeSelf)
            StartCoroutine(nameof(CooldownAttack));
    }

    IEnumerator CooldownAttack()
    {
        attackObject.SetActive(true);
        _MGR_SoundDesign.Instance.PlaySound("Player_Attack");
        attackActivate = true;
        yield return new WaitForSeconds(attackDuration);
        //animator.SetBool("IsAttacking", false);
        attackObject.SetActive(false);
        attackActivate = false;
        yield return new WaitForSeconds(waitBeforeAttack);
        StopCoroutine(CooldownAttack());
    }

    public void TakesDamages(int amount, GameObject source)
    {
        //Debug.Log("Le joueur il se fait martyriser");
        if (!invulnerable)
        {
            _MGR_SoundDesign.Instance.PlaySound("Litos_Damage");
            lifePoints -= amount;
            healthAmountImage.fillAmount = (float)lifePoints / maxLife;
            StartCoroutine(InvulnerableDelay(source));
            if (lifePoints <= 0)
            {
                CaracterDeath();
            }
        }

    }

    IEnumerator InvulnerableDelay(GameObject colliderOther)
    {
        invulnerable = true;
        //Debug.Log("je suis INVINCIBLE");
        StartCoroutine(FlashPlayer());

        GameObject[] AllEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject colliderOtherSame in AllEnemies)
        {
            foreach (Collider2D collider in colliderOtherSame.GetComponentsInChildren<Collider2D>())
                Physics2D.IgnoreCollision(collider, GetComponent<Collider2D>());
        }
        var colorPH = gameObject.GetComponent<SpriteRenderer>().color;
        yield return new WaitForSeconds(invulnerabilityPeriod);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        StopCoroutine(FlashPlayer());
        StopCoroutine(InvulnerableDelay(colliderOther));
        AllEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject colliderOtherSame in AllEnemies)
        {
            foreach (Collider2D collider in colliderOtherSame.GetComponentsInChildren<Collider2D>())
                Physics2D.IgnoreCollision(collider, GetComponent<Collider2D>(), false);
        }
        invulnerable = false;
        //Debug.Log("ah ba non");

    }

    IEnumerator FlashPlayer()
    {
        while (invulnerable == true)
        {
            var colorPH = gameObject.GetComponent<SpriteRenderer>().color;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 0.6f);
            yield return new WaitForSeconds(flashDelay);
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(flashDelay);

        }
    }

    public void CaracterDeath()
    {
        _MGR_SoundDesign.Instance.PlaySound("Litos_Death");
        _MGR_SceneManager.Instance.victory = false;
        _MGR_SceneManager.Instance.LoadScene("End");
        //là tu met ce que tu veux
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("collision avec :" + other.gameObject);
    }

}
