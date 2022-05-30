using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Praktikum_Week_14_Jeffri_Lieca_H_0706022110016
{
    public partial class Form1 : Form
    {
        public static string sqlConnection = "server=localhost;uid=root;pwd=;database=premier_league";
        public MySqlConnection sqlConnect = new MySqlConnection(sqlConnection);
        public MySqlCommand sqlCommand;
        public MySqlDataAdapter sqlAdapter;

        public int counter = 0;
        public int lastData;
        public string timID;
        DataTable dtLoad = new DataTable();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Perbarui();
        }

        public void Perbarui()
        {
            string sqlQuery = "select t.team_name as TeamName, m.manager_name as ManagerName, n.nation as ManagerNation, t.home_stadium as Stadium, t.city as City, t.capacity as Capacity, t.team_id as ID from manager m, team t,nationality n where m.manager_id=t.manager_id and m.nationality_id=n.nationality_id;";
            sqlCommand = new MySqlCommand(sqlQuery, sqlConnect);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);
            DataTable dtLoad = new DataTable();
            sqlAdapter.Fill(dtLoad);
            labelTeamName.Text = dtLoad.Rows[counter][0].ToString();
            labelManager.Text = dtLoad.Rows[counter][1].ToString() + $" ({dtLoad.Rows[counter][2]})";
            labelStadium.Text = dtLoad.Rows[counter][3].ToString() + $", {dtLoad.Rows[counter][4].ToString()} ({dtLoad.Rows[counter][5]})";
            lastData = dtLoad.Rows.Count - 1;
            timID = dtLoad.Rows[counter][6].ToString();

            sqlQuery = "select p.player_name as TopName, gol.Goal+golpe.GoalPen as Goalya, golpe.GoalPen as Penalty from player p, (select d.player_id as id, sum(if(d.type='GO',1,0)) as Goal from dmatch d group by 1) gol, (select d.player_id as id,sum(if(d.type='GP',1,0)) as GoalPen from dmatch d group by 1) golpe where p.player_id=gol.id and gol.id=golpe.id and p.team_id='"+timID+"' order by 2 desc;";
            sqlCommand = new MySqlCommand(sqlQuery, sqlConnect);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);
            DataTable dtLoadGol = new DataTable();
            sqlAdapter.Fill(dtLoadGol);
            labelTopScorer.Text = dtLoadGol.Rows[0][0].ToString() + $" {dtLoadGol.Rows[0][1].ToString()}({dtLoadGol.Rows[0][2].ToString()})";
        }

        private void buttonFirst_Click(object sender, EventArgs e)
        {
            counter = 0;
            Perbarui();
        }

        private void buttonPrev_Click(object sender, EventArgs e)
        {
            if (counter == 0)
            {
                MessageBox.Show("Sudah Data Pertama");
            }
            else
            {
                counter--;
                Perbarui();
            }
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (counter == lastData)
            {
                MessageBox.Show("Sudah Data Terakhir");
            }
            else
            {
                counter++;
                Perbarui();
            }
        }

        private void buttonLast_Click(object sender, EventArgs e)
        {
            counter = lastData;
            Perbarui();
        }
    }
}
