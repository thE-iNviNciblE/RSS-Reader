using System;
using System.Reflection.Metadata.Ecma335;
using System.Xml.Linq;
using Plugin.LocalNotification;
namespace RSS_Reader;
public class CustomCell : ViewCell
{
    public CustomCell()
    {
        var label = new Label { FontSize = 24 };
        label.SetBinding(Label.TextProperty, "title");

        var label2 = new Label { FontSize = 24 };
        label2.SetBinding(Label.TextProperty, "pubDate");
         
    }
} 

public partial class MainPage : ContentPage
{
	public MainPage()	{
		InitializeComponent();
	}


    private async void OnCounterClicked(object sender, EventArgs e)
    {

        await DisplayAlert("Alert", "RSS Feed wird eingelesen", "OK");

        var request = new NotificationRequest
        {
            NotificationId = 1000,
            Title = "Subscribe for me",
            Subtitle = "Hello Friends",
            Description = "Stay Tuned",
            BadgeNumber = 42,
            //Schedule = new NotificationRequestSchedule
            //{
            //    NotifyTime = DateTime.Now.AddSeconds(5),
            //    NotifyRepeatInterval = TimeSpan.FromDays(1)
            //}
        };
        await LocalNotificationCenter.Current.Show(request);

        var client = new HttpClient();
        var url = "https://tsecurity.de/RSS/9/CVE/";
        var xml = await client.GetStringAsync(url);
      
        var doc = XDocument.Parse(xml,LoadOptions.PreserveWhitespace ); 
        var channel = doc.Root; 

        var ns = channel.GetDefaultNamespace();
        var nsContent = channel.GetNamespaceOfPrefix("content");
        var nsRDF = channel.GetNamespaceOfPrefix("rdf");
        var items = doc.Descendants(ns + "item").Select(x => new RSS_Item()
        {
            title = (string)x.Element(ns + "title"),
            link = (string)x.Element(ns + "link"),
            /*comments = (string)x.Element(ns + "comments"),*/
            pubDate = (DateTime)x.Element(ns + "pubDate"),
            /*category = (string)x.Element(ns + "category"),*/
            guid = (string)x.Element(ns + "guid"),
            description = (string)x.Element(ns + "description"),
            time_left = $"{(int)(DateTime.Now - (DateTime)x.Element(ns + "pubDate")).TotalMinutes} Minuten "
        }).ToList();
  
        foreach (RSS_Item item in items)
        {
            item.title = (string)item.link;
            Console.WriteLine(item.title);
            Console.WriteLine(item.link);
            Console.WriteLine(item.pubDate);
            Console.WriteLine(item.guid);
            Console.WriteLine(item.description);
            Console.WriteLine(item.time_left);
        }
 
        lvwRSS_Liste.ItemsSource = items; 
    }
    private async void OnItemClick(object sender, ItemTappedEventArgs e)
    {
        // Handle item click
        var item = (RSS_Item) e.Item;
        await Launcher.OpenAsync(item.link);
    }    

    public class RSS_Item
    {
        public string title { get; set; }
        public string link { get; set; }
        /*public string comments { get; set; }*/
        public DateTime pubDate { get; set; }
        /*public string category { get; set; }*/
        public string guid { get; set; }
        public string description { get; set; }
        public string time_left{ get; set; }
    }

}

