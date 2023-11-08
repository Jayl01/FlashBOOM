using FlashBOOM.Effects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FlashBOOM.Entities.Enemies
{
    public class BlockerEnemy : Enemy
    {
        // initial variables
        public static Texture2D enemyTexture;

        public override int EnemyWidth => 16; // experiment and change this
        public override int EnemyHeight => 16; // experiment and change this
        public override int EnemyHealth => 6;

        public override CollisionType collisionType => CollisionType.Enemies;
        public override CollisionType[] colliderTypes => new CollisionType[1] { CollisionType.Player };

        public static void NewBlockerEnemy(Vector2 pos)
        {
            BlockerEnemy enemy = new BlockerEnemy();
            enemy.position = pos;
            enemy.Initialize();
            Main.activeEnemies.Add(enemy);
        }

        private int damageCooldown = 0;

        public override void Initialize()
        {
            hitbox = new Rectangle(position.ToPoint(), new Point(16, 16));
        }

        public override void Update()
        {
            int amountOfSmoke = Main.random.Next(1, 4 + 1);
            for (int i = 0; i < amountOfSmoke; i++)
            {
                Vector2 smokePos = position + new Vector2(Main.random.Next(0, 16 + 1), Main.random.Next(0, 16 + 1));
                Vector2 smokeVelocity = new Vector2(0f, Main.random.Next(-8, -2) / 10f);
                Color smokeColor = Color.Gray;
                Smoke.NewSmokeParticle(smokePos, smokeVelocity, smokeColor, Color.Black, 60, 120, 60, 0.4f, foreground: true);
            }
            if (damageCooldown > 0)
                damageCooldown--;

            if (damageCooldown <= 0 && hitbox.Intersects(Main.currentPlayer.hitbox))
            {
                damageCooldown = 60;
                Main.currentPlayer.playerHealth -= 1;
                if (Main.currentPlayer.playerHealth <= 0)
                    Main.EndGame();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(enemyTexture, position, Color.White);
        }
    }

}