using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;
using System.Diagnostics;
using HtmlAgilityPack;
using System.Net.Http;
namespace page_crawl
{
    class Program
    {
        static void Main(string[] args)
        {
            Uri url = new Uri("http://www.youtube.com/watch?v=e0HQYnzrdt0");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader read = new StreamReader(response.GetResponseStream());
            //StreamReader read2 = new StreamReader(response.GetResponseStream());
            String line;
            List<String> page_data = new List<String>();
            while ((line = read.ReadLine()) != null)
            {
             page_data.Add(line);
           }
            read.Close();
            response.Close();
            Console.WriteLine(page_data.Count);
            for (int x = 0; x < page_data.Count;x++ )
            {
                if (page_data[x]=="")
                    page_data.RemoveAt(x);
                //Console.WriteLine(x + " " + page_data[x]);
            }
            Console.WriteLine(page_data.Count);
            var sw = Stopwatch.StartNew();
            foreach (String link in page_data)
            {
                DumpHRefs(link);
            }
            sw.Stop();
            Console.WriteLine("Time Elapsed: " + sw.ElapsedMilliseconds+"ms");
            var sw2 = Stopwatch.StartNew();
            agilePack();
            sw2.Stop();
            Console.WriteLine("Time Elapsed: " + sw2.ElapsedMilliseconds + "ms");
            System.Console.Read();   
            
        }
    
    private static void DumpHRefs(string inputString) 
{
   Match m;
   string HRefPattern = "href\\s*=\\s*(?:[\"'](?<1>[^\"']*)[\"']|(?<1>\\S+))";

   try {
      m = Regex.Match(inputString, HRefPattern, 
                      RegexOptions.IgnoreCase | RegexOptions.Compiled, 
                      TimeSpan.FromSeconds(1));
      while (m.Success)
      {
         
          if(m.Groups[1].ToString().Contains("watch?v"))
          Console.WriteLine("Found href " + m.Groups[1] + " at " 
            + m.Groups[1].Index);
         m = m.NextMatch();
      }   
   }
   catch (RegexMatchTimeoutException) {
      Console.WriteLine("The matching operation timed out.");
   }
}
    public static void agilePack() 
    {
        var webget = new HtmlWeb();
        var doc = webget.Load("http://www.youtube.com/watch?v=e0HQYnzrdt0");
        List<String> links = new List<String>();
        foreach (HtmlNode nd in doc.DocumentNode.SelectNodes("//a[@href]"))
 {
     
         links.Add(nd.Attributes["href"].Value);
     Console.WriteLine(links);
        }

    
    
    }
    }
}
