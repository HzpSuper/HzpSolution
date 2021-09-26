using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HzpSolution.Views
{
    /// <summary>
    /// LoginView.xaml 的交互逻辑
    /// </summary>
    public partial class LoginView : Window
    {
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SnackBar.MessageQueue?.Enqueue(
                $"密码错误",
                null,
                null,
                null,
                false,
                true,
                TimeSpan.FromSeconds(2));
        }

        public LoginView()
        {
            InitializeComponent();
        }

        private void Login_Loaded(object sender, RoutedEventArgs e)
        {
            CompositionTarget.Rendering += CompositionTarget_Rendering;
            InitStar();
        }

        private void Login_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }


        private void MinWindow_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            tm.Tick += new EventHandler(tm_Tick);
            tm.Interval = TimeSpan.FromSeconds(1);
            tm.Start();
        }

        System.Windows.Threading.DispatcherTimer tm = new System.Windows.Threading.DispatcherTimer();
        private void tm_Tick(object? sender, EventArgs e)
        {
            this.Close();
        }

        #region 星星动画

        #region 私有成员变量
        /// <summary>
        /// 星星PathData
        /// </summary>
        private readonly Geometry _pathDataStar = PathGeometry.Parse("M16.001007,0L20.944,10.533997 32,12.223022 23.998993,20.421997 25.889008,32 16.001007,26.533997 6.1109924,32 8,20.421997 0,12.223022 11.057007,10.533997z");

        /// <summary>
        /// 星星个数
        /// </summary>
        private readonly int _starCount = 12;

        /// <summary>
        /// 星星最小尺寸
        /// </summary>
        private readonly int _starSizeMin = 5;

        /// <summary>
        /// 星星最大尺寸
        /// </summary>
        private readonly int _starSizeMax = 20;

        /// <summary>
        /// 星星运动的最大速度
        /// </summary>
        private readonly int _starVMax = 20;

        /// <summary>
        /// 星星转动的最小速度
        /// </summary>
        private readonly int _starRVMin = 90;

        /// <summary>
        /// 星星转动的最大速度
        /// </summary>
        private readonly int _starRVMax = 360;

        /// <summary>
        /// 倍率 (乘以星星长度)
        /// </summary>
        private readonly int _lineRate = 3;

        /// <summary>
        /// 随机数
        /// </summary>
        private readonly Random _random = new();

        /// <summary>
        /// 星星数组
        /// </summary>
        private List<StarInfo> _stars = new();
        #endregion


        /// <summary>
        /// 帧渲染事件
        /// </summary>
        private void CompositionTarget_Rendering(object? sender, EventArgs e)
        {
            StarRoamAnimation();
            AddOrRemoveStarLine();
            MoveStarLine();
        }


        /// <summary>
        /// 初始化星星
        /// </summary>
        private void InitStar()
        {
            //清空星星容器
            _stars = new (_starCount);
            cvs_starContainer.Children.Clear();
            grid_lineContainer.Children.Clear();
            //生成星星
            for (int i = 0; i < _starCount; i++)
            {
                double size = _random.Next(_starSizeMin, _starSizeMax + 1);//星星尺寸
                StarInfo starInfo = new()
                {
                    X = _random.Next(0, (int)cvs_starContainer.ActualWidth),
                    XV = (double)_random.Next(-_starVMax, _starVMax) / 60,
                    XT = _random.Next(6, 301),//帧
                    Y = _random.Next(0, (int)cvs_starContainer.ActualHeight),
                    YV = (double)_random.Next(-_starVMax, _starVMax) / 60,
                    YT = _random.Next(6, 301),//帧
                    StarLines = new Dictionary<StarInfo, Line>()
                };
                Path star = new()
                {
                    Data = _pathDataStar,
                    Width = size,
                    Height = size,
                    Stretch = Stretch.Fill,
                    Fill = GetRandomColorBursh(),
                    RenderTransformOrigin = new Point(0.5, 0.5),
                    RenderTransform = new RotateTransform() { Angle = 0 }
                };
                Canvas.SetLeft(star, starInfo.X);
                Canvas.SetTop(star, starInfo.Y);
                starInfo.StarRef = star;
                //设置星星旋转动画
                SetStarRotateAnimation(star);
                //添加到容器
                _stars.Add(starInfo);
                cvs_starContainer.Children.Add(star);
            }
        }

        /// <summary>
        /// 获取随机颜色画刷(偏亮)
        /// </summary>
        /// <returns>SolidColorBrush</returns>
        private SolidColorBrush GetRandomColorBursh()
        {
            byte r = (byte)_random.Next(128, 256);
            byte g = (byte)_random.Next(128, 256);
            byte b = (byte)_random.Next(128, 256);
            return new SolidColorBrush(Color.FromRgb(r, g, b));
        }

        /// <summary>
        /// 设置星星旋转动画
        /// </summary>
        /// <param name="star"></param>
        private void SetStarRotateAnimation(Path star)
        {
            double v = _random.Next(_starRVMin, _starRVMax + 1);//速度
            double a = _random.Next(0, 360 * 5);//角度
            double t = a / v;//时间
            Duration dur = new(new TimeSpan(0, 0, 0, 0, (int)(t * 1000)));

            Storyboard sb = new()
            {
                Duration = dur
            };
            //动画完成事件 再次设置此动画
            sb.Completed += (S, E) =>
            {
                SetStarRotateAnimation(star);
            };

            DoubleAnimation da = new ()
            {
                To = a,
                Duration = dur
            };
            Storyboard.SetTarget(da, star);
            Storyboard.SetTargetProperty(da, new PropertyPath("(UIElement.RenderTransform).(RotateTransform.Angle)"));
            sb.Children.Add(da);
            sb.Begin(this);
        }

        /// <summary>
        /// 星星漫游动画
        /// </summary>
        private void StarRoamAnimation()
        {
            if (_stars == null)
            {
                return;
            }

            foreach (StarInfo starInfo in _stars)
            {
                //X轴运动
                if (starInfo.XT > 0)
                {
                    //运动时间大于0,继续运动
                    if (starInfo.X >= cvs_starContainer.ActualWidth || starInfo.X <= -_starSizeMax)
                    {
                        //碰到边缘,速度取反向
                        starInfo.XV = -starInfo.XV;
                    }
                    //位移加,时间减
                    starInfo.X += starInfo.XV;
                    starInfo.XT--;
                    Canvas.SetLeft(starInfo.StarRef, starInfo.X);
                }
                else
                {
                    //运动时间小于0,重新设置速度和时间
                    starInfo.XV = (double)_random.Next(-_starVMax, _starVMax) / 60;
                    starInfo.XT = _random.Next(100, 1001);
                }
                //Y轴运动
                if (starInfo.YT > 0)
                {
                    //运动时间大于0,继续运动
                    if (starInfo.Y >= cvs_starContainer.ActualHeight || starInfo.Y <= -_starSizeMax)
                    {
                        //碰到边缘,速度取反向
                        starInfo.YV = -starInfo.YV;
                    }
                    //位移加,时间减
                    starInfo.Y += starInfo.YV;
                    starInfo.YT--;
                    Canvas.SetTop(starInfo.StarRef, starInfo.Y);
                }
                else
                {
                    //运动时间小于0,重新设置速度和时间
                    starInfo.YV = (double)_random.Next(-_starVMax, _starVMax) / 60;
                    starInfo.YT = _random.Next(100, 1001);
                }
            }
        }

        /// <summary>
        /// 添加或者移除星星之间的连线
        /// </summary>
        private void AddOrRemoveStarLine()
        {
            //没有星星 直接返回
            if (_stars == null)
            {
                return;
            }

            //生成星星间的连线
            for (int i = 0; i < _starCount - 1; i++)
            {
                for (int j = i + 1; j < _starCount; j++)
                {
                    StarInfo star1 = _stars[i];
                    double x1 = star1.X + star1.StarRef.Width / 2;
                    double y1 = star1.Y + star1.StarRef.Height / 2;
                    StarInfo star2 = _stars[j];
                    double x2 = star2.X + star2.StarRef.Width / 2;
                    double y2 = star2.Y + star2.StarRef.Height / 2;
                    double s = Math.Sqrt(((y2 - y1) * (y2 - y1)) + (x2 - x1) * (x2 - x1));//两个星星间的距离
                    double threshold = star1.StarRef.Width * _lineRate + star2.StarRef.Width * _lineRate;
                    if (s <= threshold)
                    {
                        if (!star1.StarLines.ContainsKey(star2))
                        {
                            Line line = new()
                            {
                                X1 = x1,
                                Y1 = y1,
                                X2 = x2,
                                Y2 = y2,
                                Stroke = GetStarLineBrush(star1.StarRef, star2.StarRef)
                            };
                            star1.StarLines.Add(star2, line);
                            _ = grid_lineContainer.Children.Add(line);
                        }
                    }
                    else
                    {
                        if (star1.StarLines.ContainsKey(star2))
                        {
                            grid_lineContainer.Children.Remove(star1.StarLines[star2]);
                            _ = star1.StarLines.Remove(star2);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 移动星星之间的连线
        /// </summary>
        private void MoveStarLine()
        {
            //没有星星 直接返回
            if (_stars == null)
            {
                return;
            }

            foreach (StarInfo? star in _stars)
            {
                foreach (KeyValuePair<StarInfo, Line> starLine in star.StarLines)
                {
                    Line line = starLine.Value;
                    line.X1 = star.X + star.StarRef.Width / 2;
                    line.Y1 = star.Y + star.StarRef.Height / 2;
                    line.X2 = starLine.Key.X + starLine.Key.StarRef.Width / 2;
                    line.Y2 = starLine.Key.Y + starLine.Key.StarRef.Height / 2;
                }
            }
        }

        /// <summary>
        /// 获取星星连线颜色画刷
        /// </summary>
        /// <param name="star0">起始星星</param>
        /// <param name="star1">终点星星</param>
        /// <returns>LinearGradientBrush</returns>
        private static LinearGradientBrush GetStarLineBrush(Path star0, Path star1)
        {
            return new LinearGradientBrush()
            {
                GradientStops = new GradientStopCollection()
                {
                    new GradientStop() { Offset=0,Color=((SolidColorBrush)star0.Fill).Color},
                    new GradientStop() { Offset=1,Color=((SolidColorBrush)star1.Fill).Color}
                }
            };
        }

        #endregion


    }

    /// <summary>
    /// 星星
    /// </summary>
    public class StarInfo
    {
        /// <summary>
        /// X坐标
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// X轴速度(单位距离/帧)
        /// </summary>
        public double XV { get; set; }

        /// <summary>
        /// X坐标以X轴速度运行的时间(帧)
        /// </summary>
        public int XT { get; set; }

        /// <summary>
        /// Y坐标
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Y轴速度(单位距离/帧)
        /// </summary>
        public double YV { get; set; }

        /// <summary>
        /// Y坐标以Y轴速度运行的时间(帧)
        /// </summary>
        public int YT { get; set; }

        /// <summary>
        /// 对星星的引用
        /// </summary>
        public Path StarRef { get; set; } = new();

        public Dictionary<StarInfo, Line> StarLines { get; set; } = new();
    }

}
