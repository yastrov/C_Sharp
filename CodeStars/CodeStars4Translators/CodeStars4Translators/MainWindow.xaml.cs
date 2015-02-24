using System;
using System.Collections.Generic;
using System.IO;
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

namespace CodeStars4Translators
{
    public interface IProblem
    {
        KeyValuePair<string, string> GetStart();
        bool IsGoal(KeyValuePair<string, string> pair);
        List<KeyValuePair<string, string>> GetSuccessors(KeyValuePair<string, string> pair);
        int GetCostOfActions(Queue<KeyValuePair<string, string>> queue);
    }

    /// <summary>
    /// Our problem
    /// </summary>
    public class Problem: IProblem
    {
        private List<KeyValuePair<string, string>> _listOfPairs;
        private string _start, _goal;
        public Problem(List<KeyValuePair<string, string>> listOfPairs, string start, string goal)
        {
            _listOfPairs = listOfPairs;
            _start = start;
            _goal = goal;
        }
        public KeyValuePair<string, string> GetStart()
        {
            return _listOfPairs.Where((p) => p.Key == _start).First();
        }

        /// <summary>
        /// Is it good end of our translators chain (Queue)?
        /// </summary>
        /// <param name="pair">Translator unit</param>
        /// <returns></returns>
        public bool IsGoal(KeyValuePair<string, string> pair)
        {
            if (pair.Value.Equals(_goal))
                return true;
            else return false;
        }

        /// <summary>
        /// Possible translators for this language (for language what know our translator)
        /// </summary>
        /// <param name="pair">Translator unit</param>
        /// <returns></returns>
        public List<KeyValuePair<string, string>> GetSuccessors(KeyValuePair<string, string> pair)
        {
            return _listOfPairs.Where((p) => p.Key.Equals(pair.Value)).Select((p) => p).ToList();
        }
    
        public int GetCostOfActions(Queue<KeyValuePair<string, string>> queue)
        {
            return queue.Count;
        }
    }

    /// <summary>
    /// Algorythms collection
    /// </summary>
    public static class Algorythms
    {
        public static Queue<KeyValuePair<string, string>> UniformCostSearch(IProblem problem)
        {
            //It's for me!: SortedList<TKey, TValue>,Queue<T>,Stack<>
            Queue<KeyValuePair<string, string>> result = null; 
            HashSet<KeyValuePair<string, string>> visited = new HashSet<KeyValuePair<string, string>>();
            // It's need other collection as it possible!
            List<Queue<KeyValuePair<string, string>>> fringe = new List<Queue<KeyValuePair<string, string>>>();
            var firstQ = new Queue<KeyValuePair<string, string>>();
            firstQ.Enqueue(problem.GetStart());
            fringe.Add(firstQ);
            while(fringe.Count != 0)
            {
                // If we use heuristic function with problem.GetCostOfActions(q), we can take A* algorythm:).
                // It's need other collection as it possible!
                Queue<KeyValuePair<string, string>> currPath = fringe.OrderBy((q) => problem.GetCostOfActions(q)).First();
                fringe.Remove(currPath);
                KeyValuePair<string, string> currNode = currPath.Last();
                if(problem.IsGoal(currNode))
                {
                    result = currPath;
                    break;
                }
                if( !visited.Contains(currNode) )
                {
                    visited.Add(currNode);
                    foreach(var node in problem.GetSuccessors(currNode))
                    {
                        // It's not good idea, but I have no more best solution on this time!
                        var newQueue = new Queue<KeyValuePair<string, string>>(currPath);
                        newQueue.Enqueue(node);
                        fringe.Add(newQueue);
                    }
                }
            }
            return result;
        }
    }

    public static class Helpers
{
        public static List<KeyValuePair<string, string>> LoadFromFile(string fileName, Char separator=' ')
        {
            // This text is added only once to the file.
            if (!File.Exists(fileName))
            {
                MessageBox.Show("No input file!");
            }
            string[] lines = File.ReadAllLines(fileName, Encoding.UTF8);
            List<KeyValuePair<string, string>> translators = new List<KeyValuePair<string, string>>();
            foreach (var line in lines)
            {
                var __pair = line.Split(separator);
                translators.Add(new KeyValuePair<string, string>(__pair[0], __pair[1]));
            }
            return translators;
        }

        /// <summary>
        /// Represent result Queue as string
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static string ResultToString(this Queue<KeyValuePair<string, string>> result)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var pair in result)
            {
                sb.Append(String.Format("{0} - {1}", pair.Key, pair.Value));
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }
}
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string path = @"translators.txt";
            var pairs = Helpers.LoadFromFile(path);
            var p = new Problem(pairs, "исландский", "русский");
            var result = Algorythms.UniformCostSearch(p);
            if (result != null && result.Count != 0)
            {
                textBlock.Text = result.ResultToString();
            }
            else
            {
                MessageBox.Show("No Solution!", "Attention!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        
    }
}
