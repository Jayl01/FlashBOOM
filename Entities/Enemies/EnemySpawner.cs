using AnotherLib;
using AnotherLib.Utilities;
using Microsoft.Xna.Framework;

namespace FlashBOOM.Entities.Enemies
{
    public class EnemySpawner
    {
        public const int EnemySpawnTime = 3 * 60;

        private int enemySpawnTimer = 0;

        public void Update()
        {
            enemySpawnTimer++;
            int enemySpawnTime = EnemySpawnTime - (Enemy.EnemiesKilled / 2);
            if (enemySpawnTime < 45)
                enemySpawnTime = 45;

            if (enemySpawnTimer >= enemySpawnTime)
            {
                enemySpawnTimer = 0;
                Vector2 spawnPos = Main.currentPlayer.playerCenter + (Vector2Utils.CreateAngleVector(MathHelper.ToRadians(Main.random.Next(0, 360))) * GameScreen.halfScreenWidth);

                int enemyType = Main.random.Next(0, 1 + 1);
                if (enemyType == 0)
                    EnemyShooter.NewEnemyShooter(spawnPos);
                else if (enemyType == 1)
                    BlockerEnemy.NewBlockerEnemy(spawnPos);
            }
        }
    }
}
