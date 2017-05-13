using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Projekt_5___Lokaty
{
    public partial class Lokaty_Jaskula44185 : Form
    {
        public Lokaty_Jaskula44185()
        {
            InitializeComponent();
            obrazek.Visible = true;
            TabelaLokaty.Visible = false;
            wykresLokaty.Visible = false;
        }
        bool PobierzDane(out float pj_k, out uint pj_n, out float pj_p)
        {
            pj_k = 0; pj_n = 1; pj_p = 3.1f;
            if (string.IsNullOrEmpty(wysokoscK.Text))
            {
                errorProvider1.SetError(wysokoscK, "ERROR: Musisz podać wartość");
                return false;
            }
            else errorProvider1.Dispose();
            if (string.IsNullOrEmpty(lataLokaty.Text))
            {
                errorProvider1.SetError(lataLokaty, "ERROR: Musisz podać wartość");
                return false;
            }
            else errorProvider1.Dispose();
            if (string.IsNullOrEmpty(stopaP.Text))
            {
                errorProvider1.SetError(stopaP, "ERROR: Musisz wybrać wartość");
                return false;
            }
            else errorProvider1.Dispose();
            if (!float.TryParse(wysokoscK.Text, out pj_k))
            {
                errorProvider1.SetError(wysokoscK, "ERROR: Wystąpił błąd w zapisie - niedozwolony znak");
                return false;
            }
            else errorProvider1.Dispose();
            if (!float.TryParse(stopaP.Text, out pj_p))
            {
                errorProvider1.SetError(stopaP, "ERROR: Wystąpił błąd w zapisie - niedozwolony znak");
                return false;
            }
            else errorProvider1.Dispose();
            if (!uint.TryParse(lataLokaty.Text, out pj_n))
            {
                errorProvider1.SetError(lataLokaty, "ERROR: Wystąpił błąd w zapisie - niedozwolony znak");
                return false;
            }
            else errorProvider1.Dispose();
            
            return true;
        }
        private void PrzejscieDalej_Click(object sender, EventArgs e)
        {
            this.Hide();
            Kredyty_Jaskula44185 EgzemplarzKredyty = new Kredyty_Jaskula44185();
            EgzemplarzKredyty.Show();
        }

        private void ObliczStanKonta_Click(object sender, EventArgs e)
        {
            float pj_k, pj_Kn, pj_p;
            uint pj_n;
            if(!PobierzDane(out pj_k, out pj_n, out pj_p))
                return;
            pj_Kn = pj_k * (float)Math.Pow(1 + (pj_p/100), pj_n);

            stanKonta.Text = pj_Kn.ToString();
            ObliczStanKonta.Enabled = false;
        }
        void RozliczLokatę(ref float[,] TablicaRozliczenLokaty, float pj_k, float pj_p, uint pj_n)
        {
            float StanNaPoczatu, StanNaKoncu, OdsetkiZaOkres;
            StanNaPoczatu = 0.0f;
            OdsetkiZaOkres = 0.0f;
            StanNaKoncu = pj_k;
            for (int i = 0; i < TablicaRozliczenLokaty.GetLength(0); i++)
            {
                TablicaRozliczenLokaty[i, 0] = StanNaPoczatu;
                TablicaRozliczenLokaty[i, 1] = OdsetkiZaOkres;
                TablicaRozliczenLokaty[i, 2] = StanNaKoncu;

                StanNaPoczatu = StanNaKoncu;
                OdsetkiZaOkres = (pj_p/100) * StanNaPoczatu;
                StanNaKoncu = StanNaPoczatu + OdsetkiZaOkres;
            }
        }
        private void Tabelaryczna_Click(object sender, EventArgs e)
        {
            float pj_k, pj_p;
            uint pj_n;
            if (!PobierzDane(out pj_k, out pj_n, out pj_p))
                return;
            float[,] Tablica = new float[pj_n + 1, 3];
            RozliczLokatę(ref Tablica, pj_k, pj_p, pj_n);
            TabelaLokaty.Visible = true; obrazek.Visible = false;
            for (int i = 0; i < Tablica.GetLength(0); i++)
            {
                TabelaLokaty.Rows.Add();
                TabelaLokaty.Rows[i].Cells[0].Value = i;
                TabelaLokaty.Rows[i].Cells[1].Value = Tablica[i,0];
                TabelaLokaty.Rows[i].Cells[2].Value = Tablica[i, 1];
                TabelaLokaty.Rows[i].Cells[3].Value = Tablica[i, 2];
            }
            Tabelaryczna.Enabled = false;
        }

        private void Graficzna_Click(object sender, EventArgs e)
        {
            float pj_k, pj_p;
            uint pj_n;
            if (!PobierzDane(out pj_k, out pj_n, out pj_p))
                return;
            float[,] Tablica = new float[pj_n + 1, 3];
            RozliczLokatę(ref Tablica, pj_k, pj_p, pj_n);
            TabelaLokaty.Visible = false; wykresLokaty.Visible = true; obrazek.Visible = false;
            wykresLokaty.Titles.Add("Zmiana stanu konta w okresie lokaty");
            wykresLokaty.Series[0].Name = "Stan konta";
            int[] NumerOktesowLokaty = new int[Tablica.GetLength(0)];
            for (int i = 0; i < Tablica.GetLength(0); i++)
                NumerOktesowLokaty[i] = i;
            float[] StanKontaKN = new float[Tablica.GetLength(0)];
            for (int i = 0; i < Tablica.GetLength(0); i++)
            {
                StanKontaKN[i] = Tablica[i, 2];
                wykresLokaty.Series[0].Points.DataBindXY(NumerOktesowLokaty, StanKontaKN);
            }
            Graficzna.Enabled = false;
        }

        private void kolorTla_Click(object sender, EventArgs e)
        {
            ColorDialog k1 = new ColorDialog();
            if (k1.ShowDialog() == DialogResult.OK)
            {
                kolorT.BackColor = k1.Color;
                wykresLokaty.BackColor = kolorT.BackColor;
            }
        }

        private void kolorLinii_Click(object sender, EventArgs e)
        {
            ColorDialog k2 = new ColorDialog();
            if (k2.ShowDialog() == DialogResult.OK)
            {
                kolorL.BackColor = k2.Color;
                wykresLokaty.Series[0].Color = kolorL.BackColor;
            }
        }

        private void gruboscLinii_ValueChanged(object sender, EventArgs e)
        {
            if (gruboscLinii.Value == 1)
            {
                wykresLokaty.Series[0].BorderWidth = 1;
            }
            if (gruboscLinii.Value == 2)
            {
                wykresLokaty.Series[0].BorderWidth = 2;
            }
            if (gruboscLinii.Value == 3)
            {
                wykresLokaty.Series[0].BorderWidth = 3;
            }
            if (gruboscLinii.Value == 4)
            {
                wykresLokaty.Series[0].BorderWidth = 4;
            }
            if (gruboscLinii.Value == 5)
            {
                wykresLokaty.Series[0].BorderWidth = 5;
            }
            if (gruboscLinii.Value == 6)
            {
                wykresLokaty.Series[0].BorderWidth = 6;
            }
            if (gruboscLinii.Value == 7)
            {
                wykresLokaty.Series[0].BorderWidth = 7;
            }
            if (gruboscLinii.Value == 8)
            {
                wykresLokaty.Series[0].BorderWidth = 8;
            }
            if (gruboscLinii.Value == 9)
            {
                wykresLokaty.Series[0].BorderWidth = 9;
            }
            if (gruboscLinii.Value == 10)
            {
                wykresLokaty.Series[0].BorderWidth = 10;
            }
        }

        private void stylLini_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (stylLini.SelectedIndex)
            {
                case 0:
                    wykresLokaty.Series[0].BorderDashStyle = ChartDashStyle.Solid;
                    break;
                case 1:
                    wykresLokaty.Series[0].BorderDashStyle = ChartDashStyle.Dash;
                    break;
                case 2:
                    wykresLokaty.Series[0].BorderDashStyle = ChartDashStyle.Dot;
                    break;
                case 3:
                    wykresLokaty.Series[0].BorderDashStyle = ChartDashStyle.DashDot;
                    break;
                default:
                    break;
            }
        }

        private void TypLinii_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (TypLinii.SelectedIndex)
            {
                case 0:
                    wykresLokaty.Series[0].ChartType = SeriesChartType.Bar;
                    break;
                case 1:
                    wykresLokaty.Series[0].ChartType = SeriesChartType.Line;
                    break;
                case 2:
                    wykresLokaty.Series[0].ChartType = SeriesChartType.Column;
                    break;
                case 3:
                    wykresLokaty.Series[0].ChartType = SeriesChartType.Point;
                    break;
                case 4:
                    wykresLokaty.Series[0].ChartType = SeriesChartType.Pyramid;
                    break;
                case 5:
                    wykresLokaty.Series[0].ChartType = SeriesChartType.Bubble;
                    break;
                default:
                    break;
            }
        }

        private void wyjscieZProgramuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Lokaty_Jaskula44185_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult OknoDialogowe = MessageBox.Show("Czy chcesz zamknąć ten formularz?",
            this.Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);

            switch (OknoDialogowe)
            {
                case DialogResult.No:
                    MessageBox.Show("Formularz nie będzie zamknięty (przyczyną próby zamknięcia było zdarzenie: " + e.CloseReason + ")");
                    e.Cancel = true;
                    break;
                case DialogResult.Yes:
                    MessageBox.Show("Teraz nastąpi zamknięcie formularza");
                    e.Cancel = false;
                    break;
                default:
                    MessageBox.Show("Anulowanie zamknięcia formularza");
                    e.Cancel = true;
                    break;
            }
        }

        private void wyjścieZFormularzaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void barToolStripMenuItem_Click(object sender, EventArgs e)
        {
            wykresLokaty.Series[0].ChartType = SeriesChartType.Bar;
        }

        private void lineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            wykresLokaty.Series[0].ChartType = SeriesChartType.Line;
        }

        private void columnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            wykresLokaty.Series[0].ChartType = SeriesChartType.Column;
        }

        private void pyramidToolStripMenuItem_Click(object sender, EventArgs e)
        {
            wykresLokaty.Series[0].ChartType = SeriesChartType.Pyramid;
        }

        private void bubbleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            wykresLokaty.Series[0].ChartType = SeriesChartType.Bubble;
        }

        private void pointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            wykresLokaty.Series[0].ChartType = SeriesChartType.Point;
        }
    }
}
