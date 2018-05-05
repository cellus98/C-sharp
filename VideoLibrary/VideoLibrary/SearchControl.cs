using System;
using System.Windows.Forms;
using System.Collections;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.IO;
using Microsoft.VisualBasic;
using System.Data;
using System.Globalization;

namespace VideoLibrary
{
    public partial class SearchControl : UserControl
    {
        RadioButton GenreRadioButton = new RadioButton();
        RadioButton MPAARadioButton = new RadioButton();
        RadioButton RatingRadioButton = new RadioButton();
        RadioButton AwardsRadioButton = new RadioButton();
        RadioButton NameRadioButton = new RadioButton();
        RadioButton DirectRadioButton = new RadioButton();
        RadioButton ActRadioButton = new RadioButton();

        string GenreSearch = "";
        string MPAASearch = "";
        string RatingSearch = "";
        string AwardsSearch = "";
        string TitleSearch = "";
        string DirectorSearch = "";
        string ActorSearch = "";

        DataGridView SearchGrid = new DataGridView();
        DataTable SearchTable = new DataTable();

        int TotalSearchCount = 0;

        bool TitleSelected = false;
        bool DirectorSelected = false;
        bool ActorSelected = false;
        bool Blank = false;

        Form SearchControlForm = new Form();

        public SearchControl()
        {
            InitializeComponent();
        }



    private void Get_DataTable()
        {

            //string filePath = @"C:\Users\v-mase\Desktop\C#\Jerry\test2.xlsx";

            string filePath;
            OpenFileDialog openfile = VideoLibrary.OpenNewFile();
            openfile.ShowDialog();
            filePath = VideoLibrary.CheckOpenFile(openfile);
            while (!(File.Exists(filePath)))
            {
                filePath = VideoLibrary.CheckOpenFile(openfile);
            }

            Excel.Application objExcel;
            Excel.Workbook objBook;
            Excel._Worksheet objSheet;
            Excel.Range rowRange;
            VideoLibrary.SetupExcel(filePath, out objExcel, out objBook, out objSheet, out rowRange);

            SearchTable.Columns.Add("Title");
            SearchTable.Columns.Add("Year");
            SearchTable.Columns.Add("Rating");
            SearchTable.Columns.Add("Length");
            SearchTable.Columns.Add("MPAA");
            SearchTable.Columns.Add("Genre");
            SearchTable.Columns.Add("Actors");
            SearchTable.Columns.Add("Directors");
            SearchTable.Columns.Add("Awards");
            SearchTable.Columns.Add("Comments");
            SearchTable.Columns.Add("Location(Note Book and Disc)[2]");
            SearchTable.Columns.Add("IMDB");
            
            string column = "A";
            int count = 2;
            int x = 0;
            int rows;
            rows = objSheet.UsedRange.Rows.Count;

            while (x < rows - 1)
            {
                DataRow excelRow = SearchTable.NewRow();
                for (int i = 0; i < 12; i++)
                {
                    if (column == "D")
                    {
                        var value = (objSheet.Cells[count, column]).Text;
                        excelRow[i] = value;
                        column = VideoLibrary.Increment(column);
                    }
                    else
                    {
                        var value = (objSheet.Cells[count, column]).Value2;
                        excelRow[i] = Convert.ToString(value);
                        column = VideoLibrary.Increment(column);
                    }
                }
                SearchTable.Rows.Add(excelRow);
                x++;
                count++;
                column = "A";
            }
            VideoLibrary.ReleaseExcelObject(filePath, objExcel, objBook, objSheet);
        }


        private void Get_Input()
        {
            string Message = "";

            if (TitleSelected)
            {
                TitleSearch = TitleTextBox.Text.ToString();
                while (String.IsNullOrWhiteSpace(TitleSearch) || String.IsNullOrEmpty(TitleSearch))
                {
                    MessageBox.Show("Invalid title entered try again", "Invalid Title", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Message = "You entered blank Title Name!" + Environment.NewLine + "      Would you like to try again ?";
                    if (MessageBox.Show(Message, "New Title Search", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        TitleSearch = Interaction.InputBox("Enter Title Name To Find", "Locate Title", "");
                    }
                    else
                    {
                        return;                    // Exits out current function
                    }
                }
            }

            if (DirectorSelected)
            {
                DirectorSearch = DirectorTextBox.Text.ToString();
                while (String.IsNullOrWhiteSpace(DirectorSearch))
                {
                    MessageBox.Show("Invalid Director Name", "Invalid Director Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Message = "You entered blank Director Name!" + Environment.NewLine + "      Would you like to try again ?";
                    if (MessageBox.Show(Message, "New Director Search", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        DirectorSearch = Interaction.InputBox("Enter Director Name To Find", "Locate Director", "");
                    }
                    else
                    {
                        return;                    // Exits out current function
                    }
                }
            }

            if (ActorSelected)
            {
                ActorSearch = ActorTextBox.Text.ToString();
                while (String.IsNullOrWhiteSpace(ActorSearch))
                {
                    MessageBox.Show("Invalid Actor/Actress Name", "Invalid Actor/Actress Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Message = "You entered blank Actor/Actress Name!" + Environment.NewLine + "      Would you like to try again ?";
                    if (MessageBox.Show(Message, "New Actor/Actress Search", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        ActorSearch = Interaction.InputBox("Enter Actor/Actress Name To Find", "Locate Actor/Actress", "");
                    }
                    else
                    {
                        return;                    // Exits out current function
                    }
                }
            }
        }


        private void SearchButton_Click(object sender, EventArgs e)
        {

            Get_DataTable();
            Get_Input();

            string MessageFilter = "";
            string TitleFilter = "Title LIKE '%" + TitleSearch + "%'";
            string DirectorFilter = "Directors LIKE '%" + DirectorSearch + "%'";
            string ActorFilter = "Actors LIKE '%" + ActorSearch + "%'";
            string AwardsFilter = "Awards LIKE '%" + AwardsSearch + "%'";
            string RatingFilter = "Rating = '" + RatingSearch + "'";
            string MPAAFilter = "MPAA = '" + MPAASearch + "'";
            string GenreFilter = "Genre LIKE '%" + GenreSearch + "%'";

            Form DisplayAllForm = new Form()
            {
                Text = "Search Results",
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FormBorderStyle = FormBorderStyle.Fixed3D,
                AutoScroll = true,
                AllowDrop = true,           // allows user to drag and drop
                Enabled = true             // changes if user can enter data (true = yes, false = no)
            };

            DisplayControl NewSubControl = new DisplayControl()
            {
                ForeColor = System.Drawing.Color.Black,
                BackColor = System.Drawing.Color.Lavender,
                AutoSize = true,
                BorderStyle = BorderStyle.Fixed3D,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                AutoScroll = true
            };

            SearchGrid.Location = new System.Drawing.Point(11, 33);
            SearchGrid.Name = "dataGridView1";
            SearchGrid.Size = new System.Drawing.Size(1000, 600);
            SearchGrid.TabIndex = 0;

            NewSubControl.Controls.Add(SearchGrid);
            DisplayAllForm.Controls.Add(NewSubControl);

            switch (TotalSearchCount)
            {
                case 1:
                    if ((!String.IsNullOrWhiteSpace(TitleSearch) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(ActorSearch) && String.IsNullOrWhiteSpace(AwardsSearch) && String.IsNullOrWhiteSpace(RatingSearch) && String.IsNullOrWhiteSpace(MPAASearch) && String.IsNullOrWhiteSpace(GenreSearch)))
                    {
                        MessageFilter = TitleFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(TitleSearch) && String.IsNullOrWhiteSpace(ActorSearch) && String.IsNullOrWhiteSpace(AwardsSearch) && String.IsNullOrWhiteSpace(RatingSearch) && String.IsNullOrWhiteSpace(MPAASearch) && String.IsNullOrWhiteSpace(GenreSearch)))
                    {
                        MessageFilter = DirectorFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(ActorSearch) && String.IsNullOrWhiteSpace(TitleSearch) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(AwardsSearch) && String.IsNullOrWhiteSpace(RatingSearch) && String.IsNullOrWhiteSpace(MPAASearch) && String.IsNullOrWhiteSpace(GenreSearch)))
                    {
                        MessageFilter = ActorFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(AwardsSearch) && String.IsNullOrWhiteSpace(TitleSearch) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(ActorSearch) && String.IsNullOrWhiteSpace(RatingSearch) && String.IsNullOrWhiteSpace(MPAASearch) && String.IsNullOrWhiteSpace(GenreSearch)))
                    {
                        MessageFilter = AwardsFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(RatingSearch) && String.IsNullOrWhiteSpace(TitleSearch) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(ActorSearch) && String.IsNullOrWhiteSpace(AwardsSearch) && String.IsNullOrWhiteSpace(MPAASearch) && String.IsNullOrWhiteSpace(GenreSearch)))
                    {
                        MessageFilter = RatingFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(MPAASearch) && String.IsNullOrWhiteSpace(TitleSearch) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(ActorSearch) && String.IsNullOrWhiteSpace(AwardsSearch) && String.IsNullOrWhiteSpace(RatingSearch) && String.IsNullOrWhiteSpace(GenreSearch)))
                    {
                        MessageFilter = MPAAFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(GenreSearch) && String.IsNullOrWhiteSpace(TitleSearch) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(ActorSearch) && String.IsNullOrWhiteSpace(AwardsSearch) && String.IsNullOrWhiteSpace(RatingSearch) && String.IsNullOrWhiteSpace(MPAASearch)))
                    {
                        MessageFilter = GenreFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    break;

                case 2:

                    if ((!String.IsNullOrWhiteSpace(TitleSearch)) && (!String.IsNullOrWhiteSpace(DirectorSearch)) && (String.IsNullOrWhiteSpace(ActorSearch)) && (String.IsNullOrWhiteSpace(AwardsSearch)) && (String.IsNullOrWhiteSpace(RatingSearch)) && (String.IsNullOrWhiteSpace(MPAASearch)) && (String.IsNullOrWhiteSpace(GenreSearch)))
                    {
                        MessageFilter = TitleFilter + " AND " + DirectorFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(ActorSearch)) && (!String.IsNullOrWhiteSpace(AwardsSearch)) && String.IsNullOrWhiteSpace(TitleSearch) && String.IsNullOrWhiteSpace(AwardsSearch) && String.IsNullOrWhiteSpace(RatingSearch) && String.IsNullOrWhiteSpace(MPAASearch) && String.IsNullOrWhiteSpace(GenreSearch))
                    {
                        MessageFilter = ActorFilter + " AND " + AwardsFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(RatingSearch)) && (!String.IsNullOrWhiteSpace(MPAASearch)) && String.IsNullOrWhiteSpace(TitleSearch) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(RatingSearch) && String.IsNullOrWhiteSpace(MPAASearch) && String.IsNullOrWhiteSpace(GenreSearch))
                    {
                        MessageFilter = RatingFilter + " AND " + MPAAFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(DirectorSearch)) && (!String.IsNullOrWhiteSpace(ActorSearch)) && String.IsNullOrWhiteSpace(TitleSearch) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(ActorSearch) && String.IsNullOrWhiteSpace(MPAASearch) && String.IsNullOrWhiteSpace(GenreSearch))
                    {
                        MessageFilter = DirectorFilter + " AND " + ActorFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(AwardsSearch)) && (!String.IsNullOrWhiteSpace(RatingSearch)) && String.IsNullOrWhiteSpace(TitleSearch) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(ActorSearch) && String.IsNullOrWhiteSpace(AwardsSearch) && String.IsNullOrWhiteSpace(GenreSearch))
                    {
                        MessageFilter = AwardsFilter + " AND " + RatingFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(MPAASearch)) && (!String.IsNullOrWhiteSpace(GenreSearch)) && String.IsNullOrWhiteSpace(TitleSearch) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(ActorSearch) && String.IsNullOrWhiteSpace(AwardsSearch) && String.IsNullOrWhiteSpace(RatingSearch))
                    {
                        MessageFilter = MPAAFilter + " AND " + GenreFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(TitleSearch)) && (!String.IsNullOrWhiteSpace(ActorSearch)) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(AwardsSearch) && String.IsNullOrWhiteSpace(RatingSearch) && String.IsNullOrWhiteSpace(MPAASearch) && String.IsNullOrWhiteSpace(GenreSearch))
                    {
                        MessageFilter = TitleFilter + " AND " + ActorFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(DirectorSearch)) && (!String.IsNullOrWhiteSpace(AwardsSearch)) && String.IsNullOrWhiteSpace(TitleSearch) && String.IsNullOrWhiteSpace(ActorSearch) && String.IsNullOrWhiteSpace(RatingSearch) && String.IsNullOrWhiteSpace(MPAASearch) && String.IsNullOrWhiteSpace(GenreSearch))
                    {
                        MessageFilter = DirectorFilter + " AND " + AwardsFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(ActorSearch)) && (!String.IsNullOrWhiteSpace(RatingSearch)) && String.IsNullOrWhiteSpace(TitleSearch) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(AwardsSearch) && String.IsNullOrWhiteSpace(MPAASearch) && String.IsNullOrWhiteSpace(GenreSearch))
                    {
                        MessageFilter = ActorFilter + " AND " + RatingFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(AwardsSearch)) && (!String.IsNullOrWhiteSpace(MPAASearch)) && String.IsNullOrWhiteSpace(TitleSearch) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(ActorSearch) && String.IsNullOrWhiteSpace(RatingSearch) && String.IsNullOrWhiteSpace(GenreSearch))
                    {
                        MessageFilter = AwardsFilter + " AND " + MPAAFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(RatingSearch)) && (!String.IsNullOrWhiteSpace(GenreSearch)) && String.IsNullOrWhiteSpace(TitleSearch) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(ActorSearch) && String.IsNullOrWhiteSpace(AwardsSearch) && String.IsNullOrWhiteSpace(MPAASearch))
                    {
                        MessageFilter = RatingFilter + " AND " + GenreFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(TitleSearch)) && (!String.IsNullOrWhiteSpace(AwardsSearch)) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(ActorSearch) && String.IsNullOrWhiteSpace(RatingSearch) && String.IsNullOrWhiteSpace(MPAASearch) && String.IsNullOrWhiteSpace(GenreSearch))
                    {
                        MessageFilter = TitleFilter + " AND " + AwardsFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(DirectorSearch)) && (!String.IsNullOrWhiteSpace(RatingSearch)) && String.IsNullOrWhiteSpace(TitleSearch) && String.IsNullOrWhiteSpace(ActorSearch) && String.IsNullOrWhiteSpace(AwardsSearch) && String.IsNullOrWhiteSpace(MPAASearch) && String.IsNullOrWhiteSpace(GenreSearch))
                    {
                        MessageFilter = DirectorFilter + " AND " + RatingFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(ActorSearch)) && (!String.IsNullOrWhiteSpace(MPAASearch)) && String.IsNullOrWhiteSpace(TitleSearch) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(AwardsSearch) && String.IsNullOrWhiteSpace(RatingSearch) && String.IsNullOrWhiteSpace(GenreSearch))
                    {
                        MessageFilter = ActorFilter + " AND " + MPAAFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(AwardsSearch)) && (!String.IsNullOrWhiteSpace(GenreSearch)) && String.IsNullOrWhiteSpace(TitleSearch) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(ActorSearch) && String.IsNullOrWhiteSpace(RatingSearch) && String.IsNullOrWhiteSpace(MPAASearch))
                    {
                        MessageFilter = AwardsFilter + " AND " + GenreFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(TitleSearch)) && (!String.IsNullOrWhiteSpace(RatingSearch)) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(ActorSearch) && String.IsNullOrWhiteSpace(AwardsSearch) && String.IsNullOrWhiteSpace(MPAASearch) && String.IsNullOrWhiteSpace(GenreSearch))
                    {
                        MessageFilter = TitleFilter + " AND " + RatingFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(DirectorSearch)) && (!String.IsNullOrWhiteSpace(MPAASearch)) && String.IsNullOrWhiteSpace(TitleSearch) && String.IsNullOrWhiteSpace(ActorSearch) && String.IsNullOrWhiteSpace(AwardsSearch) && String.IsNullOrWhiteSpace(RatingSearch) && String.IsNullOrWhiteSpace(GenreSearch))
                    {
                        MessageFilter = DirectorFilter + " AND " + MPAAFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(ActorSearch)) && (!String.IsNullOrWhiteSpace(GenreSearch)) && String.IsNullOrWhiteSpace(TitleSearch) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(AwardsSearch) && String.IsNullOrWhiteSpace(RatingSearch) && String.IsNullOrWhiteSpace(MPAASearch))
                    {
                        MessageFilter = ActorFilter + " AND " + GenreFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(TitleSearch)) && (!String.IsNullOrWhiteSpace(MPAASearch)) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(ActorSearch) && String.IsNullOrWhiteSpace(AwardsSearch) && String.IsNullOrWhiteSpace(RatingSearch) && String.IsNullOrWhiteSpace(GenreSearch))
                    {
                        MessageFilter = TitleFilter + " AND " + MPAAFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(DirectorSearch)) && (!String.IsNullOrWhiteSpace(GenreSearch)) && String.IsNullOrWhiteSpace(TitleSearch) && String.IsNullOrWhiteSpace(ActorSearch) && String.IsNullOrWhiteSpace(AwardsSearch) && String.IsNullOrWhiteSpace(RatingSearch) && String.IsNullOrWhiteSpace(MPAASearch))
                    {
                        MessageFilter = DirectorFilter + " AND " + GenreFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(TitleSearch)) && (!String.IsNullOrWhiteSpace(GenreSearch)) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(ActorSearch) && String.IsNullOrWhiteSpace(AwardsSearch) && String.IsNullOrWhiteSpace(RatingSearch) && String.IsNullOrWhiteSpace(MPAASearch))
                    {
                        MessageFilter = TitleFilter + " AND " + GenreFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    break;

                case 3:
                    if ((!String.IsNullOrWhiteSpace(TitleSearch)) && (!String.IsNullOrWhiteSpace(DirectorSearch)) && (!String.IsNullOrWhiteSpace(ActorSearch)) && String.IsNullOrWhiteSpace(AwardsSearch) && String.IsNullOrWhiteSpace(RatingSearch) && String.IsNullOrWhiteSpace(MPAASearch) && String.IsNullOrWhiteSpace(GenreSearch))
                    {
                        MessageFilter = TitleFilter + " AND " + DirectorFilter + " AND " + ActorFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(TitleSearch)) && (!String.IsNullOrWhiteSpace(DirectorSearch)) && (!String.IsNullOrWhiteSpace(AwardsSearch)) && String.IsNullOrWhiteSpace(ActorSearch) && String.IsNullOrWhiteSpace(RatingSearch) && String.IsNullOrWhiteSpace(MPAASearch) && String.IsNullOrWhiteSpace(GenreSearch))
                    {
                        MessageFilter = TitleFilter + " AND " + DirectorFilter + " AND " + AwardsFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(TitleSearch)) && (!String.IsNullOrWhiteSpace(DirectorSearch)) && (!String.IsNullOrWhiteSpace(RatingSearch)) && String.IsNullOrWhiteSpace(ActorSearch) && String.IsNullOrWhiteSpace(AwardsSearch) && String.IsNullOrWhiteSpace(MPAASearch) && String.IsNullOrWhiteSpace(GenreSearch))
                    {
                        MessageFilter = TitleFilter + " AND " + DirectorFilter + " AND " + RatingFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(TitleSearch)) && (!String.IsNullOrWhiteSpace(DirectorSearch)) && (!String.IsNullOrWhiteSpace(MPAASearch)) && String.IsNullOrWhiteSpace(ActorSearch) && String.IsNullOrWhiteSpace(AwardsSearch) && String.IsNullOrWhiteSpace(RatingSearch) && String.IsNullOrWhiteSpace(GenreSearch))
                    {
                        MessageFilter = TitleFilter + " AND " + DirectorFilter + " AND " + MPAAFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(TitleSearch)) && (!String.IsNullOrWhiteSpace(DirectorSearch)) && (!String.IsNullOrWhiteSpace(GenreSearch)) && String.IsNullOrWhiteSpace(ActorSearch) && String.IsNullOrWhiteSpace(AwardsSearch) && String.IsNullOrWhiteSpace(RatingSearch) && String.IsNullOrWhiteSpace(MPAASearch))
                    {
                        MessageFilter = TitleFilter + " AND " + DirectorFilter + " AND " + GenreFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(TitleSearch)) && (!String.IsNullOrWhiteSpace(ActorSearch)) && (!String.IsNullOrWhiteSpace(AwardsSearch)) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(RatingSearch) && String.IsNullOrWhiteSpace(MPAASearch) && String.IsNullOrWhiteSpace(GenreSearch))
                    {
                        MessageFilter = TitleFilter + " AND " + ActorFilter + " AND " + AwardsFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(TitleSearch)) && (!String.IsNullOrWhiteSpace(ActorSearch)) && (!String.IsNullOrWhiteSpace(RatingSearch)) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(AwardsSearch) && String.IsNullOrWhiteSpace(MPAASearch) && String.IsNullOrWhiteSpace(GenreSearch))
                    {
                        MessageFilter = TitleFilter + " AND " + ActorFilter + " AND " + RatingFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(TitleSearch)) && (!String.IsNullOrWhiteSpace(ActorSearch)) && (!String.IsNullOrWhiteSpace(MPAASearch)) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(AwardsSearch) && String.IsNullOrWhiteSpace(RatingSearch) && String.IsNullOrWhiteSpace(GenreSearch))
                    {
                        MessageFilter = TitleFilter + " AND " + ActorFilter + " AND " + MPAAFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(TitleSearch)) && (!String.IsNullOrWhiteSpace(ActorSearch)) && (!String.IsNullOrWhiteSpace(GenreSearch)) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(AwardsSearch) && String.IsNullOrWhiteSpace(RatingSearch) && String.IsNullOrWhiteSpace(MPAASearch))
                    {
                        MessageFilter = TitleFilter + " AND " + ActorFilter + " AND " + GenreFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(TitleSearch)) && (!String.IsNullOrWhiteSpace(AwardsSearch)) && (!String.IsNullOrWhiteSpace(RatingSearch)) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(ActorSearch) && String.IsNullOrWhiteSpace(MPAASearch) && String.IsNullOrWhiteSpace(GenreSearch))
                    {
                        MessageFilter = TitleFilter + " AND " + AwardsFilter + " AND " + RatingFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(TitleSearch)) && (!String.IsNullOrWhiteSpace(AwardsSearch)) && (!String.IsNullOrWhiteSpace(RatingSearch)) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(ActorSearch) && String.IsNullOrWhiteSpace(MPAASearch) && String.IsNullOrWhiteSpace(GenreSearch))
                    {
                        MessageFilter = TitleFilter + " AND " + AwardsFilter + " AND " + RatingFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(TitleSearch)) && (!String.IsNullOrWhiteSpace(AwardsSearch)) && (!String.IsNullOrWhiteSpace(MPAASearch)) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(ActorSearch) && String.IsNullOrWhiteSpace(RatingSearch) && String.IsNullOrWhiteSpace(GenreSearch))
                    {
                        MessageFilter = TitleFilter + " AND " + AwardsFilter + " AND " + MPAAFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(TitleSearch)) && (!String.IsNullOrWhiteSpace(AwardsSearch)) && (!String.IsNullOrWhiteSpace(GenreSearch)) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(ActorSearch) && String.IsNullOrWhiteSpace(RatingSearch) && String.IsNullOrWhiteSpace(MPAASearch))
                    {
                        MessageFilter = TitleFilter + " AND " + AwardsFilter + " AND " + GenreFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(TitleSearch)) && (!String.IsNullOrWhiteSpace(RatingSearch)) && (!String.IsNullOrWhiteSpace(MPAASearch)) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(ActorSearch) && String.IsNullOrWhiteSpace(AwardsSearch) && String.IsNullOrWhiteSpace(GenreSearch))
                    {
                        MessageFilter = TitleFilter + " AND " + RatingFilter + " AND " + MPAAFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(TitleSearch)) && (!String.IsNullOrWhiteSpace(RatingSearch)) && (!String.IsNullOrWhiteSpace(GenreSearch)) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(ActorSearch) && String.IsNullOrWhiteSpace(AwardsSearch) && String.IsNullOrWhiteSpace(MPAASearch))
                    {
                        MessageFilter = TitleFilter + " AND " + RatingFilter + " AND " + GenreFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(TitleSearch)) && (!String.IsNullOrWhiteSpace(MPAASearch)) && (!String.IsNullOrWhiteSpace(GenreSearch)) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(ActorSearch) && String.IsNullOrWhiteSpace(AwardsSearch) && String.IsNullOrWhiteSpace(RatingSearch))
                    {
                        MessageFilter = TitleFilter + " AND " + MPAAFilter + " AND " + GenreFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(DirectorSearch)) && (!String.IsNullOrWhiteSpace(ActorSearch)) && (!String.IsNullOrWhiteSpace(AwardsSearch)) && String.IsNullOrWhiteSpace(TitleSearch) && String.IsNullOrWhiteSpace(RatingSearch) && String.IsNullOrWhiteSpace(MPAASearch) && String.IsNullOrWhiteSpace(GenreSearch))
                    {
                        MessageFilter = DirectorFilter + " AND " + ActorFilter + " AND " + AwardsFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(DirectorSearch)) && (!String.IsNullOrWhiteSpace(ActorSearch)) && (!String.IsNullOrWhiteSpace(RatingSearch)) && String.IsNullOrWhiteSpace(TitleSearch) && String.IsNullOrWhiteSpace(AwardsSearch) && String.IsNullOrWhiteSpace(MPAASearch) && String.IsNullOrWhiteSpace(GenreSearch))
                    {
                        MessageFilter = DirectorFilter + " AND " + ActorFilter + " AND " + RatingFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(DirectorSearch)) && (!String.IsNullOrWhiteSpace(ActorSearch)) && (!String.IsNullOrWhiteSpace(MPAASearch)) && String.IsNullOrWhiteSpace(TitleSearch) && String.IsNullOrWhiteSpace(AwardsSearch) && String.IsNullOrWhiteSpace(RatingSearch) && String.IsNullOrWhiteSpace(GenreSearch))
                    {
                        MessageFilter = DirectorFilter + " AND " + ActorFilter + " AND " + MPAAFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(DirectorSearch)) && (!String.IsNullOrWhiteSpace(ActorSearch)) && (!String.IsNullOrWhiteSpace(GenreSearch)) && String.IsNullOrWhiteSpace(TitleSearch) && String.IsNullOrWhiteSpace(AwardsSearch) && String.IsNullOrWhiteSpace(RatingSearch) && String.IsNullOrWhiteSpace(MPAASearch))
                    {
                        MessageFilter = DirectorFilter + " AND " + ActorFilter + " AND " + GenreFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(DirectorSearch)) && (!String.IsNullOrWhiteSpace(AwardsSearch)) && (!String.IsNullOrWhiteSpace(RatingSearch)) && String.IsNullOrWhiteSpace(TitleSearch) && String.IsNullOrWhiteSpace(ActorSearch) && String.IsNullOrWhiteSpace(MPAASearch) && String.IsNullOrWhiteSpace(GenreSearch))
                    {
                        MessageFilter = DirectorFilter + " AND " + AwardsFilter + " AND " + RatingFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(DirectorSearch)) && (!String.IsNullOrWhiteSpace(AwardsSearch)) && (!String.IsNullOrWhiteSpace(MPAASearch)) && String.IsNullOrWhiteSpace(TitleSearch) && String.IsNullOrWhiteSpace(ActorSearch) && String.IsNullOrWhiteSpace(RatingSearch) && String.IsNullOrWhiteSpace(GenreSearch))
                    {
                        MessageFilter = DirectorFilter + " AND " + AwardsFilter + " AND " + MPAAFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(DirectorSearch)) && (!String.IsNullOrWhiteSpace(AwardsSearch)) && (!String.IsNullOrWhiteSpace(GenreSearch)) && String.IsNullOrWhiteSpace(TitleSearch) && String.IsNullOrWhiteSpace(ActorSearch) && String.IsNullOrWhiteSpace(RatingSearch) && String.IsNullOrWhiteSpace(MPAASearch))
                    {
                        MessageFilter = DirectorFilter + " AND " + AwardsFilter + " AND " + GenreFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(DirectorSearch)) && (!String.IsNullOrWhiteSpace(RatingSearch)) && (!String.IsNullOrWhiteSpace(MPAASearch)) && String.IsNullOrWhiteSpace(TitleSearch) && String.IsNullOrWhiteSpace(ActorSearch) && String.IsNullOrWhiteSpace(AwardsSearch) && String.IsNullOrWhiteSpace(GenreSearch))
                    {
                        MessageFilter = DirectorFilter + " AND " + RatingFilter + " AND " + MPAAFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(DirectorSearch)) && (!String.IsNullOrWhiteSpace(RatingSearch)) && (!String.IsNullOrWhiteSpace(GenreSearch)) && String.IsNullOrWhiteSpace(TitleSearch) && String.IsNullOrWhiteSpace(ActorSearch) && String.IsNullOrWhiteSpace(AwardsSearch) && String.IsNullOrWhiteSpace(MPAASearch))
                    {
                        MessageFilter = DirectorFilter + " AND " + RatingFilter + " AND " + GenreFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(DirectorSearch)) && (!String.IsNullOrWhiteSpace(MPAASearch)) && (!String.IsNullOrWhiteSpace(GenreSearch)) && String.IsNullOrWhiteSpace(TitleSearch) && String.IsNullOrWhiteSpace(ActorSearch) && String.IsNullOrWhiteSpace(AwardsSearch) && String.IsNullOrWhiteSpace(RatingSearch))
                    {
                        MessageFilter = DirectorFilter + " AND " + MPAAFilter + " AND " + GenreFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(ActorSearch)) && (!String.IsNullOrWhiteSpace(AwardsSearch)) && (!String.IsNullOrWhiteSpace(RatingSearch)) && String.IsNullOrWhiteSpace(TitleSearch) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(MPAASearch) && String.IsNullOrWhiteSpace(GenreSearch))
                    {
                        MessageFilter = ActorFilter + " AND " + AwardsFilter + " AND " + RatingFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(ActorSearch)) && (!String.IsNullOrWhiteSpace(AwardsSearch)) && (!String.IsNullOrWhiteSpace(MPAASearch)) && String.IsNullOrWhiteSpace(TitleSearch) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(RatingSearch) && String.IsNullOrWhiteSpace(GenreSearch))
                    {
                        MessageFilter = ActorFilter + " AND " + AwardsFilter + " AND " + MPAAFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(ActorSearch)) && (!String.IsNullOrWhiteSpace(AwardsSearch)) && (!String.IsNullOrWhiteSpace(GenreSearch)) && String.IsNullOrWhiteSpace(TitleSearch) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(RatingSearch) && String.IsNullOrWhiteSpace(MPAASearch))
                    {
                        MessageFilter = ActorFilter + " AND " + AwardsFilter + " AND " + GenreFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(ActorSearch)) && (!String.IsNullOrWhiteSpace(RatingSearch)) && (!String.IsNullOrWhiteSpace(MPAASearch)) && String.IsNullOrWhiteSpace(TitleSearch) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(AwardsSearch) && String.IsNullOrWhiteSpace(GenreSearch))
                    {
                        MessageFilter = ActorFilter + " AND " + RatingFilter + " AND " + MPAAFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(ActorSearch)) && (!String.IsNullOrWhiteSpace(RatingSearch)) && (!String.IsNullOrWhiteSpace(GenreSearch)) && String.IsNullOrWhiteSpace(TitleSearch) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(AwardsSearch) && String.IsNullOrWhiteSpace(MPAASearch))
                    {
                        MessageFilter = ActorFilter + " AND " + RatingFilter + " AND " + GenreFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(ActorSearch)) && (!String.IsNullOrWhiteSpace(MPAASearch)) && (!String.IsNullOrWhiteSpace(GenreSearch)) && String.IsNullOrWhiteSpace(TitleSearch) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(AwardsSearch) && String.IsNullOrWhiteSpace(RatingSearch))
                    {
                        MessageFilter = ActorFilter + " AND " + MPAAFilter + " AND " + GenreFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(AwardsSearch)) && (!String.IsNullOrWhiteSpace(RatingSearch)) && (!String.IsNullOrWhiteSpace(MPAASearch)) && String.IsNullOrWhiteSpace(TitleSearch) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(ActorSearch) && String.IsNullOrWhiteSpace(GenreSearch))
                    {
                        MessageFilter = AwardsFilter + " AND " + RatingFilter + " AND " + MPAAFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(AwardsSearch)) && (!String.IsNullOrWhiteSpace(RatingSearch)) && (!String.IsNullOrWhiteSpace(GenreSearch)) && String.IsNullOrWhiteSpace(TitleSearch) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(ActorSearch) && String.IsNullOrWhiteSpace(MPAASearch))
                    {
                        MessageFilter = AwardsFilter + " AND " + RatingFilter + " AND " + GenreFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(AwardsSearch)) && (!String.IsNullOrWhiteSpace(MPAASearch)) && (!String.IsNullOrWhiteSpace(GenreSearch)) && String.IsNullOrWhiteSpace(TitleSearch) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(ActorSearch) && String.IsNullOrWhiteSpace(RatingSearch))
                    {
                        MessageFilter = AwardsFilter + " AND " + MPAAFilter + " AND " + GenreFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(RatingSearch)) && (!String.IsNullOrWhiteSpace(MPAASearch)) && (!String.IsNullOrWhiteSpace(GenreSearch)) && String.IsNullOrWhiteSpace(TitleSearch) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(ActorSearch) && String.IsNullOrWhiteSpace(AwardsSearch))
                    {
                        MessageFilter = RatingFilter + " AND " + MPAAFilter + " AND " + GenreFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    break;

                case 4:
                    if ((!String.IsNullOrWhiteSpace(TitleSearch)) && (!String.IsNullOrWhiteSpace(DirectorSearch)) && (!String.IsNullOrWhiteSpace(ActorSearch)) && (!String.IsNullOrWhiteSpace(AwardsSearch)) && String.IsNullOrWhiteSpace(RatingSearch) && String.IsNullOrWhiteSpace(MPAASearch) && String.IsNullOrWhiteSpace(GenreSearch))
                    {
                        MessageFilter = TitleFilter + " AND " + DirectorFilter + " AND " + ActorFilter + " AND " + AwardsFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(TitleSearch)) && (!String.IsNullOrWhiteSpace(DirectorSearch)) && (!String.IsNullOrWhiteSpace(ActorSearch)) && (!String.IsNullOrWhiteSpace(RatingSearch)) && String.IsNullOrWhiteSpace(AwardsSearch) && String.IsNullOrWhiteSpace(MPAASearch) && String.IsNullOrWhiteSpace(GenreSearch))
                    {
                        MessageFilter = TitleFilter + " AND " + DirectorFilter + " AND " + ActorFilter + " AND " + RatingFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(TitleSearch)) && (!String.IsNullOrWhiteSpace(DirectorSearch)) && (!String.IsNullOrWhiteSpace(ActorSearch)) && (!String.IsNullOrWhiteSpace(MPAASearch)) && String.IsNullOrWhiteSpace(AwardsSearch) && String.IsNullOrWhiteSpace(RatingSearch) && String.IsNullOrWhiteSpace(GenreSearch))
                    {
                        MessageFilter = TitleFilter + " AND " + DirectorFilter + " AND " + ActorFilter + " AND " + MPAAFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(TitleSearch)) && (!String.IsNullOrWhiteSpace(DirectorSearch)) && (!String.IsNullOrWhiteSpace(ActorSearch)) && (!String.IsNullOrWhiteSpace(GenreSearch)) && String.IsNullOrWhiteSpace(AwardsSearch) && String.IsNullOrWhiteSpace(RatingSearch) && String.IsNullOrWhiteSpace(MPAASearch))
                    {
                        MessageFilter = TitleFilter + " AND " + DirectorFilter + " AND " + ActorFilter + " AND " + GenreFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(TitleSearch)) && (!String.IsNullOrWhiteSpace(DirectorSearch)) && (!String.IsNullOrWhiteSpace(AwardsSearch)) && (!String.IsNullOrWhiteSpace(RatingSearch)) && String.IsNullOrWhiteSpace(ActorSearch) && String.IsNullOrWhiteSpace(MPAASearch) && String.IsNullOrWhiteSpace(GenreSearch))
                    {
                        MessageFilter = TitleFilter + " AND " + DirectorFilter + " AND " + AwardsFilter + " AND " + RatingFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(TitleSearch)) && (!String.IsNullOrWhiteSpace(DirectorSearch)) && (!String.IsNullOrWhiteSpace(AwardsSearch)) && (!String.IsNullOrWhiteSpace(MPAASearch)) && String.IsNullOrWhiteSpace(ActorSearch) && String.IsNullOrWhiteSpace(RatingSearch) && String.IsNullOrWhiteSpace(GenreSearch))
                    {
                        MessageFilter = TitleFilter + " AND " + DirectorFilter + " AND " + AwardsFilter + " AND " + MPAAFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(TitleSearch)) && (!String.IsNullOrWhiteSpace(DirectorSearch)) && (!String.IsNullOrWhiteSpace(AwardsSearch)) && (!String.IsNullOrWhiteSpace(GenreSearch)) && String.IsNullOrWhiteSpace(ActorSearch) && String.IsNullOrWhiteSpace(RatingSearch) && String.IsNullOrWhiteSpace(MPAASearch))
                    {
                        MessageFilter = TitleFilter + " AND " + DirectorFilter + " AND " + AwardsFilter + " AND " + GenreFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(TitleSearch)) && (!String.IsNullOrWhiteSpace(DirectorSearch)) && (!String.IsNullOrWhiteSpace(RatingSearch)) && (!String.IsNullOrWhiteSpace(MPAASearch)) && String.IsNullOrWhiteSpace(ActorSearch) && String.IsNullOrWhiteSpace(AwardsSearch) && String.IsNullOrWhiteSpace(GenreSearch))
                    {
                        MessageFilter = TitleFilter + " AND " + DirectorFilter + " AND " + RatingFilter + " AND " + MPAAFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(TitleSearch)) && (!String.IsNullOrWhiteSpace(DirectorSearch)) && (!String.IsNullOrWhiteSpace(RatingSearch)) && (!String.IsNullOrWhiteSpace(GenreSearch)) && String.IsNullOrWhiteSpace(ActorSearch) && String.IsNullOrWhiteSpace(AwardsSearch) && String.IsNullOrWhiteSpace(MPAASearch))
                    {
                        MessageFilter = TitleFilter + " AND " + DirectorFilter + " AND " + RatingFilter + " AND " + GenreFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(TitleSearch)) && (!String.IsNullOrWhiteSpace(DirectorSearch)) && (!String.IsNullOrWhiteSpace(MPAASearch)) && (!String.IsNullOrWhiteSpace(GenreSearch)) && String.IsNullOrWhiteSpace(ActorSearch) && String.IsNullOrWhiteSpace(AwardsSearch) && String.IsNullOrWhiteSpace(RatingSearch))
                    {
                        MessageFilter = TitleFilter + " AND " + DirectorFilter + " AND " + MPAAFilter + " AND " + GenreFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(TitleSearch)) && (!String.IsNullOrWhiteSpace(ActorSearch)) && (!String.IsNullOrWhiteSpace(AwardsSearch)) && (!String.IsNullOrWhiteSpace(RatingSearch)) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(MPAASearch) && String.IsNullOrWhiteSpace(GenreSearch))
                    {
                        MessageFilter = TitleFilter + " AND " + ActorFilter + " AND " + AwardsFilter + " AND " + RatingFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(TitleSearch)) && (!String.IsNullOrWhiteSpace(ActorSearch)) && (!String.IsNullOrWhiteSpace(AwardsSearch)) && (!String.IsNullOrWhiteSpace(MPAASearch)) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(RatingSearch) && String.IsNullOrWhiteSpace(GenreSearch))
                    {
                        MessageFilter = TitleFilter + " AND " + ActorFilter + " AND " + AwardsFilter + " AND " + MPAAFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(TitleSearch)) && (!String.IsNullOrWhiteSpace(ActorSearch)) && (!String.IsNullOrWhiteSpace(AwardsSearch)) && (!String.IsNullOrWhiteSpace(GenreSearch)) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(RatingSearch) && String.IsNullOrWhiteSpace(MPAASearch))
                    {
                        MessageFilter = TitleFilter + " AND " + ActorFilter + " AND " + AwardsFilter + " AND " + GenreFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(TitleSearch)) && (!String.IsNullOrWhiteSpace(ActorSearch)) && (!String.IsNullOrWhiteSpace(RatingSearch)) && (!String.IsNullOrWhiteSpace(MPAASearch)) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(AwardsSearch) && String.IsNullOrWhiteSpace(GenreSearch))
                    {
                        MessageFilter = TitleFilter + " AND " + ActorFilter + " AND " + RatingFilter + " AND " + MPAAFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(TitleSearch)) && (!String.IsNullOrWhiteSpace(ActorSearch)) && (!String.IsNullOrWhiteSpace(RatingSearch)) && (!String.IsNullOrWhiteSpace(GenreSearch)) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(AwardsSearch) && String.IsNullOrWhiteSpace(MPAASearch))
                    {
                        MessageFilter = TitleFilter + " AND " + ActorFilter + " AND " + RatingFilter + " AND " + GenreFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(TitleSearch)) && (!String.IsNullOrWhiteSpace(ActorSearch)) && (!String.IsNullOrWhiteSpace(MPAASearch)) && (!String.IsNullOrWhiteSpace(GenreSearch)) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(AwardsSearch) && String.IsNullOrWhiteSpace(RatingSearch))
                    {
                        MessageFilter = TitleFilter + " AND " + ActorFilter + " AND " + MPAAFilter + " AND " + GenreFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(TitleSearch)) && (!String.IsNullOrWhiteSpace(AwardsSearch)) && (!String.IsNullOrWhiteSpace(RatingSearch)) && (!String.IsNullOrWhiteSpace(MPAASearch)) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(AwardsSearch) && String.IsNullOrWhiteSpace(GenreSearch))
                    {
                        MessageFilter = TitleFilter + " AND " + AwardsFilter + " AND " + RatingFilter + " AND " + MPAAFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(TitleSearch)) && (!String.IsNullOrWhiteSpace(AwardsSearch)) && (!String.IsNullOrWhiteSpace(RatingSearch)) && (!String.IsNullOrWhiteSpace(GenreSearch)) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(AwardsSearch) && String.IsNullOrWhiteSpace(MPAASearch))
                    {
                        MessageFilter = TitleFilter + " AND " + AwardsFilter + " AND " + RatingFilter + " AND " + GenreFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(TitleSearch)) && (!String.IsNullOrWhiteSpace(AwardsSearch)) && (!String.IsNullOrWhiteSpace(MPAASearch)) && (!String.IsNullOrWhiteSpace(GenreSearch)) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(AwardsSearch) && String.IsNullOrWhiteSpace(RatingSearch))
                    {
                        MessageFilter = TitleFilter + " AND " + AwardsFilter + " AND " + MPAAFilter + " AND " + GenreFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(TitleSearch)) && (!String.IsNullOrWhiteSpace(RatingSearch)) && (!String.IsNullOrWhiteSpace(MPAASearch)) && (!String.IsNullOrWhiteSpace(GenreSearch)) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(ActorSearch) && String.IsNullOrWhiteSpace(AwardsSearch))
                    {
                        MessageFilter = TitleFilter + " AND " + RatingFilter + " AND " + MPAAFilter + " AND " + GenreFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(DirectorSearch)) && (!String.IsNullOrWhiteSpace(ActorSearch)) && (!String.IsNullOrWhiteSpace(AwardsSearch)) && (!String.IsNullOrWhiteSpace(RatingSearch)) && String.IsNullOrWhiteSpace(TitleSearch) && String.IsNullOrWhiteSpace(MPAASearch) && String.IsNullOrWhiteSpace(GenreSearch))
                    {
                        MessageFilter = DirectorFilter + " AND " + ActorFilter + " AND " + AwardsFilter + " AND " + RatingFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(DirectorSearch)) && (!String.IsNullOrWhiteSpace(ActorSearch)) && (!String.IsNullOrWhiteSpace(AwardsSearch)) && (!String.IsNullOrWhiteSpace(MPAASearch)) && String.IsNullOrWhiteSpace(TitleSearch) && String.IsNullOrWhiteSpace(RatingSearch) && String.IsNullOrWhiteSpace(GenreSearch))
                    {
                        MessageFilter = DirectorFilter + " AND " + ActorFilter + " AND " + AwardsFilter + " AND " + MPAAFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(DirectorSearch)) && (!String.IsNullOrWhiteSpace(ActorSearch)) && (!String.IsNullOrWhiteSpace(AwardsSearch)) && (!String.IsNullOrWhiteSpace(GenreSearch)) && String.IsNullOrWhiteSpace(TitleSearch) && String.IsNullOrWhiteSpace(RatingSearch) && String.IsNullOrWhiteSpace(MPAASearch))
                    {
                        MessageFilter = DirectorFilter + " AND " + ActorFilter + " AND " + AwardsFilter + " AND " + GenreFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(DirectorSearch)) && (!String.IsNullOrWhiteSpace(AwardsSearch)) && (!String.IsNullOrWhiteSpace(RatingSearch)) && (!String.IsNullOrWhiteSpace(MPAASearch)) && String.IsNullOrWhiteSpace(TitleSearch) && String.IsNullOrWhiteSpace(ActorSearch) && String.IsNullOrWhiteSpace(GenreSearch))
                    {
                        MessageFilter = DirectorFilter + " AND " + AwardsFilter + " AND " + RatingFilter + " AND " + MPAAFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(DirectorSearch)) && (!String.IsNullOrWhiteSpace(AwardsSearch)) && (!String.IsNullOrWhiteSpace(RatingSearch)) && (!String.IsNullOrWhiteSpace(GenreSearch)) && String.IsNullOrWhiteSpace(TitleSearch) && String.IsNullOrWhiteSpace(ActorSearch) && String.IsNullOrWhiteSpace(MPAASearch))
                    {
                        MessageFilter = DirectorFilter + " AND " + AwardsFilter + " AND " + RatingFilter + " AND " + GenreFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(DirectorSearch)) && (!String.IsNullOrWhiteSpace(AwardsSearch)) && (!String.IsNullOrWhiteSpace(MPAASearch)) && (!String.IsNullOrWhiteSpace(GenreSearch)) && String.IsNullOrWhiteSpace(TitleSearch) && String.IsNullOrWhiteSpace(ActorSearch) && String.IsNullOrWhiteSpace(RatingSearch))
                    {
                        MessageFilter = DirectorFilter + " AND " + AwardsFilter + " AND " + MPAAFilter + " AND " + GenreFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(DirectorSearch)) && (!String.IsNullOrWhiteSpace(RatingSearch)) && (!String.IsNullOrWhiteSpace(MPAASearch)) && (!String.IsNullOrWhiteSpace(GenreSearch)) && String.IsNullOrWhiteSpace(TitleSearch) && String.IsNullOrWhiteSpace(ActorSearch) && String.IsNullOrWhiteSpace(AwardsSearch))
                    {
                        MessageFilter = DirectorFilter + " AND " + RatingFilter + " AND " + MPAAFilter + " AND " + GenreFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(ActorSearch)) && (!String.IsNullOrWhiteSpace(AwardsSearch)) && (!String.IsNullOrWhiteSpace(RatingSearch)) && (!String.IsNullOrWhiteSpace(MPAASearch)) && String.IsNullOrWhiteSpace(TitleSearch) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(GenreSearch))
                    {
                        MessageFilter = ActorFilter + " AND " + AwardsFilter + " AND " + RatingFilter + " AND " + MPAAFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(ActorSearch)) && (!String.IsNullOrWhiteSpace(AwardsSearch)) && (!String.IsNullOrWhiteSpace(RatingSearch)) && (!String.IsNullOrWhiteSpace(GenreSearch)) && String.IsNullOrWhiteSpace(TitleSearch) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(MPAASearch))
                    {
                        MessageFilter = ActorFilter + " AND " + AwardsFilter + " AND " + RatingFilter + " AND " + GenreFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(ActorSearch)) && (!String.IsNullOrWhiteSpace(RatingSearch)) && (!String.IsNullOrWhiteSpace(MPAASearch)) && (!String.IsNullOrWhiteSpace(GenreSearch)) && String.IsNullOrWhiteSpace(TitleSearch) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(AwardsSearch))
                    {
                        MessageFilter = ActorFilter + " AND " + RatingFilter + " AND " + MPAAFilter + " AND " + GenreFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(AwardsSearch)) && (!String.IsNullOrWhiteSpace(RatingSearch)) && (!String.IsNullOrWhiteSpace(MPAASearch)) && (!String.IsNullOrWhiteSpace(GenreSearch)) && String.IsNullOrWhiteSpace(TitleSearch) && String.IsNullOrWhiteSpace(DirectorSearch) && String.IsNullOrWhiteSpace(ActorSearch))
                    {
                        MessageFilter = AwardsFilter + " AND " + RatingFilter + " AND " + MPAAFilter + " AND " + GenreFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    break;

                case 5: 
                    if ((!String.IsNullOrWhiteSpace(TitleSearch)) && (!String.IsNullOrWhiteSpace(DirectorSearch)) && (!String.IsNullOrWhiteSpace(ActorSearch)) && (!String.IsNullOrWhiteSpace(AwardsSearch)) && (!String.IsNullOrWhiteSpace(RatingSearch)))
                    {
                        MessageFilter = TitleFilter + " AND " + DirectorFilter + " AND " + ActorFilter + " AND " + AwardsFilter + " AND " + RatingFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(TitleSearch)) && (!String.IsNullOrWhiteSpace(DirectorSearch)) && (!String.IsNullOrWhiteSpace(ActorSearch)) && (!String.IsNullOrWhiteSpace(AwardsSearch)) && (!String.IsNullOrWhiteSpace(GenreSearch)))
                    {
                        MessageFilter = TitleFilter + " AND " + DirectorFilter + " AND " + ActorFilter + " AND " + AwardsFilter + " AND " + GenreFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(TitleSearch)) && (!String.IsNullOrWhiteSpace(DirectorSearch)) && (!String.IsNullOrWhiteSpace(ActorSearch)) && (!String.IsNullOrWhiteSpace(MPAASearch)) && (!String.IsNullOrWhiteSpace(GenreSearch)))
                    {
                        MessageFilter = TitleFilter + " AND " + DirectorFilter + " AND " + ActorFilter + " AND " + MPAAFilter + " AND " + GenreFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(TitleSearch)) && (!String.IsNullOrWhiteSpace(DirectorSearch)) && (!String.IsNullOrWhiteSpace(RatingSearch)) && (!String.IsNullOrWhiteSpace(MPAASearch)) && (!String.IsNullOrWhiteSpace(GenreSearch)))
                    {
                        MessageFilter = TitleFilter + " AND " + DirectorFilter + " AND " + RatingFilter + " AND " + MPAAFilter + " AND " + GenreFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(TitleSearch)) && (!String.IsNullOrWhiteSpace(AwardsSearch)) && (!String.IsNullOrWhiteSpace(RatingSearch)) && (!String.IsNullOrWhiteSpace(MPAASearch)) && (!String.IsNullOrWhiteSpace(GenreSearch)))
                    {
                        MessageFilter = TitleFilter + " AND " + AwardsFilter + " AND " + RatingFilter + " AND " + MPAAFilter + " AND " + GenreFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(ActorSearch)) && (!String.IsNullOrWhiteSpace(AwardsSearch)) && (!String.IsNullOrWhiteSpace(RatingSearch)) && (!String.IsNullOrWhiteSpace(MPAASearch)) && (!String.IsNullOrWhiteSpace(GenreSearch)))
                    {
                        MessageFilter = ActorFilter + " AND " + AwardsFilter + " AND " + RatingFilter + " AND " + MPAAFilter + " AND " + GenreFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    break;

                case 6: 
                    if ((!String.IsNullOrWhiteSpace(TitleSearch)) && (!String.IsNullOrWhiteSpace(DirectorSearch)) && (!String.IsNullOrWhiteSpace(ActorSearch)) && (!String.IsNullOrWhiteSpace(AwardsSearch)) && (!String.IsNullOrWhiteSpace(RatingSearch)) && (!String.IsNullOrWhiteSpace(MPAASearch)))
                    {
                        MessageFilter = TitleFilter + " AND " + DirectorFilter + " AND " + ActorFilter + " AND " + AwardsFilter + " AND " + RatingFilter + " AND " + MPAAFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;

                    }
                    if ((!String.IsNullOrWhiteSpace(TitleSearch)) && (!String.IsNullOrWhiteSpace(DirectorSearch)) && (!String.IsNullOrWhiteSpace(ActorSearch)) && (!String.IsNullOrWhiteSpace(AwardsSearch)) && (!String.IsNullOrWhiteSpace(RatingSearch)) && (!String.IsNullOrWhiteSpace(GenreSearch)))
                    {
                        MessageFilter = TitleFilter + " AND " + DirectorFilter + " AND " + ActorFilter + " AND " + AwardsFilter + " AND " + RatingFilter + " AND " + GenreFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(TitleSearch)) && (!String.IsNullOrWhiteSpace(DirectorSearch)) && (!String.IsNullOrWhiteSpace(ActorSearch)) && (!String.IsNullOrWhiteSpace(AwardsSearch)) && (!String.IsNullOrWhiteSpace(MPAASearch)) && (!String.IsNullOrWhiteSpace(GenreSearch)))
                    {
                        MessageFilter = TitleFilter + " AND " + DirectorFilter + " AND " + ActorFilter + " AND " + AwardsFilter + " AND " + MPAAFilter + " AND " + GenreFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(TitleSearch)) && (!String.IsNullOrWhiteSpace(DirectorSearch)) && (!String.IsNullOrWhiteSpace(ActorSearch)) && (!String.IsNullOrWhiteSpace(RatingSearch)) && (!String.IsNullOrWhiteSpace(MPAASearch)) && (!String.IsNullOrWhiteSpace(GenreSearch)))
                    {
                        MessageFilter = TitleFilter + " AND " + DirectorFilter + " AND " + ActorFilter + " AND " + RatingFilter + " AND " + MPAAFilter + " AND " + GenreFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(TitleSearch)) && (!String.IsNullOrWhiteSpace(DirectorSearch)) && (!String.IsNullOrWhiteSpace(AwardsSearch)) && (!String.IsNullOrWhiteSpace(RatingSearch)) && (!String.IsNullOrWhiteSpace(MPAASearch)) && (!String.IsNullOrWhiteSpace(GenreSearch)))
                    {
                        MessageFilter = TitleFilter + " AND " + DirectorFilter + " AND " + AwardsFilter + " AND " + RatingFilter + " AND " + MPAAFilter + " AND " + GenreFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(TitleSearch)) && (!String.IsNullOrWhiteSpace(ActorSearch)) && (!String.IsNullOrWhiteSpace(AwardsSearch)) && (!String.IsNullOrWhiteSpace(RatingSearch)) && (!String.IsNullOrWhiteSpace(MPAASearch)) && (!String.IsNullOrWhiteSpace(GenreSearch)))
                    {
                        MessageFilter = TitleFilter + " AND " + ActorFilter + " AND " + AwardsFilter + " AND " + RatingFilter + " AND " + MPAAFilter + " AND " + GenreFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    if ((!String.IsNullOrWhiteSpace(DirectorSearch)) && (!String.IsNullOrWhiteSpace(ActorSearch)) && (!String.IsNullOrWhiteSpace(AwardsSearch)) && (!String.IsNullOrWhiteSpace(RatingSearch)) && (!String.IsNullOrWhiteSpace(MPAASearch)) && (!String.IsNullOrWhiteSpace(GenreSearch)))
                    {
                        MessageFilter = DirectorFilter + " AND " + ActorFilter + " AND " + AwardsFilter + " AND " + RatingFilter + " AND " + MPAAFilter + " AND " + GenreFilter;
                        SearchTable.DefaultView.RowFilter = MessageFilter;
                        SearchGrid.DataSource = SearchTable;
                    }
                    break;

                case 7:
                    MessageFilter = TitleFilter + " AND " + DirectorFilter + " AND " + ActorFilter + " AND " + AwardsFilter + " AND " + RatingFilter + " AND " + MPAAFilter + " AND " + GenreFilter;
                    SearchTable.DefaultView.RowFilter = MessageFilter;
                    SearchGrid.DataSource = SearchTable;
                    break;
                default:
                    string BlankMessage = "Nothing was entered" + Environment.NewLine + "try your search again";
                    MessageBox.Show(BlankMessage,"Blank Search Performed",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                    Blank = true;
                    break;
            }
            if (!Blank)
            {
                foreach (DataGridViewColumn col in SearchGrid.Columns)
                {
                    col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    col.HeaderCell.Style.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
                    col.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }
                DisplayAllForm.Show();
            }
            SearchControlForm.Close();
        }


        public void Set_Form(Form SearchForm)
        {
            SearchControlForm = SearchForm;
        }

        private void Set_ActorRadioButton(RadioButton rb)
        {
            ActRadioButton = rb;
            switch (ActRadioButton.Name)
            {
                case "ActorRadioButton":
                    ActorSelected = true;
                    TotalSearchCount++;
                    break;
            }
        }

        private void Set_DirectorRadioButton(RadioButton rb)
        {
            DirectRadioButton = rb;
            switch (DirectRadioButton.Name)
            {
                case "DirectorRadioButton":
                    DirectorSelected = true;
                    TotalSearchCount++;
                    break;
            }
        }

        private void Set_TitleRadioButton(RadioButton rb)
        {
            NameRadioButton = rb;
            switch (NameRadioButton.Name)
            {
                case "TitleRadioButton":
                    TitleSelected = true;
                    TotalSearchCount++;
                    break;
            }
        }

        private void Set_RatingRadioButton(RadioButton rb)
        {
            RatingRadioButton = rb;
            switch (RatingRadioButton.Name)
            {
                case "OneStarRadioButton":
                    RatingSearch = "*";
                    TotalSearchCount++;
                    break;
                case "OneHalfStarRadioButton":
                    RatingSearch = "*½";
                    TotalSearchCount++;
                    break;
                case "TwoStarRadioButton":
                    RatingSearch = "**";
                    TotalSearchCount++;
                    break;
                case "TwoHalfStarRadioButton":
                    RatingSearch = "**½";
                    TotalSearchCount++;
                    break;
                case "ThreeStarRadioButton":
                    RatingSearch = "***";
                    TotalSearchCount++;
                    break;
                case "ThreeHalfStarRadioButton":
                    RatingSearch = "***½";
                    TotalSearchCount++;
                    break;
                case "FourStarRadioButton":
                    RatingSearch = "****";
                    TotalSearchCount++;
                    break;
                case "FourHalfStarRadioButton":
                    RatingSearch = "****½";
                    TotalSearchCount++;
                    break;
                case "FiveStarRadioButton":
                    RatingSearch = "*****";
                    TotalSearchCount++;
                    break;
            }
        }

        private void Set_AwardsRadioButton(RadioButton rb)
        {
            AwardsRadioButton = rb;
            switch (AwardsRadioButton.Name)
            {
                case "TonyRadioButton":
                    AwardsSearch = "Tony";
                    TotalSearchCount++;
                    break;
                case "EmmyRadioButton":
                    AwardsSearch = "Emmy";
                    TotalSearchCount++;
                    break;
                case "OscarRadioButton":
                    AwardsSearch = "Oscar";
                    TotalSearchCount++;
                    break;
                case "BestSupportingRadioButton":
                    AwardsSearch = "Best Supporting";
                    TotalSearchCount++;
                    break;
                case "BestSpecialRadioButton":
                    AwardsSearch = "Best Special";
                    TotalSearchCount++;
                    break;
                case "BestSoundRadioButton":
                    AwardsSearch = "Best Sound";
                    TotalSearchCount++;
                    break;
                case "BestSongRadioButton":
                    AwardsSearch = "Best Song";
                    TotalSearchCount++;
                    break;
                case "BestScoreRadioButton":
                    AwardsSearch = "Best Score";
                    break;
                case "BestProductionRadioButton":
                    AwardsSearch = "Best Production";
                    TotalSearchCount++;
                    break;
                case "BestPictureRadioButton":
                    AwardsSearch = "Best Picture";
                    TotalSearchCount++;
                    break;
                case "BestMakeupRadioButton":
                    AwardsSearch = "Best Makeup";
                    TotalSearchCount++;
                    break;
                case "BestForeignRadioButton":
                    AwardsSearch = "Best Foreign";
                    TotalSearchCount++;
                    break;
                case "BestFilmRadioButton":
                    AwardsSearch = "Best Film";
                    TotalSearchCount++;
                    break;
                case "BestDirectorRadioButton":
                    AwardsSearch = "Best Director";
                    TotalSearchCount++;
                    break;
                case "BestCostumeRadioButton":
                    AwardsSearch = "Best Costume";
                    TotalSearchCount++;
                    break;
                case "BestCinematographyRadioButton":
                    AwardsSearch = "Best Cinematography";
                    TotalSearchCount++;
                    break;
                case "BestAnimatedRadioButton":
                    AwardsSearch = "Best Animated";
                    TotalSearchCount++;
                    break;
                case "BestOriginalRadioButton":
                    AwardsSearch = "Best Original";
                    TotalSearchCount++;
                    break;
                case "BestAdaptedRadioButton":
                    AwardsSearch = "Best Adapted";
                    TotalSearchCount++;
                    break;
                case "BestActorRadioButton":
                    AwardsSearch = "Best Act*";
                    TotalSearchCount++;
                    break;
            }
        }

        private void Set_GenreRadioButton(RadioButton rb)
        {
            GenreRadioButton = rb;
            switch (GenreRadioButton.Name)
            {
                case "ActionRadioButton":
                    GenreSearch = "Action";
                    TotalSearchCount++;
                    break;
                case "AdventureRadioButton":
                    GenreSearch = "Adventure";
                    TotalSearchCount++;
                    break;
                case "BiographicalRadioButton":
                    GenreSearch = "Biographical";
                    TotalSearchCount++;
                    break;
                case "BiographyRadioButton":
                    GenreSearch = "Biography";
                    TotalSearchCount++;
                    break;
                case "ComedyRadioButton":
                    GenreSearch = "Comedy";
                    TotalSearchCount++;
                    break;
                case "CrimeRadioButton":
                    GenreSearch = "Crime";
                    TotalSearchCount++;
                    break;
                case "DocumentaryRadioButton":
                    GenreSearch = "Documentary";
                    TotalSearchCount++;
                    break;
                case "DramaRadioButton":
                    GenreSearch = "Drama";
                    TotalSearchCount++;
                    break;
                case "FantasyRadioButton":
                    GenreSearch = "Fantasy";
                    TotalSearchCount++;
                    break;
                case "HistoricalRadioButton":
                    GenreSearch = "Historical";
                    TotalSearchCount++;
                    break;
                case "HistoricalFictionRadioButton":
                    GenreSearch = "Historical fiction";
                    TotalSearchCount++;
                    break;
                case "HorrorRadioButton":
                    GenreSearch = "Horror";
                    TotalSearchCount++;
                    break;
                case "MagicalRadioButton":
                    GenreSearch = "Magical realism";
                    TotalSearchCount++;
                    break;
                case "MysteryRadioButton":
                    GenreSearch = "Mystery";
                    TotalSearchCount++;
                    break;
                case "ParanoidRadioButton":
                    GenreSearch = "Paranoid";
                    TotalSearchCount++;
                    break;
                case "PhilosophicalRadioButton":
                    GenreSearch = "Philosophical";
                    TotalSearchCount++;
                    break;
                case "PoliticalRadioButton":
                    GenreSearch = "Political";
                    TotalSearchCount++;
                    break;
                case "RomanceRadioButton":
                    GenreSearch = "Romance";
                    TotalSearchCount++;
                    break;
                case "SatireRadioButton":
                    GenreSearch = "Satire";
                    TotalSearchCount++;
                    break;
                case "ScienceFictionRadioButton":
                    GenreSearch = "Science fiction";
                    TotalSearchCount++;
                    break;
                case "SocialRadioButton":
                    GenreSearch = "Social";
                    TotalSearchCount++;
                    break;
                case "SpeculativeRadioButton":
                    GenreSearch = "Speculative";
                    TotalSearchCount++;
                    break;
                case "ThrillerRadioButton":
                    GenreSearch = "Thriller";
                    TotalSearchCount++;
                    break;
                case "UrbanRadioButton":
                    GenreSearch = "Urban";
                    TotalSearchCount++;
                    break;
                case "WesternRadioButton":
                    GenreSearch = "Western";
                    TotalSearchCount++;
                    break;
            }
        }

        private void Set_MPAARadioButton(RadioButton rb)
        {
            MPAARadioButton = rb;
            switch (MPAARadioButton.Name)
            {
                case "TVMARadioButton":
                    MPAASearch = "TV-MA";
                    TotalSearchCount++;
                    break;
                case "TV14RadioButton":
                    MPAASearch = "TV-14";
                    TotalSearchCount++;
                    break;
                case "TVPGRadioButton":
                    MPAASearch = "TV-PG";
                    TotalSearchCount++;
                    break;
                case "TVGRadioButton":
                    MPAASearch = "TV-G";
                    TotalSearchCount++;
                    break;
                case "TVY7RadioButton":
                    MPAASearch = "TV-Y7";
                    TotalSearchCount++;
                    break;
                case "TVYRadioButton":
                    MPAASearch = "TV-Y";
                    TotalSearchCount++;
                    break;
                case "URRadioButton":
                    MPAASearch = "UR";
                    TotalSearchCount++;
                    break;
                case "NRRadioButton":
                    MPAASearch = "NR";
                    TotalSearchCount++;
                    break;
                case "NC17RadioButton":
                    MPAASearch = "NC-17";
                    TotalSearchCount++;
                    break;
                case "RRadioButton":
                    MPAASearch = "R";
                    TotalSearchCount++;
                    break;
                case "PG13RadioButton":
                    MPAASearch = "PG-13";
                    TotalSearchCount++;
                    break;
                case "PGRadioButton":
                    MPAASearch = "PG";
                    TotalSearchCount++;
                    break;
                case "GRadioButton":
                    MPAASearch = "G";
                    TotalSearchCount++;
                    break;
            }
        }

        private void RatingGroupRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton Rating = sender as RadioButton;
            if (Rating.Checked)
            {
                Set_RatingRadioButton(Rating);
            }
        }

        private void AwardsGroupRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton Awards = sender as RadioButton;
            if (Awards.Checked)
            {
                Set_AwardsRadioButton(Awards);
            }
        }

        private void GenreGroupRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton Genre = sender as RadioButton;
            if (Genre.Checked)
            {
                Set_GenreRadioButton(Genre);
            }
        }

        private void MPAAGroupRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton MPAA = sender as RadioButton;
            if (MPAA.Checked)
            {
                Set_MPAARadioButton(MPAA);
            }
        }

        private void TitleRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton Title = sender as RadioButton;
            if (Title.Checked)
            {
                Set_TitleRadioButton(Title);
            }
        }

        private void DirectorRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton Director = sender as RadioButton;
            if (Director.Checked)
            {
                Set_DirectorRadioButton(Director);
            }
        }

        private void ActorRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton Actor = sender as RadioButton;
            if (Actor.Checked)
            {
                Set_ActorRadioButton(Actor);
            }
        }

        public void ClearSelections()
        {
            GenreRadioButton.Checked = false;
            MPAARadioButton.Checked = false;
            RatingRadioButton.Checked = false;
            AwardsRadioButton.Checked = false;
            TitleRadioButton.Checked = false;
            DirectorRadioButton.Checked = false;
            ActorRadioButton.Checked = false;
            TotalSearchCount = 0;
            TitleTextBox.Text = null;
            DirectorTextBox.Text = null;
            ActorTextBox.Text = null;
            GenreSearch = null;
            MPAASearch = null;
            RatingSearch = null;
            AwardsSearch = null;
            TitleSearch = null;
            DirectorSearch = null;
            ActorSearch = null;
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            ClearSelections();
        }
    }
}
