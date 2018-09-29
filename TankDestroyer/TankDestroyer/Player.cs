/*
 * Marina Lucaj, Ibrahim Dabajeh, Luke Pacheco, Michael Awada, Anthony Chua
 * CIS 297 Capstone Project
 * This is the player class that affects the player's movement with the tank
 */
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using MonoGame.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TankDestroyer
{
    public class Player
    {
        public Texture2D playerTank, playerBullet, playerHealth;
        public Vector2 playerOrigin, playerPos, tankUpperLeftPos, bulletPos;
        public Rectangle playerBounds, bulletBounds, healthRect;
        public int tankSpeed, bulletSpeed, health, playerID;
        public float playerRotation;
        public bool fire;
        public enum Direction { Up, Down, Left, Right };
        public Direction playerDir, bulletDir;

        public Vector2 Shoot(Vector2 bulletPosition, int bulletSpd, Direction bulletDirection)
        {
            if (bulletDirection == Direction.Up)
                bulletPosition.Y -= bulletSpd;
            else if (bulletDirection == Direction.Down)
                bulletPosition.Y += bulletSpd;
            else if (bulletDirection == Direction.Right)
                bulletPosition.X += bulletSpd;
            else if (bulletDirection == Direction.Left)
                bulletPosition.X -= bulletSpd;

            return bulletPosition;
        }

         public Player(int ID)
        {
           
            playerID = ID;

        }

        public void Update (GameTime gameTime)
        {
            if (playerID == 1)
            {
                //initialize
                healthRect = new Rectangle(5, 5, health, 25);
                playerBounds = new Rectangle((int)tankUpperLeftPos.X, (int)tankUpperLeftPos.Y, playerTank.Width, playerTank.Height);
                if (fire == true)
                {
                    bulletBounds = new Rectangle((int)bulletPos.X, (int)bulletPos.Y,
                        playerBullet.Width, playerBullet.Height);
                }
                playerOrigin = new Vector2(playerTank.Width / 2, playerTank.Height / 2);


                //MOVEMENT
                //move left
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    if (playerPos.X > 0 + .5 * playerTank.Width)
                    {
                        playerPos.X -= tankSpeed;
                        tankUpperLeftPos.X -= tankSpeed;
                        playerRotation = MathHelper.ToRadians(-90);
                        playerDir = Player.Direction.Left;
                    }
                }

                //p1 move right
                if (Keyboard.GetState().IsKeyDown(Keys.D))
                {
                    if (playerPos.X < 800 - .5 * playerTank.Width)
                    {

                        playerPos.X += tankSpeed;
                        tankUpperLeftPos.X += tankSpeed;
                        playerRotation = MathHelper.ToRadians(90);
                        playerDir = Player.Direction.Right;
                    }
                }

                //p1 move up
                if (Keyboard.GetState().IsKeyDown(Keys.W))
                {
                    if (playerPos.Y > 0 + .5 * playerTank.Height)
                    {
                        playerPos.Y -= tankSpeed;
                        tankUpperLeftPos.Y -= tankSpeed;
                        playerRotation = MathHelper.ToRadians(360);
                        playerDir = Player.Direction.Up;
                    }

                }

                //p1 move down
                if (Keyboard.GetState().IsKeyDown(Keys.S))
                {
                    if (playerPos.Y < 480 - .5 * playerTank.Height)
                    {
                        playerPos.Y += tankSpeed;
                        tankUpperLeftPos.Y += tankSpeed;
                        playerRotation = MathHelper.ToRadians(180);
                        playerDir = Player.Direction.Down;
                    }
                }
            }

            if(playerID == 2)
            {
                //Initialize 
                healthRect = new Rectangle(695, 450, health, 25);
                playerBounds = new Rectangle((int)tankUpperLeftPos.X, (int)tankUpperLeftPos.Y, playerTank.Width, playerTank.Height);
                if (fire == true)
                {
                    bulletBounds = new Rectangle((int)bulletPos.X, (int)bulletPos.Y,
                          playerBullet.Width, playerBullet.Height);
                }
                playerOrigin = new Vector2(playerTank.Width / 2, playerTank.Height / 2);

                //PLAYER MOVEMENT
                //p2 move left
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    if (playerPos.X > 0 + .5 * playerTank.Width)
                    {
                        playerPos.X -= tankSpeed;
                        tankUpperLeftPos.X -= tankSpeed;
                        playerRotation = MathHelper.ToRadians(90);
                        playerDir = Player.Direction.Left;
                    }
                }
                //p2 move right
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    if (playerPos.X < 800 - .5 * playerTank.Width)
                    {
                        playerPos.X += tankSpeed;
                        tankUpperLeftPos.X += tankSpeed;
                        playerRotation = MathHelper.ToRadians(-90);
                        playerDir = Player.Direction.Right;
                    }
                }
                //p2 move up
                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    if (playerPos.Y > 0 + .5 * playerTank.Height)
                    {
                        playerPos.Y -= tankSpeed;
                        tankUpperLeftPos.Y -= tankSpeed;
                        playerRotation = MathHelper.ToRadians(180);
                        playerDir = Player.Direction.Up;

                    }

                }
                //p2 move down
                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    if (playerPos.Y < 480 - .5 * playerTank.Height)
                    {
                        playerPos.Y += tankSpeed;
                        tankUpperLeftPos.Y += tankSpeed;
                        playerRotation = MathHelper.ToRadians(360);
                        playerDir = Player.Direction.Down;
                    }
                }              
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch )
        {
            if(playerID == 1)
            {
                
                spriteBatch.Draw(playerTank, playerPos, new Rectangle(0, 0, 50, 50),
                    Color.OrangeRed, playerRotation, playerOrigin, 1, SpriteEffects.FlipVertically, 0);
            }
            if(playerID == 2)
            {
                
                spriteBatch.Draw(playerTank, playerPos, new Rectangle(0, 0, 50, 50),
                    Color.Teal, playerRotation, playerOrigin, 1, SpriteEffects.None, 0);
            }
        }
    }
}
