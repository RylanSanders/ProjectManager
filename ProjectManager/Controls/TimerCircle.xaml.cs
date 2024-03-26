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

namespace ProjectManager.Controls
{
    /// <summary>
    /// Interaction logic for TimerCircle.xaml
    /// </summary>
    public partial class TimerCircle : UserControl
    {
        private TimeSpan _currentTime = TimeSpan.Zero;
        public TimeSpan CurrentTime {
            get
            {
                return _currentTime;
            }
            set
            {
                _currentTime = value;
                DurationTextBox.Text = CurrentTime.ToString(@"dd\.hh\:mm\:ss\.ff");
            }
        }

        private TimeSpan _currentDailyTime = TimeSpan.Zero;
        public TimeSpan CurrentDailyTime
        {
            get
            {
                return _currentDailyTime;
            }
            set
            {
                _currentDailyTime = value;
                DailyTimeTextBox.Text = CurrentDailyTime.ToString(@"dd\.hh\:mm\:ss\.ff");
            }
        }

        public TimerCircle()
        {
            InitializeComponent();

            DurationTextBox.Text = CurrentTime.ToString();
            DailyTimeTextBox.Text = CurrentDailyTime.ToString(@"dd\.hh\:mm\:ss\.ff");
        }
    }
}
