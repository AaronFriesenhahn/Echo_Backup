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

    [Header("Jump Particles")]
    [SerializeField] GameObject FireParticles;
    [SerializeField] GameObject IceParticles;
    [SerializeField] GameObject ElectricParticles;
    [SerializeField] Transform _JumpParticleEmitLocation;

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

    //for hovering
    float targetTime = 2.75f;
    bool isHovering = false;

    bool deathSoundPlayed = false;

    public bool _PlayerDied = false;
    public bool _PlayerGrounded = false;

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
        GameObject RightLeg = GameObject.Find("Player/Echo_Model_With_Upgrades/RightLeg");
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
        GameObject IceParticlesClone = Instantiate(IceParticles, _JumpParticleEmitLocation.position, _JumpParticleEmitLocation.rotation);
    }

    void OnSprintReleased()
    {
        _isSprinting = false;
        sprinting = false;
    }

    void OnFire()
    {
        GameObject RightArm = GameObject.Find("Player/Echo_Model_With_Upgrades/RightArm");
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
            DetectHealth();
            DetectGround();
            PlayerAwake();
        }
        targetTime -= Time.deltaTime;
        if (targetTime <= 0.0f)
        {
            _rigidbody.drag = 0;
        }
        if (targetTime == 0)
        {
            _rigidbody.drag = 0;
        }
        //testing hover particles
        if (isHovering == true)
        {
            GameObject ElectricParticlesClone = Instantiate(ElectricParticles, _JumpParticleEmitLocation.position, _JumpParticleEmitLocation.rotation);
        }
    }

    public void PlayerAwake()
    {
        GameObject LeftEye = GameObject.Find("Player/Echo_Model_With_Upgrades/Head/LeftEye");
        GameObject RightEye = GameObject.Find("Player/Echo_Model_With_Upgrades/Head/RightEye");
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
                    GameObject FireParticlesClone = Instantiate(FireParticles, _JumpParticleEmitLocation.position, _JumpParticleEmitLocation.rotation);
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
                    isHovering = true;
                    AudioHelper.PlayClip2D(_electricHoverSound, 1f);
                    _rigidbody.drag = 20;
                    x = 1;
                    targetTime = 2.75f;
                    
                    StartCoroutine(HoverJumpDelay());                    
                }
            }
            else if (_motor._isGrounded == true)
            {
                _rigidbody.drag = 0;
                isHovering = false;
            }
        }
    }

    public void DetectArmUpgrade(int Upgrade)
    {
        //set different arm upgrade models
        GameObject RightArmModelNormal = GameObject.Find("Player/Echo_Model_With_Upgrades/RightArm/WeaponArm");
        GameObject RightArmModelFire = GameObject.Find("Player/Echo_Model_With_Upgrades/RightArm/WeaponArmFire");
        GameObject RightArmModelIce = GameObject.Find("Player/Echo_Model_With_Upgrades/RightArm/WeaponArmIce");
        GameObject RightArmModelElectric = GameObject.Find("Player/Echo_Model_With_Upgrades/RightArm/WeaponArmElectric");
        GameObject RightArmModelGround = GameObject.Find("Player/Echo_Model_With_Upgrades/RightArm/WeaponArmGround");

        GameObject RightArm = GameObject.Find("Player/Echo_Model_With_Upgrades/RightArm");
        //var RightArmRenderer = RightArm.GetComponent<Renderer>();


        //change right arm to fire
        if (Upgrade == 1)
        {
            RightArmModelNormal.SetActive(false);
            RightArmModelIce.SetActive(false);
            RightArmModelElectric.SetActive(false);
            RightArmModelGround.SetActive(false);
            RightArmModelFire.SetActive(true);

            //RightArmRenderer.material = _FireMaterial;
            FireUpgradeOn = true;
            IceUpgradeOn = false;
            GroundUpgradeOn = false;
            ElectricUpgradeOn = false;
        }
        //change right arm to ice
        else if (Upgrade == 2)
        {
            RightArmModelNormal.SetActive(false);
            RightArmModelIce.SetActive(true);
            RightArmModelElectric.SetActive(false);
            RightArmModelGround.SetActive(false);
            RightArmModelFire.SetActive(false);

            //RightArmRenderer.material = _IceMaterial;
            IceUpgradeOn = true;
            FireUpgradeOn = false;
            GroundUpgradeOn = false;
            ElectricUpgradeOn = false;
        }
        //charge right arm to ground
        else if (Upgrade == 3)
        {
            RightArmModelNormal.SetActive(false);
            RightArmModelIce.SetActive(false);
            RightArmModelElectric.SetActive(false);
            RightArmModelGround.SetActive(true);
            RightArmModelFire.SetActive(false);

            //RightArmRenderer.material = _GroundMaterial;
            GroundUpgradeOn = true;
            FireUpgradeOn = false;
            IceUpgradeOn = false;
            ElectricUpgradeOn = false;
        }
        //change right arm to electric
        else if (Upgrade == 4)
        {
            RightArmModelNormal.SetActive(false);
            RightArmModelIce.SetActive(false);
            RightArmModelElectric.SetActive(true);
            RightArmModelGround.SetActive(false);
            RightArmModelFire.SetActive(false);

            //RightArmRenderer.material = _ElectricMaterial;
            ElectricUpgradeOn = true;
            FireUpgradeOn = false;
            IceUpgradeOn = false;
            GroundUpgradeOn = false;
        }
        //return to normal
        else if (Upgrade == 0)
        {
            RightArm.SetActive(true);

            RightArmModelNormal.SetActive(true);
            RightArmModelIce.SetActive(false);
            RightArmModelElectric.SetActive(false);
            RightArmModelGround.SetActive(false);
            RightArmModelFire.SetActive(false);

            //RightArmRenderer.material = _PlayerAppendageMaterial;
            FireUpgradeOn = false;
            IceUpgradeOn = false;
            GroundUpgradeOn = false;
            ElectricUpgradeOn = false;
        }
    }
    public void DetectLegUpgrade(int Upgrade)
    {
        GameObject RightLegNormal = GameObject.Find("Player/Echo_Model_With_Upgrades/RightLeg/RightLeg/NormalBoot");
        //GameObject RightLeg2 = GameObject.Find("Player/Echo_Model_With_Upgrades/RightLeg/UpperLegR");
        GameObject LeftLegNormal = GameObject.Find("Player/Echo_Model_With_Upgrades/LeftLeg/LeftLeg/NormalBoot");
        //GameObject LeftLeg2 = GameObject.Find("Player/Echo_Model_With_Upgrades/LeftLeg/UpperLegL");
        GameObject RightLegFire = GameObject.Find("Player/Echo_Model_With_Upgrades/RightLeg/RightLeg/FireBoot");
        GameObject RightLegIce = GameObject.Find("Player/Echo_Model_With_Upgrades/RightLeg/RightLeg/IceBoot");
        GameObject RightLegElectric = GameObject.Find("Player/Echo_Model_With_Upgrades/RightLeg/RightLeg/ElectricBoot");
        GameObject LeftLegFire = GameObject.Find("Player/Echo_Model_With_Upgrades/LeftLeg/LeftLeg/FireBoot");
        GameObject LeftLegIce = GameObject.Find("Player/Echo_Model_With_Upgrades/LeftLeg/LeftLeg/IceBoot");
        GameObject LeftLegElectric = GameObject.Find("Player/Echo_Model_With_Upgrades/LeftLeg/LeftLeg/ElectricBoot");

        GameObject RightLeg = GameObject.Find("Player/Echo_Model_With_Upgrades/RightLeg/RightLeg");
        GameObject LeftLeg = GameObject.Find("Player/Echo_Model_With_Upgrades/LeftLeg/LeftLeg");
        var RightLegRenderer = RightLeg.GetComponent<Renderer>();
        var LeftLegRenderer = LeftLeg.GetComponent<Renderer>();

        GameObject RightLegTutorial = GameObject.Find("Player/Echo_Model_With_Upgrades/RightLeg");

        //change legs to fire
        if (Upgrade == 1)
        {
            RightLegNormal.SetActive(false);
            RightLegFire.SetActive(true);
            RightLegIce.SetActive(false);
            RightLegElectric.SetActive(false);
            LeftLegNormal.SetActive(false);
            LeftLegFire.SetActive(true);
            LeftLegIce.SetActive(false);
            LeftLegElectric.SetActive(false);

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
            RightLegNormal.SetActive(false);
            RightLegFire.SetActive(false);
            RightLegIce.SetActive(true);
            RightLegElectric.SetActive(false);
            LeftLegNormal.SetActive(false);
            LeftLegFire.SetActive(false);
            LeftLegIce.SetActive(true);
            LeftLegElectric.SetActive(false);

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
            //didn't model a ground boot so Normal is set to active
            RightLegNormal.SetActive(true);
            RightLegFire.SetActive(false);
            RightLegIce.SetActive(false);
            RightLegElectric.SetActive(false);
            LeftLegNormal.SetActive(true);
            LeftLegFire.SetActive(false);
            LeftLegIce.SetActive(false);
            LeftLegElectric.SetActive(false);

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
            RightLegNormal.SetActive(false);
            RightLegFire.SetActive(false);
            RightLegIce.SetActive(false);
            RightLegElectric.SetActive(true);
            LeftLegNormal.SetActive(false);
            LeftLegFire.SetActive(false);
            LeftLegIce.SetActive(false);
            LeftLegElectric.SetActive(true);

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
            RightLegTutorial.SetActive(true);

            RightLegNormal.SetActive(true);
            RightLegFire.SetActive(false);
            RightLegIce.SetActive(false);
            RightLegElectric.SetActive(false);
            LeftLegNormal.SetActive(true);
            LeftLegFire.SetActive(false);
            LeftLegIce.SetActive(false);
            LeftLegElectric.SetActive(false);

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
        isHovering = false;
        //_rigidbody.drag = 0;
    }

    void DetectGround()
    {
        if (_motor._isGrounded == true)
        {
            x = 0;
            _PlayerGrounded = true;
        }
        else
        {
            _PlayerGrounded = false;
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
