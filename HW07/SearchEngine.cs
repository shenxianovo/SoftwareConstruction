using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static System.Net.WebRequestMethods;

namespace HW07;

public class SearchEngine : ISearchEngine
{
    private readonly HttpClient _httpClient = new();

    public async Task<string> SearchAsync(string engine, string keyword)
    {
        // 避免重复添加 User-Agent（每次都加会报错）
        if (!_httpClient.DefaultRequestHeaders.Contains("User-Agent"))
        {
            _httpClient.DefaultRequestHeaders.Add("User-Agent",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 " +
                "(KHTML, like Gecko) Chrome/122.0.0.0 Safari/537.36");
        }

        string urlHead = engine switch
        {
            "Bing" => "https://www.bing.com/search?q=",
            "DuckDuckGo" => "https://html.duckduckgo.com/html/?q=",
            _ => throw new ArgumentException("不支持的搜索引擎：" + engine)
        };

        try
        {
            string url = urlHead + HttpUtility.UrlEncode(keyword);
            string html = await _httpClient.GetStringAsync(url);
            return ExtractTextFromHtml(html, engine);
        }
        catch (Exception ex)
        {
            return $"{engine} 搜索失败：" + ex.Message;
        }
    }

    private string ExtractTextFromHtml_Bing(HtmlDocument doc)
    {
        var nodes = doc.DocumentNode.SelectNodes("//li[@class='b_algo']");
        if (nodes == null) return "没有找到 Bing 搜索结果";

        var result = "";
        int count = 0;
        foreach (var node in nodes)
        {
            var titleNode = node.SelectSingleNode(".//h2/a");
            var descNode = node.SelectSingleNode(".//div[@class='b_caption']/p");

            string title = HttpUtility.HtmlDecode(titleNode?.InnerText.Trim() ?? "(无标题)");
            string desc = HttpUtility.HtmlDecode(descNode?.InnerText.Trim() ?? "(无副标题)");

            count++;
            result += $"{count}. {title}\r\n   {desc}\r\n\r\n";

            if (count >= 5) break;
        }

        return result.Trim();
    }

    private string ExtractTextFromHtml_DuckDuckGo(HtmlDocument doc)
    {
        var nodes = doc.DocumentNode.SelectNodes("//div[contains(@class, 'result__body')]");
        if (nodes == null) return "没有找到 DuckDuckGo 搜索结果";

        var result = "";
        int count = 0;

        foreach (var node in nodes)
        {
            var titleNode = node.SelectSingleNode(".//a[contains(@class, 'result__a')]");
            var descNode = node.SelectSingleNode(".//a[contains(@class, 'result__snippet')]");

            string title = HttpUtility.HtmlDecode(titleNode?.InnerText.Trim() ?? "(无标题)");
            string desc = HttpUtility.HtmlDecode(descNode?.InnerText.Trim() ?? "(无摘要)");

            count++;
            result += $"{count}. {title}\r\n   {desc}\r\n\r\n";

            if (count >= 5) break;
        }

        return result.Trim();

    }


    private string ExtractTextFromHtml(string html, string source)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        string text = "";

        if (source == "Bing")
        {
            text = ExtractTextFromHtml_Bing(doc);
        }
        else if (source == "DuckDuckGo")
        {
            text = ExtractTextFromHtml_DuckDuckGo(doc);
        }
        else
        {
            // fallback: 通用抓取文本
            text = doc.DocumentNode.InnerText;
        }

        return text;
    }
}
