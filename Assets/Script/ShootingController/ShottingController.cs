using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShottingController : MonoBehaviour
{
    public GameObject Bullet;
    public float CoolDownBullet;
    private float ShootColldown;
    public Animator GunAnimator;
    public ParticleSystem MuzzleParticle;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ShootColldown -= Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && GameManager.instance.GameState==GameManager.GameStatus.Running)
        {
            if (ShootColldown <= 0)
            {
                MuzzleParticle.Play();
                   GameObject IstantiatedBullet = Instantiate(Bullet, this.transform);
                IstantiatedBullet.transform.localPosition = this.transform.localPosition;
                IstantiatedBullet.GetComponent<Bullet>().Tag = "Enemy";

                IstantiatedBullet.GetComponent<Bullet>().Originiated = this.gameObject;
                IstantiatedBullet.SetActive(true);
                SoundController.instance.FireSoundPlay();
                GunAnimator.Play(0);
            }

        }

    }

}
