using System;
using Monocle;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Celeste.Mod.AnalogViewer
{
    class AnalogDisplay : Entity
    {
        public AnalogDisplay()
        {
            Tag = Tags.HUD;
        }

        public override void Render()
        {
            if (!AnalogViewerModule.Settings.Enabled) return;
            const int labelX = 25;
            const int coordX = 100;
            const int angleX = 425;
            //int arrowX = 525;
            //int arrowY = 200;
            int arrowX = (int)Math.Round(Engine.Instance.GraphicsDevice.PresentationParameters.BackBufferWidth / (float)2);
            int arrowY = (int)Math.Round(Engine.Instance.GraphicsDevice.PresentationParameters.BackBufferHeight / (float)2);
            int textY = 200;
            Vector2 arrow = new Vector2(arrowX, arrowY);
            Vector2 offset = Vector2.Zero;
            GamePadDeadZone deadZone;
            Level level = SceneAs<Level>();

            deadZone = GamePadDeadZone.IndependentAxes;
            MTexture icon = GetIcon();
            if (level.Tracker.GetEntity<Player>() != null) { arrow = level.WorldToScreen(level.Tracker.GetEntity<Player>().TopLeft); }
            arrow = Vector2.Add(arrow, GetAnalogCoordinatesVec2(deadZone) * 100);
            icon?.Draw(arrow, Vector2.Zero, Color.White, ActiveFont.LineHeight / icon.Height);

            if (AnalogViewerModule.Settings.ShowIndepdentAxes)
            {
                DrawText("+ : ", labelX, textY);
                DrawText(GetAnalogCoordinates(deadZone), coordX, textY);
                DrawText(GetAnalogAngle(deadZone), angleX, textY);
                textY += 75;
            }

            if (AnalogViewerModule.Settings.ShowNoDeadZone)
            {
                deadZone = GamePadDeadZone.None;
                DrawText("X:", labelX, textY);
                DrawText(GetAnalogCoordinates(deadZone), coordX, textY);
                DrawText(GetAnalogAngle(deadZone), angleX, textY);
                textY += 75;
            }

            if (AnalogViewerModule.Settings.ShowCircularDeadZone)
            {
                deadZone = GamePadDeadZone.Circular;
                DrawText("O:", labelX, textY);
                DrawText(GetAnalogCoordinates(deadZone), coordX, textY);
                DrawText(GetAnalogAngle(deadZone), angleX, textY);
            }
        }

        private string GetAnalogCoordinates(GamePadDeadZone DeadZoneConfig = GamePadDeadZone.IndependentAxes)
        {
            GamePadState gamePadState = GamePad.GetState(0, DeadZoneConfig);
            return string.Format("({0:F2}, {1:F2})", gamePadState.ThumbSticks.Left.X, gamePadState.ThumbSticks.Left.Y);
        }

        private Vector2 GetAnalogCoordinatesVec2(GamePadDeadZone DeadZoneConfig = GamePadDeadZone.IndependentAxes)
        {
            GamePadState gamePadState = GamePad.GetState(0, DeadZoneConfig);
            return new Vector2(gamePadState.ThumbSticks.Left.X, -gamePadState.ThumbSticks.Left.Y);
        }

        private string GetAnalogAngle(GamePadDeadZone DeadZoneConfig = GamePadDeadZone.IndependentAxes)
        {
            GamePadState gamePadState = GamePad.GetState(0, DeadZoneConfig);
            return string.Format("{0:F0}°", gamePadState.ThumbSticks.Left.Angle() * 180.0 / Math.PI);
        }

        public MTexture GetIcon()
        {
            Vector2 corrected = CorrectDashPrecision(Input.LastAim);

            return Input.GuiDirection(corrected);
        }

        private Vector2 CorrectDashPrecision(Vector2 dir)
        {
            if (dir.X != 0f && Math.Abs(dir.X) < 0.001f)
            {
                dir.X = 0f;
                dir.Y = Math.Sign(dir.Y);
            }
            else if (dir.Y != 0f && Math.Abs(dir.Y) < 0.001f)
            {
                dir.Y = 0f;
                dir.X = Math.Sign(dir.X);
            }
            return dir;
        }

        private void DrawText(string text, int x, int y)
        {
            ActiveFont.DrawOutline(text,
                                position: new Vector2(x, y),
                                justify: new Vector2(0f, 0f),
                                scale: Vector2.One,
                                color: Color.White, stroke: 2f,
                                strokeColor: Color.Black);
        }
    }
}
