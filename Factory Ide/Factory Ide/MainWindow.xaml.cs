﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Factory_Ide.Controls;

namespace Factory_Ide
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private struct Component
        {
            public string Name;
            public Type Type;
        }
        private List<Type> m_components;

        public MainWindow()
        {
            InitializeComponent();
            LoadSupportedComponents();

            // AddCanvasComponents();

            LbxComponents.MouseDoubleClick += (sender, args) =>
            {
                Control control = (Control) Activator.CreateInstance(m_components[LbxComponents.SelectedIndex]);
                SetTextOnControl(control, control.GetType().Name);
                AddComponent(control);
            };
            ClearProperties();
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

        private void AddCanvasComponents()
        {
            AddComponent(new Button()
            {
                Content = "Button"
            });
            
        }

        private void AddComponent(Control control)
        {
            Canvas.SetTop(control, 10);
            Canvas.SetLeft(control, 10);
            CvsInterface.Children.Add(control);


            control.GotFocus += OnCanvasComponentSelected;
            control.Focus();
        }

        private void UpdatePropertiesEvent(object sender, SelectionChangedEventArgs args)
        {

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

        private void ClearProperties()
        {
            LbxProperties.ItemsSource = null;
        }

        private void OnCanvasComponentSelected(object sender, RoutedEventArgs args)
        {
            ClearProperties();
            Control c = (Control) sender;
            List<PropertyControl> properties = new List<PropertyControl>();

            if (c is ContentControl control)
            {
                var content = new PropertyControl("Content", control.Content.ToString());
                content.OnTextboxDataChanged += (o, eventArgs) => control.Content = (o as TextBox)?.Text;
                properties.Add(content);
            } else if (c is TextBox textbox)
            {
                var content = new PropertyControl("Content", textbox.Text.ToString());
                content.OnTextboxDataChanged += (o, eventArgs) => textbox.Text = (o as TextBox)?.Text;
                properties.Add(content);
            }

            var height = new PropertyNumberControl("Height", c.Height);
            height.OnTextboxDataChanged += (o, eventArgs) =>
            {
                if (o is TextBox textbox)
                {
                    height.SetPropertyValue(textbox.Text);
                    textbox.Text = height.PropertyValue.ToString();
                    c.Height = height.PropertyValue;
                }

            };
            properties.Add(height);

            var width = new PropertyNumberControl("Width", c.Width);
            width.OnTextboxDataChanged += (o, eventArgs) =>
            {
                if (o is TextBox textbox)
                {
                    width.SetPropertyValue(textbox.Text);
                    textbox.Text = width.PropertyValue.ToString();
                    c.Width = width.PropertyValue;
                }
            };
            properties.Add(width);

            var left = new PropertyNumberControl("X", Canvas.GetLeft(c));
            left.OnTextboxDataChanged += (o, eventArgs) =>
            {
                if (o is TextBox textbox)
                {
                    left.SetPropertyValue(textbox.Text);
                    textbox.Text = left.PropertyValue.ToString();
                    CvsInterface.Children.Remove(c);
                    Canvas.SetLeft(c, left.PropertyValue);
                    CvsInterface.Children.Add(c);
                }
            };
            properties.Add(left);

            var top = new PropertyNumberControl("Y", Canvas.GetTop(c));
            top.OnTextboxDataChanged += (o, eventArgs) =>
            {
                if (o is TextBox textbox)
                {
                    top.SetPropertyValue(textbox.Text);
                    textbox.Text = top.PropertyValue.ToString();
                    CvsInterface.Children.Remove(c);
                    Canvas.SetTop(c, top.PropertyValue);
                    CvsInterface.Children.Add(c);
                }
            };
            properties.Add(top);
            LbxProperties.ItemsSource = properties;
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


    }
}
