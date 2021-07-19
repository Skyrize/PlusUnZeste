using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Animations;
using UnityEngine.UI;

public class Level1Cinematic : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public Rigidbody playerBody;
    public Transform knife;
    public Transform window;
    public Transform viewTarget;
    public EventBinder eventBinder;
    public ParticleSystem psShocked;
    public ParticleSystem psDrop;
    public ParticleSystem psEscape;
    public ParticleSystem psHit;
    public ParticleSystem psIdea;
    public Image blackScreen;
    [Space]
    [Header("Animations")]
    float baseFOV = 0;
    public float cameraFOV = 60f;
    public float cameraFOVDuration = 1.3f;
    public Ease cameraFOVEase = Ease.InOutQuad;
    [Space]

    public float cameraWeightDuration = 1.3f;
    public Ease cameraWeightEase = Ease.InOutQuad;
    ConstraintSource source;
    [Space]

    public float windowDuration = 3f;
    public Ease windowEase = Ease.InOutQuad;

    [Space]
    public Vector3 playerJump = Vector3.one;
    public float blackScreenFadeDuration = 3;
    public float lookAtKnifeDuration = 1;
    public float knifeYFall = 1;
    public float knifeFallTimer = .3f;
    public float recoverTimer = 3f;
    public float dropTimer = 1f;
    public float lookAtWindowDuration = 1f;

    
    void SetLookAt()
    {
        var lookat = playerCamera.GetComponent<LookAtConstraint>();
        // lookat.rotationAtRest = playerCamera.transform.rotation.eulerAngles;
        source.weight = 1;
        source.sourceTransform = viewTarget;
        lookat.constraintActive = true;
        lookat.AddSource(source);
        DOTween.To(() => lookat.weight, x => lookat.weight = x, 1, cameraWeightDuration).SetEase(cameraWeightEase);
    }

    void UnsetLookAt()
    {
        var lookat = playerCamera.GetComponent<LookAtConstraint>();
        // lookat.rotationAtRest = playerCamera.transform.rotation.eulerAngles;
        DOTween.To(() => lookat.weight, x => lookat.weight = x, 0, cameraWeightDuration / 2).SetEase(cameraWeightEase);

    }

    void Save()
    {
        baseFOV = playerCamera.fieldOfView;
    }

    IEnumerator Cinematic()
    {
        eventBinder.CallEvent("Start Cinematic");
        CameraController controller = playerCamera.GetComponentInParent<CameraController>();
        Save();

        viewTarget.parent = knife;
        viewTarget.localPosition = Vector3.zero;

        blackScreen.DOFade(0, blackScreenFadeDuration).SetEase(Ease.InExpo);
        yield return new WaitForSeconds(blackScreenFadeDuration);

        //enable lookat
        SetLookAt();
        controller.SmoothLookAt(viewTarget.position, lookAtKnifeDuration);
        playerCamera.DOFieldOfView(cameraFOV + 10, cameraFOVDuration / 2).SetEase(cameraFOVEase);

        yield return new WaitForSeconds(knifeFallTimer);
        float knifeY = knife.position.y;
        float dist = knifeY - knifeYFall;
        float time = Mathf.Sqrt((2f * dist) / Mathf.Abs(Physics.gravity.y)) / 1.5f;
        knife.DOMoveY(dist, time).SetEase(Ease.InExpo);

        yield return new WaitForSeconds(time);
        psHit.Play();
        psShocked.Play();
        playerBody.GetComponent<JumpComponent>().Jump(playerJump);

        yield return new WaitForSeconds(recoverTimer);
        playerBody.GetComponent<RigidbodyAnimation>().SlowDown();
        psDrop.Play();
        yield return new WaitForSeconds(dropTimer);

        viewTarget.parent = window;
        viewTarget.DOLocalMove(Vector3.zero, lookAtWindowDuration).SetEase(cameraWeightEase);
        controller.SmoothLookAt(window.position, lookAtWindowDuration);
        yield return new WaitForSeconds(lookAtWindowDuration);
        psIdea.Play();
        yield return new WaitForSeconds(psIdea.main.duration);
        playerCamera.DOFieldOfView(cameraFOV - 45, cameraFOVDuration).SetEase(Ease.OutBack);
        
        yield return new WaitForSeconds(cameraFOVDuration);

        UnsetLookAt();
        playerCamera.DOFieldOfView(baseFOV, cameraFOVDuration / 2).SetEase(Ease.InExpo);
        yield return new WaitForSeconds(cameraFOVDuration / 2);
        psEscape.Play();
        // controller.SmoothLookAt(hideout.position, lookAtDuration / 2);


        eventBinder.CallEvent("End Cinematic");
        
    }

    void Start()
    {
        StartCoroutine(Cinematic());
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(knife.position, knife.position - Vector3.up * knifeYFall);
        Gizmos.DrawRay(playerBody.transform.position, playerJump);
    }

}
