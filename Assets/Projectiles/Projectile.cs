using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public bool player_owned;
	public Rigidbody2D body;
	public Collider2D bounds;
	public SpriteRenderer sprite;
    public move_type move_type;
    public int facing_dir;
    public bool facing_right;
    public double speed;
    public double angle;
    public int life;
    public GameObject player;
    public GameObject[] enemies;
    public GameObject[] platforms;

    public void init(move_type move_type, double speed, double angle, int active_time, int facing_dir, bool player_owned) {
        this.move_type = move_type;
        this.speed = speed;
        this.angle = angle;
        this.life = active_time;
        this.facing_dir = facing_dir;
        this.player_owned = player_owned;
        this.facing_right = (facing_dir > 0);
    }

    void Start() {
        body = GetComponent<Rigidbody2D> ();
		bounds = GetComponent<Collider2D> ();
		sprite = GetComponent<SpriteRenderer>();
        life = -1;
    }

    void FixedUpdate() {
        process_life();
        update_platforms();
        switch (move_type) {
            case(move_type.MOVE_TYPE_STRAIGHT): {
                move_straight();
            } break;
            default: {

            } break;
        }
        if (player_owned) {
            check_collisions_enemy();
        }
        else {
            check_collisions_player();
        }
        process_render();
    }


	void set_pos(double x, double y) {
		Vector2 move;
		move.x = (float)x;
		move.y = (float)y;
		body.MovePosition(move);
	}

    void add_pos(double angle, double speed) {
        Vector2 move = utils.get_rotated_pos((float)speed, (float)0.0, (float)angle);
        body.MoveRotation((float)angle);
		body.MovePosition(body.position + move * Time.fixedDeltaTime);
    }

    public void move_straight() {
        add_pos(angle, speed * facing_dir);
    }

    void update_platforms() {
        player = GameObject.FindWithTag("Player");
		platforms = GameObject.FindGameObjectsWithTag("Platform");
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
	}

    void process_render() {
		sprite.flipX = !facing_right;
	}

    public void check_collisions_player() {
		if ((body.position.x <= (player.GetComponent<Rigidbody2D>().position.x + (player.GetComponent<BoxCollider2D>().bounds.size.x / 2)) &&
			body.position.x >= (player.GetComponent<Rigidbody2D>().position.x - (player.GetComponent<BoxCollider2D>().bounds.size.x / 2))) && 
			(body.position.y <= (player.GetComponent<Rigidbody2D>().position.y + player.GetComponent<BoxCollider2D>().bounds.size.y / 2) &&
	        body.position.y >= (player.GetComponent<Rigidbody2D>().position.y - player.GetComponent<BoxCollider2D>().bounds.size.y / 2))) {
            process_hit_player();
        }
        else {
            check_collisions_platform();
        }
    }

    public void check_collisions_enemy() {
        bool match = false;
        for (int i = 0; i < enemies.Length; i++) {
            if ((body.position.x <= (enemies[i].GetComponent<Rigidbody2D>().position.x + (enemies[i].GetComponent<BoxCollider2D>().bounds.size.x / 2)) &&
			body.position.x >= (enemies[i].GetComponent<Rigidbody2D>().position.x - (enemies[i].GetComponent<BoxCollider2D>().bounds.size.x / 2))) && 
			(body.position.y <= (enemies[i].GetComponent<Rigidbody2D>().position.y + enemies[i].GetComponent<BoxCollider2D>().bounds.size.y / 2) &&
	        body.position.y >= (enemies[i].GetComponent<Rigidbody2D>().position.y - enemies[i].GetComponent<BoxCollider2D>().bounds.size.y / 2))) {
            process_hit_player();
        }
        }
        if (!match) {
            check_collisions_platform();
        }
    }

    public void check_collisions_platform() {
        for (int i = 0; i < platforms.Length; i++) {
		    if ((body.position.x <= (platforms[i].GetComponent<Rigidbody2D>().position.x - platforms[i].GetComponent<BoxCollider2D>().offset.x + (platforms[i].GetComponent<BoxCollider2D>().bounds.size.x / 2)) &&
				body.position.x >= (platforms[i].GetComponent<Rigidbody2D>().position.x - platforms[i].GetComponent<BoxCollider2D>().offset.x - (platforms[i].GetComponent<BoxCollider2D>().bounds.size.x / 2))) && 
	            (body.position.y <= (platforms[i].GetComponent<Rigidbody2D>().position.y - platforms[i].GetComponent<BoxCollider2D>().offset.y + platforms[i].GetComponent<BoxCollider2D>().bounds.size.y / 2) &&
				body.position.y >= (platforms[i].GetComponent<Rigidbody2D>().position.y - platforms[i].GetComponent<BoxCollider2D>().offset.y - platforms[i].GetComponent<BoxCollider2D>().bounds.size.y / 2))) {
                    process_hit_platform();
    		}
    	}
    }

    void process_life() {
        if (life != -1) {
            life--;
            if (life == 0) {
                despawn(false);
            }
        }
    }

    void process_hit_enemy(int index) {
        Enemy enemy_instance = enemies[index].GetComponent<Enemy>();
        if (enemy_instance.status_kind != statuses.STATUS_KIND_DEAD) {
            enemy_instance.change_status(statuses.STATUS_KIND_HITSTUN);
        }
    }

    void process_hit_player() {
        Player player_instance = player.GetComponent<Player>();
        if (!(player_instance.hit_timer == 0)) {
            player_instance.change_status(statuses.STATUS_KIND_HITSTUN);
        }
        despawn(true);
    }

    void process_hit_platform() {
        if (player_owned) {
            for (int i = 0; i < enemies.Length; i++) {
                Enemy enemy_instance = enemies[i].GetComponent<Enemy>();
                enemy_instance.enrage();
            }
        }
        despawn(true);
    }

    void despawn(bool hit) {
        Destroy(this.gameObject);
    }
}