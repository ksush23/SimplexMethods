namespace ЗЛП
{
    partial class Form1
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
            this.buttonLoad = new System.Windows.Forms.Button();
            this.symplexMethod = new System.Windows.Forms.Button();
            this.results = new System.Windows.Forms.RichTextBox();
            this.modSymplexMethod = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonLoad
            // 
            this.buttonLoad.Location = new System.Drawing.Point(60, 34);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(163, 64);
            this.buttonLoad.TabIndex = 0;
            this.buttonLoad.Text = "Загрузити файл";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // symplexMethod
            // 
            this.symplexMethod.Location = new System.Drawing.Point(281, 36);
            this.symplexMethod.Name = "symplexMethod";
            this.symplexMethod.Size = new System.Drawing.Size(172, 61);
            this.symplexMethod.TabIndex = 1;
            this.symplexMethod.Text = "Розв\'язати симплекс методом";
            this.symplexMethod.UseVisualStyleBackColor = true;
            this.symplexMethod.Click += new System.EventHandler(this.symplexMethod_Click);
            // 
            // results
            // 
            this.results.Location = new System.Drawing.Point(60, 138);
            this.results.Name = "results";
            this.results.Size = new System.Drawing.Size(618, 277);
            this.results.TabIndex = 3;
            this.results.Text = "";
            // 
            // modSymplexMethod
            // 
            this.modSymplexMethod.Location = new System.Drawing.Point(509, 36);
            this.modSymplexMethod.Name = "modSymplexMethod";
            this.modSymplexMethod.Size = new System.Drawing.Size(136, 62);
            this.modSymplexMethod.TabIndex = 4;
            this.modSymplexMethod.Text = "Розв\'язати модифікованим";
            this.modSymplexMethod.UseVisualStyleBackColor = true;
            this.modSymplexMethod.Click += new System.EventHandler(this.modSymplexMethod_Click_1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.modSymplexMethod);
            this.Controls.Add(this.results);
            this.Controls.Add(this.symplexMethod);
            this.Controls.Add(this.buttonLoad);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.Button symplexMethod;
        private System.Windows.Forms.RichTextBox results;
        private System.Windows.Forms.Button modSymplexMethod;
    }
}

