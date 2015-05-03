using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace MissAndCan
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        #region Inotify
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
        private bool _isEnabled = true;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                NotifyPropertyChanged("IsEnabled");
            }
        }
        private ObservableCollection<StateView> _combinations = null;
        public ObservableCollection<StateView> Combinations
        {
            get { return _combinations; }
            set
            {
                _combinations = value;
                NotifyPropertyChanged("Combinations");
            }
        }
        private CancellationTokenSource cancelSource = null;
        private int boatSize = 2;
        public int BoatSize
        {
            get { return boatSize; }
            set
            {
                boatSize = value;
                NotifyPropertyChanged("BoatSize");
            }
        }
        
        private int numberOfEach = 3;
        public int NumberOfEach
        {
            get { return numberOfEach; }
            set
            {
                numberOfEach = value;
                NotifyPropertyChanged("NumberOfEach");
            }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        async private void StartButtoCllick(object sender, RoutedEventArgs e)
        {
            cancelSource = new CancellationTokenSource();
            Cursor = Cursors.Wait;
            IsEnabled = false;
            try
            {
                var result = await MakeResult(cancelSource.Token);
                if (result != null && result.Count != 0)
                    Combinations = new ObservableCollection<StateView>(result);
                else
                    MessageBox.Show("No solution!", Title, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch(OperationCanceledException ex)
            {
                MessageBox.Show("Cancelled by user!", Title, MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), Title, MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            finally
            {
                Cursor = Cursors.Arrow;
                IsEnabled = true;
            }
        }

        async private Task<List<StateView>> MakeResult(CancellationToken cancelToken)
        {
            return await Task.Run(() =>
            {
                List<StateView> resultCollection = null;
                //Do not use this approach in enterprise!.
                StateFactory.SetNumberOfEach(NumberOfEach);
                var p = new Problem(StateFactory.Make(NumberOfEach, NumberOfEach, BoatState.Left, 0),
                    StateFactory.Make(0, 0, BoatState.Right, 0), BoatSize);
                var result = Algorithm.UniformCostSearch(p, cancelToken);
                if (result != null)
                {
                    Stack<State> stack = new Stack<State>();
                    while (result != null)
                    {
                        stack.Push(result);
                        result = result.PrevState;
                    }
                    cancelToken.ThrowIfCancellationRequested();
                    resultCollection = new List<StateView>(stack.Count);
                    int n = 0;
                    foreach (var state in stack)
                    {
                        resultCollection.Add(new StateView(state, n++));
                    }
                    return resultCollection;
                }
                return new List<StateView>();
            }, cancelToken);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (cancelSource != null)
            {
                try
                {
                    cancelSource.Cancel();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), Title, MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void CancelButtoCllick(object sender, RoutedEventArgs e)
        {
            if (cancelSource != null)
            {
                cancelSource.Cancel();
            }
        }

    }
}
