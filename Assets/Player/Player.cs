using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public enum statuses {
        STATUS_KIND_JUMP_SQUAT,
        STATUS_KIND_JUMP,
        STATUS_KIND_HITSTUN,
        STATUS_KIND_LANDING,
    };

    public enum situations {
        SITUATION_KIND_GROUND,
        SITUATION_KIND_AIR,
    };

    public Rigidbody2D body; //Unity uses this for physics, positions etc.
    public float health;
    public float max_health;
    public float ammo; //idk what ammo looks like for jokes but we should prob include it
    public float max_ammo;
    public float reticle_angle; //Gonna use this for aiming 
    public statuses status_kind; //The state the player is in (Jumping, taking damage, etc.)
    public situations situation_kind; //If the player is in the air or on the ground
    public double frame; //The frame of the player's current animation
    public double air_speed; //The player's vertical speed
    public double gravity; //How much the player's vertical speed drops by on every frame

    public double max_fall_speed; //How fast the player can be falling before gravity stops having an effect


    // Start is called before the first frame update
    void Start() {
        health = max_health;  
        ammo = max_ammo;
    }

    // Update is called once per frame
    void Update() {
        frame++;
        process_aim();
        process_inputs();
        execute_status();
    }

    bool can_act() {
        if (status_kind == statuses.STATUS_KIND_JUMP_SQUAT
        || status_kind == statuses.STATUS_KIND_HITSTUN) {
            return false;
        }
        return true;
    }

    void process_aim() {
        reticle_angle = Input.GetAxisRaw("Vertical");
    }

    void process_inputs() {
        if (can_act()) {
            if (Input.GetButton("D")) {
                add_pos(1.0, 0.0);
            }
            if (Input.GetButton("A")) {
                add_pos(-1.0, 0.0);
            }
            if (situation_kind == situations.SITUATION_KIND_GROUND) {
                if (Input.GetButton("Space")) {
                    change_status(statuses.STATUS_KIND_JUMP_SQUAT);
                }
            }
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
            case (statuses.STATUS_KIND_JUMP): {
                status_jump();
            } break;
            case (statuses.STATUS_KIND_JUMP_SQUAT) : {
                status_jumpsquat();
            } break;
            case (statuses.STATUS_KIND_HITSTUN): {
                status_hitstun();
            } break;
            case (statuses.STATUS_KIND_LANDING) : {
                status_landing();
            } break;
            default: {

            } break;
        }
    }

    void execute_entry_status() {
            switch(status_kind) {
            case (statuses.STATUS_KIND_JUMP): {
                entry_status_jump();
            } break;
            case (statuses.STATUS_KIND_JUMP_SQUAT) : {
                entry_status_jumpsquat();
            } break;
            case (statuses.STATUS_KIND_HITSTUN): {
                entry_status_hitstun();
            } break;
            case (statuses.STATUS_KIND_LANDING) : {
                entry_status_landing();
            } break;
            default: {

            } break;
        }
    }

    void execute_exit_status() {
        switch(status_kind) {
            case (statuses.STATUS_KIND_JUMP): {
                exit_status_jump();
            } break;
            case (statuses.STATUS_KIND_JUMP_SQUAT) : {
                exit_status_jumpsquat();
            } break;
            case (statuses.STATUS_KIND_HITSTUN): {
                exit_status_hitstun();
            } break;
            case (statuses.STATUS_KIND_LANDING) : {
                exit_status_landing();
            } break;
            default: {

            } break;
        }
    }

    void add_pos(double x, double y) {
        Vector2 move;
        move.x = (float)x;
        move.y = (float)y;
        body.MovePosition(body.position + move * Time.fixedDeltaTime);
    }
    
    void shoot() {

    }

    /*

  /$$$$$$  /$$$$$$$$ /$$$$$$  /$$$$$$$$ /$$   /$$  /$$$$$$         /$$$$$$   /$$$$$$  /$$$$$$$  /$$$$$$ /$$$$$$$  /$$$$$$$$ /$$$$$$ 
 /$$__  $$|__  $$__//$$__  $$|__  $$__/| $$  | $$ /$$__  $$       /$$__  $$ /$$__  $$| $$__  $$|_  $$_/| $$__  $$|__  $$__//$$__  $$
| $$  \__/   | $$  | $$  \ $$   | $$   | $$  | $$| $$  \__/      | $$  \__/| $$  \__/| $$  \ $$  | $$  | $$  \ $$   | $$  | $$  \__/
|  $$$$$$    | $$  | $$$$$$$$   | $$   | $$  | $$|  $$$$$$       |  $$$$$$ | $$      | $$$$$$$/  | $$  | $$$$$$$/   | $$  |  $$$$$$ 
 \____  $$   | $$  | $$__  $$   | $$   | $$  | $$ \____  $$       \____  $$| $$      | $$__  $$  | $$  | $$____/    | $$   \____  $$
 /$$  \ $$   | $$  | $$  | $$   | $$   | $$  | $$ /$$  \ $$       /$$  \ $$| $$    $$| $$  \ $$  | $$  | $$         | $$   /$$  \ $$
|  $$$$$$/   | $$  | $$  | $$   | $$   |  $$$$$$/|  $$$$$$/      |  $$$$$$/|  $$$$$$/| $$  | $$ /$$$$$$| $$         | $$  |  $$$$$$/
 \______/    |__/  |__/  |__/   |__/    \______/  \______/        \______/  \______/ |__/  |__/|______/|__/         |__/   \______/ 

    */

    void status_jump() {
        if (air_speed > max_fall_speed) {
            air_speed -= gravity;
        }
        add_pos(0.0, air_speed);
        //if (we collide with a platform or the ground from the top) {
        //  change_status(statuses.STATUS_KIND_LANDING);
        //}
    }

    void entry_status_jump() {
        situation_kind = situations.SITUATION_KIND_AIR;
        air_speed = 40.0;
    }

    void exit_status_jump() {

    }

    void status_jumpsquat() {
        if (frame == 3.0) {
            change_status(statuses.STATUS_KIND_JUMP);
        }
    }

    void entry_status_jumpsquat() {

    }

    void exit_status_jumpsquat() {

    }

    void status_hitstun() {

    }

    void entry_status_hitstun() {

    }

    void exit_status_hitstun() {
        
    }

    void status_landing() {

    }

    void entry_status_landing() {

    }

    void exit_status_landing() {

    }
}
