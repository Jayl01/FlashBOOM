/*
Enemy that shoots shadow bullets towards the player.
*/
using FlashBOOM.Effects;
using FlashBOOM.Entities.Players;
using FlashBOOM.Entities.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FlashBOOM.Entities.Enemies
{
    public class EnemyShooter : Enemy
    {
        // initial variables
        public float rangeRadius = 10f;


        public override int EnemyWidth => 16; // experiment and change this
        public override int EnemyHeight => 16; // experiment and change this

        public override int EnemyHealth => 4;

        public float bulletSpeed = 1.0f; // the speed of the bulle
        public static Texture2D enemyTexture;

        public override CollisionType collisionType => CollisionType.Enemies;

        public static void NewEnemyShooter(Vector2 pos)
        {
            // create a new enemy
            EnemyShooter enemy = new EnemyShooter();
            // set the position of the enemy to the given position
            enemy.position = pos;
            // add the enemy to the list of active enemies
            Main.activeEnemies.Add(enemy);
        }

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
        public void ShootShadow(Player player)
        {
            ShadowProjectile shadow = new ShadowProjectile();
            shadow.position = hitbox.Center.ToVector2();
            Vector2 direction = Vector2.Normalize(player.position - this.position);
            shadow.velocity += direction * bulletSpeed;
            Main.activeProjectiles.Add(shadow);
        }

        public enum EnemyState
        {
            Idle,
            Attacking,
            Dead
        }

        public override void Update()
        {
            int amountOfSmoke = Main.random.Next(1, 4 + 1);
            for (int i = 0; i <  amountOfSmoke; i++)
            {
                Vector2 smokePos = position + new Vector2(Main.random.Next(8, 12 + 1), Main.random.Next(8, 12 + 1));
                Vector2 smokeVelocity = new Vector2(0f, Main.random.Next(-8, -2) / 10f);
                Color smokeColor = Color.Gray;
                Smoke.NewSmokeParticle(smokePos, smokeVelocity, smokeColor, Color.Black, 60, 120, 60, 0.4f, foreground: true);
            }
            if (Vector2.Distance(position, Main.activePlayers[0].position) < rangeRadius)
                ShootShadow(Main.currentPlayer);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(enemyTexture, position, Color.White);
        }
    }
}