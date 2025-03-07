﻿using System;

using System.Collections.Generic;

using System.ComponentModel;

using System.Data;

using System.Drawing;

using System.Drawing.Drawing2D;//用于优化绘制的结果

using System.Linq;

using System.Text;

using System.Threading.Tasks;

using System.Windows.Forms;

using Microsoft.ML.OnnxRuntime;

using System.Numerics.Tensors;
using System.Media;




namespace Mnistwf

{

    public partial class Form1 : Form

    {

        public Form1()

        {

            InitializeComponent();
           
        }



        private Bitmap digitImage;//用来保存手写数字

        private Point startPoint;//用于绘制线段，作为线段的初始端点坐标

        //private Mnist model;//用于识别手写数字

        private const int MnistImageSize = 28;//Mnist模型所需的输入图片大小



        private void Form1_Load(object sender, EventArgs e)

        {

            //当窗口加载时，绘制一个白色方框

            //model = new Mnist();

            digitImage = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            Graphics g = Graphics.FromImage(digitImage);

            g.Clear(Color.White);

            pictureBox1.Image = digitImage;

        }



        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)

        {

            //当鼠标左键被按下时，记录下需要绘制的线段的起始坐标

            startPoint = (e.Button == MouseButtons.Left) ? e.Location : startPoint;
            startPoint = (e.Button == MouseButtons.Right) ? e.Location : startPoint;
        }



        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)

        {

            //当鼠标在移动，且当前处于绘制状态时，根据鼠标的实时位置与记录的起始坐标绘制线段，同时更新需要绘制的线段的起始坐标

            if (e.Button == MouseButtons.Left)

            {

                Graphics g = Graphics.FromImage(digitImage);

                Pen myPen = new Pen(Color.BlueViolet, 40);

                myPen.StartCap = LineCap.Round;

                myPen.EndCap = LineCap.Round;

                g.DrawLine(myPen, startPoint, e.Location);

                pictureBox1.Image = digitImage;

                g.Dispose();

                startPoint = e.Location;

            }
            if (e.Button == MouseButtons.Right)

            {
                Cursor = Cursors.No;
                Graphics g = Graphics.FromImage(digitImage);
                Pen myPen = new Pen(Color.White, 40);
                myPen.StartCap = LineCap.Round;
                myPen.EndCap = LineCap.Round;
                g.DrawLine(myPen, startPoint, e.Location);
                pictureBox1.Image = digitImage;
                g.Dispose();
                startPoint = e.Location;
            }
        }



        private void button1_Click(object sender, EventArgs e)

        {

            //当点击清除时，重新绘制一个白色方框，同时清除label1显示的文本

            digitImage = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            Graphics g = Graphics.FromImage(digitImage);

            //g.Clear(Color.Wheat);

            pictureBox1.Image = digitImage;

            label1.Text = "";

        }



        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)

        {

            //当鼠标左键释放时

            //开始处理图片进行推理

            if (e.Button == MouseButtons.Left)

            {

                Bitmap digitTmp = (Bitmap)digitImage.Clone();//复制digitImage

                //调整图片大小为Mnist模型可接收的大小：28×28

                using (Graphics g = Graphics.FromImage(digitTmp))

                {

                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                    g.DrawImage(digitTmp, 0, 0, MnistImageSize, MnistImageSize);

                }



                //将图片转为灰阶图，并将图片的像素信息保存在list中

                float[] imageArray = new float[MnistImageSize * MnistImageSize];

                for (int y = 0; y < MnistImageSize; y++)

                {

                    for (int x = 0; x < MnistImageSize; x++)

                    {

                        var color = digitTmp.GetPixel(x, y);

                        var a = (float)(0.5 - (color.R + color.G + color.B) / (3.0 * 255));



                        imageArray[y * MnistImageSize + x] = a;



                    }

                }



                // 设置要加载的模型的路径，跟据需要改为你的模型名称

                string modelPath = AppDomain.CurrentDomain.BaseDirectory + "mnist.onnx";



                using (var session = new InferenceSession(modelPath))

                {

                    var inputMeta = session.InputMetadata;

                    var container = new List<NamedOnnxValue>();





                    // 用Netron看到需要的输入类型是float32[1, 1, 28, 28]

                    // 第一维None表示可以传入多张图片进行推理

                    // 这里只使用一张图片，所以使用的输入数据尺寸为[1, 1, 28, 28]

                    var shape = new int[] { 1, 1, MnistImageSize, MnistImageSize };

                    var tensor = new DenseTensor<float>(imageArray, shape);



                    // 支持多个输入，对于mnist模型，只需要一个输入，输入的名称是input3

                    container.Add(NamedOnnxValue.CreateFromTensor<float>("Input3", tensor));



                    // 推理

                    var results = session.Run(container);



                    // 输出结果: Plus214_Output_0

                    IList<float> imageList = results.FirstOrDefault(item => item.Name == "Plus214_Output_0").AsTensor<float>().ToList();



                    // Query to check for highest probability digit

                    var maxIndex = imageList.IndexOf(imageList.Max());



                    // Display the results

                    label1.Text = maxIndex.ToString();





                }







            }



        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button3_BackgroundImageChanged(object sender, EventArgs e)
        {

        }
        interface Command
        {
            void execute();
            void undo();
        }
        Stack<Command> undoStack = new Stack<Command>();
        Stack<Command> redoStack = new Stack<Command>();

        public object CommonFunc { get; private set; }

        private void button4_Click(object sender, EventArgs e)
        {
            
        }

            private void button5_Click(object sender, EventArgs e)
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer();
            player.SoundLocation = @"D:\l\Hero.wav";
            player.Load();
            player.Play();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SoundPlayer player = new SoundPlayer("D:\\l\\ten.wav");
            bool isPlaying = false;
            if (isPlaying)
                player.Stop();
            else
                player.Play();
            player.Play();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
           
        }

        private void button9_Click(object sender, EventArgs e)
        {
          
            {
                if (this.WindowState == FormWindowState.Maximized) //若最大化
                {
                    this.WindowState = FormWindowState.Normal; //则正常化
                }
                else if (this.WindowState == FormWindowState.Normal) //若正常化
                {
                    this.WindowState = FormWindowState.Maximized; //则最大化
                }
            }
        }

        private void textBox1_TextChanged_2(object sender, EventArgs e)
        {

        }
    }
}
 