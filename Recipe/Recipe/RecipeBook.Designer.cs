namespace Recipe
{
    partial class RecipeBook
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RecipeBook));
            this.CreateNewRecipeList = new System.Windows.Forms.Button();
            this.AddOneRecipe = new System.Windows.Forms.Button();
            this.AddMultipleRecipes = new System.Windows.Forms.Button();
            this.FindRecipe = new System.Windows.Forms.Button();
            this.UpdateRecipe = new System.Windows.Forms.Button();
            this.RemoveRecipe = new System.Windows.Forms.Button();
            this.Substitutionslabel = new System.Windows.Forms.Label();
            this.GetSubstitutions = new System.Windows.Forms.Button();
            this.SaveSubstitutions = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Measurement = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Ingredient = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SubstitutedMeasurement = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SubstitutedIngredient = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DisplayAll = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // CreateNewRecipeList
            // 
            this.CreateNewRecipeList.BackColor = System.Drawing.Color.Navy;
            this.CreateNewRecipeList.FlatAppearance.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.CreateNewRecipeList.FlatAppearance.BorderSize = 3;
            this.CreateNewRecipeList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CreateNewRecipeList.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CreateNewRecipeList.ForeColor = System.Drawing.Color.White;
            this.CreateNewRecipeList.Location = new System.Drawing.Point(230, 297);
            this.CreateNewRecipeList.Name = "CreateNewRecipeList";
            this.CreateNewRecipeList.Size = new System.Drawing.Size(135, 50);
            this.CreateNewRecipeList.TabIndex = 0;
            this.CreateNewRecipeList.Text = "Create New Recipe List";
            this.CreateNewRecipeList.UseVisualStyleBackColor = false;
            this.CreateNewRecipeList.Click += new System.EventHandler(this.CreateNewRecipeList_Click);
            // 
            // AddOneRecipe
            // 
            this.AddOneRecipe.BackColor = System.Drawing.Color.Navy;
            this.AddOneRecipe.FlatAppearance.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.AddOneRecipe.FlatAppearance.BorderSize = 3;
            this.AddOneRecipe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddOneRecipe.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddOneRecipe.ForeColor = System.Drawing.Color.White;
            this.AddOneRecipe.Location = new System.Drawing.Point(90, 297);
            this.AddOneRecipe.Name = "AddOneRecipe";
            this.AddOneRecipe.Size = new System.Drawing.Size(135, 50);
            this.AddOneRecipe.TabIndex = 1;
            this.AddOneRecipe.Text = "Add One Recipe";
            this.AddOneRecipe.UseVisualStyleBackColor = false;
            this.AddOneRecipe.Click += new System.EventHandler(this.AddOneRecipe_Click);
            // 
            // AddMultipleRecipes
            // 
            this.AddMultipleRecipes.BackColor = System.Drawing.Color.Navy;
            this.AddMultipleRecipes.FlatAppearance.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.AddMultipleRecipes.FlatAppearance.BorderSize = 3;
            this.AddMultipleRecipes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddMultipleRecipes.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddMultipleRecipes.ForeColor = System.Drawing.Color.White;
            this.AddMultipleRecipes.Location = new System.Drawing.Point(90, 360);
            this.AddMultipleRecipes.Name = "AddMultipleRecipes";
            this.AddMultipleRecipes.Size = new System.Drawing.Size(135, 50);
            this.AddMultipleRecipes.TabIndex = 2;
            this.AddMultipleRecipes.Text = "Add Multiple Recipes";
            this.AddMultipleRecipes.UseVisualStyleBackColor = false;
            this.AddMultipleRecipes.Click += new System.EventHandler(this.AddMultipleRecipes_Click);
            // 
            // FindRecipe
            // 
            this.FindRecipe.BackColor = System.Drawing.Color.Navy;
            this.FindRecipe.FlatAppearance.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.FindRecipe.FlatAppearance.BorderSize = 3;
            this.FindRecipe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FindRecipe.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FindRecipe.ForeColor = System.Drawing.Color.White;
            this.FindRecipe.Location = new System.Drawing.Point(371, 297);
            this.FindRecipe.Name = "FindRecipe";
            this.FindRecipe.Size = new System.Drawing.Size(134, 50);
            this.FindRecipe.TabIndex = 3;
            this.FindRecipe.Text = "Find Recipe";
            this.FindRecipe.UseVisualStyleBackColor = false;
            this.FindRecipe.Click += new System.EventHandler(this.FindRecipe_Click);
            // 
            // UpdateRecipe
            // 
            this.UpdateRecipe.BackColor = System.Drawing.Color.Navy;
            this.UpdateRecipe.FlatAppearance.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.UpdateRecipe.FlatAppearance.BorderSize = 3;
            this.UpdateRecipe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.UpdateRecipe.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UpdateRecipe.ForeColor = System.Drawing.Color.White;
            this.UpdateRecipe.Location = new System.Drawing.Point(371, 360);
            this.UpdateRecipe.Name = "UpdateRecipe";
            this.UpdateRecipe.Size = new System.Drawing.Size(134, 50);
            this.UpdateRecipe.TabIndex = 4;
            this.UpdateRecipe.Text = "Update Recipe";
            this.UpdateRecipe.UseVisualStyleBackColor = false;
            this.UpdateRecipe.Click += new System.EventHandler(this.UpdateRecipe_Click);
            // 
            // RemoveRecipe
            // 
            this.RemoveRecipe.BackColor = System.Drawing.Color.Navy;
            this.RemoveRecipe.FlatAppearance.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.RemoveRecipe.FlatAppearance.BorderSize = 3;
            this.RemoveRecipe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RemoveRecipe.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RemoveRecipe.ForeColor = System.Drawing.Color.White;
            this.RemoveRecipe.Location = new System.Drawing.Point(230, 360);
            this.RemoveRecipe.Name = "RemoveRecipe";
            this.RemoveRecipe.Size = new System.Drawing.Size(135, 50);
            this.RemoveRecipe.TabIndex = 5;
            this.RemoveRecipe.Text = "Remove Recipe";
            this.RemoveRecipe.UseVisualStyleBackColor = false;
            this.RemoveRecipe.Click += new System.EventHandler(this.RemoveRecipe_Click);
            // 
            // Substitutionslabel
            // 
            this.Substitutionslabel.AutoSize = true;
            this.Substitutionslabel.Font = new System.Drawing.Font("Old English Text MT", 14F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Substitutionslabel.Location = new System.Drawing.Point(243, 43);
            this.Substitutionslabel.Name = "Substitutionslabel";
            this.Substitutionslabel.Size = new System.Drawing.Size(120, 23);
            this.Substitutionslabel.TabIndex = 0;
            this.Substitutionslabel.Text = "Substitutions";
            // 
            // GetSubstitutions
            // 
            this.GetSubstitutions.BackColor = System.Drawing.Color.Navy;
            this.GetSubstitutions.FlatAppearance.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.GetSubstitutions.FlatAppearance.BorderSize = 3;
            this.GetSubstitutions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.GetSubstitutions.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GetSubstitutions.ForeColor = System.Drawing.Color.White;
            this.GetSubstitutions.Location = new System.Drawing.Point(90, 241);
            this.GetSubstitutions.Name = "GetSubstitutions";
            this.GetSubstitutions.Size = new System.Drawing.Size(135, 50);
            this.GetSubstitutions.TabIndex = 7;
            this.GetSubstitutions.Text = "Get Substitutions";
            this.GetSubstitutions.UseVisualStyleBackColor = false;
            this.GetSubstitutions.Click += new System.EventHandler(this.GetSubstitutions_Click);
            // 
            // SaveSubstitutions
            // 
            this.SaveSubstitutions.BackColor = System.Drawing.Color.Navy;
            this.SaveSubstitutions.FlatAppearance.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.SaveSubstitutions.FlatAppearance.BorderSize = 3;
            this.SaveSubstitutions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SaveSubstitutions.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaveSubstitutions.ForeColor = System.Drawing.Color.White;
            this.SaveSubstitutions.Location = new System.Drawing.Point(230, 241);
            this.SaveSubstitutions.Name = "SaveSubstitutions";
            this.SaveSubstitutions.Size = new System.Drawing.Size(133, 50);
            this.SaveSubstitutions.TabIndex = 8;
            this.SaveSubstitutions.Text = "Save Substitutions";
            this.SaveSubstitutions.UseVisualStyleBackColor = false;
            this.SaveSubstitutions.Click += new System.EventHandler(this.SaveSubstitutions_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Measurement,
            this.Ingredient,
            this.SubstitutedMeasurement,
            this.SubstitutedIngredient});
            this.dataGridView1.Location = new System.Drawing.Point(35, 69);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 99;
            this.dataGridView1.Size = new System.Drawing.Size(542, 152);
            this.dataGridView1.TabIndex = 9;
            // 
            // Measurement
            // 
            this.Measurement.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.Measurement.DividerWidth = 1;
            this.Measurement.HeaderText = "Measurement";
            this.Measurement.Name = "Measurement";
            this.Measurement.Width = 97;
            // 
            // Ingredient
            // 
            this.Ingredient.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.Ingredient.DividerWidth = 1;
            this.Ingredient.HeaderText = "Ingredient";
            this.Ingredient.Name = "Ingredient";
            this.Ingredient.Width = 80;
            // 
            // SubstitutedMeasurement
            // 
            this.SubstitutedMeasurement.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.SubstitutedMeasurement.DividerWidth = 1;
            this.SubstitutedMeasurement.HeaderText = "Substituted Measurement";
            this.SubstitutedMeasurement.Name = "SubstitutedMeasurement";
            this.SubstitutedMeasurement.Width = 140;
            // 
            // SubstitutedIngredient
            // 
            this.SubstitutedIngredient.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.SubstitutedIngredient.DividerWidth = 1;
            this.SubstitutedIngredient.HeaderText = "Substituted Ingredient";
            this.SubstitutedIngredient.Name = "SubstitutedIngredient";
            this.SubstitutedIngredient.Width = 124;
            // 
            // DisplayAll
            // 
            this.DisplayAll.BackColor = System.Drawing.Color.Navy;
            this.DisplayAll.FlatAppearance.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.DisplayAll.FlatAppearance.BorderSize = 3;
            this.DisplayAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DisplayAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DisplayAll.ForeColor = System.Drawing.Color.White;
            this.DisplayAll.Location = new System.Drawing.Point(371, 241);
            this.DisplayAll.Name = "DisplayAll";
            this.DisplayAll.Size = new System.Drawing.Size(133, 50);
            this.DisplayAll.TabIndex = 10;
            this.DisplayAll.Text = "Display All Recipes";
            this.DisplayAll.UseVisualStyleBackColor = false;
            this.DisplayAll.Click += new System.EventHandler(this.DisplayAll_Click);
            // 
            // RecipeBook
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Pink;
            this.ClientSize = new System.Drawing.Size(611, 436);
            this.Controls.Add(this.DisplayAll);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.SaveSubstitutions);
            this.Controls.Add(this.GetSubstitutions);
            this.Controls.Add(this.Substitutionslabel);
            this.Controls.Add(this.RemoveRecipe);
            this.Controls.Add(this.UpdateRecipe);
            this.Controls.Add(this.FindRecipe);
            this.Controls.Add(this.AddMultipleRecipes);
            this.Controls.Add(this.AddOneRecipe);
            this.Controls.Add(this.CreateNewRecipeList);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RecipeBook";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Recipe Book";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CreateNewRecipeList;
        private System.Windows.Forms.Button AddOneRecipe;
        private System.Windows.Forms.Button AddMultipleRecipes;
        private System.Windows.Forms.Button FindRecipe;
        private System.Windows.Forms.Button UpdateRecipe;
        private System.Windows.Forms.Button RemoveRecipe;
        private System.Windows.Forms.Label Substitutionslabel;
        private System.Windows.Forms.Button GetSubstitutions;
        private System.Windows.Forms.Button SaveSubstitutions;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Measurement;
        private System.Windows.Forms.DataGridViewTextBoxColumn Ingredient;
        private System.Windows.Forms.DataGridViewTextBoxColumn SubstitutedMeasurement;
        private System.Windows.Forms.DataGridViewTextBoxColumn SubstitutedIngredient;
        private System.Windows.Forms.Button DisplayAll;
    }
}

