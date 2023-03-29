using System;
using System.Xml.Linq;

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
	public MainPage()
	{
		InitializeComponent();
	}

    private void OnCounterClicked(object sender, EventArgs e)
    {
        const string URL = "https://tsecurity.de/RSS/1/Windows%2011/";


        XDocument doc = XDocument.Load(URL);
        XElement channel = doc.Root;
        XNamespace ns = channel.GetDefaultNamespace();
        XNamespace nsContent = channel.GetNamespaceOfPrefix("content");
        XNamespace nsRDF = channel.GetNamespaceOfPrefix("rdf");

        List<RSS_Item> items = doc.Descendants(ns + "item").Select(x => new RSS_Item()
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

        /* lvwRSS_Liste.ItemsSource = new List<Item>
         * var timeSpan = DateTime.Now - dateTime
          {
              new Item { title = "Person 1", pubDate = "person1.png" },
              new Item { title = "Person 2", pubDate = "person2.png" },
          };

         lvwRSS_Liste.ItemTemplate = new DataTemplate(typeof(CustomCell));*/

        //lvwRSS_Liste.ItemTemplate = new DataTemplate(() =>
        //{
        //    var label = new Label { FontSize = 24 };
        //    label.SetBinding(Label.TextProperty, items[0].title);
        //    return new ViewCell { View = label };
        //});
        lvwRSS_Liste.ItemsSource = items;
        // ListView listView = new ListView();
        // listView.ItemsSource = new List<string> { Item };

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

