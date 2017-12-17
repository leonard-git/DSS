﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class FormTuVan : Form
    {
        public FormTuVan()
        {
            InitializeComponent();
        }
        string id;
        string nam;
        string loi = "Lỗi";
        private string ketnoicsdl(string id, string nam)
        {
            // Lấy dữ liệu trường theo mã trường
            string sql = "select Truong.TenTruong, TuyenSinh.* from Truong,TuyenSinh where TuyenSinh.MaTruong='" + id + "' AND Truong.MaTruong='" + id + "' AND TuyenSinh.Nam = '" + nam + "'";
            try
            {
                DataTable thamchieu = ExcuteSql.connectDB(sql);

                if (thamchieu.Rows.Count == 0)
                {
                    return null;
                }
                else
                {
                    return thamchieu.Rows[0][4].ToString();
                }
            }
            catch
            {
                MessageBox.Show("Thông tin nhập không hợp lệ");
                return loi;
            }

        }
        private DataTable ketnoicsdlMa(string id)
        {
            string sql = "select Truong.TenTruong, TuyenSinh.* from Truong,TuyenSinh where TuyenSinh.MaTruong='" + id + "' AND Truong.MaTruong='" + id + "'";
            return ExcuteSql.connectDB(sql);
            
        }
        private DataTable ketnoicsdlTen(string id)
        {
            string sql = "select MaTruong from Truong where TenTruong = N'" + id + "'";
            return ExcuteSql.connectDB(sql);
        }
        private void buttonDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormTuVan_Load(object sender, EventArgs e)
        {
            comboBoxTuVan.SelectedIndex = 0;
        }

        private void textBoxTruong_TextChanged(object sender, EventArgs e)
        {
            /*String sql;
            if (comboBoxTuVan.SelectedIndex == 0)
            {

                sql = "select * from Truong where MaTruong like '%" + textBoxTruong.Text + "%'";  // lay het du lieu trong bang truong                
            }
            else
            {
                sql = "select * from Truong where TenTruong like N'%" + textBoxTruong.Text + "%'";  // lay het du lieu trong bang truong 
            }

            SqlDataReader dr = ExcuteSql.excuteSqlReader(sql);

            AutoCompleteStringCollection autoCom = new AutoCompleteStringCollection();
            while (dr.Read())
            {
                if (comboBoxTuVan.SelectedIndex == 0)
                    autoCom.Add(dr.GetString(0));
                else
                    autoCom.Add(dr.GetString(1));
            }
            
            textBoxTruong.AutoCompleteCustomSource = autoCom;

            dr.Close();
            */

        }
        private string ketqua(string id, string nam)
        {
            if(string.IsNullOrEmpty(id) && string.IsNullOrEmpty(nam))
                MessageBox.Show("Chưa nhập đủ thông tin để tư vấn");
            else if (string.IsNullOrEmpty(id))
                MessageBox.Show("Chưa nhập mã trường hoặc tên trường");
            else if (string.IsNullOrEmpty(nam))
                MessageBox.Show("Chưa nhập năm dự báo");
            else
            {
                String ketqua = ketnoicsdl(id, nam);
                if (ketqua == null)
                {
                    DataTable tb = ketnoicsdlMa(id);
                    if (tb.Rows.Count != 0)
                    {
                        int n = tb.Rows.Count;
                        double sl, a = 0, dudoanM = int.Parse(tb.Rows[0][4].ToString()), dudoanC = int.Parse(tb.Rows[0][4].ToString());
                        for (int i = 2; i <= n; i++)
                        {
                            sl = int.Parse(tb.Rows[i - 1][4].ToString());
                            dudoanM += (sl - dudoanC);
                            if (sl - dudoanC != 0)
                                a += (dudoanM - dudoanC) / (sl - dudoanC);
                            else
                                a += 0;
                            dudoanC = dudoanM;
                        }
                        a = a / n;
                        if (a != 0)
                        {
                            dudoanC = int.Parse(tb.Rows[0][4].ToString());
                            sl = int.Parse(tb.Rows[1][4].ToString());
                            for (int i = 2; i <= (int.Parse(nam) - 2013 + 1); i++)
                            {
                                dudoanM = dudoanC + a * (sl - dudoanC);
                                dudoanC = dudoanM;
                                try
                                {
                                    sl = int.Parse(tb.Rows[i - 1][4].ToString());
                                }
                                catch
                                {
                                    sl += (dudoanM - dudoanC);
                                }

                            }
                        }
                        return dudoanM.ToString();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy Trường như yêu cầu.");
                        return null;
                    }
                }
                else if( ketqua != loi)
                {
                    return ketqua;
                }
            }
            return null;
        }

        private void buttonTuVan_Click(object sender, EventArgs e)
        {
            nam = textBoxNam.Text;
            if (comboBoxTuVan.SelectedIndex == 0)
            {
                id = textBoxTruong.Text;
            }
            else
            {
                string id1 = textBoxTruong.Text;
                DataTable tb = ketnoicsdlTen(id1);
                id = tb.Rows[0][0].ToString();
            }
            labelKetQua.Text = ketqua(id, nam);
        }

    }
}
