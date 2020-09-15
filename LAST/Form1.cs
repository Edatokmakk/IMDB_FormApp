using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LAST
{
    public partial class Form1 : Form
    {

        List<string> listOfTitleLinks = new List<string>();
        public DBConnection dBConnect = new DBConnection();
        SqlConnection cnn;
        SqlDataReader reader;
        public Form1()
        {
            InitializeComponent();
        }
        public string clientMethod(string url)
        {
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8; //Türkçe karakter ve unicode karakter için
            return client.DownloadString(url);//sayfanın içeriğini parse etme
        }
        private void searchTxtBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void searchbtn_Click(object sender, EventArgs e)
        {
            string name = searchTxtBox.Text;
            if (dBConnect.searchControl(name))
            {
                //MessageBox.Show("Already exist in your movie library!");
                cnn = new SqlConnection(@"server=.\sqlexpress;database=IMDB;trusted_connection=true");
                cnn.Open();
                SqlCommand movieCommand = new SqlCommand("select * from Movie where movieName like '%' + @name + '%'", cnn);
                movieCommand.Parameters.AddWithValue("@name", name);
                reader = movieCommand.ExecuteReader();
                while (reader.Read())
                {
                    Movie movie = new Movie();
                    movie.ImdbTitleID = reader.GetString(0);
                    movie.Name = reader.GetString(1);
                    movie.DatePublished = reader.GetString(2);
                    movie.Rate = reader.GetString(3);
                    movie.Desc = reader.GetString(4);
                    //movie.ImageUrl = reader.GetString(5);
                    lstBoxTitles.Items.Add(movie);
                }
                reader.Close();

                cnn.Close();
            }
            else
            {
                name = searchTxtBox.Text;
                string url = ($"https://www.imdb.com/find?q={name}&ref_=nv_sr_sm");
                string htmlString = clientMethod(url);
                int uniqueIndex = htmlString.IndexOf("findList");
                int uniqueEndIndex = htmlString.IndexOf("findMoreMatches");
                int contentLength = uniqueEndIndex - uniqueIndex;
                string lastReply = htmlString.Substring(uniqueIndex, contentLength);//isimler ve tarihleri çekeceğim içeriği verir.
                while (lastReply.IndexOf("\"result_text\"") > 0)
                {
                    Movie movie = new Movie();
                    int indexOfContent = lastReply.IndexOf("\"result_text\"");
                    int lastIndexOfContent = lastReply.IndexOf("</tr>");
                    string htmlContent = lastReply.Substring(indexOfContent, lastIndexOfContent - indexOfContent);

                    lastReply = lastReply.Remove(0, lastIndexOfContent + "</tr>".Length);

                    int indexOfTemp = htmlContent.IndexOf("/");
                    int indexOfLastTemp = htmlContent.LastIndexOf("\" >");

                    movie.ImdbTitleID = htmlContent.Substring(indexOfTemp, indexOfLastTemp - indexOfTemp);
                    listOfTitleLinks.Add(movie.ImdbTitleID);
                    htmlContent = htmlContent.Remove(0, indexOfLastTemp + "\"> ".Length);

                    int titleStart = htmlContent.IndexOf("<");
                    movie.Name = htmlContent.Substring(0, titleStart);
                    lstBoxTitles.Items.Add(movie);
                }
                foreach (Movie item in lstBoxTitles.Items)
                {
                    string movieUrl = ("https://www.imdb.com" + item.ImdbTitleID);
                    string htmlDetailString = clientMethod(movieUrl);
                    string titleForİmg = (item.ImdbTitleID.Substring(0, ("/title/tt0133093/").Length));
                    string Imgurl = ("https://www.imdb.com" + titleForİmg + "mediaindex?ref_=tt_ov_mi_sm");
                    string htmlForImgs = clientMethod(Imgurl);
                    int imgStart=htmlForImgs.IndexOf(" \"image\": [") + (" \"image\": [\n {").Length;
                    int imgEnd = htmlForImgs.IndexOf("(function (win)");
                    string[] imgSeps = { "\"@type\": \"ImageObject\"" };
                    string[] imgSepHtmls=htmlForImgs.Substring(imgStart, imgEnd - imgStart).Split(imgSeps, StringSplitOptions.None);
                    
                    //MOVIE IMAGES
                    foreach(string sepImg in imgSepHtmls)
                    {
                        int imgStartIndex =sepImg.IndexOf("\"url\": ") + ("\"url\": ").Length;
                        int imgEndIndex = sepImg.IndexOf(",\n\"mainEntityOfPage\":");
                        if (imgStartIndex != -1 && imgEndIndex != -1)
                        {
                            string finalImg = sepImg.Substring(imgStartIndex, imgEndIndex);
                        }
                    }
                    int rateStart = htmlDetailString.IndexOf("\"ratingValue\": ");
                    if (rateStart != -1)
                    {
                        int rateEnd = htmlDetailString.IndexOf("\"review\":");
                        if (rateEnd != -1)
                        {
                            item.Rate = htmlDetailString.Substring(rateStart + ("\"ratingValue\": \"").Length, (rateEnd - rateStart) - ("\"\n  },\n  \"review\": {\n    ").Length);
                        }
                    }
                    else
                    {
                        item.Rate = "";
                    }
                    int yearStart = htmlDetailString.IndexOf("\"datePublished\":");
                    item.DatePublished = htmlDetailString.Substring(yearStart + ("\"datePublished\":\"").Length, ("\"1999-03-31\"").Length);
                    int contentsLocation = htmlDetailString.IndexOf("\"@type\":");
                    int endOfContentsLocation = htmlDetailString.IndexOf("\"description\":");
                    if (endOfContentsLocation == -1)
                    { endOfContentsLocation = htmlDetailString.IndexOf("\"datePublished\":"); }

                    string specialContent = htmlDetailString.Substring(contentsLocation, endOfContentsLocation - contentsLocation + 1);


                    if (specialContent.IndexOf("\"image\": \"") != -1)
                    {
                        int picLocation = specialContent.IndexOf("\"image\": \"") + ("\"image\": \"").Length;
                        int picEndLocation = specialContent.IndexOf("\",\n  \"genre\"");
                        item.ImageUrl = specialContent.Substring(picLocation, picEndLocation - picLocation);  // RESIMLER
                    }
                    else
                    {
                        item.ImageUrl = null;
                    }

                    if (htmlDetailString.IndexOf("\"datePublished\":") < htmlDetailString.IndexOf("\"uploadDate\":"))
                    {
                        item.Desc = htmlDetailString.Substring(htmlDetailString.IndexOf("\"description\": ") + ("\"description\": ").Length, htmlDetailString.IndexOf("\"uploadDate\":") - (htmlDetailString.IndexOf("\"description\": ") + (",\n  \"uploadDate\"").Length)).Trim();
                    }
                    else
                    {
                        item.Desc = htmlDetailString.Substring(htmlDetailString.IndexOf("\"description\": ") + ("\"description\": ").Length, (htmlDetailString.IndexOf("\"datePublished\":") - ((htmlDetailString.IndexOf("\"description\": ") + (",\n  \"datePublished\"").Length)))).Trim();
                    }
                    string actorText;
                    string directorText;
                    string creatorText;

                    int count;
                    string[] seps = { "\"@type\": \"Person\"" };

                    int actorStart = specialContent.IndexOf("\"actor\": ");
                    int directorStart = specialContent.IndexOf("\"director\": ");
                    int creatorStart = specialContent.IndexOf("\"creator\":");


                    if (specialContent.Contains("\"director\": "))
                    {
                        if (actorStart != -1)
                        {
                            actorText = specialContent.Substring(actorStart, directorStart - actorStart).Remove(0, 20 + ("\"@type\": \"Person\",").Length);
                            string[] actors = actorText.Split(seps, StringSplitOptions.None);

                            foreach (string actor in actors)
                            {
                                count = 0;
                                Cast person2 = new Cast();
                                int acUrlIndex = actor.IndexOf("\"url\":") + ("\"url\":").Length;
                                int acUrlEndIndex = actor.IndexOf("\"name\":");
                                person2.ImdbID = actor.Substring(acUrlIndex, acUrlEndIndex - (acUrlIndex + 1)).Trim();
                                person2.Name = actor.Substring(actor.IndexOf("\"name\": ") + ("\"name\": ").Length, actor.IndexOf("}") - (actor.IndexOf("\"name\": ") + ("\"name\": ").Length));
                                //person2.Roles.Add(Role.Actor);
                                foreach (Cast cast in item.Casts)
                                {
                                    if (person2.Name == cast.Name)
                                    {
                                        //cast.Roles.Add(Role.Actor);
                                        count++;
                                    }

                                }
                                if (count == 0)
                                {
                                    person2.Roles.Add(Role.Actor);
                                }
                                if (count < 1)
                                {
                                    item.Casts.Add(person2);
                                }

                            }
                        }
                        if (creatorStart != -1)
                        {
                            directorText = specialContent.Substring(directorStart, creatorStart - directorStart).Remove(0, 25 + ("\"@type\": \"Person\",").Length);
                            string[] directors = directorText.Split(seps, StringSplitOptions.None);

                            foreach (string director in directors)
                            {
                                count = 0;
                                Cast person1 = new Cast();
                                Person p = new Person();
                                int dirUrlIndex = director.IndexOf("\"url\":") + ("\"url\":").Length;
                                int dirUrlEndIndex = director.IndexOf("\"name\":");
                                person1.ImdbID = director.Substring(dirUrlIndex, dirUrlEndIndex - (dirUrlIndex + 1)).Trim();
                                person1.Name = director.Substring(director.IndexOf("\"name\": ") + ("\"name\": ").Length, director.IndexOf("}") - (director.IndexOf("\"name\": ") + ("\"name\": ").Length));
                                person1.Roles.Add(Role.Director);

                                foreach (Cast cast in item.Casts)
                                {
                                    if (person1.Name == cast.Name)
                                    {
                                        count++;
                                    }
                                }

                                if (count < 1)
                                {
                                    item.Casts.Add(person1);
                                }

                            }
                        }
                        else
                        {
                            specialContent = specialContent.Remove(0, directorStart + ("\"director\": {\n  ").Length).Trim();
                            string[] directors = specialContent.Split(seps, StringSplitOptions.RemoveEmptyEntries);

                            foreach (string director in directors)
                            {
                                count = 0;
                                Cast person1 = new Cast();
                                Person p = new Person();
                                int dirUrlIndex = director.IndexOf("\"url\":") + ("\"url\":").Length;
                                int dirUrlEndIndex = director.IndexOf("\"name\":");
                                person1.ImdbID = director.Substring(dirUrlIndex, dirUrlEndIndex - (dirUrlIndex + 1)).Trim();
                                person1.Name = director.Substring(director.IndexOf("\"name\": ") + ("\"name\": ").Length, director.IndexOf("}") - (director.IndexOf("\"name\": ") + ("\"name\": ").Length));
                                person1.Roles.Add(Role.Director);

                                foreach (Cast cast in item.Casts)
                                {
                                    if (person1.Name == cast.Name)
                                    {
                                        count++;
                                    }
                                }

                                if (count < 1)
                                {
                                    item.Casts.Add(person1);
                                }

                            }
                        }
                    }
                    else
                    {
                        if (creatorStart != -1)
                        {
                            actorText = specialContent.Substring(actorStart, creatorStart - actorStart).Remove(0, 20 + ("\"@type\": \"Person\",").Length);
                        }
                        
                    }

                    if (specialContent.Contains("\"creator\": "))
                    {
                        creatorText = specialContent.Substring(creatorStart).Remove(0, "\"creator\": [\n    {\n      ".Length).Trim();
                        string[] creators = creatorText.Split(seps, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string creator in creators)
                        {
                            count = 0;
                            Cast person3 = new Cast();
                            int crUrlIndex = creator.IndexOf("\"url\":") + ("\"url\":").Length;
                            int crUrlEndIndex = creator.IndexOf("\"name\":");
                            person3.ImdbID = creator.Substring(crUrlIndex, crUrlEndIndex - crUrlIndex).Trim();
                            person3.Name = creator.Substring(creator.IndexOf("\"name\": ") + ("\"name\": ").Length, creator.IndexOf("}") - (creator.IndexOf("\"name\": ") + ("\"name\": ").Length));

                            person3.Roles.Add(Role.Writer);

                            foreach (Cast cast in item.Casts)
                            {
                                if (person3.Name == cast.Name)
                                {
                                    count++;
                                }
                            }
                            if (count < 1)
                            {
                                item.Casts.Add(person3);
                            }

                        }
                    }

                }
            }

        }

        private void lstBoxTitles_SelectedIndexChanged(object sender, EventArgs e)
        {
            directorListBox.Items.Clear();
            actorListBox.Items.Clear();
            writerListBox.Items.Clear();
            descTxtBox.Clear();
            picBox.Refresh();
            castListBox.Items.Clear();
            rateTxtBox.Clear();
            genreLstBox.Items.Clear();

            Movie movie_2 = (Movie)lstBoxTitles.SelectedItem;

            //SECİLİ FİLM DBDE VARSA
            if (dBConnect.movieControl(movie_2))
            {
                cnn = new SqlConnection(@"server=.\sqlexpress;database=IMDB;trusted_connection=true");
                cnn.Open();
                SqlCommand getmovieCommand = new SqlCommand($"select * from Movie where MovieID @movId", cnn);
                getmovieCommand.Parameters.AddWithValue("@movId", movie_2.ImdbTitleID);
                reader = getmovieCommand.ExecuteReader();
                while (reader.Read())
                {
                    movie_2.ImdbTitleID = reader.GetString(0);
                    movie_2.Name = reader.GetString(1);
                    movie_2.DatePublished = reader.GetString(2);
                    movie_2.Rate = reader.GetString(3);
                    movie_2.Desc = reader.GetString(4);
                    //movie_2.ImageUrl = reader.GetString(5);

                }
                reader.Close();

                cnn.Close();

                movieDetail(movie_2);
            }
            else
            {
                movieDetail(movie_2);
            }
        }

        public void movieDetail(Movie movie)
        {
            if (movie.ImageUrl == null)
            {
                picBox.Image = null;
            }
            else
            {
                picBox.Load(movie.ImageUrl);
            }
            descTxtBox.Text = movie.Desc;
            yearTxtBox.Text = movie.DatePublished;
            if (movie.Rate != null)
            {
                rateTxtBox.Text = movie.Rate;
            }


            for (int i = 0; i < movie.Casts.Count; i++)
            {
                castListBox.Items.Add(movie.Casts[i]);

                if (movie.Casts[i].Roles.Contains(Role.Director))
                {
                    string personHtml;
                    string personUrl;
                    string urlName;
                    personNameTxtBox.Text = movie.Casts[i].Name;
                    urlName = movie.Casts[i].ImdbID.Replace(',', ' ').Trim().Replace('\"', ' ').Trim();
                    personUrl = "https://www.imdb.com" + urlName + "?ref_=tt_ov_dr";
                    personHtml = clientMethod(personUrl);
                    int jobTitleStart = personHtml.IndexOf("\"jobTitle\": [") + ("\"jobTitle\": [").Length;
                    int jobTitleEnd = personHtml.IndexOf("],\n  \"description\":");
                    movie.Casts[i].JobTitle = personHtml.Substring(jobTitleStart, jobTitleEnd - jobTitleStart).Trim().Replace('\"', ' ');
                    jobTitleTxtBox.Text = movie.Casts[i].JobTitle;

                    int personPicStart = personHtml.IndexOf("\"image\":") + ("\"image\":\"\"").Length;
                    if (personPicStart != 9)
                    {
                        movie.Casts[i].ImageUrl = personHtml.Substring(personPicStart, jobTitleStart - (personPicStart + ("\",\n  \"jobTitle\": [").Length));
                        personPicBox.Load(movie.Casts[i].ImageUrl);
                    }


                    int bioStart = personHtml.IndexOf("\"description\": \"") + ("\"description\": \"").Length;
                    int bioEnd = personHtml.IndexOf("\",\n  \"birthDate\":");
                    if (bioEnd == -1) { bioEnd = personHtml.IndexOf("(function (win)"); }
                    movie.Casts[i].Bio = personHtml.Substring(bioStart, bioEnd - bioStart);
                    bioTxtBox.Text = movie.Casts[i].Bio;

                    int bDateStart = personHtml.IndexOf("\"birthDate\": \"") + ("\"birthDate\": \"").Length;
                    movie.Casts[i].BirthDate = personHtml.Substring(bDateStart, 10);
                    birthTxtBox.Text = movie.Casts[i].BirthDate;

                    directorListBox.Items.Add(movie.Casts[i]);
                }
                if (movie.Casts[i].Roles.Contains(Role.Actor))
                {
                    string personHtml;
                    string personUrl;
                    string urlName;

                    personNameTxtBox.Text = movie.Casts[i].Name;
                    urlName = movie.Casts[i].ImdbID.Replace(',', ' ').Trim().Replace('\"', ' ').Trim();
                    personUrl = "https://www.imdb.com" + urlName + "?ref_=tt_ov_st_sm";
                    personHtml = clientMethod(personUrl);
                    int jobTitleStart = personHtml.IndexOf("\"jobTitle\": [") + ("\"jobTitle\": [").Length;
                    int jobTitleEnd = personHtml.IndexOf("\"description\":");
                    movie.Casts[i].JobTitle = personHtml.Substring(jobTitleStart, jobTitleEnd - (jobTitleStart + ("],").Length)).Trim().Replace('\"', ' ');
                    jobTitleTxtBox.Text = movie.Casts[i].JobTitle;

                    int personPicStart = personHtml.IndexOf("\"image\":") + ("\"image\":\"\"").Length;
                    if (personPicStart != 9)
                    {
                        if (jobTitleStart != 12)
                        {
                            movie.Casts[i].ImageUrl = personHtml.Substring(personPicStart, jobTitleStart - (personPicStart + ("\",\n  \"jobTitle\": [").Length));
                            personPicBox.Load(movie.Casts[i].ImageUrl);
                        }
                    }
                    //movie.Casts[i].ImageUrl = personHtml.Substring(personPicStart, jobTitleStart - (personPicStart + ("\",\n  \"jobTitle\": [").Length));
                    //personPicBox.Load(movie.Casts[i].ImageUrl);

                    int bioStart = personHtml.IndexOf("\"description\": \"") + ("\"description\": \"").Length;
                    int bioEnd = personHtml.IndexOf("\",\n  \"birthDate\":");
                    if (bioEnd == -1) { bioEnd = personHtml.IndexOf("(function (win)"); }
                    movie.Casts[i].Bio = personHtml.Substring(bioStart, bioEnd - bioStart);
                    bioTxtBox.Text = movie.Casts[i].Bio;

                    int bDateStart = personHtml.IndexOf("\"birthDate\": \"") + ("\"birthDate\": \"").Length;
                    movie.Casts[i].BirthDate = personHtml.Substring(bDateStart, 10);
                    birthTxtBox.Text = movie.Casts[i].BirthDate;

                    actorListBox.Items.Add(movie.Casts[i]);
                }
                if (movie.Casts[i].Roles.Contains(Role.Writer))
                {
                    string personHtml;
                    string personUrl;
                    string urlName;

                    personNameTxtBox.Text = movie.Casts[i].Name;
                    urlName = movie.Casts[i].ImdbID.Replace(',', ' ').Trim().Replace('\"', ' ').Trim();
                    personUrl = "https://www.imdb.com" + urlName + "?ref_=tt_ov_wr";
                    personHtml = clientMethod(personUrl);
                    int jobTitleStart = personHtml.IndexOf("\"jobTitle\": [") + ("\"jobTitle\": [").Length;
                    int jobTitleEnd = personHtml.IndexOf("],\n  \"description\":");
                    if (jobTitleEnd == -1)
                    {
                        jobTitleEnd = personHtml.IndexOf("(function (win)");
                    }
                    movie.Casts[i].JobTitle = personHtml.Substring(jobTitleStart, jobTitleEnd - jobTitleStart).Trim().Replace('\"', ' ');
                    jobTitleTxtBox.Text = movie.Casts[i].JobTitle;

                    int personPicStart = personHtml.IndexOf("\"image\":") + ("\"image\":\"\"").Length;

                    if (personPicStart != 9)
                    {
                        movie.Casts[i].ImageUrl = personHtml.Substring(personPicStart, jobTitleStart - (personPicStart + ("\",\n  \"jobTitle\": [").Length));
                        personPicBox.Load(movie.Casts[i].ImageUrl);
                    }

                    int bioStart = personHtml.IndexOf("\"description\": \"") + ("\"description\": \"").Length;
                    int bioEnd = personHtml.IndexOf("\",\n  \"birthDate\":");
                    if (bioEnd == -1) { bioEnd = personHtml.IndexOf("(function (win)"); }
                    movie.Casts[i].Bio = personHtml.Substring(bioStart, bioEnd - bioStart);
                    bioTxtBox.Text = movie.Casts[i].Bio;

                    int bDateStart = personHtml.IndexOf("\"birthDate\": \"") + ("\"birthDate\": \"").Length;
                    movie.Casts[i].BirthDate = personHtml.Substring(bDateStart, 10);
                    birthTxtBox.Text = movie.Casts[i].BirthDate;

                    writerListBox.Items.Add(movie.Casts[i]);
                }
            }
        }

        private void directorLstBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string personHtml;
            string personUrl;
            string urlName;

            Cast person = (Cast)directorListBox.SelectedItem;

            personNameTxtBox.Text = person.Name;

            urlName = person.ImdbID.Replace(',', ' ').Trim().Replace('\"', ' ').Trim();
            personUrl = "https://www.imdb.com" + urlName + "?ref_=tt_ov_dr";
            personHtml = clientMethod(personUrl);
            int jobTitleStart = personHtml.IndexOf("\"jobTitle\": [") + ("\"jobTitle\": [").Length;
            int jobTitleEnd = personHtml.IndexOf("],\n  \"description\":");
            person.JobTitle = personHtml.Substring(jobTitleStart, jobTitleEnd - jobTitleStart).Trim().Replace('\"', ' ');
            jobTitleTxtBox.Text = person.JobTitle;
            int personPicStart = personHtml.IndexOf("\"image\":") + ("\"image\":\"\"").Length;

            if (personPicStart != 9)
            {
                person.ImageUrl = personHtml.Substring(personPicStart, jobTitleStart - (personPicStart + ("\",\n  \"jobTitle\": [").Length));
                personPicBox.Load(person.ImageUrl);
            }

            int bioStart = personHtml.IndexOf("\"description\": \"") + ("\"description\": \"").Length;
            int bioEnd = personHtml.IndexOf("\",\n  \"birthDate\":");
            person.Bio = personHtml.Substring(bioStart, bioEnd - bioStart);
            bioTxtBox.Text = person.Bio;

            int bDateStart = personHtml.IndexOf("\"birthDate\": \"") + ("\"birthDate\": \"").Length;
            person.BirthDate = personHtml.Substring(bDateStart, 10);
            birthTxtBox.Text = person.BirthDate;

        }

        private void writerListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string personHtml;
            string personUrl;
            string urlName;

            Cast person_writer = (Cast)writerListBox.SelectedItem;
            //Person person_writer = new Person();
            personNameTxtBox.Text = person_writer.Name;
            urlName = person_writer.ImdbID.Replace(',', ' ').Trim().Replace('\"', ' ').Trim();
            personUrl = "https://www.imdb.com" + urlName + "?ref_=tt_ov_wr";
            personHtml = clientMethod(personUrl);
            int jobTitleStart = personHtml.IndexOf("\"jobTitle\": [") + ("\"jobTitle\": [").Length;
            int jobTitleEnd = personHtml.IndexOf("],\n  \"description\":");
            if (jobTitleEnd == -1)
            {
                jobTitleEnd = personHtml.IndexOf("(function (win)");
            }
            person_writer.JobTitle = personHtml.Substring(jobTitleStart, jobTitleEnd - jobTitleStart).Trim().Replace('\"', ' ');
            jobTitleTxtBox.Text = person_writer.JobTitle;

            int personPicStart = personHtml.IndexOf("\"image\":") + ("\"image\":\"\"").Length;
            if (personPicStart != 9)
            {
                person_writer.ImageUrl = personHtml.Substring(personPicStart, jobTitleStart - (personPicStart + ("\",\n  \"jobTitle\": [").Length));
                personPicBox.Load(person_writer.ImageUrl);
            }
            int bioStart = personHtml.IndexOf("\"description\": \"") + ("\"description\": \"").Length;
            int bioEnd = personHtml.IndexOf("\",\n  \"birthDate\":");
            person_writer.Bio = personHtml.Substring(bioStart, bioEnd - bioStart);
            bioTxtBox.Text = person_writer.Bio;

            int bDateStart = personHtml.IndexOf("\"birthDate\": \"") + ("\"birthDate\": \"").Length;
            person_writer.BirthDate = personHtml.Substring(bDateStart, 10);
            birthTxtBox.Text = person_writer.BirthDate;

            //dBConnect.savePerson(person_writer);
        }

        private void actorListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string personHtml;
            string personUrl;
            string urlName;

            Cast person_actor = (Cast)actorListBox.SelectedItem;
            personNameTxtBox.Text = person_actor.Name;
            urlName = person_actor.ImdbID.Replace(',', ' ').Trim().Replace('\"', ' ').Trim();
            personUrl = "https://www.imdb.com" + urlName + "?ref_=tt_ov_st_sm";
            personHtml = clientMethod(personUrl);
            int jobTitleStart = personHtml.IndexOf("\"jobTitle\": [") + ("\"jobTitle\": [").Length;
            int jobTitleEnd = personHtml.IndexOf("\"description\":");
            person_actor.JobTitle = personHtml.Substring(jobTitleStart, jobTitleEnd - (jobTitleStart + ("],").Length)).Trim().Replace('\"', ' ');
            jobTitleTxtBox.Text = person_actor.JobTitle;

            int personPicStart = personHtml.IndexOf("\"image\":") + ("\"image\":\"\"").Length;
            if (personPicStart != 9)
            {
                person_actor.ImageUrl = personHtml.Substring(personPicStart, jobTitleStart - (personPicStart + ("\",\n  \"jobTitle\": [").Length));
                personPicBox.Load(person_actor.ImageUrl);
            }
            int bioStart = personHtml.IndexOf("\"description\": \"") + ("\"description\": \"").Length;
            int bioEnd = personHtml.IndexOf("\",\n  \"birthDate\":");
            person_actor.Bio = personHtml.Substring(bioStart, bioEnd - bioStart);
            bioTxtBox.Text = person_actor.Bio;

            int bDateStart = personHtml.IndexOf("\"birthDate\": \"") + ("\"birthDate\": \"").Length;
            person_actor.BirthDate = personHtml.Substring(bDateStart, 10);
            birthTxtBox.Text = person_actor.BirthDate;

        }

        private void saveMovieBtn_Click(object sender, EventArgs e)
        {
                Movie item = (Movie)lstBoxTitles.SelectedItem;
                if (dBConnect.movieControl(item) != true)
                {
                    string movieUrl = ("https://www.imdb.com" + item.ImdbTitleID);
                    string htmlDetailString = clientMethod(movieUrl);

                    int rateStart = htmlDetailString.IndexOf("\"ratingValue\": ");
                    if (rateStart != -1)
                    {
                        int rateEnd = htmlDetailString.IndexOf("\"review\":");
                        if (rateEnd != -1)
                        {
                            item.Rate = htmlDetailString.Substring(rateStart + ("\"ratingValue\": \"").Length, (rateEnd - rateStart) - ("\"\n  },\n  \"review\": {\n    ").Length);
                        }
                    }
                    else
                    {
                        item.Rate = "";
                    }
                    int yearStart = htmlDetailString.IndexOf("\"datePublished\":");
                    item.DatePublished = htmlDetailString.Substring(yearStart + ("\"datePublished\":\"").Length, ("\"1999-03-31\"").Length);
                    int contentsLocation = htmlDetailString.IndexOf("\"@type\":");
                    int endOfContentsLocation = htmlDetailString.IndexOf("\"description\":");
                    if (endOfContentsLocation == -1)
                    { endOfContentsLocation = htmlDetailString.IndexOf("\"datePublished\":"); }

                    string specialContent = htmlDetailString.Substring(contentsLocation, endOfContentsLocation - contentsLocation + 1);

                    //GENRE FUNCTIONS 
                    int genreLocation = specialContent.IndexOf("\"genre\":") + ("\"genre\": [").Length;
                    int genreEndLocation = specialContent.IndexOf(" ],");
                    string genreText = specialContent.Substring(genreLocation, genreEndLocation - genreLocation);
                    string[] genreSeps = { "," };
                    string[] genres = genreText.Split(genreSeps, StringSplitOptions.None);
                    foreach (string gen in genres)
                    {
                        dBConnect.saveGenres(gen.Replace("\"","").Trim());
                    }

                    if (specialContent.IndexOf("\"image\": \"") != -1)
                    {
                        int picLocation = specialContent.IndexOf("\"image\": \"") + ("\"image\": \"").Length;
                        int picEndLocation = specialContent.IndexOf("\",\n  \"genre\"");
                        item.ImageUrl = specialContent.Substring(picLocation, picEndLocation - picLocation);  // RESIMLER
                    }
                    else
                    {
                        item.ImageUrl = null;
                    }

                    if (htmlDetailString.IndexOf("\"datePublished\":") < htmlDetailString.IndexOf("\"uploadDate\":"))
                    {
                        item.Desc = htmlDetailString.Substring(htmlDetailString.IndexOf("\"description\": ") + ("\"description\": ").Length, htmlDetailString.IndexOf("\"uploadDate\":") - (htmlDetailString.IndexOf("\"description\": ") + (",\n  \"uploadDate\"").Length)).Trim();
                    }
                    else
                    {
                        item.Desc = htmlDetailString.Substring(htmlDetailString.IndexOf("\"description\": ") + ("\"description\": ").Length, (htmlDetailString.IndexOf("\"datePublished\":") - ((htmlDetailString.IndexOf("\"description\": ") + (",\n  \"datePublished\"").Length)))).Trim();
                    }
                    string actorText;
                    string directorText;
                    string creatorText;

                    int count;
                    string[] seps = { "\"@type\": \"Person\"" };

                    int actorStart = specialContent.IndexOf("\"actor\": ");
                    int directorStart = specialContent.IndexOf("\"director\": ");
                    int creatorStart = specialContent.IndexOf("\"creator\":");


                    if (specialContent.Contains("\"director\": "))
                    {
                        if (actorStart != -1)
                        {
                            actorText = specialContent.Substring(actorStart, directorStart - actorStart).Remove(0, 20 + ("\"@type\": \"Person\",").Length);
                            string[] actors = actorText.Split(seps, StringSplitOptions.None);

                            foreach (string actor in actors)
                            {
                                count = 0;
                                Cast person2 = new Cast();
                                int acUrlIndex = actor.IndexOf("\"url\":") + ("\"url\":").Length;
                                int acUrlEndIndex = actor.IndexOf("\"name\":");
                                person2.ImdbID = actor.Substring(acUrlIndex, acUrlEndIndex - (acUrlIndex + 1)).Trim();
                                person2.Name = actor.Substring(actor.IndexOf("\"name\": ") + ("\"name\": ").Length, actor.IndexOf("}") - (actor.IndexOf("\"name\": ") + ("\"name\": ").Length));
                                //person2.Roles.Add(Role.Actor);
                                foreach (Cast cast in item.Casts)
                                {
                                    if (person2.Name == cast.Name)
                                    {
                                        //cast.Roles.Add(Role.Actor);
                                        count++;
                                    }

                                }
                                if (count < 1)
                                {
                                    item.Casts.Add(person2);
                                }

                            }
                        }
                        if (creatorStart != -1)
                        {
                            directorText = specialContent.Substring(directorStart, creatorStart - directorStart).Remove(0, 25 + ("\"@type\": \"Person\",").Length);
                            string[] directors = directorText.Split(seps, StringSplitOptions.None);

                            foreach (string director in directors)
                            {
                                count = 0;
                                Cast person1 = new Cast();
                                Person p = new Person();
                                int dirUrlIndex = director.IndexOf("\"url\":") + ("\"url\":").Length;
                                int dirUrlEndIndex = director.IndexOf("\"name\":");
                                person1.ImdbID = director.Substring(dirUrlIndex, dirUrlEndIndex - (dirUrlIndex + 1)).Trim();
                                person1.Name = director.Substring(director.IndexOf("\"name\": ") + ("\"name\": ").Length, director.IndexOf("}") - (director.IndexOf("\"name\": ") + ("\"name\": ").Length));
                                person1.Roles.Add(Role.Director);

                                foreach (Cast cast in item.Casts)
                                {
                                    if (person1.Name == cast.Name)
                                    {
                                        count++;
                                    }
                                }

                                if (count < 1)
                                {
                                    item.Casts.Add(person1);
                                }

                            }
                        }
                        else
                        {
                            specialContent = specialContent.Remove(0, directorStart + ("\"director\": {\n  ").Length).Trim();
                            string[] directors = specialContent.Split(seps, StringSplitOptions.RemoveEmptyEntries);

                            foreach (string director in directors)
                            {
                                count = 0;
                                Cast person1 = new Cast();
                                Person p = new Person();
                                int dirUrlIndex = director.IndexOf("\"url\":") + ("\"url\":").Length;
                                int dirUrlEndIndex = director.IndexOf("\"name\":");
                                person1.ImdbID = director.Substring(dirUrlIndex, dirUrlEndIndex - (dirUrlIndex + 1)).Trim();
                                person1.Name = director.Substring(director.IndexOf("\"name\": ") + ("\"name\": ").Length, director.IndexOf("}") - (director.IndexOf("\"name\": ") + ("\"name\": ").Length));
                                person1.Roles.Add(Role.Director);

                                foreach (Cast cast in item.Casts)
                                {
                                    if (person1.Name == cast.Name)
                                    {
                                        count++;
                                    }
                                }

                                if (count < 1)
                                {
                                    item.Casts.Add(person1);
                                }

                            }
                        }
                    }
                    else
                    {
                        if (creatorStart != -1)
                        {
                            actorText = specialContent.Substring(actorStart, creatorStart - actorStart).Remove(0, 20 + ("\"@type\": \"Person\",").Length);
                        }
                        actorText = specialContent.Substring(actorStart);
                    }

                    if (specialContent.Contains("\"creator\": "))
                    {
                        creatorText = specialContent.Substring(creatorStart).Remove(0, "\"creator\": [\n    {\n      ".Length).Trim();
                        string[] creators = creatorText.Split(seps, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string creator in creators)
                        {
                            count = 0;
                            Cast person3 = new Cast();
                            int crUrlIndex = creator.IndexOf("\"url\":") + ("\"url\":").Length;
                            int crUrlEndIndex = creator.IndexOf("\"name\":");
                            person3.ImdbID = creator.Substring(crUrlIndex, crUrlEndIndex - crUrlIndex).Trim();
                            person3.Name = creator.Substring(creator.IndexOf("\"name\": ") + ("\"name\": ").Length, creator.IndexOf("}") - (creator.IndexOf("\"name\": ") + ("\"name\": ").Length));

                            //person3.Roles.Add(Role.Writer);

                            foreach (Cast cast in item.Casts)
                            {
                                if (person3.Name == cast.Name)
                                {
                                    cast.Roles.Add(Role.Writer);
                                    count++;
                                }
                            }
                            if (count < 1)
                            {
                                item.Casts.Add(person3);
                            }

                        }
                    }


                    dBConnect.saveMovies(item);
                }
            
        }


    }


}

