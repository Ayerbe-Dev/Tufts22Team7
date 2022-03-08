using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public Rigidbody2D body;
	public Collider2D bounds;
	public SpriteRenderer sprite;
	public string anim_kind;
	public float health;
	public float max_health;
    public int facing_dir;
	public bool facing_right;
    public double frame;
    public double attack_timer;
    public double attack_interval;
    public double walk_timer;
    public double walk_interval;
    public double walk_speed;
    public int rage_timer;
    public int projectile_angle;
    public double rage_mul;
    public bool attacked_in_status;
    public int id;

    public move_type move_type;
    public GameObject[] platforms;
    public statuses status_kind;
    public Projectile projectile;
    public CameraFollowPlayer game_camera;

    // Start is called before the first frame update
    void Start() {
        body = GetComponent<Rigidbody2D> ();
		bounds = GetComponent<Collider2D> ();
        sprite = GetComponent<SpriteRenderer>();
        game_camera = GetComponent<CameraFollowPlayer>();
        rage_mul = 1.0;
        facing_dir = 1;
        facing_right = true;
    }

    // Update is called once per frame
    void FixedUpdate() {
//        if (game_camera.is_on_camera(body.position, bounds.offset)) {
         	frame += rage_mul;
            update_platforms();
            execute_status();
            process_rage_timer();
            process_render();   
 //       }
    }

    public void change_status(statuses new_status_kind) {
		execute_exit_status();
		status_kind = new_status_kind;
		execute_entry_status();
	}

    bool is_anim_end() {
		return (utils.find_sprite(anim_kind, (int)frame) == null);
	}

    void add_pos(double x, double y) {
        Vector2 prev_pos = body.position;
        int prev_plat_index = get_platform_index();
		Vector2 move;
		move.x = (float)x;
		move.y = (float)y;
		body.MovePosition(body.position + move * Time.fixedDeltaTime);
        if (get_platform_index() != prev_plat_index) {
            walk_timer = walk_interval / rage_mul;
            facing_dir *= -1;
            facing_right = !facing_right;
    		body.MovePosition(prev_pos);
        }
	}

	void set_pos(double x, double y) {
		Vector2 move;
		move.x = (float)x;
		move.y = (float)y;
		body.MovePosition(move);
	}

    void update_platforms() {
		platforms = GameObject.FindGameObjectsWithTag("Platform");
	}

    int get_platform_index() {
		for (int i = 0; i < platforms.Length; i++) {
			Vector2 platform_edge1 = utils.get_rotated_pos(
				(float)(platforms[i].GetComponent<Rigidbody2D>().position.x + platforms[i].GetComponent<BoxCollider2D>().offset.x + (platforms[i].GetComponent<BoxCollider2D>().bounds.size.x / 2)),
				(float)(platforms[i].GetComponent<Rigidbody2D>().position.x + platforms[i].GetComponent<BoxCollider2D>().offset.x - (platforms[i].GetComponent<BoxCollider2D>().bounds.size.x / 2)),
				platforms[i].GetComponent<Rigidbody2D>().rotation
			);
		
			Vector2 platform_edge2 = utils.get_rotated_pos(
				(float)(platforms[i].GetComponent<Rigidbody2D>().position.y + platforms[i].GetComponent<BoxCollider2D>().offset.y + platforms[i].GetComponent<BoxCollider2D>().bounds.size.y + .15),
				(float)(platforms[i].GetComponent<Rigidbody2D>().position.y + platforms[i].GetComponent<BoxCollider2D>().offset.y + platforms[i].GetComponent<BoxCollider2D>().bounds.size.y),
				platforms[i].GetComponent<Rigidbody2D>().rotation
			);
			if ((body.position.x <= platform_edge1.x && body.position.x >= platform_edge1.y) && (body.position.y <= platform_edge2.x && body.position.y >= platform_edge2.y)) {
				return i;
			}
		}
		return -1;
	}

    void process_attack_interval() {
        if (status_kind != statuses.STATUS_KIND_ATTACK) {
            if (attack_timer == 0) {
                attack_timer = attack_interval / rage_mul;
                change_status(statuses.STATUS_KIND_ATTACK);
            }
            else {
                attack_timer--;
            }
        }
    }

    void process_walk_interval() {
        if (walk_timer == 0) {
            walk_timer = walk_interval / rage_mul;
            facing_dir *= -1;
            facing_right = !facing_right;
        }
        else {
            walk_timer--;
        }
    }

    void process_rage_timer() {
        if (rage_timer == 0) {
            rage_mul = 1.0;
        }
        else {
            rage_timer--;
        }
    }

    void process_render() {
		sprite.sprite = utils.find_sprite(anim_kind, (int)frame);
		sprite.flipX = !facing_right;
	}

    void change_anim(string name) {
		frame = 0.0;
		anim_kind = "enemy" + id + "_" + name;
	}

	void check_loop_anim() {
		if (is_anim_end()) {
			frame = 0.0;
		}
	}

    void execute_status() {
		switch(status_kind) {
            case (statuses.STATUS_KIND_WALK): {
				status_walk();
			} break;
			case (statuses.STATUS_KIND_HITSTUN): {
				status_hitstun();
			} break;
			case (statuses.STATUS_KIND_DEAD) : {
				status_dead();
			} break;
            case (statuses.STATUS_KIND_ATTACK) : {
                status_attack();
            } break;
            case (statuses.STATUS_KIND_ENRAGED) : {
                status_enraged();
            } break;
			default: {

			} break;
        }
    }

    void execute_entry_status() {
		switch(status_kind) {
            case (statuses.STATUS_KIND_WALK): {
				entry_status_walk();
			} break;
			case (statuses.STATUS_KIND_HITSTUN): {
				entry_status_hitstun();
			} break;
			case (statuses.STATUS_KIND_DEAD) : {
				entry_status_dead();
			} break;
            case (statuses.STATUS_KIND_ATTACK) : {
                entry_status_attack();
            } break;
            case (statuses.STATUS_KIND_ENRAGED) : {
                entry_status_enraged();
            } break;
			default: {

			} break;
        }
    }

    void execute_exit_status() {
		switch(status_kind) {
			case (statuses.STATUS_KIND_WALK): {
				exit_status_walk();
			} break;
			case (statuses.STATUS_KIND_HITSTUN): {
				exit_status_hitstun();
			} break;
			case (statuses.STATUS_KIND_DEAD) : {
				exit_status_dead();
			} break;
            case (statuses.STATUS_KIND_ATTACK) : {
                exit_status_attack();
            } break;
            case (statuses.STATUS_KIND_ENRAGED) : {
                exit_status_enraged();
            } break;
			default: {

			} break;
        }
    }

    void status_walk() {
        process_attack_interval();
        process_walk_interval();
        check_loop_anim();
        add_pos(walk_speed * rage_mul * facing_dir, 0);
    }

    void entry_status_walk() {
        change_anim("walk");
    }

    void exit_status_walk() {

    }

    void status_enraged() {
        if (is_anim_end()) {
            change_status(statuses.STATUS_KIND_WALK);
        }
    }

    void entry_status_enraged() {
        change_anim("enraged");
        rage_timer = 600;
        rage_mul += 0.2;
    }

    void exit_status_enraged() {

    }

    void status_hitstun() {
		if (health == 0) {
			change_status(statuses.STATUS_KIND_DEAD);
		}
		if (is_anim_end()) {
			change_status(statuses.STATUS_KIND_WALK);
		}
	}

	void entry_status_hitstun() {
		health--;
		change_anim("hitstun");
	}

	void exit_status_hitstun() {
		
	}

    void status_attack() {
//        if (frame >= 20.0 && !attacked_in_status) {
            utils.spawn_projectile(projectile, body, move_type, 2.0, 10, 480, facing_dir, false);
            attacked_in_status = true;
//        }
        if (is_anim_end()) {
            change_status(statuses.STATUS_KIND_WALK);
        }
    }

    void entry_status_attack() {
        change_anim("attack");
        attacked_in_status = false;
    }

    void exit_status_attack() {

    }

	void status_dead() {
		if (is_anim_end()) {
            Destroy(this.gameObject);
		}
	}

	void entry_status_dead() {
		change_anim("dead");
	}

	void exit_status_dead() {

	}

    public void enrage() {
//        if (game_camera.is_on_camera(body.position, bounds.offset)) {
            if (status_kind != statuses.STATUS_KIND_ENRAGED) {
                change_status(statuses.STATUS_KIND_ENRAGED);
            }
//        }
    }
}
