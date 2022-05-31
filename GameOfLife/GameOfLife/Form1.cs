using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GameOfLife
{
    public partial class Form1 : Form
    {
        int dopForN;
        int dopForM;

        int mapSizeN;
        int mapSizeM;

        int[,] currentState;
        int[,] nextState;

        int cellSize = 30;

        bool isPlay = false;
        bool isprimenit = false;

        Button[,] cells;

        Timer mainTimer;

        Random rnd = new Random();

        public Form1()
        {
            InitializeComponent();
            this.button2.Enabled = true;
            this.button3.Enabled = false;
            this.button4.Enabled = false;
            this.button1.Enabled = false;
        }

        /*void SetFormSize(int N, int M) 
        {
            this.Width  = (N + 1) * cellSize;
            this.Height = (M + 1) * cellSize;
        }
        */

        private void Init()
        {
            
            isPlay = false;
            mainTimer = new Timer();
            mainTimer.Interval = 200;
            mainTimer.Tick += new EventHandler(UpdateStates);

            currentState = new int[mapSizeN, mapSizeM];
            nextState = new int[mapSizeN, mapSizeM];
            cells = new Button[mapSizeN, mapSizeM];

            currentState = InitMap();
            nextState = InitMap();

            InitCelles();
        }

        void ClearGame()
        {
            isPlay = false;
            mainTimer = new Timer();
            mainTimer.Interval = 200;
            mainTimer.Tick += new EventHandler(UpdateStates);
            currentState = InitMap();
            nextState = InitMap();
            ResetCells();
        }

        void ResetCells()
        {
            for (int i = 0; i < mapSizeN; i++)
            {
                for (int j = 0; j < mapSizeM; j++)
                {
                    cells[i, j].BackColor = Color.Black;
                }
            }
        }

        private void UpdateStates(object sender, EventArgs e)
        {
            CalculateNextState();
            DisplayMap();
            if (CheckGenerationDead())
            {
                mainTimer.Stop();
                MessageBox.Show("The end");
            }
        }

        bool CheckGenerationDead()
        {
            for (int i = 0; i < mapSizeN; i++)
            {
                for (int j = 0; j < mapSizeM; j++)
                {
                    if (currentState[i, j] == 1)
                        return false;
                }
            }
            return true;
        }

        void DisplayMap()
        {
            for (int i = 0; i < mapSizeN; i++)
            {
                for (int j = 0; j < mapSizeM; j++)
                {
                    if (currentState[i, j] == 1)
                    {
                        cells[i, j].BackColor = Color.White;
                    }
                    else
                    {
                        cells[i, j].BackColor = Color.Black;
                    }
                }
            }
        } // Отрисовка цветов текущего поколения

        void CalculateNextState()
        {
            for (int i = 0; i < mapSizeN; i++)
            {
                for (int j = 0; j < mapSizeM; j++)
                {
                    var CountNeighb = CountNeighboors(i, j);
                    if (currentState[i, j] == 0 && CountNeighb == 3)
                    {
                        nextState[i, j] = 1;
                    }
                    else if (currentState[i, j] == 1 && (CountNeighb < 2 || CountNeighb > 3))
                    {
                        nextState[i, j] = 0;
                    }
                    else if (currentState[i, j] == 1 && (CountNeighb >= 2 || CountNeighb <= 3))
                    {
                        nextState[i, j] = 1;
                    }
                    else
                    {
                        nextState[i, j] = 0;
                    }
                }
            }
            bool proverkaPovtoreniya = false;


            for (int i = 0; i < mapSizeN; i++)
            {
                for (int j = 0; j < mapSizeM; j++)
                {
                    if (currentState[i, j] != nextState[i, j])
                        proverkaPovtoreniya = true;
                }
            }

            currentState = nextState;
            nextState = InitMap();
            if (!proverkaPovtoreniya)
            {
                mainTimer.Stop();
                MessageBox.Show("The end");
            }
        } // Следующее поколение

        int CountNeighboors(int i, int j)
        {
            var count = 0;
            for (int k = i - 1; k <= i + 1; k++)
            {
                for (int l = j - 1; l <= j + 1; l++)
                {
                    if (!IsInsideMap(k, l))
                    {
                        continue;
                    }
                    if (k == i && j == l)
                    {
                        continue;
                    }
                    if (currentState[k, l] == 1)
                    {
                        count++;
                    }
                }
            }
            return count;
        }  // кол-во живых соседей

        bool IsInsideMap(int i, int j)
        {
            if (i < 0 || i >= mapSizeN || j < 0 || j >= mapSizeM)
            {
                return false;
            }
            return true;
        } // Проверка на границы

        private int[,] InitMap()
        {

            int[,] arr = new int[mapSizeN, mapSizeM];
            for (int i = 0; i < mapSizeN; i++)
            {
                for (int j = 0; j < mapSizeM; j++)
                {
                    arr[i, j] = 0;
                }
            }
            return arr;
        } // создание карты N x M 

        private void InitCelles()
        {
            for (int i = 0; i < mapSizeN; i++)
            {
                for (int j = 0; j < mapSizeM; j++)
                {
                    Button button = new Button();
                    button.Size = new Size(cellSize, cellSize);

                    button.BackColor = Color.Black;

                    button.Location = new Point(j * cellSize + 15, i * cellSize + 15);
                    button.Click += new EventHandler(OnCellClick);
                    this.Controls.Add(button);
                    cells[i, j] = button;
                }
            }
        }

        private void OnCellClick(object sender, EventArgs e)
        {
            var pressedButton = sender as Button;

            if (!isPlay)
            {
                var i = pressedButton.Location.Y / cellSize;
                var j = pressedButton.Location.X / cellSize;

                if (currentState[i, j] == 0)
                {
                    currentState[i, j] = 1;
                    cells[i, j].BackColor = Color.White;
                }
                else
                {
                    currentState[i, j] = 0;
                    cells[i, j].BackColor = Color.Black;
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            var textboxik = sender as TextBox;
            if (textboxik != null && textboxik.Text != "")
            {
                dopForN = Convert.ToInt32(value: textboxik.Text);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            var textboxik = sender as TextBox;
            if (textboxik != null && textboxik.Text != "")
            {
                dopForM = Convert.ToInt32(value: textboxik.Text);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.button3.Enabled = true;
            this.button2.Enabled = false;
            this.button4.Enabled = false;
            if (!isPlay)
            {
                isPlay = true;
                mainTimer.Start();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.button4.Enabled = true;
            this.button1.Enabled = true;
            

            for (int i = 0; i < mapSizeN; i++)
            {
                for (int j = 0; j < mapSizeN; j++)
                {
                    this.cells[i, j].Dispose();
                }
            
            }
            mapSizeN = dopForN;
            mapSizeM = dopForM;
            Init();
            isprimenit = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.button2.Enabled = true;
            this.button4.Enabled = false;
            this.button1.Enabled = false;
            if (isPlay)
            {
                mainTimer.Stop();
                ClearGame();
              //  isprimenit = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.button1.Enabled = true;
            if (!isprimenit && !isPlay)
            {
                ClearGame();
                var kol = rnd.Next(0, (mapSizeN * mapSizeM) / 2);

                  for (int k = 0; k <= kol; k++)
                  {
                    var i = rnd.Next(0, mapSizeN);
                    var j = rnd.Next(0, mapSizeM);

                    currentState[i, j] = 1;
                    cells[i, j].BackColor = Color.White;
                }
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }
    }
}