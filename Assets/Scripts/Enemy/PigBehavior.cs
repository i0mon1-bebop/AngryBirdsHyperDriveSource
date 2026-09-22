using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigBehavior : MonoBehaviour
{

    // Start is called before the first frame update

    public Transform player;
    public SpriteRenderer sprite;
    public Rigidbody2D rb;
    public bool useless = false;
    public float speed = 4f;
    public float jumpForce = 7f;
    public float distance = 25f;
    public Animator anim;
    public bool canMove = true;
    public bool moving = false;
    public bool grounded = true;
    public bool attacked = false;
    public bool movable = false;
    public float attacktimer = 0f;
    public float attackmaxtimer = 1f;

    public float backforce = 16f;
    public float backforce2 = 55f;

    public float movabletimer = 0f;
    public float movablemaxtimer = 1f;

    public float enmDirection = 1;
    public float hits = 3;
    public float currentHits = 3;

    [SerializeField] private AudioSource _enmSource;
    [SerializeField] private AudioSource _enmSource2;
    public List<AudioClip> footstepList = new List<AudioClip>();
    public List<AudioClip> oinkList = new List<AudioClip>();

    [SerializeField] private float rayDistance = 5f;
    [SerializeField] private float rayDistance2 = 5f;
    [SerializeField] private float rayDistance3 = 0.5f;
    [SerializeField] private LayerMask _testGround;
    [SerializeField] private LayerMask _testGround2;

    public PlrMovement testy;



    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        testy = player.GetComponent<PlrMovement>();
        rb = GetComponent<Rigidbody2D>();
       anim = GetComponent<Animator>();
        if (useless == false)
        {
             rb.freezeRotation = true;
        }
        else if (useless == true)
        {
            rb.freezeRotation = false;
        }
    }
    private int lastPlayedIndex = -1;
    void PigFootstep()
    {

        if (grounded == true)
        {
            if (footstepList == null || footstepList.Count == 0) return;

            int randomIndex;


            if (footstepList.Count > 1)
            {
                do
                {
                    randomIndex = Random.Range(0, footstepList.Count);
                }
                while (randomIndex == lastPlayedIndex);
            }
            else
            {
                randomIndex = 0;
            }

            lastPlayedIndex = randomIndex;
            _enmSource2.PlayOneShot(footstepList[randomIndex]);


        }

    }

    public void TestyWestyTest()
    {
        if (testy.Attacking == true)
        {
            testy.Attacking = false;
            attacked = true;

            if (testy.rolling == false)
            {
                rb.velocity = new Vector2(-(enmDirection) * backforce, 5);
            }
            else if (testy.rolling == true)
            {
                rb.velocity = new Vector2(-(enmDirection) * backforce2, 5);
            }

        }
        else if  (testy.Attacking == false)
            {
                
                movable = true;

                if (testy.rolling == false)
                {
                    rb.velocity = new Vector2(-(enmDirection) * backforce, 5);
                }
                else if (testy.rolling == true)
                {
                    rb.velocity = new Vector2(-(enmDirection) * backforce2, 5);
                }

            }
        else if (testy.damaged == false)
        {
            movable = true;
            rb.velocity = new Vector2(-(enmDirection) * backforce, 5);

        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TestyWestyTest();

        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            movable = true;

            if (collision.gameObject.GetComponent<PigBehavior>().attacked == true)
            {
                rb.velocity = new Vector2(-(enmDirection) * backforce, 5);
            }
            else if (collision.gameObject.GetComponent<PigBehavior>().attacked == true)
            {
                rb.velocity = new Vector2(-(enmDirection) * backforce, 5);
            }
      


        }
        if (collision.gameObject.CompareTag("Block") && attacked == true)
        {
            testy.DestroyObj(collision.gameObject.gameObject, "Block");
        }

       
    }
   


    public void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.gameObject.CompareTag("DeadTrigger"))
        {
            testy.PigsKilled -= 1f;

            testy.ChangeText((testy.pigcounter), testy.PigsKilled.ToString("000"));
        }
    }


        // Update is called once per frame
        void FixedUpdate()
    {
        if (useless == false)
        {
            if (player != null)
            {

                if (currentHits != hits)
                {
                    currentHits = hits;
                    attacked = true;
                    rb.velocity = new Vector2(-(enmDirection) * 8, 5);
                }
                if (movable == true)
                {
                    if (movabletimer < movablemaxtimer)
                    {



                        movabletimer += 2 * Time.deltaTime;
                    }
                    else if (movabletimer > movablemaxtimer)
                    {
                        if (grounded == true)

                        {
                            movable = false;

                        }


                    }
                }   
                

                    if (attacked == true)
                {
                    if (attacktimer < attackmaxtimer)
                    { 
              
                        if (rb.freezeRotation == true)
                        {
                            Quaternion test = Quaternion.Euler(0, 0, 25);

                            transform.rotation = Quaternion.Lerp(transform.rotation, test, Time.time * speed);

                            rb.freezeRotation = false;

                        }                            

                      
                        attacktimer += 2 * Time.deltaTime;
                    }
                    else if (attacktimer > attackmaxtimer)
                    {
                        if (grounded == true)

                        {
                            if (rb.freezeRotation == false)
                            {
                                rb.freezeRotation = true;

                                Quaternion test = Quaternion.Euler(0, 0, 0);

                                transform.rotation = Quaternion.Lerp(transform.rotation, test, Time.time * speed);
                            }
                            attacktimer = 0f;
                            attacked = false;

                        }

                        
                    }
                }

                RaycastHit2D groundhit = Physics2D.Raycast(transform.position, new Vector2(0, -1), rayDistance2, _testGround);

                RaycastHit2D hit2 = Physics2D.Raycast(transform.position, new Vector2(enmDirection, 1), rayDistance, _testGround);

                RaycastHit2D wallhit = Physics2D.Raycast(transform.position, new Vector2(enmDirection, 0), rayDistance, _testGround);

                RaycastHit2D plrhit = Physics2D.Raycast(transform.position, new Vector2(enmDirection, 0), rayDistance3, _testGround2);


                RaycastHit2D wallhit2 = Physics2D.Raycast(transform.position, new Vector2(enmDirection, -1), rayDistance, _testGround);

                float directionX = Mathf.Sign(player.position.x - rb.position.x);
                enmDirection = directionX;

                Vector2 distancea = ((Vector2)player.position - rb.position);
                float distancep = distancea.magnitude;

                if (wallhit.collider != null)
                {
                    canMove = false;
                }

                if (plrhit.collider != null)
                {
                    rb.velocity = new Vector2(-(enmDirection) * backforce, 5);
                }

                else if (wallhit.collider == null)
                {
                  
                    canMove = true;
                }
                if (groundhit.collider != null)
                {
                    grounded = true;
                }
                else if (groundhit.collider == null)
                {

                    grounded = false;
                }

                if (wallhit2.collider != null && grounded == false)
                {
                    canMove = false;
                }
                else if (wallhit2.collider == null && grounded == false)
                {

                    canMove = true;
                }


                if (distancep < distance && canMove == true)
                {
                   
                    if (moving == false)
                    {
                        int randomIndex = Random.Range(0, oinkList.Count);
                        _enmSource.PlayOneShot(oinkList[randomIndex]);
                        moving = true;
                    }

                    anim.SetBool("Running", true);
                    if (attacked == false && movable == false)
                    {
                        rb.velocity = new Vector2(directionX * speed, rb.velocity.y);
                    }
                  
                  

                    if (player.position.x > rb.position.x)
                    {
                        sprite.flipX = false;
                    }
                    else if (player.position.x < rb.position.x)
                    {
                        sprite.flipX = true;
                    }

                   

                }
                if (hit2.collider == null && canMove == false && grounded == true && moving == true)
                {
                    grounded = false;
                
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);

                    int randomIndex = Random.Range(0, oinkList.Count);
                    _enmSource.PlayOneShot(oinkList[randomIndex]);

                }




                else if (distancep > distance)
                {
                 
                    if (moving == true)
                    {
                        int randomIndex = Random.Range(0, oinkList.Count);
                        _enmSource.PlayOneShot(oinkList[randomIndex]);
                        anim.SetBool("Running", false);
                        moving = false;
                    }
                }
                else if (canMove == false)
                {
                    if (moving == true)
                    {
                        int randomIndex = Random.Range(0, oinkList.Count);
                        _enmSource.PlayOneShot(oinkList[randomIndex]);
                        anim.SetBool("Running", false);
                        moving = false;
                    }
                }



            }

        }
    }
}
