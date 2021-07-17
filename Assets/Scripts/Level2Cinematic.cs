using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Animations;

public class Level2Cinematic : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    // public Camera firstCamera;
    public Transform bowl;
    public Transform doorFrame;
    public Transform viewTarget;
    public EventBinder eventBinder;
    public Transform cook;
    public Transform hideout;

    public ParticleSystem playerPs1;
    public ParticleSystem playerPs2;
    public ParticleSystem psIdea;
    public ParticleSystem cookPs;

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

    public float doorPivotDuration = 3f;
    public Ease doorPivotEase = Ease.InOutQuad;
    [Space]

    public float lookAtDuration = 3;
    [Space]

    public float waitFall = 3;
    public float waitCook = 5;
    public float waitEnter = 5;
    public float waitDiscover = 2f;

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
        CameraController controller = playerCamera.GetComponentInParent<CameraController>();
        Save();
        //Disable collisions to prevent triggering cinematic again
        GetComponent<BoxCollider>().enabled = false;
        playerPs1.Play();

        viewTarget.parent = bowl.Find("bowl");
        viewTarget.localPosition = Vector3.zero;
        //enable lookat
        SetLookAt();
        controller.SmoothLookAt(viewTarget.position, lookAtDuration);


        playerCamera.DOFieldOfView(cameraFOV, cameraFOVDuration).SetEase(cameraFOVEase);

        // Rigidbody rb = bowl.GetComponent<Rigidbody>();

        float bowlZ = bowl.position.z;
        bowl.DOMoveZ(bowlZ + .05f, cameraWeightDuration - .2f);

        yield return new WaitForSeconds(waitFall);

        viewTarget.parent = cook.Find("Top");
        viewTarget.DOLocalMove(Vector3.zero, doorPivotDuration).SetEase(doorPivotEase);
        controller.SmoothLookAt(doorFrame.position, lookAtDuration);

        yield return new WaitForSeconds(waitCook);
        
        cook.GetComponent<CustomAgent>().MoveTo(Vector3.zero, -Vector3.forward);

        yield return new WaitForSeconds(waitEnter);

        cook.GetComponent<SeekTarget>().SetTargetView();

        cookPs.Play();
        playerPs2.Play();
        yield return new WaitForSeconds(1);
        UnsetLookAt();
        playerCamera.DOFieldOfView(baseFOV, cameraFOVDuration / 2).SetEase(cameraFOVEase);
        controller.SmoothLookAt(hideout.position, lookAtDuration / 2);
        Invoke("Idea", lookAtDuration / 2);

        yield return new WaitForSeconds(waitDiscover);

        eventBinder.CallEvent("End Cinematic");
        cook.GetComponent<SeekTarget>().enabled = true;
        cook.GetComponent<CookerController>().enabled = true;
        
    }

    void Idea()
    {
        psIdea.Play();
    }

    public void StartCinematic()
    {
        StartCoroutine(Cinematic());
    }


}
