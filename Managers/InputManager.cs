using System.Windows.Forms;

namespace AP_Final_Project.Managers
{
    public class InputManager
    {
        public bool MoveLeft { get; private set; }
        public bool MoveRight { get; private set; }
        public bool MoveUp { get; private set; }
        public bool MoveDown { get; private set; }
        public bool Shooting { get; private set; }
        public bool PausePressed { get; private set; }

        public void OnKeyDown(Keys key)
        {
            switch (key)
            {
                case Keys.Left:
                case Keys.A:
                    MoveLeft = true;
                    break;
                case Keys.Right:
                case Keys.D:
                    MoveRight = true;
                    break;
                case Keys.Up:
                case Keys.W:
                    MoveUp = true;
                    break;
                case Keys.Down:
                case Keys.S:
                    MoveDown = true;
                    break;
                case Keys.Space:
                    Shooting = true;
                    break;
                case Keys.Escape:
                    PausePressed = true;
                    break;
            }
        }

        public void OnKeyUp(Keys key)
        {
            switch (key)
            {
                case Keys.Left:
                case Keys.A:
                    MoveLeft = false;
                    break;
                case Keys.Right:
                case Keys.D:
                    MoveRight = false;
                    break;
                case Keys.Up:
                case Keys.W:
                    MoveUp = false;
                    break;
                case Keys.Down:
                case Keys.S:
                    MoveDown = false;
                    break;
                case Keys.Space:
                    Shooting = false;
                    break;
            }
        }

        public void ConsumePause()
        {
            PausePressed = false;
        }
    }
}
