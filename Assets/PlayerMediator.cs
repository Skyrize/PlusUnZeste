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
    [Space]
    [Header("References")]
    [SerializeField] HealthComponent m_healthComp;
    [SerializeField] CameraShaker m_cameraShakerComp;
    [SerializeField] TweenAnimation m_tweenAnimationComp;
    [SerializeField] AudioComponent m_audioComp;
    [SerializeField] ParticleSystem m_squeezeParticleComp;
    [SerializeField] JumpComponent m_jumpComp;

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

    void OnDeath()
    {

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
        m_healthComp.onLifeUpdate.AddListener(OnLifeUpdate);
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

    // Update is called once per frame
    int test = 0;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            OnDamage(test);
            test += 10;
            if (test > 100)
                test = 0;
        }
    }
}
