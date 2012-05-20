using System;
using System.Threading;

namespace GadgeteerObjectTracking
{
    public class ObjectTracker
    {
        private IrCompoundEye _irEye;
        private PanAndTiltController _panAndTiltController;
        private Thread _trackingThread;

        public int DistanceMax { get; set; }

        public int BestDistance { get; set; }

        public byte LRScaleFactor { get; set; }

        public byte UDScaleFactor { get; set; }

        public bool IsTracking { get; private set; }

        public ObjectTracker(IrCompoundEye irEye, PanAndTiltController panAndTiltController)
        {
            //set default values
            DistanceMax = 220;
            BestDistance = 550;
            LRScaleFactor = 4;
            UDScaleFactor = 4;

            _irEye = irEye;
            _panAndTiltController = panAndTiltController;          
        }

        public void StartTracking()
        {
            _trackingThread = new Thread(new ThreadStart(TrackingThread));
            _trackingThread.Start();
            IsTracking = true;
        }

        public void StopTracking()
        {
            if (_trackingThread != null)
            {
                _trackingThread.Abort();
            }

            IsTracking = false;
        }

        private void TrackingThread()
        {
            while (true)
            {
                var irEyeData = _irEye.Read();

                if (irEyeData.CurrentDistance < DistanceMax)
                {
                    _panAndTiltController.Center(true);
                }
                else
                {
                    var panScale = (irEyeData.LeftIrValue + irEyeData.RightIrValue) / LRScaleFactor;
                    var tiltScale = (irEyeData.UpIrValue + irEyeData.DownIrValue) / UDScaleFactor;

                    _panAndTiltController.Pan += (int)GetPanTiltAdjustment(irEyeData.RightIrValue, irEyeData.LeftIrValue, panScale);
                    _panAndTiltController.Tilt += (int)GetPanTiltAdjustment(irEyeData.DownIrValue, irEyeData.UpIrValue, panScale);

                    _panAndTiltController.SetPan();
                    _panAndTiltController.SetTilt();
                }
            }
        }

        private double GetPanTiltAdjustment(double irValue1, double irValue2, double scale)
        {
            return (irValue1 - irValue2) * 5 / scale;
        }
    }
}
