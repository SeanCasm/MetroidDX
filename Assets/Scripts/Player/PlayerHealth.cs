using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Player;
using System;

public class PlayerHealth : Health<int>,IDamageable<int>,IFreezeable
{
    #region Properties
    [SerializeField] int energyTanks, maxTotalHealth;
    [SerializeField] Materials materials;
    [SerializeField] BaseData baseData;
    private int healthRound=1;
    private float currentTankSize;
    private PlayerController player;
    private AudioSource audioPlayer;
    private GameData data;
    public AudioClip damageClip;
    public int CurrentMaxTotalHealth{get;set;}
    public int Tanks { get { return energyTanks; } set {energyTanks=value; } }
    public int TotalHealth { get { return healthRound; } set { healthRound = value; } }
    public int MaxTotalHealth { get { return maxTotalHealth; } set{maxTotalHealth=value;} }
    public bool invulnerability { get; set; }public bool freezed { get; set; }
    private bool freezeInvulnerablility;
     
    #endregion
    #region Unity Methods
    private void Awake()
    {
        anim = GetComponent<Animator>();
        _renderer = GetComponentInChildren<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        player = GetComponent<PlayerController>();
        audioPlayer = GetComponent<AudioSource>();
        baseData.SetHealthData(this);
    }
    private void OnEnable() {
        GameEvents.OnRetry -= OnRetry;

        GameEvents.healthTank+=HandleIncrementHealthStats;
        GameEvents.refullAll+=HandleRefullAll;
        GameEvents.OnRetry+=OnRetry;
    }
    private void OnDisable()
    {
        GameEvents.refullAll -= HandleRefullAll;
        GameEvents.healthTank-=HandleIncrementHealthStats;
    }
    #endregion
    #region Public Methods
    public void FreezeMe(){
        if(!freezeInvulnerablility){
            CancelInvoke("Unfreeze");
            StopAllCoroutines();
            Invoke("Unfreeze", 4f);
            _renderer.material = materials.freeze;
            player.Freeze();
            rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
            SetEnableComponentsAtFreeze(false);
            freezed=freezeInvulnerablility = true;
            StartCoroutine(FreezeVisualFeedBack());
        }
    }
    public void Unfreeze()
    {
        Invoke("CanBeFreeze",2.5f);
        _renderer.material = materials.defaultMaterial;
        rb2d.constraints = RigidbodyConstraints2D.None;
        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        SetEnableComponentsAtFreeze(true);
        freezed = false;
    }
    public void LoadHealth(GameData data)
    {
        this.data=data;
        healthRound = data.tanks + 1;
        CurrentMaxTotalHealth=maxTotalHealth = healthRound * 99;
        energyTanks = data.tanks;
        health = 99;
        currentTankSize = 16f * energyTanks;
    }
    private void OnRetry(){
        if(!SaveAndLoad.newGame){
            LoadHealth(data);
        }else{
            baseData.SetHealthData(this);
        }
        GameEvents.playerHealth.Invoke(health, energyTanks);
    }
    /// <summary>
    /// Add a tank to the total tank count and refill the player health
    /// </summary>
    private void HandleIncrementHealthStats()
    {
        energyTanks += 1;
        health = 99;
        CurrentMaxTotalHealth=maxTotalHealth += 99;
        healthRound = energyTanks + 1;
        GameEvents.playerHealth.Invoke(health,energyTanks);
    }
    /// <summary>
    /// Set damage to the player
    /// </summary>
    /// <param name="amount">amount of damage received</param>
    public void AddDamage(int amount)
    {
        if (!player.inSBVelo && !player.Screwing && !invulnerability && !player.HyperJumping)
        {
            invulnerability = true;
            SetDamage(amount);
            Invoke("Vulnerable", 1f);
        }
    }
    public void ConstantDamage(int damageAmount)
    {
        SetDamage(damageAmount);
    }
    /// <summary>
    /// Add health points to player health count
    /// </summary>
    /// <param name="amount">amount of health earned</param>
    public void AddHealth(int amount)
    {
        if (health + amount >= 99 && healthRound == energyTanks + 1)
        {
            health = 99;
        }
        else
        if (health + amount <= 99)
        {
            health = health + amount;
        }
        else
        if (health + amount > 99 && healthRound < energyTanks + 1)
        {
            int healthNext = health + amount - 99;
            health = healthNext;
            healthRound++;
            GameEvents.playerHealth.Invoke(health,energyTanks);
            return;
        }
        CurrentMaxTotalHealth+=amount;
        GameEvents.playerHealth.Invoke(health,energyTanks);
    }

    #endregion
    #region Private Methods
    private void CanBeFreeze(){
        freezeInvulnerablility=false;
    }
    private void SetEnableComponentsAtFreeze(bool value){
        Animator anim=GetComponentInParent<Animator>();
        PlayerInput input=GetComponentInParent<PlayerInput>();
        input.enabled=anim.enabled = value;
    }
    public IEnumerator FreezeVisualFeedBack()
    {
        _renderer.color = _renderer.color.Default();
        yield return new WaitForSeconds(2f);
        while (freezed)
        {
            for (float i = 1; i >= 0.5f; i -= .5f)
            {
                Color color=_renderer.color;
                _renderer.color =color.SetColorRGB(i);
                yield return new WaitForSeconds(0.05f);
            }
            for (float i = 0.5f; i <= 1; i += .5f)
            {
                Color color = _renderer.color;
                _renderer.color = color.SetColorRGB(i);
                yield return new WaitForSeconds(0.05f);
            }
        }
    }
    public void SetConstantDamage(int amount){
        Damage(amount);
    }
    private void SetDamage(int amount)
    {
        Damage(amount);
        audioPlayer.loop=false;
        audioPlayer.ClipAndPlay(damageClip);
    }
    private void Damage(int amount){
        StartCoroutine("VisualFeedBack");
        if (health >= amount)
        {
            health -= amount;
            GameEvents.playerHealth.Invoke(health, energyTanks);
        }
        else
        if (health < amount)
        {
            healthRound--;
            if (healthRound == 0)
            {
                GameEvents.retry.Invoke(true);
                health = 0;
                Pause.onAnyMenu = AudioListener.pause = true;
                _renderer.color = Color.white;
                Time.timeScale = 0f;
                return;
            }
            if (energyTanks > 0) energyTanks--;
            int healthPrev = amount - health;
            health = 99 - healthPrev;
            GameEvents.playerHealth.Invoke(health, energyTanks);
        }
        CurrentMaxTotalHealth -= amount;
    }
    void Vulnerable()
    {
        invulnerability = false;
    }
    private IEnumerator VisualFeedBack()
    {
        _renderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        _renderer.color = Color.white;
    }
    private void HandleRefullAll()
    {
        this.health=99;this.healthRound=energyTanks+1;
        this.CurrentMaxTotalHealth =maxTotalHealth;
        GameEvents.playerHealth.Invoke(health,energyTanks);
    }
    #endregion
}