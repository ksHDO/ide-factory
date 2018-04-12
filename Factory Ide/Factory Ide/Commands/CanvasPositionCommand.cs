using System;
using System.Windows;
using System.Windows.Controls;

namespace Factory_Ide.Commands
{
    public class CanvasPositionCommand : IFactoryIdeCommand
    {
        public enum Direction
        {
            Top, Left, Bottom, Right
        }

        private Direction m_direction;
        private Canvas m_canvas;
        private Control m_control;
        private double m_previousPosition;
        private double m_setPosition;
        private TextBox m_textBox;

        public CanvasPositionCommand(Direction direction, Canvas canvas, Control control, TextBox textBox, double position)
        {
            m_direction = direction;
            m_canvas = canvas;
            m_control = control;
            m_previousPosition = GetControlPos(direction, control);
            m_setPosition = position;
            m_textBox = textBox;
        }

        public void Do()
        {
            SetControl(m_setPosition);
        }

        public void Undo()
        {
            SetControl(m_previousPosition);
        }

        private void SetControl(double pos)
        {
            var inCanvas = m_canvas.Children.Contains(m_control);

            if (inCanvas) m_canvas.Children.Remove(m_control);

            SetControlPos(m_direction, m_control, pos);

            if (inCanvas) m_canvas.Children.Add(m_control);

            if (MainWindow.Instance.SelectedControl == m_control)
                m_textBox.Text = pos.ToString();
        }

        private static double GetControlPos(Direction direction, UIElement element)
        {
            double value = 0.0;
            switch (direction)
            {
                case Direction.Top:
                    value = Canvas.GetTop(element);
                    break;
                case Direction.Left:
                    value = Canvas.GetLeft(element);
                    break;
                case Direction.Bottom:
                    value = Canvas.GetBottom(element);
                    break;
                case Direction.Right:
                    value = Canvas.GetRight(element);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }

            return value;
        }

        private static void SetControlPos(Direction direction, UIElement element, double position)
        {
            switch (direction)
            {
                case Direction.Top:
                    Canvas.SetTop(element, position);
                    break;
                case Direction.Left:
                    Canvas.SetLeft(element, position);
                    break;
                case Direction.Bottom:
                    Canvas.SetBottom(element, position);
                    break;
                case Direction.Right:
                    Canvas.SetRight(element, position);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }
    }
}