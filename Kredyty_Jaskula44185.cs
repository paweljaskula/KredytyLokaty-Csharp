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
    public partial class Kredyty_Jaskula44185 : Form
    {
        public Kredyty_Jaskula44185()
        {
            InitializeComponent();
            grpWyborLiniiWykresu.Visible = false;
        }
        bool PobierzDane(out float pj_k, out uint pj_n, out float pj_p, out uint pj_m)
        {
            pj_k = 0; pj_n = 1; pj_p = 3.1f; pj_m = 1;
            if (string.IsNullOrEmpty(wysokoscKredytu.Text))
            {
                errorProvider1.SetError(wysokoscKredytu, "ERROR: Musisz podać wartość");
                return false;
            }
            else errorProvider1.Dispose();
            if (string.IsNullOrEmpty(rocznaStopaProc.Text))
            {
                errorProvider1.SetError(rocznaStopaProc, "ERROR: Musisz podać wartość");
                return false;
            }
            else errorProvider1.Dispose();
            if (string.IsNullOrEmpty(okresSplaty.Text))
            {
                errorProvider1.SetError(okresSplaty, "ERROR: Musisz wybrać wartość");
                return false;
            }
            else errorProvider1.Dispose();
            if (!float.TryParse(wysokoscKredytu.Text, out pj_k))
            {
                errorProvider1.SetError(wysokoscKredytu, "ERROR: Wystąpił błąd w zapisie - niedozwolony znak");
                return false;
            }
            else errorProvider1.Dispose();
            if (!float.TryParse(rocznaStopaProc.Text, out pj_p))
            {
                errorProvider1.SetError(rocznaStopaProc, "ERROR: Wystąpił błąd w zapisie - niedozwolony znak");
                return false;
            }
            else errorProvider1.Dispose();
            if (!uint.TryParse(okresSplaty.Text, out pj_n))
            {
                errorProvider1.SetError(okresSplaty, "ERROR: Wystąpił błąd w zapisie - niedozwolony znak");
                return false;
            }
            else errorProvider1.Dispose();
            if (razWRoku.Checked)
                pj_m = 1;
            else if (coPolRoku.Checked)
                pj_m = 2;
            else if (coKwartal.Checked)
                pj_m = 4;
            else if (coMiesiac.Checked)
                pj_m = 12;
            else
            {
                errorProvider1.SetError(groupBox1, "ERROR: Musisz wybrać opcję");
                return false;
            }
               
            
            return true;
        }
        private void Przejscie_Click(object sender, EventArgs e)
        {
            this.Hide();

            foreach (Form Formularz in Application.OpenForms)
            {
                if (Formularz.Name == "Lokaty")
                {
                    Formularz.Show();
                    return;
                }
                
            }
            Lokaty_Jaskula44185 EgzemplarzLokaty = new Lokaty_Jaskula44185();
            EgzemplarzLokaty.Show();
        }
        private void tabelkaRoz_Click(object sender, EventArgs e)
        {
            float pj_K, pj_p;
            uint pj_n, pj_m;
            if (!PobierzDane(out pj_K, out pj_n, out pj_p, out pj_m))
                return;
            
	        if(malejace.Checked)
            {
                float pj_Ro, pj_Rk, pj_R;
                double pj_Z;
                float[,] RozliczenieKredytu = new float[pj_n * pj_m + 1, 3];

                RozliczenieKredytu[0, 0] = 0.0f;
                RozliczenieKredytu[0, 1] = 0.0f;
                RozliczenieKredytu[0, 2] = pj_K;
                pj_Z = pj_K; pj_Rk = pj_K / (pj_n * pj_m); pj_Ro = 0.0f; pj_R = 0.0f;
                float KosztKredytu = 0.0f;
                for (int i = 0; i < RozliczenieKredytu.GetLength(0); i++)
                {
                    RozliczenieKredytu[i, 0] = pj_R;
                    RozliczenieKredytu[i, 1] = pj_Ro;
                    RozliczenieKredytu[i, 2] = (float)pj_Z;
                    pj_Ro = (float)pj_Z * pj_p / pj_m;
                    pj_R = pj_Rk + pj_Ro;
                    pj_Z = pj_Z - pj_Rk;
                    KosztKredytu += pj_Ro;
                }
                kosztKredytu.Text = KosztKredytu.ToString();
                for (int i = 0; i < RozliczenieKredytu.GetLength(0); i++)
                {
                    dvgRozliczenieKredytu.Rows.Add();
                    dvgRozliczenieKredytu.Rows[i].Cells[0].Value = i;
                    dvgRozliczenieKredytu.Rows[i].Cells[1].Value = RozliczenieKredytu[i,0];
                    dvgRozliczenieKredytu.Rows[i].Cells[2].Value = RozliczenieKredytu[i, 1];
                    dvgRozliczenieKredytu.Rows[i].Cells[3].Value = RozliczenieKredytu[i, 2];
                }
                RataRk.Text = pj_Rk.ToString();
                StanZadluzenia.Text = RozliczenieKredytu[pj_n * pj_m, 2].ToString();
                tabelkaRoz.Enabled = false;
                this.TabCtrl.SelectedTab = tabelaryczneRoz;
            }
            else
                if (rosnace.Checked)
                {
                    float pj_Ro, pj_Rk, pj_R;
                    double pj_Z;
                    float[,] RozliczenieKredytu = new float[pj_n * pj_m + 1, 3];
                    RozliczenieKredytu[0, 0] = 0.0f;
                    RozliczenieKredytu[0, 1] = 0.0f;
                    RozliczenieKredytu[0, 2] = pj_K;
                    pj_Z = pj_K; pj_Rk = pj_K / (pj_n * pj_m); pj_Ro = 0.0f; pj_R = 0.0f;
                    
                    float KosztKredytu = 0.0f;
                    for (int i = 0; i < RozliczenieKredytu.GetLength(0); i++)
                    {
                        RozliczenieKredytu[i, 0] = pj_R;
                        RozliczenieKredytu[i, 1] = pj_Ro;
                        RozliczenieKredytu[i, 2] = (float)pj_Z;
                        if (i >= 1)
                        {
                            pj_Ro = i * pj_Rk * pj_p / pj_m;
                        }
                        
                        pj_R = pj_Rk + pj_Ro;
                        pj_Z = pj_Z - pj_Rk;
                        KosztKredytu += pj_Ro;
                    }
                    kosztKredytu.Text = KosztKredytu.ToString();
                    for (int i = 0; i < RozliczenieKredytu.GetLength(0); i++)
                    {
                        dvgRozliczenieKredytu.Rows.Add();
                        dvgRozliczenieKredytu.Rows[i].Cells[0].Value = i;
                        dvgRozliczenieKredytu.Rows[i].Cells[1].Value = RozliczenieKredytu[i, 0];
                        dvgRozliczenieKredytu.Rows[i].Cells[2].Value = RozliczenieKredytu[i, 1];
                        dvgRozliczenieKredytu.Rows[i].Cells[3].Value = RozliczenieKredytu[i, 2];
                    }
                    RataRk.Text = pj_Rk.ToString();
                    StanZadluzenia.Text = RozliczenieKredytu[pj_n * pj_m, 2].ToString();
                    tabelkaRoz.Enabled = false;
                    this.TabCtrl.SelectedTab = tabelaryczneRoz;
                }
                else
                    if (stale.Checked)
                    {
                        float pj_Ro, pj_Rk, pj_R;
                        double pj_Z;
                        float[,] RozliczenieKredytu = new float[pj_n * pj_m + 1, 3];

                        RozliczenieKredytu[0, 0] = 0.0f;
                        RozliczenieKredytu[0, 1] = 0.0f;
                        RozliczenieKredytu[0, 2] = pj_K;
                        pj_Z = pj_K; pj_Ro = 0.0f;
                        double pj_x = pj_p / pj_m;
                        double pj_Y = Math.Pow(1 + pj_x, pj_n * pj_m);
                        pj_R = 0.0f;
                        float KosztKredytu = 0.0f;
                        for (int i = 0; i < RozliczenieKredytu.GetLength(0); i++)
                        {
                            RozliczenieKredytu[i, 0] = pj_R;
                            RozliczenieKredytu[i, 1] = pj_Ro;
                            RozliczenieKredytu[i, 2] = (float)pj_Z;
                            pj_R = pj_K * (float)((pj_x * pj_Y) / (pj_Y - 1));
                            pj_Ro =(float) pj_Z * pj_p / pj_m;
                            pj_Rk = pj_R - pj_Ro;
                            pj_Z = pj_Z - pj_Rk;
                            KosztKredytu += pj_Ro;
                        }
                        kosztKredytu.Text = KosztKredytu.ToString();
                        for (int i = 0; i < RozliczenieKredytu.GetLength(0); i++)
                        {
                            dvgRozliczenieKredytu.Rows.Add();
                            dvgRozliczenieKredytu.Rows[i].Cells[0].Value = i;
                            dvgRozliczenieKredytu.Rows[i].Cells[1].Value = RozliczenieKredytu[i, 0];
                            dvgRozliczenieKredytu.Rows[i].Cells[2].Value = RozliczenieKredytu[i, 1];
                            dvgRozliczenieKredytu.Rows[i].Cells[3].Value = RozliczenieKredytu[i, 2];
                        }
                        RataRk.Text = "różne wartości";
                        StanZadluzenia.Text = RozliczenieKredytu[pj_n * pj_m, 2].ToString();
                        tabelkaRoz.Enabled = false;
                        this.TabCtrl.SelectedTab = tabelaryczneRoz;
                    }
            else errorProvider1.SetError(groupBox2, "ERROR: Musisz wybrać opcję");
        }

        private void wykresRoz_Click(object sender, EventArgs e)
        {
            float pj_K, pj_p;
            uint pj_n, pj_m;
            if (!PobierzDane(out pj_K, out pj_n, out pj_p, out pj_m))
                return;
            grpWyborLiniiWykresu.Visible = true;
            if (malejace.Checked)
            {
                float pj_Ro, pj_Rk, pj_R;
                double pj_Z;
                float[,] RozliczenieKredytu = new float[pj_n * pj_m + 1, 3];

                RozliczenieKredytu[0, 0] = 0.0f;
                RozliczenieKredytu[0, 1] = 0.0f;
                RozliczenieKredytu[0, 2] = pj_K;
                pj_Z = pj_K; pj_Rk = pj_K / (pj_n * pj_m); pj_Ro = 0.0f; pj_R = 0.0f;
                float KosztKredytu = 0.0f;
                for (int i = 0; i < RozliczenieKredytu.GetLength(0); i++)
                {
                    RozliczenieKredytu[i, 0] = pj_R;
                    RozliczenieKredytu[i, 1] = pj_Ro;
                    RozliczenieKredytu[i, 2] = (float)pj_Z;
                    pj_Ro = (float)pj_Z * pj_p / pj_m;
                    pj_R = pj_Rk + pj_Ro;
                    pj_Z = pj_Z - pj_Rk;
                    KosztKredytu += pj_Ro;
                }
                kosztKredytu.Text = KosztKredytu.ToString();
                StanZadluzenia.Text = RozliczenieKredytu[pj_m * pj_n, 2].ToString();

                wykresKredytu.Titles.Add("Spłata kredytu w ratach malejących");
                wykresKredytu.Legends.FindByName("Legend1").Docking = Docking.Bottom;
                wykresKredytu.Series[0].Name = "Rata łączna R";
                wykresKredytu.Series[0].ChartType = SeriesChartType.Line;
                wykresKredytu.Series[0].Color = Color.Blue;
                wykresKredytu.Series[0].BorderDashStyle = ChartDashStyle.Solid;
                wykresKredytu.Series[0].BorderWidth = 2;
                int[] NumerRatyKredytu = new int[RozliczenieKredytu.GetLength(0) - 1];
                for (int i = 0; i < RozliczenieKredytu.GetLength(0)-1; i++)
                {
                    NumerRatyKredytu[i] = i + 1;
                }
                float[] PunktyWykresu = new float[RozliczenieKredytu.GetLength(0) - 1];
                for (int i = 0; i < RozliczenieKredytu.GetLength(0)-1; i++)
                {
                    PunktyWykresu[i] = RozliczenieKredytu[i + 1, 0];
                }
                wykresKredytu.Series[0].Points.DataBindXY(NumerRatyKredytu, PunktyWykresu);
                wykresKredytu.Series.Add("Seria 1");
                wykresKredytu.Series[1].Name = "Rata odsetkowa Ro";
                wykresKredytu.Series[1].ChartType = SeriesChartType.Line;
                wykresKredytu.Series[1].Color = Color.Red;
                wykresKredytu.Series[1].BorderDashStyle = ChartDashStyle.Dash;
                wykresKredytu.Series[1].BorderWidth = 2;
                for (int i = 0; i < RozliczenieKredytu.GetLength(0)-1; i++)
                {
                    PunktyWykresu[i] = RozliczenieKredytu[i + 1, 1];
                }
                wykresKredytu.Series[1].Points.DataBindXY(NumerRatyKredytu, PunktyWykresu);
                wykresKredytu.Series.Add("Seria 2");
                wykresKredytu.Series[2].Name = "Rata kapitałowa Rk";
                wykresKredytu.Series[2].ChartType = SeriesChartType.Line;
                wykresKredytu.Series[2].Color = Color.Green;
                wykresKredytu.Series[2].BorderDashStyle = ChartDashStyle.Dot;

                for (int i = 0; i < RozliczenieKredytu.GetLength(0) - 1; i++)
                {
                    PunktyWykresu[i] = pj_Rk;
                }
                wykresKredytu.Series[2].Points.DataBindXY(NumerRatyKredytu, PunktyWykresu);
                wykresRoz.Enabled = false;
                this.TabCtrl.SelectedTab = graphRoz;
            }
            if (stale.Checked)
            {
                float pj_Ro, pj_Rk, pj_R;
                double pj_Z;
                float[,] RozliczenieKredytu = new float[pj_n * pj_m + 1, 3];

                RozliczenieKredytu[0, 0] = 0.0f;
                RozliczenieKredytu[0, 1] = 0.0f;
                RozliczenieKredytu[0, 2] = pj_K;
                pj_Z = pj_K; pj_Ro = 0.0f;
                double pj_x = pj_p / pj_m;
                double pj_Y = Math.Pow(1 + pj_x, pj_n * pj_m);
                pj_R = 0.0f;
                float KosztKredytu = 0.0f;
                for (int i = 0; i < RozliczenieKredytu.GetLength(0); i++)
                {
                    RozliczenieKredytu[i, 0] = pj_R;
                    RozliczenieKredytu[i, 1] = pj_Ro;
                    RozliczenieKredytu[i, 2] = (float)pj_Z;
                    pj_R = pj_K * (float)((pj_x * pj_Y) / (pj_Y - 1));
                    pj_Ro = (float)pj_Z * pj_p / pj_m;
                    pj_Rk = pj_R - pj_Ro;
                    pj_Z = pj_Z - pj_Rk;
                    KosztKredytu += pj_Ro;
                }
                kosztKredytu.Text = KosztKredytu.ToString();
                StanZadluzenia.Text = RozliczenieKredytu[pj_m * pj_n, 2].ToString();

                wykresKredytu.Titles.Add("Spłata kredytu w ratach stałych");
                wykresKredytu.Legends.FindByName("Legend1").Docking = Docking.Bottom;
                wykresKredytu.Series[0].Name = "Rata łączna R";
                wykresKredytu.Series[0].ChartType = SeriesChartType.Line;
                wykresKredytu.Series[0].Color = Color.Blue;
                wykresKredytu.Series[0].BorderDashStyle = ChartDashStyle.Solid;
                wykresKredytu.Series[0].BorderWidth = 2;
                int[] NumerRatyKredytu = new int[RozliczenieKredytu.GetLength(0) - 1];
                for (int i = 0; i < RozliczenieKredytu.GetLength(0) - 1; i++)
                {
                    NumerRatyKredytu[i] = i + 1;
                }
                float[] PunktyWykresu = new float[RozliczenieKredytu.GetLength(0) - 1];
                for (int i = 0; i < RozliczenieKredytu.GetLength(0) - 1; i++)
                {
                    PunktyWykresu[i] = RozliczenieKredytu[i + 1, 0];
                }
                wykresKredytu.Series[0].Points.DataBindXY(NumerRatyKredytu, PunktyWykresu);
                wykresKredytu.Series.Add("Seria 1");
                wykresKredytu.Series[1].Name = "Rata odsetkowa Ro";
                wykresKredytu.Series[1].ChartType = SeriesChartType.Line;
                wykresKredytu.Series[1].Color = Color.Red;
                wykresKredytu.Series[1].BorderDashStyle = ChartDashStyle.Dash;
                wykresKredytu.Series[1].BorderWidth = 2;
                for (int i = 0; i < RozliczenieKredytu.GetLength(0) - 1; i++)
                {
                    PunktyWykresu[i] = RozliczenieKredytu[i + 1, 1];
                }
                wykresKredytu.Series[1].Points.DataBindXY(NumerRatyKredytu, PunktyWykresu);
                wykresKredytu.Series.Add("Seria 2");
                wykresKredytu.Series[2].Name = "Rata kapitałowa Rk";
                wykresKredytu.Series[2].ChartType = SeriesChartType.Line;
                wykresKredytu.Series[2].Color = Color.Green;
                wykresKredytu.Series[2].BorderDashStyle = ChartDashStyle.Dot;

                for (int i = 0; i < RozliczenieKredytu.GetLength(0) - 1; i++)
                {
                    PunktyWykresu[i] = RozliczenieKredytu[i + 1, 0] - RozliczenieKredytu[i + 1, 1];
                }
                wykresKredytu.Series[2].Points.DataBindXY(NumerRatyKredytu, PunktyWykresu);
                wykresRoz.Enabled = false;
                this.TabCtrl.SelectedTab = graphRoz;
            }
            if (rosnace.Checked)
            {
                float pj_Ro, pj_Rk, pj_R;
                double pj_Z;
                float[,] RozliczenieKredytu = new float[pj_n * pj_m + 1, 3];
                RozliczenieKredytu[0, 0] = 0.0f;
                RozliczenieKredytu[0, 1] = 0.0f;
                RozliczenieKredytu[0, 2] = pj_K;
                pj_Z = pj_K; pj_Rk = pj_K / (pj_n * pj_m); pj_Ro = 0.0f; pj_R = 0.0f;

                float KosztKredytu = 0.0f;
                for (int i = 0; i < RozliczenieKredytu.GetLength(0); i++)
                {
                    RozliczenieKredytu[i, 0] = pj_R;
                    RozliczenieKredytu[i, 1] = pj_Ro;
                    RozliczenieKredytu[i, 2] = (float)pj_Z;
                    if (i >= 1)
                    {
                        pj_Ro = i * pj_Rk * pj_p / pj_m;
                    }

                    pj_R = pj_Rk + pj_Ro;
                    pj_Z = pj_Z - pj_Rk;
                    KosztKredytu += pj_Ro;
                }
                kosztKredytu.Text = KosztKredytu.ToString();
                StanZadluzenia.Text = RozliczenieKredytu[pj_m * pj_n, 2].ToString();

                wykresKredytu.Titles.Add("Spłata kredytu w ratach rosnących");
                wykresKredytu.Legends.FindByName("Legend1").Docking = Docking.Bottom;
                wykresKredytu.Series[0].Name = "Rata łączna R";
                wykresKredytu.Series[0].ChartType = SeriesChartType.Line;
                wykresKredytu.Series[0].Color = Color.Blue;
                wykresKredytu.Series[0].BorderDashStyle = ChartDashStyle.Solid;
                wykresKredytu.Series[0].BorderWidth = 2;
                int[] NumerRatyKredytu = new int[RozliczenieKredytu.GetLength(0) - 1];
                for (int i = 0; i < RozliczenieKredytu.GetLength(0) - 1; i++)
                {
                    NumerRatyKredytu[i] = i + 1;
                }
                float[] PunktyWykresu = new float[RozliczenieKredytu.GetLength(0) - 1];
                for (int i = 0; i < RozliczenieKredytu.GetLength(0) - 1; i++)
                {
                    PunktyWykresu[i] = RozliczenieKredytu[i + 1, 0];
                }
                wykresKredytu.Series[0].Points.DataBindXY(NumerRatyKredytu, PunktyWykresu);
                wykresKredytu.Series.Add("Seria 1");
                wykresKredytu.Series[1].Name = "Rata odsetkowa Ro";
                wykresKredytu.Series[1].ChartType = SeriesChartType.Line;
                wykresKredytu.Series[1].Color = Color.Red;
                wykresKredytu.Series[1].BorderDashStyle = ChartDashStyle.Dash;
                wykresKredytu.Series[1].BorderWidth = 2;
                for (int i = 0; i < RozliczenieKredytu.GetLength(0) - 1; i++)
                {
                    PunktyWykresu[i] = RozliczenieKredytu[i + 1, 1];
                }
                wykresKredytu.Series[1].Points.DataBindXY(NumerRatyKredytu, PunktyWykresu);
                wykresKredytu.Series.Add("Seria 2");
                wykresKredytu.Series[2].Name = "Rata kapitałowa Rk";
                wykresKredytu.Series[2].ChartType = SeriesChartType.Line;
                wykresKredytu.Series[2].Color = Color.Green;
                wykresKredytu.Series[2].BorderDashStyle = ChartDashStyle.Dot;

                for (int i = 0; i < RozliczenieKredytu.GetLength(0) - 1; i++)
                {
                    PunktyWykresu[i] = RozliczenieKredytu[i + 1, 0] - RozliczenieKredytu[i + 1, 1];
                }
                wykresKredytu.Series[2].Points.DataBindXY(NumerRatyKredytu, PunktyWykresu);
                wykresRoz.Enabled = false;
                this.TabCtrl.SelectedTab = graphRoz;
            }
        }

        private void kolorLiniiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog OknoKolorow = new ColorDialog();
            if (OknoKolorow.ShowDialog() == DialogResult.OK)
            {
                if (RataLaczna.Checked)
                    wykresKredytu.Series[0].Color = OknoKolorow.Color;
                else
                    if (RataOdsetkowa.Checked)
                        wykresKredytu.Series[1].Color = OknoKolorow.Color;
                    else
                        if (RataKapitalowa.Checked)
                            wykresKredytu.Series[2].Color = OknoKolorow.Color;
            }
        }

        private void zwiększGrubośćLiniiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (RataLaczna.Checked)
                wykresKredytu.Series[0].BorderWidth++;
            else
                if (RataOdsetkowa.Checked)
                wykresKredytu.Series[1].BorderWidth++;
                else
                    if (RataKapitalowa.Checked)
                        wykresKredytu.Series[2].BorderWidth++;
        }

        private void zmiejszGrubośćLiniiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (RataLaczna.Checked)
            { 
                if(wykresKredytu.Series[0].BorderWidth>1)
                    wykresKredytu.Series[0].BorderWidth--;
            }
                
            else
                if (RataOdsetkowa.Checked)
                { 
                    if (wykresKredytu.Series[1].BorderWidth > 1)
                        wykresKredytu.Series[1].BorderWidth--;
                }
                    
                else
                    if (RataKapitalowa.Checked)
                    { 
                        if (wykresKredytu.Series[2].BorderWidth > 1)
                            wykresKredytu.Series[2].BorderWidth--;
                    }
                        
        }

        private void solidToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (RataLaczna.Checked)
                wykresKredytu.Series[0].BorderDashStyle = ChartDashStyle.Solid;
            else
                if (RataOdsetkowa.Checked)
                    wykresKredytu.Series[1].BorderDashStyle = ChartDashStyle.Solid;
                else
                    if (RataKapitalowa.Checked)
                        wykresKredytu.Series[2].BorderDashStyle = ChartDashStyle.Solid;
        }

        private void dashToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (RataLaczna.Checked)
                wykresKredytu.Series[0].BorderDashStyle = ChartDashStyle.Dash;
            else
                if (RataOdsetkowa.Checked)
                    wykresKredytu.Series[1].BorderDashStyle = ChartDashStyle.Dash;
                else
                    if (RataKapitalowa.Checked)
                        wykresKredytu.Series[2].BorderDashStyle = ChartDashStyle.Dash;
        }

        private void dotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (RataLaczna.Checked)
                wykresKredytu.Series[0].BorderDashStyle = ChartDashStyle.Dot;
            else
                if (RataOdsetkowa.Checked)
                    wykresKredytu.Series[1].BorderDashStyle = ChartDashStyle.Dot;
                else
                    if (RataKapitalowa.Checked)
                        wykresKredytu.Series[2].BorderDashStyle = ChartDashStyle.Dot;
        }

        private void dashDotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (RataLaczna.Checked)
                wykresKredytu.Series[0].BorderDashStyle = ChartDashStyle.DashDot;
            else
                if (RataOdsetkowa.Checked)
                    wykresKredytu.Series[1].BorderDashStyle = ChartDashStyle.DashDot;
                else
                    if (RataKapitalowa.Checked)
                        wykresKredytu.Series[2].BorderDashStyle = ChartDashStyle.DashDot;
        }

        private void Kredyty_Jaskula44185_FormClosing(object sender, FormClosingEventArgs e)
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

        private void reset_Click(object sender, EventArgs e)
        {
            this.TabCtrl.SelectedTab = pulpitRoz;
            dvgRozliczenieKredytu.Rows.Clear();
            grpWyborLiniiWykresu.Visible = false;
            wysokoscKredytu.Text = "";
            okresSplaty.Text = "";
            rocznaStopaProc.Text = "Wybierz";
            razWRoku.Checked = false;
            coPolRoku.Checked = false;
            coKwartal.Checked = false;
            coMiesiac.Checked = false;
            malejace.Checked = false;
            stale.Checked = false;
            rosnace.Checked = false;
            tabelkaRoz.Enabled = true;
            wykresRoz.Enabled = true;
            kosztKredytu.Text = "";
            StanZadluzenia.Text = "";
            RataRk.Text = "";

        }

        private void wyjścieZProgramuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void zamknięcieFormularzaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

       

    }
}
