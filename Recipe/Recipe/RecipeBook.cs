using System;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;


namespace Recipe
{
    public partial class RecipeBook : Form
    {
        public RecipeBook()
        {
            InitializeComponent();
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

        private static OpenFileDialog OpenNewFile()
        {
            return new OpenFileDialog()
            {
                Filter = "All Excel Files|*.xl*;*.xlsx|Text Documents|*.txt|All Files|*.*",
                Title = "Open File",
                InitialDirectory = @"C:\Users\v-mase\Desktop\C#\files\"
            };
        }

        private string CheckOpenFile(OpenFileDialog openfile)
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

        static string Increment(string s)
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

        private static void ReleaseExcelObject(string filePath, Excel.Application objExcel, Excel.Workbook objBook, Excel._Worksheet objSheet)
        {
            objBook.Close(true, filePath, null);
            Marshal.ReleaseComObject(objSheet);
            Marshal.ReleaseComObject(objBook);
            Marshal.ReleaseComObject(objExcel);
        }

        private static void SetupExcel(string filePath, out Excel.Application objExcel, out Excel.Workbook objBook, out Excel._Worksheet objSheet, out Excel.Range rowRange)
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

        private void FindExcelDataButton_Click(object sender, EventArgs e, Form createForm, Form getRecipeForm, DataGridView temp, string location, string filePath)
        {

            string str;
            Excel.Application objExcel;
            Excel.Workbook objBook;
            Excel._Worksheet objSheet;
            Excel.Range rowRange;
            SetupExcel(filePath, out objExcel, out objBook, out objSheet, out rowRange);
            Excel.Range objRange = objSheet.get_Range("A1", rowRange);

            location = GetExcelValue(createForm, temp, location);
            var column = "A";
            int columnSize = Convert.ToInt32(location);
            while (columnSize > 1)
            {
                column = Increment(column);
                columnSize--;
            }

            RecipeBookControl getRecipeControl = new RecipeBookControl()
            {
                ForeColor = System.Drawing.Color.DarkBlue,
                BackColor = System.Drawing.Color.Pink
            };

            getRecipeForm.Controls.Add(getRecipeControl);
            getRecipeForm.Show();

            var tBox = "textBox";
            TextBox tb = new TextBox();

            for (int count = 1; count < 28; count++)
            {
                tBox = "textBox" + count;
                tb = (TextBox)getRecipeControl.Controls[tBox];
                rowRange = objSheet.Cells[count, column];
                var str1 = rowRange.Value;
                if (str1 == null)
                {
                    str = " ";
                    tb.Text = str;
                }
                else
                {
                    str = str1.ToString();
                    tb.Text = str;
                }
                tb.ReadOnly = true;
            }

            objExcel.Quit();
            ReleaseExcelObject(filePath, objExcel, objBook, objSheet);
        }

        private string GetExcelValue(Form createForm, DataGridView temp, string location)
        {

            int rowLocation = 0;
            int cellLocation = 0;
            string value = "";

            if (temp.SelectedCells.Count > 0)
            {
                cellLocation = temp.SelectedCells[0].ColumnIndex;
                cellLocation++;
                rowLocation = temp.CurrentRow.Index;
                value = temp.Rows[rowLocation].Cells[cellLocation].Value.ToString();
            }

            location = value;

            createForm.Close();
            return location;
        }

        private string FindExcelData(string filePath, string searchData)
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
            currentFind = objRange.Find(searchData, Type.Missing, Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, false, Type.Missing, Type.Missing);

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
                location = (firstFind.Column).ToString();
            }

            ReleaseExcelObject(filePath, objExcel, objBook, objSheet);
            return location;
        }

        private void AddOneRecipeButton_Click(object sender, EventArgs e, Form createForm, RecipeBookControl control, string column, string filePath)
        {
            Excel.Application objExcel;
            Excel.Workbook objBook;
            Excel._Worksheet objSheet;
            Excel.Range rowRange;
            SetupExcel(filePath, out objExcel, out objBook, out objSheet, out rowRange);

            string searchData;
            var tBox = "textBox";
            TextBox tb = new TextBox();

            for (int count = 1; count < 28; count++)
            {
                tBox = "textBox" + count;
                tb = (TextBox)control.Controls[tBox];
                rowRange = objSheet.Cells[count, column];

                objSheet.Cells[count, column] = tb.Text;
            }
            tb = (TextBox)control.Controls["textBox1"];
            searchData = tb.Text;
            string recipeMessage = "Recipe Name " + searchData + " has been added!" + Environment.NewLine;
            MessageBox.Show(recipeMessage, "Add New Recipe", MessageBoxButtons.OK, MessageBoxIcon.Information);
            objSheet.Columns.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            objSheet.Columns.AutoFit();
            objBook.SaveAs(filePath);
            ReleaseExcelObject(filePath, objExcel, objBook, objSheet);
            createForm.Close();
        }

        private void RemoveRecipeButton_Click(object sender, EventArgs e, Form createForm, string searchData, int columnSize, string column, string filePath)
        {

            Excel.Application objExcel;
            Excel.Workbook objBook;
            Excel._Worksheet objSheet;
            Excel.Range rowRange;
            SetupExcel(filePath, out objExcel, out objBook, out objSheet, out rowRange);
            string recipeMessage = "Delete selected Recipe Name: " + searchData + Environment.NewLine;
            if (MessageBox.Show(recipeMessage, "New Recipe Search", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                objSheet.Cells[columnSize, column].EntireColumn.Delete(Excel.XlDirection.xlToLeft);
                recipeMessage = "Recipe Name " + searchData + " has been removed!" + Environment.NewLine;
                MessageBox.Show(recipeMessage, "Recipe Deletion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                objBook.SaveAs(filePath);
            }
            else
            {
                recipeMessage = "Recipe Name " + searchData + " was not removed!" + Environment.NewLine;
                MessageBox.Show(recipeMessage, "Recipe Deletion", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            ReleaseExcelObject(filePath, objExcel, objBook, objSheet);
            createForm.Close();
        }

        private void UpdateRecipeButton_Click(object sender, EventArgs e, Form createForm, RecipeBookControl control, string searchData, int columnSize, string column, string filePath)
        {

            Excel.Application objExcel;
            Excel.Workbook objBook;
            Excel._Worksheet objSheet;
            Excel.Range rowRange;
            SetupExcel(filePath, out objExcel, out objBook, out objSheet, out rowRange);

            string recipeMessage = "Update selected Recipe Name: " + searchData + Environment.NewLine;
            if (MessageBox.Show(recipeMessage, "Update Recipe Search", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var tBox = "textBox";
                TextBox tb = new TextBox();

                for (int count = 1; count < 28; count++)
                {
                    tBox = "textBox" + count;
                    tb = (TextBox)control.Controls[tBox];
                    rowRange = objSheet.Cells[count, column];
                    objSheet.Cells[count, column] = tb.Text;
                }

                recipeMessage = "Recipe Name " + searchData + " has been updated!" + Environment.NewLine;
                MessageBox.Show(recipeMessage, "Recipe Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                objBook.SaveAs(filePath);
            }
            else
            {
                recipeMessage = "Recipe Name " + searchData + " was not updated!" + Environment.NewLine;
                MessageBox.Show(recipeMessage, "Recipe Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            ReleaseExcelObject(filePath, objExcel, objBook, objSheet);
            createForm.Close();
        }

        private void AddMultipleRecipeButton_Click(object sender, EventArgs e, Form createForm, RecipeBookMultipleControl control, string column, string filePath)
        {

            Excel.Application objExcel;
            Excel.Workbook objBook;
            Excel._Worksheet objSheet;
            Excel.Range rowRange;
            SetupExcel(filePath, out objExcel, out objBook, out objSheet, out rowRange);

            string recipeName1 = "";
            string recipeName2 = "";
            string recipeName3 = "";
            string recipeName4 = "";
            string recipeName5 = "";
            int number = 1;
            var tBox = "textBox";
            TextBox tb = new TextBox();

            tb = (TextBox)control.Controls["textBox1"];
            if (tb.Text != null)
            {
                for (int count = 1; count < 28; count++)
                {
                    tBox = "textBox" + number;
                    tb = (TextBox)control.Controls[tBox];
                    rowRange = objSheet.Cells[count, column];
                    objSheet.Cells[count, column] = tb.Text;
                    number++;
                }
                tb = (TextBox)control.Controls["textBox1"];
                recipeName1 = tb.Text;
                column = Increment(column);
                objSheet.Columns.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                objSheet.Columns.AutoFit();
            }

            tb = (TextBox)control.Controls["textBox28"];
            if (tb.Text != null)
            {
                number = 28;
                for (int count = 1; count < 28; count++)
                {
                    tBox = "textBox" + number;
                    tb = (TextBox)control.Controls[tBox];
                    rowRange = objSheet.Cells[count, column];
                    objSheet.Cells[count, column] = tb.Text;
                    number++;
                }
                tb = (TextBox)control.Controls["textBox28"];
                recipeName2 = tb.Text;
                column = Increment(column);
                objSheet.Columns.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                objSheet.Columns.AutoFit();
            }

            tb = (TextBox)control.Controls["textBox55"];
            if (tb.Text != null)
            {
                number = 55;
                for (int count = 1; count < 28; count++)
                {
                    tBox = "textBox" + number;
                    tb = (TextBox)control.Controls[tBox];
                    rowRange = objSheet.Cells[count, column];
                    objSheet.Cells[count, column] = tb.Text;
                    number++;
                }
                tb = (TextBox)control.Controls["textBox55"];
                recipeName3 = tb.Text;
                column = Increment(column);
                objSheet.Columns.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                objSheet.Columns.AutoFit();
            }

            tb = (TextBox)control.Controls["textBox82"];
            if (tb.Text != null)
            {
                number = 82;
                for (int count = 1; count < 28; count++)
                {
                    tBox = "textBox" + number;
                    tb = (TextBox)control.Controls[tBox];
                    rowRange = objSheet.Cells[count, column];
                    objSheet.Cells[count, column] = tb.Text;
                    number++;
                }
                tb = (TextBox)control.Controls["textBox82"];
                recipeName4 = tb.Text;
                column = Increment(column);
                objSheet.Columns.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                objSheet.Columns.AutoFit();
            }

            tb = (TextBox)control.Controls["textBox109"];
            if (tb.Text != null)
            {
                number = 109;
                for (int count = 1; count < 28; count++)
                {
                    tBox = "textBox" + number;
                    tb = (TextBox)control.Controls[tBox];
                    rowRange = objSheet.Cells[count, column];
                    objSheet.Cells[count, column] = tb.Text;
                    number++;
                }
                tb = (TextBox)control.Controls["textBox109"];
                recipeName5 = tb.Text;
                objSheet.Columns.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                objSheet.Columns.AutoFit();
            }

            string recipeMessage = "Below Recipes " + "\r\n" + "\t" + recipeName1 + "\r\n" + "\t" + recipeName2 + "\r\n" + "\t" + recipeName3 + "\r\n" + "\t" + recipeName4 + "\r\n" + "\t" + recipeName5 + "\r\n" + " have been added!" + Environment.NewLine;
            MessageBox.Show(recipeMessage, "Add New Recipe", MessageBoxButtons.OK, MessageBoxIcon.Information);

            objBook.SaveAs(filePath);
            ReleaseExcelObject(filePath, objExcel, objBook, objSheet);
            createForm.Close();
        }

        private void CreateNewRecipeList_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = SaveNewFile;
            saveFileDialog1.ShowDialog();
            string filePath = saveFileDialog1.FileName;
            string fileName = Path.GetFileName(filePath);

            Excel.Application objExcel;
            Excel.Workbook objBook;
            Excel._Worksheet objSheet;
            Excel.Range rowRange;

            objExcel = new Excel.Application();
            objBook = objExcel.Workbooks.Add(System.Reflection.Missing.Value);
            objSheet = (Excel.Worksheet)objBook.Worksheets.get_Item(1);
            rowRange = objSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
            Excel.Range objRange = objSheet.get_Range("A1", rowRange);
            var row = rowRange.Row;
            var column = "A";
            var count = 1;
            int value = 25;
            var total = 2;

            while (value > 0)
            {
                objSheet.Cells[total, column] = "Ingredient " + " " + count;
                count++;
                value--;
                total++;
            }
            objSheet.Cells[1, "A"] = "Name";
            objSheet.Cells[27, "A"] = "Instructions";
            objRange = objSheet.get_Range("A1", "A27");
            objRange.Font.Bold = true;
            objRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            objSheet.Columns.AutoFit();
            objBook.SaveAs(filePath);
            ReleaseExcelObject(filePath, objExcel, objBook, objSheet);

            string recipeMessage = "Recipe file named " + fileName + " has been created!" + Environment.NewLine;
            MessageBox.Show(recipeMessage, "Create New Recipe File", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void FindRecipe_Click(object sender, EventArgs e)
        {

            OpenFileDialog openfile = OpenNewFile();
            openfile.ShowDialog();
            string filePath = CheckOpenFile(openfile);
            while (!(File.Exists(filePath)))
            {
                filePath = CheckOpenFile(openfile);
            }

            string str;
            string location = "";
            string recipeMessage = "";
            int total = 0;
            Excel.Application objExcel;
            Excel.Workbook objBook;
            Excel._Worksheet objSheet;
            Excel.Range rowRange;
            SetupExcel(filePath, out objExcel, out objBook, out objSheet, out rowRange);

            Excel.Range objRange = objSheet.get_Range("B1", rowRange);
            Excel.Range currentFind = null;
            Excel.Range firstFind = null;

            Form getRecipeForm = new Form()
            {
                Text = "Get Recipe",
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                AutoScroll = true,
                Enabled = true             // changes if user can enter data (true = yes, false = no)
            };

            string searchData = Interaction.InputBox("Enter Recipe Name To Find", "Locate Recipe", "");

            while (string.IsNullOrEmpty(searchData) || string.IsNullOrWhiteSpace(searchData))
            {
                recipeMessage = "You entered blank Recipe Name!" + Environment.NewLine + "      Would you like to try again ?";
                if (MessageBox.Show(recipeMessage, "New Recipe Search", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    searchData = Interaction.InputBox("Enter Recipe Name To Find", "Locate Recipe", "");
                }
                else
                {
                    return;                    // Exits out current function
                }
            }

            recipeMessage = "You selected Recipe Name: " + searchData + Environment.NewLine;
            MessageBox.Show(recipeMessage, "New Recipe Search", MessageBoxButtons.OK, MessageBoxIcon.Information);

            int rows;
            rows = objSheet.UsedRange.Rows.Count;
            System.Collections.ArrayList name = new System.Collections.ArrayList();
            System.Collections.ArrayList allRanges = new System.Collections.ArrayList();

            currentFind = objRange.Find(searchData, Type.Missing, Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, false, Type.Missing, Type.Missing);
            if (currentFind == null)
            {
                recipeMessage = "\t" + "Recipe Name " + searchData + " not found!";
                MessageBox.Show(recipeMessage, "Recipe Not Found", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                allRanges.Add((currentFind.Column).ToString());


                while (currentFind != null)
                {
                    if (firstFind == null)
                    {
                        firstFind = currentFind;
                    }

                    currentFind = objRange.FindNext(currentFind);
                    allRanges.Add((currentFind.Column).ToString());

                    if (currentFind.get_Address(Excel.XlReferenceStyle.xlA1) == firstFind.get_Address(Excel.XlReferenceStyle.xlA1))
                    {
                        break;
                    }
                }

                total = allRanges.Count - 1;

                if (total > 1)
                {

                    Form getSubForm = new Form()
                    {
                        Text = "Get Selection",
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
                        Location = new System.Drawing.Point(202, 33),
                        Name = "Get Selection",
                        Size = new System.Drawing.Size(130, 39),
                        TabIndex = 54,
                        Text = "Get Selection",
                        UseVisualStyleBackColor = true
                    };
                    getSubForm.Controls.Add(btn);

                    DataGridControl newSubControl = new DataGridControl()
                    {
                        ForeColor = System.Drawing.Color.Black,
                        BackColor = System.Drawing.Color.Aquamarine,
                        AutoSize = true,
                        AutoSizeMode = AutoSizeMode.GrowAndShrink,
                        AutoScroll = true
                    };

                    DataGridView temp = new DataGridView();
                    DataGridViewTextBoxColumn col1 = new DataGridViewTextBoxColumn();
                    DataGridViewTextBoxColumn col2 = new DataGridViewTextBoxColumn();

                    col1.Name = "Name";
                    col2.Name = "Location";

                    temp.Columns.AddRange(new DataGridViewColumn[]
                    {
                col1,
                col2
                    });
                    temp.Location = new System.Drawing.Point(11, 33);
                    temp.Name = "dataGridView1";
                    temp.Size = new System.Drawing.Size(405, 290);
                    temp.TabIndex = 0;

                    newSubControl.Controls.Add(temp);
                    getSubForm.Controls.Add(newSubControl);

                    for (int i = 0; i < allRanges.Count - 1; i++)
                    {

                        location = allRanges[i].ToString();

                        var column = "A";
                        int columnSize = Convert.ToInt32(location);
                        while (columnSize > 1)
                        {
                            column = Increment(column);
                            columnSize--;
                        }

                        rowRange = objSheet.Cells[1, column];
                        name.Add(rowRange.Value);
                        temp.Rows.Add(rowRange.Value, location);
                    }

                    int height = temp.Bottom;
                    height = height + 30;
                    btn.Location = new System.Drawing.Point(150, height);
                    //newSubControl.BackColor = System.Drawing.Color.Aquamarine;
                    getSubForm.BackColor = System.Drawing.Color.Aquamarine;
                    btn.BackColor = System.Drawing.Color.Gray;
                    getSubForm.Show();
                    btn.Click += (sender2, e2) => FindExcelDataButton_Click(sender2, e2, getSubForm, getRecipeForm, temp, location, filePath);
                }

                if (total == 1)
                {
                    location = (firstFind.Column).ToString();


                    var column = "A";
                    int columnSize = Convert.ToInt32(location);
                    while (columnSize > 1)
                    {
                        column = Increment(column);
                        columnSize--;
                    }

                    RecipeBookControl getRecipeControl = new RecipeBookControl()
                    {
                        ForeColor = System.Drawing.Color.DarkBlue,
                        BackColor = System.Drawing.Color.Pink

                    };

                    getRecipeForm.Controls.Add(getRecipeControl);
                    getRecipeForm.Show();

                    var tBox = "textBox";
                    TextBox tb = new TextBox();

                    for (int count = 1; count < 28; count++)
                    {
                        tBox = "textBox" + count;
                        tb = (TextBox)getRecipeControl.Controls[tBox];
                        rowRange = objSheet.Cells[count, column];
                        var str1 = rowRange.Value;
                        if (str1 == null)
                        {
                            str = " ";
                            tb.Text = str;
                        }
                        else
                        {
                            str = str1.ToString();
                            tb.Text = str;
                        }
                        tb.ReadOnly = true;
                    }
                }

                if (total < 1)
                {
                    recipeMessage = "\t" + "Recipe Name " + searchData + " not found!";
                    MessageBox.Show(recipeMessage, "Recipe Not Found", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }

            }
        }

        private void AddOneRecipe_Click(object sender, EventArgs e)
        {
            Form newRecipeForm = new Form()
            {
                Text = "Add One New Recipe",
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FormBorderStyle = FormBorderStyle.Fixed3D,
                StartPosition = FormStartPosition.CenterScreen,
                AutoScroll = true,
                AllowDrop = true,           // allows user to drag and drop
                Enabled = true             // changes if user can enter data (true = yes, false = no)
            };

            Button btn = new Button()
            {
                FlatStyle = FlatStyle.Popup,
                Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                Location = new System.Drawing.Point(402, 532),
                Name = "Add One New Recipe",
                Size = new System.Drawing.Size(130, 39),
                TabIndex = 54,
                Text = "Add One New Recipe",
                UseVisualStyleBackColor = true
            };
            newRecipeForm.Controls.Add(btn);

            RecipeBookControl newRecipeControl = new RecipeBookControl()
            {
                ForeColor = System.Drawing.Color.Black,
                BackColor = System.Drawing.Color.Aquamarine
            };

            Excel.Application objExcel;
            Excel.Workbook objBook;
            Excel._Worksheet objSheet;
            Excel.Range rowRange;
            string column;
            string filePath;

            OpenFileDialog openfile = OpenNewFile();
            openfile.ShowDialog();
            filePath = CheckOpenFile(openfile);
            while (!(File.Exists(filePath)))
            {
                filePath = CheckOpenFile(openfile);
            }

            SetupExcel(filePath, out objExcel, out objBook, out objSheet, out rowRange);
            Excel.Range objRange = objSheet.get_Range("A1", rowRange);
            column = "A";
            var columnSize = objSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Column;

            while (columnSize > 0)
            {
                column = Increment(column);
                columnSize--;
            }

            newRecipeForm.Controls.Add(newRecipeControl);
            newRecipeForm.Show();
            btn.Click += (sender2, e2) => AddOneRecipeButton_Click(sender2, e2, newRecipeForm, newRecipeControl, column, filePath);
        }

        private void UpdateRecipe_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfile = OpenNewFile();
            openfile.ShowDialog();
            string filePath = CheckOpenFile(openfile);
            while (!(File.Exists(filePath)))
            {
                filePath = CheckOpenFile(openfile);
            }

            string str;
            string location = "";
            string recipeMessage = "";
            Excel.Application objExcel;
            Excel.Workbook objBook;
            Excel._Worksheet objSheet;
            Excel.Range rowRange;
            SetupExcel(filePath, out objExcel, out objBook, out objSheet, out rowRange);

            Form getRecipeForm = new Form()
            {
                Text = "Get Recipe",
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                StartPosition = FormStartPosition.CenterScreen,
                AutoScroll = true,
                Enabled = true             // changes if user can enter data (true = yes, false = no)
            };

            Button btn = new Button()
            {
                FlatStyle = FlatStyle.Popup,
                Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                Location = new System.Drawing.Point(402, 532),
                Name = "Update Recipe",
                Size = new System.Drawing.Size(130, 39),
                TabIndex = 54,
                Text = "Update Recipe",
                UseVisualStyleBackColor = true
            };
            getRecipeForm.Controls.Add(btn);


            string searchData = Interaction.InputBox("Enter Recipe Name To Find", "Locate Recipe", "");

            while (string.IsNullOrEmpty(searchData) || string.IsNullOrWhiteSpace(searchData))
            {
                recipeMessage = "You entered blank Recipe Name!" + Environment.NewLine + "      Would you like to try again ?";
                if (MessageBox.Show(recipeMessage, "New Recipe Search", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    searchData = Interaction.InputBox("Enter Recipe Name To Find", "Locate Recipe", "");
                }
                else
                {
                    return;                    // Exits out current function
                }
            }

            recipeMessage = "You selected Recipe Name: " + searchData + Environment.NewLine;
            MessageBox.Show(recipeMessage, "New Recipe Search", MessageBoxButtons.OK, MessageBoxIcon.Information);

            location = FindExcelData(filePath, searchData);
            while (location == "")
            {
                recipeMessage = "\t" + "Recipe Name " + searchData + " not found!" + Environment.NewLine + "Do you want to search for a different recipe?";
                if (MessageBox.Show(recipeMessage, "New Recipe Search", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    searchData = Interaction.InputBox("Enter Recipe Name To Find", "Locate Recipe", "");
                    location = FindExcelData(filePath, searchData);
                }
                else
                {
                    return;                    // Exits out current function
                }
            }

            string column = "A";
            int columnSize = Convert.ToInt32(location);
            while (columnSize > 1)
            {
                column = Increment(column);
                columnSize--;
            }

            RecipeBookControl getRecipeControl = new RecipeBookControl()
            {
                ForeColor = System.Drawing.Color.MediumPurple,
                BackColor = System.Drawing.Color.LightSkyBlue
            };
            getRecipeForm.Controls.Add(getRecipeControl);
            getRecipeForm.Show();

            var tBox = "textBox";
            TextBox tb = new TextBox();

            for (int count = 1; count < 28; count++)
            {
                tBox = "textBox" + count;
                tb = (TextBox)getRecipeControl.Controls[tBox];
                rowRange = objSheet.Cells[count, column];
                var str1 = rowRange.Value;
                if (str1 == null)
                {
                    str = " ";
                    tb.Text = str;
                }
                else
                {
                    str = str1.ToString();
                    tb.Text = str;
                }
                tb.ReadOnly = false;
            }

            columnSize = Convert.ToInt32(location);

            btn.Click += (sender2, e2) => UpdateRecipeButton_Click(sender2, e2, getRecipeForm, getRecipeControl, searchData, columnSize, column, filePath);
        }

        private void AddMultipleRecipes_Click(object sender, EventArgs e)
        {
            Form newMultipleRecipeForm = new Form()
            {
                Text = "Add Multiple Recipes",
                AutoSize = true,
                StartPosition = FormStartPosition.CenterScreen,
                WindowState = FormWindowState.Maximized,
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
                Location = new System.Drawing.Point(557, 860),
                Name = "Add Multiple New Recipes",
                Size = new System.Drawing.Size(120, 50),
                TabIndex = 305,
                Text = "Add Multiple New Recipes",
                UseVisualStyleBackColor = true
            };
            newMultipleRecipeForm.Controls.Add(btn);

            RecipeBookMultipleControl newMultipleRecipeControl = new RecipeBookMultipleControl()
            {
                ForeColor = System.Drawing.Color.Black,
                BackColor = System.Drawing.Color.Aquamarine,
                AutoSize = true,
                AutoScroll = true
            };

            int number = 27;
            var tBox = "textBox" + number;
            TextBox tb = new TextBox();
            tb = (TextBox)newMultipleRecipeControl.Controls[tBox];
            tb.AcceptsReturn = true;
            tb.AllowDrop = true;
            tb.ScrollBars = ScrollBars.Both;
            number = 54;
            tBox = "textBox" + number;
            tb = (TextBox)newMultipleRecipeControl.Controls[tBox];
            tb.AcceptsReturn = true;
            tb.AllowDrop = true;
            tb.ScrollBars = ScrollBars.Both;
            number = 81;
            tBox = "textBox" + number;
            tb = (TextBox)newMultipleRecipeControl.Controls[tBox];
            tb.AcceptsReturn = true;
            tb.AllowDrop = true;
            tb.ScrollBars = ScrollBars.Both;
            number = 108;
            tBox = "textBox" + number;
            tb = (TextBox)newMultipleRecipeControl.Controls[tBox];
            tb.AcceptsReturn = true;
            tb.AllowDrop = true;
            tb.ScrollBars = ScrollBars.Both;
            number = 135;
            tBox = "textBox" + number;
            tb = (TextBox)newMultipleRecipeControl.Controls[tBox];
            tb.AcceptsReturn = true;
            tb.AllowDrop = true;
            tb.ScrollBars = ScrollBars.Both;

            Excel.Application objExcel;
            Excel.Workbook objBook;
            Excel._Worksheet objSheet;
            Excel.Range rowRange;
            string column;
            string filePath;

            OpenFileDialog openfile = OpenNewFile();
            openfile.ShowDialog();
            filePath = CheckOpenFile(openfile);
            while (!(File.Exists(filePath)))
            {
                filePath = CheckOpenFile(openfile);
            }

            SetupExcel(filePath, out objExcel, out objBook, out objSheet, out rowRange);
            Excel.Range objRange = objSheet.get_Range("A1", rowRange);
            column = "A";
            var columnSize = objSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Column;

            while (columnSize > 0)
            {
                column = Increment(column);
                columnSize--;
            }

            tb = (TextBox)newMultipleRecipeControl.Controls["textBox1"];

            newMultipleRecipeForm.Controls.Add(newMultipleRecipeControl);
            newMultipleRecipeForm.ActiveControl = tb;
            newMultipleRecipeForm.Show();
            btn.Click += (sender2, e2) => AddMultipleRecipeButton_Click(sender2, e2, newMultipleRecipeForm, newMultipleRecipeControl, column, filePath);

        }

        private void RemoveRecipe_Click(object sender, EventArgs e)
        {

            OpenFileDialog openfile = OpenNewFile();
            openfile.ShowDialog();
            string filePath = CheckOpenFile(openfile);
            while (!(File.Exists(filePath)))
            {
                filePath = CheckOpenFile(openfile);
            }

            string str;
            string location = "";
            string recipeMessage = "";
            Excel.Application objExcel;
            Excel.Workbook objBook;
            Excel._Worksheet objSheet;
            Excel.Range rowRange;
            SetupExcel(filePath, out objExcel, out objBook, out objSheet, out rowRange);

            Form getRecipeForm = new Form()
            {
                Text = "Get Recipe",
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                AutoScroll = true,
                StartPosition = FormStartPosition.CenterScreen,
                Enabled = true             // changes if user can enter data (true = yes, false = no)
                
            };

            Button btn = new Button()
            {
                FlatStyle = FlatStyle.Popup,
                Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                Location = new System.Drawing.Point(402, 532),
                Name = "Remove Recipe",
                Size = new System.Drawing.Size(130, 39),
                TabIndex = 54,
                Text = "Remove Recipe",
                UseVisualStyleBackColor = true
            };
            getRecipeForm.Controls.Add(btn);

            string searchData = Interaction.InputBox("Enter Recipe Name To Find", "Locate Recipe", "");

            while (string.IsNullOrEmpty(searchData) || string.IsNullOrWhiteSpace(searchData))
            {
                recipeMessage = "You entered blank Recipe Name!" + Environment.NewLine + "      Would you like to try again ?";
                if (MessageBox.Show(recipeMessage, "New Recipe Search", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    searchData = Interaction.InputBox("Enter Recipe Name To Find", "Locate Recipe", "");
                }
                else
                {
                    return;                    // Exits out current function
                }
            }

            recipeMessage = "You selected Recipe Name: " + searchData + Environment.NewLine;
            MessageBox.Show(recipeMessage, "New Recipe Search", MessageBoxButtons.OK, MessageBoxIcon.Information);

            location = FindExcelData(filePath, searchData);
            while (location == "")
            {
                recipeMessage = "\t" + "Recipe Name " + searchData + " not found!" + Environment.NewLine + "Do you want to search for a different recipe?";
                if (MessageBox.Show(recipeMessage, "New Recipe Search", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    searchData = Interaction.InputBox("Enter Recipe Name To Find", "Locate Recipe", "");
                    location = FindExcelData(filePath, searchData);
                }
                else
                {
                    return;                    // Exits out current function
                }
            }

            string column = "A";
            int columnSize = Convert.ToInt32(location);
            while (columnSize > 1)
            {
                column = Increment(column);
                columnSize--;
            }

            RecipeBookControl getRecipeControl = new RecipeBookControl()
            {
                ForeColor = System.Drawing.Color.DarkBlue,
                BackColor = System.Drawing.Color.Pink
               
            };

            getRecipeForm.Controls.Add(getRecipeControl);
            getRecipeForm.Show();

            var tBox = "textBox";
            TextBox tb = new TextBox();

            for (int count = 1; count < 28; count++)
            {
                tBox = "textBox" + count;
                tb = (TextBox)getRecipeControl.Controls[tBox];
                rowRange = objSheet.Cells[count, column];
                var str1 = rowRange.Value;
                if (str1 == null)
                {
                    str = " ";
                    tb.Text = str;
                }
                else
                {
                    str = str1.ToString();
                    tb.Text = str;
                }
                tb.ReadOnly = true;
            }

            columnSize = Convert.ToInt32(location);
            btn.Click += (sender2, e2) => RemoveRecipeButton_Click(sender2, e2, getRecipeForm, searchData, columnSize, column, filePath);
        }

        private void SaveSubstitutions_Click(object sender, EventArgs e)
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
            Excel.Range objRange = objSheet.get_Range("A1", rowRange);

            string[] excelRow = new string[4];

/*
            string[] row1 = new string[4];
            string[] row2 = new string[4];
            string[] row3 = new string[4];
            string[] row4 = new string[4];
*/

            string column = "A";
            int count = 2;
            int rows;
            rows = objSheet.UsedRange.Rows.Count;

            objSheet.Cells[1, "A"] = "Amount";
            objSheet.Cells[1, "B"] = "Ingredient";
            objSheet.Cells[1, "C"] = "Substituted Amount";
            objSheet.Cells[1, "D"] = "Substituted Ingredient";
            objRange = objSheet.get_Range("A1", "D1");
            objRange.Font.Bold = true;


            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                for (int j = 0; j < dataGridView1.Columns.Count; j++)
                {
                    objSheet.Cells[count, column] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                    column = Increment(column);
                }
                count++;
                column = "A";
            }


            objRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            objSheet.Columns.AutoFit();
            objBook.SaveAs(filePath);
            ReleaseExcelObject(filePath, objExcel, objBook, objSheet);
        }

        private void GetSubstitutions_Click(object sender, EventArgs e)
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

            string[] excelRow = new string[4];

/*
            string[] row1 = new string[4];
            string[] row2 = new string[4];
            string[] row3 = new string[4];
            string[] row4 = new string[4];
*/

            string column = "A";
            int count = 2;
            int x = 1;
            int rows;
            rows = objSheet.UsedRange.Rows.Count;

            while (x < rows)
            {

                for (int i = 0; i < 4; i++)
                {
                    excelRow[i] = (objSheet.Cells[count, column]).Value2;
                    column = Increment(column);
                }
                dataGridView1.Rows.Add(excelRow);
                x++;
                count++;
                column = "A";
            }

            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.HeaderCell.Style.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic , System.Drawing.GraphicsUnit.Point, 0);
                col.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            ReleaseExcelObject(filePath, objExcel, objBook, objSheet);
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

            Form displayAllForm = new Form()
            {
                Text = "Display All",
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FormBorderStyle = FormBorderStyle.Fixed3D,
                AutoScroll = true,
                AllowDrop = true,           // allows user to drag and drop
                Enabled = true             // changes if user can enter data (true = yes, false = no)
            };

            DataGridControl newSubControl = new DataGridControl()
            {
                ForeColor = System.Drawing.Color.Black,
                BackColor = System.Drawing.Color.Aquamarine,
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
            temp.ColumnCount = 27;

            temp.Columns[0].Name = "Name";
            temp.Columns[26].Name = "Instructions";

            for (int i = 1; i < 26; i++)
            {
                temp.Columns[i].Name = "Ingredient " + i;
            }

            newSubControl.Controls.Add(temp);
            displayAllForm.Controls.Add(newSubControl);

            string[] excelRow = new string[27];
            string column = "B";
            int count = 1;
            int x = 0;
            int rows;
            rows = objSheet.UsedRange.Columns.Count;

            while (x < rows)
            {

                for (int i = 0; i < 27; i++)
                {
                    var value = (objSheet.Cells[count, column]).Value2;
                    excelRow[i] = Convert.ToString(value);
                    count++;
                }

                temp.Rows.Add(excelRow);
                x++;
                count = 1;
                column = Increment(column);
            }

            foreach (DataGridViewColumn col in temp.Columns)
            {
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.HeaderCell.Style.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
                col.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            displayAllForm.Show();
            ReleaseExcelObject(filePath, objExcel, objBook, objSheet);
        }
    }
}
