using AnotherLib.Utilities;
using FlashBOOM.Effects;
using FlashBOOM.Entities.Enemies;
using FlashBOOM.Entities.Players;
using FlashBOOM.Entities.Projectiles;
using FlashBOOM.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FlashBOOM.Utilities
{
    public class AssetLoader : ContentLoader
    {
        private static ContentLoader contentLoader;
        private static AssetLoader assetLoader;

        public AssetLoader(ContentManager content) : base(content)
        {
            contentManager = content;
        }

        public static void LoadAssets(ContentManager content)
        {
            assetLoader = new AssetLoader(content);
            Main.gameFont = assetLoader.LoadFont("MainFont");
            assetLoader.LoadTextures();
            assetLoader.LoadSounds();
        }

        private void LoadTextures()
        {
            Tile.tileTextures = new Dictionary<Tile.TileType, Texture2D>
            {
                { Tile.TileType.Void, LoadTex("Tiles/Void") },
                { Tile.TileType.Grass, LoadTex("Tiles/Grass_1") },
                { Tile.TileType.Grass_2, LoadTex("Tiles/Grass_2") },
                { Tile.TileType.Grass_3, LoadTex("Tiles/Grass_3") },
                { Tile.TileType.Grass_4, LoadTex("Tiles/Grass_4") },
            };

            Player.playerWalkSpritesheets = new Texture2D[4];
            Player.playerWalkSpritesheets[(int)Player.Direction.Front] = LoadTex("Player/Player_WalkDown");
            Player.playerWalkSpritesheets[(int)Player.Direction.Left] = LoadTex("Player/Player_WalkLeft");
            Player.playerWalkSpritesheets[(int)Player.Direction.Right] = LoadTex("Player/Player_WalkRight");
            Player.playerWalkSpritesheets[(int)Player.Direction.Back] = LoadTex("Player/Player_WalkUp");
            Player.playerFlashlightTexture = LoadTex("Player/PlayerFlashlight");
            LightProjectile.bulletTexture = LoadTex("Projectiles/FlashlightBullet");

            EnemyShooter.enemyTexture = LoadTex("Enemies/EnemyShooter");
            BlockerEnemy.enemyTexture = LoadTex("Enemies/BlockerEnemy");

            Gore.goreTextures = new Texture2D[2];

            Smoke.smokePixelTextures = new Texture2D[1];
            Smoke.smokePixelTextures[Smoke.WhitePixelTexture] = TextureGenerator.CreatePanelTexture(2, 2, 1, Color.White, Color.White, false);

        }

        private void LoadSounds()
        {
            SoundPlayer.sounds = new SoundEffect[16];
            Main.ambienceMusic = LoadSFX("Music/Ambient").CreateInstance();
            Main.actionMusic = LoadSFX("Music/Action").CreateInstance();
        }
    }
}
