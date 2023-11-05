using FlashBOOM.World.WorldObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FlashBOOM.World
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
                    worldTiles[x, y] = Tile.GetTileInfo(Tile.TileType.Void, new Vector2(x, y) * 16f);
                }
            }
            Main.camera.bounds = new Vector2(width, height) * 16;

            int paddingSize = 5;
            for (int x = paddingSize; x < width - paddingSize; x++)
            {
                for (int y = paddingSize; y < height - paddingSize; y++)
                {
                    worldTiles[x, y] = Tile.GetTileInfo((Tile.TileType)Main.random.Next((int)Tile.TileType.Grass, (int)Tile.TileType.Grass + 1), new Vector2(x, y) * 16f);
                }
            }

            int darkPatchAmount = Main.random.Next(12, 24 + 1);
            for (int i = 0; i < darkPatchAmount; i++)
            {
                int darkPatchRadius = Main.random.Next(4, 16 + 1);
                Point darkPatchCenter = new Point(Main.random.Next(paddingSize, width - paddingSize + 1), Main.random.Next(paddingSize, height - paddingSize + 1));
                for (int x = darkPatchCenter.X - (darkPatchRadius / 2); x < darkPatchCenter.X + (darkPatchRadius / 2); x++)
                {
                    for (int y = darkPatchCenter.Y - (darkPatchRadius / 2); y < darkPatchCenter.Y - (darkPatchRadius / 2); y++)
                    {
                        if (CoordsInBounds(width, height, new Point(x,y)) && Vector2.Distance(darkPatchCenter.ToVector2(), new Vector2(x, y)) < darkPatchRadius)
                        {
                            if (worldTiles[x, y].tileType == Tile.TileType.Grass)
                                worldTiles[x, y] = Tile.GetTileInfo(Tile.TileType.Grass_3, new Vector2(x, y) * 16f);
                            else if (worldTiles[x, y].tileType == Tile.TileType.Grass_2)
                                worldTiles[x, y] = Tile.GetTileInfo(Tile.TileType.Grass_4, new Vector2(x, y) * 16f);
                        }
                    }
                }
            }

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
        }

        private static bool CoordsInBounds(int width, int height, Point coords) => coords.X > 0 && coords.X < width && coords.Y > 0 && coords.Y < height;

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
