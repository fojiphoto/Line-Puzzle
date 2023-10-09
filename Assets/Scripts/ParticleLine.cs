using UnityEngine;
using System.Collections;

public class ParticleLine : MonoBehaviour {

    public Transform endTransform;
    private ParticleSystem particleSystem;

    private float nowAngle = 0.0f;
    void Awake()
    {
        this.particleSystem = this.GetComponentInChildren<ParticleSystem>();
    }
	
	// Update is called once per frame
	void Update () {

        if (endTransform != null)
        {
            Vector3 dirToEnd =
                this.endTransform.position - this.transform.position;
            float length = dirToEnd.magnitude;
            dirToEnd *= 1.0f / length;

            this.transform.rotation = Quaternion.LookRotation(dirToEnd);

            float lifeTime = length / this.particleSystem.startSpeed;

            this.particleSystem.startLifetime = lifeTime;
        }


            this.nowAngle += Random.Range(360, 360) * Time.deltaTime;

            Vector3 dir = new Vector3(
                Mathf.Cos(nowAngle * Mathf.Deg2Rad),
                Mathf.Sin(nowAngle * Mathf.Deg2Rad),
                0.0f);

            this.particleSystem.transform.localPosition =
                dir * Random.Range(0.05f, 0.1f);
      
        
	}
}
