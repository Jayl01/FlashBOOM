/*
Enemy that shoots shadow bullets towards the player.
*/
using FlashBOOM.Entities.Players;
using FlashBOOM.Entities.Projectiles;
using Microsoft.Xna.Framework;
using System;

namespace FlashBOOM.Entities.Enemies
{
    public class EnemyShooter : Enemy 
    {
        // initial variables
        public float rangeRadius = 10f;


        public override int EnemyWidth => 16; // experiment and change this
        public override int EnemyHeight  => 16; // experiment and change this

        public float bulletSpeed = 1.0f; // the speed of the bulle
        public Vector2 bulletVelocity = new Vector2(0, 0); // the velocity of the bullet

        public override CollisionType collisionType => CollisionType.Enemies;

        public override void Initialize()
        {
            base.Initialize();
            health = EnemyHealth; 
            hitbox = new Rectangle((int)position.X, (int)position.Y, EnemyWidth, EnemyHeight);
        
        }

        public void shootShadow(Vector2 velocity, Player player)
        {
            // create a new shadow bullet
            ShadowProjectile shadow = new ShadowProjectile();
            // set the position of the shadow bullet to the position of the player
            shadow.position =  player.position;
            // set the velocity of the shadow bullet to the given velocity
            shadow.velocity = bulletVelocity * Vector2.Normalize(velocity);
            // add the shadow bullet to the list of active projectiles
            Main.activeProjectiles.Add(shadow);
        }
        public static void SpawnShooter(Vector2 pos)
        {
            // create a new enemy
            EnemyShooter enemy = new EnemyShooter();
            // set the position of the enemy to the given position
            enemy.position = pos;
            // add the enemy to the list of active enemies
            Main.activeEnemies.Add(enemy);
        }
        public enum EnemyState
        {
            Idle,
            Moving,
            Attacking,
            Dead
        }
        public void calculatePath()
        {
            // TODO calculate a path to the player
        }
        public override void Update()
        {
            base.Update();
            EnemyState state = EnemyState.Idle;
            // if the player is in range of the enemy then call shootShadow()
            if (Vector2.Distance(this.position, Main.activePlayers[0].position) < rangeRadius)
            {
                state = EnemyState.Attacking;
                shootShadow(Main.activePlayers[0].position - this.position, Main.currentPlayer);
            }
            else {
                state = EnemyState.Moving;
                calculatePath();
            }
            // if out of range move towards the player 
            // TODO AI that calculates a path to the player

        }
    }
    
}