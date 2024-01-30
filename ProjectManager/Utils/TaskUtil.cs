using MaterialDesignThemes.Wpf.Converters;
using Newtonsoft.Json;
using ProjectManager.DataObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProjectManager.Utils
{
    public class TaskUtil
    {
        public static TaskUtil _instance;
        private const string TaskFilePath = "Tasks.json";

        public static TaskUtil GetInstance()
        {
            if (_instance == null)
            {
                _instance = new TaskUtil();
            }
            return _instance;
        }

        public List<TaskItemDO> TaskItems { get; set; }

        public TaskUtil() 
        { 
            TaskItems = new List<TaskItemDO>();
        }

        public static void Save()
        {
            TaskUtil instance = GetInstance();
            string Json = System.Text.Json.JsonSerializer.Serialize(instance.TaskItems);
            File.WriteAllText(TaskFilePath,Json);  
        }

        public static void Load()
        {
            TaskUtil instance = GetInstance();
            if (File.Exists(TaskFilePath)) 
            {
                instance.TaskItems = JsonConvert.DeserializeObject<List<TaskItemDO>>(File.ReadAllText(TaskFilePath));
            }
            
        }
    }
}
