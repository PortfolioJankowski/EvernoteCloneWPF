using EvernoteCloneWPF.Model;
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

namespace EvernoteCloneWPF.View.UserControls
{
    /// <summary>
    /// Logika interakcji dla klasy DisplayNote.xaml
    /// </summary>
    public partial class DisplayNote : UserControl
    {
        public DisplayNote()
        {
            InitializeComponent();
        }

        public Note Note
        {
            get { return (Note)GetValue(NoteProperty); }
            set { SetValue(NoteProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Note.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NoteProperty =
            DependencyProperty.Register("Note", typeof(Note), typeof(DisplayNote), new PropertyMetadata(null, SetValues));

        private static void SetValues(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DisplayNote ctrl = d as DisplayNote;
            if (ctrl != null)
            {
                ctrl.DataContext = ctrl.Note;
            }
        }
    }
}
