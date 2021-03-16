using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.Weapon;
using UnityEngine.Events;
public class Sensor : MonoBehaviour
{
    [SerializeField] private Sprite lockDoorSprite;
    [SerializeField] bool bossDoor;
    [SerializeField] WeaponType weaponType;
    [SerializeField] UnityEvent openEvent,closeEvent;
    Sprite defaultSprite;
    public bool BossDoor{get=>bossDoor;
    set{
        bossDoor=value;
        if(bossDoor)spriteRenderer.sprite=lockDoorSprite;
        else spriteRenderer.sprite=defaultSprite;
    }} 
    private SpriteRenderer spriteRenderer;
    private Animator _animator;
    public AudioClip clip;
    public AudioClip clip2;
    public bool close;
    private AudioSource audioClip;
    #region Unity Methods
    private void Awake()
    {
        audioClip = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!bossDoor)
        {
            if (collision.GetComponent<IPlayerWeapon>()!=null &&
                (weaponType==collision.GetComponent<Weapon>().BeamType||
                weaponType==WeaponType.All))
            {
                openEvent.Invoke();
                _animator.SetTrigger("Detect");
            }
        }
    }
    #endregion
    public void Close(){
        closeEvent.Invoke();
    }
    public void AudioOpen()
    {
        audioClip.clip=clip;
        audioClip.Play();
    }
    public void AudioClose()
    {
        audioClip.clip = clip2;
        audioClip.Play();
    }
}
