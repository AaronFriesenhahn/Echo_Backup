using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(Health))]
public class PlayerController : MonoBehaviour
{
    PlayerInput _input = null;
    PlayerMotor _motor = null;

    Rigidbody _rigidbody = null;
    GameManager _gameManager;

    [Header("Player Stats")]
    [SerializeField] float _moveSpeed = .1f;
    [SerializeField] float _jumpStrength = 10f;
    [SerializeField] float _sprintSpeed = 2f;
    //public float health = 100f;
    Health _healthsystem;
    //weaponDamage is public for now, may need to be private
    //to allow weaponDamage to change based on Projectile used
    [SerializeField] public int weaponDamage = 10;

    [Header("Projectiles")]
    [SerializeField] Transform EmitLocation;
    [SerializeField] GameObject PlayerProjectile;
    [SerializeField] GameObject FireProjectile;
    [SerializeField] GameObject IceProjectile;
    [SerializeField] GameObject GroundProjectile;
    [SerializeField] GameObject ElectricProjectile;

    [Header("Projectile Particles")]
    [SerializeField] GameObject FireParticles;

    [Header("Materials")]
    [SerializeField] Material _PlayerAppendageMaterial;
    [SerializeField] Material _FireMaterial;
    [SerializeField] Material _IceMaterial;
    [SerializeField] Material _GroundMaterial;
    [SerializeField] Material _ElectricMaterial;

    [Header("Audio")]
    [SerializeField] AudioClip _WeaponSound;
    [SerializeField] AudioClip _fireWeaponSound;
    [SerializeField] AudioClip _iceWeaponSound;
    [SerializeField] AudioClip _groundWeaponSound;
    [SerializeField] AudioClip _electricWeaponSound;
    [SerializeField] AudioClip _jumpSound;
    [SerializeField] AudioClip _fireJumpSound;
    [SerializeField] AudioClip _iceSprintSound;
    [SerializeField] AudioClip _groundJumpSound;
    [SerializeField] AudioClip _electricHoverSound;
    [SerializeField] AudioClip _hurtSound;
    [SerializeField] AudioClip _DeathSound;
    //moving sound?

    bool FaceDirection = true;

    bool FireUpgradeOn = false;
    bool IceUpgradeOn = false;
    bool GroundUpgradeOn = false;
    bool ElectricUpgradeOn = false;
    bool LegFireUpgradeOn = false;
    bool LegIceUpgradeOn = false;
    bool LegGroundUpgradeOn = false;
    bool LegElectricUpgradeOn = false;

    public bool SecondJumpAbility = true;

    public event Action Sprint = delegate { };

    bool _isSprinting = false;
    bool _isFiring = false;

    //for testing jumping
    int x = 0;

    //for testing particles
    int fireParticles = 0;

    //for testing sprinting
    bool sprinting = false;

    //for testing projectiles
    bool canFire = true;

    //for animation variables
    int rightlegrunning = 0;
    int leftlegrunning = 0;
    

    bool deathSoundPlayed = false;

    public bool _PlayerDied = false;

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _motor = GetComponent<PlayerMotor>();
        _rigidbody = GetComponent<Rigidbody>();
        _healthsystem = GetComponent<Health>();
    }

    private void Start()
    {
        //Debug.Log("Locking cursor.");
        //Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        if (GameManager.GameIsPaused == false)
        {
            _input.MoveInput += OnMove;
            _input.JumpInput += OnJump;
            _input.SprintPressed += OnSprintPressed;
            _input.SprintReleased += OnSprintReleased;
            _input.FireInput += OnFire;
        }
    }

    private void OnDisable()
    {
        if (GameManager.GameIsPaused == false)
        {
            _input.MoveInput -= OnMove;
            _input.JumpInput -= OnJump;
            _input.SprintPressed -= OnSprintPressed;
            _input.SprintReleased -= OnSprintReleased;
            _input.FireInput -= OnFire;
        }
    }

    public void IncreaseHealth(int amount)
    {
        _healthsystem.IncreaseHealth(amount);
    }

    void OnMove(Vector3 movement)
    {
        //incorporate our move speed
        if (_isSprinting == true)
        {
            _motor.Move(movement * _moveSpeed * _sprintSpeed);
            
        }
        else if (_isSprinting == false)
        {
            _motor.Move(movement * _moveSpeed);            
        }
    }
    void OnJump()
    {
        GameObject RightLeg = GameObject.Find("Player/Art/RightLeg");
        if (RightLeg.activeSelf == true)
        {
            Debug.Log("Jumping." + _motor._isGrounded);
            if (_motor._isGrounded == true && x == 0)
            {
                AudioHelper.PlayClip2D(_jumpSound, 1f);
                //apply our jump force to our motor
                _motor.Jump(_jumpStrength);
                DetectSecondJumpAbility();
            }
            else if (_motor._isGrounded == false)
            {
                Debug.Log("Calling Second Jump.");
                //apply second jump capabilities
                DetectSecondJumpAbility();
            }
        }

    }

    void OnSprintPressed()
    {
        if (LegIceUpgradeOn == true && _motor._isGrounded == true)
        {
            if (sprinting == false)
            {
                AudioHelper.PlayClip2D(_iceSprintSound, 1f);
                sprinting = true;
            }
            _isSprinting = true;
            //notify others that we are sprinting (animations, etc.)
            Sprint?.Invoke();
        }
    }

    void OnSprintReleased()
    {
        _isSprinting = false;
        sprinting = false;
    }

    void OnFire()
    {
        GameObject RightArm = GameObject.Find("Player/Art/RightArm");
        if (RightArm.activeSelf == true)
        {
            //apply effects to simulate firing of weapon
            if (GameManager.GameIsPaused == false)
            {
                //check what powerup the player has, THEN instantitate projectile
                //if no powerups detected
                if (FireUpgradeOn == true)
                {
                    if (canFire == true)
                    {
                        canFire = false;
                        GameObject projectile = Instantiate(FireProjectile, EmitLocation.position, EmitLocation.rotation);
                        FireProjectileInDirection(projectile);
                        AudioHelper.PlayClip2D(_fireWeaponSound, 1f);
                        //set weaponDamage?
                        Debug.Log("Fire attack.");
                        weaponDamage = 5;
                        //fire particles
                        GameObject particles = Instantiate(FireParticles, EmitLocation.position, EmitLocation.rotation);

                        StartCoroutine(FireProjectileDelay());
                        StartCoroutine(OnFireCooldown(1f));
                    }

                }
                else if (IceUpgradeOn == true)
                {
                    if (canFire == true)
                    {
                        canFire = false;
                        GameObject projectile = Instantiate(IceProjectile, EmitLocation.position, EmitLocation.rotation);
                        FireProjectileInDirection(projectile);
                        AudioHelper.PlayClip2D(_WeaponSound, 1f);
                        //set weaponDamage?
                        weaponDamage = 20;
                        //ice particles

                        StartCoroutine(IceProjectileDelay());
                        StartCoroutine(OnFireCooldown(1f));
                    }

                }
                else if (GroundUpgradeOn == true)
                {
                    if (canFire == true)
                    {
                        canFire = false;
                        GameObject projectile = Instantiate(GroundProjectile, EmitLocation.position, EmitLocation.rotation);
                        FireProjectileInDirection(projectile);
                        AudioHelper.PlayClip2D(_WeaponSound, 1f);
                        //set weaponDamage?
                        weaponDamage = 1;
                        //ground particles

                        StartCoroutine(GroundProjectileDelay());
                        StartCoroutine(OnFireCooldown(1f));
                    }
                }
                else if (ElectricUpgradeOn == true)
                {
                    if (canFire == true)
                    {
                        canFire = false;
                        GameObject projectile = Instantiate(ElectricProjectile, EmitLocation.position, EmitLocation.rotation);
                        FireProjectileInDirection(projectile);
                        AudioHelper.PlayClip2D(_WeaponSound, 1f);
                        //is a laser beam, so increase bullet speed
                        if (_motor.isFacingRight)
                        {
                            projectile.GetComponent<Rigidbody>().AddForce(transform.right * 500);
                        }
                        else if (!_motor.isFacingRight)
                        {
                            projectile.GetComponent<Rigidbody>().AddForce(transform.right * -500);
                        }
                        //set weaponDamage?
                        weaponDamage = 50;
                        //electric particles

                        StartCoroutine(OnFireCooldown(1f));
                    }

                }
                //no Upgrades
                else
                {
                    if (canFire == true)
                    {
                        canFire = false;
                        GameObject projectile = Instantiate(PlayerProjectile, EmitLocation.position, EmitLocation.rotation);
                        FireProjectileInDirection(projectile);

                        AudioHelper.PlayClip2D(_WeaponSound, 1f);
                        //set weaponDamage?
                        weaponDamage = 10;

                        StartCoroutine(OnFireCooldown(.25f));
                    }
                }
            }
        }
    }

    //checks direction character is facing, then adds force to projectile
    private void FireProjectileInDirection(GameObject projectile)
    {
        if (_motor.isFacingRight)
        {
            projectile.GetComponent<Rigidbody>().AddForce(transform.right * 500);
        }
        else if (!_motor.isFacingRight)
        {
            projectile.GetComponent<Rigidbody>().AddForce(transform.right * -500);
        }
    }

    public void Update()
    {
        if (GameManager.GameIsPaused == false)
        {
            PlayAnimation();
            DetectHealth();
            DetectGround();
        }
    }

    void PlayAnimation()
    {
        //makes sure Player is healthy so animations don't play on death
        if (_healthsystem._currentHealth > 0)
        {
            //detect if firing weapon
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                //play shooting animation? (only affects arms)
                //Debug.Log("Shooting Animation Played.");
            }
            //detect if moving
            else if (Input.GetKey(KeyCode.A) || (Input.GetKey(KeyCode.LeftArrow))
                || (Input.GetKey(KeyCode.D) || (Input.GetKey(KeyCode.RightArrow))))
            {
                if (_motor._isGrounded == true)
                {
                    if (rightlegrunning == 0)
                    {
                        rightlegrunning = 1;
                        //play running animation
                        //import from Maya?
                    }
                    
                    //Debug.Log("Running Animation Played.");
                    if (!Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        //play arm animation
                        //Debug.Log("Moving Arms Animation Played.");
                    }
                }
            }
            //detect if jumping
            else if (_motor._isGrounded == false)
            {
                //play jump animation
                //Debug.Log("Jump/Fall Animation Played.");
                if (SecondJumpAbility == true && Input.GetKeyDown(KeyCode.Space))
                {
                    //Play Second Jump Animation
                    //Debug.Log("Second Jump Animation Played.");
                }
            }
            else
            {
                //play idle animation
                //Debug.Log("Idle Animation Played.");
            }
        }
    }

    public void PlayerAwake()
    {
        GameObject LeftEye = GameObject.Find("Player/Art/Head/LeftEye");
        GameObject RightEye = GameObject.Find("Player/Art/Head/RightEye");
        LeftEye.SetActive(true);
        RightEye.SetActive(true);
    }

    void DetectHealth()
    {
        if (_healthsystem._currentHealth <= 0)
        {
            //make player immobile
            _PlayerDied = true;
            //play death animation?
            
            if (deathSoundPlayed == false)
            {
                AudioHelper.PlayClip2D(_DeathSound, 1f);
                deathSoundPlayed = true;
            }
            //--temporarily respawn player at beginning of sandbox--//

            StartCoroutine(RespawnDelay());
            //_PlayerIsDead = false;
            //------------------------------------------------------//
            //turn screen red and pop up "You Died! Try Again? Y/N" screen
            //reload scene if Y is pressed (use loadscene (current scene index possibly) from gamemanager)
            //quit if N is pressed (use loadscene (main menu) from gamemanager)
        }
    }

    IEnumerator RespawnDelay()
    {
        _healthsystem._currentHealth = _healthsystem._maxHealth;
        deathSoundPlayed = false;
        gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
    }

    void DetectSecondJumpAbility()
    {
        //Debug.Log("Second Jump Called...");
        //detects if Second Jump Ability is acquired and activates
        if (LegFireUpgradeOn == true)
        {
            if (_motor._isGrounded == false)
            {
                //double jump
                if (x == 0)
                {
                    Debug.Log("Second Jump, please!");
                    AudioHelper.PlayClip2D(_fireJumpSound, 1f);
                    //remove momentum
                    _rigidbody.velocity = Vector3.zero;
                    _rigidbody.angularVelocity = Vector3.zero;
                    //add jump force
                    _rigidbody.AddForce(Vector3.up * (_jumpStrength * 1.25f));
                    x = 1;
                }
            }
        }
        else if (LegGroundUpgradeOn == true)
        {
            if (_motor._isGrounded == true)
            {
                //increased jump height
                if (x == 0)
                {
                    Debug.Log("Increased Jump, please!");
                    AudioHelper.PlayClip2D(_groundJumpSound, 1f);
                    _rigidbody.AddForce(Vector3.up * (_jumpStrength / 1.25f));
                    x = 1;
                }
            }
        }
        else if (LegIceUpgradeOn == true)
        {
            //increased movement
        }
        else if (LegElectricUpgradeOn == true)
        {
            if (_motor._isGrounded == false)
            {
                //hover
                if (x == 0)
                {
                    Debug.Log("Hover, please!");
                    AudioHelper.PlayClip2D(_electricHoverSound, 1f);
                    _rigidbody.drag = 20;
                    x = 1;
                    StartCoroutine(HoverJumpDelay());
                }
            }
        }
    }

    public void DetectArmUpgrade(int Upgrade)
    {
        GameObject RightArm = GameObject.Find("Player/Art/RightArm");
        var RightArmRenderer = RightArm.GetComponent<Renderer>();
        if (RightArm.activeSelf == false)
        {
            RightArm.SetActive(true);
        }

        //change right arm to fire
        if (Upgrade == 1)
        {

            RightArmRenderer.material = _FireMaterial;
            FireUpgradeOn = true;
            IceUpgradeOn = false;
            GroundUpgradeOn = false;
            ElectricUpgradeOn = false;
        }
        //change right arm to ice
        else if (Upgrade == 2)
        {
            RightArmRenderer.material = _IceMaterial;
            IceUpgradeOn = true;
            FireUpgradeOn = false;
            GroundUpgradeOn = false;
            ElectricUpgradeOn = false;
        }
        //charge right arm to ground
        else if (Upgrade == 3)
        {
            RightArmRenderer.material = _GroundMaterial;
            FireUpgradeOn = false;
            IceUpgradeOn = false;
            GroundUpgradeOn = false;
            ElectricUpgradeOn = false;
            GroundUpgradeOn = true;
            FireUpgradeOn = false;
            IceUpgradeOn = false;
            ElectricUpgradeOn = false;
        }
        //change right arm to electric
        else if (Upgrade == 4)
        {
            RightArmRenderer.material = _ElectricMaterial;
            ElectricUpgradeOn = true;
            FireUpgradeOn = false;
            IceUpgradeOn = false;
            GroundUpgradeOn = false;
        }
        //return to normal
        else if (Upgrade == 0)
        {
            RightArmRenderer.material = _PlayerAppendageMaterial;
            FireUpgradeOn = false;
            IceUpgradeOn = false;
            GroundUpgradeOn = false;
            ElectricUpgradeOn = false;
        }
    }
    public void DetectLegUpgrade(int Upgrade)
    {
        GameObject RightLeg = GameObject.Find("Player/Art/RightLeg");
        GameObject LeftLeg = GameObject.Find("Player/Art/LeftLeg");
        var RightLegRenderer = RightLeg.GetComponent<Renderer>();
        var LeftLegRenderer = LeftLeg.GetComponent<Renderer>();

        if (RightLeg.activeSelf == false)
        {
            RightLeg.SetActive(true);
        }

        //change legs to fire
        if (Upgrade == 1)
        {
            RightLegRenderer.material = _FireMaterial;
            LeftLegRenderer.material = _FireMaterial;
            LegFireUpgradeOn = true;
            LegIceUpgradeOn = false;
            LegGroundUpgradeOn = false;
            LegElectricUpgradeOn = false;

        }
        //change legs to ice
        else if (Upgrade == 2)
        {
            RightLegRenderer.material = _IceMaterial;
            LeftLegRenderer.material = _IceMaterial;
            LegFireUpgradeOn = false;
            LegIceUpgradeOn = true;
            LegGroundUpgradeOn = false;
            LegElectricUpgradeOn = false;
        }
        //change legs to ground
        else if (Upgrade == 3)
        {
            RightLegRenderer.material = _GroundMaterial;
            LeftLegRenderer.material = _GroundMaterial;
            LegFireUpgradeOn = false;
            LegIceUpgradeOn = false;
            LegGroundUpgradeOn = true;
            LegElectricUpgradeOn = false;
        }
        //change legs to electric
        else if (Upgrade == 4)
        {
            RightLegRenderer.material = _ElectricMaterial;
            LeftLegRenderer.material = _ElectricMaterial;
            LegFireUpgradeOn = false;
            LegIceUpgradeOn = false;
            LegGroundUpgradeOn = false;
            LegElectricUpgradeOn = true;
        }
        //return to normal
        else if (Upgrade == 0)
        {
            RightLegRenderer.material = _PlayerAppendageMaterial;
            LeftLegRenderer.material = _PlayerAppendageMaterial;
            LegFireUpgradeOn = false;
            LegIceUpgradeOn = false;
            LegGroundUpgradeOn = false;
            LegElectricUpgradeOn = false;
        }
    }

    IEnumerator HoverJumpDelay()
    {
        yield return new WaitForSeconds(2.5f);
        Debug.Log("Delay! HoverJump activated.");
        //return drag to normal
        _rigidbody.drag = 0;
    }

    void DetectGround()
    {
        if (_motor._isGrounded == true)
        {
            x = 0;
        }
    }

    IEnumerator OnFireCooldown(float value)
    {
        yield return new WaitForSeconds(value);
        canFire = true;
    }

    IEnumerator FireProjectileDelay()
    {
        yield return new WaitForSeconds(.05f);
        Vector3 offset = new Vector3(0, .5f, 0);
        GameObject projectile = Instantiate(FireProjectile, EmitLocation.position + offset, EmitLocation.rotation);
        FireProjectileInDirection(projectile);
        yield return new WaitForSeconds(.05f);
        projectile = Instantiate(FireProjectile, EmitLocation.position, EmitLocation.rotation);
        FireProjectileInDirection(projectile);
        yield return new WaitForSeconds(.05f);
        projectile = Instantiate(FireProjectile, EmitLocation.position - offset, EmitLocation.rotation);
        FireProjectileInDirection(projectile);
        yield return new WaitForSeconds(.05f);
        projectile = Instantiate(FireProjectile, EmitLocation.position, EmitLocation.rotation);
        FireProjectileInDirection(projectile);
    }

    IEnumerator IceProjectileDelay()
    {
        //fire a projectile upwards
        yield return new WaitForSeconds(.05f);
        Vector3 offset = new Vector3(0, .5f, 0);
        GameObject projectile = Instantiate(IceProjectile, EmitLocation.position + offset, EmitLocation.rotation);
        FireProjectileInDirection(projectile);
        //fire a projectile downwards
        //yield return new WaitForSeconds(.05f);
        projectile = Instantiate(IceProjectile, EmitLocation.position - offset, EmitLocation.rotation);
        FireProjectileInDirection(projectile);
        yield return new WaitForSeconds(.05f);
    }

    IEnumerator GroundProjectileDelay()
    {
        yield return new WaitForSeconds(.05f);
        GameObject projectile = Instantiate(GroundProjectile, EmitLocation.position, EmitLocation.rotation);
        FireProjectileInDirection(projectile);
        AudioHelper.PlayClip2D(_WeaponSound, 1f);
        yield return new WaitForSeconds(.05f);
        projectile = Instantiate(GroundProjectile, EmitLocation.position, EmitLocation.rotation);
        FireProjectileInDirection(projectile);
        AudioHelper.PlayClip2D(_WeaponSound, 1f);
        yield return new WaitForSeconds(.05f);
        projectile = Instantiate(GroundProjectile, EmitLocation.position, EmitLocation.rotation);
        FireProjectileInDirection(projectile);
        AudioHelper.PlayClip2D(_WeaponSound, 1f);
        yield return new WaitForSeconds(.05f);
        projectile = Instantiate(GroundProjectile, EmitLocation.position, EmitLocation.rotation);
        FireProjectileInDirection(projectile);
        AudioHelper.PlayClip2D(_WeaponSound, 1f);
    }

    
}
