using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeManager : MonoBehaviour
{
    public static SmokeManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public bool isSmoking = false;

    [SerializeField]
    private ParticleSystem ps;

    private float EXHALE_THRESHOLD = 50;

    private float EXHALE_TIMEOUT = 3.0f;

    private TCPServer server;

    private float exhaustRate = 0;

    private float previousExhaustRate = 0;

    public bool canExhale = false;

    private float exhaleStartTime = 9999;

    public void BreatheSmoke()
    {
        canExhale = true;
        exhaleStartTime = Time.time;
    }

    void Start()
    {
        server = TCPServer.instance;
    }

    float Map(float value, float start1, float stop1, float start2, float stop2)
    {
        return start2 + (stop2 - start2) * ((value - start1) / (stop1 - start1));
    }

    void Update()
    {
        var em = ps.emission;
        if (server == null){
            Debug.Log(this.isSmoking);
            if(GameManager.instance.isFired && !this.isSmoking){
                em.rateOverTime = 400;
            }
            else{
                em.rateOverTime = 0;
            }
            return;
        }

        
        float normalizedCo2 = Map((int)(server.GetCo2()), 0, 32000, 0, 400);

        if (normalizedCo2 > EXHALE_THRESHOLD)
        {
            exhaustRate = normalizedCo2;
        }
        else
        {
            exhaustRate = 0;
        }

        if (canExhale && !isSmoking)
        {
            ps.Play();
        }
        if (isSmoking || !canExhale)
        {
            ps.Stop();
        }

        em.rateOverTime = exhaustRate;
        if(Time.time - exhaleStartTime > EXHALE_TIMEOUT){
            canExhale = false;
        }
    }
}
