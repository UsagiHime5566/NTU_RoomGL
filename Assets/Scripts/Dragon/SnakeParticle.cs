using UnityEngine;

public class SnakeParticle : MonoBehaviour
{
    [Header("物件設定")]
    public ParticleSystem particle;
    public AudioSource burstAudio;
    public AudioClip burstClip;

    [Header("爆炸參數")]
    public float const_burst_defaultValue = 0;
    public float const_burst_toValue = 5;
    public float const_burst_speed = 0.7f;
    public float const_burst_returnSpeed = 0.35f;

    [Header("Runtimes")]
    [SerializeField] bool bursted = false;
    [SerializeField] float burst_value = 0.1f;
    [SerializeField] ParticleSystem.ShapeModule shapeModule;


    private void Start()
    {
        burst_value = const_burst_defaultValue;
        shapeModule = particle.shape;
    }

    void Update()
    {
        shapeModule.randomPositionAmount = burst_value;

        if (bursted)
        {
            burst_value = Mathf.LerpUnclamped(burst_value, const_burst_toValue, Time.deltaTime * const_burst_speed);
        }
        else
        {
            burst_value = Mathf.Lerp(burst_value, const_burst_defaultValue, Time.deltaTime * const_burst_returnSpeed);
        }
        if (burst_value >= const_burst_toValue - 0.5f)
        {
            bursted = false;
        }
    }
    
    public void BurstActionSwipe()
    {
        if(bursted) return;

        bursted = true;
        burstAudio.PlayOneShot(burstClip);
    }
}
