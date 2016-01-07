using DescentDirX.Characters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DescentDirX.UI
{
    class GameCamera
    {
        private const int CAMERA_MOVE_AMOUNT = 8;
        private const int SCREEN_OFFSET_SCROLL = 80;

        private static GameCamera instance;
        private int GlobalOffsetX { get; set; }
        private int GlobalOffsetY { get; set; }
        private Viewport Viewport { get; set; }
        public bool FixedPosition { get; set; } = true;
        public Rectangle MapBounds { get; set; }
        public Character ActualCharacter { get; set; }

        private Viewport screenViewport;
        public Viewport ScreenViewport {
            get { return screenViewport; }
            set {
                screenViewport = value;
                Viewport = value;
        } }

        public static GameCamera Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameCamera();
                }
                return instance;
            }
            private set { instance = value; }
        }

        private GameCamera()
        {
        }

        public void MoveLeft()
        {
            if (Math.Abs(GlobalOffsetX) + Viewport.Width - SCREEN_OFFSET_SCROLL < MapBounds.Width)
            {
                GlobalOffsetX -= CAMERA_MOVE_AMOUNT;
            }
        }

        public void MoveRight()
        {
            if (GlobalOffsetX < MapBounds.X + SCREEN_OFFSET_SCROLL)
            {
                GlobalOffsetX += CAMERA_MOVE_AMOUNT;
            }
        }

        public void MoveUp()
        {
            if (GlobalOffsetY < MapBounds.Y + SCREEN_OFFSET_SCROLL)
            {
                GlobalOffsetY += CAMERA_MOVE_AMOUNT;
            }
        }

        public void MoveDown()
        {
            if (GlobalOffsetY + Viewport.Height > MapBounds.Height)
            { 
                GlobalOffsetY -= CAMERA_MOVE_AMOUNT;
            }
        }

        public void Update(int mouseX, int mouseY)
        {
            if (mouseX >= 0 && mouseX < SCREEN_OFFSET_SCROLL && !FixedPosition)
            {
                MoveRight();
            }
            else if (mouseX <= Viewport.Width && mouseX > Viewport.Width - SCREEN_OFFSET_SCROLL && !FixedPosition)
            {
                MoveLeft();
            }

            if (mouseY >= 0 && mouseY < SCREEN_OFFSET_SCROLL && !FixedPosition && mouseX >= 0 && mouseX <= Viewport.Width)
            {
                MoveUp();
            }
            else if (mouseY <= Viewport.Height && mouseY > Viewport.Height - SCREEN_OFFSET_SCROLL && !FixedPosition && mouseX >= 0 && mouseX <= Viewport.Width)
            {
                MoveDown();
            }
        }

        public Vector2 GetMovedVector(Vector2 oldVector)
        {
            var newVector = new Vector2(oldVector.X + GlobalOffsetX, oldVector.Y + GlobalOffsetY);
            return newVector;
        }

        public Rectangle GetMovedRectangle(Rectangle oldRect)
        {
            var newRect = new Rectangle(oldRect.X + GlobalOffsetX, oldRect.Y + GlobalOffsetY, oldRect.Width, oldRect.Height);
            return newRect;
        }

        public void ReduceViewport(Rectangle rect)
        {
            Viewport = new Viewport(Viewport.X, Viewport.Y, Viewport.Width - rect.Width, Viewport.Height);
        }

        public static int GetScreenWidth()
        {
            return GameCamera.Instance.ScreenViewport.Width;
        }

        public static int GetScreenHeight()
        {
            return GameCamera.Instance.ScreenViewport.Height;
        }
    }
}
