using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MooUI;
using MooUI.Widgets;

namespace MooUITest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MooViewer m = new MooViewer();
            Content = m;

            Accordion a = new Accordion(m.MaxContentWidth, m.MaxContentHeight);
            Checkbox c = new Checkbox("Checked!");

            a.AddChild(new ExpandingTextBox(20, 5));
            a.AddChild(c);

            m.SetContent(a);
        }
    }
}
