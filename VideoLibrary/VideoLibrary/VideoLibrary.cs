using System;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;
using System.Globalization;
using System.Threading;

namespace VideoLibrary
{
    public partial class VideoLibrary : Form
    {

        //Form SearchForm = new Form();

        public VideoLibrary()
        {
            InitializeComponent();
            Get_PictureBox();
        }

        private static SaveFileDialog SaveNewFile => new SaveFileDialog
        {
            Filter = "All Excel Files|*.xl*;*.xlsx|Text Documents|*.txt|All Files|*.*",
            Title = "Save new File As",
            OverwritePrompt = true,
            InitialDirectory = @"C:\Users\v-mase\Desktop\C#\files\",
            CreatePrompt = true,
            RestoreDirectory = true
        };

        public static OpenFileDialog OpenNewFile()
        {
            return new OpenFileDialog()
            {
                Filter = "All Excel Files|*.xl*;*.xlsx|Text Documents|*.txt|All Files|*.*",
                Title = "Open File",
                InitialDirectory = @"C:\Users\v-mase\Desktop\C#\Jerry"
            };
        }

        public static string CheckOpenFile(OpenFileDialog openfile)
        {

            string filePath = openfile.FileName;
            if (!(File.Exists(filePath)))
            {
                MessageBox.Show("File doesn't exist!!!!!!!!", "File Error!");
                DialogResult res = MessageBox.Show("Would you like to select another file?", "Select File Confirmation", MessageBoxButtons.YesNo);
                if (res == DialogResult.Yes)
                {
                    openfile.ShowDialog();
                    filePath = openfile.FileName;
                }
                else
                {
                    Environment.Exit(1);        // exits the program
                }
            }

            return filePath;
        }

        private async void Get_PictureBox()
        {

            String[] Images = new String[]
            {
                @"C:\Users\v-mase\Desktop\C#\Jerry\images\theater\1.gif",
                @"C:\Users\v-mase\Desktop\C#\Jerry\images\theater\2.gif",
                @"C:\Users\v-mase\Desktop\C#\Jerry\images\theater\3.gif",
                @"C:\Users\v-mase\Desktop\C#\Jerry\images\theater\4.gif",
                @"C:\Users\v-mase\Desktop\C#\Jerry\images\theater\5.gif",
                @"C:\Users\v-mase\Desktop\C#\Jerry\images\theater\6.gif",
                @"C:\Users\v-mase\Desktop\C#\Jerry\images\theater\7.gif",
                @"C:\Users\v-mase\Desktop\C#\Jerry\images\theater\8.gif",
                @"C:\Users\v-mase\Desktop\C#\Jerry\images\theater\9.gif",
                @"C:\Users\v-mase\Desktop\C#\Jerry\images\theater\10.gif",
                @"C:\Users\v-mase\Desktop\C#\Jerry\images\theater\11.gif",
                @"C:\Users\v-mase\Desktop\C#\Jerry\images\theater\12.gif"
            };


            for (int i = 0; i < Images.Length; i++)
            {
                pictureBox1.ImageLocation = Images[i];
                pictureBox1.Size = new System.Drawing.Size(450, 250);
                pictureBox1.Location = new System.Drawing.Point(30, 20);
                pictureBox1.BackColor = System.Drawing.Color.Transparent;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox1.Show();
                await System.Threading.Tasks.Task.Delay(15000);
            }
        }


        public static string Increment(string s)
        {
            // next case - last char is less than 'z': simply increment last char
            char lastChar = s[s.Length - 1];
            string fragment = s.Substring(0, s.Length - 1);
            if (lastChar < 'z')
            {
                ++lastChar;
                return fragment + lastChar;
            }
            // next case - last char is 'z': roll over and increment preceding string
            return Increment(fragment) + 'a';
        }

        public static void ReleaseExcelObject(string filePath, Excel.Application objExcel, Excel.Workbook objBook, Excel._Worksheet objSheet)
        {
            objBook.Close(true, filePath, null);
            Marshal.ReleaseComObject(objSheet);
            Marshal.ReleaseComObject(objBook);
            Marshal.ReleaseComObject(objExcel);
        }

        public static void SetupExcel(string filePath, out Excel.Application objExcel, out Excel.Workbook objBook, out Excel._Worksheet objSheet, out Excel.Range rowRange)
        {
            objExcel = new Excel.Application
            {
                DisplayAlerts = false,
                Visible = false
            };
            objBook = objExcel.Workbooks.Add(filePath);
            objSheet = (Excel.Worksheet)objExcel.ActiveSheet;
            rowRange = objSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
            Excel.Range objRange = objSheet.get_Range("A1", rowRange);
        }

        public string FindExcelData(string filePath, string SearchData)
        {

            Excel.Application objExcel;
            Excel.Workbook objBook;
            Excel._Worksheet objSheet;
            Excel.Range rowRange;
            SetupExcel(filePath, out objExcel, out objBook, out objSheet, out rowRange);
            Excel.Range objRange = objSheet.get_Range("A1", rowRange);
            Excel.Range currentFind = null;
            Excel.Range firstFind = null;
            string location = "";
            currentFind = objRange.Find(SearchData, Type.Missing, Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, false, Type.Missing, Type.Missing);

            while (currentFind != null)
            {
                if (firstFind == null)
                {
                    firstFind = currentFind;
                }
                else if (currentFind.get_Address(Excel.XlReferenceStyle.xlA1) == firstFind.get_Address(Excel.XlReferenceStyle.xlA1))
                {
                    break;
                }
                location = (firstFind.Row).ToString();
            }

            ReleaseExcelObject(filePath, objExcel, objBook, objSheet);
            return location;
        }

        private void RemoveButton_Click(object sender, EventArgs e, Form createForm, string SearchData, string location, string column, string filePath)
        {

            Excel.Application objExcel;
            Excel.Workbook objBook;
            Excel._Worksheet objSheet;
            Excel.Range rowRange;
            SetupExcel(filePath, out objExcel, out objBook, out objSheet, out rowRange);
            string recipeMessage = "Delete selected Title Name: " + SearchData + Environment.NewLine;
            if (MessageBox.Show(recipeMessage, "New Title Search", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                objSheet.Cells[location, column].EntireRow.Delete(Excel.XlDirection.xlToLeft);
                recipeMessage = "Title Name " + SearchData + " has been removed!" + Environment.NewLine;
                MessageBox.Show(recipeMessage, "Title Deletion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                objBook.SaveAs(filePath);
            }
            else
            {
                recipeMessage = "Title Name " + SearchData + " was not removed!" + Environment.NewLine;
                MessageBox.Show(recipeMessage, "Title Deletion", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            ReleaseExcelObject(filePath, objExcel, objBook, objSheet);
            createForm.Close();
        }

        private void UpdateButton_Click(object sender, EventArgs e, Form createForm, VideoLibraryControl control, string SearchData, string location, string column, string filePath)
        {

            Excel.Application objExcel;
            Excel.Workbook objBook;
            Excel._Worksheet objSheet;
            Excel.Range rowRange;
            SetupExcel(filePath, out objExcel, out objBook, out objSheet, out rowRange);

            string recipeMessage = "Update selected Title: " + SearchData + Environment.NewLine;
            if (MessageBox.Show(recipeMessage, "Update Title Search", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var tBox = "textBox";
                TextBox tb = new TextBox();

                for (int count = 1; count < 13; count++)
                {
                    tBox = "textBox" + count;
                    tb = (TextBox)control.Controls[tBox];
                    rowRange = objSheet.Cells[location, column];
                    objSheet.Cells[location, column] = tb.Text;
                    column = Increment(column);
                }

                recipeMessage = "Title Name " + SearchData + " has been updated!" + Environment.NewLine;
                MessageBox.Show(recipeMessage, "Title Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                objBook.SaveAs(filePath);
            }
            else
            {
                recipeMessage = "Title Name " + SearchData + " was not updated!" + Environment.NewLine;
                MessageBox.Show(recipeMessage, "Title Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            ReleaseExcelObject(filePath, objExcel, objBook, objSheet);
            createForm.Close();
        }

        private void DisplayAll_Click(object sender, EventArgs e)
        {

            string filePath;
            OpenFileDialog openfile = OpenNewFile();
            openfile.ShowDialog();
            filePath = CheckOpenFile(openfile);
            while (!(File.Exists(filePath)))
            {
                filePath = CheckOpenFile(openfile);
            }

            Excel.Application objExcel;
            Excel.Workbook objBook;
            Excel._Worksheet objSheet;
            Excel.Range rowRange;
            SetupExcel(filePath, out objExcel, out objBook, out objSheet, out rowRange);

            Form DisplayAllForm = new Form()
            {
                Text = "Display All",
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

            DataGridView temp = new DataGridView();
            temp.Location = new System.Drawing.Point(11, 33);
            temp.Name = "dataGridView1";
            temp.Size = new System.Drawing.Size(1000, 600);
            temp.TabIndex = 0;
            temp.ColumnCount = 12;

            temp.Columns[0].Name = "Title";
            temp.Columns[1].Name = "Year or Release";
            temp.Columns[2].Name = "Rating(Quality)";
            temp.Columns[3].Name = "Length(Hours/Minutes)";
            temp.Columns[4].Name = "MPAA Rating";
            temp.Columns[5].Name = "Genre";
            temp.Columns[6].Name = "Actor(s)/Actress(es)";
            temp.Columns[7].Name = "Director(s)";
            temp.Columns[8].Name = "Awards";
            temp.Columns[9].Name = "Comments";
            temp.Columns[10].Name = "Location(Note Book and Disc)[2]";
            temp.Columns[11].Name = "IMDB";

            NewSubControl.Controls.Add(temp);
            DisplayAllForm.Controls.Add(NewSubControl);

            string[] excelRow = new string[12];
            string column = "A";
            int count = 2;
            int x = 0;
            int rows;
            rows = objSheet.UsedRange.Rows.Count;

            while (x < rows - 1)
            {
                for (int i = 0; i < 12; i++)
                {
                    if (column == "D")
                    {
                        var value = (objSheet.Cells[count, column]).Text;
                        excelRow[i] = value;
                        column = Increment(column);
                    }
                    else
                    {
                        var value = (objSheet.Cells[count, column]).Value2;
                        excelRow[i] = Convert.ToString(value);
                        column = Increment(column);
                    }
                }
                temp.Rows.Add(excelRow);
                x++;
                count++;
                column = "A";
            }

            foreach (DataGridViewColumn col in temp.Columns)
            {
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.HeaderCell.Style.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
                col.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            DisplayAllForm.Show();
            ReleaseExcelObject(filePath, objExcel, objBook, objSheet);
        }

        private void Search_Click(object sender, EventArgs e)
        {

            Form SearchForm = new Form()
            {
                Text = "Search Library Item",
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FormBorderStyle = FormBorderStyle.Fixed3D,
                AutoScroll = true,
                AllowDrop = true,           // allows user to drag and drop
                Enabled = true             // changes if user can enter data (true = yes, false = no)
            };


            SearchControl SearchControl = new SearchControl()
            {
                ForeColor = System.Drawing.Color.Black,
                BackColor = System.Drawing.Color.Lavender
            };

            SearchForm.Controls.Add(SearchControl);
            SearchForm.Show();
            SearchControl.ClearSelections();
            SearchControl.Set_Form(SearchForm);
        }

        private void Update_Click(object sender, EventArgs e)
        {

            string filePath;
            OpenFileDialog openfile = OpenNewFile();
            openfile.ShowDialog();
            filePath = CheckOpenFile(openfile);
            while (!(File.Exists(filePath)))
            {
                filePath = CheckOpenFile(openfile);
            }

            string str;
            string location = "";
            string LibraryMessage = "";
            Excel.Application objExcel;
            Excel.Workbook objBook;
            Excel._Worksheet objSheet;
            Excel.Range rowRange;
            SetupExcel(filePath, out objExcel, out objBook, out objSheet, out rowRange);

            Form UpdateForm = new Form()
            {
                Text = "Update Library Item",
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FormBorderStyle = FormBorderStyle.Fixed3D,
                AutoScroll = true,
                AllowDrop = true,           // allows user to drag and drop
                Enabled = true             // changes if user can enter data (true = yes, false = no)
            };

            Button btn = new Button()
            {
                FlatStyle = FlatStyle.Popup,
                Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                Location = new System.Drawing.Point(335, 640),
                Name = "Update",
                Size = new System.Drawing.Size(116, 56),
                TabIndex = 54,
                Text = "Update",
                UseVisualStyleBackColor = true
            };
            UpdateForm.Controls.Add(btn);

            string SearchData = Interaction.InputBox("Enter Title To Find", "Search Title", "");

            while (string.IsNullOrEmpty(SearchData) || string.IsNullOrWhiteSpace(SearchData))
            {
                LibraryMessage = "You entered blank Title!" + Environment.NewLine + "      Would you like to try again ?";
                if (MessageBox.Show(LibraryMessage, "New Title Search", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    SearchData = Interaction.InputBox("Enter Title To Find", "Locate Title", "");
                }
                else
                {
                    return;                    // Exits out current function
                }
            }

            LibraryMessage = "You selected Title: " + SearchData + Environment.NewLine;
            MessageBox.Show(LibraryMessage, "New Title Search", MessageBoxButtons.OK, MessageBoxIcon.Information);

            location = FindExcelData(filePath, SearchData);
            while (location == "")
            {
                LibraryMessage = "\t" + "Title Name " + SearchData + " not found!" + Environment.NewLine + "Do you want to search for a different Title?";
                if (MessageBox.Show(LibraryMessage, "New Title Search", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    SearchData = Interaction.InputBox("Enter Title To Find", "Locate Title", "");
                    location = FindExcelData(filePath, SearchData);
                }
                else
                {
                    return;                    // Exits out current function
                }
            }

            string column = "A";
            VideoLibraryControl vlc = new VideoLibraryControl()
            {
                ForeColor = System.Drawing.Color.Black,
                BackColor = System.Drawing.Color.Lavender
            };
            UpdateForm.Controls.Add(vlc);
            UpdateForm.Show();

            var tBox = "textBox";
            TextBox tb = new TextBox();
            for (int count = 1; count < 13; count++)
            {
                tBox = "textBox" + count;
                tb = (TextBox)vlc.Controls[tBox];
                rowRange = objSheet.Cells[location, column];
                var str1 = rowRange.Value;

                if (str1 == null)
                {
                    str = " ";
                    tb.Text = str;
                }
                else
                {
                    if (column == "D")
                    {
                        DateTime dt = new DateTime();
                        DateTime.TryParse(rowRange.Text, out dt);
                        tb.Text = String.Format("{0:hh:mm}", dt);
                    }
                    else
                    {
                        str = str1.ToString();
                        tb.Text = str;
                    }
                }
                tb.ReadOnly = false;
                column = Increment(column);
            }
            column = "A";
            btn.Click += (sender2, e2) => UpdateButton_Click(sender2, e2, UpdateForm, vlc, SearchData, location, column, filePath);
        }

        private void Remove_Click(object sender, EventArgs e)
        {

            string filePath;
            OpenFileDialog openfile = OpenNewFile();
            openfile.ShowDialog();
            filePath = CheckOpenFile(openfile);
            while (!(File.Exists(filePath)))
            {
                filePath = CheckOpenFile(openfile);
            }

            string str;
            string location = "";
            string LibraryMessage = "";
            Excel.Application objExcel;
            Excel.Workbook objBook;
            Excel._Worksheet objSheet;
            Excel.Range rowRange;
            SetupExcel(filePath, out objExcel, out objBook, out objSheet, out rowRange);

            Form RemoveForm = new Form()
            {
                Text = "Remove Library Item",
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FormBorderStyle = FormBorderStyle.Fixed3D,
                AutoScroll = true,
                AllowDrop = true,           // allows user to drag and drop
                Enabled = true             // changes if user can enter data (true = yes, false = no)
            };

            Button btn = new Button()
            {
                FlatStyle = FlatStyle.Popup,
                Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                Location = new System.Drawing.Point(335, 640),
                Name = "Remove",
                Size = new System.Drawing.Size(116, 56),
                TabIndex = 54,
                Text = "Remove",
                UseVisualStyleBackColor = true
            };
            RemoveForm.Controls.Add(btn);

            string SearchData = Interaction.InputBox("Enter Title To Find", "Search Title", "");

            while (string.IsNullOrEmpty(SearchData) || string.IsNullOrWhiteSpace(SearchData))
            {
                LibraryMessage = "You entered blank Title!" + Environment.NewLine + "      Would you like to try again ?";
                if (MessageBox.Show(LibraryMessage, "New Title Search", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    SearchData = Interaction.InputBox("Enter Title To Find", "Locate Title", "");
                }
                else
                {
                    return;                    // Exits out current function
                }
            }

            LibraryMessage = "You selected Title: " + SearchData + Environment.NewLine;
            MessageBox.Show(LibraryMessage, "New Title Search", MessageBoxButtons.OK, MessageBoxIcon.Information);

            location = FindExcelData(filePath, SearchData);
            while (location == "")
            {
                LibraryMessage = "\t" + "Title Name " + SearchData + " not found!" + Environment.NewLine + "Do you want to search for a different Title?";
                if (MessageBox.Show(LibraryMessage, "New Title Search", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    SearchData = Interaction.InputBox("Enter Title To Find", "Locate Title", "");
                    location = FindExcelData(filePath, SearchData);
                }
                else
                {
                    return;                    // Exits out current function
                }
            }

            string column = "A";
            VideoLibraryControl vlc = new VideoLibraryControl()
            {
                ForeColor = System.Drawing.Color.Black,
                BackColor = System.Drawing.Color.Lavender
            };
            RemoveForm.Controls.Add(vlc);
            RemoveForm.Show();

            var tBox = "textBox";
            TextBox tb = new TextBox();
            for (int count = 1; count < 13; count++)
            {
                tBox = "textBox" + count;
                tb = (TextBox)vlc.Controls[tBox];
                rowRange = objSheet.Cells[location, column];
                var str1 = rowRange.Value;

                if (str1 == null)
                {
                    str = " ";
                    tb.Text = str;
                }
                else
                {
                    if (column == "D")
                    {
                        DateTime dt = new DateTime();
                        DateTime.TryParse(rowRange.Text, out dt);
                        tb.Text = String.Format("{0:hh:mm}", dt);
                    }
                    else
                    {
                        str = str1.ToString();
                        tb.Text = str;
                    }
                }
                tb.ReadOnly = false;
                column = Increment(column);
            }

            column = "A";
            btn.Click += (sender2, e2) => RemoveButton_Click(sender2, e2, RemoveForm, SearchData, location, column, filePath);
        }
    }
}
