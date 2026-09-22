using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlrMovement : MonoBehaviour
{

    [SerializeField] private float plrSpeed = 5.0f;
    [SerializeField] private float acc = 0.5f;
    [SerializeField] private float deacc = 0.5f;
    [SerializeField] private float fireDeacc = 0.5f;
    [SerializeField] private float turnDeAcc = 25f;
    [SerializeField] private float maxSpeed = 6.0f;
    [SerializeField] private float sprintSpeed = 12.0f;
    [SerializeField] private float defSpeed = 12.0f;

    [SerializeField] private LayerMask _testGround;

    [SerializeField] public AudioSource _destroySource;
    [SerializeField] private AudioSource _playerSource;
    [SerializeField] private AudioSource _playerSource2;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource healthSource;
    public List<AudioClip> slingshotMusic = new List<AudioClip>();

    [SerializeField] private AudioClip _slingshot1;
    public List<AudioClip> soundThrow = new List<AudioClip>();
    [SerializeField] private AudioClip _slingshot3;
    public List<AudioClip> _slingshot4 = new List<AudioClip>();
    [SerializeField] private AudioClip _slingshotScream;
    [SerializeField] private AudioClip _JumpSound;
    public List<AudioClip> soundJumpNoise = new List<AudioClip>();

    public List<AudioClip> KickSounds = new List<AudioClip>();
    public List<AudioClip> KillPig = new List<AudioClip>();

    public List<AudioClip> soundList = new List<AudioClip>();
    public List<AudioClip> footstepList = new List<AudioClip>();

    public Rigidbody2D rb;
    public SpriteRenderer sprite;
    public Animator anim;

    public float plrDirection = 1;
    public float plrDirectionY = 1;

    private Vector2 movementInput;
    private float movementMagnitude;

    public Transform currentTrans;

    [SerializeField] private float X = 0;
    [SerializeField] private float Y = 0;
    [SerializeField] private float rayDistance = 5f;
    [SerializeField] private float rayDistance2 = 5f;
    [SerializeField] private float rayDistance3 = 5f;
    [SerializeField] private bool wallColliding;
    [SerializeField] public bool grounded;
    [SerializeField] public bool punching;

    [SerializeField] private bool jumping;
    [SerializeField] private bool holdingJump;

    [SerializeField] private float jumptimer;
    [SerializeField] private float maxjumptimer = 1f;
    [SerializeField] private float JumpForce = 6.0f;
    [SerializeField] private float JumpAddForce = 6.0f;
    [SerializeField] private float JumpMaxForce = 16.0f;
    [SerializeField] private float JumpMinForce = 3.0f;

    [SerializeField] private float SlingShotTime = 0f;
    [SerializeField] private float SlingShotMaxTime = 2f;

    [SerializeField] public GameObject woodParticles;
    [SerializeField] public GameObject impactParticles;
    [SerializeField] public GameObject impactParticles2;

    [SerializeField] private GameObject runSmokeParticles;

    [SerializeField] private Transform Cam;
    [SerializeField] private Transform Cam1;
    [SerializeField] private Transform Cam2;
    [SerializeField] private Transform CamDef;
    [SerializeField] private float CamSpeed = 5f;


    public bool slingshotState = false;

    public bool FiredState = false;
    public bool rolling = false;

    public bool Attacking = false;

    public bool Dead = false;
    public bool GoToTitle = false;

    public float Health = 2f;

    public float PigsKilled = 0;

    public Sprite hbg2;
    public Sprite hbg1;
    public Sprite hbg0;

    public Sprite hnum1;
    public Sprite hnum2;

    public Image healthbg;
    public Image healthnum;

    public bool damaged = false;
    public float damagedtimer = 0f;
    public float damagedmaxtimer = 1f;
    public float score = 0f;

    public bool Useless;

    public Animator animdamagedui;

    public Animator healthui;
    public Animator pigui;


    public GameObject successuiA;
   
    public Animator successui1;
    public Animator successui2;

    public float ScoreForOneStar;
    public float ScoreForTwoStar;
    public float ScoreForThreeStar;

    [SerializeField] private AudioClip okay;

    [SerializeField] private AudioClip highScoreSound;
    [SerializeField] private AudioClip bouncesound;
    [SerializeField] private AudioClip spinSound;
    public List<AudioClip> hitAs = new List<AudioClip>();

    public List<AudioClip> SuccessSounds = new List<AudioClip>();

    public List<TextMeshProUGUI> pigcounter = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> scores = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> livestest = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> newrecordtext = new List<TextMeshProUGUI>();

    public SavedData _dataTest;

    // Start is called before the first frame update
    void Start()
    {
        _dataTest = GameObject.FindGameObjectWithTag("Data").GetComponent<SavedData>();
        rb = GetComponent<Rigidbody2D>();
        PigsKilled = (float)GameObject.FindGameObjectsWithTag("Enemy").Length;
        ChangeText(pigcounter, PigsKilled.ToString("000"));
        ChangeText(livestest, _dataTest.currentLives.ToString("000"));
    }

    void StopTurn()
    {

        anim.SetBool("Turning", false);
    }
    void StopPunch()
    {
        anim.SetBool("Punching", false);
        Attacking = false;
    }
    void StartFall()
    {
        anim.SetBool("Running", false);
        anim.SetBool("Turning", false);
        anim.SetBool("Jumping", false);
        anim.SetBool("Falling", true);
        anim.SetBool("RunJumping", false);
    }
    private int lastPlayedIndex = -1;
    void Footstep()
    {

        if (grounded == true && Useless == false)
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
            _playerSource.PlayOneShot(footstepList[randomIndex]);

            GameObject effect = Instantiate(runSmokeParticles, transform.position, Quaternion.identity);

            ParticleSystemRenderer psRenderer = effect.GetComponent<ParticleSystemRenderer>();

            if (psRenderer != null)
            {
                psRenderer.flip = new Vector3(plrDirection, 0, 0);
            }

            Destroy(effect, .5f);

        }
      
    }


    public void DestroyObj(GameObject obj, string type)
    {
        Attacking = false;
        Destroy(obj);

        GameObject effect2 = Instantiate(impactParticles, obj.transform.position, Quaternion.identity);
        Destroy(effect2, .5f);
        GameObject effect3 = Instantiate(impactParticles2, obj.transform.position, Quaternion.identity);
        Destroy(effect3, .5f);


        if (type == "Block")
        {
            int randomIndex = Random.Range(0, soundList.Count);
            _destroySource.PlayOneShot(soundList[randomIndex]);

            score += 50;
            ChangeText(scores, score.ToString("00000"));

            GameObject effect = Instantiate(woodParticles, obj.transform.position, Quaternion.identity);
            Destroy(effect, 2f);
            
            if (rolling == true)
            {
                plrSpeed -= 5 * Time.deltaTime;
            }

        }

        if (type == "Pig")
        {

         
            PigsKilled -= 1;
            score += 100;
            ChangeText(pigcounter, PigsKilled.ToString("000"));

            ChangeText(scores, score.ToString("00000"));

            int randomIndex1 = Random.Range(0, KickSounds.Count);
            _playerSource.PlayOneShot(KickSounds[randomIndex1]);

            int randomIndex = Random.Range(0, KillPig.Count);
            _destroySource.PlayOneShot(KillPig[randomIndex]);
       


        }


    }



    void CameraWork()
    {
        if (plrSpeed > 0.1)
        {
            if (plrDirection > 0)
            {
                Cam.position = Vector2.Lerp(Cam.position, Cam1.position, CamSpeed * Time.deltaTime);
            }
            else if (plrDirection < 0)
            {
                Cam.position = Vector2.Lerp(Cam.position, Cam2.position, CamSpeed * Time.deltaTime);
            }




        }
        else if (plrSpeed < 0.1)
        {

        

            Cam.position = Vector2.Lerp(Cam.position, CamDef.position, CamSpeed * Time.deltaTime);
        }



    }

    void Attack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            maxSpeed = sprintSpeed;
        }
       else if (Input.GetButtonUp("Fire1"))
        {
            maxSpeed = defSpeed;
        }
        else if (Input.GetAxis("Fire1") > 0.5f)
        {
            maxSpeed = sprintSpeed;
        }
        else if (Input.GetAxis("Fire1") < 0.5f)
        {
            maxSpeed = defSpeed;
        }

        if (Input.GetButtonDown("Fire3"))
        {
            if (rolling == false && Attacking == false && damaged == false && FiredState == false && grounded == true)
            {
                _playerSource.PlayOneShot(spinSound);


                Attacking = true;
                plrSpeed = 20;
                rolling = true;
                anim.SetBool("Turning", false);
                anim.SetBool("Falling", false);
                anim.SetBool("Running", false);
                GameObject effect = Instantiate(runSmokeParticles, transform.position, Quaternion.identity);

                ParticleSystemRenderer psRenderer = effect.GetComponent<ParticleSystemRenderer>();

                if (psRenderer != null)
                {
                    psRenderer.flip = new Vector3(plrDirection, 0, 0);
                }

                Destroy(effect, .5f);

            }

        }

        if (rolling == true && plrSpeed < sprintSpeed)
        {
            rolling = false;
        }
        if (rolling == true && plrSpeed < sprintSpeed)
        {
            rolling = false;
            Attacking = false;
        }
        if (rolling == true && grounded == false)
        {
            rolling = false;
            Attacking = false;
        }

        if (rolling == true) 
        {
            Attacking = true;
           
        }

        if (Attacking == true)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(plrDirection, 0), rayDistance2, _testGround);

            if (hit.collider != null)
            {
              
                if (hit.collider.CompareTag("Block") && (Attacking == true))
                {
                    DestroyObj(hit.collider.gameObject, "Block");
                }
                if (hit.collider.CompareTag("Enemy") && (Attacking == true))
                {
                    Debug.Log("AttackPig");

                    Attacking = false;

                    if (rolling == true)
                    {
                        plrSpeed = defSpeed;


                            rb.velocity = new Vector2(-5 * plrDirection, 5);
                        
                        

                    }


                    if (hit.collider.gameObject.GetComponent<PigBehavior>().hits < 1)
                    {
                        DestroyObj(hit.collider.gameObject, "Pig");
                    }

                    else if (hit.collider.gameObject.GetComponent<PigBehavior>().hits > 0)
                    {
                        int randomIndex1 = Random.Range(0, KickSounds.Count);
                        _playerSource.PlayOneShot(KickSounds[randomIndex1]);

                        hit.collider.gameObject.GetComponent<PigBehavior>().TestyWestyTest();

                        hit.collider.gameObject.GetComponent<PigBehavior>().hits -= 1;
                    }


                }
            }
        }


        if (Input.GetButtonDown("Fire2") && damaged == false && rolling == false && Attacking == false)
        {
            Attacking = true;

            anim.SetBool("Punching", true);
            anim.SetBool("Turning", false);
            int randomIndex = Random.Range(0, hitAs.Count);
           _playerSource.PlayOneShot(hitAs[randomIndex]);

        }

    }
    void Jump()
    {
        if (jumptimer > maxjumptimer && grounded == true)
        {
            jumping = false;
            anim.SetBool("Jumping", false);
            anim.SetBool("Falling", false);
            anim.SetBool("RunJumping", false);
            jumptimer = 0;
        }

        if (holdingJump == true)
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpForce);


            if (JumpForce < JumpMaxForce)
            {
                JumpForce += JumpAddForce * Time.deltaTime;
            }
            if (JumpForce > JumpMaxForce && holdingJump == true) 
            {
                JumpForce = JumpMinForce;
                holdingJump = false;
            }

        }
        if (jumping == true && jumptimer < maxjumptimer)
        {
            jumptimer += 2 * Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (jumping == false && grounded == true)
            {


                anim.SetBool("Turning", false);
            
                if (plrSpeed > .5)
                {
                    if (anim.GetBool("Jumping") == false)
                    {
                       
                        anim.SetBool("Jumping", false);
                        anim.SetBool("RunJumping", true);
                    
                    }
                }
                else if (plrSpeed < .5)
                {

                    if (anim.GetBool("RunJumping") == false)
                    {
                       
                        anim.SetBool("Jumping", true);
                        anim.SetBool("RunJumping", false);
                    }

                }



                int randomIndex = Random.Range(0, soundJumpNoise.Count);
            _playerSource2.PlayOneShot(soundJumpNoise[randomIndex]);

                _playerSource.PlayOneShot(_JumpSound);

                if (FiredState == true)
                {
                    FiredState = false;
                }
                if (rolling == true)
                {
                    rolling = false;
                    Attacking = false;
                }
                jumping = true;
                holdingJump = true;
            }
          
        }
        else if (Input.GetButtonUp("Jump"))
        {
            JumpForce = JumpMinForce;
            holdingJump = false;
        }
    }

    public void ChangeText(List<TextMeshProUGUI> mList, string NewText)
    { 
       
            foreach (TextMeshProUGUI a in mList)
            {
               
                if (a != null)
                {
                    a.text = NewText;
                }
            }
        
       
    }

    public void PlayAllListSounds(List<AudioClip> mList)
    {

        foreach (AudioClip a in mList)
        {

            if (a != null)
            {
                _playerSource.PlayOneShot(a);
            }
        }


    }
    void Win()
    {

        successuiA.SetActive(true);

        pigui.SetBool("End", true);
        healthui.SetBool("End", true);

        successui1.SetBool("Win", true);
        successui2.SetBool("Win", true);

        musicSource.Stop();

        if (_dataTest.highScore < score)
        {
            _playerSource.PlayOneShot(highScoreSound);
            _dataTest.highScore = score;
            Debug.Log("HIGHSCORE!");
            ChangeText(newrecordtext, "NUEVO RECORD!");
        }

       

        PlayAllListSounds(SuccessSounds);

      
    }

    void UI() 
    {
        if (Useless == false)
        {

            if (Health == 1)
            {
                if (healthbg.sprite != hbg1)
                {
                    healthbg.sprite = hbg1;
                    healthnum.sprite = hnum1;
                    healthnum.enabled = true;
                    animdamagedui.SetBool("Damaged", true);
                    healthSource.Play();
                }

            }
            else if (Health == 2)
            {
                if (healthbg.sprite != hbg2)
                {
                    healthbg.sprite = hbg2;
                    healthnum.sprite = hnum2;

                    healthnum.enabled = true;
                    healthSource.Stop();
                    animdamagedui.SetBool("Damaged", false);
                }


            }
            else if (Health == 0)
            {
                if (healthbg.sprite != hbg0)
                {
                    healthSource.Stop();
                    animdamagedui.SetBool("Damaged", false);
                    healthnum.enabled = false;
                    healthbg.sprite = hbg0;
                    if (Dead == false)
                    {
                        Dead = true;
                    }
                }


            }
        }
        else  if (Useless == true)
        {
            if (healthbg.sprite != hbg0)
            {
                healthbg.sprite = hbg0;
                

                healthnum.enabled = false;
                healthSource.Stop();
                animdamagedui.SetBool("Damaged", false);
               
                Win();
            }
        }

    }



    void AnimHandler()
    {
        if (grounded == true)
            if (anim.GetBool("Falling"))
            {

                anim.SetBool("Falling", false);

            }
            else if ((anim.GetBool("Jumping") || anim.GetBool("RunJumping")) && rb.velocity.y < -0.1)
            {
                anim.SetBool("Jumping", false);

                anim.SetBool("RunJumping", false);
            }
        {
            if (plrSpeed > 0.1)
            {
                anim.SetBool("Running", true);
                anim.SetFloat("AnmSpeed", plrSpeed);
            }
            else if (plrSpeed < 0.1)
            {
                anim.SetBool("Running", false);
            }
        }

        if (FiredState == true || rolling == true)
        {
            anim.SetBool("Running", false);
            anim.SetBool("Rolling", true);

            anim.SetBool("Turning", false);
            anim.SetBool("Falling", false);
            anim.SetBool("Jumping", false);
            anim.SetBool("RunJumping", false);
            anim.SetFloat("AnmSpeed", plrSpeed);
        }
        else if (FiredState == false || rolling == false)
        {
            anim.SetBool("Rolling", false);


        }
        if (damaged == true)
        {
            anim.SetBool("Running", false);
            anim.SetBool("Damaged", true);
            anim.SetBool("Rolling", false);
            anim.SetBool("Turning", false);
            anim.SetBool("Falling", false);
            anim.SetBool("Jumping", false);
            anim.SetBool("RunJumping", false);
            anim.SetFloat("AnmSpeed", plrSpeed);
        }
        if (damaged == false && anim.GetBool("Damaged"))
        {
            anim.SetBool("Damaged", false);
        }
     
        if (rb.velocity.y < -0.26 && grounded == false && rolling == false)
        {
            anim.SetBool("Running", false);
            anim.SetBool("Falling", true);
            anim.SetBool("Turning", false);

        }

    }

   
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Block") && (FiredState == true||rolling == true||Attacking == true))
        {



            DestroyObj(collision.gameObject, "Block");

        }
        if (collision.gameObject.CompareTag("Enemy") && (FiredState == true))
        {
            Debug.Log("AttackPig");
            if (collision.gameObject.GetComponent<PigBehavior>().hits < 1)
            {
                DestroyObj(collision.gameObject, "Pig");
            }

            else if (collision.gameObject.GetComponent<PigBehavior>().hits > 0)
            {
                int randomIndex1 = Random.Range(0, KickSounds.Count);
                _playerSource.PlayOneShot(KickSounds[randomIndex1]);

                collision.gameObject.GetComponent<PigBehavior>().TestyWestyTest();

                collision.gameObject.GetComponent<PigBehavior>().hits -= 1;
            }


        }
        else if (collision.gameObject.CompareTag("Enemy") && (FiredState == false|| Attacking == false))
        {

            RaycastHit2D groundhit = Physics2D.Raycast(transform.position, new Vector2(0, -1), rayDistance3, _testGround);
            RaycastHit2D airhit = Physics2D.Raycast(transform.position, new Vector2(0, 1), rayDistance3, _testGround);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(-1, 0), rayDistance3, _testGround);
            RaycastHit2D hit2 = Physics2D.Raycast(transform.position, new Vector2(1, 0), rayDistance3, _testGround);

            if ((groundhit.collider != null && (hit.collider == null && hit2.collider == null && airhit.collider == null)))
            {
                Debug.Log("GOOMBA");



                if (plrSpeed > .5)
                {
                    if (anim.GetBool("Jumping") == false)
                    {

                        anim.SetBool("Jumping", false);
                        anim.SetBool("RunJumping", true);

                    }
                }
                else if (plrSpeed < .5)
                {

                    if (anim.GetBool("RunJumping") == false)
                    {

                        anim.SetBool("Jumping", true);
                        anim.SetBool("RunJumping", false);
                    }

                }

                rb.velocity = new Vector2(rb.velocity.x, 10);

                _playerSource.PlayOneShot(bouncesound);

                if (collision.gameObject.GetComponent<PigBehavior>().hits < 1)
                {
                    DestroyObj(collision.gameObject, "Pig");
                }

                else if (collision.gameObject.GetComponent<PigBehavior>().hits > 0)
                {
                    int randomIndex1 = Random.Range(0, KickSounds.Count);
                    _playerSource.PlayOneShot(KickSounds[randomIndex1]);


                    if (hit2.collider != null)
                    {


                        rb.velocity = new Vector2(-5, 5);
                    }
                    else if (hit.collider != null)
                    {

                        rb.velocity = new Vector2(5, 5);
                    }


                    collision.gameObject.GetComponent<PigBehavior>().hits -= 1;
                }


            }
            else if ((hit.collider != null|| hit2.collider != null || airhit.collider != null) )
            {
                bool test = hit.collider != null && hit.collider.GetComponent<PigBehavior>()?.attacked == false;
                bool test2 = hit2.collider != null && hit2.collider.GetComponent<PigBehavior>()?.attacked == false;

                if (test||test2)
                {
                    if (damaged == false)
                    {
                        Health -= 1;



                        plrSpeed = 0;

                        damaged = true;

                        int randomIndex1 = Random.Range(0, KickSounds.Count);
                        _playerSource.PlayOneShot(KickSounds[randomIndex1]);


                    }




                    if (hit2.collider != null)
                    {


                        rb.velocity = new Vector2(-5, 5);
                    }
                    else if (hit.collider != null)
                    {

                        rb.velocity = new Vector2(5, 5);
                    }
                }
               

            }
       


        }
    }

    private void OnTriggerEnter2D(Collider2D hit)
    {


        if (hit.CompareTag("Launcher") && FiredState == false)
        {

            SlingShotCode scriptObj = hit.GetComponent<SlingShotCode>();

            rb.isKinematic = true;

            jumping = false;

            rb.velocity = new Vector2(0, 0);

            slingshotState = true;

            Transform transformtest = scriptObj.centerpart;

            currentTrans = transformtest;

        }

        if (hit.CompareTag("DeadTrigger"))
        {

            Dead = true;
        }


    }

    float LastY;
    float LastX;

    void Movement()
    {

        if (slingshotState == true)
        {

            Vector2 test = new Vector2((currentTrans.position.x + (X*2)), (currentTrans.position.y + (Y*2)));
            Vector2 test2 = new Vector2((currentTrans.position.x), (currentTrans.position.y));


            if (movementMagnitude > .2)
            {

                LastY = Y;
                LastX = X;
                transform.position = Vector2.MoveTowards(transform.position, test, 4 * Time.deltaTime);

                

                if (SlingShotTime < SlingShotMaxTime)
                {
                    if (SlingShotTime <= 0)
                    {
                        _playerSource.PlayOneShot(_slingshot1);

                    }
                    SlingShotTime += 2 * Time.fixedDeltaTime;
                }

            }

            else if (movementMagnitude < .2)
            {
                if (SlingShotTime > SlingShotMaxTime)
                {



                    rb.velocity = new Vector2(plrSpeed * LastX, 40 * -(LastY));
                    plrDirection = 1;
                    plrSpeed = 40;

                    rb.isKinematic = false;
                    slingshotState = false;
                    FiredState = true;

                    int randomIndex = Random.Range(0, soundThrow.Count);
                    _playerSource2.PlayOneShot(soundThrow[randomIndex]);

                    int randomIndex2 = Random.Range(0, _slingshot4.Count);
                    _playerSource2.PlayOneShot(_slingshot4[randomIndex2]);

                    _playerSource2.PlayOneShot(_slingshotScream);
                    _playerSource2.PlayOneShot(_slingshot3);


                    int randomIndex5 = Random.Range(0, slingshotMusic.Count);
                    musicSource.PlayOneShot(slingshotMusic[randomIndex5]);
                }
                SlingShotTime = 0;

                transform.position = Vector2.MoveTowards(transform.position, test2, 4 * Time.deltaTime);
            }
            
            

        }
    
        if (X != 0 && FiredState == false && rolling == false)
        {
            plrDirection = Mathf.Sign(X);
        }
            if (slingshotState == false)
        {

         

            if (X != 0 && FiredState == false && rolling == false)



            {
                if (plrSpeed < maxSpeed)
                {
                    plrSpeed += acc * Time.fixedDeltaTime;
                }
                if (plrSpeed > maxSpeed)
                {
                    plrSpeed -= deacc * Time.fixedDeltaTime;
                }



                if (X < 0)
                {


                    if (rb.velocity.x > 0.1 || sprite.flipX != true)
                    {
                        if (plrSpeed > 2)
                        {
                            plrSpeed -= turnDeAcc * Time.fixedDeltaTime;
                        }

                        sprite.flipX = true;
                        anim.SetBool("Turning", true);
                    }

                }
                else if (X > 0)
                {


                    if (rb.velocity.x < -0.1 || sprite.flipX != false)
                    {
                        if (plrSpeed > 2)
                        {
                            plrSpeed -= turnDeAcc * Time.fixedDeltaTime;
                        }
                        sprite.flipX = false;
                        anim.SetBool("Turning", true);
                    }


                }

            }
            else if (X == 0)
            {
                if (plrSpeed > 0 && FiredState == false && rolling == false)
                {
                    plrSpeed -= deacc * Time.fixedDeltaTime;
                }
              
                if (plrSpeed < 0)
                {
                    plrSpeed = 0f;
                }

            }

            if (plrSpeed > 0 && (FiredState == true || rolling == true) )
            {
                plrSpeed -= fireDeacc * Time.fixedDeltaTime;
            }


            if (wallColliding == true)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
            }
            else if (wallColliding == false && damaged == false)
            {

                rb.velocity = new Vector2(plrDirection * plrSpeed, rb.velocity.y);
            }
        }

            

    }

    void RaycastFuncs()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(plrDirection, -1), rayDistance, _testGround);
        RaycastHit2D groundhit = Physics2D.Raycast(transform.position, new Vector2(0, -1), rayDistance3, _testGround);
        if (groundhit.collider != null)
        {

            grounded = true;



        }
        else if (groundhit.collider == null)
        {

            grounded = false;
        }


        if (grounded == true)
        {
            hit = Physics2D.Raycast(transform.position, new Vector2(plrDirection, 0), rayDistance, _testGround);
        }
        else if (grounded == false || FiredState == true)
        {
            hit = Physics2D.Raycast(transform.position, new Vector2(plrDirection, -1), rayDistance, _testGround);
        }


        if (hit.collider != null)
        {


            wallColliding = true;
        }
        else if (hit.collider == null)
        {

            wallColliding = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        X = Input.GetAxisRaw("Horizontal");
        Y = Input.GetAxisRaw("Vertical");

        movementInput = new Vector2(X, Y);
        movementMagnitude = movementInput.magnitude;
        
        if (Dead == true)
        {
            musicSource.Stop();
        }
      

        if (damaged == true)
        {
            if (damagedtimer < damagedmaxtimer)
            {
                damagedtimer += 2 * Time.deltaTime;
            }
            else if (damagedtimer > damagedmaxtimer)
            {
                damagedtimer = 0f;
                damaged = false;
            }
        }

        if (PigsKilled < 1)
        {
            if (Useless == false)
            {
                Useless = true;
            }
        }    

        if (Useless == false)
        {
            Attack();
            Jump();
            CameraWork();
            

        }
        else if (Useless == true)
        {
       
            if (Input.GetButtonDown("Jump"))
            {
                GoToTitle = true;
                _playerSource.PlayOneShot(okay);
            }
            if (Input.GetButtonDown("Fire2"))
            {
                Dead = true;
                _playerSource.PlayOneShot(okay);
            }

        }
        UI();
       
    


        
    }
    void FixedUpdate()
    {
        AnimHandler();
        if (Useless == false)
        {
            RaycastFuncs();
            Movement();
        }
        
    }
}
