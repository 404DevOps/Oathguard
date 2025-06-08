using UnityEngine;

public class PlayerConfiguration : EntityConfiguration
{
    [Header("Dodge")]
    public float DodgeSpeed = 30f;
    public float DodgeDuration = 0.2f;
    public float DodgeSlowDownDuration = 0.2f;

    public float ControllerActivationThreshhold = 0.1f;
}