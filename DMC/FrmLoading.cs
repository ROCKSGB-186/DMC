using DMC.Helper;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using ThreadingTimer = System.Threading.Timer;
using UITimer = System.Windows.Forms.Timer;

namespace DMC
{
    public partial class FrmLoading : Form, ISplashForm
    {

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int RightRect, int nBottonRect, int nWidthEllipse, int nHeightEllipse);
        #region 简化方法 窗体移动,直接变化Left、Top
        private Point originLocation;

        private void WindowMove_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                #region 通过Left、Top计算直接+=变化即可
                Left += e.Location.X - originLocation.X;
                Top += e.Location.Y - originLocation.Y;
                #endregion
            }
        }

        private void WindowMove_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                originLocation = e.Location;
            }
        }
        #endregion
        public FrmLoading()
        {
            InitializeComponent();
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 25, 25));

            //初始化绘图timer
            _tmrGraphics = new UITimer { Interval = 1 };
            //Invalidate()强制重绘,绘图操作在OnPaint中实现
            _tmrGraphics.Tick += (sender, e) => PnlImage.Invalidate(false);
            _dotSize = PnlImage.Width / 10f;
            //初始化"点"
            _dots = new LoadingDotHelper[5];
            //Color = Color.Orange;
            Color = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(191)))), ((int)(((byte)(215)))));

            Message = "正在加载中，请稍后...";
        }

        private void FrmLoading_Load(object sender, EventArgs e)
        {
            Start();
        }

        void ISplashForm.SetStatusInfo(string NewStatusInfo)
        {
            if (!string.IsNullOrWhiteSpace(NewStatusInfo))
            {
                Message = NewStatusInfo;
            }
        }

        #region 属性

        [Description("消息")]
        public string Message
        {
            get { return label2.Text; }
            set { label2.Text = value; }
        }

        [Browsable(false), Description("圆心")]
        public PointF CircleCenter => new PointF(PnlImage.Width / 2f, PnlImage.Height / 2f);

        [Browsable(false), Description("半径")]
        public float CircleRadius => PnlImage.Width / 2f - _dotSize;

        [Browsable(true), Category("Appearance"), Description("设置\"点\"的前景色")]
        public Color Color { get; set; }

        #endregion 属性

        #region 字段

        [Description("工作是否完成")]
        public bool IsWorkCompleted;

        [Description("工作异常")]
        public Exception WorkException { get; private set; }

        [Description("点数组")]
        private readonly LoadingDotHelper[] _dots;

        [Description("UITimer")]
        private readonly UITimer _tmrGraphics;

        [Description("ThreadingTimer")]
        private ThreadingTimer _tmrAction;

        [Description("点大小")]
        private float _dotSize;

        [Description("是否活动")]
        private bool _isActived;

        [Description("是否绘制：用于状态重置时挂起与恢复绘图")]
        private bool _isDrawing = true;

        [Description("Timer计数：用于延迟启动每个点 ")]
        private int _timerCount;

        #endregion 字段

        #region 常量

        [Description("动作间隔(Timer)")]
        private const int ActionInterval = 30;

        [Description("计数基数：用于计算每个点启动延迟：index * timerCountRadix")]
        private const int TimerCountRadix = 45;

        #endregion 常量

        #region 方法
        /// <summary>
        /// 检查是否重置
        /// </summary>
        /// <returns></returns>
        private bool CheckToReset()
        {
            return _dots.Count(d => d.Opacity > 0) == 0;
        }

        /// <summary>
        /// 初始化点元素
        /// </summary>
        private void CreateLoadingDots()
        {
            for (var i = 0; i < _dots.Length; ++i)
                _dots[i] = new LoadingDotHelper(CircleCenter, CircleRadius);
        }

        /// <summary>
        /// 开始
        /// </summary>
        public void Start()
        {
            CreateLoadingDots();
            _timerCount = 0;
            foreach (var dot in _dots)
            {
                dot.Reset();
            }
            _tmrGraphics.Start();
            //初始化动作timer
            _tmrAction = new ThreadingTimer(
                state =>
                {
                    //动画动作
                    for (var i = 0; i < _dots.Length; i++)
                    {
                        if (_timerCount++ > i * TimerCountRadix)
                        {
                            _dots[i].LoadingDotAction();
                        }
                    }
                    //是否重置
                    if (CheckToReset())
                    {
                        //重置前暂停绘图
                        _isDrawing = false;
                        _timerCount = 0;
                        foreach (var dot in _dots)
                        {
                            dot.Reset();
                        }
                        //恢复绘图
                        _isDrawing = true;
                    }
                    _tmrAction.Change(ActionInterval, Timeout.Infinite);
                },
                null, ActionInterval, Timeout.Infinite);
            _isActived = true;
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            _tmrGraphics.Stop();
            _tmrAction.Dispose();
            _isActived = false;
        }

        #endregion 方法        

        #region 重写

        protected override void OnPaint(PaintEventArgs e)
        {
            if (IsWorkCompleted)
            {
                Stop();
                Close();
            }
        }

        private void PnlImage_Paint(object sender, PaintEventArgs e)
        {
            if (_isActived && _isDrawing)
            {
                //抗锯齿
                e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                using (var bitmap = new Bitmap(200, 200))
                {
                    //缓冲绘制
                    using (var bufferGraphics = Graphics.FromImage(bitmap))
                    {
                        //抗锯齿
                        bufferGraphics.SmoothingMode = SmoothingMode.HighQuality;
                        foreach (var dot in _dots)
                        {
                            var rectangleF = new RectangleF(
                                new PointF(dot.Location.X - _dotSize / 2, dot.Location.Y - _dotSize / 2),
                                new SizeF(_dotSize, _dotSize));
                            bufferGraphics.FillEllipse(new SolidBrush(Color.FromArgb(dot.Opacity, Color)),
                                rectangleF);
                        }
                    }
                    //贴图
                    e.Graphics.DrawImage(bitmap, new PointF(0, 0));
                } //bmp disposed
            }
            base.OnPaint(e);
        }

        private void PnlImage_Resize(object sender, EventArgs e)
        {
            PnlImage.Height = PnlImage.Width;
            _dotSize = PnlImage.Width / 12f;
            OnResize(e);
        }

        #endregion 重写        

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;

            }
        }

    }
}
