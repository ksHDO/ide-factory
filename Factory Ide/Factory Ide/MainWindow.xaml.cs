using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Factory_Ide.Commands;
using Factory_Ide.Controls;
using ICommand = System.Windows.Input.ICommand;

namespace Factory_Ide
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; }
        private Control m_selectedControl;
        public Control SelectedControl
        {
            get => m_selectedControl;
            set
            {
                m_selectedControl = value;
                ClearProperties();
                if (value != null)
                    ShowProperties(value);
            }
        }

        private List<Type> m_components;

        private readonly Stack<IFactoryIdeCommand> m_undoHistory;
        private readonly Stack<IFactoryIdeCommand> m_redoHistory;

        static MainWindow()
        {
            Instance = new MainWindow();
        }

        private MainWindow()
        {
            InitializeComponent();
            LoadSupportedComponents();

            // AddCanvasComponents();
            m_undoHistory = new Stack<IFactoryIdeCommand>(200);
            m_redoHistory = new Stack<IFactoryIdeCommand>(200);

            LbxComponents.MouseDoubleClick += (sender, args) =>
            {
                Control control = (Control) Activator.CreateInstance(m_components[LbxComponents.SelectedIndex]);
                SetTextOnControl(control, control.GetType().Name);
                AddComponent(control);
            };
            ClearProperties();
        }

        public void PerformCommand(IFactoryIdeCommand command)
        {
            command.Do();
            m_redoHistory.Clear();
            m_undoHistory.Push(command);
        }

        public void UndoCommand()
        {
            if (m_undoHistory.Count <= 0) return;
            var command = m_undoHistory.Pop();
            command.Undo();
            m_redoHistory.Push(command);
        }

        public void RedoCommand()
        {
            if (m_redoHistory.Count <= 0) return;
            var command = m_redoHistory.Pop();
            command.Do();
            m_undoHistory.Push(command);
        }

        public void ResetComponents()
        {
            CvsInterface.Children.Clear();
            ClearProperties();
        }

        private void LoadSupportedComponents()
        {
            m_components = new List<Type>();

            m_components.Add(typeof(Button));
            m_components.Add(typeof(LabelTextbox));
            m_components.Add(typeof(TextBox));


            LbxComponents.ItemsSource = m_components.Select(s => s.Name);
            
        }

        private void AddComponent(Control control)
        {
            Canvas.SetTop(control, 10);
            Canvas.SetLeft(control, 10);

            control.GotFocus += OnCanvasComponentSelected;
            PerformCommand(new AddComponentCommand(CvsInterface, control));

        }

        private static void SetTextOnControl(Control c, string text)
        {
            if (c is ContentControl contentControl)
            {
                contentControl.Content = text;
            }
            else if (c is TextBox textBox)
            {
                textBox.Text = text;
            }
        }

        public void ClearProperties()
        {
            LbxProperties.ItemsSource = null;
        }

        public void ShowProperties(Control c)
        {
            ClearProperties();
            List<Control> properties = new List<Control>();

            if (c is ContentControl control)
            {
                var content = new PropertyControl("Content", control.Content.ToString());
                content.TbxValue.LostFocus += UpdateComponentEvent(textBox => content.Content = textBox.Text, content);
                properties.Add(content);
            }
            else if (c is TextBox textbox)
            {
                var content = new PropertyControl("Content", textbox.Text.ToString());
                content.TbxValue.LostFocus += (o, eventArgs) => UpdateComponentEvent(textBox => content.Content = textBox.Text, content);
                properties.Add(content);
            }

            var height = new PropertyNumberControl("Height", c.Height);
            height.TbxValue.LostFocus += UpdateComponentEvent(textBox =>
            {
                height.SetPropertyValue(textBox.Text);
                textBox.Text = height.PropertyValue.ToString();
                c.Height = height.PropertyValue;
            }, height);
            properties.Add(height);

            var width = new PropertyNumberControl("Width", c.Width);
            width.TbxValue.LostFocus += UpdateComponentEvent(textBox =>
            {
                width.SetPropertyValue(textBox.Text);
                textBox.Text = width.PropertyValue.ToString();
                c.Width = width.PropertyValue;
            }, width);
            properties.Add(width);

            var left = new PropertyNumberControl("X", Canvas.GetLeft(c));
            left.TbxValue.LostFocus += UpdateComponentEvent(propertyControl =>
            {
                CvsInterface.Children.Remove(c);
                Canvas.SetLeft(c, left.PropertyValue);
                CvsInterface.Children.Add(c);
            }, left);
            properties.Add(left);

            var top = new PropertyNumberControl("Y", Canvas.GetTop(c));
            top.TbxValue.LostFocus += UpdateComponentEvent(textBox =>
            {
                CvsInterface.Children.Remove(c);
                Canvas.SetTop(c, top.PropertyValue);
                CvsInterface.Children.Add(c);
            }, top);
            properties.Add(top);

            var remove = new Button()
            {
                Content = "Remove Component"
            };
            remove.Click += (o, eventArgs) =>
            {
                PerformCommand(new RemoveComponentCommand(CvsInterface, c));
            };

            properties.Add(remove);
            LbxProperties.ItemsSource = properties;
        }

        private RoutedEventHandler UpdateComponentEvent(Action<TextBox> action, PropertyControl propertyControl)
        {
            return (sender, args) =>
            {
                if (sender is TextBox textbox)
                {
                    if (propertyControl is PropertyNumberControl numberControl)
                    {
                        numberControl.SetPropertyValue(numberControl.TbxValue.Text);
                        numberControl.TbxValue.Text = numberControl.PropertyValue.ToString();
                    }
                    action(textbox);
                }
            };
        }

        public void OnCanvasComponentSelected(object sender, RoutedEventArgs args)
        {
            ClearProperties();
            Control c = (Control) sender;
            SelectedControl = c;
            ShowProperties(c);
        }

        private void MnuNew_OnClick(object sender, RoutedEventArgs e)
        {
            ResetComponents();
        }

        private void MnuExport_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MnuExit_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }


        private void MnuUndo_OnClick(object sender, RoutedEventArgs e)
        {
            UndoCommand();
        }

        private void MnuRedo_OnClick(object sender, RoutedEventArgs e)
        {
            RedoCommand();
        }
    }
}
