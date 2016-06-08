using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using TMDbLib.Client;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using System.Text.RegularExpressions;

namespace firstapp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var files = Directory
                .GetFiles("D:\\Movies2", "*.*", SearchOption.AllDirectories)
                .Where(s => s.ToLower().EndsWith(".mp4") || s.ToLower().EndsWith(".mkv") || s.ToLower().EndsWith(".avi"))
                .Where(s => !s.ToLower().Contains("sample") && !s.ToLower().Contains("etrg"));

            var files_array = files.ToArray();
            Array.Sort(files_array);

            var filecount = files.ToArray().Length;
            textBox1.Text = filecount + " Movies found";

            ColumnHeader header = new ColumnHeader();
            header.Text = "";
            header.Name = "col1";
            listView1.Columns.Add(header);
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

            var imagelist = new ImageList();
            foreach (var movie_name in files_array)
            {
                string filenameWithoutPath = Path.GetFileNameWithoutExtension(movie_name);
                var clean_name = CleanName(filenameWithoutPath);
                var movie_poster = GetPoster(clean_name);

                imagelist.ImageSize = new Size(91, 134);
                if (movie_poster == "No results found !" || movie_poster == "http://image.tmdb.org/t/p/original" || movie_poster == "http://image.tmdb.org/t/p/w342")
                {
                    imagelist.Images.Add(clean_name, Image.FromFile("C:\\Users/ritu/Pictures/no-preview.png"));
                }
                else
                {
                    imagelist.Images.Add(clean_name, LoadImage(movie_poster));
                }
            }

            int count = 0;
            foreach (var movie_name in files_array)
            {
                var item = new ListViewItem();
                string filenameWithoutPath = Path.GetFileNameWithoutExtension(movie_name);
                var clean_name = CleanName(filenameWithoutPath);

                listView1.LargeImageList = imagelist;
                listView1.Items.Add(new ListViewItem { ImageIndex = count++, Text = clean_name });
            }
        }

        private Image LoadImage(string url)
        {
            System.Net.WebRequest request = System.Net.WebRequest.Create(url);

            System.Net.WebResponse response = request.GetResponse();
            System.IO.Stream responseStream = response.GetResponseStream();

            Bitmap bmp = new Bitmap(responseStream);

            responseStream.Dispose();

            return bmp;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click_1(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            TMDbClient client = new TMDbClient("dbbcc1175aa83f5e3a59bee59dabac58");
            client.GetConfig();
            SearchContainer<SearchMovie> results = client.SearchMovie("Birdman");

            var movie = results.Results;
                
            if (movie.Count > 0)
            {
                var poster = movie.First().PosterPath;
                var url = client.GetImageUrl("original", poster);
                String str = url.ToString();
                textBox1.Text = str;
            }
            else
            {
                var url = "No results found !";
                String str = url.ToString();
                textBox1.Text = str;
            }
        }

        public static string GetPoster(string moviename)
        {
            TMDbClient client = new TMDbClient("dbbcc1175aa83f5e3a59bee59dabac58");
            client.GetConfig();
            SearchContainer<SearchMovie> results = client.SearchMovie(moviename);

            var movie = results.Results;

            if (movie.Count > 0)
            {
                var poster = movie.First().PosterPath;
                var url = client.GetImageUrl("w342", poster);
                String str = url.ToString();
                return str;
            }
            else
            {
                var url = "No results found !";
                String str = url.ToString();
                return str;
            }
        }

        public static string CleanName(string dirtyname)
        {

            string input = dirtyname;

            string pattern = "\\W";
            string pattern2 = "20\\d{2}|19\\d{2}";

            string replacement = " ";
            Regex rgx = new Regex(pattern);
            string result = rgx.Replace(input, replacement);
            string[] substrings = Regex.Split(result, pattern2);

            Console.WriteLine("Original String: {0}", input);
            Console.WriteLine("Replacement String: {0}", result);
            Console.WriteLine("before year: {0}", substrings[0]);

            return substrings[0];
        }
    }
}
