using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TaskManagerWPF;

namespace TaskManagerWPF
{
    public partial class MainWindow : Window
    {
        private TaskViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new TaskViewModel();
            DataContext = viewModel;
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            var newTask = new Task
            {
                Title = TaskTitleTextBox.Text,
                Description = TaskDescriptionTextBox.Text,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                Priority = Convert.ToInt32((TaskPriorityComboBox.SelectedItem as ComboBoxItem)?.Content),
                Category = CategoryList.SelectedItem as string
            };

            if (!string.IsNullOrWhiteSpace(newTask.Title) && !string.IsNullOrWhiteSpace(newTask.Category))
            {
                viewModel.AddTask(newTask);
                TaskTitleTextBox.Clear();
                TaskDescriptionTextBox.Clear();
                TaskPriorityComboBox.SelectedIndex = -1; 
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните название задачи и выберите категорию.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void RemoveTask_Click(object sender, RoutedEventArgs e)
        {
            var selectedTask = TaskList.SelectedItem as Task;
            if (selectedTask != null)
            {
                viewModel.RemoveTask(selectedTask);
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите задачу для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void TaskList_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Task)))
            {
                var task = e.Data.GetData(typeof(Task)) as Task;
                if (task != null)
                {
                    var category = CategoryList.SelectedItem as string;
                    task.Category = category;
                    viewModel.SaveData();
                }
            }
        }

        private void TaskList_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Task)))
            {
                e.Effects = DragDropEffects.Move;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = true;
        }

        private void TaskList_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var task = ((FrameworkElement)e.OriginalSource).DataContext as Task;
            if (task != null)
            {
                DragDrop.DoDragDrop(TaskList, task, DragDropEffects.Move);
            }
        }
    }
}