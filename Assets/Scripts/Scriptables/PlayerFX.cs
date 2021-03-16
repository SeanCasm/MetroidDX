using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerFX", menuName = "ScriptableObjects/Player/PlayerFX")]
public class PlayerFX : ScriptableObject
{
    [SerializeField]AudioClip[] playerSteps;
    [SerializeField] AudioClip jump, roll, ballJump, morfballed, hyperJumpCharged, hyperJumping, screwAttack, screwAttackLoop;
    public AudioClip Jump{get=>jump;}public AudioClip Roll { get => roll; }
    public AudioClip BallJump { get => ballJump; }public AudioClip Morfballed { get => morfballed; }
    public AudioClip HyperJumpCharged { get => hyperJumpCharged; }public AudioClip HyperJumping { get => hyperJumping; }
    public AudioClip ScrewAttack { get => screwAttack; }public AudioClip ScrewAttackLoop { get => screwAttackLoop; }
    public void PlayRandomStep(AudioSource audioSource){
        audioSource.clip=playerSteps[Random.Range(0,3)];
        audioSource.Play();
    }

}