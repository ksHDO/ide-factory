using System;
using System.Collections.Generic;
using System.IO;
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
using AppBuilder;
using AppBuilder.Commands;
using Factory_Ide.Commands;
using Factory_Ide.Controls;
using Microsoft.CSharp;
using Microsoft.Win32;

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

        private LanguageFactory m_languageFactory;
        private List<string> m_supportedLanguages;
        private int m_selectedLanguage;
        private List<Type> m_components;

        private Dictionary<string, Type> m_associatedComponents = new Dictionary<string, Type>
        {
            { "label", typeof(LabelTextbox) },
            { "textbox", typeof(TextBox) },
            { "button", typeof(Button) }
        };
        private readonly CommandHistory m_history;

        static MainWindow()
        {
            Instance = new MainWindow();
        }

        private MainWindow()
        {
            InitializeComponent();

            m_languageFactory = new LanguageFactory();
            m_history = new CommandHistory();

            m_supportedLanguages = m_languageFactory.GetSupportedLanguages();
            MnuLanguage.ItemsSource = m_supportedLanguages.Select(s =>
            {
                var item = new MenuItem()
                {
                    Header = s,
                    IsCheckable = true
                };
                item.Checked += (sender, args) =>
                {
                    if (sender is MenuItem mnuItem)
                    {
                        LoadSupportedComponents(GetLanguageIndex(mnuItem));
                    }
                };
                return item;
            });

            LoadSupportedComponents(0);
            
            LbxComponents.MouseDoubleClick += (sender, args) =>
            {
                Control control = (Control) Activator.CreateInstance(m_components[LbxComponents.SelectedIndex]);
                if (control is TextBoxBase textbox)
                {
                    textbox.IsReadOnly = true;
                }
                SetTextOnControl(control, control.GetType().Name);
                AddComponent(control);
            };
            ClearProperties();
        }

        private int GetLanguageIndex(MenuItem menuItem)
        {
            for (int i = 0; i < MnuLanguage.Items.Count; ++i)
            {
                if (ReferenceEquals(MnuLanguage.Items[i], menuItem))
                {
                    return i;
                }
            }

            return -1;
        }

        public void ResetComponents()
        {
            CvsInterface.Children.Clear();
            ClearProperties();
        }

        private void LoadSupportedComponents(int languageIndex)
        {
            int languagesCount = m_supportedLanguages.Count;
            if (languageIndex < 0 || languageIndex >= languagesCount)
                throw new IndexOutOfRangeException();
            
            for (int i = 0; i < languagesCount; ++i)
            {
                ((MenuItem)MnuLanguage.Items[i]).IsChecked = i == languageIndex;
            }

            m_selectedLanguage = languageIndex;
            m_components = new List<Type>();

            var supportedComponents = m_languageFactory.GetSupportedComponentsFor(m_supportedLanguages[languageIndex]);
            foreach (var component in supportedComponents)
            {
                Type t = GetAssociatedComponent(component);

                m_components.Add(t);
            }

            LbxComponents.ItemsSource = m_components.Select(s => s.Name);
            
        }

        private Type GetAssociatedComponent(string component)
        {
            return m_associatedComponents[component];
        }

        private string GetAssociatedComponent(Type type)
        {
            return m_associatedComponents.FirstOrDefault(i => i.Value == type).Key;
        }

        private void AddComponent(Control control)
        {
            Canvas.SetTop(control, 10);
            Canvas.SetLeft(control, 10);

            control.GotFocus += OnCanvasComponentSelected;
            m_history.PerformCommand(new AddComponentCommand(CvsInterface, control));
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
                content.TbxValue.LostFocus += UpdateComponentEvent(textBox =>
                {
                    m_history.PerformCommand(new PropertyEditedCommand(control, "Content", textBox, textBox.Text));
                }, content);
                properties.Add(content);
            }
            else if (c is TextBox textbox)
            {
                var content = new PropertyControl("Content", textbox.Text);
                content.TbxValue.LostFocus += UpdateComponentEvent(textBox =>
                {
                    m_history.PerformCommand(new PropertyEditedCommand(textbox, "Text", textBox, textBox.Text));
                }, content);
                properties.Add(content);
            }

            var height = new PropertyNumberControl("Height", c.Height);
            height.TbxValue.LostFocus += UpdateComponentEvent(textBox =>
            {
                m_history.PerformCommand(new PropertyEditedCommand(c, "Height", textBox, height.PropertyValue));
            }, height);
            properties.Add(height);

            var width = new PropertyNumberControl("Width", c.Width);
            width.TbxValue.LostFocus += UpdateComponentEvent(textBox =>
            {
                m_history.PerformCommand(new PropertyEditedCommand(c, "Width", textBox, width.PropertyValue));
            }, width);
            properties.Add(width);

            var left = new PropertyNumberControl("X", Canvas.GetLeft(c));
            left.TbxValue.LostFocus += UpdateComponentEvent(textBox =>
            {
                m_history.PerformCommand(new CanvasPositionCommand(
                    CanvasPositionCommand.Direction.Left, CvsInterface, c, textBox, left.PropertyValue)
                );
            }, left);
            properties.Add(left);

            var top = new PropertyNumberControl("Y", Canvas.GetTop(c));
            top.TbxValue.LostFocus += UpdateComponentEvent(textBox =>
            {
                m_history.PerformCommand(new CanvasPositionCommand(
                    CanvasPositionCommand.Direction.Top, CvsInterface, c, textBox, top.PropertyValue)
                );
            }, top);
            properties.Add(top);

            var remove = new Button()
            {
                Content = "Remove Component"
            };
            remove.Click += (o, eventArgs) =>
            {
                m_history.PerformCommand(new RemoveComponentCommand(CvsInterface, c));
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
            var ofd = new SaveFileDialog()
            {
              
            };
            if (ofd.ShowDialog() == true)
            {
                string file = System.IO.Path.GetFileName(ofd.FileName);
                string path = System.IO.Path.GetDirectoryName(ofd.FileName);
                var elements = new List<ElementInfo>();
                foreach (Control child in CvsInterface.Children)
                {
                    string text = "";
                    if (child is ContentControl cc)
                        text = cc.Content.ToString();
                    else if (child is TextBox tbb)
                        text = tbb.Text;
                    elements.Add(new ElementInfo(GetAssociatedComponent(child.GetType()), text, (int) Canvas.GetTop(child), (int) Canvas.GetLeft(child), (int) child.Width, (int) child.Height));
                }
                m_languageFactory.BuildApplication(m_supportedLanguages[m_selectedLanguage], elements, path, file);
            };
            
        }

        private void MnuExit_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MnuUndo_OnClick(object sender, RoutedEventArgs e)
        {
            m_history.UndoCommand();
        }

        private void MnuRedo_OnClick(object sender, RoutedEventArgs e)
        {
            m_history.RedoCommand();
        }
    }
}
