using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public Rigidbody2D body;
	public Collider2D bounds;
	public SpriteRenderer sprite;
	public float health;
	public float max_health;
    public int facing_dir;
	public bool facing_right;
    public double frame;
    public double attack_timer;
    public double attack_interval;
    public double walk_speed;

    public move_type move_type;
    public GameObject[] platforms;
    public statuses status_kind;
    public Projectile projectile;

    // Start is called before the first frame update
    void Start() {
        body = GetComponent<Rigidbody2D> ();
		bounds = GetComponent<Collider2D> ();
        sprite = GetComponent<SpriteRenderer>();
        facing_dir = 1;
        facing_right = true;
    }

    // Update is called once per frame
    void FixedUpdate() {
		frame++;
        process_attack_interval();
        process_render();
    }

    void change_status(statuses new_status_kind) {
		frame = 0.0;
		execute_exit_status();
		status_kind = new_status_kind;
		execute_entry_status();
	}

    void process_attack_interval() {
        if (attack_timer == 0) {
            attack_timer = attack_interval;
            attack();
        }
        else {
            attack_timer--;
        }
    }

    void process_render() {
		sprite.flipX = !facing_right;
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
        utils.spawn_projectile(projectile, body, move_type, 2.0, 10.0, 480, facing_dir);
    }
}
