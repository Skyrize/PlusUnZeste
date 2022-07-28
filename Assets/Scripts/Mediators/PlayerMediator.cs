using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class PlayerMediator : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] string m_onDamageAnimName = "Squeeze";
    [SerializeField] string m_onDamageSoundName = "Squeeze";
    [SerializeField] string m_onJumpAnimName = "Jump";
    [SerializeField] string m_onJumpSoundName = "Jump";
    [SerializeField] AnimationCurve m_squeezeOnDamageCurve;
    [SerializeField] AnimationCurve m_shakeOnDamageCurve;
    [SerializeField] CameraShakeInstance m_baseCamShakeOnDamage;
    [SerializeField] float m_inertiaDecreaseOnWin = 6;
    [Space]
    [Header("References")]
    [SerializeField] HealthComponent m_healthComp;
    [SerializeField] CameraShaker m_cameraShakerComp;
    [SerializeField] TweenAnimation m_tweenAnimationComp;
    [SerializeField] AudioComponent m_audioComp;
    [SerializeField] ParticleSystem m_squeezeParticleComp;
    [SerializeField] JumpComponent m_jumpComp;
    [SerializeField] PlayerController m_playerControllerComp;
    [SerializeField] CameraController m_cameraControllerComp;
    [SerializeField] Rigidbody m_rigidBodyComp;
    [SerializeField] RigidbodyAnimation m_rigidBodyAnimationComp;

    void OnDamage(float _amount)
    {
        m_tweenAnimationComp.Play(m_onDamageAnimName);
        m_squeezeParticleComp.Emit(Mathf.RoundToInt(m_squeezeOnDamageCurve.Evaluate(_amount)));
        m_audioComp.Play(m_onDamageSoundName);

        m_cameraShakerComp.ShakeOnce(
            m_shakeOnDamageCurve.Evaluate(_amount),
            m_baseCamShakeOnDamage.Roughness,
            m_baseCamShakeOnDamage.fadeInDuration,
            m_baseCamShakeOnDamage.fadeOutDuration
        );
    }

    void OnHeal(float _amount)
    {

    }

    void OnLifeUpdate(float _amount)
    {

    }

    void ToggleControllers(bool value)
    {
        m_playerControllerComp.enabled = value;
        m_cameraControllerComp.enabled = value;
        m_jumpComp.enabled = value;
    }

    void OnDeath()
    {
        //Todo : reword when I add new input system
        // PlayerInput.DeactivateInput
        ToggleControllers(false);
    }

    void OnWin()
    {
        m_rigidBodyComp.useGravity = false;
        m_rigidBodyAnimationComp.SlowDown(6);
        ToggleControllers(false);
    }

    void OnRespawn()
    {
        ToggleControllers(true);
        m_rigidBodyComp.isKinematic = false;
    }

    void OnJump()
    {
        m_tweenAnimationComp.Play(m_onJumpAnimName);
        m_audioComp.Play(m_onJumpSoundName);
    }

    void InitializeHealthEvents()
    {
        m_healthComp.onDamageEvent.AddListener(OnDamage);
        m_healthComp.onHealEvent.AddListener(OnHeal);
        m_healthComp.onHealthRatioChanged.AddListener(OnLifeUpdate);
        m_healthComp.onDeathEvent.AddListener(OnDeath);
    }

    void InitializeJumpEvents()
    {
        m_jumpComp.onJump.AddListener(OnJump);
    }

    void Awake()
    {
        InitializeHealthEvents();
        InitializeJumpEvents();
    }
}
