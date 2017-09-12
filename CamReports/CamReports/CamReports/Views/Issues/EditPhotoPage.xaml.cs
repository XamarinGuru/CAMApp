using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CamReports.Effects.Touch;
using CamReports.Models;
using CamReports.Services;
using CamReports.ViewModel.Issues;
using PCLStorage;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XLabs;

namespace CamReports.Views.Issues
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditPhotoPage
    {
        public EditPhotoPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);

            CanvasView.PaintSurface += OnCanvasViewPaintSurface;
        }

        private SKBitmap _Bitmap;
        private EditPhotoViewModel _ViewModel;

        protected override async void OnAppearing()
        {
            _ViewModel = BindingContext as EditPhotoViewModel;

            var fileService = DependencyService.Get<ISaveAndLoad>();
            var data = fileService.LoadFile(_ViewModel.Issue.ImagePath);
            var stream = new MemoryStream(data);
            using (var managedStream = new SKManagedStream(stream))
            {
                _Bitmap = SKBitmap.Decode(managedStream);
            }

            CanvasView.InvalidateSurface();

            base.OnAppearing();
        }

        private Rectangle _ImageRect = new Rectangle();
        private readonly int _angle = 50;
        private readonly float _l2 = 40;
        private const int CircleRadius = 40;

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            if (_Bitmap != null)
            {
                if (_Bitmap.Width > info.Width)
                {
                    var koeff = (double)info.Width / (double)_Bitmap.Width;
                    _Bitmap = _Bitmap.Resize(new SKImageInfo((int)(_Bitmap.Width * koeff), (int)(_Bitmap.Height * koeff)),
                        SKBitmapResizeMethod.Triangle);
                }
                if (_Bitmap.Height > info.Height)
                {
                    var koeff = (double)info.Height / (double)_Bitmap.Height;
                    _Bitmap = _Bitmap.Resize(new SKImageInfo((int)(_Bitmap.Width * koeff), (int)(_Bitmap.Height * koeff)),
                        SKBitmapResizeMethod.Triangle);
                }

                float x = (info.Width - _Bitmap.Width) / 2;
                float y = (info.Height - _Bitmap.Height) / 2;
                _ImageRect.X = x;
                _ImageRect.Y = y;
                _ImageRect.Width = _Bitmap.Width;
                _ImageRect.Height = _Bitmap.Height;
                //canvas.ClipRect(new SKRect(x, y, x + _Bitmap.Width, y + _Bitmap.Height));

                canvas.DrawBitmap(_Bitmap, x, y);
            }

            foreach (var path in _CompletedPaths)
            {
                paint.Color = new SKColor((byte)(path.Item2.Color.R*255),
                    (byte)(path.Item2.Color.G*255), (byte)(path.Item2.Color.B*255));
                canvas.DrawPath(path.Item1, paint);
            }

            foreach (var path in _InProgressPaths.Values)
            {
                paint.Color = new SKColor((byte)(path.Item2.Color.R * 255),
                    (byte)(path.Item2.Color.G * 255), (byte)(path.Item2.Color.B * 255));
                canvas.DrawPath(path.Item1, paint);
            }

            if (_IsCircleInProgress)
            {
                paint.Color = new SKColor((byte)(_ViewModel.SelectedColor.Color.R * 255),
                    (byte)(_ViewModel.SelectedColor.Color.G * 255), (byte)(_ViewModel.SelectedColor.Color.B * 255));
                canvas.DrawCircle(_CirclePoint.X, _CirclePoint.Y, CircleRadius, paint);
            }

            foreach (var circle in _Circles)
            {
                paint.Color = new SKColor((byte)(circle.Item2.Color.R * 255),
                    (byte)(circle.Item2.Color.G * 255), (byte)(circle.Item2.Color.B * 255));
                canvas.DrawCircle(circle.Item1.X, circle.Item1.Y, CircleRadius, paint);
            }

            if (_IsCircleInProgress)
            {
                paint.Color = new SKColor((byte)(_ViewModel.SelectedColor.Color.R * 255),
                    (byte)(_ViewModel.SelectedColor.Color.G * 255), (byte)(_ViewModel.SelectedColor.Color.B * 255));
                canvas.DrawCircle(_CirclePoint.X, _CirclePoint.Y, 40, paint);
            }

            foreach (var arrow in _Arrows)
            {
                DrawArrowOnCanvas(arrow, canvas);
            }
            if (_IsArrowInProgress)
            {
                DrawArrowOnCanvas(new Tuple<SKPoint, SKPoint, ColorItemViewModel>(_ArrowPoint1, _ArrowPoint2, _ViewModel.SelectedColor), canvas);
            }

            ResultImage = surface.Snapshot();
            if (_Bitmap != null)
            {
                float x = (info.Width - _Bitmap.Width) / 2;
                float y = (info.Height - _Bitmap.Height) / 2;
                ResultImage = ResultImage.Subset(new SKRectI((int)x, (int)y, (int)x + _Bitmap.Width, (int)y + _Bitmap.Height));
            }
        }

        public SKImage ResultImage { get; set; }

        private void DrawArrowOnCanvas(Tuple<SKPoint, SKPoint, ColorItemViewModel> arrow, SKCanvas canvas)
        {
            paint.Color = new SKColor((byte)(arrow.Item3.Color.R * 255),
                (byte)(arrow.Item3.Color.G * 255), (byte)(arrow.Item3.Color.B * 255));

            var x1 = arrow.Item1.X; var y1 = arrow.Item1.Y;
            var x2 = arrow.Item2.X; var y2 = arrow.Item2.Y;

            if (x1 - x2 <= float.Epsilon && y1 - y2 <= float.Epsilon)
                return;

            float l1 = (float)Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));

            float x3 = x2 + _l2 / l1 * ((x1 - x2) * (float)Math.Cos(_angle) + (y1 - y2) * (float)Math.Sin(_angle));
            float y3 = y2 + _l2 / l1 * ((y1 - y2) * (float)Math.Cos(_angle) - (x1 - x2) * (float)Math.Sin(_angle));
            float x4 = x2 + _l2 / l1 * ((x1 - x2) * (float)Math.Cos(_angle) - (y1 - y2) * (float)Math.Sin(_angle));
            float y4 = y2 + _l2 / l1 * ((y1 - y2) * (float)Math.Cos(_angle) + (x1 - x2) * (float)Math.Sin(_angle));

            canvas.DrawLine(x1, y1, x2, y2, paint);
            canvas.DrawLine(x2, y2, x3, y3, paint);
            canvas.DrawLine(x2, y2, x4, y4, paint);
        }

        private readonly Dictionary<long, Tuple<SKPath, ColorItemViewModel>> _InProgressPaths = 
            new Dictionary<long, Tuple<SKPath, ColorItemViewModel>>();
        private readonly List<Tuple<SKPath, ColorItemViewModel>> _CompletedPaths = 
            new List<Tuple<SKPath, ColorItemViewModel>>();
        private readonly List<Tuple<SKPoint, ColorItemViewModel>> _Circles =
            new List<Tuple<SKPoint, ColorItemViewModel>>();
        private readonly List<Tuple<SKPoint, SKPoint, ColorItemViewModel>> _Arrows =
            new List<Tuple<SKPoint, SKPoint, ColorItemViewModel>>();

        SKPaint paint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Blue,
            StrokeWidth = 10,
            StrokeCap = SKStrokeCap.Round,
            StrokeJoin = SKStrokeJoin.Round
        };

        private void TouchEffect_OnTouchAction(object sender, TouchActionEventArgs args)
        {
            if(_ViewModel.SelectedTab.Id == 0)
                DrawArrow(args);
            if (_ViewModel.SelectedTab.Id == 1)
                DrawCircle(args);
            if (_ViewModel.SelectedTab.Id == 2)
                DrawPath(args);
            if (_ViewModel.SelectedTab.Id == 3 && args.Type == TouchActionType.Released)
                Erase(args);
        }

        private SKPoint _CirclePoint = new SKPoint();
        private bool _IsCircleInProgress;

        private void DrawCircle(TouchActionEventArgs args)
        {
            var point = ConvertToPixel(args.Location);
            if (point.X < _ImageRect.X || point.X > (_ImageRect.Width + _ImageRect.X)
                || point.Y < _ImageRect.Y || point.Y > (_ImageRect.Height + _ImageRect.Y))
            {
                if (args.Type == TouchActionType.Released)
                {
                    _Circles.Add(new Tuple<SKPoint, ColorItemViewModel>(_CirclePoint, _ViewModel.SelectedColor));
                    _IsCircleInProgress = false;
                    CanvasView.InvalidateSurface();
                }

                return;
            }

            switch (args.Type)
            {
                case TouchActionType.Pressed:
                    if (!_IsCircleInProgress)
                    {
                        _CirclePoint = point;
                        _IsCircleInProgress = true;
                        CanvasView.InvalidateSurface();
                    }
                    break;

                case TouchActionType.Moved:
                    if (_IsCircleInProgress)
                    {
                        _CirclePoint = point;
                        CanvasView.InvalidateSurface();
                    }
                    break;

                case TouchActionType.Released:
                    if (_IsCircleInProgress)
                    {
                        _Circles.Add(new Tuple<SKPoint, ColorItemViewModel>(_CirclePoint, _ViewModel.SelectedColor));
                        _IsCircleInProgress = false;
                        CanvasView.InvalidateSurface();
                    }
                    break;

                case TouchActionType.Cancelled:
                    if (_IsCircleInProgress)
                    {
                        _IsCircleInProgress = false;
                        CanvasView.InvalidateSurface();
                    }
                    break;
            }
        }

        private SKPoint _ArrowPoint1 = new SKPoint();
        private SKPoint _ArrowPoint2 = new SKPoint();
        private bool _IsArrowInProgress;

        private void DrawArrow(TouchActionEventArgs args)
        {
            var point = ConvertToPixel(args.Location);
            if (point.X < _ImageRect.X || point.X > (_ImageRect.Width + _ImageRect.X)
                || point.Y < _ImageRect.Y || point.Y > (_ImageRect.Height + _ImageRect.Y))
            {
                if (args.Type == TouchActionType.Released)
                {
                    _Arrows.Add(new Tuple<SKPoint, SKPoint, ColorItemViewModel>(_ArrowPoint1, _ArrowPoint2, _ViewModel.SelectedColor));
                    _IsArrowInProgress = false;
                    CanvasView.InvalidateSurface();
                }

                return;
            }

            switch (args.Type)
            {
                case TouchActionType.Pressed:
                    if (!_IsArrowInProgress)
                    {
                        _ArrowPoint1 = point;
                        _IsArrowInProgress = true;
                        CanvasView.InvalidateSurface();
                    }
                    break;

                case TouchActionType.Moved:
                    if (_IsArrowInProgress)
                    {
                        _ArrowPoint2 = point;
                        CanvasView.InvalidateSurface();
                    }
                    break;

                case TouchActionType.Released:
                    if (_IsArrowInProgress)
                    {
                        _Arrows.Add(new Tuple<SKPoint, SKPoint, ColorItemViewModel>(_ArrowPoint1, _ArrowPoint2, _ViewModel.SelectedColor));
                        _IsArrowInProgress = false;
                        CanvasView.InvalidateSurface();
                    }
                    break;

                case TouchActionType.Cancelled:
                    if (_IsArrowInProgress)
                    {
                        _IsArrowInProgress = false;
                        CanvasView.InvalidateSurface();
                    }
                    break;
            }
        }

        private void DrawPath(TouchActionEventArgs args)
        {
            var point = ConvertToPixel(args.Location);
            if (point.X < _ImageRect.X || point.X > (_ImageRect.Width + _ImageRect.X)
                || point.Y < _ImageRect.Y || point.Y > (_ImageRect.Height + _ImageRect.Y))
            {
                if (args.Type == TouchActionType.Released)
                {
                    _InProgressPaths.Remove(args.Id);
                    CanvasView.InvalidateSurface();
                }

                return;
            }

            switch (args.Type)
            {
                case TouchActionType.Pressed:
                    if (!_InProgressPaths.ContainsKey(args.Id))
                    {
                        SKPath path = new SKPath();
                        path.MoveTo(ConvertToPixel(args.Location));
                        _InProgressPaths.Add(args.Id, new Tuple<SKPath, ColorItemViewModel>(path, _ViewModel.SelectedColor));
                        CanvasView.InvalidateSurface();
                    }
                    break;

                case TouchActionType.Moved:
                    if (_InProgressPaths.ContainsKey(args.Id))
                    {
                        SKPath path = _InProgressPaths[args.Id].Item1;
                        path.LineTo(ConvertToPixel(args.Location));
                        CanvasView.InvalidateSurface();
                    }
                    break;

                case TouchActionType.Released:
                    if (_InProgressPaths.ContainsKey(args.Id))
                    {
                        _CompletedPaths.Add(_InProgressPaths[args.Id]);
                        _InProgressPaths.Remove(args.Id);
                        CanvasView.InvalidateSurface();
                    }
                    break;

                case TouchActionType.Cancelled:
                    if (_InProgressPaths.ContainsKey(args.Id))
                    {
                        _InProgressPaths.Remove(args.Id);
                        CanvasView.InvalidateSurface();
                    }
                    break;
            }
        }

        private void Erase(TouchActionEventArgs args)
        {
            var distances = _Arrows.Select(
                item => new Tuple<object, double>(item, DistanceToArrow(item.Item1, item.Item2, ConvertToPixel(args.Location)))).ToList();

            var distancesToCircles = _Circles.Select(item => new Tuple<object, double>(item,
                DistanceToCircle(item.Item1, CircleRadius, ConvertToPixel(args.Location))));
            distances.AddRange(distancesToCircles);

            var distancesToPaths = _CompletedPaths.Select(item => new Tuple<object, double>(item,
                DistanceToPath(item.Item1, ConvertToPixel(args.Location))));
            distances.AddRange(distancesToPaths);

            if (!distances.Any())
                return;

            var shape = distances.Aggregate((i1, i2) => i1.Item2 < i2.Item2 ? i1 : i2);

            if (shape.Item1 is Tuple<SKPoint, ColorItemViewModel>)
                _Circles.Remove((Tuple<SKPoint, ColorItemViewModel>) shape.Item1);
            if (shape.Item1 is Tuple<SKPoint, SKPoint, ColorItemViewModel>)
                _Arrows.Remove((Tuple<SKPoint, SKPoint, ColorItemViewModel>)shape.Item1);
            if (shape.Item1 is Tuple<SKPath, ColorItemViewModel>)
                _CompletedPaths.Remove((Tuple<SKPath, ColorItemViewModel>)shape.Item1);

            CanvasView.InvalidateSurface();
        }

        SKPoint ConvertToPixel(Point pt)
        {
            return new SKPoint((float)(CanvasView.CanvasSize.Width * pt.X / CanvasView.Width),
                (float)(CanvasView.CanvasSize.Height * pt.Y / CanvasView.Height));
        }

        #region Distance

        private double DistanceToArrow(SKPoint arrowPoint1, SKPoint arrowPoint2, SKPoint point)
        {
            var numerator = Math.Abs((arrowPoint2.Y - arrowPoint1.Y) * point.X
                     - (arrowPoint2.X - arrowPoint1.X) * point.Y
                     + arrowPoint2.X * arrowPoint1.Y
                     - arrowPoint2.Y * arrowPoint1.X);

            var denominator = Math.Sqrt((arrowPoint2.Y - arrowPoint1.Y) * (arrowPoint2.Y - arrowPoint1.Y)
                                        + (arrowPoint2.X - arrowPoint1.X) * (arrowPoint2.X - arrowPoint1.X));

            return numerator / denominator;
        }

        private double DistanceToCircle(SKPoint circleCenter, double radius, SKPoint point)
        {
            var distanceToCenter = Math.Sqrt((circleCenter.Y - point.Y) * (circleCenter.Y - point.Y)
                                             + (circleCenter.X - point.X) * (circleCenter.X - point.X));

            var distance = Math.Abs(distanceToCenter - radius);

            return distance;
        }

        private double DistanceToPath(SKPath path, SKPoint touchPoint)
        {
            var points = path.GetPoints(1000);
            if (points == null || points.Length == 0)
                return -1;

            var point1 = points[0];
            double distance = 10000;
            for(int i = 1; i < points.Length; i++)
            {
                var newDistance = DistanceToArrow(point1, points[i], touchPoint);

                if (distance > newDistance)
                    distance = newDistance;

                point1 = points[i];
            }

            return distance;
        }

        #endregion
    }
}