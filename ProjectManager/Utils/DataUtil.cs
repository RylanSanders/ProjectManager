using MaterialDesignThemes.Wpf.Converters;
using Newtonsoft.Json;
using ProjectManager.DataObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Printing;
using System.Security.RightsManagement;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProjectManager.Utils
{
    public class DataUtil
    {
        public static DataUtil _instance;
        private const string DataFilePath = "Data.json";
        private const string ArchiveDataFilePath = "Archived.json";
        public static DataUtil GetInstance()
        {
            if (_instance == null)
            {
                _instance = new DataUtil();
            }
            return _instance;
        }

        private DataDO DataDO { get; set; }
        public List<TaskItemDO> TaskItems { get { return DataDO.Tasks; } }
        public List<DateDO> Dates { get { return DataDO.Dates; } }
        public List<ProjectDefinitionDO> ProjectDefinitions { get{return DataDO.ProjectDefinitions;} }
        public List<KanbanCardDO> KanbanCards {get{return DataDO.KanbanCards;}} 
        public List<TagDefinitionDO> TagDefinitions {get{return DataDO.TagDefinitions;} }
        public List<NoteDO> Notes { get { return DataDO.Notes;} }
        public List<TodoDO> Todos { get { return DataDO.ToDos; } }

        public DataDO ArchivedData { get; set; }
        public DataUtil() 
        {
            DataDO = new DataDO();
            ArchivedData = new DataDO();
        }

        public static void Save()
        {
            DataUtil instance = GetInstance();
            string Json = System.Text.Json.JsonSerializer.Serialize(instance.DataDO);
            File.WriteAllText(DataFilePath,Json);
            string archivedJson = System.Text.Json.JsonSerializer.Serialize(instance.ArchivedData);
            File.WriteAllText(ArchiveDataFilePath, archivedJson);
        }

        public static void Load()
        {
            DataUtil instance = GetInstance();
            if (File.Exists(DataFilePath))
            {
                instance.DataDO = JsonConvert.DeserializeObject<DataDO>(File.ReadAllText(DataFilePath));
                if (instance.DataDO == null) instance.DataDO = new DataDO();
            }
            else
            {
                File.Create(DataFilePath).Close();
            }
            if (File.Exists(ArchiveDataFilePath))
            {
                instance.ArchivedData = JsonConvert.DeserializeObject<DataDO>(File.ReadAllText(ArchiveDataFilePath));
                if(instance.ArchivedData==null) instance.ArchivedData=new DataDO();
            }
            else
            {
                File.Create(ArchiveDataFilePath).Close();
            }
        }
    }
}
