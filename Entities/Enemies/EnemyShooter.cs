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

        public override CollisionType collisionType => CollisionType.Enemies;

        public override void Initialize()
        {
            base.Initialize();
            health = EnemyHealth; 
            hitbox = new Rectangle((int)position.X, (int)position.Y, EnemyWidth, EnemyHeight);
        
        }

        /// <summary>
        /// Creates a new shadow bullet and adds it to the list of active projectiles, moving it slowly towards the player.
        /// </summary>
        /// <param name="player">The player to aim the bullet at.</param>
        public void shootShadow(Player player)
        {
            // create a new shadow bullet
            ShadowProjectile shadow = new ShadowProjectile();
           // move the bullet slowly to the player
            Vector2 direction = Vector2.Normalize(player.position - this.position);
            shadow.position += direction * bulletSpeed;
            
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
            Attacking,
            Dead
        }
        public override void Update()
        {
            base.Update();
            EnemyState state = EnemyState.Idle;
            // if the player is in range of the enemy then call shootShadow()
            if (Vector2.Distance(this.position, Main.activePlayers[0].position) < rangeRadius)
            {
                state = EnemyState.Attacking;
                shootShadow(Main.currentPlayer);
            }
          

        }
    }
    
}