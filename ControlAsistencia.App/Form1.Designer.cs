namespace ControlAsistencia.App
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblTitulo = new Label();
            lblPasoLeer = new Label();
            lblPasoGuardar = new Label();
            lblArchivo = new Label();
            lblEstado = new Label();
            btnLeerArchivo = new Button();
            btnGuardarResultados = new Button();
            SuspendLayout();
            //
            // lblTitulo
            //
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitulo.Location = new Point(42, 35);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(287, 25);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Procesador de asistencia";
            //
            // lblPasoLeer
            //
            lblPasoLeer.AutoSize = true;
            lblPasoLeer.Location = new Point(45, 95);
            lblPasoLeer.Name = "lblPasoLeer";
            lblPasoLeer.Size = new Size(278, 15);
            lblPasoLeer.TabIndex = 1;
            lblPasoLeer.Text = "1. Lee el Excel de marcaciones y procesa los datos.";
            //
            // lblPasoGuardar
            //
            lblPasoGuardar.AutoSize = true;
            lblPasoGuardar.Location = new Point(45, 165);
            lblPasoGuardar.Name = "lblPasoGuardar";
            lblPasoGuardar.Size = new Size(323, 15);
            lblPasoGuardar.TabIndex = 3;
            lblPasoGuardar.Text = "2. Guarda el resumen y el reporte de novedades en Excel.";
            //
            // lblArchivo
            //
            lblArchivo.AutoSize = true;
            lblArchivo.ForeColor = SystemColors.ControlDarkDark;
            lblArchivo.Location = new Point(45, 245);
            lblArchivo.Name = "lblArchivo";
            lblArchivo.Size = new Size(162, 15);
            lblArchivo.TabIndex = 5;
            lblArchivo.Text = "Archivo cargado: ninguno";
            //
            // lblEstado
            //
            lblEstado.AutoSize = true;
            lblEstado.ForeColor = SystemColors.ControlDarkDark;
            lblEstado.Location = new Point(45, 275);
            lblEstado.Name = "lblEstado";
            lblEstado.Size = new Size(174, 15);
            lblEstado.TabIndex = 6;
            lblEstado.Text = "Estado: esperando archivo";
            //
            // btnLeerArchivo
            //
            btnLeerArchivo.Location = new Point(45, 118);
            btnLeerArchivo.Name = "btnLeerArchivo";
            btnLeerArchivo.Size = new Size(160, 32);
            btnLeerArchivo.TabIndex = 2;
            btnLeerArchivo.Text = "Leer archivo";
            btnLeerArchivo.UseVisualStyleBackColor = true;
            btnLeerArchivo.Click += btnLeerArchivo_Click;
            //
            // btnGuardarResultados
            // 
            btnGuardarResultados.Enabled = false;
            btnGuardarResultados.Location = new Point(45, 188);
            btnGuardarResultados.Name = "btnGuardarResultados";
            btnGuardarResultados.Size = new Size(160, 32);
            btnGuardarResultados.TabIndex = 4;
            btnGuardarResultados.Text = "Guardar resultados";
            btnGuardarResultados.UseVisualStyleBackColor = true;
            btnGuardarResultados.Click += btnGuardarResultados_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(520, 340);
            Controls.Add(lblEstado);
            Controls.Add(lblArchivo);
            Controls.Add(btnGuardarResultados);
            Controls.Add(lblPasoGuardar);
            Controls.Add(btnLeerArchivo);
            Controls.Add(lblPasoLeer);
            Controls.Add(lblTitulo);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Control de Asistencia";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Label lblPasoLeer;
        private Label lblPasoGuardar;
        private Label lblArchivo;
        private Label lblEstado;
        private Button btnLeerArchivo;
        private Button btnGuardarResultados;
    }
}
