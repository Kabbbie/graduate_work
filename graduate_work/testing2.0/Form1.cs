﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;

namespace testing2._0
{
    public partial class Form1 : Form
    {
        #region Переменные
        

        Capture capweb;
        const int siteCount = 21;//Количество сайтов(сигналов) 
        int sqr = (int)Math.Ceiling(Math.Sqrt(siteCount));//Корень для определления квадрата вороного
        bool bln = false;//Пауза на видео
        int N;// проверка при чтение с сервера
        short[] t;
        short[][] g = new short[siteCount][];
        int z = 0, r = 0;
        double XMin_Ratio,YMin_Ratio;
        double WidthRatio=0.0, HeightRatio=0.0;
        int markerCordX=100, markerCordY=200;
        

        int last_yq;
        int last_xq;


        #endregion

        #region WEBCAM

        private void GetVideo(object sender, EventArgs e)
        {
            Image<Bgr, Byte> Kadr = capweb.QueryFrame().Resize(400, 300, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);  // Получаем кадр с вебкамеры
            
            Image<Gray, Byte> gray = Kadr.Convert<Gray, Byte>().PyrDown().PyrUp();   //Convert the image to grayscale and filter out the noise

            Gray cannyThreshold = new Gray(180);
            Gray cannyThresholdLinking = new Gray(120);
            Gray circleAccumulatorThreshold = new Gray(120);

            Image<Gray, Byte> cannyEdges = gray.Canny(cannyThreshold, cannyThresholdLinking);
            LineSegment2D[] lines = cannyEdges.HoughLinesBinary(
                1, //Distance resolution in pixel-related units
                Math.PI / 45.0, //Angle resolution measured in radians.
                20, //threshold
                30, //min Line width
                10 //gap between lines
                )[0]; //Get the lines from the first channel
           
            #region Find rectangles          
            List<MCvBox2D> boxList = new List<MCvBox2D>();

            using (MemStorage storage = new MemStorage()) //allocate storage for contour approximation
                for (Contour<Point> contours = cannyEdges.FindContours(); contours != null; contours = contours.HNext)
                {
                    Contour<Point> currentContour = contours.ApproxPoly(contours.Perimeter * 0.05, storage);

                    if (contours.Area > 150 && contours.Area < 1000) //only consider contours with area greater than 700 and lesser than 1200
                    {
                        if (currentContour.Total == 4) //The contour has 4 vertices.
                        {
                            #region determine if all the angles in the contour are within the range of [0, 180] degree
                            bool isRectangle = true;
                            Point[] pts = currentContour.ToArray();
                            LineSegment2D[] edges = PointCollection.PolyLine(pts, true);

                            for (int i = 0; i < edges.Length; i++)
                            {
                                double angle = Math.Abs(
                                   edges[(i + 1) % edges.Length].GetExteriorAngleDegree(edges[i]));
                                if (angle < 0 || angle > 180)
                                {
                                    isRectangle = false;
                                    break;
                                }
                            }
                            #endregion

                            if (isRectangle)
                            {
                                boxList.Add(currentContour.GetMinAreaRect());
                            }
                        }
                    }
                }
            #endregion
            #region draw triangles and rectangles
            Image<Bgr, Byte> triangleRectangleImage = Kadr.Copy();
            foreach (MCvBox2D box in boxList)
                Kadr.Draw(box, new Bgr(Color.DarkOrange), 3);

            #endregion
            List<MCvBox2D> triangleList = findTriangles(cannyEdges, Kadr);
            if (boxList.Count > 0)
            {
                
                int minX = Convert.ToInt32(boxList[0].center.X);
                int maxX = minX;
                int minY = Convert.ToInt32(boxList[0].center.Y);
                int maxY = minY;
                for (int i = 0; i < boxList.Count; i++)
                {
                    int a1 = Convert.ToInt32(boxList[i].center.X), a2 = Convert.ToInt32(boxList[i].center.Y);
                    if (minX > a1) minX = a1;
                    else if (maxX < a1) maxX = a1;

                    if (minY > a2) minY = a2;
                    else if (maxY < a2) maxY = a2;
                }


                int yq = maxY - minY;
                int xq = maxX - minX;
                
                int mX = trackBar1.Value;
                int mY = trackBar2.Value;
                int trX = trackBar3.Value;
                int trY = trackBar4.Value;
                int Opac = trackBar5.Value;
                
                if (xq > 0 && yq > 0)
                {
                    displayPictureWithOpacity(Kadr, minX, maxX, minY, maxY, mX, mY, trX, trY, Opac,triangleList);
                    foreach (MCvBox2D tri in triangleList)
                        Kadr.Draw(tri, new Bgr(Color.Green), 3);
                }
                last_yq = yq;
                last_xq = xq;
                
            }

            
            ib1.Image = Kadr;
        }
        List<MCvBox2D> findTriangles(Image<Gray, Byte> cannyEdges, Image<Bgr, Byte> Kadr) {
            #region Find rectangles          
            List<MCvBox2D> triangleList = new List<MCvBox2D>();

            using (MemStorage storage = new MemStorage()) //allocate storage for contour approximation
                for (Contour<Point> contours = cannyEdges.FindContours(); contours != null; contours = contours.HNext)
                {
                    Contour<Point> currentContour = contours.ApproxPoly(contours.Perimeter * 0.05, storage);

                    if (contours.Area > 150 && contours.Area < 1000) //only consider contours with area greater than 700 and lesser than 1200
                    {
                        if (currentContour.Total == 3) //The contour has 3 vertices.
                        {
                            #region determine if all the angles in the contour are within the range of [0, 180] degree
                            bool isTriangle = true;
                            Point[] pts = currentContour.ToArray();
                            LineSegment2D[] edges = PointCollection.PolyLine(pts, true);

                            for (int i = 0; i < edges.Length; i++)
                            {
                                double angle = Math.Abs(
                                   edges[(i + 1) % edges.Length].GetExteriorAngleDegree(edges[i]));
                                if (angle < 0 || angle > 180)
                                {
                                    isTriangle = false;
                                    break;
                                }
                            }
                            #endregion

                            if (isTriangle)
                            {
                                triangleList.Add(currentContour.GetMinAreaRect());
                            }
                        }
                    }
                }
            #endregion
            //#region draw triangles
            //Image<Bgr, Byte> triangleRectangleImage = Kadr.Copy();
            //foreach (MCvBox2D box in triangleList)
            //    Kadr.Draw(box, new Bgr(Color.Green), 3);

            //#endregion
            return triangleList;
        }
        Bitmap setImageOpacity(Image image, float opacity)
        {
            try
            {
                //create a Bitmap the size of the image provided  
                Bitmap bmp = new Bitmap(image.Width, image.Height);

                //create a graphics object from the image  
                using (Graphics gfx = Graphics.FromImage(bmp))
                {

                    //create a color matrix object  
                    ColorMatrix matrix = new ColorMatrix();

                    //set the opacity  
                    matrix.Matrix33 = opacity;

                    //create image attributes  
                    ImageAttributes attributes = new ImageAttributes();

                    //set the color(opacity) of the image  
                    attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                    //now draw the image  
                    gfx.DrawImage(image, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
                }
                return bmp;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        void displayPictureWithOpacity(Image<Bgr, Byte> frame, int startX,int endX, 
                                        int startY,int endY, 
                                        int multX, int multY, 
                                        int transX, int transY,
                                        int Opac,List<MCvBox2D> triangleList) {
            Bitmap frame_bitmap = frame.Bitmap;

            #region Resize
            int lengX = (endX - startX) * multX;
            int lengY = (endY - startY) * multY;
            #endregion
            Size size = new Size(lengX, lengY);
            Bitmap picture = new Bitmap(Image.FromFile("wado.png"), size);
            float opacity = 0.1f * Opac;
            picture = setImageOpacity(picture, opacity);
            Graphics temp= Graphics.FromImage(frame_bitmap);
            #region SomeWork
            if (WidthRatio > 0 && HeightRatio > 0)
            {
                
                int DeltaX = Convert.ToInt32(Math.Round(XMin_Ratio * lengX));
                int DeltaY = Convert.ToInt32(Math.Round(YMin_Ratio * lengY));
                startX = startX - DeltaX;
                startY = startY - DeltaY;
                lengX = Convert.ToInt32(Math.Round(lengX / WidthRatio));
                lengY = Convert.ToInt32(Math.Round(lengY / HeightRatio));
            }
            #endregion
            int tempCordX=0,tempCordY=0;
            if (triangleList.Count > 0) {
                if (Convert.ToInt32(triangleList[0].center.X) >= startX) {
                    tempCordX = startX - Convert.ToInt32(triangleList[0].center.X);
                    tempCordY = startY - Convert.ToInt32(triangleList[0].center.Y);
                }
            }
            double tempRatioX = (double)(tempCordX / lengX);
            double tempRatioY = (double)(tempCordY / lengY);
            //markerCordX = Convert.ToInt32(Math.Round(tempRatioX * 600));
            //markerCordY = Convert.ToInt32(Math.Round(tempRatioY * 600));


            temp.DrawImage(picture, startX+transX, startY+transY, lengX, lengY);
            picture.Dispose();
        }
        void displayPic(Image<Bgr, Byte> bit, int startX, int endX, int startY, int endY,int multX, int multY,int transX,int transY)
        {

            #region Resize
            int lengX = (endX - startX) * multX;
            int lengY = (endY - startY) * multY;
            #endregion

            Bitmap bitmap = bit.Bitmap;
            Size size = new Size(lengX, lengY);
            Bitmap rentbit = new Bitmap(Image.FromFile("wado1.png"), size);
            int k=0;
            int l=0;
            
            for(int x = startX;x<startX+lengX;x++)
            {
                
                for (int y = startY; y < startY + lengY; y++)
                {
                    Color clrPix = rentbit.GetPixel(k, l);
                    if(x + transX < bitmap.Width && y + transY < bitmap.Height)
                        bitmap.SetPixel(x + transX, y + transY, clrPix);
                    
                    l++;
                    
                }
                l = 0;
                k++;
                
            }
            rentbit.Dispose();
            
        }
        void Video()
        {
            try
            {
                capweb = new Capture();

            }
            catch (NullReferenceException e)
            {
                MessageBox.Show("Подключите камеру");
            }
            Application.Idle += GetVideo;//ProcessFrameAndUpdateGUI;
            bln = true;
        }


        #endregion

        
        private void Form1_Load(object sender, EventArgs e)
        {
            Bitmap Choise= new Bitmap(Image.FromFile("wado.jpg"), 600, 600);
            pictureBox1.Width = Choise.Width;
            pictureBox1.Height = Choise.Height;
            pictureBox1.Image = Choise;
                
            Video();
        }

        private void Display_Image()
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Graphics g = pictureBox1.CreateGraphics();

            g.DrawEllipse(new Pen(Brushes.Green, 7), markerCordX, markerCordY, 15, 15);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Graphics g = pictureBox1.CreateGraphics();

            g.DrawEllipse(new Pen(Brushes.Green, 7), markerCordX, markerCordY, 15, 15);
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            textBox1.Text = Convert.ToString(e.X);
            textBox2.Text = Convert.ToString(e.Y);
        }

        

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            textBox3.Text = Convert.ToString(e.X);
            textBox4.Text = Convert.ToString(e.Y);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region Convertation
                int AnchoredXMin = Convert.ToInt32(textBox1.Text);
                int AnchoredXMax = Convert.ToInt32(textBox3.Text);
                int AnchoredYMin = Convert.ToInt32(textBox2.Text);
                int AnchoredYMax = Convert.ToInt32(textBox4.Text);
                #region UI
                    Graphics UI_graphics;
                    UI_graphics = Graphics.FromImage(pictureBox1.Image);
                    Pen UI_pen = new Pen(Color.Red, 4.0F);
                    UI_graphics.DrawEllipse(UI_pen, AnchoredXMin, AnchoredYMin, 15, 15);
                    UI_graphics.DrawEllipse(UI_pen, AnchoredXMax, AnchoredYMax,15,15);
                #endregion
            #endregion
            int AnchoredFrameWidth = Math.Abs(AnchoredXMax - AnchoredXMin);
            int AnchoredFrameHeight= Math.Abs(AnchoredYMax - AnchoredYMin);
            XMin_Ratio = (double)AnchoredXMin / AnchoredFrameWidth;
            YMin_Ratio = (double)AnchoredYMin / AnchoredFrameHeight;
            WidthRatio = ((double)AnchoredFrameWidth / 600);
            HeightRatio = ((double)AnchoredFrameHeight / 600);
        }

        public Form1()
        {
            InitializeComponent();
        }
    }
}
