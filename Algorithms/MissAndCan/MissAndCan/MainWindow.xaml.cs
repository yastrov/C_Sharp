﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

        public MainWindow()
        {
            InitializeComponent();
        }

        async private void StartButtoCllick(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            IsEnabled = false;
            try
            {
                var result = await MakeResult();
                if (result != null && result.Count != 0)
                    Combinations = new ObservableCollection<StateView>(result);
                else
                    MessageBox.Show("No solution!", Title, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            finally
            {
                Cursor = Cursors.Arrow;
                IsEnabled = true;
            }
        }

        async private Task<List<StateView>> MakeResult()
        {
            return await Task.Run(() =>
            {
                List<StateView> resultCollection = null;
                var p = new Problem(new State(3, 3, BoatState.Left, 0),
                new State(0, 0, BoatState.Right, 0));
                var result = Algorithm.UniformCostSearch(p);
                if (result != null)
                {
                    Stack<State> stack = new Stack<State>();
                    while (result != null)
                    {
                        stack.Push(result);
                        result = result.PrevState;
                    }
                    resultCollection = new List<StateView>(stack.Count);
                    int n = 0;
                    foreach (var state in stack)
                    {
                        resultCollection.Add(new StateView(state, n++));
                    }
                    return resultCollection;
                }
                return new List<StateView>();
            });
        }


    }
}