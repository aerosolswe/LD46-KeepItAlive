using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterWeaponBullet : MonoBehaviour {

    private Rigidbody rb;
    private HitInfo hitInfo;

    public float speed = 15;

    public float spread = 10;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    public void Init(HitInfo hitInfo, Vector3 pos, Vector3 dir) {
        transform.forward = dir;
        transform.Rotate(Random.Range(-spread, spread), Random.Range(-spread, spread), 0);

        rb.velocity = transform.forward * speed;
        transform.position = pos;
        this.hitInfo = hitInfo;
    }
    
    private void OnCollisionEnter(Collision collision) {
        Fighter fighter = collision.transform.GetComponent<Fighter>();

        fighter?.TakeDamage(hitInfo);

        Explosion e = ExplosionParent.instance.GetExplosion();
        e.transform.position = transform.position;
        e.gameObject.SetActive(true);

        ResetBullet();
    }

    public void ResetBullet() {
        transform.position = new Vector3(100, 100, 0);
        rb.velocity = Vector3.zero;
        gameObject.SetActive(false);
    }

}
