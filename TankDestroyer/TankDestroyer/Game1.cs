/*
 * Marina Lucaj, Ibrahim Dabajeh, Luke Pacheco, Michael Awada, Anthony Chua
 * CIS 297 Capstone Project
 * This program is a simple game where 2 user controlled tanks fight to the death
 * Player 1 uses WASD to move and SPACE to fire
 * Player 2 uses ARROWKEYS to move and ENTER to fire
 * 3 bombs and 1 healthkit spawn on the map
 * There are 2 maps to choose from
 * 3 hits from either bullets or bombs will cause that player to lose
 */
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TankDestroyer
{
    public class Game1: Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player p1, p2;
        bool healthPackAcquired, bomb1Hit, bomb2Hit, bomb3Hit;
        SoundEffect shot, hit, destroy, backgroundMusic;
        Random rand;
        Texture2D healthPack, bomb1, bomb2, bomb3, level1BG, level2BG, snowbush, tree, mainMenu, mapSelect, p1WinScreen, p2WinScreen, drawScreen;
        Vector2 healthPackPos, bomb1Pos, bomb2Pos, bomb3Pos;
        Rectangle healthPackBounds, bomb1Bounds, bomb2Bounds, bomb3Bounds, level1Viewport, level2Viewport, treeRect1, treeRect2;

        public enum State { MainMenu, StageSelect, Stage1Play, Stage2Play, P1WinScreen, P2WinScreen, DrawScreen };
        State gameState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            gameState = State.MainMenu;
            p1 = new Player(1);
            p2 = new Player(2);

            p1.tankSpeed = 2;
            p2.tankSpeed = 2;
            p1.bulletSpeed = 5;
            p2.bulletSpeed = 5;
            p1.health = 99;
            p2.health = 99;

            p1.fire = false;
            p2.fire = false;
            healthPackAcquired = false;
            bomb1Hit = false;
            bomb2Hit = false;
            bomb3Hit = false;

            rand = new Random();

            level1Viewport = new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width,
                graphics.GraphicsDevice.Viewport.Height);
            level2Viewport = new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width,
                graphics.GraphicsDevice.Viewport.Height);
            treeRect1 = new Rectangle(50, 50, 100, 100);
            treeRect2 = new Rectangle(450, 350, 100, 100);

            //spawn healthkit in middle of map
            healthPackPos.X = 350;
            healthPackPos.Y = 240;
            healthPackBounds = new Rectangle((int)healthPackPos.X, (int)healthPackPos.Y, 80, 80);


            //initialize bomb 1, 2, and 3 in random locations 
            bomb1Pos.X = rand.Next(0, Window.ClientBounds.Width);
            bomb1Pos.Y = rand.Next(30, 400);
            bomb1Bounds = new Rectangle((int)bomb1Pos.X, (int)bomb1Pos.Y, 40, 40);
            bomb2Pos.X = rand.Next(0, Window.ClientBounds.Width);
            bomb2Pos.Y = rand.Next(30, 400);
            bomb2Bounds = new Rectangle((int)bomb2Pos.X, (int)bomb2Pos.Y, 40, 40);
            bomb3Pos.X = rand.Next(0, Window.ClientBounds.Width);
            bomb3Pos.Y = rand.Next(30, 400);
            bomb3Bounds = new Rectangle((int)bomb3Pos.X, (int)bomb3Pos.Y, 40, 40);


            p1.playerDir = Player.Direction.Up;
            p2.playerDir = Player.Direction.Down;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Loading images
            p1.playerTank = Content.Load<Texture2D>(@"Images\Tank");
            p2.playerTank = Content.Load<Texture2D>(@"Images\Tank");
            p1.playerBullet = Content.Load<Texture2D>(@"Images\Bullet");
            p2.playerBullet = Content.Load<Texture2D>(@"Images\Bullet");

            healthPack = Content.Load<Texture2D>(@"Images\healthPack");
            bomb1 = Content.Load<Texture2D>(@"Images\bomb");
            bomb2 = Content.Load<Texture2D>(@"Images\bomb");
            bomb3 = Content.Load<Texture2D>(@"Images\bomb");
            level1BG = Content.Load<Texture2D>(@"Images\Level1");
            level2BG = Content.Load<Texture2D>(@"Images\Level2");
            p1.playerHealth = Content.Load<Texture2D>(@"Images\HealthBar");
            p2.playerHealth = Content.Load<Texture2D>(@"Images\HealthBar");
            mainMenu = Content.Load<Texture2D>(@"Images\MainMenu");
            p1WinScreen = Content.Load<Texture2D>(@"Images\p1WinScreen");
            p2WinScreen = Content.Load<Texture2D>(@"Images\p2WinScreen");
            snowbush = Content.Load<Texture2D>(@"Images\Snowbush");
            tree = Content.Load<Texture2D>(@"Images\Tree");
            drawScreen = Content.Load<Texture2D>(@"Images\stalemate");
            mapSelect = Content.Load<Texture2D>(@"Images\mapSelect");


            //Loading audio
            shot = Content.Load<SoundEffect>(@"Audio\TankShot");
            hit = Content.Load<SoundEffect>(@"Audio\Explosion");
            destroy = Content.Load<SoundEffect>(@"Audio\Explosion 03");
            backgroundMusic = Content.Load<SoundEffect>(@"Audio\Resolution_PremiumBeat");
            SoundEffectInstance instance = backgroundMusic.CreateInstance();
            instance.Play();
            instance.IsLooped = true;

            //initializing player1's position at bottom left corner
            p1.playerPos.X = 0 + (float).5 * p1.playerTank.Width;
            p1.playerPos.Y = Window.ClientBounds.Height - (float).5 * p1.playerTank.Height;

            // initializing p2's position at top right
            p2.playerPos.X = Window.ClientBounds.Width - (float).5 * p2.playerTank.Width;
            p2.playerPos.Y = 0 + (float).5 * p2.playerTank.Height;

            //specifying upper left positions of tank images for more accurate rectangle/intersections,  
            //as the playerPos attribute specifies the center of the image (for rotation purposes)
            p1.tankUpperLeftPos.X = 0;
            p1.tankUpperLeftPos.Y = Window.ClientBounds.Height - p1.playerTank.Height;
            p2.tankUpperLeftPos.X = Window.ClientBounds.Width - p2.playerTank.Width;
            p2.tankUpperLeftPos.Y = 0;

        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

            //Makes sure bombs don't spawn on trees
            if(bomb1Bounds.Intersects(treeRect1) || bomb1Bounds.Intersects(treeRect2))
            {
                bomb1Pos.X = rand.Next(0, Window.ClientBounds.Width);
                bomb1Pos.Y = rand.Next(30, 400);
                bomb1Bounds = new Rectangle((int)bomb1Pos.X, (int)bomb1Pos.Y, 40, 40);
            }
            if(bomb2Bounds.Intersects(treeRect1) || bomb2Bounds.Intersects(treeRect2))
            {
                bomb2Pos.X = rand.Next(0, Window.ClientBounds.Width);
                bomb2Pos.Y = rand.Next(30, 400);
                bomb2Bounds = new Rectangle((int)bomb2Pos.X, (int)bomb2Pos.Y, 40, 40);
            }
            if(bomb3Bounds.Intersects(treeRect1) || bomb3Bounds.Intersects(treeRect2))
            {
                bomb3Pos.X = rand.Next(0, Window.ClientBounds.Width);
                bomb3Pos.Y = rand.Next(30, 400);
                bomb3Bounds = new Rectangle((int)bomb3Pos.X, (int)bomb3Pos.Y, 40, 40);
            }


            KeyboardState keyState = Keyboard.GetState();
            //main menu state
            if (gameState == State.MainMenu)
            {
                if (keyState.IsKeyDown(Keys.Enter))
                    gameState = State.StageSelect;
            }
            //map select state
            else if (gameState == State.StageSelect)
            {
                if (keyState.IsKeyDown(Keys.D1))
                    gameState = State.Stage1Play;
                else if (keyState.IsKeyDown(Keys.D2))
                    gameState = State.Stage2Play;
            }
            //player 1 wins
            else if (gameState == State.P1WinScreen)
            {
                if (keyState.IsKeyDown(Keys.Enter))
                {
                    gameState = State.MainMenu;
                    this.Initialize();

                }
                else if (keyState.IsKeyDown(Keys.Escape))
                this.Exit();
            }
            //player 2 wins
            else if (gameState == State.P2WinScreen)
            {

                if (keyState.IsKeyDown(Keys.Enter))
                {
                    gameState = State.MainMenu;
                    this.Initialize();
                }
                 else if (keyState.IsKeyDown(Keys.Escape))
                this.Exit();
            }
            //Stalemate
            else if (gameState == State.DrawScreen)
            {
                if (keyState.IsKeyDown(Keys.Enter))
                {
                    gameState = State.MainMenu;
                    this.Initialize();

                }
                else if (keyState.IsKeyDown(Keys.Escape))
                    this.Exit();
            }

            //CODE WHEN MAIN GAME IS BEING PLAYED
            else if (gameState == State.Stage1Play || gameState == State.Stage2Play)
            {
                //update player 1 and 2
                p1.Update(gameTime);
                p2.Update(gameTime);
                //p1 fire
                if (keyState.IsKeyDown(Keys.Space))
                {
                    if (p1.fire == false)
                    {
                        p1.fire = true;
                        shot.Play();
                        p1.bulletPos = p1.playerPos;
                        p1.bulletDir = p1.playerDir;
                    }
                }
                if (p1.fire == true)
                {
                    p1.bulletPos = p1.Shoot(p1.bulletPos, p1.bulletSpeed, p1.bulletDir);
                }
             
                //p2 fire
                if (keyState.IsKeyDown(Keys.Enter))
                {
                    if(p2.fire == false)
                    {
                    p2.fire = true;
                    shot.Play();
                    p2.bulletPos = p2.playerPos;
                    p2.bulletDir = p2.playerDir;
                    }
                   
                }
                if (p2.fire == true)
                {
                    p2.bulletPos = p2.Shoot(p2.bulletPos, p2.bulletSpeed, p2.bulletDir);
                }

                //p1 bullet collision with window
                if (p1.bulletBounds.X < 0 || p1.bulletBounds.X > Window.ClientBounds.Width ||
                    p1.bulletBounds.Y < 0 || p1.bulletBounds.Y > Window.ClientBounds.Height)
                {
                    p1.fire = false;
                    p1.bulletBounds.X = p1.playerBounds.X;
                    p1.bulletBounds.Y = p1.playerBounds.Y;
                }
                //p2 bullet colision with window
                if (p2.bulletBounds.X < 0 || p2.bulletBounds.X > Window.ClientBounds.Width ||
                    p2.bulletBounds.Y < 0 || p2.bulletBounds.Y > Window.ClientBounds.Height)
                {
                    p2.fire = false;
                    p2.bulletBounds.X = p2.playerBounds.X;
                    p2.bulletBounds.Y = p2.playerBounds.Y;
                }
                //Collision detect for bullets
                if (p1.bulletBounds.Intersects(p2.playerBounds))
                {
                    hit.Play();  //soundeffect not currently rendering properly
                    p1.fire = false;
                    p2.health -= 33;
                    p1.bulletBounds.Y = -1000;
                }
                if (p2.bulletBounds.Intersects(p1.playerBounds))
                {
                    hit.Play();  //soundeffect not currently rendering properly
                    p2.fire = false;
                    p1.health -= 33;
                    p2.bulletBounds.Y = -1000;
                }
                //Collision detect for items
                //bomb1 collision
                if ((p1.playerBounds.Intersects(bomb1Bounds) && bomb1Hit == false))
                {
                    hit.Play();
                    bomb1Hit = true;
                    p1.health -= 33;
                }
                if (p2.playerBounds.Intersects(bomb1Bounds) && bomb1Hit == false)
                {
                    hit.Play();
                    bomb1Hit = true;
                    p2.health -= 33;
                }
                //bomb2 collision
                if (p1.playerBounds.Intersects(bomb2Bounds) && bomb2Hit == false)
                {
                    hit.Play();
                    bomb2Hit = true;
                    p1.health -= 33;
                }
                if (p2.playerBounds.Intersects(bomb2Bounds) && bomb2Hit == false)
                {
                    hit.Play();
                    bomb2Hit = true;
                    p2.health -= 33;
                }
                //bomb3 collision
                if (p1.playerBounds.Intersects(bomb3Bounds) && bomb3Hit == false)
                {
                    hit.Play();
                    bomb3Hit = true;
                    p1.health -= 33;
                }
                if (p2.playerBounds.Intersects(bomb3Bounds) && bomb3Hit == false)
                {
                    hit.Play();
                    bomb3Hit = true;
                    p2.health -= 33;
                }
                //health pack collision
                if (p1.playerBounds.Intersects(healthPackBounds) && healthPackAcquired == false)
                {
                    if (p1.health < 99)
                    {
                        healthPackAcquired = true;
                        p1.health += 33;
                    }
                }
                if (p2.playerBounds.Intersects(healthPackBounds) && healthPackAcquired == false)
                {
                    if (p2.health < 99)
                    {
                        healthPackAcquired = true;
                        p2.health += 33;
                    }
                }

                //Colliding with a tree results in it moving slower

                if (p2.playerBounds.Intersects(treeRect1) || p2.playerBounds.Intersects(treeRect2))
                {
                    p2.tankSpeed = 1;
                }
                else
                    p2.tankSpeed = 2;
                if (p1.playerBounds.Intersects(treeRect1) || p1.playerBounds.Intersects(treeRect2))
                {
                    p1.tankSpeed = 1;
                }
                else
                    p1.tankSpeed = 2;
             
                //Determines winner
                if (p1.health == 0)
                    gameState = State.P2WinScreen;

                else if (p2.health == 0)
                    gameState = State.P1WinScreen;
                else if (p1.playerBounds.Intersects(p2.playerBounds))
                    gameState = State.DrawScreen;
            
             }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            if (gameState == State.MainMenu)
                spriteBatch.Draw(mainMenu, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height),
                    Color.White);
            else if (gameState == State.StageSelect)
                spriteBatch.Draw(mapSelect, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height),
                    Color.White);
            else if (gameState == State.P1WinScreen)
                spriteBatch.Draw(p1WinScreen, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height),
                    Color.White);
            else if (gameState == State.P2WinScreen)
                spriteBatch.Draw(p2WinScreen, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height),
                    Color.White);
            else if (gameState == State.DrawScreen)
                spriteBatch.Draw(drawScreen, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height),
                    Color.White);
            else if (gameState == State.Stage1Play || gameState == State.Stage2Play)
            {

                //draw map
                if (gameState == State.Stage1Play)
                    spriteBatch.Draw(level1BG, level1Viewport, Color.White);
                else if (gameState == State.Stage2Play)
                    spriteBatch.Draw(level2BG, level2Viewport, Color.White);
                //draw trees
                if (gameState == State.Stage1Play)
                {
                    spriteBatch.Draw(tree, treeRect1, Color.White);
                    spriteBatch.Draw(tree, treeRect2, Color.White);
                }
                if(gameState == State.Stage2Play)
                {
                    spriteBatch.Draw(snowbush, treeRect1, Color.Gray);
                    spriteBatch.Draw(snowbush, treeRect2, Color.Gray);
                }
                spriteBatch.Draw(p1.playerHealth, p1.healthRect, Color.OrangeRed);
                spriteBatch.Draw(p2.playerHealth, p2.healthRect, Color.Cyan);
                //draw healthpack
                if (healthPackAcquired == false)
                    spriteBatch.Draw(healthPack, new Rectangle((int)healthPackPos.X, (int)healthPackPos.Y, 60, 60), Color.White);
                //draw bomb
                if (bomb1Hit == false)
                    spriteBatch.Draw(bomb1, new Rectangle((int)bomb1Pos.X, (int)bomb1Pos.Y, 40, 40), Color.White);
                if (bomb2Hit == false)
                    spriteBatch.Draw(bomb2, new Rectangle((int)bomb2Pos.X, (int)bomb2Pos.Y, 40, 40), Color.White);
                if (bomb3Hit == false)
                    spriteBatch.Draw(bomb3, new Rectangle((int)bomb3Pos.X, (int)bomb3Pos.Y, 40, 40), Color.White);
                p1.Draw(gameTime, spriteBatch);
                p2.Draw(gameTime, spriteBatch);
                //draw bullet
                if (p1.fire == true)
                {
                    spriteBatch.Draw(p1.playerBullet, p1.bulletPos, Color.White);
                }
                if (p2.fire == true)
                {
                    spriteBatch.Draw(p2.playerBullet, p2.bulletPos, Color.White);
                }
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
