using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Text.Json;
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace HW06
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }
        private async void FindButton_Click(object sender, RoutedEventArgs e)
        {

            // JavaScript 正则表达式，用于查找电话号码和邮箱
            string script = """
                            
                                (function() {
                                    const phoneRegex = /(1[3-9]\d{9})|(0\d{2,3}-\d{7,8})/g;
                                    const emailRegex = /[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}/g;
                                    
                
                                    const text = document.body.innerText;
                                    const phones = text.match(phoneRegex) || [];
                                    const emails = text.match(emailRegex) || [];
                
                                    return JSON.stringify({ phones, emails });
                                })();
                                        
                            """;

            // 执行 JavaScript 并获取结果
            string result = await WebView.ExecuteScriptAsync(script);

            // 解析和格式化结果
            if (!string.IsNullOrEmpty(result))
            {
                try
                {
                    // 清理 JSON 格式
                    result = result.Trim('"').Replace("\\\"", "\"");

                    // 使用 Newtonsoft.Json 解析 JSON
                    var parsedJson = JToken.Parse(result);

                    // 去除转义字符并格式化输出
                    var phones = parsedJson["phones"]?.Select(p => p.ToString().Replace("\\n", "").Trim()).ToList();
                    var emails = parsedJson["emails"]?.Select(e => e.ToString().Trim()).ToList();

                    // 构建清晰的输出
                    var formattedResult = new StringBuilder();
                    formattedResult.AppendLine("查找到的结果：");
                    formattedResult.AppendLine("\n电话号码：");
                    if (phones != null && phones.Any())
                    {
                        foreach (var phone in phones)
                        {
                            formattedResult.AppendLine($"- {phone}");
                        }
                    }
                    else
                    {
                        formattedResult.AppendLine("未找到电话号码。");
                    }

                    formattedResult.AppendLine("\n邮箱：");
                    if (emails != null && emails.Any())
                    {
                        foreach (var email in emails)
                        {
                            formattedResult.AppendLine($"- {email}");
                        }
                    }
                    else
                    {
                        formattedResult.AppendLine("未找到邮箱地址。");
                    }

                    // 显示结果
                    ResultTextBox.Text = formattedResult.ToString();
                }
                catch (Exception ex)
                {
                    ResultTextBox.Text = $"解析结果时出错：{ex.Message}";
                }
            }
            else
            {
                ResultTextBox.Text = "未找到任何电话号码或邮箱。";
            }
        }

        private void SetWebSource()
        {
            string url = UrlTextBox.Text;

            if (!string.IsNullOrWhiteSpace(url))
            {
                // 如果用户输入的 URL 没有以 "http://" 或 "https://" 开头，自动补全
                if (!url.StartsWith("http://") && !url.StartsWith("https://"))
                {
                    url = "https://" + url;
                }
            }

            try
            {
                WebView.Source = new Uri(url);
            }
            catch (UriFormatException)
            {
                ResultTextBox.Text = "URL 无效";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SetWebSource();
        }
    }
}
