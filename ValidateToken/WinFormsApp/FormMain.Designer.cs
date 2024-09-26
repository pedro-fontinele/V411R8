using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using ValidateToken.Application;

namespace WinFormsApp
{
    partial class FormMain : Form
    {
        private IContainer _components = null;
        private Button _buttonExecute; // Botão para validar token
        private Panel _notificationPanel; // Painel para a notificação
        private Label _labelNotification; // Label para notificação

        protected override void Dispose(bool disposing)
        {
            if (disposing && (_components != null))
            {
                _components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            _buttonExecute = new Button();
            _notificationPanel = new Panel();
            _labelNotification = new Label();
            _notificationPanel.SuspendLayout();
            SuspendLayout();
            // 
            // btnExecute
            // 
            _buttonExecute.BackColor = System.Drawing.Color.FromArgb(34, 34, 34);
            _buttonExecute.FlatStyle = FlatStyle.Flat;
            _buttonExecute.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            _buttonExecute.ForeColor = System.Drawing.Color.White;
            _buttonExecute.Location = new System.Drawing.Point(325, 200);
            _buttonExecute.Name = "btnExecute";
            _buttonExecute.Size = new System.Drawing.Size(150, 50);
            _buttonExecute.TabIndex = 0;
            _buttonExecute.Text = "Validar token";
            _buttonExecute.UseVisualStyleBackColor = false;
            _buttonExecute.Click += btnExecute_Click;
            // 
            // notificationPanel
            // 
            _notificationPanel.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
            _notificationPanel.Controls.Add(_labelNotification);
            _notificationPanel.Location = new System.Drawing.Point(0, 0);
            _notificationPanel.Name = "notificationPanel";
            _notificationPanel.Size = new System.Drawing.Size(800, 50);
            _notificationPanel.TabIndex = 1;
            _notificationPanel.Visible = false;
            // 
            // lblNotification
            // 
            _labelNotification.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Bold);
            _labelNotification.ForeColor = System.Drawing.Color.White;
            _labelNotification.Location = new System.Drawing.Point(0, 0);
            _labelNotification.Name = "lblNotification";
            _labelNotification.Size = new System.Drawing.Size(784, 50);
            _labelNotification.TabIndex = 0;
            _labelNotification.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormMain
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            ClientSize = new System.Drawing.Size(784, 411);
            Controls.Add(_buttonExecute);
            Controls.Add(_notificationPanel);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MaximumSize = new System.Drawing.Size(800, 450);
            MinimumSize = new System.Drawing.Size(800, 450);
            Name = "FormMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Validação de Tokens";
            _notificationPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        // Manipulador de evento para o clique do botão
        private async void btnExecute_Click(object sender, EventArgs eventArgs)
        {
            // Exibe a faixa de notificação
            _labelNotification.Text = "Validando, aguarde um momento...";
            _notificationPanel.Visible = true;

            var serviceProvider = Program.GetServiceProvider(); // Método que retorna o serviceProvider
            var app = serviceProvider.GetRequiredService<IWorker>();
            await app.RunAsync(); // Chama o método RunAsync do IWorker

            // Mensagem exibida após a validação do token
            MessageBox.Show("Planilhas com os tokens dos clientes anexados em:\nC:\\TokensValidos\n\nClique aqui para abrir.", "Tokens Válidos", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Ação para abrir o diretório após o clique
            var result = MessageBox.Show("Deseja abrir a pasta?", "Abrir Pasta", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                Process.Start("explorer.exe", @"C:\TokensValidos"); // Abre a pasta no Explorer
            }

            // Limpa a notificação
            _notificationPanel.Visible = false;
            Application.Exit(); // Fecha o programa
        }
    }
}
