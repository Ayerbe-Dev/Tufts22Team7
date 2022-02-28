using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public Rigidbody2D body;
	public Collider2D bounds;
	public float health;
	public float max_health;
	public bool facing_right;
    public double frame;
    public double attack_timer;
    public double attack_interval;
    public double walk_speed;
    public move_type move_type;
    public double projectile_speed;
    public double projectile_angle;
    public GameObject[] platforms;
    public statuses status_kind;
    public Projectile projectile;

    // Start is called before the first frame update
    void Start() {
        body = GetComponent<Rigidbody2D> ();
		bounds = GetComponent<Collider2D> ();

    }

    // Update is called once per frame
    void FixedUpdate() {
		frame++;
        if (attack_timer == 0) {
            attack_timer = attack_interval;
            attack();
        }
        else {
            attack_timer--;
        }
    }

    void change_status(statuses new_status_kind) {
		frame = 0.0;
		execute_exit_status();
		status_kind = new_status_kind;
		execute_entry_status();
	}

    void execute_status() {
		switch(status_kind) {

        }
    }

    void execute_entry_status() {
		switch(status_kind) {

        }
    }

    void execute_exit_status() {
		switch(status_kind) {

        }
    }

    void attack() {
        Debug.Log("Attacking!");
        utils.spawn_projectile(projectile, body, move_type, projectile_speed, projectile_angle);
    }
}
