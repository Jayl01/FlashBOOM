using GameTemplate.World.WorldObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GameTemplate.World
{
    public class WorldClass
    {
        public static List<WorldObject> activeWorldObjects;
        public static WorldData activeWorldData;
        public static Tile[,] activeWorldChunk;
        public static WorldDetails worldDetails;

        public static int CurrentWorldWidth;
        public static int CurrentWorldHeight;

        public static void GenerateWorld(int width, int height)
        {
            Tile[,] worldTiles = new Tile[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    worldTiles[x, y] = Tile.GetTileInfo(Tile.TileType.None, new Vector2(x, y) * 16f);
                }
            }
            Main.camera.bounds = new Vector2(width, height) * 16;



            WorldData worldData = new WorldData(0, width, height);
            worldData.tiles = worldTiles;
            activeWorldData = worldData;
            activeWorldObjects = new List<WorldObject>();
            CurrentWorldWidth = width;
            CurrentWorldHeight = height;
            CleanupWorld();
            GenerateExtraWorldFeatures();
            ChunkLoader.ResetChunkDimensions();
            activeWorldChunk = new Tile[ChunkLoader.ChunkSizeWidth, ChunkLoader.ChunkSizeHeight];
            if (Main.currentPlayer != null)
            {
                Main.currentPlayer.position = new Vector2(width * 16 / 2, height * 16 / 2);
                ChunkLoader.ForceUpdateActiveWorldChunk(Main.currentPlayer.position);
            }
            worldDetails = new WorldDetails();
            worldDetails.Initialize();
        }

        public static void GenerateExtraWorldFeatures()
        {

        }

        public static void CleanupWorld()
        {
            for (int x = 0; x < activeWorldData.dimensions.X; x++)
            {
                for (int y = 0; y < activeWorldData.dimensions.Y; y++)
                {
                    Tile currentTile = activeWorldData.tiles[x, y];
                    if (x > 1 && x < activeWorldData.dimensions.X - 1)
                    {
                        Tile leftTile = activeWorldData.tiles[x - 1, y];
                        Tile rightTile = activeWorldData.tiles[x + 1, y];
                        if (currentTile.tileType == Tile.TileType.Grass)
                        {
                            if (leftTile.tileType == Tile.TileType.None)
                                currentTile = Tile.GetTileInfo(Tile.TileType.LeftGrass, currentTile.tilePosition);
                            if (rightTile.tileType == Tile.TileType.None)
                                currentTile = Tile.GetTileInfo(Tile.TileType.RightGrass, currentTile.tilePosition);
                        }
                    }

                    activeWorldData.tiles[x, y] = currentTile;
                }
            }
        }

        public static void Update()
        {
            worldDetails.Update();
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            if (activeWorldChunk == null)
                return;

            foreach (Tile tile in activeWorldChunk)
                tile.Draw(spriteBatch);
            foreach (WorldObject obj in activeWorldObjects)
                obj.Draw(spriteBatch);
        }

        public static void DrawDetails(SpriteBatch spriteBatch)
        {
            worldDetails.Draw(spriteBatch);
        }
    }
}
