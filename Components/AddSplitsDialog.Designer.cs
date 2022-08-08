namespace Livesplit.AVP2.Components
{
    partial class AddSplitsDialog
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
            this.areyousure = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cancel = new System.Windows.Forms.Button();
            this.addSubsplits = new System.Windows.Forms.Button();
            this.addNormal = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // areyousure
            // 
            this.areyousure.AutoSize = true;
            this.areyousure.Location = new System.Drawing.Point(29, 29);
            this.areyousure.Margin = new System.Windows.Forms.Padding(20, 20, 0, 0);
            this.areyousure.Name = "areyousure";
            this.areyousure.Size = new System.Drawing.Size(379, 13);
            this.areyousure.TabIndex = 1;
            this.areyousure.Text = "Are you sure you want to add these %LevelCount% %Game% - %Name% splits?";
            this.areyousure.Click += new System.EventHandler(this.areyousure_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 62);
            this.label1.Margin = new System.Windows.Forms.Padding(20, 20, 0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(311, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Choose \"Add as Subsplits\" to add the splits in the Subsplits form.";
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cancel.Location = new System.Drawing.Point(29, 104);
            this.cancel.Margin = new System.Windows.Forms.Padding(20, 3, 3, 10);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 8;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // addSubsplits
            // 
            this.addSubsplits.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.addSubsplits.Location = new System.Drawing.Point(234, 104);
            this.addSubsplits.Margin = new System.Windows.Forms.Padding(3, 3, 10, 10);
            this.addSubsplits.Name = "addSubsplits";
            this.addSubsplits.Size = new System.Drawing.Size(96, 23);
            this.addSubsplits.TabIndex = 7;
            this.addSubsplits.Text = "Add as Subsplits";
            this.addSubsplits.UseVisualStyleBackColor = true;
            this.addSubsplits.Click += new System.EventHandler(this.addSubsplits_Click);
            // 
            // addNormal
            // 
            this.addNormal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.addNormal.Location = new System.Drawing.Point(343, 104);
            this.addNormal.Margin = new System.Windows.Forms.Padding(3, 3, 20, 10);
            this.addNormal.Name = "addNormal";
            this.addNormal.Size = new System.Drawing.Size(75, 23);
            this.addNormal.TabIndex = 6;
            this.addNormal.Text = "Add";
            this.addNormal.UseVisualStyleBackColor = true;
            this.addNormal.Click += new System.EventHandler(this.addNormal_Click);
            // 
            // AddSplitsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(447, 146);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.addSubsplits);
            this.Controls.Add(this.addNormal);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.areyousure);
            this.Name = "AddSplitsDialog";
            this.Text = "AVP2 | Confirmation";
            this.Load += new System.EventHandler(this.AddSplitsDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label areyousure;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Button addSubsplits;
        private System.Windows.Forms.Button addNormal;
    }
}