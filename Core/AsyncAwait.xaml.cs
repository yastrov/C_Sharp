/*
 See also http://blog.stephencleary.com/
 * and book:
 * Cleary S. - "Concurrency in C# Cookbook", year 2014 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace ProgressTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private CancellationTokenSource cancelSource;

        // Best version of approach:)
        // public static Task<T> Bar() for returned type T
        public static async Task Foo (IProgress<int> onProgressPercentChanged,
                                        CancellationToken cancellationToken)
        {
            for (int i = 0; i < 1000; i++)
            {
                if (i % 10 == 0)
                {
                    onProgressPercentChanged.Report(i / 10);
                    await Task.Delay(500, cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                }
            }
        }

        public static Task Bar(IProgress<int> onProgressPercentChanged,
                                CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                for (int i = 0; i < 1000; i++)
                    if (i % 10 == 0)
                    {
                        onProgressPercentChanged.Report(i / 10);
                        cancellationToken.ThrowIfCancellationRequested();
                    }
            });
        }

        private async void Start_Button1_Click(object sender, RoutedEventArgs e)
        {
            var progress = new Progress<int>(i => ProgressIndicatorBar.Value=i);
            //You may declare it here, but in this moments, cancel from other button click;
            //var cancelSource = new CancellationTokenSource();
            cancelSource = new CancellationTokenSource();
            try {
                //In theoretical, you may pass "await" keyword and see the result:
                await Foo(progress, cancelSource.Token);
                await Bar(progress, cancelSource.Token);
            }
            catch (OperationCanceledException ex) {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Stop_Buton_Click(object sender, RoutedEventArgs e)
        {
            cancelSource.Cancel();
        }
    }
}
