using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleSystemController : MonoBehaviour
{
    [SerializeField] float ratio = 1;
    [SerializeField] ParticleSystem ps;

    public void EmitRatio(float amount)
    {
        int realAmount = (int)(amount * ratio);
        ps.Emit(realAmount);
    }
}
