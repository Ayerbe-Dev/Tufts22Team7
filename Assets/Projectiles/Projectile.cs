using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
	public Rigidbody2D body;
	public Collider2D bounds;
    public move_type move_type;
    public double speed;
    public double angle;
    public GameObject player;
    public GameObject[] platforms;

    public void init(move_type move_type, double speed, double angle) {
        this.move_type = move_type;
        this.speed = speed;
        this.angle = angle;
    }

    void Start() {
        body = GetComponent<Rigidbody2D> ();
		bounds = GetComponent<Collider2D> ();
    }

    void FixedUpdate() {
        update_platforms();
        switch (move_type) {
            case(move_type.MOVE_TYPE_STRAIGHT): {
                move_straight();
            } break;
            default: {

            } break;
        }
        check_collisions();
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
        add_pos(angle, speed);
    }

    void update_platforms() {
        player = GameObject.FindWithTag("Player");
		platforms = GameObject.FindGameObjectsWithTag("Platform");
	}

    public void check_collisions() {
		if ((body.position.x <= (player.GetComponent<Rigidbody2D>().position.x + (player.GetComponent<BoxCollider2D>().bounds.size.x / 2)) &&
			body.position.x >= (player.GetComponent<Rigidbody2D>().position.x - (player.GetComponent<BoxCollider2D>().bounds.size.x / 2))) && 
			(body.position.y <= (player.GetComponent<Rigidbody2D>().position.y + player.GetComponent<BoxCollider2D>().bounds.size.y / 2) &&
	        body.position.y >= (player.GetComponent<Rigidbody2D>().position.y - player.GetComponent<BoxCollider2D>().bounds.size.y / 2))) {
            //Make the player take damage
        }
        else {
            for (int i = 0; i < platforms.Length; i++) {
			    if ((body.position.x <= (platforms[i].GetComponent<Rigidbody2D>().position.x - platforms[i].GetComponent<BoxCollider2D>().offset.x + (platforms[i].GetComponent<BoxCollider2D>().bounds.size.x / 2)) &&
					body.position.x >= (platforms[i].GetComponent<Rigidbody2D>().position.x - platforms[i].GetComponent<BoxCollider2D>().offset.x - (platforms[i].GetComponent<BoxCollider2D>().bounds.size.x / 2))) && 
	                (body.position.y <= (platforms[i].GetComponent<Rigidbody2D>().position.y - platforms[i].GetComponent<BoxCollider2D>().offset.y + platforms[i].GetComponent<BoxCollider2D>().bounds.size.y / 2) &&
					body.position.y >= (platforms[i].GetComponent<Rigidbody2D>().position.y - platforms[i].GetComponent<BoxCollider2D>().offset.y - platforms[i].GetComponent<BoxCollider2D>().bounds.size.y / 2))) {
                    //Despawn the projectile without doing anything else
    			}
    		}
        }
    }
}