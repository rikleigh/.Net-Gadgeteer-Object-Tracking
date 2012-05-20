using System;
using Gadgeteer.Modules.GHIElectronics;
using GT = Gadgeteer;

namespace GadgeteerObjectTracking
{
    public class PanAndTiltController
    {
        private const int PanCenter = 1500;
        private const int TiltCenter = 1500;
        private const int PanMax = PanCenter + 700;
        private const int PanMin = PanCenter - 700;
        private const int TiltMax = TiltCenter + 700;
        private const int TiltMin = TiltCenter - 200;

        private uint _pwmPulsePeriod;

        public GT.Interfaces.PWMOutput PanPwm { get; set; }

        public GT.Interfaces.PWMOutput TiltPwm { get; set; }

        public int Pan { get; set; }

        public int Tilt { get; set; }

        public PanAndTiltController(Extender extender, uint pwmPulsePeriod)
        {
            _pwmPulsePeriod = pwmPulsePeriod;
            PanPwm = extender.SetupPWMOutput(GT.Socket.Pin.Seven);
            TiltPwm = extender.SetupPWMOutput(GT.Socket.Pin.Eight);

            if (PanPwm == null || TiltPwm == null)
            {
                throw new ApplicationException("one or more pins are null");
            }

            Center(false);
        }

        public void SetPan()
        {
            if (Pan < PanMin) Pan = PanMin;
            if (Pan > PanMax) Pan = PanMax;

            Servo(PanPwm, Pan);
        }

        public void SetTilt()
        {
            if (Tilt < TiltMin) Tilt = TiltMin;
            if (Tilt > TiltMax) Tilt = TiltMax;

            Servo(TiltPwm, Tilt);
        }

        public void Center(bool step)
        {
            if (step)
            {
                StepTowardsCenter();
            }
            else
            {
                Servo(PanPwm, PanCenter);
                Servo(TiltPwm, TiltCenter);
            }
        }

        private void StepTowardsCenter()
        {
            if (Pan > PanCenter) Pan -= 1;
            if (Pan < PanCenter) Pan += 1;
            if (Tilt > TiltCenter) Tilt -= 1;
            if (Tilt < TiltCenter) Tilt += 1;

            SetPan();
            SetTilt();
        }

        private void Servo(GT.Interfaces.PWMOutput pwm, int pwmPulseHighTime)
        {
            pwm.SetPulse(_pwmPulsePeriod, (uint)pwmPulseHighTime);
        }
    }
}
