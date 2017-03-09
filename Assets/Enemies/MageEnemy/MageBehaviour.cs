﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageBehaviour : MonoBehaviour {
    public GameObject lightningBlast;
    public int health = 20;
    Animator animator;
    Rigidbody2D rig;
    [Range(1, 1000)]
    public int attackProb = 100;
    GameObject player;

    GameObject cloud;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        float distX = player.transform.position.x - transform.position.x;
        if (distX < -0.5 && transform.rotation.eulerAngles.y != 0) {
            //transform.rotation = Quaternion.Euler(0, 0, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 0), 0.2f);
        } else if (distX > 0.5 && transform.rotation.eulerAngles.y != 180) {
            //transform.rotation = Quaternion.Euler(0, 180, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 180, 0), 0.2f);
        }
        if (Random.Range(0, attackProb) == 1 && !animator.GetBool("Attack") && cloud == null && !animator.GetBool("Death")) {
            StartCoroutine(attack());
        }
	}

    public void death() {
        if (!animator.GetBool("Death")) {
            animator.SetBool("Death", true);
            rig.constraints = RigidbodyConstraints2D.None;
            rig.velocity = new Vector2(rig.velocity.x * 3, rig.velocity.y + 30);
            StartCoroutine(destroyOnTime());
        }
    }

    public void hurt() {
        health -= 10;
        if (health <= 0) {
            death();
        }
    }

    IEnumerator attack() {
        animator.SetBool("Attack", true);
        rig.velocity = new Vector2(70 * Mathf.Sign(Random.Range(-2,2)), Random.Range(-5, 5));
        yield return new WaitForSeconds(0.2f);
        cloud = Instantiate(lightningBlast, new Vector2(player.transform.position.x + Random.Range(-3,3), 14), Quaternion.identity);
    }

    IEnumerator destroyOnTime() {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
