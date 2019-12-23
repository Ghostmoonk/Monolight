using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public enum State
    {
        ATTACKING, MOVING, DYING, SPAWNING
    }

    #region BecauseTheCodeIs à l'arrache
    private bool playerAtRange;
    #endregion
    #region UI
    [SerializeField] Image healthImg;
    #endregion
    #region Components

    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Collider2D enemyCollider;
    [SerializeField] Collider2D attackArea;
    // objet pour déterminer les collisions
    [SerializeField] Animator animator;
    #endregion

    #region Enemy stats

    //La vie max de l'ennemi
    public int maxLife = 100;
    //La vie actuelle de l'ennemi
    public int pointLife;
    [SerializeField] private float speed;
    [SerializeField] private int damageDealed;
    public State state;
    [SerializeField] float attackCooldown;
    [SerializeField] float durationFollowing;
    public int percent;
    public float delayDamageSeen;
    public float attackDodgedTime;

    #endregion

    #region AI related
    public GameObject[] arr_target;
    public GameObject currentTarget;
    [SerializeField] private int rageGauge;
    [HideInInspector] public int countArrayTarget = 0;
    private bool enemyInRange = false;
    bool chasePlayer;
    bool isAttackingNexus;
    bool canAttack;
    private GameObject objectDeterCol;

    Monolithe monolithe;
    Player player;
    #endregion

    [SerializeField] private float durationInSpawn;

    public List<GameObject> targetsAtRangeList;

    void Start()
    {
        targetsAtRangeList = new List<GameObject>();
        _MGR_Enemies.Instance.list_enemies.Add(this);
        pointLife = maxLife;
        currentTarget = arr_target[0];
        state = State.SPAWNING;
        StartCoroutine(DurationSpawning());
        objectDeterCol = new GameObject();
        //Possibilité d'animation de spawn
        /*
        state = State.SPAWNING;
        StartCoroutine(SpawnDelay());
        
         */

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        monolithe = GameObject.FindGameObjectWithTag("Monolithe").GetComponent<Monolithe>();
        playerAtRange = false;
        chasePlayer = false;
        canAttack = true;
    }

    /*IEnumerator SpawnDelay(){

        animator.SetBool("IsSpawning", true);
        yield return new WaitForSeconds(1f);
        animator.SetBool("IsSpawning", false);
        state = State.MOVING;

    }
    */

    private void MoveTo(GameObject target)
    {
        Vector3 dir = target.transform.position - transform.position;
        transform.position += dir.normalized * speed * Time.deltaTime;

        if (dir.x > 0)
        {
            spriteRenderer.flipX = true;
            enemyCollider.gameObject.transform.localScale = new Vector3(-1f, 1f, 1f);
            attackArea.gameObject.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (dir.x < 0)
        {
            spriteRenderer.flipX = false;
            enemyCollider.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
            attackArea.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    void FixedUpdate()
    {
        if (monolithe.lifePoints > 0)
        {
            if (!animator.GetBool("IsAttacking") && targetsAtRangeList.Count == 0)
            {
                MoveTo(currentTarget);
            }

            if (targetsAtRangeList.Count > 0 && canAttack)
            {
                StartCoroutine(AttackAtRangeDelay());
            }
        }
        else
        {
            ChangeTarget(player.gameObject);
            MoveTo(currentTarget);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "turret")
        {
            if (collision.gameObject.GetComponent<Turret>().isBuild == true)
            {
                // GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                // state = State.ATTACKING;
            }
            else if (countArrayTarget < arr_target.Length - 1 && objectDeterCol != collision.gameObject)
            {
                countArrayTarget++;
            }
            currentTarget = arr_target[countArrayTarget];
            objectDeterCol = collision.gameObject;
        }

        if (collision.gameObject.tag == "Monolithe")
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            //state = State.ATTACKING;
        }

        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponentInParent<Player>().TakesDamages(damageDealed, gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("wtf ?????" + objectDeterCol + "et ça" + collision.gameObject);
        if (collision.gameObject.tag == "wayPoint" && currentTarget.tag != "Player" && objectDeterCol != collision.gameObject)
        {
            if (countArrayTarget < arr_target.Length - 1)
            {
                countArrayTarget++;
            }
            currentTarget = arr_target[countArrayTarget];
            objectDeterCol = collision.gameObject;

        }

        if (collision.GetComponent<Collider2D>() != null)
        {
            if (collision.gameObject.tag == "turret")
            {
                if (collision.gameObject.GetComponentInParent<Turret>().isBuild && collision.gameObject.GetComponentInParent<Turret>().lifePoints > 0)
                {
                    if (!collision.GetComponent<Collider2D>().isTrigger)
                    {
                        targetsAtRangeList.Add(collision.gameObject);
                        //state = State.ATTACKING;
                        //enemyInRange = true;
                        //StartCoroutine(DelayAttacking(collision));
                    }
                }
            }
            if (collision.gameObject.tag == "Monolithe" && !collision.isTrigger)
            {
                targetsAtRangeList.Add(collision.gameObject);
                //state = State.ATTACKING;
                //enemyInRange = true;
                //StartCoroutine(DelayAttacking(collision));
            }
            if (collision.gameObject.tag == "Player")
            {
                playerAtRange = true;
                targetsAtRangeList.Add(collision.gameObject);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (targetsAtRangeList.Contains(other.gameObject))
        {
            targetsAtRangeList.Remove(other.gameObject);
        }

        if (other.gameObject.tag == "Player")
        {
            playerAtRange = false;
        }
        if (other.gameObject.tag == "Monolithe" && !chasePlayer)
        {
            animator.SetBool("IsAttacking", false);
            // state = State.MOVING;
            // animator.SetBool("IsAttacking", false);
            // StopCoroutine(DelayAttacking(other));
        }
        if (other.gameObject.tag == "turret" && !chasePlayer)
        {
            animator.SetBool("IsAttacking", false);
        }
    }

    public void DestroyThis()
    {
        _MGR_SoundDesign.Instance.PlaySound("Monster_Death");
        _MGR_Enemies.Instance.list_enemies.Remove(this);
        Destroy(this.gameObject);
    }

    public void TakesDamages(int amount, GameObject source)
    {
        _MGR_SoundDesign.Instance.PlaySound("Monster_Damage");
        pointLife -= amount;
        healthImg.fillAmount = (float)pointLife / maxLife;
        if (pointLife <= 0)
        {
            DestroyThis();
        }

        StartCoroutine(DamageTaken());
        if (source.tag == "Player")
        {
            if (pointLife < maxLife * percent / 100)
            {
                StartCoroutine(DurationFollowing(source));
            }
        }
    }

    public void ChangeTarget(GameObject newTarget)
    {
        currentTarget = newTarget;
    }

    IEnumerator DamageTaken()
    {
        for (int i = 0; i < 2; i++)
        {
            Color baseColor = spriteRenderer.color;
            spriteRenderer.color = new Color(spriteRenderer.color.r + 0.5f, spriteRenderer.color.g + 0.5f, spriteRenderer.color.b + 0.5f, 1f);
            yield return new WaitForSeconds(delayDamageSeen);
            spriteRenderer.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(delayDamageSeen);
        }
        StopCoroutine(DamageTaken());

    }

    IEnumerator AttackAtRangeDelay()
    {
        //Attaque lancée
        canAttack = false;
        _MGR_SoundDesign.Instance.PlaySound("Monster_Attack");
        animator.SetBool("IsAttacking", true);
        yield return new WaitForSeconds(attackDodgedTime);
        //Les cibles n'étant pas Exit trigger sont dans la liste
        for (int i = 0; i < targetsAtRangeList.Count; i++)
        {
            if (targetsAtRangeList[i].GetComponent<Player>() != null)
            {
                targetsAtRangeList[i].GetComponent<Player>().TakesDamages(damageDealed, gameObject);
            }
            else if (targetsAtRangeList[i].GetComponent<Turret>() != null)
            {
                targetsAtRangeList[i].GetComponent<Turret>().TakesDamages(damageDealed, this);
            }
            else if (targetsAtRangeList[i].GetComponent<Monolithe>())
            {
                targetsAtRangeList[i].GetComponent<Monolithe>().TakesDamages(damageDealed);
            }
        }
        yield return new WaitForSeconds(attackCooldown);
        //L'ennemi peut réattaquer
        canAttack = true;
        StopCoroutine(AttackAtRangeDelay());
    }

    IEnumerator DurationFollowing(GameObject other)
    {
        ChangeTarget(other);
        chasePlayer = true;
        yield return new WaitForSeconds(durationFollowing);
        currentTarget = arr_target[countArrayTarget];
        chasePlayer = false;
    }

    IEnumerator DurationSpawning()
    {

        yield return new WaitForSeconds(durationFollowing);
        animator.SetBool("IsAttacking", false);
        state = State.MOVING;
        StopCoroutine(DurationSpawning());
    }
}

