using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Anima2D;

public class Player : MonoBehaviour
{

    //Turns on debug view in scene view.
    public bool debug;

    public int playerNum;
    public float walkSpeed;
    public float runSpeed;
    public float jumpSpeed;
    float minWalkInput = 0.2f;
    float minRunInput = 0.5f;
    bool runButton;
    bool actionable;
    Rigidbody2D rb;
    Animator anim;
    public float runStopFrames;
    public bool touchingGround;
    public int directionMod;
    bool crouching;
    bool canAttack;

    public GameObject meshSkeleton;

    //Attack stuff
    public float dashBoostMultiplier;
    bool strong;
    int normals;
    int maxNormals = 3;
    public float normalGrav;
    public float attackGrav = 0;

    //JumpStuff
    Transform footOrigin;
    public float footRaycastDistance;
    float shortHopFrames = 3;
    bool leaping;
    public float leapForce;
    float leapInfluenceMod = 0.25f;

    //Animation stuff;
    public bool walking;
    public bool running;

    //Run Stuff
    public float afterImageTimerCurrent;
    float afterImageTimerMax = 0.1f;

    //RPS Stuff
    public RPS_State currentState;
    public List<SpriteMeshInstance> meshes = new List<SpriteMeshInstance>();
    SpriteMeshAnimation sign;
    SpriteMeshAnimation emblem;
    Color rpsColor;
    Color faded;
    Color dark;
    public float timeInState;

    //Getting Hit/Dying Stuff
    int BurstParticles = 30;
    bool dead;
    bool hit;
    float hitStun;
    float respawnTimer;
    Vector3 respawnPos;

    // Use this for initialization
    void Start()
    {
        actionable = true;
        canAttack = true;
        GetReferences();
        currentState = RPS_State.Basic;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localScale.x > 0)
        {
            directionMod = 1;
        }
        else
        {
            directionMod = -1;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            actionable = false;
            Vector2 testAngle = new Vector2(1, 0);
            float testMagnitude = 10;
            TakeHit(testAngle, testMagnitude);
        }

        if (debug)
        {
            DebugStuff();
        }
        if (!dead)
        {
            HandleAnimations();
            HandleRPSTimer();
            HandleHitStun();
            if (actionable)
            {
                Actions();
            }

            //      if (running) 
            //      {
            //          AfterImageEffect ();
            //      }
            LeapStop();
            if (canAttack)
            {
                AttackControls();
            }

            if (normals >= maxNormals)
            {
                strong = true;
            }
            else
            {
                strong = false;
            }
        }
        else
        {
            Respawn();
        }
    }

    void GetReferences()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        footOrigin = transform.GetChild(0);
        normalGrav = rb.gravityScale;
        respawnPos = transform.position;
        // Gets a reference to every mesh.
        GameObject meshSkeleton = transform.GetChild(1).gameObject;
        for (int i = 0; i < meshSkeleton.transform.childCount; i++)
        {
            GameObject currentMesh = meshSkeleton.transform.GetChild(i).gameObject;
            SpriteMeshInstance smi = currentMesh.GetComponent<SpriteMeshInstance>();
            meshes.Add(smi);
            if (i == 0)
            {
                sign = currentMesh.GetComponent<SpriteMeshAnimation>();
            }
            if (i == 1)
            {
               emblem = currentMesh.GetComponent<SpriteMeshAnimation>();
            }
        }
    }

    //Actions the player can do while actionable
    void Actions()
    {
        if (!hit)
        {
            Move();
            JumpControls();
            //Determines if the run trigger is held
            if (Input.GetAxis("RT_P" + playerNum) == 1)
            {
                runButton = true;
            }
            else
            {
                runButton = false;
            }
        }
    }

    //Controls Player Movement
    void Move()
    {
        //Finds stick input
        float stickInput = Input.GetAxis("LeftStickX_P" + playerNum);
        float stickInputAbs = Mathf.Abs(stickInput);

        if (running)
        {
            if (!runButton || stickInputAbs < minRunInput)
            {
                if (grounded())
                {
                    running = false;
                    StartCoroutine(RunStop(rb.velocity));
                }
            }
        }

        //Determines movement properties on the ground. Changes values based on whether or not the run button is held.
        if (leaping)
        {
            float horiontalInfluence = stickInput * leapInfluenceMod;
            float modX = rb.velocity.x;
            modX += horiontalInfluence;
            rb.velocity = new Vector2(modX, rb.velocity.y);
        }
        else
        {
            if (runButton)
            {
                if (stickInputAbs > minRunInput)
                {
                    rb.velocity = new Vector2(stickInput * runSpeed, rb.velocity.y);
                    running = true;
                    walking = false;
                }
            }
            else
            {
                if (stickInputAbs > minWalkInput)
                {
                    rb.velocity = new Vector2(stickInput * walkSpeed, rb.velocity.y);
                    walking = true;
                    running = false;
                }
                else
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                    walking = false;
                }
            }
        }

        //Makes sure character is facing the right way
        if (stickInput > 0 && transform.localScale.x < 0 && !leaping)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
        if (stickInput < 0 && transform.localScale.x > 0 && !leaping)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
    }

    //Controls Jumping
    void JumpControls()
    {
        if (Input.GetButtonDown("AButton_P" + playerNum) && grounded())
        {
            //          Jump (jumpSpeed, running);
            anim.SetTrigger("Jump");
        }

        //Slows vertical momentum when button is released during jump.
        if (Input.GetButtonUp("AButton_P" + playerNum) && !grounded() && rb.velocity.y > 0)
        {
            StartCoroutine(StuntJump(rb.velocity));
        }
    }

    //Function that actually makes the character jump.
    void Jump(float jumpforce, bool leap)
    {
        leaping = leap;
        if (leap)
        {
            rb.velocity = new Vector2(rb.velocity.x * leapForce, jumpforce);
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpforce);
        }
    }

    //Go into runstop after landing in a leap
    void LeapStop()
    {
        if (leaping && grounded() && rb.velocity.y <= 0)
        {
            leaping = false;
            //StartCoroutine (RunStop (rb.velocity));
        }
    }

    //Attack Controls
    void AttackControls()
    {
        if (Input.GetButtonDown("XButton_P" + playerNum))
        {
            //Increments normals so that it goes into strong attack;
            normals++;

            //Reads stick input so we know direction.
            float stickInputX = Input.GetAxis("LeftStickX_P" + playerNum);
            float stickInputY = Input.GetAxis("LeftStickY_P" + playerNum);
            //Makes it so the player can switch directions while attacking.
            if (stickInputX > 0 && transform.localScale.x < 0 && !leaping)
            {
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            }
            if (stickInputX < 0 && transform.localScale.x > 0 && !leaping)
            {
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            }

            if (!grounded())
            {
                rb.gravityScale = attackGrav;
            }

            string attackInput = RPSX.Input(stickInputX, stickInputY, directionMod, grounded(), running, crouching, leaping, strong);
            Attack hitbox = GetComponent<AttackMoveset>().GetAttack(attackInput);
            hitbox.stickInputX = stickInputX;
            hitbox.stickInputY = stickInputY;
            hitbox.directionMod = directionMod;
            hitbox.playersHit.Clear();

            //Sends info to the animator to tell it what attack animation to do.
            anim.SetTrigger(attackInput);
            if (leaping)
            {
                leaping = false;
            }
        }
    }

    //RPS Controls
    public void ChangeRPSState(RPS_State rps)
    {
        currentState = rps;
        timeInState = 0;
        int frame = 0;
        if (rps == RPS_State.Scissors)
        {
            frame = 3;
            rpsColor = RPSX.scissorsColor;
            faded = RPSX.scissorsColorFaded;
            dark = RPSX.scissorsColorDark;
        }
        else if (rps == RPS_State.Rock)
        {
            frame = 1;
            rpsColor = RPSX.rockColor;
            faded = RPSX.rockColorFaded;
            dark = RPSX.rockColorDark;
        }
        else if (rps == RPS_State.Paper)
        {
            frame = 2;
            rpsColor = RPSX.paperColor;
            faded = RPSX.paperColorFaded;
            dark = RPSX.paperColorDark;
        }
        else
        {
            frame = 0;
            rpsColor = RPSX.basicColor;
            faded = RPSX.basicColorFaded;
            dark = RPSX.basicColor;
        }

        foreach (SpriteMeshInstance mesh in meshes)
        {
            mesh.color = rpsColor;
        }

        sign.frame = frame;
        emblem.frame = frame;
    }

    void HandleRPSTimer()
    {
        if (currentState != RPS_State.Basic)
        {
            timeInState += Time.deltaTime;
        }

        if (timeInState > PlayerManager.maxStateTime)
        {
            ChangeRPSState(RPS_State.Basic);
        }
    }

    //Handles dying
    public void KillCharacter(Color color)
    {

        GameObject burst = Instantiate(Resources.Load("Prefabs/PlayerBurst")) as GameObject;
        burst.transform.position = transform.position;
        ParticleController pc = burst.GetComponent<ParticleController>();
        pc.burst(color, BurstParticles);
        Camera.main.gameObject.GetComponent<ScreenShaker>().shake(10);
        dead = true;
        PlayerManager.TakeDamage(playerNum - 1);
        transform.position = PlayerManager.deadzone;
        ChangeRPSState(RPS_State.Basic);
    }

    //Called when the player is hit by an attack.
    public void TakeHit (Vector2 angle, float magnitude)
    {
        Vector2 knockback = angle * magnitude;
        rb.velocity = knockback;
        rb.gravityScale = (normalGrav/2);
        Camera.main.GetComponent<ScreenShaker>().shake(10);
        bool hit = true;
        hitStun = 1;
        rb.drag = 3;
    }

    //Handles hitstun
    public void HandleHitStun ()
    {
        if (hitStun > 0)
        {
            hitStun -= Time.deltaTime;
        }
        if (hitStun <= 0)
        {
            hitStun = 0;
            hit = false;
            rb.gravityScale = normalGrav;
            rb.drag = 0;
        }
    }

    //Functionality that dictates how the player respawns after dying.
    public void Respawn()
    {
        respawnTimer += Time.deltaTime;
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        if (respawnTimer >= PlayerManager.respawnTime)
        {
            rb.isKinematic = false;
            transform.position = respawnPos;
            dead = false;
            respawnTimer = 0;
        }
    }


    //Coroutine used to determine runstop slide;
    IEnumerator RunStop(Vector2 endingRunSpeed)
    {
        actionable = false;
        float currentFrame = 0.0f;
        anim.SetTrigger("RunStop");
        do
        {
            rb.velocity = Vector2.Lerp(endingRunSpeed, Vector2.zero, currentFrame / runStopFrames);
            currentFrame++;
            yield return null;
        }
        while (currentFrame <= runStopFrames);

        actionable = true;
    }

    //Coroutine used for short hops.
    IEnumerator StuntJump(Vector2 endingJump)
    {
        float currentFrame = 0.0f;
        Vector2 stopJump = new Vector2(rb.velocity.x, 0);
        do
        {
            rb.velocity = Vector2.Lerp(endingJump, stopJump, currentFrame / shortHopFrames);
            currentFrame++;
            yield return null;
        }
        while (currentFrame <= shortHopFrames);
    }

    //Used to stop momentum during an air attack.
    IEnumerator AirAttackSmooting(Vector2 endingMomentum, float frames)
    {
        float currentFrame = 0.0f;
        do
        {
            rb.velocity = Vector2.Lerp(endingMomentum, Vector2.zero, currentFrame / frames);
            currentFrame++;
            yield return null;
        }
        while (currentFrame <= shortHopFrames);
    }


    //Keeps track of if the player is touching the ground or not
    bool grounded()
    {
        RaycastHit2D below = Physics2D.Raycast(footOrigin.transform.position, Vector2.down, footRaycastDistance);
        if (below.collider != null)
        {
            //Debug.Log (below.collider.gameObject.name);
            return below.transform.gameObject.tag == "Floor";
        }
        else
        {
            return false;
        }
    }

    //Sends appropriate information to the animator.
    void HandleAnimations()
    {
        anim.SetBool("Walking", walking);
        anim.SetBool("Running", running);
        anim.SetFloat("VerticalVelocity", rb.velocity.y);
        anim.SetBool("Grounded", grounded());
        anim.SetBool("Leaping", leaping);
    }

    //Animation Events
    public void ActionsON()
    {
        actionable = true;
        ResetNormals();
    }

    public void ActionsOFF()
    {
        actionable = false;
    }

    public void doJump()
    {
        Jump(jumpSpeed, running);
    }

    public void AttackingON()
    {
        canAttack = true;
    }

    public void AttackingOFF()
    {
        canAttack = false;
    }

    public void stopHorizontalMomentum()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
        Debug.Log("Stop");
    }

    public void airAttackStop(float frames)
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        StopCoroutine(AirAttackSmooting(rb.velocity, frames));
        StartCoroutine(AirAttackSmooting(rb.velocity, frames));
    }

    public void ResetNormals()
    {
        normals = 0;
    }

    public void ResetGravity()
    {
        rb.gravityScale = normalGrav;
    }

    public void RunStop()
    {
        actionable = false;
        float slidingSpeed = rb.velocity.x;
        DOTween.To(() => slidingSpeed, x => slidingSpeed = x, 0, 1);
        rb.velocity = new Vector2(slidingSpeed, rb.velocity.y);
    }

    public void DashAttackSlide()
    {
        running = false;
        float slidingSpeed = rb.velocity.x * dashBoostMultiplier;
        DOTween.To(() => slidingSpeed, x => slidingSpeed = x, 0, 0.5f);
        rb.velocity = new Vector2(slidingSpeed, rb.velocity.y);
    }

    //Used to default out all variable values.
    public void ResetValues()
    {
        leaping = false;
        running = false;
        walking = false;
        rb.gravityScale = normalGrav;
        normals = maxNormals;
    }

    //Used for visualizing Raycasts and such.
    void DebugStuff()
    {
        Vector3 footEndPoint = new Vector3(footOrigin.position.x, footOrigin.position.y - footRaycastDistance, footOrigin.position.z);
        Debug.DrawLine(footOrigin.transform.position, footEndPoint, Color.green);

        //      Vector3 frontEndPoint = new Vector3 (frontOrigin.position.x + frontRaycastDistance * directionModifier, frontOrigin.position.y, frontOrigin.position.z);
        //      Debug.DrawLine (frontOrigin.transform.position, frontEndPoint , Color.red);
        //
        //      Vector3 backEndPoint = new Vector3 (backOrigin.position.x - backRaycastDistance * directionModifier, backOrigin.position.y, backOrigin.position.z);
        //      Debug.DrawLine (backOrigin.transform.position, backEndPoint , Color.blue);
    }

    //Temp effect to show when running
    //  void AfterImageEffect()
    //  {
    //      afterImageTimerCurrent += Time.deltaTime;
    //
    //      if (afterImageTimerCurrent >= afterImageTimerMax) 
    //      {
    //          afterImageTimerCurrent = 0;
    //          GameObject afterImage = Instantiate (Resources.Load ("Prefabs/AfterImage")) as GameObject;
    ////            SpriteRenderer afterImageSR = afterImage.GetComponent<SpriteRenderer> ();
    //          afterImage.transform.position = this.transform.position;
    ////            afterImageSR.sprite = sr.sprite;
    ////            afterImageSR.flipX = sr.flipX;
    ////            afterImageSR.color = new Color (sr.color.r, sr.color.g, sr.color.b, 0.75f);
    //      }
    //  }
}
