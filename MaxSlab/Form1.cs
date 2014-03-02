using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MaxSDll;

namespace MaxSlab
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private double[,] LeftParser(string s)
        {
            var firstCol = new List<List<double>>();
            var afterFirst = false;
            var parts = s.Split('{');


            var y = 0;
            foreach (var part in parts.Where(part => !string.IsNullOrEmpty(part)))
            {
                if (!string.IsNullOrEmpty(part))
                {
                    var nums = part.Split(',').Select(t => t.Trim(' ').Trim('}'));
                    firstCol.Add(new List<double>());
                    foreach (var num in nums.Where(num => !string.IsNullOrEmpty(num)))
                    {
                        firstCol[y].Add(Convert.ToDouble(num, ifp));
                    }
                    y++;
                }
            }
            var o = new double[firstCol.Count, firstCol[0].Count];

            for (var i = 0; i < firstCol.Count; i++)
            {
                for (var j = 0; j < firstCol[0].Count; j++)
                {
                    o[i, j] = firstCol[i][j];
                }
            }

            return o;
        }
        //{{1.7,0.8,0.9},{0.8,0.7,0.3},{0.9,0.3,1.7}}

        private IFormatProvider ifp;

        private void textBox1_TextChanged(object sender, EventArgs e) {
            label3.Text = string.Empty;
            double[,] m = null;
            try {
                m = LeftParser(textBox1.Text);
            }
            catch (Exception ex) {
                label3.Text = ex.ToString();
            }
            MaxSdResult s = new MaxSdResult();
            try {
                label2.Text = MaxSd.Method(m, out s).ToString(CultureInfo.InvariantCulture);
            }
            catch (Exception ex) {
                label3.Text += Environment.NewLine + ex;
            }

            label1.Text = ResultToText(s);
        }

        private string ResultToText(MaxSdResult r) {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Максимальное собственное");
            sb.AppendLine(r.SobstvMax.ToString(CultureInfo.InvariantCulture));
            sb.AppendLine("Соответствующий собственный вектор");
            sb.Append("(");
            if(r.SobstvVector != null)
            foreach (var p in r.SobstvVector) {
                sb.Append(p.ToString(CultureInfo.InvariantCulture));
                if (p != r.SobstvVector.Last()) {
                    sb.Append(", ");
                }
            }
            sb.AppendLine(")");

            sb.AppendLine("Второе собственное");
            sb.AppendLine(r.Sobstv2.ToString(CultureInfo.InvariantCulture));
            return sb.ToString();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            wrongSyntaxLabel.Visible = false;
            textBox1_TextChanged(null, null);
            ifp = new CultureInfo("en-US");
        }
    }
}
