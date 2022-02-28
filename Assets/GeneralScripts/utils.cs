using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Resources;

public enum move_type {
    MOVE_TYPE_STRAIGHT,
    MOVE_TYPE_HOMING,
}

public enum enemy_type {
    ENEMY_TYPE_DEFAULT,
    ENEMY_TYPE_FAST,
}

public enum statuses {
	STATUS_KIND_WAIT, //Idle
	STATUS_KIND_WALK, //Basic ground movement
	STATUS_KIND_CROUCH_D, //Starting to crouch
	STATUS_KIND_CROUCH, //Staying in a crouch
	STATUS_KIND_CROUCH_U, //Rising from a crouch
	//To clarify, I've worked with crouch animations before and as weird as having 3 different states for crouching is, it's 
	//VERY important for basic movement to feel smooth, and this is the best way to do that
	STATUS_KIND_JUMP_SQUAT, //Prejump frames, basically
	STATUS_KIND_JUMP, //Actually jumping
	STATUS_KIND_FALL, //Falling, either after a jump or walking off a platform
	STATUS_KIND_HITSTUN,
	STATUS_KIND_LANDING,
	STATUS_KIND_MAX, //If I ever turn this into a jumptable, having a value that just says "This is the maximum value for the
	//status list" is really convenient
};

public enum situations {
	SITUATION_KIND_GROUND,
	SITUATION_KIND_AIR,
	SITUATION_KIND_MAX,
};

public enum buttons {
	BUTTON_UP,
	BUTTON_DOWN,
	BUTTON_LEFT,
	BUTTON_RIGHT,
	BUTTON_JUMP,

	BUTTON_MAX,
};

public struct Button {
	public KeyCode mapping;
	public bool on;
	public bool changed;
};

public class utils : MonoBehaviour {
    public static Vector2 get_rotated_pos(float x, float y, float angle) {
	    Vector2 ret;
	    ret.x = x;
	    ret.y = y;

	    float sin = Mathf.Sin(angle * Mathf.Deg2Rad);
        float cos = Mathf.Cos(angle * Mathf.Deg2Rad);

	    float tx = ret.x;
        float ty = ret.y;

	    ret.x = (cos * tx) - (sin * ty);
        ret.y = (sin * tx) + (cos * ty);

	    return ret;
    }

    public static void spawn_projectile(Projectile projectile, Rigidbody2D pos, move_type move_type, double speed, double angle, int active_time, int facing_dir) {
        Projectile instance = Instantiate(projectile, pos.position, Quaternion.identity);
        instance.init(move_type, speed, angle, active_time, facing_dir);
    }
};