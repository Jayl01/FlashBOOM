/*
Light illuminating from the flashlight
Indicates how far you can see
Little recoil ; can't see for a second 
Upgradeable object
It stays alive for 5 seconds or dies hitting an object or an enemy 
*/
using AnotherLib.Collision;
using AnotherLib.Utilities;
using FlashBOOM.Entities.Enemies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FlashBOOM.Entities.Projectiles
{
    public class LightProjectile : Projectile
    {
        public int timer = 0;
        public int hitBoxWidth = 3;
        public int hitBoxHeight = 6;
        public float projectileSpeed = 8.0f;
        public Vector2 bulVel = new Vector2(0, 0); // the velocity vector 
        public static Texture2D bulletTexture;

        public static void NewLightProjectile(Vector2 position, Vector2 velocity)
        {
            LightProjectile lightProjectile = new LightProjectile();
            lightProjectile.position = position;
            lightProjectile.bulVel = velocity;
            Main.activeProjectiles.Add(lightProjectile);
        }

        public override void Initialize()
        {
            hitbox = new Rectangle((int)position.X, (int)position.Y, hitBoxWidth, hitBoxHeight);
        }

        public override void Update()
        {
            timer++;
            if (timer >= 3 * 60)
                DestroyInstance();

            if (DetectTileCollisionsByCollisionStyle(this.position))
                DestroyInstance();

            DetectCollisions(Main.activeEnemies);
            Move(projectileSpeed);
        }

        public override void HandleCollisions(CollisionBody collider, CollisionType colliderType)
        {
            base.HandleCollisions(collider, colliderType);
            if (colliderType == CollisionType.Enemies)
            {
                (collider as Enemy).health -= 1;
                if ((collider as Enemy).health <= 0)
                    (collider as Enemy).DestroyInstance();
                DestroyInstance();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(bulletTexture, position, null, Color.White, bulVel.GetRotation() + MathHelper.ToRadians(90f), new Vector2(1.5f, 3f), 1f, SpriteEffects.None, 0f);
        }

        /// <summary>
        /// Moves the projectile by updating its position based on its velocity and the given speed.
        /// </summary>
        /// <param name="speed">The speed at which to move the projectile.</param>
        private void Move(float speed)
        {
            position.X += bulVel.X * speed;
            position.Y += bulVel.Y * speed;
            hitbox.Location = position.ToPoint();
        }
    }
}
