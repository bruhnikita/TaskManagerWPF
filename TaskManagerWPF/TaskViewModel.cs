using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text.Json;

namespace TaskManagerWPF
{
    public class TaskViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Task> Tasks { get; set; }
        public ObservableCollection<string> Categories { get; set; }

        public TaskViewModel()
        {
            Tasks = new ObservableCollection<Task>();
            Categories = new ObservableCollection<string> { "Работа", "Учёба", "Домашние дела" };
            LoadData();
        }

        public void AddTask(Task task)
        {
            Tasks.Add(task);
            SaveData();
            OnPropertyChanged(nameof(Tasks));
        }

        public void RemoveTask(Task task)
        {
            Tasks.Remove(task);
            SaveData();
            OnPropertyChanged(nameof(Tasks));
        }

        public void SaveData()
        {
            var json = JsonSerializer.Serialize(Tasks);
            File.WriteAllText("tasks.json", json);
        }

        public void LoadData()
        {
            if (File.Exists("tasks.json"))
            {
                var json = File.ReadAllText("tasks.json");
                var tasks = JsonSerializer.Deserialize<ObservableCollection<Task>>(json);
                if (tasks != null)
                {
                    Tasks = tasks;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

