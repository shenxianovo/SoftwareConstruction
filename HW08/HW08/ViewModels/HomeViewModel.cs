using HW08.Library.DataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HW08.Library.Models;

namespace HW08.ViewModels
{
    public partial class HomeViewModel : ObservableRecipient
    {

        public List<Word> Words { get; set; } = [];
        public void InitDb()
        {
            string databasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "words.db");
            var dbContext = new DbContext(databasePath);

            // 初始化数据库
            dbContext.InitializeDatabase();

            // 插入数据
            //dbContext.AddWord("hello", "你好");
            //dbContext.AddWord("abandon", "遗弃");
            //dbContext.AddWord("model", "模型");
            //dbContext.AddWord("view", "视图");
            //dbContext.AddWord("database", "数据库");
            //dbContext.AddWord("static", "静态的");
            //dbContext.AddWord("integer", "整数");
            //dbContext.AddWord("abstract", "抽象的");
            //dbContext.AddWord("world", "世界");

            // 查询数据
            Words = dbContext.GetAllWords();

            Chinese = Words[_index].Chinese;
        }

        [ObservableProperty]
        private string english;

        [ObservableProperty]
        private string chinese;

        private int _index = 0;

        [RelayCommand]
        private async void NextWord()
        {
            English = Words[_index].English;
            await Task.Delay(2000);
            English = "";
            _index = (_index + 1) % Words.Count;
            Chinese = Words[_index].Chinese;
        }

        [RelayCommand]
        private async void Update()
        {
            var correct = Words[_index].English;
            var actual = English;

            _index = (_index + 1) % Words.Count;

            if (actual == correct)
            {
                Chinese += "\n正确";
                await Task.Delay(2000);
            }
            else
            {
                Chinese += "\n错误";
                English = "正确答案：" + correct;
                await Task.Delay(2000);
                English = "";
            }

            Chinese = Words[_index].Chinese;
        }
    }
}
