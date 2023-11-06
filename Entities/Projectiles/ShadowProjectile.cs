/*
It stays alive for 5 seconds or dies hitting an object or the player 
similar to the LightProjectile but is more basic
*/

using AnotherLib.Collision;
using Microsoft.Xna.Framework;
using System;

namespace FlashBOOM.Entities.Projectiles
{
    public class ShadowProjectile : Projectile
    {
        public int timer = 0;
        public int hitBoxWidth = 5;
        public int hitBoxHeight = 5;
        public float projectileSpeed = 1.0f; 
        
        public Vector2 velocity = new Vector2(0, 0); // the velocity vector 
        
        public ShadowProjectile()
        {
        }
        public override void Initialize()
        {
            base.Initialize();

            Rectangle hitbox = new Rectangle((int)position.X, (int)position.Y, 
                                              hitBoxWidth, hitBoxHeight);
        }
        /// <summary>
        /// Moves the projectile by updating its position based on its velocity and the given speed.
        /// </summary>
        /// <param name="speed">The speed at which to move the projectile.</param>
        private void Move(float speed)
        {
        
            position.X += velocity.X * speed;
            position.Y += velocity.Y * speed;

        }
        public override void Update()
        {
            base.Update();
            timer++;
            if (timer >= 5)
            {
                DestroyInstance();
            }
            // 
            if (DetectTileCollisionsByCollisionStyle(this.position))
            {
                DestroyInstance();
            }
            DetectCollisions(Main.activePlayers);

           
            

            
        }
        public override void HandleCollisions(CollisionBody collider, CollisionType colliderType)
        {
            base.HandleCollisions(collider, colliderType);
            if (colliderType == CollisionType.Player)
            {
                DestroyInstance();
            }
        }
    }
    
    
}
