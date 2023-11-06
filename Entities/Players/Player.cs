using AnotherLib;
using AnotherLib.Input;
using AnotherLib.Utilities;
using FlashBOOM.Effects;
using FlashBOOM.Entities.Projectiles;
using FlashBOOM.UI;
using FlashBOOM.Utilities;
using FlashBOOM.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace FlashBOOM.Entities.Players
{
    public class Player : PlatformerBody
    {
        private const int PlayerWidth = 24;
        private const int PlayerHeight = 24;
        private const float MoveSpeed = 1.7f;
        private readonly Vector2 FlashlightPlacementOffset = new Vector2(5, 13);
        private readonly Vector2 FlashlightOrigin = new Vector2(2, 3);
        private readonly Vector2 FlashlightSize = new Vector2(13, 7);
        public static Texture2D[] playerWalkSpritesheets;
        public static Texture2D playerFlashlightTexture;
        private Texture2D currentTexture;


        public Vector2 playerCenter;
        public Vector2 oldPosition;
        private Vector2 throwVelocity;
        public Direction direction = Direction.Front;
        public int playerHealth = 3;
        private int immunityTimer = 0;
        private float flashlightRotation;

        private int frame = 0;
        private int frameCounter = 0;
        private Rectangle animRect;
        private PlayerState playerState;
        private PlayerState oldPlayerState;
        private bool loadedWorld = false;
        private Vector2 currentVelocity;

        public override CollisionType collisionType => CollisionType.Player;
        public override CollisionType[] colliderTypes => new CollisionType[2] { CollisionType.Enemies, CollisionType.EnemyProjectiles };

        private enum PlayerState
        {
            Walking,
            Dead
        }

        public enum Direction      //The way the player is facing in the sprite
        {
            Front,
            Right,
            Left,
            Back
        }

        private struct AfterImageData
        {
            public Vector2 position;
            public Rectangle animRect;
            public Color drawColor;
            public SpriteEffects spriteEffects;
            public int lifeTime;
            public int lifeTimer;
            public float alpha;
        }

        public override void Initialize()
        {
            currentTexture = playerWalkSpritesheets[(int)Direction.Front];
            hitbox = new Rectangle(0, 0, PlayerWidth, PlayerHeight);
            animRect = new Rectangle(0, 0, PlayerWidth, PlayerHeight);
            Main.uiList.Add(PlayerUI.NewPlayerUI());
        }

        public override void Update()
        {
            if (immunityTimer > 0)
                immunityTimer--;

            if (playerHealth > 0)
                flashlightRotation = (GameData.MouseWorldPosition - playerCenter).GetRotation();

            Vector2 moveVelocity = Move(MoveSpeed);
            if (playerHealth <= 0)
                moveVelocity = Vector2.Zero;
            Vector2 velocity = moveVelocity + throwVelocity;
            if (tileCollisionDirection[CollisionDirection_Bottom] && tileCollisionDirection[CollisionDirection_Left] && tileCollisionDirection[CollisionDirection_Right])
                position.Y -= 1f;
            if (throwVelocity != Vector2.Zero)
            {
                if (Math.Abs(throwVelocity.X) < 0.01f && Math.Abs(throwVelocity.Y) < 0.01f)
                    throwVelocity = Vector2.Zero;
                else
                    throwVelocity *= 0.97f;
            }

            if (playerHealth > 0)
            {
                if (InputManager.IsMouseLeftJustPressed() || InputManager.IsButtonJustPressed(InputManager.attackButton))
                {

                }
                if (GameInput.IsAttackHeld())
                {

                }
            }

            playerState = PlayerState.Walking;
            position += velocity;
            currentVelocity = velocity;
            playerCenter = position + new Vector2(PlayerWidth / 2f, PlayerHeight / 2f);
            hitbox.X = (int)(position.X + hitboxOffset.X);
            hitbox.Y = (int)(position.Y + hitboxOffset.Y);
            DetectTileCollisions();
            GameData.AudioPosition = playerCenter;
            AnimatePlayer();
            ChunkLoader.UpdateActiveWorldChunk(playerCenter);
            Main.camera.UpdateCamera(playerCenter);
            oldPosition = position;
            if (!loadedWorld)
            {
                loadedWorld = true;
                ChunkLoader.ForceUpdateActiveWorldChunk(playerCenter);
            }
        }

        public Vector2 Move(float moveSpeed)
        {
            Vector2 velocity = Vector2.Zero;

            DetectWorldObjectCollisions();
            if (!GameInput.ControllerConnected)
            {
                if (GameInput.IsLeftPressed() && !tileCollisionDirection[CollisionDirection_Left])
                {
                    direction = Direction.Left;
                    velocity.X -= moveSpeed;
                }
                if (GameInput.IsRightPressed() && !tileCollisionDirection[CollisionDirection_Right])
                {
                    direction = Direction.Right;
                    velocity.X += moveSpeed;
                }
                if (GameInput.IsUpPressed() && !tileCollisionDirection[CollisionDirection_Top])
                {
                    direction = Direction.Back;
                    velocity.Y -= moveSpeed;
                }
                if (GameInput.IsDownPressed() && !tileCollisionDirection[CollisionDirection_Bottom])
                {
                    direction = Direction.Front;
                    velocity.Y += moveSpeed;
                }
            }
            else
            {
                Vector2 leftAnalog = GameInput.GetLeftAnalogVector();
                velocity = leftAnalog * moveSpeed;
                if (velocity.Y < 0f && tileCollisionDirection[CollisionDirection_Top])
                    velocity.Y = 0f;
                if (velocity.X < 0f && tileCollisionDirection[CollisionDirection_Left])
                    velocity.X = 0f;
                if (velocity.Y > 0f && tileCollisionDirection[CollisionDirection_Bottom])
                    velocity.Y = 0f;
                if (velocity.X > 0f && tileCollisionDirection[CollisionDirection_Right])
                    velocity.X = 0f;

                if (velocity.X < 0.05f)
                    direction = Direction.Left;
                if (velocity.X > 0.05f)
                    direction = Direction.Right;
                if (velocity.Y < 0.05f)
                    direction = Direction.Back;
                if (velocity.Y > 0.05f)
                    direction = Direction.Front;
            }

            return velocity;
        }

        public Vector2 Move(Vector2 velocity, bool detectCollisions = false)
        {
            if (detectCollisions)
            {
                DetectTileCollisions();
                if (velocity.Y < 0 && tileCollisionDirection[CollisionDirection_Top])
                    velocity.Y = 0f;
                else if (velocity.Y > 0 && tileCollisionDirection[CollisionDirection_Bottom])
                    velocity.Y = 0f;

                if (velocity.X < 0 && tileCollisionDirection[CollisionDirection_Left])
                    velocity.X = 0f;
                else if (velocity.X > 0 && tileCollisionDirection[CollisionDirection_Right])
                    velocity.X = 0f;
            }

            position += velocity;
            playerCenter = position + new Vector2(PlayerWidth / 2f, PlayerHeight / 2f);
            hitbox.X = (int)(position.X + hitboxOffset.X);
            hitbox.Y = (int)(position.Y + hitboxOffset.Y);
            GameData.AudioPosition = playerCenter;
            return velocity;
        }

        private void AnimatePlayer()
        {
            if (oldPlayerState != playerState)
            {
                frame = 0;
                frameCounter = 0;
                animRect.Y = 0;
                oldPlayerState = playerState;
            }

            frameCounter++;
            if (playerState == PlayerState.Walking)
            {
                if (currentTexture != playerWalkSpritesheets[(int)direction])
                    currentTexture = playerWalkSpritesheets[(int)direction];
                if (currentVelocity == Vector2.Zero)
                {
                    frameCounter = 0;
                    frame = 1;
                }

                if (frameCounter >= 7)
                {
                    frame += 1;
                    frameCounter = 0;
                    if (frame >= 4)
                        frame = 0;

                    animRect.Y = frame * PlayerHeight;
                    //if (frame == 1 || frame == 3)
                        //SoundPlayer.PlaySoundFromOtherSource(Main.random.Next(Sounds.Step_1, Sounds.Step_3 + 1), playerCenter, 12, soundPitch: Main.random.Next(-4, 4 + 1) / 10f);
                }

            }
        }

        public void Hurt()
        {
            if (immunityTimer > 0)
                return;

            immunityTimer = 60;
            playerHealth--;
            Main.camera.ShakeCamera(1, 15);
            //if (playerHealth <= 0)
            //End game
            //SoundPlayer.PlayLocalSound(Sounds.AttemptHit);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            SpriteEffects spriteEffects = SpriteEffects.None;
            SpriteEffects armSpriteEffects = SpriteEffects.None;
            Vector2 armDrawPosition = position + FlashlightPlacementOffset;

            spriteBatch.Draw(currentTexture, position, animRect, Color.White, 0f, Vector2.Zero, 1f, spriteEffects, 0f);
            //spriteBatch.Draw(playerFlashlightTexture, armDrawPosition, null, Color.White, flashlightRotation, FlashlightOrigin, 1f, armSpriteEffects, 0f);
        }
    }
}
