using AppBuilder.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Factory_Ide.Commands
{
    public class AddComponentCommand : IFactoryIdeCommand
    {
        private Canvas m_canvas;
        private Control m_control;

        public AddComponentCommand(Canvas canvas, Control control)
        {
            m_control = control;
            m_canvas = canvas;
        }

        public void Do()
        {
            m_canvas.Children.Add(m_control);
            MainWindow.Instance.SelectedControl = m_control;
        }

        public void Undo()
        {
            m_canvas.Children.Remove(m_control);
            // ReSharper disable once PossibleUnintendedReferenceComparison
            if (MainWindow.Instance.SelectedControl == m_control)
                MainWindow.Instance.SelectedControl = null;
        }
    }
}
