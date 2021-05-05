using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int id;
    public string username;
    public CharacterController controller;
    public Transform shootOrigin;
    
    public float moveSpeed = Constants.DEFAULT_MOVE_SPEED;
    public float jumpSpeed = Constants.DEFAULT_JUMP_SPEED;
    public float throwForce = 600f;
    private float gravity = Constants.GRAVITY;
    public int health;
    public int maxHealth = 100;
    public int damage = 50;
    private bool[] inputs;
    private float yVelocity = 0;
    public int itemAmount = 0;
    public int maxItemAmount = 3;

    private void Start(){
        gravity *= Time.fixedDeltaTime * Time.fixedDeltaTime;
        moveSpeed *= Time.fixedDeltaTime;
        jumpSpeed *= Time.fixedDeltaTime;
    }

    public void Initialize(int _id, string _username){
        id = _id;
        username = _username;

        health = maxHealth;
        inputs = new bool[Constants.NUM_INPUTS];
    }

    public void FixedUpdate(){
        if(health <= 0){
            return;
        }
        Vector2 _inputDirection = Vector2.zero;
        // Forward?
        if(inputs[0]){
            _inputDirection.y += 1;
        }
        if(inputs[1]){
            _inputDirection.x -= 1;
        }
        if(inputs[2]){
            _inputDirection.y -= 1;
        }
        // Right?
        if(inputs[3]){
            _inputDirection.x += 1;
        }

        Move(_inputDirection);
    }

    private void Move(Vector2 _inputDirection){
        Vector3 _moveDirection = transform.right * _inputDirection.x + transform.forward * _inputDirection.y;
        _moveDirection *= moveSpeed;

        controller.Move(_moveDirection);

        if(controller.isGrounded){
            yVelocity = 0f;
            
        }

        if(inputs[4]){
            yVelocity = jumpSpeed;
        }
        
        yVelocity += gravity;

        _moveDirection.y = yVelocity;
        controller.Move(_moveDirection);

        ServerSend.PlayerPosition(this);
        ServerSend.PlayerRotation(this);
    }

    public void SetInput(bool[] _inputs, Quaternion _rotation){
        inputs = _inputs;
        transform.rotation = _rotation;
    }

    public void Shoot(Vector3 _viewDirection){
        if(health <= 0){
            return;
        }
        if(Physics.Raycast(shootOrigin.position, _viewDirection, out RaycastHit _hit, 25f)){
            if(_hit.collider.CompareTag("Player")){
                _hit.collider.GetComponent<Player>().TakeDamage(damage);
            }
        }
    }

    public void ThrowItem(Vector3 _viewDirection){
        if(health <=  0){
            return;
        }
        if(itemAmount > 0){
            itemAmount--;
            NetworkManager.instance.InstantiateProjectile(shootOrigin).Initialize(_viewDirection, throwForce, id);
        }
    }

    public void TakeDamage(int _damage){
        if(health <= 0){
            return;
        }

        health -= _damage;
        Debug.Log($"Setting {username}'s health to {health}");
        if(health <= 0){
            health = 0;
            controller.enabled = false;
            transform.position = new Vector3(0f, 25f, 0f);
            ServerSend.PlayerPosition(this);

            StartCoroutine(Respawn());
        }

        ServerSend.PlayerHealth(this);
    }

    private IEnumerator Respawn(){
        yield return new WaitForSeconds(5f);

        health = maxHealth;
        controller.enabled = true;

        ServerSend.PlayerRespawned(this);
    }

    public bool AttemptPickupItem(){
        if(itemAmount >= maxItemAmount){
            return false;
        }
        itemAmount++;
        return true;
    }
}
