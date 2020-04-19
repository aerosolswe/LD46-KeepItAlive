using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionParent : MonoBehaviour {

    public static ExplosionParent instance = null;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != null) {
            Destroy(this.gameObject);
            return;
        }
    }

    private List<Explosion> explosions = new List<Explosion>();

    public Explosion explosionObject;

    public int Amount = 10;

    private void Start() {
        for (int i = 0; i < Amount; i++) {
            Explosion hb = CreateExplosion();
            explosions.Add(hb);
        }
        
    }
    
    public Explosion GetExplosion() {
        foreach (Explosion bullet in explosions) {
            if (!bullet.gameObject.activeInHierarchy) {
                return bullet;
            }
        }

        Explosion e = CreateExplosion();
        explosions.Add(e);

        return e;
    }

    private Explosion CreateExplosion() {
        Explosion hb = Instantiate(explosionObject, transform);
        hb.gameObject.SetActive(false);
        hb.transform.localPosition = Vector3.zero;

        return hb;
    }
}
